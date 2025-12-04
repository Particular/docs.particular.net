#!/bin/bash
# Script to add all samples referencing NServiceBus v10.0.0-alpha.X to a GitHub project board
#
# Usage:
#   ./add-alpha-samples-to-project.sh <PROJECT_NUMBER> [OWNER] [REPO] [--area AREA] [--prio PRIO]
#
# Arguments:
#   PROJECT_NUMBER - The GitHub project number (required)
#   OWNER          - Repository owner (default: Particular)
#   REPO           - Repository name (default: docs.particular.net)
#   --area AREA    - Value to set for the "Area" custom field (optional, single-select)
#   --prio PRIO    - Value to set for the "Prio" custom field (optional, number)
#
# Prerequisites:
#   - GitHub CLI (gh) must be installed and authenticated
#   - User must have write access to the specified project
#   - Bash version 4.0 or higher for associative arrays
#
# Example:
#   ./add-alpha-samples-to-project.sh 42
#   ./add-alpha-samples-to-project.sh 42 Particular docs.particular.net
#   ./add-alpha-samples-to-project.sh 42 --area "Core" --prio 1
#   ./add-alpha-samples-to-project.sh 42 Particular docs.particular.net --area "Core"

set -e

# Enable debug mode if DEBUG env var is set
if [ -n "$DEBUG" ]; then
    set -x
fi

# Check bash version (need 4.0+ for associative arrays)
BASH_MAJOR_VERSION="${BASH_VERSINFO[0]}"
if [ "$BASH_MAJOR_VERSION" -lt 4 ]; then
    echo "Error: This script requires Bash version 4.0 or higher"
    echo "Current version: $BASH_VERSION"
    echo "Please run with: bash $0 $*"
    exit 1
fi

echo "==> Checking prerequisites..."

echo "==> Checking prerequisites..."

# Check if gh CLI is available
if ! command -v gh &> /dev/null; then
    echo "Error: GitHub CLI (gh) is not installed or not in PATH"
    echo "Please install from: https://cli.github.com/"
    exit 1
fi
echo "  ✓ GitHub CLI found"

# Check if jq is available (needed for parsing JSON from gh CLI)
if ! command -v jq &> /dev/null; then
    echo "Error: jq is not installed or not in PATH"
    echo "jq is required for parsing JSON responses from GitHub CLI"
    echo "Please install from: https://jqlang.github.io/jq/"
    exit 1
fi
echo "  ✓ jq found"
echo ""

# Check if required argument is provided
if [ -z "$1" ]; then
    echo "Error: PROJECT_NUMBER is required"
    echo ""
    echo "Usage: $0 <PROJECT_NUMBER> [OWNER] [REPO] [--area AREA] [--prio PRIO]"
    echo ""
    echo "Example: $0 42"
    echo "Example: $0 42 --area \"Core\" --prio \"High\""
    exit 1
fi

# Parse arguments
PROJECT_NUMBER=$1
shift

OWNER="Particular"
REPO="docs.particular.net"
AREA_VALUE=""
PRIO_VALUE=""

# Parse remaining arguments
while [[ $# -gt 0 ]]; do
    case $1 in
        --area)
            AREA_VALUE="$2"
            shift 2
            ;;
        --prio)
            PRIO_VALUE="$2"
            shift 2
            ;;
        *)
            # First non-flag argument is OWNER, second is REPO
            if [ "$OWNER" = "Particular" ]; then
                OWNER="$1"
            elif [ "$REPO" = "docs.particular.net" ]; then
                REPO="$1"
            fi
            shift
            ;;
    esac
done

echo "Configuration:"
echo "  Project Number: $PROJECT_NUMBER"
echo "  Owner: $OWNER"
echo "  Repo: $REPO"
if [ -n "$AREA_VALUE" ]; then
    echo "  Area: $AREA_VALUE"
fi
if [ -n "$PRIO_VALUE" ]; then
    echo "  Prio: $PRIO_VALUE"
fi
echo ""

# Helper function to add an option to a single-select field
add_field_option() {
    local FIELD_NAME=$1
    local FIELD_ID=$2
    local OPTION_VALUE=$3
    local PROJECT_ID=$4
    
    echo "Adding '$OPTION_VALUE' option to '$FIELD_NAME' field..."
    
    MUTATION=$(cat <<EOF
mutation {
  addProjectV2SingleSelectFieldOption(input: {
    projectId: "$PROJECT_ID"
    fieldId: "$FIELD_ID"
    name: "$OPTION_VALUE"
  }) {
    option {
      id
      name
    }
  }
}
EOF
)
    
    ADD_OPTION_RESULT=$(gh api graphql -f query="$MUTATION" 2>/dev/null)
    if [ $? -eq 0 ]; then
        echo "  ✓ Added '$OPTION_VALUE' option to '$FIELD_NAME' field"
        # Extract and return the new option ID
        echo "$ADD_OPTION_RESULT" | jq -r '.data.addProjectV2SingleSelectFieldOption.option.id'
        return 0
    else
        echo "  ✗ Failed to add option"
        # Show the error
        gh api graphql -f query="$MUTATION"
        return 1
    fi
}

# Get project ID and field IDs if custom fields are requested
PROJECT_ID=""
AREA_FIELD_ID=""
AREA_OPTION_ID=""
PRIO_FIELD_ID=""

if [ -n "$AREA_VALUE" ] || [ -n "$PRIO_VALUE" ]; then
    echo "Retrieving project and field information..."
    
    # Get project ID
    # Note: Don't redirect stderr to stdout to avoid mixing debug output with JSON
    PROJECT_DATA=$(gh project view "$PROJECT_NUMBER" --owner "$OWNER" --format json 2>/dev/null)
    EXIT_CODE=$?
    if [ $EXIT_CODE -ne 0 ]; then
        echo "Error: Failed to get project information"
        # Try again with stderr to show the error
        gh project view "$PROJECT_NUMBER" --owner "$OWNER" --format json
        exit 1
    fi
    PROJECT_ID=$(echo "$PROJECT_DATA" | jq -r '.id')
    
    if [ -z "$PROJECT_ID" ] || [ "$PROJECT_ID" = "null" ]; then
        echo "Error: Could not retrieve project ID"
        exit 1
    fi
    
    # Get field information
    FIELDS_DATA=$(gh project field-list "$PROJECT_NUMBER" --owner "$OWNER" --format json --limit 100 2>/dev/null)
    EXIT_CODE=$?
    if [ $EXIT_CODE -ne 0 ]; then
        echo "Error: Failed to get field information"
        # Try again with stderr to show the error
        gh project field-list "$PROJECT_NUMBER" --owner "$OWNER" --format json --limit 100
        exit 1
    fi
    
    # Find Area field ID
    if [ -n "$AREA_VALUE" ]; then
        echo "==> Processing Area field..."
        AREA_FIELD_ID=$(echo "$FIELDS_DATA" | jq -r '.fields[] | select(.name == "Area") | .id')
        if [ -z "$AREA_FIELD_ID" ] || [ "$AREA_FIELD_ID" = "null" ]; then
            echo "  Creating 'Area' field in project..."
            # Create the Area field with the requested value as the first option
            CREATE_RESULT=$(gh project field-create "$PROJECT_NUMBER" --owner "$OWNER" --name "Area" --data-type "SINGLE_SELECT" --single-select-options "$AREA_VALUE" --format json 2>/dev/null)
            if [ $? -eq 0 ]; then
                echo "  ✓ Created 'Area' field with option '$AREA_VALUE'"
                # Refresh field data
                FIELDS_DATA=$(gh project field-list "$PROJECT_NUMBER" --owner "$OWNER" --format json --limit 100 2>/dev/null)
                AREA_FIELD_ID=$(echo "$FIELDS_DATA" | jq -r '.fields[] | select(.name == "Area") | .id')
                AREA_OPTION_ID=$(echo "$FIELDS_DATA" | jq -r ".fields[] | select(.name == \"Area\") | .options[]? | select(.name == \"$AREA_VALUE\") | .id")
            else
                echo "  ✗ Failed to create 'Area' field"
                gh project field-create "$PROJECT_NUMBER" --owner "$OWNER" --name "Area" --data-type "SINGLE_SELECT" --single-select-options "$AREA_VALUE" --format json
                AREA_VALUE=""
            fi
        else
            echo "  ✓ Found existing 'Area' field"
            # For single-select fields, we need to get the option ID
            AREA_OPTION_ID=$(echo "$FIELDS_DATA" | jq -r ".fields[] | select(.name == \"Area\") | .options[]? | select(.name == \"$AREA_VALUE\") | .id")
            if [ -z "$AREA_OPTION_ID" ] || [ "$AREA_OPTION_ID" = "null" ]; then
                # Add the new option to the existing field using helper function
                AREA_OPTION_ID=$(add_field_option "Area" "$AREA_FIELD_ID" "$AREA_VALUE" "$PROJECT_ID")
                if [ $? -ne 0 ] || [ -z "$AREA_OPTION_ID" ] || [ "$AREA_OPTION_ID" = "null" ]; then
                    echo "  Available options:"
                    echo "$FIELDS_DATA" | jq -r '.fields[] | select(.name == "Area") | .options[]? | .name' | sed 's/^/  - /'
                    AREA_VALUE=""
                fi
            else
                echo "  ✓ Found option '$AREA_VALUE' in 'Area' field"
            fi
        fi
    fi
    
    # Find Prio field ID
    if [ -n "$PRIO_VALUE" ]; then
        echo "==> Processing Prio field..."
        PRIO_FIELD_ID=$(echo "$FIELDS_DATA" | jq -r '.fields[] | select(.name == "Prio") | .id')
        PRIO_FIELD_TYPE=$(echo "$FIELDS_DATA" | jq -r '.fields[] | select(.name == "Prio") | .type')
        
        if [ -z "$PRIO_FIELD_ID" ] || [ "$PRIO_FIELD_ID" = "null" ]; then
            echo "  Creating 'Prio' field in project as NUMBER type..."
            # Create the Prio field as a NUMBER field
            CREATE_RESULT=$(gh project field-create "$PROJECT_NUMBER" --owner "$OWNER" --name "Prio" --data-type "NUMBER" --format json 2>/dev/null)
            if [ $? -eq 0 ]; then
                echo "  ✓ Created 'Prio' field"
                # Refresh field data
                FIELDS_DATA=$(gh project field-list "$PROJECT_NUMBER" --owner "$OWNER" --format json --limit 100 2>/dev/null)
                PRIO_FIELD_ID=$(echo "$FIELDS_DATA" | jq -r '.fields[] | select(.name == "Prio") | .id')
            else
                echo "  ✗ Failed to create 'Prio' field"
                gh project field-create "$PROJECT_NUMBER" --owner "$OWNER" --name "Prio" --data-type "NUMBER" --format json
                PRIO_VALUE=""
            fi
        else
            echo "  ✓ Found existing 'Prio' field (type: $PRIO_FIELD_TYPE)"
            # Validate that the value is a number
            if ! [[ "$PRIO_VALUE" =~ ^[0-9]+$ ]]; then
                echo "  ✗ Warning: 'Prio' value '$PRIO_VALUE' is not a valid number. Skipping Prio field updates."
                PRIO_VALUE=""
            fi
        fi
    fi
    
    echo ""
fi

# Navigate to repository root
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "$SCRIPT_DIR/.." && pwd)"
cd "$REPO_ROOT"

echo "==> Finding samples that reference NServiceBus v10.0.0-alpha.X..."
echo ""

# Find all .csproj files that reference any 10.0.0-alpha version
# For each file, walk up the directory tree to find the sample.md file
# Group samples by their parent folder
echo "  Scanning .csproj files..."
SAMPLES_RAW=$(find samples -name "*.csproj" -exec grep -l "NServiceBus.*10\.0\.0-alpha" {} \; | \
    while read file; do
        dir=$(dirname "$file")
        # Walk up until we find sample.md or reach samples directory
        while [ "$dir" != "samples" ] && [ ! -f "$dir/sample.md" ]; do
            dir=$(dirname "$dir")
        done
        # Only output if we found a sample.md
        if [ -f "$dir/sample.md" ]; then
            parent=$(dirname "$dir")
            echo "$parent|$dir"
        fi
    done | sort -u)

echo "  ✓ Found $(echo "$SAMPLES_RAW" | wc -l) sample entries"
echo ""
echo "==> Grouping samples by parent folder..."

# Group samples by parent folder
declare -A SAMPLE_GROUPS
while IFS='|' read -r parent sample; do
    if [ -n "$parent" ] && [ -n "$sample" ]; then
        if [ -z "${SAMPLE_GROUPS[$parent]}" ]; then
            SAMPLE_GROUPS[$parent]="$sample"
        else
            SAMPLE_GROUPS[$parent]="${SAMPLE_GROUPS[$parent]}
$sample"
        fi
    fi
done <<< "$SAMPLES_RAW"

# Count total parent folders (cards to be created)
TOTAL=${#SAMPLE_GROUPS[@]}

echo "Found $TOTAL sample groups referencing NServiceBus v10.0.0-alpha.X"
echo ""
echo "Sample groups to be added:"
for parent in $(printf '%s\n' "${!SAMPLE_GROUPS[@]}" | sort); do
    echo "  $parent:"
    while IFS= read -r sample; do
        echo "    - $sample"
    done <<< "${SAMPLE_GROUPS[$parent]}"
done
echo ""

read -p "Do you want to proceed and add these $TOTAL groups to project $PROJECT_NUMBER? (y/N) " -n 1 -r
echo ""

if [[ ! $REPLY =~ ^[Yy]$ ]]; then
    echo "Aborted by user"
    exit 0
fi

echo ""
echo "==> Adding sample groups to project..."
echo ""

SUCCESS_COUNT=0
FAILED_COUNT=0
ALREADY_EXISTS_COUNT=0

# Process each parent folder (create one card per group)
for parent in "${!SAMPLE_GROUPS[@]}"; do
    echo "==> Processing group: $parent"
    # Extract parent folder name for the title
    PARENT_NAME=$(basename "$parent")
    CATEGORY=$(echo "$parent" | cut -d'/' -f2)
    
    # Create a meaningful title for the card
    TITLE="[v10 alpha] $CATEGORY/$PARENT_NAME"
    echo "  Title: $TITLE"
    
    # Build the card body with checkboxes for each sample
    BODY="**Sample group:** \`$parent\`

Samples in this group that reference NServiceBus v10.0.0-alpha.X:

"
    
    echo "  Building card body with samples..."
    # Add each sample as a checkbox
    while IFS= read -r sample; do
        [ -z "$sample" ] && continue
        
        # Extract sample name
        SAMPLE_NAME=$(basename "$sample")
        
        # Extract the sample title from sample.md if it exists
        SAMPLE_TITLE=""
        if [ -f "$sample/sample.md" ]; then
            SAMPLE_TITLE=$(grep "^title:" "$sample/sample.md" | head -1 | sed 's/^title: *//' | sed 's/^"//' | sed 's/"$//')
        fi
        
        # Use sample name if no title found
        if [ -z "$SAMPLE_TITLE" ]; then
            SAMPLE_TITLE="$SAMPLE_NAME"
        fi
        
        # Add checkbox with link
        BODY="${BODY}- [ ] [$SAMPLE_TITLE](https://github.com/$OWNER/$REPO/tree/main/$sample)
"
        echo "    - $SAMPLE_TITLE"
    done <<< "${SAMPLE_GROUPS[$parent]}"
    
    echo "  Adding card to project..."
    
    # Try to create draft item in project using gh CLI
    # Note: This uses the new Projects (beta) API
    # Capture output but avoid mixing debug output with JSON
    ADD_OUTPUT=$(gh project item-create "$PROJECT_NUMBER" --owner "$OWNER" --title "$TITLE" --body "$BODY" --format json 2>/dev/null)
    EXIT_CODE=$?
    
    if [ $EXIT_CODE -eq 0 ]; then
        echo "  ✓ Successfully added"
        ((SUCCESS_COUNT++))
        
        # Set custom fields if requested
        if [ -n "$AREA_VALUE" ] || [ -n "$PRIO_VALUE" ]; then
            # Extract item ID from the response
            ITEM_ID=$(echo "$ADD_OUTPUT" | jq -r '.id')
            
            if [ -n "$ITEM_ID" ] && [ "$ITEM_ID" != "null" ]; then
                # Set Area field
                if [ -n "$AREA_VALUE" ] && [ -n "$AREA_FIELD_ID" ] && [ -n "$AREA_OPTION_ID" ]; then
                    if gh project item-edit --id "$ITEM_ID" --field-id "$AREA_FIELD_ID" --project-id "$PROJECT_ID" --single-select-option-id "$AREA_OPTION_ID" > /dev/null 2>&1; then
                        echo "    ✓ Set Area: $AREA_VALUE"
                    else
                        echo "    ⚠ Failed to set Area field"
                    fi
                fi
                
                # Set Prio field (NUMBER type)
                if [ -n "$PRIO_VALUE" ] && [ -n "$PRIO_FIELD_ID" ]; then
                    if gh project item-edit --id "$ITEM_ID" --field-id "$PRIO_FIELD_ID" --project-id "$PROJECT_ID" --number "$PRIO_VALUE" > /dev/null 2>&1; then
                        echo "    ✓ Set Prio: $PRIO_VALUE"
                    else
                        echo "    ⚠ Failed to set Prio field"
                    fi
                fi
            fi
        fi
    elif echo "$ADD_OUTPUT" | grep -q "already exists"; then
        echo "  ⚠ Already exists in project"
        ((ALREADY_EXISTS_COUNT++))
    else
        echo "  ✗ Failed: $ADD_OUTPUT"
        ((FAILED_COUNT++))
    fi
    echo ""
done

echo "============================================"
echo "Summary:"
echo "  Total sample groups found: $TOTAL"
echo "  Successfully added: $SUCCESS_COUNT"
echo "  Already existed: $ALREADY_EXISTS_COUNT"
echo "  Failed: $FAILED_COUNT"
echo "============================================"

if [ $FAILED_COUNT -gt 0 ]; then
    exit 1
fi

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
#   --area AREA    - Value to set for the "Area" custom field (optional)
#   --prio PRIO    - Value to set for the "Prio" custom field (optional)
#
# Prerequisites:
#   - GitHub CLI (gh) must be installed and authenticated
#   - User must have write access to the specified project
#
# Example:
#   ./add-alpha-samples-to-project.sh 42
#   ./add-alpha-samples-to-project.sh 42 Particular docs.particular.net
#   ./add-alpha-samples-to-project.sh 42 --area "Core" --prio "High"
#   ./add-alpha-samples-to-project.sh 42 Particular docs.particular.net --area "Core"

set -e

# Check if gh CLI is available
if ! command -v gh &> /dev/null; then
    echo "Error: GitHub CLI (gh) is not installed or not in PATH"
    echo "Please install from: https://cli.github.com/"
    exit 1
fi

# Check if jq is available (needed for parsing JSON from gh CLI)
if ! command -v jq &> /dev/null; then
    echo "Error: jq is not installed or not in PATH"
    echo "jq is required for parsing JSON responses from GitHub CLI"
    echo "Please install from: https://jqlang.github.io/jq/"
    exit 1
fi

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

# Get project ID and field IDs if custom fields are requested
PROJECT_ID=""
AREA_FIELD_ID=""
AREA_OPTION_ID=""
PRIO_FIELD_ID=""
PRIO_OPTION_ID=""

if [ -n "$AREA_VALUE" ] || [ -n "$PRIO_VALUE" ]; then
    echo "Retrieving project and field information..."
    
    # Get project ID
    PROJECT_DATA=$(gh project view "$PROJECT_NUMBER" --owner "$OWNER" --format json 2>&1)
    if [ $? -ne 0 ]; then
        echo "Error: Failed to get project information"
        echo "$PROJECT_DATA"
        exit 1
    fi
    PROJECT_ID=$(echo "$PROJECT_DATA" | jq -r '.id')
    
    if [ -z "$PROJECT_ID" ] || [ "$PROJECT_ID" = "null" ]; then
        echo "Error: Could not retrieve project ID"
        exit 1
    fi
    
    # Get field information
    FIELDS_DATA=$(gh project field-list "$PROJECT_NUMBER" --owner "$OWNER" --format json --limit 100 2>&1)
    if [ $? -ne 0 ]; then
        echo "Error: Failed to get field information"
        echo "$FIELDS_DATA"
        exit 1
    fi
    
    # Find Area field ID
    if [ -n "$AREA_VALUE" ]; then
        AREA_FIELD_ID=$(echo "$FIELDS_DATA" | jq -r '.fields[] | select(.name == "Area") | .id')
        if [ -z "$AREA_FIELD_ID" ] || [ "$AREA_FIELD_ID" = "null" ]; then
            echo "Creating 'Area' field in project..."
            # Create the Area field with the requested value as the first option
            CREATE_RESULT=$(gh project field-create "$PROJECT_NUMBER" --owner "$OWNER" --name "Area" --data-type "SINGLE_SELECT" --single-select-options "$AREA_VALUE" --format json 2>&1)
            if [ $? -eq 0 ]; then
                echo "  ✓ Created 'Area' field with option '$AREA_VALUE'"
                # Refresh field data
                FIELDS_DATA=$(gh project field-list "$PROJECT_NUMBER" --owner "$OWNER" --format json --limit 100 2>&1)
                AREA_FIELD_ID=$(echo "$FIELDS_DATA" | jq -r '.fields[] | select(.name == "Area") | .id')
                AREA_OPTION_ID=$(echo "$FIELDS_DATA" | jq -r ".fields[] | select(.name == \"Area\") | .options[]? | select(.name == \"$AREA_VALUE\") | .id")
            else
                echo "  ✗ Failed to create 'Area' field: $CREATE_RESULT"
                AREA_VALUE=""
            fi
        else
            # For single-select fields, we need to get the option ID
            AREA_OPTION_ID=$(echo "$FIELDS_DATA" | jq -r ".fields[] | select(.name == \"Area\") | .options[]? | select(.name == \"$AREA_VALUE\") | .id")
            if [ -z "$AREA_OPTION_ID" ] || [ "$AREA_OPTION_ID" = "null" ]; then
                echo "Adding '$AREA_VALUE' option to 'Area' field..."
                # Add the new option to the existing field using GraphQL
                MUTATION=$(cat <<EOF
mutation {
  addProjectV2SingleSelectFieldOption(input: {
    projectId: "$PROJECT_ID"
    fieldId: "$AREA_FIELD_ID"
    name: "$AREA_VALUE"
  }) {
    option {
      id
      name
    }
  }
}
EOF
)
                ADD_OPTION_RESULT=$(gh api graphql -f query="$MUTATION" 2>&1)
                if [ $? -eq 0 ]; then
                    echo "  ✓ Added '$AREA_VALUE' option to 'Area' field"
                    # Extract the new option ID
                    AREA_OPTION_ID=$(echo "$ADD_OPTION_RESULT" | jq -r '.data.addProjectV2SingleSelectFieldOption.option.id')
                else
                    echo "  ✗ Failed to add option: $ADD_OPTION_RESULT"
                    echo "Available options:"
                    echo "$FIELDS_DATA" | jq -r '.fields[] | select(.name == "Area") | .options[]? | .name' | sed 's/^/  - /'
                    AREA_VALUE=""
                fi
            fi
        fi
    fi
    
    # Find Prio field ID
    if [ -n "$PRIO_VALUE" ]; then
        PRIO_FIELD_ID=$(echo "$FIELDS_DATA" | jq -r '.fields[] | select(.name == "Prio") | .id')
        if [ -z "$PRIO_FIELD_ID" ] || [ "$PRIO_FIELD_ID" = "null" ]; then
            echo "Creating 'Prio' field in project..."
            # Create the Prio field with the requested value as the first option
            CREATE_RESULT=$(gh project field-create "$PROJECT_NUMBER" --owner "$OWNER" --name "Prio" --data-type "SINGLE_SELECT" --single-select-options "$PRIO_VALUE" --format json 2>&1)
            if [ $? -eq 0 ]; then
                echo "  ✓ Created 'Prio' field with option '$PRIO_VALUE'"
                # Refresh field data
                FIELDS_DATA=$(gh project field-list "$PROJECT_NUMBER" --owner "$OWNER" --format json --limit 100 2>&1)
                PRIO_FIELD_ID=$(echo "$FIELDS_DATA" | jq -r '.fields[] | select(.name == "Prio") | .id')
                PRIO_OPTION_ID=$(echo "$FIELDS_DATA" | jq -r ".fields[] | select(.name == \"Prio\") | .options[]? | select(.name == \"$PRIO_VALUE\") | .id")
            else
                echo "  ✗ Failed to create 'Prio' field: $CREATE_RESULT"
                PRIO_VALUE=""
            fi
        else
            # For single-select fields, we need to get the option ID
            PRIO_OPTION_ID=$(echo "$FIELDS_DATA" | jq -r ".fields[] | select(.name == \"Prio\") | .options[]? | select(.name == \"$PRIO_VALUE\") | .id")
            if [ -z "$PRIO_OPTION_ID" ] || [ "$PRIO_OPTION_ID" = "null" ]; then
                echo "Adding '$PRIO_VALUE' option to 'Prio' field..."
                # Add the new option to the existing field using GraphQL
                MUTATION=$(cat <<EOF
mutation {
  addProjectV2SingleSelectFieldOption(input: {
    projectId: "$PROJECT_ID"
    fieldId: "$PRIO_FIELD_ID"
    name: "$PRIO_VALUE"
  }) {
    option {
      id
      name
    }
  }
}
EOF
)
                ADD_OPTION_RESULT=$(gh api graphql -f query="$MUTATION" 2>&1)
                if [ $? -eq 0 ]; then
                    echo "  ✓ Added '$PRIO_VALUE' option to 'Prio' field"
                    # Extract the new option ID
                    PRIO_OPTION_ID=$(echo "$ADD_OPTION_RESULT" | jq -r '.data.addProjectV2SingleSelectFieldOption.option.id')
                else
                    echo "  ✗ Failed to add option: $ADD_OPTION_RESULT"
                    echo "Available options:"
                    echo "$FIELDS_DATA" | jq -r '.fields[] | select(.name == "Prio") | .options[]? | .name' | sed 's/^/  - /'
                    PRIO_VALUE=""
                fi
            fi
        fi
    fi
    
    echo ""
fi

# Navigate to repository root
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "$SCRIPT_DIR/.." && pwd)"
cd "$REPO_ROOT"

echo "Finding samples that reference NServiceBus v10.0.0-alpha.X..."
echo ""

# Find all .csproj files that reference any 10.0.0-alpha version
# For each file, walk up the directory tree to find the sample.md file
# This correctly identifies the sample root directory
SAMPLES=$(find samples -name "*.csproj" -exec grep -l "NServiceBus.*10\.0\.0-alpha" {} \; | \
    while read file; do
        dir=$(dirname "$file")
        # Walk up until we find sample.md or reach samples directory
        while [ "$dir" != "samples" ] && [ ! -f "$dir/sample.md" ]; do
            dir=$(dirname "$dir")
        done
        # Only output if we found a sample.md
        if [ -f "$dir/sample.md" ]; then
            echo "$dir"
        fi
    done | sort -u)

# Count total samples
if [ -z "$SAMPLES" ] || [ "$SAMPLES" = "" ]; then
    TOTAL=0
else
    TOTAL=$(echo "$SAMPLES" | grep -c '^')
fi

echo "Found $TOTAL unique samples referencing NServiceBus v10.0.0-alpha.X"
echo ""
echo "Samples to be added:"
echo "$SAMPLES"
echo ""

read -p "Do you want to proceed and add these $TOTAL samples to project $PROJECT_NUMBER? (y/N) " -n 1 -r
echo ""

if [[ ! $REPLY =~ ^[Yy]$ ]]; then
    echo "Aborted by user"
    exit 0
fi

echo ""
echo "Adding samples to project..."
echo ""

SUCCESS_COUNT=0
FAILED_COUNT=0
ALREADY_EXISTS_COUNT=0

# Process each sample
while IFS= read -r sample; do
    # Skip empty lines
    [ -z "$sample" ] && continue
    
    # Extract the sample title from sample.md if it exists
    SAMPLE_TITLE=""
    if [ -f "$sample/sample.md" ]; then
        # Try to extract title from the markdown frontmatter
        SAMPLE_TITLE=$(grep "^title:" "$sample/sample.md" | head -1 | sed 's/^title: *//' | sed 's/^"//' | sed 's/"$//')
    fi
    
    # Fallback to path-based name if no title found
    if [ -z "$SAMPLE_TITLE" ]; then
        CATEGORY=$(echo "$sample" | cut -d'/' -f2)
        SAMPLE_NAME=$(echo "$sample" | cut -d'/' -f3-)
        SAMPLE_TITLE="$CATEGORY/$SAMPLE_NAME"
    fi
    
    # Create a meaningful title for the card
    TITLE="[v10 alpha] $SAMPLE_TITLE"
    
    # Create card body with link to sample
    BODY="**Sample path:** \`$sample\`

This sample references NServiceBus v10.0.0-alpha.X and may need to be reviewed/updated.

**Repository link:** https://github.com/$OWNER/$REPO/tree/main/$sample"
    
    echo "Adding: $TITLE"
    
    # Try to add item to project using gh CLI
    # Note: This uses the new Projects (beta) API
    # Capture output to avoid duplicate calls
    ADD_OUTPUT=$(gh project item-add "$PROJECT_NUMBER" --owner "$OWNER" --title "$TITLE" --body "$BODY" --format json 2>&1)
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
                
                # Set Prio field
                if [ -n "$PRIO_VALUE" ] && [ -n "$PRIO_FIELD_ID" ] && [ -n "$PRIO_OPTION_ID" ]; then
                    if gh project item-edit --id "$ITEM_ID" --field-id "$PRIO_FIELD_ID" --project-id "$PROJECT_ID" --single-select-option-id "$PRIO_OPTION_ID" > /dev/null 2>&1; then
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
done <<< "$SAMPLES"

echo "============================================"
echo "Summary:"
echo "  Total samples found: $TOTAL"
echo "  Successfully added: $SUCCESS_COUNT"
echo "  Already existed: $ALREADY_EXISTS_COUNT"
echo "  Failed: $FAILED_COUNT"
echo "============================================"

if [ $FAILED_COUNT -gt 0 ]; then
    exit 1
fi

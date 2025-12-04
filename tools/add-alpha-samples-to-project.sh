#!/bin/bash
# Script to add all samples referencing NServiceBus v10.0.0-alpha.X to a GitHub project board
#
# Usage:
#   ./add-alpha-samples-to-project.sh <PROJECT_NUMBER> [OWNER] [REPO]
#
# Arguments:
#   PROJECT_NUMBER - The GitHub project number (required)
#   OWNER          - Repository owner (default: Particular)
#   REPO           - Repository name (default: docs.particular.net)
#
# Prerequisites:
#   - GitHub CLI (gh) must be installed and authenticated
#   - User must have write access to the specified project
#
# Example:
#   ./add-alpha-samples-to-project.sh 42
#   ./add-alpha-samples-to-project.sh 42 Particular docs.particular.net

set -e

# Check if gh CLI is available
if ! command -v gh &> /dev/null; then
    echo "Error: GitHub CLI (gh) is not installed or not in PATH"
    echo "Please install from: https://cli.github.com/"
    exit 1
fi

# Check if required argument is provided
if [ -z "$1" ]; then
    echo "Error: PROJECT_NUMBER is required"
    echo ""
    echo "Usage: $0 <PROJECT_NUMBER> [OWNER] [REPO]"
    echo ""
    echo "Example: $0 42"
    exit 1
fi

PROJECT_NUMBER=$1
OWNER=${2:-Particular}
REPO=${3:-docs.particular.net}

echo "Configuration:"
echo "  Project Number: $PROJECT_NUMBER"
echo "  Owner: $OWNER"
echo "  Repo: $REPO"
echo ""

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
    ERROR_OUTPUT=$(gh project item-add "$PROJECT_NUMBER" --owner "$OWNER" --title "$TITLE" --body "$BODY" 2>&1)
    EXIT_CODE=$?
    
    if [ $EXIT_CODE -eq 0 ]; then
        echo "  ✓ Successfully added"
        ((SUCCESS_COUNT++))
    elif echo "$ERROR_OUTPUT" | grep -q "already exists"; then
        echo "  ⚠ Already exists in project"
        ((ALREADY_EXISTS_COUNT++))
    else
        echo "  ✗ Failed: $ERROR_OUTPUT"
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

#!/bin/bash
# Script to list all samples referencing NServiceBus v10.0.0-alpha.X
#
# This is a dry-run script that shows what samples would be added to a project
# without actually adding them. Useful for verification before running the
# add-alpha-samples-to-project.sh script.
#
# Usage:
#   ./list-alpha-samples.sh

set -e

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
TOTAL=$(echo "$SAMPLES" | wc -l)

echo "Found $TOTAL unique samples referencing NServiceBus v10.0.0-alpha.X"
echo ""
echo "Samples list with titles:"
echo "========================="
echo ""

# Process each sample and show what would be added
COUNT=0
while IFS= read -r sample; do
    [ -z "$sample" ] && continue
    COUNT=$((COUNT + 1))
    
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
    
    # Show what would be the card title
    CARD_TITLE="[v10 alpha] $SAMPLE_TITLE"
    
    printf "%3d. %s\n" "$COUNT" "$CARD_TITLE"
    printf "     Path: %s\n" "$sample"
    echo ""
done <<< "$SAMPLES"

echo "========================="
echo "Total: $TOTAL samples"
echo ""
echo "To add these samples to a GitHub project, run:"
echo "  ./tools/add-alpha-samples-to-project.sh <PROJECT_NUMBER>"

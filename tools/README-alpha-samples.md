# NServiceBus v10.0.0-alpha Sample Management Scripts

This directory contains scripts to help manage samples that reference NServiceBus v10.0.0-alpha.X versions.

## Scripts

### list-alpha-samples.sh

Lists all samples in the repository that reference any NServiceBus v10.0.0-alpha.X version.

**Usage:**
```bash
./tools/list-alpha-samples.sh
```

**Output:**
- Lists all samples with their titles extracted from `sample.md`
- Shows the path to each sample
- Displays total count of samples found

This is useful for:
- Getting an overview of which samples use alpha versions
- Verifying what would be added before running the add script
- Documentation and tracking purposes

### add-alpha-samples-to-project.sh

Adds all samples referencing NServiceBus v10.0.0-alpha.X as cards to a GitHub project board.

**Usage:**
```bash
./tools/add-alpha-samples-to-project.sh <PROJECT_NUMBER> [OWNER] [REPO]
```

**Arguments:**
- `PROJECT_NUMBER` (required): The GitHub project number
- `OWNER` (optional): Repository owner (default: `Particular`)
- `REPO` (optional): Repository name (default: `docs.particular.net`)

**Example:**
```bash
# Add samples to project #42
./tools/add-alpha-samples-to-project.sh 42

# Add samples to project #42 in a different repo
./tools/add-alpha-samples-to-project.sh 42 MyOrg MyRepo
```

**Prerequisites:**
- GitHub CLI (`gh`) must be installed and authenticated
- User must have write access to the specified project

**Features:**
- Automatically finds all samples referencing NServiceBus v10.0.0-alpha.X
- Extracts sample titles from `sample.md` files
- Creates project cards with meaningful titles in format: `[v10 alpha] <Sample Title>`
- Includes sample path and repository link in card body
- Shows a confirmation prompt before adding cards
- Reports progress and summary of operations
- Handles duplicate cards gracefully

## How It Works

Both scripts use the following logic to identify samples:

1. Find all `.csproj` files that reference `NServiceBus` with version `10.0.0-alpha.*`
2. For each matching project file, walk up the directory tree to find the `sample.md` file
3. Extract unique sample directories (multiple projects in a sample count as one sample)
4. Extract sample titles from the `sample.md` frontmatter

This approach correctly identifies sample boundaries and provides accurate counts.

## Finding Your Project Number

To find your GitHub project number:

1. Navigate to your project on GitHub
2. Look at the URL: `https://github.com/orgs/Particular/projects/123`
3. The number at the end (e.g., `123`) is your project number

Alternatively, use the GitHub CLI:
```bash
gh project list --owner Particular
```

## Notes

- The scripts search for `10.0.0-alpha` followed by any digit (e.g., alpha.1, alpha.2, alpha.3, etc.)
- Currently, 141 unique samples reference NServiceBus v10.0.0-alpha.X versions
- Samples are identified by their root directory containing `sample.md`, not by version folder

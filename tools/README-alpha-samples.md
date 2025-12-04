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
./tools/add-alpha-samples-to-project.sh <PROJECT_NUMBER> [OWNER] [REPO] [--area AREA] [--prio PRIO]
```

**Arguments:**
- `PROJECT_NUMBER` (required): The GitHub project number
- `OWNER` (optional): Repository owner (default: `Particular`)
- `REPO` (optional): Repository name (default: `docs.particular.net`)
- `--area AREA` (optional): Value to set for the "Area" custom project field
- `--prio PRIO` (optional): Value to set for the "Prio" custom project field

**Example:**
```bash
# Add samples to project #42
./tools/add-alpha-samples-to-project.sh 42

# Add samples to project #42 in a different repo
./tools/add-alpha-samples-to-project.sh 42 MyOrg MyRepo

# Add samples with custom field values
./tools/add-alpha-samples-to-project.sh 42 --area "Core" --prio "High"

# Combine all options
./tools/add-alpha-samples-to-project.sh 42 Particular docs.particular.net --area "Samples" --prio "Medium"
```

**Prerequisites:**
- GitHub CLI (`gh`) must be installed and authenticated
- User must have write access to the specified project
- Custom fields and their values will be created automatically if they don't exist

**Features:**
- Automatically finds all samples referencing NServiceBus v10.0.0-alpha.X
- Extracts sample titles from `sample.md` files
- Creates project cards with meaningful titles in format: `[v10 alpha] <Sample Title>`
- Includes sample path and repository link in card body
- Shows a confirmation prompt before adding cards
- Reports progress and summary of operations
- Handles duplicate cards gracefully
- Optionally sets custom project fields ("Area" and "Prio") on each card
- Automatically creates fields and options if they don't exist in the project

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

## Custom Project Fields

The script supports setting custom project fields for each card added. This is useful for organizing and categorizing samples.

### Supported Fields

- **Area**: Categorize samples by area (e.g., "Core", "Transports", "Persistence")
- **Prio**: Set priority for samples (e.g., "High", "Medium", "Low")

### Automatic Field and Option Creation

The script will automatically create fields and options if they don't exist:

1. **Field doesn't exist**: Creates the field with the specified value as the first option
2. **Field exists but option doesn't**: Adds the new option to the existing field
3. **Both exist**: Uses the existing field and option

This means you can use any value you want, and the script will set up the project accordingly.

### Finding Available Field Values

To see what values are currently available for your custom fields:

```bash
gh project field-list <PROJECT_NUMBER> --owner Particular --format json | jq '.fields[] | select(.name == "Area" or .name == "Prio") | {name, options: [.options[]?.name]}'
```

This will show you the available options for each field.

## Notes

- The scripts search for `10.0.0-alpha` followed by any digit (e.g., alpha.1, alpha.2, alpha.3, etc.)
- Samples are identified by their root directory containing `sample.md`, not by version folder
- The number of samples may change as the repository evolves

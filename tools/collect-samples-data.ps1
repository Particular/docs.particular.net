<#
Collects docs sample metadata into a single JSON payload.

Relationship to automation:
- This script is executed by .github/workflows/collect-sample-data.yml in docs.particular.net.
- The workflow uploads the generated JSON to InternalAutomation's ProcessSamplesDataResult HTTP function.
- That function stores the payload in blob storage for downstream consumers.
#>

param(
    [string]$DocsRepoRoot = "..",
    [string]$OutputPath = "../samples-data.json"
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

function Resolve-AbsolutePath([string]$basePath, [string]$targetPath) {
    if ([System.IO.Path]::IsPathRooted($targetPath)) {
        return [System.IO.Path]::GetFullPath($targetPath)
    }

    return [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($basePath, $targetPath))
}

function Parse-Scalar([string]$value) {
    if ($null -eq $value) {
        return $null
    }

    $trimmed = $value.Trim()
    if ($trimmed.Length -eq 0) {
        return ""
    }

    if (($trimmed.StartsWith('"') -and $trimmed.EndsWith('"')) -or ($trimmed.StartsWith("'") -and $trimmed.EndsWith("'"))) {
        $trimmed = $trimmed.Substring(1, $trimmed.Length - 2)
    }

    if ($trimmed -eq "true") {
        return $true
    }

    if ($trimmed -eq "false") {
        return $false
    }

    return $trimmed
}

function Parse-FrontMatter([string]$sampleMdPath) {
    $raw = Get-Content -Path $sampleMdPath -Raw
    $match = [regex]::Match($raw, "(?s)^---\s*\r?\n(.*?)\r?\n---")
    if (-not $match.Success) {
        return @{}
    }

    $frontMatter = $match.Groups[1].Value
    $lines = $frontMatter -split "\r?\n"
    $result = @{}
    $activeKey = $null

    foreach ($line in $lines) {
        if ([string]::IsNullOrWhiteSpace($line)) {
            continue
        }

        if ($line -match "^\s*([A-Za-z0-9_-]+)\s*:\s*(.*)$") {
            $activeKey = $matches[1]
            $valuePart = $matches[2]

            if ([string]::IsNullOrWhiteSpace($valuePart)) {
                $result[$activeKey] = @()
                continue
            }

            if ($valuePart.Trim().StartsWith("[") -and $valuePart.Trim().EndsWith("]")) {
                $inner = $valuePart.Trim().TrimStart("[").TrimEnd("]")
                if ([string]::IsNullOrWhiteSpace($inner)) {
                    $result[$activeKey] = @()
                }
                else {
                    $result[$activeKey] = @($inner.Split(",") | ForEach-Object { Parse-Scalar $_ })
                }
            }
            else {
                $result[$activeKey] = Parse-Scalar $valuePart
            }

            continue
        }

        if ($line -match "^\s*-\s*(.+)$" -and $null -ne $activeKey) {
            $currentValue = $result[$activeKey]
            if ($currentValue -isnot [System.Collections.IList]) {
                $currentValue = @()
            }
            $result[$activeKey] = @($currentValue) + @(Parse-Scalar $matches[1])
        }
    }

    return $result
}

function Get-PackageVersion($packageNode) {
    if ($null -ne $packageNode.Version -and -not [string]::IsNullOrWhiteSpace($packageNode.Version)) {
        return $packageNode.Version
    }

    if ($null -ne $packageNode.Attributes["Version"] -and -not [string]::IsNullOrWhiteSpace($packageNode.Attributes["Version"].Value)) {
        return $packageNode.Attributes["Version"].Value
    }

    return $null
}

function Parse-MajorVersion([string]$version) {
    if ([string]::IsNullOrWhiteSpace($version)) {
        return $null
    }

    $match = [regex]::Match($version.Trim(), "\d+")
    if (-not $match.Success) {
        return $null
    }

    return [int]$match.Value
}

function Get-NServiceBusMajorsFromPackages($packages) {
    $majorSet = [System.Collections.Generic.HashSet[int]]::new()

    foreach ($package in $packages) {
        if ($package.name -ne "NServiceBus") {
            continue
        }

        $major = Parse-MajorVersion $package.version
        if ($null -ne $major) {
            $null = $majorSet.Add($major)
        }
    }

    return @($majorSet | Sort-Object)
}

function Resolve-NServiceBusMajorsFromAssets([string]$versionDirectoryPath, $projectFiles) {
    $majorSet = [System.Collections.Generic.HashSet[int]]::new()

    try {
        $solutionFiles = @(Get-ChildItem -Path $versionDirectoryPath -Include "*.sln", "*.slnx" -File -Recurse | Sort-Object FullName)
        if ($solutionFiles.Count -gt 0) {
            foreach ($solution in $solutionFiles) {
                Write-Host "Restoring solution for NSB major detection: $($solution.FullName)"
                & dotnet restore $solution.FullName --nologo | Out-Null
                if ($LASTEXITCODE -ne 0) {
                    throw "dotnet restore failed for '$($solution.FullName)'."
                }
            }
        }
        else {
            foreach ($projectFile in $projectFiles) {
                Write-Host "Restoring project for NSB major detection: $($projectFile.FullName)"
                & dotnet restore $projectFile.FullName --nologo | Out-Null
                if ($LASTEXITCODE -ne 0) {
                    throw "dotnet restore failed for '$($projectFile.FullName)'."
                }
            }
        }
    }
    catch {
        Write-Warning "Failed to restore in '$versionDirectoryPath' for NSB major detection. $($_.Exception.Message)"
        return @()
    }

    foreach ($projectFile in $projectFiles) {
        $projectDirectory = Split-Path -Parent $projectFile.FullName
        $assetsPath = Join-Path $projectDirectory "obj/project.assets.json"

        if (-not (Test-Path $assetsPath)) {
            continue
        }

        try {
            $assets = Get-Content -Path $assetsPath -Raw | ConvertFrom-Json -AsHashtable
            $libraries = $assets["libraries"]
            if ($null -eq $libraries) {
                continue
            }

            foreach ($libraryKey in $libraries.Keys) {
                $match = [regex]::Match($libraryKey, "^NServiceBus/([^/]+)$")
                if (-not $match.Success) {
                    continue
                }

                $major = Parse-MajorVersion $match.Groups[1].Value
                if ($null -ne $major) {
                    $null = $majorSet.Add($major)
                }
            }
        }
        catch {
            Write-Warning "Failed to inspect assets file '$assetsPath'. $($_.Exception.Message)"
        }
    }

    return @($majorSet | Sort-Object)
}

function Parse-ProjectData([string]$projectPath) {
    $xmlText = Get-Content -Path $projectPath -Raw
    [xml]$projectXml = $xmlText

    $packages = @()
    $frameworks = @()
    $langVersions = @()

    $packageReferences = $projectXml.SelectNodes("//*[local-name()='PackageReference']")
    foreach ($package in $packageReferences) {
        $name = $package.Include
        if ([string]::IsNullOrWhiteSpace($name) -and $null -ne $package.Attributes["Include"]) {
            $name = $package.Attributes["Include"].Value
        }

        if ([string]::IsNullOrWhiteSpace($name)) {
            continue
        }

        if ($name -notmatch "^(NServiceBus(\.|$)|Particular\.|ServiceControl\.)") {
            continue
        }

        $version = Get-PackageVersion $package

        $packages += [pscustomobject]@{
            name = $name
            version = $version
        }
    }

    $targetFrameworksNodes = $projectXml.SelectNodes("//*[local-name()='TargetFrameworks']")
    foreach ($frameworkNode in $targetFrameworksNodes) {
        if (-not [string]::IsNullOrWhiteSpace($frameworkNode.InnerText)) {
            $frameworks += $frameworkNode.InnerText.Split(";", [System.StringSplitOptions]::RemoveEmptyEntries)
        }
    }

    $targetFrameworkNodes = $projectXml.SelectNodes("//*[local-name()='TargetFramework']")
    foreach ($frameworkNode in $targetFrameworkNodes) {
        if (-not [string]::IsNullOrWhiteSpace($frameworkNode.InnerText)) {
            $frameworks += $frameworkNode.InnerText.Trim()
        }
    }

    $langVersionNodes = $projectXml.SelectNodes("//*[local-name()='LangVersion']")
    foreach ($langVersionNode in $langVersionNodes) {
        if (-not [string]::IsNullOrWhiteSpace($langVersionNode.InnerText)) {
            $langVersions += $langVersionNode.InnerText.Trim()
        }
    }

    return [ordered]@{
        packages = @($packages | Sort-Object name, version -Unique)
        frameworks = @($frameworks | ForEach-Object { $_.Trim() } | Where-Object { $_ } | Sort-Object -Unique)
        langVersions = @($langVersions | Sort-Object -Unique)
    }
}

$scriptDirectory = Split-Path -Parent $MyInvocation.MyCommand.Path
$docsAbsolutePath = Resolve-AbsolutePath $scriptDirectory $DocsRepoRoot
$outputAbsolutePath = Resolve-AbsolutePath $scriptDirectory $OutputPath

$samplesRoot = Join-Path $docsAbsolutePath "samples"
if (-not (Test-Path $samplesRoot)) {
    throw "Unable to find samples directory at '$samplesRoot'."
}

$sampleMdFiles = Get-ChildItem -Path $samplesRoot -Filter "sample.md" -File -Recurse | Sort-Object FullName
$samples = @()

foreach ($sampleMd in $sampleMdFiles) {
    $sampleDirectory = $sampleMd.Directory.FullName
    $relativeSamplePath = [System.IO.Path]::GetRelativePath($docsAbsolutePath, $sampleDirectory).Replace("\\", "/")
    $frontMatter = Parse-FrontMatter $sampleMd.FullName

    $candidateVersionDirectories = @(Get-ChildItem -Path $sampleDirectory -Directory | Sort-Object Name)
    $versionDirectories = @()

    foreach ($candidate in $candidateVersionDirectories) {
        $projectsInCandidate = @(Get-ChildItem -Path $candidate.FullName -Filter "*.csproj" -File -Recurse)
        if ($projectsInCandidate.Count -gt 0) {
            $versionDirectories += $candidate
        }
    }

    if ($versionDirectories.Count -eq 0) {
        $versionDirectories = @([pscustomobject]@{
                Name = "root"
                FullName = $sampleDirectory
            })
    }

    $versionRecords = @()

    foreach ($versionDirectory in $versionDirectories) {
        $projectFiles = @(Get-ChildItem -Path $versionDirectory.FullName -Filter "*.csproj" -File -Recurse | Sort-Object FullName)
        if ($projectFiles.Count -eq 0) {
            continue
        }

        $allPackages = @()
        $allFrameworks = @()
        $allLangVersions = @()

        foreach ($projectFile in $projectFiles) {
            $projectData = Parse-ProjectData $projectFile.FullName
            $allPackages += $projectData.packages
            $allFrameworks += $projectData.frameworks
            $allLangVersions += $projectData.langVersions
        }

        $packages = @($allPackages | Sort-Object name, version -Unique)
        $nsbMajors = @(Get-NServiceBusMajorsFromPackages $packages)
        if ($nsbMajors.Count -eq 0) {
            $hasAnyNsbPackage = $packages | Where-Object { $_.name -like "NServiceBus.*" } | Select-Object -First 1
            if ($null -ne $hasAnyNsbPackage) {
                $nsbMajors = @(Resolve-NServiceBusMajorsFromAssets $versionDirectory.FullName $projectFiles)
            }
        }

        $relativeVersionPath = [System.IO.Path]::GetRelativePath($docsAbsolutePath, $versionDirectory.FullName).Replace("\\", "/")
        $versionRecords += [ordered]@{
            versionName = $versionDirectory.Name
            path = $relativeVersionPath
            packages = $packages
            frameworks = @($allFrameworks | Sort-Object -Unique)
            langVersions = @($allLangVersions | Sort-Object -Unique)
            nserviceBusMajors = $nsbMajors
            projectCount = $projectFiles.Count
        }
    }

    $sampleNsbMajors = @($versionRecords | ForEach-Object { $_.nserviceBusMajors } | Where-Object { $null -ne $_ } | Sort-Object -Unique)

    $sampleUrl = "https://docs.particular.net/$relativeSamplePath/"
    $samples += [ordered]@{
        title = if ($frontMatter.ContainsKey("title")) { $frontMatter["title"] } else { $sampleMd.Directory.Name }
        summary = if ($frontMatter.ContainsKey("summary")) { $frontMatter["summary"] } else { $null }
        reviewed = if ($frontMatter.ContainsKey("reviewed")) { $frontMatter["reviewed"] } else { $null }
        isLearningPath = if ($frontMatter.ContainsKey("isLearningPath")) { [bool]$frontMatter["isLearningPath"] } else { $false }
        component = if ($frontMatter.ContainsKey("component")) { $frontMatter["component"] } else { $null }
        path = $relativeSamplePath
        url = $sampleUrl
        nserviceBusMajors = $sampleNsbMajors
        versions = @($versionRecords | Sort-Object versionName)
    }
}

$samples = @($samples | Sort-Object path)
$versionCount = ($samples | ForEach-Object { $_.versions.Count } | Measure-Object -Sum).Sum

$payload = [ordered]@{
    schemaVersion = "prototype-v1"
    generatedOnUtc = (Get-Date).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ")
    sourceRepository = $docsAbsolutePath
    sampleCount = $samples.Count
    versionCount = $versionCount
    samples = $samples
}

$outputDirectory = Split-Path -Parent $outputAbsolutePath
if (-not (Test-Path $outputDirectory)) {
    New-Item -Path $outputDirectory -ItemType Directory | Out-Null
}

$json = $payload | ConvertTo-Json -Depth 12
Set-Content -Path $outputAbsolutePath -Value $json

Write-Host "Generated samples data JSON: $outputAbsolutePath"
Write-Host "Sample count: $($samples.Count) | Version count: $versionCount"
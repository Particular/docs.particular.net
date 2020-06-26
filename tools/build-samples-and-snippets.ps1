# Assumes working directory is Docs repository root folder
function CombinePaths()
{
    param
    (
        [string] $first,
        [string] $second
    )
    $combined = [System.IO.Path]::Combine($first, $second)
    $fullPath = [System.IO.Path]::GetFullPath($combined)
    return $fullPath
}

# Assumes that the current working directory is the root of the docs repo
function Get-BuildSolutions
{
    # Retrieves the current branch name
    $branch = git rev-parse --abbrev-ref HEAD

    if($branch -eq "master")
    {
        # For master branch, build every solution file in repo
        $result = Get-ChildItem -Filter *.sln -Recurse | sort LastWriteTime -Descending
        return $result
    }

    # Fetch to update origin/master to the proper position
    $ignoreGitFetchOutput = git fetch origin master

    # `origin/master...HEAD` references commit where master & current branch diverged
    # Just comparing to master will grab changes that occurred in master as well
    $changes = git diff origin/master...HEAD --name-only
    $result = @()
    
    foreach($change in $changes)
    {
        $file = CombinePaths $pwd.Path $change
        $dir = CombinePaths $file ".." -Resolve

        while($dir.Length -gt $pwd.Path.Length)
        {
            # Tracing up from a removed file may result in a directory that no longer exists
            if(Test-Path $dir)
            {
                $dirSolutions = Get-ChildItem -Path $dir -Filter *.sln
                if($dirSolutions.Length -gt 0)
                {
                    # May be separate netfx/netcore solutions in same directory
                    foreach($sln in $dirSolutions)
                    {
                        $path = CombinePaths $dir $sln.Name
                        # Convert path string to FileInfo-like item
                        $item = Get-Item $path
                        $result = $result + $item
                    }
                    
                    # Found solution(s) for change, no need to keep navigating up
                    break;
                }
            }
            $dir = CombinePaths $dir ".."
        }
    }

    return $result | Sort-Object | Get-Unique
}

$exitCode = 0
$failedProjects = New-Object Collections.Generic.List[String]
$failedProjectsOutput = CombinePaths $pwd.Path $sln.Name
 
echo "::group::Get build solutions"
$samples = Get-BuildSolutions
Write-Host "Projects to build"
$samples | ForEach-Object { echo (" * {0}" -f $_.FullName) }
echo "::endgroup::"

foreach($sample in $samples) {
    echo ("::group::Build Project {0}" -f $sample.FullName)	
    
    Set-Location -Path $sample.Directory.FullName
    Get-ChildItem -inc bin,obj -rec | Remove-Item -rec -force
    
    msbuild $sample.Name -nodeReuse:true -verbosity:normal -restore -property:RestorePackagesConfig=true
	
	if( -not $? ) {
		$exitCode = 1
		echo ("::error::Build failed: {0}" -f $sample.FullName)
		$failedProjects.Add($sample.FullName)
	}
    echo "::endgroup::"
}

If ( $failedProjects.Count -ne 0 ) {
	New-Item -ItemType "file" -Path $failedProjectsOutput
	$failedProjects | ForEach-Object { 
        Add-Content $failedProjectsOutput $_ 
    }
}

exit $exitCode

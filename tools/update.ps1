Clear-Host
$docsDiretory = (get-item $PSScriptRoot ).parent.FullName
$snippetsFile =$docsDiretory +"\Snippets\NSBDoco.sln.DotSettings"
$snippetsFile 
$solutions = Get-ChildItem $docsDiretory -Filter *.sln -Recurse
foreach($solution in $solutions) {

	$targetFile = $solution.FullName + ".DotSettings"
	if ($targetFile -ne $snippetsFile)
	{
		$targetFile
		Copy-Item  -force  $snippetsFile $targetFile
	}
}
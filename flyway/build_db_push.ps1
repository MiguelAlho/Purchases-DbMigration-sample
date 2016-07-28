Param(
	[Parameter(Mandatory=$true)][string]$buildNumber,
	[Parameter(Mandatory=$true)][string]$dbList)
	
$dbAllNames = $dbList -split ","

foreach($dbName in $dbAllNames)
{	
	$packExtension = ".nupkg"	
	$packPath = "..\packages\" + $dbName
	$packVersion = ".0.0." + $buildNumber

	$packFullPath = $packPath + $packVersion + $packExtension 

	Write-Output $packFullPath

    nuget push $packFullPath -Source http://localhost:8081/nuget/packages -ApiKey API-OGJPMYDTEWDWLTI4X3OLIEU6KLG 
}

exit $LastExitCode
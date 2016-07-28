Param(
	[Parameter(Mandatory=$true)][string]$buildNumber,
	[Parameter(Mandatory=$true)][string]$dbList)
	
$repFolder = "DbRep_" + $repType

$dbAllNames = $dbList -split ","

foreach($dbName in $dbAllNames)
{	
	nuget.exe pack .\flyway\$dbName.nuspec -Version 0.0.$buildNumber -OutputDirectory "..\packages"
}

exit $LastExitCode

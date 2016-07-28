Param(
	[Parameter(Mandatory=$true)][string]$buildNumber,
	[Parameter(Mandatory=$true)][string]$dbList,
	[Parameter(Mandatory=$true)][string]$repType)
	
$dbAllNames = $dbList -split ","

foreach($dbName in $dbAllNames)
{	
	$packExtension = ".nupkg"	
	$packPath = "..\packages\" + $dbName + "_" + $repType
	$packVersion = ".0.0." + $buildNumber

	$packFullPath = $packPath + $packVersion + $packExtension 

	Write-Output $packFullPath

	nuget push $packFullPath -Source https://proget.celfinet.com:444/nuget/cfn.database -ApiKey 60402edb-a9f0-4dab-8fd6-5f8640e9ef87
	nuget push $packFullPath -Source http://octopus.cfncloud.com:8081/repository/cfn.nuget.database -ApiKey 3df8f962-3cf7-3727-893a-159ab5a2a24d 
}

exit $LastExitCode
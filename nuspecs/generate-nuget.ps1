$nugetOutputDirectory = '../build/nuget'

$releaseNotesUri = ''
$coreFileVersion = '1.0.0'
$nugetFileName = 'nuget.exe'

Write-Host "Packing $env:solution_name"

function Get-FileVersion
{
    [OutputType([string])]
    Param ([string]$assemblyPath)

    if(!$assemblyPath)
    {
        return ""
    }

    if((Test-Path $assemblyPath))
    {
        $fileInfo = Get-Item $assemblyPath
        return $fileInfo.VersionInfo.ProductVersion
    }

    throw "Could not locate the assembly '$assemblyPath'"
}

function ConvertTo-NuGetExpression
{
    [OutputType([string])]
    Param (
        [string]$nuspecPath,
        [string]$debugVersion
    )

    $fileVersion = $debugVersion

    # if(!$fileVersion)
    # {
        # $fileVersion = $uwpVersion
    # }
	#-Prop 'releaseNotes=$($releaseNotesUri)$fileVersion'
    $expression = ".\$($nugetFileName) pack $($nuspecPath) -OutputDirectory $nugetOutputDirectory -Prop 'version=$($fileVersion)' -Prop 'coreVersion=$($fileVersion)'"

    if($wpfVersion)
    {
        $expression = "$expression -Prop 'debugVersion=$($debugVersion)'"
    }

    Write-Host "Finished Expression: $expression"
    return $expression
}

function Save-NuGetPackage ($project) 
{
    $nuspecPath = $project.NuSpec
    $debugAssemblyPath = $project.Files.debug    

    Write-Host "NuSpec: $nuspecPath"
    Write-Host "Debug Assembly: $debugAssemblyPath"

    $debugVersion = Get-FileVersion -assemblyPath $debugAssemblyPath

    Write-Host "Debug Version: $debugVersion"

    if($debugVersion -eq '')
    {
        Write-Host "Something seems to be wrong, we couldn't locate any binaries for $($project.Name)"
        return
    }

    Invoke-Expression "$(ConvertTo-NuGetExpression -nuspecPath $nuspecPath -debugVersion $debugVersion)"
}

if(Test-Path ./nuspecs)
{
    $returnPath = "../"
    Set-Location ./nuspecs
}

if (!(Test-Path $nugetOutputDirectory))
{
	Write-Host 'Creating build directory ...'
	$res = New-Item $nugetOutputDirectory -ItemType Directory
}

if (!(Test-Path $nugetFileName))
{
    Write-Host 'Downloading Nuget.exe ...'
    Invoke-WebRequest -Uri "http://nuget.org/nuget.exe" -OutFile $nugetFileName
}

$projectsJson = Get-Content -Raw -Path projects.json | ConvertFrom-Json
$coreFileVersion = Get-FileVersion -assemblyPath $projectsJson.Core

if($IsWindows -or $PSEdition -eq "Desktop")
{
    foreach ($project in $projectsJson.Projects) 
    {
        Write-Host "Building package for $($project.Name)"
        Save-NuGetPackage -project $project
    }
}
else
{
    Write-Host "This script must be executed on Windows"
}

if($returnPath)
{
    Set-Location $returnPath
}
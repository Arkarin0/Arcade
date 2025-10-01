Set-StrictMode -version 2.0
$ErrorActionPreference = "Stop"
$solution = "Akrarin0_Arcade.sln"
$officialSourceBranchName = "dev/ManuelTest"
$officialBuildId = "1234.5"
# $configuration = "Debug"

. (Join-Path $PSScriptRoot "..\..\eng\build-utils.ps1")

$ReportTool = Join-Path $RepoRoot ".\.tools\reportgenerator.exe"

function Remove-Directory([string]$path)
{
    if ((Test-Path $path)) {
        Remove-Item -path $path -force -recurse| Out-Null
      }
}

function Remove-Reports()
{
    $srcDir = Join-Path $RepoRoot "src"
    get-childitem $srcDir -Include "TestResults" -recurse -force | Remove-Item -recurse -force
}

function Ensure-ReportGenerator()
{
    if(Test-Path $ReportTool){return}

    Write-Host "Installing Reportgenerator"
    dotnet tool install dotnet-reportgenerator-globaltool --toolpath "$(Join-Path $RepoRoot ".tools")"
}

function TestWorkAround()
{
    # this is needed because we use Microsoft.Net.Test.SDK older than 17.5.0.
    # in this case no reports are generated when building in release mode.

    $projectFilePath = $solution

  

  # Tests need to locate .NET Core SDK
  $dotnet = InitializeDotNetCli

  # original, consider to implement the MS shema
  # $runTests = GetProjectOutputBinary "RunTests.dll" -tfm "netcoreapp3.1"
  #
  # if (!(Test-Path $runTests)) {
  #   Write-Host "Test runner not found: '$runTests'. Run Build.cmd first." -ForegroundColor Red 
  #   ExitWithExitCode 1
  # }
  #
  # for now just return the root folder containing the solution.
  $runTests = "test $($solution)"
  
  
  $dotnetExe = Join-Path $dotnet "dotnet.exe"
  #$args += " --dotnet `"$dotnetExe`""
  #$args += " --logs `"$LogDir`""
  $args += " --configuration $configuration"
  $args += " --no-build" # skip build, because netCore can't handle ComReferences.
  #$args += " --collect:`"Xplat Code Coverage`""
  $args += " --settings:`"$(Join-Path $PSScriptRoot "coverage.runsettings")`""

  try {
    Write-Host "$runTests $args"
    #we need to build the Components solution via MSBuild because we use COM-Refernces.
    Run-MSBuild `"$projectFilePath`" -configuration $configuration -warnAsError:$false -treatWarningAsError:$false -buildArgs "/t:rebuild"
    
    #call actual "Run Test" performed by netCore.
    Exec-Console $dotnetExe "$runTests $args"
  } finally {
    Get-Process "xunit*" -ErrorAction SilentlyContinue | Stop-Process
  }

  Write-Host "Tests finished" -ForegroundColor "Green"
}

function BuildCodeCoverateReport($reporttype="Html") {
    $tool= $ReportTool
    $targetDir = Join-Path $RepoRoot "artifacts\reports\codecoverrate\$reporttype"
    Remove-Directory  $targetDir
  
    $args+= " -reports:`"src\**\TestResults\**\coverage.cobertura.xml`""
    $args+= " -targetdir:`"$($targetDir)`""
    $args+= " -reporttypes:$reporttype"
    $args+= " -Title:`"$($solution) ($($configuration))`""
  
    Exec-Console $tool $args
  }



try {
    Push-Location $RepoRoot

    Ensure-DotnetSdk
    Ensure-ReportGenerator

    Remove-Reports
    TestWorkAround
    BuildCodeCoverateReport "Html"
    BuildCodeCoverateReport "MarkdownSummary"
    BuildCodeCoverateReport "HtmlInline_AzurePipelines"

    ExitWithExitCode 0
}
catch {
  Write-Host $_
  Write-Host $_.Exception
  Write-Host $_.ScriptStackTrace
  ExitWithExitCode 1
}
finally {
  Pop-Location
}

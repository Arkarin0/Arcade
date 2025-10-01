# Arcade SDK

Arcade SDK is a set of msbuild props and targets files and packages that provide common build features used across multiple repos, such as CI integration, packaging, VSIX and VS setup authoring, testing, and signing.

The goals are

- to reduce the number of copies of the same or similar props, targets and script files across repos
- enable cross-platform build that relies on a standalone dotnet cli (downloaded during restore) as well as desktop msbuild based build
- be as close to the latest shipping .NET Core SDK as possible, with minimal overrides and tweaks
- be modular and flexible, not all repos need all features; let the repo choose subset of features to import
- unify common operations and structure across repos
- unify Azure DevOps Build Pipelines used to produce official builds

The toolset has four kinds of features and helpers:

- Common conventions applicable to all repos using the toolset.
- Infrastructure required for Azure DevOps CI builds, MicroBuild and build from source.
- Workarounds for bugs in shipping tools (dotnet SDK, VS SDK, msbuild, VS, NuGet client, etc.).
  Will be removed once the bugs are fixed in the product and the toolset moves to the new version of the tool.
- Abstraction of peculiarities of VSSDK and VS insertion process that are not compatible with dotnet SDK.

The toolset has following requirements on the repository layout.

## Single build output

All build outputs are located under a single directory called `artifacts`. 
The Arcade SDK defines the following output structure:

```text
artifacts
  bin
    $(MSBuildProjectName)
      $(Configuration)
  packages
    $(Configuration)
      Shipping
        $(MSBuildProjectName).$(PackageVersion).nupkg
      NonShipping
        $(MSBuildProjectName).$(PackageVersion).nupkg
      Release
      PreRelease
  TestResults
    $(Configuration)
      $(MSBuildProjectName)_$(TargetFramework)_$(TestArchitecture).(xml|html|log|error.log)
  VSSetup
    $(Configuration)
      Insertion
        $(VsixPackageId).json
        $(VsixPackageId).vsmand
        $(VsixContainerName).vsix
        $(VisualStudioInsertionComponent).vsman
      DevDivPackages
        $(MSBuildProjectName).$(PackageVersion).nupkg
      $(VsixPackageId).json
      $(VsixContainerName).vsix
  VSSetup.obj
    $(Configuration)
      $(VisualStudioInsertionComponent)
  SymStore
    $(Configuration)
      $(MSBuildProjectName)
  log
    $(Configuration)
      Build.binlog
  tmp
    $(Configuration)
  obj
    $(MSBuildProjectName)
      $(Configuration)
  toolset
```

Having a common output directory structure makes it possible to unify MicroBuild definitions.

| directory         | description |
|-------------------|-------------|
| bin               | Build output of each project. |
| obj               | Intermediate directory for each project. |
| packages          | NuGet packages produced by all projects in the repo. |
| VSSetup           | Packages produced by VSIX projects in the repo. These packages are experimental and can be used for dogfooding.
| VSSetup/Insertion | Willow manifests and VSIXes to be inserted into VS.
| VSSetup.obj       | Temp files produced by VSIX build. |
| SymStore          | Storage for converted Windows PDBs |
| log               | Build binary log and other logs. |
| tmp               | Temp files generated during build. |
| toolset           | Files generated during toolset restore. |

## Build scripts and extensibility points

### /eng/common/*

The Arcade SDK requires bootstrapper scripts to be present in the repo.

The scripts in this directory shall be present and the same across all repositories using Arcade SDK.

### /eng/Tests.targets (optional)

Customization of testing process.

#### Example: add steps for tests

This is required to provide custom logic to the `Test` Task.

```xml
<!-- eng/Tests.targets -->
<Project>
  <Target Name="MyBeforeTestTask" BeforeTargets="Test">
    <!-- add your logic -->
  </Target>

  <Target Name="MyAfterTestTask" AfterTargets="Test">
    <!-- add your logic -->
  </Target>
</Project>
```

#### Example: add custom Testrunner

Provide an other testrunner then already Provided.

1. add the following file `eng/Tests.targets`.

  ```xml
  <!-- eng/Tests.targets -->
  <Project>
  <!-- Import specialized targets files of supported test runners -->
    <Import Project="$(MSBuildThisFileDirectory)UnittestFrameworks\$(TestRunnerName)\$(TestRunnerName).targets" Condition="'$(IsTestProject)' == 'true' and '$(TestRunnerName)' != '' and Exists('$(MSBuildThisFileDirectory)UnittestFrameworks\$(TestRunnerName)\$(TestRunnerName).targets')"/>
  </Project>
  ```

2. create the following folder `eng/UnittestFrameworks/MyCustomTestRunner`
3. create the following file `eng/UnittestFrameworks/MyCustomTestRunner/MyCustomTestRunner.targets`
   
  ```xml
  <Project>
    <ItemGroup>
      <!-- Add package references and other stuff -->
    </ItemGroup>

    <Import Sdk="Arkarin0.DotNet.Arcade.Sdk" Project="tools\VSTest.targets"/>
  </Project>
  ```

### /global.json

`/global.json` file is present and specifies the version of the dotnet and `Arkarin0.DotNet.Arcade.Sdk` SDKs.

For example,

```json
{
  "tools": {
    "dotnet": "7.0.103"
  },
  "msbuild-sdks": {
    "Arkarin0.DotNet.Arcade.Sdk": "1.0.0"
  }
}
```

Include `vs` entry under `tools` if the repository should be built via `msbuild` from Visual Studio installation instead of dotnet cli:

```json
{
  "tools": {
    "vs": {
      "version": "17.3"
    }
  }
}
```

Optionally, a list of Visual Studio [workload component ids](https://docs.microsoft.com/en-us/visualstudio/install/workload-component-id-vs-enterprise) may be specified under `vs`:

```json
"vs": {
  "version": "17.3",
  "components": ["Microsoft.VisualStudio.Component.VSSDK"]
}
```

#### Example: Restoring multiple .NET Core Runtimes for running tests

In /global.json, specify a `runtimes` section and list the [shared runtime versions](https://dotnet.microsoft.com/download/dotnet-core) you want installed.

Schema:

```text
{
  "tools": {
    "dotnet": "<version>",                                           // define CLI SDK version
    "runtimes": {                                                    // optional runtimes section
      "<runtime>": [ "<version>", ..., "<version>" ],
      ...,
      "<runtime>/<architecture>": [ "<version>", ..., "<version>" ]
    }
  }
}
```

`<runtime>` - One of the supported "runtime" values for the [dotnet-install](https://github.com/dotnet/cli/blob/dddac220ba5b6994e297752bebd9acffa3e72342/scripts/obtain/dotnet-install.ps1#L43) script.

`<architecture>` - Optionally include `/<architecture>` when defining the runtime to specify an explicit architecture where "architecture" is one of the supported values for the [dotnet-install](https://github.com/dotnet/cli/blob/dddac220ba5b6994e297752bebd9acffa3e72342/scripts/obtain/dotnet-install.ps1#L32) script.  Defaults to "auto" if not specified.

```json
{
  "tools": {
    "dotnet": "3.0.100-preview3-010431",
    "runtimes": {
      "dotnet/x64": [ "2.1.7" ],
      "aspnetcore/x64": [ "3.0.0-build-20190219.1" ]
    }
  }
}
```

You may also use any of the properties defined in `eng/Versions.props` to define a version.

Example

```json
{
  "tools": {
    "dotnet": "3.0.100-preview3-010431",
    "runtimes": {
      "dotnet/x64": [ "2.1.7", "$(MicrosoftNetCoreAppVersion)" ]
    }
  }
}
```

Note: defining `runtimes` in your global.json will signal to Arcade to install a local version of the SDK for the runtimes to use rather than depending on a matching global SDK.

### /NuGet.config

`/NuGet.config` file is present and specifies the MyGet feed to retrieve Arcade SDK from and other feeds required by the repository like so:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <clear />
    <!-- Feed to use to restore the Arcade SDK from -->  
    <!-- Feeds to use to restore dependent packages from -->  
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
  </packageSources>
  <disabledPackageSources>
    <clear />
  </disabledPackageSources>
</configuration>
```

### /Directory.Build.props

`Directory.Build.props` shall import Arcade SDK as shown below. The `Sdk.props` file sets various properties and item groups to default values. It is recommended to perform any customizations _after_ importing the SDK.

It is a common practice to specify properties applicable to all (most) projects in the repository in `Directory.Build.props`, e.g. public keys for `InternalsVisibleTo` project items.

```xml
<Project>
  <Import Project="Sdk.props" Sdk="Arkarin0.DotNet.Arcade.Sdk" />    
  <PropertyGroup> 
    <!-- Public keys used by InternalsVisibleTo project items -->
    <MoqPublicKey>00240000048000009400...</MoqPublicKey> 

    <!-- 
      Specify license used for packages produced by the repository.
      Use PackageLicenseExpressionInternal for closed-source licenses.
    -->
    <PackageLicenseExpression>MIT</PackageLicenseExpression>
    
    <!--
      Specify an id of the key used to generate strong names of assemblies built from this repo.
    -->
    <StrongNameKeyId>Arkarin0</StrongNameKeyId>
  </PropertyGroup>
</Project>
```

### /Directory.Build.targets

`Directory.Build.targets` shall import Arcade SDK. It may specify additional targets applicable to all source projects.

```xml
<Project>
  <Import Project="Sdk.targets" Sdk="Arkarin0.DotNet.Arcade.Sdk" />
</Project>
```

### Source Projects

Projects are located under `src` directory under root repo, in any subdirectory structure appropriate for the repo. 

Projects shall use `Microsoft.NET.Sdk` SDK like so:

```xml
<Project Sdk="Microsoft.NET.Sdk">
    ...
</Project>
```

### Project name conventions

- Unit test project file names shall end with `.UnitTests` or `.Tests`, e.g. `MyProject.UnitTests.csproj` or `MyProject.Tests.csproj`. 
- Integration test project file names shall end with `.IntegrationTests`, e.g. `MyProject.IntegrationTests.vbproj`.
- Performance test project file names shall end with `.PerformanceTests`, e.g. `MyProject.PerformanceTests.csproj`.
- If `source.extension.vsixmanifest` is present next to the project file the project is by default considered to be a VSIX producing project.

## Other Projects

It might be useful to create other top-level directories containing projects that are not standard C#/VB/F# projects. For example, projects that aggregate outputs of multiple projects into a single NuGet package or Willow component. These projects should also be included in the main solution so that the build driver includes them in build process, but their `Directory.Build.*` may be different from source projects. Hence the different root directory.

## Project Properties Defined by the SDK

### `IsShipping`, `IsShippingAssembly`, `IsShippingPackage`, `IsShippingVsix` (bool)

`IsShipping-` properties are project properties that determine which (if any) assets produced by the project are _shipping_. An asset is considered _shipping_ if it is intended to be delivered to customers via an official channel. This channel can be NuGet.org, an official installer, etc. Setting this flag to `true` does not guarantee that the asset will actually ship in the next release of the product. It might be decided after the build is complete that although the artifact is ready for shipping it won't be shipped this release cycle.

By default all assets produced by a project are considered _shipping_. Set `IsShipping` to `false` if none of the assets produced by the project are _shipping_. Test projects (`IsTestProject` is `true`) set `IsShipping` to `false` automatically.

Setting `IsShipping` property is sufficient for most projects. Projects that produce both _shipping_ and _non-shipping_ assets need a finer grained control. Set `IsShippingAssembly`, `IsShippingPackage` or `IsShippingVsix` to `false` if the assembly, package, or VSIX produced by the project is not _shipping_, respectively. 

Build targets shall not directly use `IsShipping`. Instead they shall use `IsShippingAssembly`, `IsShippingPackage` and `IsShippingVsix` depending on the asset they are dealing with.

Examples of usage:

- Set `IsShipping` property to `false` in test/build/automation utility projects.

- Set `IsShipping` property to `false` in projects that produce VSIX packages that are only used only within the repository (e.g. to facilitate integration tests or VS F5) and not expected to be installed by customers.

- Set `IsShippingPackage` property to `false` in projects that package  _shipping_ assemblies in packages that facilitate transport of assets from one repository to another one, which extracts the assemblies and _ships_ them in a  _shipping_ container.


### `SkipTests` (bool)

Set to `true` in a test project to skip running tests.

### `TestArchitectures` (list of strings)

List of test architectures (`x64`, `x86`) to run tests on.
If not specified by the project defaults to the value of `PlatformTarget` property, or `x64` if `Platform` is `AnyCPU` or unspecified.

For example, a project that targets `AnyCPU` can opt-into running tests using both 32-bit and 64-bit test runners on .NET Framework by setting `TestArchitectures` to `x64;x86`.

### `TestTargetFrameworks` (list of strings)

By default, the test runner will run tests for all frameworks a test project targets. Use `TestTargetFrameworks` to reduce the set of frameworks to run against.

For example, consider a project that has `<TargetFrameworks>netcoreapp3.1;net472</TargetFrameworks>`. To only run .NET Core tests run 

```text
msbuild Project.UnitTests.csproj /t:Test /p:TestTargetFrameworks=netcoreapp3.1
```

To specify multiple target frameworks on command line quote the property value like so:

```text
msbuild Project.UnitTests.csproj /t:Test /p:TestTargetFrameworks="netcoreapp3.1;net472"
```

### `TestRuntime` (string)

Runtime to use for running tests. Currently supported values are: `Core` (.NET Core), `Full` (.NET Framework) and `Mono` (Mono runtime).

For example, the following runs .NET Framework tests using Mono runtime:

```text
msbuild Project.UnitTests.csproj /t:Test /p:TestTargetFrameworks=net472 /p:TestRuntime=Mono
```

### `TestRunnerAdditionalArguments` (string)

Additional command line arguments passed to the test runner (e.g. `xunit.console.exe`).

### `TestRuntimeAdditionalArguments` (string)

Additional command line arguments passed to the test runtime (i.e. `dotnet` or `mono`). Applicable only when `TestRuntime` is `Core` or `Mono`. 

For example, to invoke Mono with debug flags `--debug` (to get stack traces with line number information), set `TestRuntimeAdditionalArguments` to `--debug`.
To override the default Shared Framework version that is selected based on the test project TFM, set `TestRuntimeAdditionalArguments` to `--fx-version x.y.z`.

### `TestTimeout` (int)

Timeout to apply to an individual invocation of the test runner (e.g. `xunit.console.exe`) for a single configuration. Integer number of milliseconds.

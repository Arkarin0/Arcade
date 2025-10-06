# Arkarin0.DotNet.Arcade.SDK

This package is based on [Micosoft.Arcade](https://github.com/dotnet/arcade).

It is a collection of common, dotNet framework specific, project properties and items for msbuild and SDK Style Projects.

## How to install it

1) Add `global.json`

   ```json
    {
        "tools": {
            "dotnet": "8.0.204"
        },
        "msbuild-sdks": {
            "Arkarin0.DotNet.Arcade.Sdk": "1.0.0-Preview"
        }
    }
   ```

2) create nuget.config file with a source for the `Arkarin0.DotNet.Arcade.Sdk` nuget package.
3) Add lines in Directory.Build.props

    ```xml
    <?xml version="1.0" encoding="utf-8"?>
    <Project>
    <Import Project="Sdk.props" Sdk="Arkarin0.DotNet.Arcade.Sdk" />
    ...
    <\Project>
    ```

4) Add lines in Directory.Build.targets

    ```xml
    <?xml version="1.0" encoding="utf-8"?>
    <Project>
    <Import Project="Sdk.targets" Sdk="Arkarin0.DotNet.Arcade.Sdk" />
    ...
    <\Project>
    ```

5) Copy eng\common from [`Arkarin0.Arcade`](https://github.com/Arkarin0/Arcade) into repo.
6) Add the Versions.props file to your eng\ folder.
7) optionally copy the scripts for restore, build and test to repository root folder.

## How to use it

Use Arcarin0.Acrode with the shell command or Visual Studio

```cmd
build
test
```

# Onboarding onto Arcade

- Onboard onto the Arcade SDK, which provides templates (building blocks) for
  interacting with Azure DevOps, as well as shared tooling for signing,
  packaging, publishing and general build infrastructure.  
  
  Resources: [Reference documentation](ArcadeSdk.md)

   Steps:
    1. Add a
       [global.json](https://github.com/Arkarin0/Arcade/blob/master/global.json).
    2. Add (or copy)
       [Directory.Build.props](https://github.com/Arkarin0/Arcade/blob/master/Directory.Build.props)
       and
       [Directory.build.targets](https://github.com/Arkarin0/Arcade/blob/master/Directory.build.targets).
    3. Copy `eng\common` from
       [Arcade](https://github.com/Arkarin0/Arcade/blob/master)
       into repo.
    4. Add (or copy) the
       [Versions.props](https://github.com/Arkarin0/Arcade/blob/master/Versions.props)
       file to your eng\ folder. Adjust the version prefix and prerelease label
       as necessary.
    5. Add the following feeds to your nuget.config:
       - https://api.nuget.org/v3/index.json
       
       along with any other feeds your repo needs to restore packages from. You can see which feeds Arcade uses at: [NuGet.config](https://github.com/Arkarin0/Arcade/blob/master/global.json).

    **Using Arcade packages** - See [documentation](CorePackages/) for
    information on specific packages.

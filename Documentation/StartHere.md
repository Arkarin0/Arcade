# Jumping into Arcade Infrastructure

The purpose of this document is to provide a jumping off point for repository
owners looking to start upgrading their infrastructure to Arcade. It will
provide a general overview of the direction Arcade infrastructure is going
and a general outline of steps you should take to get started with the upgrade,
with links to appropriate detailed documentation.

## What are we doing?

With every new major product cycle, we take the chance to upgrade our
infrastructure based on what we learned in the previous product cycle. The goal
is to produce a better product more efficiently in the next product cycle. .NET
is no different, though in many ways the infrastructure changes we are
making are much more an overhaul than normal. Generally, we are focusing in 3
areas:
- **Shared tooling (Arcade)** - Striving to reduce duplication of tooling,
  improve development consistency between repos and drive tooling improvements
  across a wider swath of the ecosystem more quickly.
- **Transitioning to Azure DevOps for public CI, and upgrading official builds**
  - Move away from Jenkins, improve CI reliability, increase the consistency
  between our official and PR builds, and bring first-class workflow for
  internal as well as public changes.

## I'm ready to get started, what do I do?

See the [Arcade Onboarding](Onboarding.md) guide.

## Where can I find general information on Arcade infrastructure?

There is quite a bit of documentation living under the
[Documentation](../Documentation/) folder in the dotnet/arcade repo.  Here are
some highlights

### Concepts and Goals

- [Arcade overview](Overview.md)
<!-- - [Arcade communications](./Policy/ArcadeCommunications.md) -->
<!-- - [How dependency flow works in .NET](BranchesChannelsAndSubscriptions.md) -->
<!-- - [Guidance for defaults](./Policy/DefaultsGuidance.md) -->
<!-- - [Versioning rules](CorePackages/Versioning.md) -->
<!-- - [How to Create an Arcade Package](HowToCreatePackages.md) -->
<!-- - [.NET Core Infrastructure Ecosystem Overview](InfrastructureEcosystemOverview.md) -->
<!-- - [Toolset Publish/Consume Contract](PublishConsumeContract.md) -->
<!-- - [Toolsets](Toolsets.md) -->
<!-- - [Version Querying and Updating](VersionQueryingAndUpdating.md) -->
<!-- - [How Arcade tests itself](Validation/Overview.md) -->

### Tools we are using and how we are using them

#### Code and repository configuration
  - [The Arcade Build SDK](ArcadeSdk.md)
  <!-- - GitHub and Azure Repos
    - [Mirroring public projects](AzureDevOps/internal-mirror.md)
    - [Git Sync Tools](GitSyncTools.md)
    - Bots and connectors
  - [Dependency Description Format](DependencyDescriptionFormat.md)
  - [How to See What's the Latest Version of an Arcade Package](SeePackagesLatestVersion.md) -->

<!-- #### Building projects
  - [Telemetry](CorePackages/Telemetry.md)
  - [MSBuild Task Packages](TaskPackages.md)
  - Azure Pipelines: Orchestrating continuous integration
    - [Goals](AzureDevOps/WritingBuildDefinitions.md)
    - [Onboarding to Azure DevOps](AzureDevOps/AzureDevOpsOnboarding.md)
    - [Choosing a Machine Pool](ChoosingAMachinePool.md)
    - [Migrating from `phase` to `job`](AzureDevOps/PhaseToJobSchemaChange.md) in Pipeline build definitions
    - Tasks and Templates
  - Mission Control -->

<!-- #### Testing projects
  - Helix: [Introduction](/Documentation/Helix.md), [SDK](../src/Microsoft.DotNet.Helix/Sdk/Readme.md), [JobSender](../src/Microsoft.DotNet.Helix/JobSender/Readme.md)
  - Azure Agent pools and queues
  - Docker support
  - [Dump file retrieval](Dumps/Dumps.md) -->

<!-- #### Deploying projects
  - [Packaging](CorePackages/Packaging.md)
  - [Publishing](CorePackages/Publishing.md)
  - [SignTool](CorePackages/Signing.md) (and Microbuild)
  - BAR -->

## I need help, who should I talk to?

Contact 'Arkarin0' for additional guidance via GitHub

<!-- ## Frequently Asked Questions

See the [FAQ](FAQ.md). -->

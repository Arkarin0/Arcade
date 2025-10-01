// Created/modified by Arkarin0 under one more more license(s).
using Arkarin0.Arcade.Common.Desktop;

namespace Arkarin0.Arcade.Common
{
    public partial class MSBuildTaskBase
    {
        static MSBuildTaskBase()
        {
            AssemblyResolver.Enable();
        }
    }
}

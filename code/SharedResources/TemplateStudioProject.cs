// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Templates.SharedResources
{
    public static class TemplateStudioProject
    {
#if _WPF_
        public static string AppName => Resources.TemplateStudioForAvalonia;
        public const string AssemblyName = "TemplateStudioForAvalonia";
#else
#error Invalid configuration selected. You cannot use "Debug" or "Release", choose something else.
        // If not using an appropriate build configuration,
        // Building will fail because the above properties aren't defined.
#endif
    }
}

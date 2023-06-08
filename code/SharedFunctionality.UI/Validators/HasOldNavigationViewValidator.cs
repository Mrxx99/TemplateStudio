// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using Microsoft.Templates.Core.Validation;

namespace Microsoft.Templates.UI.Validators
{
    public class HasOldNavigationViewValidator : IBreakingChangeValidator
    {
        public Version BreakingVersion { get; }

        public HasOldNavigationViewValidator()
        {
            // This is last version with old NavigationView control in templates
            var version = Core.Configuration.Current.BreakingChangesVersions.FirstOrDefault(c => c.Name == "HasOldNavigationView")?.BreakingVersion;
            BreakingVersion = version ?? new Version();
        }

        public ValidationResult Validate()
        {
            return new ValidationResult();
        }
    }
}

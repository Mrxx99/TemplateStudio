// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Linq;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Helpers;
using Microsoft.Templates.Core.Services;
using Microsoft.Templates.Core.Validation;
using Microsoft.Templates.SharedResources;

namespace Microsoft.Templates.UI.Validators
{
    public class HasPivotValidator : IBreakingChangeValidator
    {
        public Version BreakingVersion { get; }

        public HasPivotValidator()
        {
            // This is last version with Hamburguer menu control in templates
            var version = Core.Configuration.Current.BreakingChangesVersions.FirstOrDefault(c => c.Name == "HasPivot")?.BreakingVersion;
            BreakingVersion = version ?? new Version();
        }

        public ValidationResult Validate()
        {
            return new ValidationResult();
        }
    }
}

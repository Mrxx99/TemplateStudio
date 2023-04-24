// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Linq;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen.Shell;
using Microsoft.Templates.Core.Helpers;
using Microsoft.Templates.Core.Services;
using Microsoft.Templates.SharedResources;

namespace Microsoft.Templates.UI.Services
{
    public class ProjectConfigInfoService
    {
        // Framework
        public const string CodeBehind = "CodeBehind";
        public const string None = "None";
        public const string Prism = "Prism";
        public const string MvvmToolkit = "MVVMToolkit";

        // ProjectType
        private const string Blank = "Blank";
        private const string NavView = "NavView";
        private const string SplitView = "SplitView";
        private const string TabbedNav = "TabbedNav";
        private const string MenuBar = "MenuBar";
        private const string Ribbon = "Ribbon";

        private readonly IGenShell _shell;

        public ProjectConfigInfoService(IGenShell shell)
        {
            _shell = shell;
        }

        public ProjectMetadata ReadProjectConfiguration()
        {
            var projectPath = _shell.Project.GetActiveProjectPath();
            var projectMetadata = ProjectMetadataService.GetProjectMetadata(projectPath);

            if (IsValid(projectMetadata))
            {
                return projectMetadata;
            }

            var inferredConfig = InferProjectConfiguration(projectMetadata);

            if (IsValid(inferredConfig))
            {
                ProjectMetadataService.SaveProjectMetadata(inferredConfig, projectPath);
            }

            return inferredConfig;
        }

        private bool IsValid(ProjectMetadata data)
        {
            return !string.IsNullOrEmpty(data.ProjectType) &&
                   !string.IsNullOrEmpty(data.Framework) &&
                   !string.IsNullOrEmpty(data.Platform);
        }

        private ProjectMetadata InferProjectConfiguration(ProjectMetadata data)
        {
            if (string.IsNullOrEmpty(data.Platform))
            {
                data.Platform = InferPlatform();
            }

            if (string.IsNullOrEmpty(data.ProjectType))
            {
                data.ProjectType = InferProjectType(data.Platform);
            }

            if (string.IsNullOrEmpty(data.Framework))
            {
                data.Framework = InferFramework(data.Platform);
            }

            return data;
        }

        public string InferAppModel()
        {
            if (IsCSharpProject())
            {
                return ContainsSDK("Microsoft.NET.Sdk") ? AppModels.Desktop : AppModels.Uwp;
            }

            return ContainsProperty("AppContainerApplication", "false") ? AppModels.Desktop : AppModels.Uwp;
        }

        public string InferPlatform()
        {
            return Platforms.Avalonia;
        }

        private bool IsUwp()
        {
            var projectTypeGuids = _shell.Project.GetActiveProjectTypeGuids();

            if (projectTypeGuids != null && projectTypeGuids.ToUpperInvariant().Split(';').Contains("{A5A43C5B-DE2A-4C0C-9213-0A381AF9435A}"))
            {
                return true;
            }

            return false;
        }


        private string InferProjectType(string platform)
        {
            if (IsMenuBar(platform))
            {
                return MenuBar;
            }
            else if (IsSplitView(platform))
            {
                return SplitView;
            }
            else if (IsBlank(platform))
            {
                return Blank;
            }
            else if (IsRibbon(platform))
            {
                return Ribbon;
            }

            return string.Empty;
        }

        private string InferFramework(string platform)
        {
            if (IsCodeBehind(platform))
            {
                return CodeBehind;
            }
            else if (IsPrism())
            {
                return Prism;
            }
            else if (IsMvvmToolkit(platform))
            {
                return MvvmToolkit;
            }

            return string.Empty;
        }

        private bool IsBlank(string platform)
        {
            return ExistsFileInProjectPath("ShellWindow.xaml", "Views")
                && !FileContainsLine("Views", "ShellWindow.xaml", "<Menu Grid.Row=\"0\" Focusable=\"False\">")
                && !FileContainsLine("Views", "ShellWindow.xaml", "<controls:HamburgerMenu")
                && !FileContainsLine("Views", "ShellWindow.xaml", "<Fluent:Ribbon x:Name=\"ribbonControl\" Grid.Row=\"0\">");
        }


        private bool IsSplitView(string platform)
        {
            return ExistsFileInProjectPath("ShellWindow.xaml", "Views")
                && FileContainsLine("Views", "ShellWindow.xaml", "<controls:HamburgerMenu");
        }

        private bool IsMenuBar(string platform)
        {
            return ExistsFileInProjectPath("ShellWindow.xaml", "Views")
                && FileContainsLine("Views", "ShellWindow.xaml", "<Menu Grid.Row=\"0\" Focusable=\"False\">");
        }

        private bool IsRibbon(string platform)
        {
            return ExistsFileInProjectPath("ShellWindow.xaml", "Views")
                    && FileContainsLine("Views", "ShellWindow.xaml", "<Fluent:Ribbon x:Name=\"ribbonControl\" Grid.Row=\"0\">");
        }

        private bool IsCodeBehind(string platform)
        {
            if (ExistsFileInProjectPath("ApplicationHostService.cs", "Services"))
            {
                var codebehindFile = Directory.GetFiles(Path.Combine(_shell.Project.GetActiveProjectPath(), "Views"), "*.xaml.cs", SearchOption.TopDirectoryOnly).FirstOrDefault();
                if (!string.IsNullOrEmpty(codebehindFile))
                {
                    var fileContent = File.ReadAllText(codebehindFile);
                    return fileContent.Contains("INotifyPropertyChanged")
                        && fileContent.Contains("public event PropertyChangedEventHandler PropertyChanged;");
                }
            }
            return false;
        }

        private bool IsPrism()
        {
            return ContainsNugetPackage("Prism.Unity");
        }

        private bool IsMvvmToolkit(string platform)
        {
              return ContainsNugetPackage("CommunityToolkit.Mvvm");
        }

        private bool IsCSharpProject()
        {
            return Directory.GetFiles(_shell.Project.GetActiveProjectPath(), "*.csproj", SearchOption.TopDirectoryOnly).Any();
        }

        private bool IsVisualBasicProject()
        {
            return Directory.GetFiles(_shell.Project.GetActiveProjectPath(), "*.vbproj", SearchOption.TopDirectoryOnly).Any();
        }

        private bool IsCppProject()
        {
            return Directory.GetFiles(_shell.Project.GetActiveProjectPath(), "*.vcxproj", SearchOption.TopDirectoryOnly).Any();
        }

        public string GetProgrammingLanguage()
        {
            if (IsCSharpProject())
            {
                return ProgrammingLanguages.CSharp;
            }
            else if (IsCppProject())
            {
                return ProgrammingLanguages.Cpp;
            }
            else if (IsVisualBasicProject())
            {
                return ProgrammingLanguages.VisualBasic;
            }

            return string.Empty;
        }

        private bool ExistsFileInProjectPath(string fileName, string subPath = null)
        {
            try
            {
                var path = _shell.Project.GetActiveProjectPath();
                if (!string.IsNullOrEmpty(subPath))
                {
                    path = Path.Combine(path, subPath);
                }

                return Directory.Exists(path) && Directory.GetFiles(path, fileName, SearchOption.TopDirectoryOnly).Length > 0;
            }
            catch (DirectoryNotFoundException)
            {
                return false;
            }
            catch (FileNotFoundException)
            {
                return false;
            }
        }

        private bool FileContainsLine(string subPath, string fileName, string lineToFind)
        {
            try
            {
                var filePath = Path.Combine(_shell.Project.GetActiveProjectPath(), subPath, fileName);
                var fileContent = FileHelper.GetFileContent(filePath);
                return fileContent != null && fileContent.Contains(lineToFind);
            }
            catch (DirectoryNotFoundException)
            {
                return false;
            }
            catch (FileNotFoundException)
            {
                return false;
            }
        }

        private bool ContainsNugetPackage(string packageId)
        {
            var projfiles = Directory.GetFiles(_shell.Project.GetActiveProjectPath(), "*.*proj", SearchOption.TopDirectoryOnly);
            foreach (string file in projfiles)
            {
                if (File.ReadAllText(file).IndexOf($"<packagereference include=\"{packageId}", StringComparison.OrdinalIgnoreCase) != -1)
                {
                    return true;
                }
            }

            var configfiles = Directory.GetFiles(_shell.Project.GetActiveProjectPath(), "packages.config", SearchOption.TopDirectoryOnly);
            foreach (string file in configfiles)
            {
                if (File.ReadAllText(file).IndexOf($"<package id=\"{packageId}", StringComparison.OrdinalIgnoreCase) != -1)
                {
                    return true;
                }
            }

            return false;
        }

        private bool ContainsSDK(string sdkId)
        {
            var files = Directory.GetFiles(_shell.Project.GetActiveProjectPath(), "*.*proj", SearchOption.TopDirectoryOnly);
            foreach (string file in files)
            {
                if (File.ReadAllText(file).IndexOf($"sdk=\"{sdkId}\"", StringComparison.OrdinalIgnoreCase) != -1)
                {
                    return true;
                }
            }

            return false;
        }

        private bool ContainsProperty(string property, string value)
        {
            var files = Directory.GetFiles(_shell.Project.GetActiveProjectPath(), "*.*proj", SearchOption.TopDirectoryOnly);
            foreach (string file in files)
            {
                if (File.ReadAllText(file).Contains($"<{property}>{value}</{property}>"))
                {
                    return true;
                }
            }

            return false;
        }
    }
}

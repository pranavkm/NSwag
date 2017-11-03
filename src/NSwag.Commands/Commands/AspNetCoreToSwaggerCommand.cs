//-----------------------------------------------------------------------
// <copyright file="NSwagSettings.cs" company="NSwag">
//     Copyright (c) Rico Suter. All rights reserved.
// </copyright>
// <license>https://github.com/NSwag/NSwag/blob/master/LICENSE.md</license>
// <author>Rico Suter, mail@rsuter.com</author>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using NConsole;
using Newtonsoft.Json;
using NJsonSchema;
using NJsonSchema.Infrastructure;
using NSwag.Commands.SwaggerGeneration.AspNetCore;
using NSwag.SwaggerGeneration.AspNetCore;

namespace NSwag.Commands
{
    /// <summary>The generator.</summary>
    [Command(Name = "aspnetcore2swagger", Description = "Generates a Swagger specification ASP.NET Core Mvc application using ApiExplorer.")]
    public class AspNetCoreToSwaggerCommand : IConsoleCommand
    {
        private const string LauncherBinaryName = "NSwag.AspNetCore.Launcher";

        [JsonIgnore]
        public AspNetCoreToSwaggerGeneratorCommandSettings Settings { get; } = new AspNetCoreToSwaggerGeneratorCommandSettings();

        [JsonIgnore]
        [Argument(Name = "Project", IsRequired = false, Description = "The project to use.")]
        public string Project { get; set; }

        [Argument(Name = "Controllers", IsRequired = false, Description = "The MSBuild project extensions path. Defaults to \"obj\".")]
        public string MSBuildProjectExtensionsPath { get; set; }

        [Argument(Name = "Configuration", IsRequired = false, Description = "The configuration to use.")]
        public string Configuration { get; set; }

        [Argument(Name = "Runtime", IsRequired = false, Description = "The runtime to use.")]
        public string Runtime { get; set; }

        [Argument(Name = "TargetFramework", IsRequired = false, Description = "The target framework to use.")]
        public string TargetFramework { get; set; }

        [Argument(Name = "Verbose", IsRequired = false, Description = "Print verbose output.")]
        public bool Verbose { get; set; }

        [Argument(Name = "OutputPath", IsRequired = false, Description = "The output file.")]
        public string OutputPath
        {
            get => Settings.OutputPath;
            set => Settings.OutputPath = value;
        }

        [Argument(Name = "DefaultPropertyNameHandling", IsRequired = false, Description = "The default property name handling ('Default' or 'CamelCase').")]
        public PropertyNameHandling DefaultPropertyNameHandling
        {
            get => Settings.DefaultPropertyNameHandling;
            set => Settings.DefaultPropertyNameHandling = value;
        }

        [Argument(Name = "DefaultReferenceTypeNullHandling", IsRequired = false, Description = "The default null handling (if NotNullAttribute and CanBeNullAttribute are missing, default: Null, Null or NotNull).")]
        public ReferenceTypeNullHandling DefaultReferenceTypeNullHandling
        {
            get => Settings.DefaultReferenceTypeNullHandling;
            set => Settings.DefaultReferenceTypeNullHandling = value;
        }

        [Argument(Name = "DefaultEnumHandling", IsRequired = false, Description = "The default enum handling ('String' or 'Integer'), default: Integer.")]
        public EnumHandling DefaultEnumHandling
        {
            get => Settings.DefaultEnumHandling;
            set => Settings.DefaultEnumHandling = value;
        }

        [Argument(Name = "FlattenInheritanceHierarchy", IsRequired = false, Description = "Flatten the inheritance hierarchy instead of using allOf to describe inheritance (default: false).")]
        public bool FlattenInheritanceHierarchy
        {
            get => Settings.FlattenInheritanceHierarchy;
            set => Settings.FlattenInheritanceHierarchy = value;
        }

        [Argument(Name = "GenerateKnownTypes", IsRequired = false, Description = "Generate schemas for types in KnownTypeAttribute attributes (default: true).")]
        public bool GenerateKnownTypes
        {
            get => Settings.GenerateKnownTypes;
            set => Settings.GenerateKnownTypes = value;
        }

        [Argument(Name = "GenerateXmlObjects", IsRequired = false, Description = "Generate xmlObject representation for definitions (default: false).")]
        public bool GenerateXmlObjects
        {
            get => Settings.GenerateXmlObjects;
            set => Settings.GenerateXmlObjects = value;
        }

        [Argument(Name = "GenerateAbstractProperties", IsRequired = false, Description = "Generate abstract properties (i.e. interface and abstract properties. Properties may defined multiple times in a inheritance hierarchy, default: false).")]
        public bool GenerateAbstractProperties
        {
            get => Settings.GenerateAbstractProperties;
            set => Settings.GenerateAbstractProperties = value;
        }

        [Argument(Name = "ServiceHost", IsRequired = false, Description = "Overrides the service host of the web service (optional, use '.' to remove the hostname).")]
        public string ServiceHost { get; set; }

        [Argument(Name = "ServiceBasePath", IsRequired = false, Description = "The basePath of the Swagger specification (optional).")]
        public string ServiceBasePath { get; set; }

        [Argument(Name = "ServiceSchemes", IsRequired = false, Description = "Overrides the allowed schemes of the web service (optional, comma separated, 'http', 'https', 'ws', 'wss').")]
        public string[] ServiceSchemes { get; set; }

        [Argument(Name = "InfoTitle", IsRequired = false, Description = "Specify the title of the Swagger specification.")]
        public string InfoTitle
        {
            get => Settings.Title;
            set => Settings.Title = value;
        }

        [Argument(Name = "InfoDescription", IsRequired = false, Description = "Specify the description of the Swagger specification.")]
        public string InfoDescription
        {
            get => Settings.Description;
            set => Settings.Description = value;
        }

        [Argument(Name = "InfoVersion", IsRequired = false, Description = "Specify the version of the Swagger specification (default: 1.0.0).")]
        public string InfoVersion
        {
            get => Settings.Version;
            set => Settings.Version = value;
        }

        [Argument(Name = "DocumentTemplate", IsRequired = false, Description = "Specifies the Swagger document template (may be a path or JSON, default: none).")]
        public string DocumentTemplate { get; set; }

        [Argument(Name = "DocumentProcessors", IsRequired = false, Description = "Gets the document processor type names in the form 'assemblyName:fullTypeName' or 'fullTypeName').")]
        public string[] DocumentProcessorTypes
        {
            get => Settings.DocumentProcessorTypes;
            set => Settings.DocumentProcessorTypes = value;
        }

        [Argument(Name = "OperationProcessors", IsRequired = false, Description = "Gets the operation processor type names in the form 'assemblyName:fullTypeName' or 'fullTypeName').")]
        public string[] OperationProcessorTypes
        {
            get => Settings.OperationProcessorTypes;
            set => Settings.OperationProcessorTypes = value;
        }

        public async Task<object> RunAsync(CommandLineProcessor processor, IConsoleHost host)
        {
            if (!string.IsNullOrEmpty(DocumentTemplate))
            {
                if (await DynamicApis.FileExistsAsync(DocumentTemplate).ConfigureAwait(false))
                    Settings.DocumentTemplate = await DynamicApis.FileReadAllTextAsync(DocumentTemplate).ConfigureAwait(false);
                else
                    Settings.DocumentTemplate = DocumentTemplate;
            }
            else
                Settings.DocumentTemplate = null;

            var verboseHost = Verbose ? host : null;

            var projectFile = ProjectMetadata.FindProject(Project);
            var projectMetadata = await ProjectMetadata.GetProjectMetadata(
                projectFile, 
                MSBuildProjectExtensionsPath,
                TargetFramework,
                Configuration,
                Runtime,
                verboseHost).ConfigureAwait(false);

            Settings.ApplicationName = projectMetadata.AssemblyName;

            var targetDir = Path.GetFullPath(projectMetadata.ProjectDir);
            if (string.IsNullOrEmpty(OutputPath))
            {
                OutputPath = Path.Combine(targetDir, projectMetadata.OutputPath, "swagger.json");
            }
            else
            {
                OutputPath = Path.GetFullPath(OutputPath);
            }

            var cleanupFiles = new List<string>();

            var toolDirectory = Path.GetDirectoryName(typeof(AspNetCoreToSwaggerCommand).GetTypeInfo().Assembly.Location);
            var args = new List<string>();
            string executable;

            if (projectMetadata.TargetFrameworkIdentifier == ".NETFramework")
            {
                var binaryName = LauncherBinaryName + ".exe";
                var executableSource = Path.Combine(toolDirectory, binaryName);
                if (!File.Exists(executableSource))
                    throw new InvalidOperationException($"Unable to locate {binaryName} in {toolDirectory}.");

                executable = Path.Combine(projectMetadata.OutputPath, binaryName);
                File.Copy(executableSource, executable, overwrite: true);
                cleanupFiles.Add(executable);

                var appConfig = Path.Combine(projectMetadata.OutputPath, projectMetadata.TargetFileName + ".config");
                if (File.Exists(appConfig))
                {
                    var copiedAppConfig = Path.ChangeExtension(executable, ".exe.config");
                    File.Copy(appConfig, copiedAppConfig, overwrite: true);
                    cleanupFiles.Add(copiedAppConfig);
                }
            }
            else if (projectMetadata.TargetFrameworkIdentifier == ".NETCoreApp")
            {
                executable = "dotnet";
                args.Add("exec");
                args.Add("--depsfile");
                args.Add(projectMetadata.ProjectDepsFilePath);

                args.Add("--runtimeconfig");
                args.Add(projectMetadata.ProjectRuntimeConfigFilePath);

                var binaryName = LauncherBinaryName + ".dll";
                var executorBinary = Path.Combine(toolDirectory, binaryName);
                if (!File.Exists(executorBinary))
                {
                    throw new InvalidOperationException($"Unable to locate {binaryName} in {toolDirectory}.");
                }
                args.Add(executorBinary);
            }
            else
            {
                throw new InvalidOperationException($"Unsupported target framework '{projectMetadata.TargetFrameworkIdentifier}'.");
            }

            var settingsContentFile = Path.GetTempFileName();
            var settingsJson = JsonConvert.SerializeObject(Settings);
            File.WriteAllText(settingsContentFile, settingsJson);
            cleanupFiles.Add(settingsContentFile);
            
            args.Add(settingsContentFile);
            args.Add(toolDirectory);
            try
            {
                var exitCode = await Exe.RunAsync(executable, args, verboseHost).ConfigureAwait(false);
                if (exitCode == 0)
                {
                    host.WriteMessage($"Output written to {OutputPath}.");
                }

                return exitCode == 0;
            }
            finally
            {
                TryDeleteFile(cleanupFiles);
            }
        }

        private static void TryDeleteFile(List<string> files)
        {
            foreach (var file in files)
            {
                try
                {
                    if (File.Exists(file))
                        File.Delete(file);
                }
                catch
                {
                    // Don't throw any if any clean up operation fails.
                }
            }
        }
    }
}

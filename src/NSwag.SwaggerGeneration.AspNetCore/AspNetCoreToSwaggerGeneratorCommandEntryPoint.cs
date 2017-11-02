// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NSwag.SwaggerGeneration.Processors;

namespace NSwag.SwaggerGeneration.AspNetCore
{
    /// <summary>An entry point for the aspnetcore2swagger command.</summary>
    internal class AspNetCoreToSwaggerGeneratorCommandEntryPoint : AssemblyLoader.AssemblyLoader
    {
        public void Process(string settingsData)
        {
            var settings = CreateSettings(settingsData);
            var serviceProvider = GetServiceProvider(settings.ApplicationName);
            var apiDescriptionProvider = serviceProvider.GetRequiredService<IApiDescriptionGroupCollectionProvider>();

            var swaggerGenerator = new AspNetCoreToSwaggerGenerator(new AspNetCoreToSwaggerGeneratorSettings());
            var swaggerDocument = swaggerGenerator.GenerateAsync(apiDescriptionProvider.ApiDescriptionGroups).GetAwaiter().GetResult();

            var outputPathDirectory = Path.GetDirectoryName(settings.OutputPath);
            Directory.CreateDirectory(outputPathDirectory);
            File.WriteAllText(settings.OutputPath, swaggerDocument.ToJson());
        }

        private static IServiceProvider GetServiceProvider(string applicationName)
        {
            var assemblyName = new AssemblyName(applicationName);
            var assembly = Assembly.Load(assemblyName);

            if (assembly.EntryPoint == null)
            {
                throw new InvalidOperationException($"Unable to locate the program entry point for {assemblyName}.");
            }

            var entryPoint = assembly.EntryPoint?.DeclaringType;
            var buildWebHostMethod = entryPoint?.GetMethod("BuildWebHost");
            var args = new string[0];

            IWebHost webHost = null;
            if (buildWebHostMethod != null)
            {
                webHost = (IWebHost)buildWebHostMethod.Invoke(null, new object[] { args });
            }
            else
            {
                var createWebHostMethod = entryPoint?.GetMethod("CreateWebHostBuilder");
                if (createWebHostMethod != null)
                {
                    var webHostBuilder = (IWebHostBuilder)createWebHostMethod.Invoke(null, new object[] { args });
                    webHost = webHostBuilder.Build();
                }
            }

            if (webHost != null)
            {
                return webHost
                    .Services
                    .GetRequiredService<IServiceScopeFactory>()
                    .CreateScope()
                    .ServiceProvider;
            }

            throw new InvalidOperationException($"Unable to locate BuildWebHost or CreateWebHostBuilder on entry point type {assembly.EntryPoint}.");
        }

        private AspNetCoreToSwaggerGeneratorCommandSettings CreateSettings(string settingsData)
        {
            var assemblyLoader = new AssemblyLoader.AssemblyLoader();
            var settings = JsonConvert.DeserializeObject<AspNetCoreToSwaggerGeneratorCommandSettings>(settingsData);
            if (settings.DocumentProcessorTypes != null)
            {
                foreach (var p in settings.DocumentProcessorTypes)
                {
                    var processor = CreateInstance<IDocumentProcessor>(p);
                    settings.DocumentProcessors.Add(processor);
                }
            }

            if (settings.OperationProcessorTypes != null)
            {
                foreach (var p in settings.OperationProcessorTypes)
                {
                    var processor = CreateInstance<IOperationProcessor>(p);
                    settings.OperationProcessors.Add(processor);
                }
            }

            return settings;
        }
    }
}
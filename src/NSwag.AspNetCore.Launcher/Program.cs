using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace NSwag.AspNetCore.Launcher
{
    public class Program
    {
        private const string EntryPointType = "NSwag.SwaggerGeneration.AspNetCore.AspNetCoreToSwaggerGeneratorCommandEntryPoint";
        private static readonly AssemblyName AspNetCoreSwaggerGenerationAssembly = new AssemblyName("NSwag.SwaggerGeneration.AspNetCore");
        private enum ExitCode
        {
            Success = 0,
            Fail,
            InsufficientArguments,
            SettingsFileNotFound,
        };

        static int Main(string[] args)
        {
            // NSwag.Console.AspNetCore [settingsFile] [toolsDirectory]
            if (args.Length != 2)
            {
                return (int)ExitCode.InsufficientArguments;
            }

            var settingsFilePath = args[0];
            var toolsDirectory = args[1];

            if (!File.Exists(settingsFilePath))
            {
                return (int)ExitCode.SettingsFileNotFound;
            }

            var settingsContent = File.ReadAllText(settingsFilePath);

#if NETCOREAPP1_0
            AssemblyLoadContext.Default.Resolving += (context, assemblyName) =>
            {
                var assemblyLocation = Path.Combine(toolsDirectory, assemblyName.Name + ".dll");

                if (File.Exists(assemblyLocation))
                {
                    return context.LoadFromAssemblyPath(assemblyLocation);
                }

                return null;
            };
            var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(AspNetCoreSwaggerGenerationAssembly);
#else
            AppDomain.CurrentDomain.AssemblyResolve += (source, eventArgs) =>
            {
                var assemblyLocation = Path.Combine(toolsDirectory, eventArgs.Name + ".dll");

                if (File.Exists(assemblyLocation))
                {
                    return Assembly.LoadFile(assemblyLocation);
                }

                return null;
            };
            var assembly = Assembly.Load(AspNetCoreSwaggerGenerationAssembly);
#endif

            var type = assembly.GetType(EntryPointType);
            var method = type.GetMethod("Process", BindingFlags.Public | BindingFlags.Instance);

            try
            {
                var instance = Activator.CreateInstance(type);
                method.Invoke(instance, new[] { settingsContent });
            }
            catch (Exception ex)
            {
                System.Console.Error.WriteLine(ex.ToString());
                return (int)ExitCode.Fail;
            }

            return 0;
        }
    }
}

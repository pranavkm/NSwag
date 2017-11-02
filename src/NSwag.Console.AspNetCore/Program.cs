using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace NSwag
{
    public class Program
    {
        private const string AspNetCoreSwaggerGenerationAssembly = "NSwag.SwaggerGeneration.AspNetCore";
        private const string EntryPointType = "NSwag.SwaggerGeneration.AspNetCore.AspNetCoreToSwaggerGeneratorCommandEntryPoint";
        private static readonly string AssemblyDirectory = Path.GetDirectoryName(typeof(Program).GetTypeInfo().Assembly.Location);

        static int Main(string[] args)
        {
            if (args.Length != 1 || !File.Exists(args[0]))
            {
                return 1;
            }

            var settingsContent = File.ReadAllText(args[0]);

            var loadContext = AssemblyLoadContext.Default;
            loadContext.Resolving += LoadContext_Resolving;

            var assembly = loadContext.LoadFromAssemblyName(new AssemblyName(AspNetCoreSwaggerGenerationAssembly));
            var type = assembly.GetType(EntryPointType);
            var method = type.GetMethod("Process", BindingFlags.Public | BindingFlags.Instance);

            try
            {
                var instance = Activator.CreateInstance(type);
                method.Invoke(instance, new[] { settingsContent });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
                return 2;
            }

            return 0;
        }

        private static Assembly LoadContext_Resolving(AssemblyLoadContext loadContext, AssemblyName assemblyName)
        {
            var assemblyLocation = Path.Combine(AssemblyDirectory, assemblyName.Name + ".dll");

            if (File.Exists(assemblyLocation))
            {
                return loadContext.LoadFromAssemblyPath(assemblyLocation);
            }

            return null;
        }
    }
}

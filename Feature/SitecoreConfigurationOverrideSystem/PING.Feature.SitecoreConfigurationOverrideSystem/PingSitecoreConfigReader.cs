using System;
using System.CodeDom.Compiler;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Linq;
using System.Collections.Generic;

namespace PING.Feature.SitecoreConfigurationOverrideSystem
{
    public class PingSitecoreConfigReader : IConfigurationSectionHandler
    {
        private IConfigurationSectionHandler BaseConfig;

        private IConfigurationSectionHandler CreateDynamicConfigReader(Type configReaderBaseType) => Activator.CreateInstance(MyTypeFromAssembly(configReaderBaseType)) as IConfigurationSectionHandler;

        private Type MyTypeFromAssembly(Type configReaderBaseType)
        {
            var configReaderBaseTypeName = configReaderBaseType.FullName;
            var className = "PingSitecoreConfigReader";
            var nameSpace = "PING.Feature.SitecoreConfigurationOverrideSystemMirrorDimension";

            Type ret = null;


            using (var cp = CodeDomProvider.CreateProvider("CSharp"))
            {
                CompilerParameters parameters = new CompilerParameters();
                parameters.GenerateInMemory = true;
                parameters.ReferencedAssemblies.AddRange(GatherReferences(configReaderBaseType));

                var fileContent = Properties.Resources.PingSitecoreConfigOverrideReaderTemplate.Replace("$BaseTypeToken", configReaderBaseTypeName).Replace("$ClassNameToken", className).Replace("$NameSpaceToken", nameSpace);
                char[] seperators = { '\n', '\r', '\t' };
                string[] code = fileContent.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
                var result = cp.CompileAssemblyFromSource(parameters, string.Join("", code));

                if (result.Errors.Count > 0)
                {
                    var errorMessage = "";
                    foreach (CompilerError error in result.Errors)
                    {
                        errorMessage += error.ErrorText;
                    }

                    throw new Exception("Error while compiling PingSitecoreConfigReader: " + errorMessage);
                }

                ret = result.CompiledAssembly.GetType(string.Format("{0}.{1}", nameSpace, className));
            }

            return ret;
        }

        private string[] GatherReferences(Type configReaderBaseType)
        {
            var knownReferences = new[] { "System.Xml.dll", "System.dll", "System.Core.dll", "System.Configuration.dll" };

            var baseExistingRefs = configReaderBaseType.Assembly.GetReferencedAssemblies().Select(x => x.FullName).ToArray();
            var rawAssemblyList = ExtractDllLocations(baseExistingRefs).ToList();
            rawAssemblyList.AddRange(ExtractDllLocations(knownReferences));
            rawAssemblyList.Add(configReaderBaseType.Assembly.Location);

            return CleanUpAssemblyList(rawAssemblyList);
        }

        private string[] CleanUpAssemblyList(IEnumerable<string> assemList)
        {
            var result = new List<string>();

            foreach (var al in assemList)
            {
                if (!result.Any(x => x.ToUpperInvariant() == al.ToUpperInvariant()) && !result.Any(x => new FileInfo(x).Name == new FileInfo(al).Name))
                {
                    result.Add(al);
                }
            }
            return result.ToArray();
        }

        private IEnumerable<string> ExtractDllLocations(string[] dllNames)
        {
            var assemList = new List<string>();
            foreach (var ed in dllNames)
            {
                var al = "";
                try
                {
                    al = Assembly.ReflectionOnlyLoad(ed)?.Location;
                }
                catch (Exception)
                {
                    //ignore not found dlls
                }

                if (!string.IsNullOrEmpty(al) && File.Exists(al))
                {
                    assemList.Add(al);
                }
            }
            return assemList;
        }

        public object Create(object parent, object configContext, XmlNode section)
        {
            Type configReaderBaseType = Type.GetType(((XmlElement)section)?.GetAttribute("configReaderBaseType"));

            ValidateBaseType(configReaderBaseType);

            BaseConfig = CreateDynamicConfigReader(configReaderBaseType);

            return BaseConfig.Create(parent, configContext, section);
        }

        private void ValidateBaseType(Type configReaderBaseType)
        {
            if (configReaderBaseType == null)
            {
                throw new Exception("Please ensure configReaderBaseType on Sitecore node is defined correctly.");
            }
            if (configReaderBaseType.GetInterface(typeof(IConfigurationSectionHandler).FullName) == null)
            {
                throw new Exception("Please ensure configReaderBaseType implements System.Configuration.IConfigurationSectionHandler interface.");
            }
            if (GetMethod(configReaderBaseType, "ReplaceEnvironmentVariables") == null || GetMethod(configReaderBaseType, "ReplaceGlobalVariables") == null)
            {
                throw new Exception("Please ensure configReaderBaseType has overrideable ReplaceEnvironmentVariables and ReplaceGlobalVariables methods.");
            }
        }

        private MethodInfo GetMethod(Type configReaderBaseType, string methodName)
        {
            return configReaderBaseType.GetMethod(methodName, BindingFlags.FlattenHierarchy | BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(XmlNode) }, null);
        }
    }
}

using System;
using System.CodeDom.Compiler;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Xml;

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

                parameters.ReferencedAssemblies.Add("System.Xml.dll");
                parameters.ReferencedAssemblies.Add("System.dll");
                parameters.ReferencedAssemblies.Add("System.Core.dll");
                parameters.ReferencedAssemblies.Add("System.Configuration.dll");
                parameters.ReferencedAssemblies.Add(configReaderBaseType.Assembly.Location);

                var fileContent = Properties.Resources.PingSitecoreConfigOverrideReaderTemplate.Replace("$BaseTypeToken", configReaderBaseTypeName).Replace("$ClassNameToken", className).Replace("$NameSpaceToken", nameSpace);
                char[] seperators = { '\n', '\r', '\t' };
                string[] code = fileContent.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
                var result = cp.CompileAssemblyFromSource(parameters, string.Join("", code));

                if (result.Errors.Count > 0)
                {
                    foreach (var error in result.Errors)
                    {
                    }
                    throw new Exception("Error while compiling PING.Feature.SitecoreConfigurationOverrideSystemMirrorDimension.PingSitecoreConfigReader");
                }

                ret = result.CompiledAssembly.GetType(string.Format("{0}.{1}", nameSpace, className));
            }

            return ret;
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

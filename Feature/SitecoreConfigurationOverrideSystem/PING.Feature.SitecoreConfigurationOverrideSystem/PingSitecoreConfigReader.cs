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

        private IConfigurationSectionHandler CreateDynamicConfigReader(Type baseType) => Activator.CreateInstance(MyTypeFromAssembly(baseType)) as IConfigurationSectionHandler;

        private Type MyTypeFromAssembly(Type baseType)
        {
            var baseTypeName = baseType.FullName;
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
                parameters.ReferencedAssemblies.Add(Path.GetFileName(baseType.Assembly.Location));

                var fileContent = Properties.Resources.PingSitecoreConfigOverrideReaderTemplate.Replace("$BaseTypeToken", baseTypeName).Replace("$ClassNameToken", className).Replace("$NameSpaceToken", nameSpace);
                char[] seperators = { '\n', '\r', '\t' };
                string[] code = fileContent.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
                var result = cp.CompileAssemblyFromSource(parameters, string.Join("", code));

                ret = result.CompiledAssembly.GetType(string.Format("{0}.{1}", nameSpace, className));
            }

            return ret;
        }

        public object Create(object parent, object configContext, XmlNode section)
        {
            Type baseType = Type.GetType(((XmlElement)section)?.GetAttribute("baseType"));

            ValidateBaseType(baseType);

            BaseConfig = CreateDynamicConfigReader(baseType);

            return BaseConfig.Create(parent, configContext, section);
        }

        private void ValidateBaseType(Type baseType)
        {
            if (baseType == null)
            {
                throw new Exception("Please ensure baseType on Sitecore node is defined correctly.");
            }
            if (baseType.GetInterface(typeof(IConfigurationSectionHandler).FullName) == null)
            {
                throw new Exception("Please ensure baseType implements System.Configuration.IConfigurationSectionHandler interface.");
            }
            if (GetMethod(baseType, "ReplaceEnvironmentVariables") == null || GetMethod(baseType, "ReplaceGlobalVariables") == null)
            {
                throw new Exception("Please ensure baseType has overrideable ReplaceEnvironmentVariables and ReplaceGlobalVariables methods.");
            }
        }

        private MethodInfo GetMethod(Type baseType, string methodName)
        {
            return baseType.GetMethod(methodName, BindingFlags.FlattenHierarchy | BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(XmlNode) }, null);
        }
    }
}

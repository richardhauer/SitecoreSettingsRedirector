using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Xml;

namespace PING.Feature.SitecoreConfigurationOverrideSystem
{
	public class PingSitecoreConfigReader : IConfigurationSectionHandler
	{
		private const string TemplateResourceName = "PING.Feature.SitecoreConfigurationOverrideSystem.Resources.PingSitecoreConfigOverrideReaderTemplate.txt";
		private const string TargetTypeName = "PING.Feature.SitecoreConfigurationOverrideSystem.PingSitecoreConfigOverrideReader";
		private const string BaseTypeToken = "$BaseTypeToken$";

		private Assembly[] loadedAssemblies;
		public Assembly[] LoadedAssemlbies
		{
			get {
				if ( loadedAssemblies == null )
					loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
				return loadedAssemblies;
			}
		}

		private IConfigurationSectionHandler BaseConfig;

		private IConfigurationSectionHandler CreateDynamicConfigReader(Type configReaderBaseType) => Activator.CreateInstance(MyTypeFromAssembly(configReaderBaseType)) as IConfigurationSectionHandler;

		private Type MyTypeFromAssembly(Type configReaderBaseType)
		{
			Type ret = null;
			using (var cp = CodeDomProvider.CreateProvider("CSharp"))
			{
#if DEBUG
				var parameters = new CompilerParameters { GenerateInMemory = true, IncludeDebugInformation = true };
#else
				var parameters = new CompilerParameters { GenerateInMemory = true };
#endif

				parameters.ReferencedAssemblies.AddRange(GatherReferences(configReaderBaseType));
				var fileContent = GetTemplateCode( configReaderBaseType.FullName );
#if DEBUG
				var tmp = Path.Combine( Path.GetTempPath(), "PingSitecoreConfigOverrideReader.cs" );
				File.WriteAllText( tmp, fileContent );
				var result = cp.CompileAssemblyFromFile( parameters, tmp );
#else
				var result = cp.CompileAssemblyFromSource( parameters, fileContent );
#endif
				ThrowErrorsIfAny( result);

				ret = result.CompiledAssembly.GetType( TargetTypeName );
			}
			return ret;
		}

		private string GetTemplateCode( string baseTypeName )
		{
			var s = Assembly.GetAssembly( this.GetType() ).GetManifestResourceStream( TemplateResourceName );
			using ( StreamReader sr = new StreamReader( s ) )
				return sr.ReadToEnd().Replace( BaseTypeToken, baseTypeName );
		}

		private void ThrowErrorsIfAny(CompilerResults result)
		{
			if ( result.Errors.Count > 0)
				throw new Exception("Error while compiling: " + string.Join(". ", result.Errors.Cast<CompilerError>().Select(x => x.ErrorText)));
		}

		private string[] GatherReferences(Type configReaderBaseType)
		{
			var knownReferences = new[] { "netstandard", "System.Xml", "System", "System.Core", "System.Configuration" };
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
				if (!result.Any(x => x.ToUpperInvariant() == al.ToUpperInvariant()) && !result.Any(x => new FileInfo(x).Name.ToUpperInvariant() == new FileInfo(al).Name.ToUpperInvariant()))
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
				var assemblyLocation = GetFromLoadedAssemblies( ed );
				if ( assemblyLocation != null )
				{
					assemList.Add( assemblyLocation );
					continue;
				}
				
				try
				{
					assemblyLocation = Assembly.ReflectionOnlyLoad(ed)?.Location;
					if ( !string.IsNullOrEmpty( assemblyLocation ) )
						assemList.Add( assemblyLocation );
				}
				catch ( Exception ) { /* ignore not found dlls */ }
			}
			return assemList;
		}

		private string GetFromLoadedAssemblies( string ed )
		{
			return LoadedAssemlbies
					.FirstOrDefault( ass => {
						var an = ass.GetName();
						return an.Name.Equals( ed, StringComparison.OrdinalIgnoreCase ) || an.FullName.Equals( ed, StringComparison.OrdinalIgnoreCase );
					} )?.Location;
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

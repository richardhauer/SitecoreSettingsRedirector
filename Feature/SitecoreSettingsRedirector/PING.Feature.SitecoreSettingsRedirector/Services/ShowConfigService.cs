using PING.Feature.SitecoreSettingsRedirector.Abstractions;
using Sitecore.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PING.Feature.SitecoreSettingsRedirector.Services
{
	public class ShowConfigService : IShowConfigService
	{
		private readonly NameValueCollection RuleCollection;
		private string Layers;

		private IEnumerable<IConfigurationLayer> ConfigLayers =>
			new LayeredConfigurationFiles()
					.ConfigurationLayerProviders
					.SelectMany( c => c.GetLayers() );

		private IEnumerable<string> ContextRules => global::Sitecore.Context
																	  .ConfigurationRules
																		?.GetRuleDefinitionNames()
																		?.Select( n => n?.ToUpperInvariant() );

		public ShowConfigService()
		{
			RuleCollection = new NameValueCollection();
			Layers = String.Empty;
		}

		public ShowConfigService( NameValueCollection queryString ) : this()
		{
			ProcessQueryString( queryString );
		}

		private void ProcessQueryString( NameValueCollection queryString )
		{
			foreach ( var key in queryString.AllKeys )
			{
				var val = queryString[key];

				if ( key == "layer" )
					Layers = val;
				else if ( ContextRules.Any( r => r == key.ToUpperInvariant() ) )
					RuleCollection.Add( $"{key}:{RuleBasedConfigReader.RuleDefineSuffix}", val );
			}
		}

		public XmlDocument GetConfiguration()
		{
			XmlDocument config;

			if ( IsRuleBasedConfiguration() )
				config = GetRuleBasedConfiguration();
			else
				config = GetStandardConfiguration();

			ApplySettingsOverrides( config );

			return config;
		}

		private void ApplySettingsOverrides( XmlDocument config )
		{
			var settings = config.SelectNodes( "//settings/setting" );

			foreach ( XmlElement setting in settings )
				UpdateIfOverridden( setting );
		}

		private void UpdateIfOverridden( XmlElement setting )
		{
			var name = setting.GetAttribute( "name" );
			var configVal = setting.GetAttribute( "value" );
			var runtimeValue = Settings.GetSetting( name );

			if ( configVal != runtimeValue )
			{ 
				setting.SetAttribute( "value", runtimeValue );
				setting.SetAttribute( "source", "http://www.sitecore.net/xmlconfig/", "Runtime Override" );
			}
		}

		protected virtual XmlDocument GetStandardConfiguration()
		{
			return global::Sitecore.Configuration.Factory.GetConfiguration();
		}

		protected virtual XmlDocument GetRuleBasedConfiguration()
		{
			var layers = Layers.Split( new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries );
			var reader = new RuleBasedConfigReader( this.GetIncludeFiles( layers ), RuleCollection );

			// yep
			var doGetConfiguration = reader.GetType().GetMethod( "DoGetConfiguration", BindingFlags.Instance | BindingFlags.NonPublic );
			return doGetConfiguration.Invoke( reader, null ) as XmlDocument;
		}

		protected IEnumerable<string> GetIncludeFiles( string[] layers )
		{
			return ConfigLayers
					.Where( l => !layers.Any() || layers.Contains( l.Name ) )
					.SelectMany( c => c.GetConfigurationFiles() )
					.Distinct( StringComparer.OrdinalIgnoreCase );
		}

		protected virtual bool IsRuleBasedConfiguration()
		{
			return RuleCollection.HasKeys() || Layers != String.Empty;
		}

	}
}

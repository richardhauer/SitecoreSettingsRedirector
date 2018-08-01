using Sitecore.Abstractions;
using Sitecore.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PING.Feature.SitecoreSettingsRedirector.Services
{
	public class OverridableSettings : DefaultSettings
	{
		private const string AppSettingsPrefix = "SitecoreSetting.";

		public OverridableSettings( BaseFactory factory, BaseLog log ) : base( factory, log )
		{
		}

		public override string GetSetting( string name, string defaultValue )
		{
			if ( AppSettingExists( name ) )
				return AppSettingEquivalent( name );

			return base.GetSetting( name, defaultValue );
		}

		private string AppSettingEquivalent( string name )
		{
			return ConfigurationManager.AppSettings[ AppSettingsPrefix + name ];
		}

		private bool AppSettingExists( string name )
		{
			return ConfigurationManager.AppSettings.AllKeys.Contains( AppSettingsPrefix + name );
		}
	}
}

using Sitecore;
using Sitecore.Abstractions;
using Sitecore.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
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
				return AppSettingEquivalentAsString( name );

			return base.GetSetting( name, defaultValue );
		}
		private string AppSettingEquivalentAsString( string name )
		{
			return ConfigurationManager.AppSettings[AppSettingsPrefix + name];
		}

		#region Bool
		public override bool GetBoolSetting( string name, bool defaultValue )
		{
			if ( AppSettingExists( name ) && AppSettingIsBool( name ) )
				return AppSettingEquivalentAsBool( name );

			return base.GetBoolSetting( name, defaultValue );
		}
		private bool AppSettingIsBool( string name )
		{
			var val = ConfigurationManager.AppSettings[ AppSettingsPrefix + name ];
			return bool.TryParse( val, out _ );
		}
		private bool AppSettingEquivalentAsBool( string name )
		{
			return bool.Parse( ConfigurationManager.AppSettings[AppSettingsPrefix + name] );
		}
		#endregion

		#region Int
		public override int GetIntSetting( string name, int defaultValue )
		{
			if ( AppSettingExists( name ) && AppSettingIsInt( name ) )
				return AppSettingEquivalentAsInt( name );

			return base.GetIntSetting( name, defaultValue );
		}
		private bool AppSettingIsInt( string name )
		{
			var val = ConfigurationManager.AppSettings[ AppSettingsPrefix + name ];
			return val.EndsWith( "b", StringComparison.OrdinalIgnoreCase ) || int.TryParse( val, out _ );
		}
		private int AppSettingEquivalentAsInt( string name )
		{
			var val = ConfigurationManager.AppSettings[ AppSettingsPrefix + name ];
			if ( val.EndsWith( "b", StringComparison.OrdinalIgnoreCase ) )
				return (int)StringUtil.ParseSizeString( val );

			return int.Parse( val );
		}
		#endregion

		#region Long
		public override long GetLongSetting( string name, long defaultValue )
		{
			if ( AppSettingExists( name ) && AppSettingIsLong( name ) )
				return AppSettingEquivalentAsLong( name );

			return base.GetLongSetting( name, defaultValue );
		}
		private bool AppSettingIsLong( string name )
		{
			var val = ConfigurationManager.AppSettings[ AppSettingsPrefix + name ];
			return val.EndsWith( "b", StringComparison.OrdinalIgnoreCase ) || long.TryParse( val, out _ );
		}
		private long AppSettingEquivalentAsLong( string name )
		{
			var val = ConfigurationManager.AppSettings[ AppSettingsPrefix + name ];
			if ( val.EndsWith( "b", StringComparison.OrdinalIgnoreCase ) )
				return StringUtil.ParseSizeString( val );

			return long.Parse( val );
		}
		#endregion

		#region Double
		public override double GetDoubleSetting( string name, double defaultValue )
		{
			if ( AppSettingExists( name ) && AppSettingIsDouble( name ) )
				return AppSettingEquivalentAsDouble( name );

			return base.GetDoubleSetting( name, defaultValue );
		}
		private bool AppSettingIsDouble( string name )
		{
			var val = ConfigurationManager.AppSettings[ AppSettingsPrefix + name ];
			return double.TryParse( val, out _ );
		}
		private double AppSettingEquivalentAsDouble( string name )
		{
			var val = ConfigurationManager.AppSettings[ AppSettingsPrefix + name ];
			return double.Parse( val );
		}
		#endregion

		#region Timespan
		public override TimeSpan GetTimeSpanSetting( string name, TimeSpan defaultValue, CultureInfo cultureInfo )
		{
			if ( AppSettingExists( name ) && AppSettingIsTimeSpan( name, cultureInfo ) )
				return AppSettingEquivalentAsTimeSpan( name, cultureInfo );

			return base.GetTimeSpanSetting( name, defaultValue, cultureInfo );
		}
		private bool AppSettingIsTimeSpan( string name, CultureInfo cultureInfo )
		{
			var val = ConfigurationManager.AppSettings[ AppSettingsPrefix + name ];
			return TimeSpan.TryParse( val, cultureInfo, out _ );
		}
		private TimeSpan AppSettingEquivalentAsTimeSpan( string name, CultureInfo cultureInfo )
		{
			var val = ConfigurationManager.AppSettings[ AppSettingsPrefix + name ];
			return TimeSpan.Parse( val, cultureInfo );
		}
		#endregion

		private bool AppSettingExists( string name )
		{
			return ConfigurationManager.AppSettings.AllKeys.Any( key => key.Equals( AppSettingsPrefix + name, StringComparison.InvariantCultureIgnoreCase ) );
		}
	}
}

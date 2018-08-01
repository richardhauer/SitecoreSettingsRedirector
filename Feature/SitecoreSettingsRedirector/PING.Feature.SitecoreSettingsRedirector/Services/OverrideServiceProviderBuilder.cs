
using Sitecore.Configuration;
using Sitecore.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace PING.Feature.SitecoreSettingsRedirector.Services
{
	public class OverrideServiceProviderBuilder : DefaultServiceProviderBuilder
	{
		private const string SitecoreConfigReplacementPath = "/sitecore/overrideDefaultServiceRegistrations/override";
		private readonly IDictionary<Type,IServicesConfigurator> ConfiguratorReplacements;

		public OverrideServiceProviderBuilder()
		{
			ConfiguratorReplacements = new Dictionary<Type, IServicesConfigurator>
			{
				{ typeof( DefaultSitecoreServicesConfigurator ), CreateConfigurator() }
			};
		}

		public IServicesConfigurator CreateConfigurator()
		{
			var ret = new OverridableSitecoreServicesConfigurator();
			var replacementConfigs = ConfigReader.GetConfiguration().SelectNodes( SitecoreConfigReplacementPath );

			foreach ( XmlNode replacement in replacementConfigs )
				ret.RegisterReplacement( replacement );

			return ret;
		}

		public void RegisterReplacement( Type replace, IServicesConfigurator with )
		{
			if ( ConfiguratorReplacements.ContainsKey( replace ) )
				ConfiguratorReplacements.Remove( replace );

			ConfiguratorReplacements.Add( replace, with );
		}

		public override IEnumerable<IServicesConfigurator> GetServicesConfigurators()
		{
			return ReplaceConfigurators( base.GetServicesConfigurators() );
		}

		private IEnumerable<IServicesConfigurator> ReplaceConfigurators( IEnumerable<IServicesConfigurator> configurators )
		{
			var toReplace = configurators
								.Where( c => ConfiguratorReplacements.ContainsKey( c.GetType() ) );

			var newConfigurators = ConfiguratorReplacements
										.Where( kvp => toReplace.Select( c => c.GetType() ).Contains( kvp.Key ) )
										.Select( kvp => kvp.Value );

			return configurators
						.Where( c => !ConfiguratorReplacements.ContainsKey( c.GetType() ) )
						.Concat( newConfigurators );
		}
	}
}

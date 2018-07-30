using PING.Feature.SitecoreSettingsRedirector.Abstractions;
using PING.Feature.SitecoreSettingsRedirector.Services;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Abstractions;
using Sitecore.Pipelines;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PING.Feature.SitecoreSettingsRedirector.Pipelines.Initialize
{
	public class InitializeSolrSettingsRedirector : ISitecoreInitializePipelineProcessor
	{
		private readonly ISitecoreStaticSettings SettingsAdaptor;

		public InitializeSolrSettingsRedirector( ISitecoreStaticSettings settingsAdaptor )
		{
			SettingsAdaptor = settingsAdaptor;
		}

		public void Process( PipelineArgs args )
		{
			lock( ContentSearchManager.Locator )
			{
				ContentSearchManager.Locator.UnRegister( typeof( ISettings ) );
				ContentSearchManager.Locator.Register<ISettings>( _ => new AzureSolrSettingsRedirector( SettingsAdaptor, ConfigurationManager.AppSettings ) );
			}
		}
	}
}

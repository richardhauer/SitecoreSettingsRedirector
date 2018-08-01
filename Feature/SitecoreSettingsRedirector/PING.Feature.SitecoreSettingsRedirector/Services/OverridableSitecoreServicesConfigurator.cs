using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PING.Feature.SitecoreSettingsRedirector.Services
{
	public class OverridableSitecoreServicesConfigurator : IServicesConfigurator
	{
		private readonly IServicesConfigurator DefaultSitecoreServicesConfigurator;
		private readonly IList<ServiceDescriptor> ServiceReplacements;


		public OverridableSitecoreServicesConfigurator()
		{
			DefaultSitecoreServicesConfigurator = new DefaultSitecoreServicesConfigurator();
			ServiceReplacements = new List<ServiceDescriptor>();
		}

		public void RegisterReplacement( XmlNode configNode )
		{
			var serviceTypeName = configNode.Attributes["serviceType"].Value;
			var implTypeName = configNode.Attributes["implementationType"].Value;
			var lifetime = configNode.Attributes["lifetime"].Value;

			var descriptor = new ServiceDescriptor(
									Type.GetType( serviceTypeName, true, true ),
									Type.GetType( implTypeName, true, true ),
									(ServiceLifetime)Enum.Parse( typeof(ServiceLifetime), lifetime )
								);

			RegisterReplacement( descriptor );
		}

		public void RegisterReplacement( ServiceDescriptor descriptor )
		{
			ServiceReplacements.Add( descriptor );
		}

		public void Configure( IServiceCollection serviceCollection )
		{
			DefaultSitecoreServicesConfigurator.Configure( serviceCollection );
			ProcessReplacements( serviceCollection );
		}

		private void ProcessReplacements( IServiceCollection serviceCollection )
		{
			foreach ( var descriptor in ServiceReplacements )
				ProcessReplacement( serviceCollection, descriptor );
		}

		private void ProcessReplacement( IServiceCollection serviceCollection, ServiceDescriptor descriptor )
		{
			var toRemove = serviceCollection.FirstOrDefault( d => d.ServiceType == descriptor.ServiceType );

			if ( toRemove == null )
				return;

			serviceCollection.Remove( toRemove );
			serviceCollection.Add( descriptor );
		}
	}
}

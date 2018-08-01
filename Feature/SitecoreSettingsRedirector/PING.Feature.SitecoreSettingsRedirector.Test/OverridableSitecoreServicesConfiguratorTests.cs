using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PING.Feature.SitecoreSettingsRedirector.Services;
using Sitecore.Abstractions;
using Sitecore.Configuration;
using Sitecore.DependencyInjection;

namespace PING.Feature.SitecoreSettingsRedirector.Test
{
	[TestClass]
	public class OverridableSitecoreServicesConfiguratorTests
	{
		private interface ISampleInterface { }
		private class SampleClass : ISampleInterface { }


		[TestMethod]
		public void OverridableSitecoreServicesConfigurator_Interfaces()
		{
			var svc = new OverridableSitecoreServicesConfigurator();

			Assert.IsInstanceOfType( svc, typeof( IServicesConfigurator ) );
		}

		[TestMethod]
		public void OverridableSitecoreServicesConfigurator_WithoutReplacingServices()
		{
			var svc = new OverridableSitecoreServicesConfigurator();
			var services = new ServiceCollection();

			svc.Configure( services );

			Assert.AreEqual( typeof( DefaultSettings ), GetImplementationType( services, typeof( BaseSettings ) ) );
		}

		[TestMethod]
		public void OverridableSitecoreServicesConfigurator_WithReplacingServices()
		{
			var svc = new OverridableSitecoreServicesConfigurator();
			svc.RegisterReplacement( ServiceDescriptor.Singleton<BaseSettings, OverridableSettings>() );
			var services = new ServiceCollection();

			svc.Configure( services );

			Assert.AreEqual( typeof( OverridableSettings ), GetImplementationType( services, typeof( BaseSettings ) ) );
		}

		[TestMethod]
		public void OverridableSitecoreServicesConfigurator_ReplaceUnregisteredService()
		{
			var svc = new OverridableSitecoreServicesConfigurator();
			svc.RegisterReplacement( ServiceDescriptor.Singleton<ISampleInterface, SampleClass>() );
			var services = new ServiceCollection();

			svc.Configure( services );

			Assert.AreEqual( typeof( DefaultSettings ), GetImplementationType( services, typeof( BaseSettings ) ) );
			Assert.IsNull( GetImplementationType( services, typeof( ISampleInterface ) ) );
		}

		private Type GetImplementationType( ServiceCollection services, Type type )
		{
			for ( int i = 0; i < services.Count; i++ )
			{
				if ( services[i].ServiceType == type )
					return services[i].ImplementationType;
			}

			return null;
		}
	}
}

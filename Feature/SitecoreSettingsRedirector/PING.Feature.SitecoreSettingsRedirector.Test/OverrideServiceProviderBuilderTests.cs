using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PING.Feature.SitecoreSettingsRedirector.Services;
using Sitecore.Abstractions;
using Sitecore.DependencyInjection;

namespace PING.Feature.SitecoreSettingsRedirector.Test
{
	public class TestConfigurator : IServicesConfigurator
	{
		public void Configure( IServiceCollection serviceCollection )
		{
			/* we don't need an implementation for this test */
		}
	}

	[TestClass]
	public class OverrideServiceProviderBuilderTests
	{
		[TestMethod]
		public void OverrideServiceProviderBuilder_Inheritance()
		{
			var svc = new OverrideServiceProviderBuilder();

			Assert.IsInstanceOfType( svc, typeof( BaseServiceProviderBuilder ) );
		}

		[TestMethod]
		public void OverrideServiceProviderBuilder_GetServicesConfiguratorsWithReplacements()
		{
			var svc = new OverrideServiceProviderBuilder();
			svc.RegisterReplacement( typeof( DefaultSitecoreServicesConfigurator ), new TestConfigurator() );
			var configurators = svc.GetServicesConfigurators();

			var removedConfigurator = configurators.FirstOrDefault( c => c is  DefaultSitecoreServicesConfigurator );
			var replacedConfigurator = configurators.FirstOrDefault( c => c is TestConfigurator );

			Assert.IsNull( removedConfigurator );
			Assert.IsNotNull( replacedConfigurator );
		}
	}
}

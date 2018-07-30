using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PING.Feature.SitecoreSettingsRedirector.Abstractions;
using PING.Feature.SitecoreSettingsRedirector.Pipelines.Initialize;
using PING.Feature.SitecoreSettingsRedirector.Services;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Abstractions;

namespace PING.Feature.SitecoreSettingsRedirector.Test
{
	[TestClass]
	public class InitializeSolrSettingsRedirectorTests
	{
		private Type OldISettingsType;

		[TestInitialize]
		public void Setup()
		{
			OldISettingsType = Type.GetType( "Sitecore.ContentSearch.Abstractions.SettingsWrapper, Sitecore.ContentSearch", true );

		}

		[TestMethod]
		public void InitializeSolrSettingsRedirector_Interface()
		{
			var svc = new InitializeSolrSettingsRedirector( null );

			Assert.IsInstanceOfType( svc, typeof( ISitecoreInitializePipelineProcessor ) );
		}

		[TestMethod]
		public void InitializeSolrSettingsRedirector_ProcessOverridesRegistration()
		{
			var oldService = ContentSearchManager.Locator.GetInstance<ISettings>();
			Assert.IsInstanceOfType( oldService, OldISettingsType );

			var svc = new InitializeSolrSettingsRedirector( null );
			svc.Process( null );

			var newService = ContentSearchManager.Locator.GetInstance<ISettings>();
			Assert.IsInstanceOfType( newService, typeof( AzureSolrSettingsRedirector ) );
		}
	}
}

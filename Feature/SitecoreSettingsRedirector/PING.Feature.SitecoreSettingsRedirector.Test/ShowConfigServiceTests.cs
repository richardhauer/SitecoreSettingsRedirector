using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PING.Feature.SitecoreSettingsRedirector.Abstractions;
using PING.Feature.SitecoreSettingsRedirector.Services;
using Sitecore.Configuration;

namespace PING.Feature.SitecoreSettingsRedirector.Test
{
	[TestClass]
	public class ShowConfigServiceTests
	{
		private IShowConfigService Service;

		[TestInitialize]
		public void Setup()
		{
			Service = new ShowConfigService();
		}

		[TestMethod]
		public void ShowConfigService_GetStandardConfiguration()
		{
			var xml = Service.GetConfiguration();

			// not overridden setting
			Assert.AreEqual( "test", GetAttribute( xml, "//sitecore/settings/setting[@name='stringSetting']", "value" ) );
			// overridden setting
			Assert.AreEqual( "app-value", GetAttribute( xml, "//sitecore/settings/setting[@name='overridenSetting']", "value" ) );
			Assert.AreEqual( "Runtime Override", GetAttribute( xml, "//sitecore/settings/setting[@name='overridenSetting']", "source" ) );
		}

		[TestMethod]
		public void ShowConfigService_GetLayeredConfiguration()
		{
			/*
			 *  Note: There is no specific behaviour change here in our code, I'm just exercising the layered config path through the code
			 */

			SetupConfugrationRules();
			var queryString = new NameValueCollection();
			queryString.Add( "layer", "Sitecore|Modules|Custom|Environment" );
			queryString.Add( "role", "Standalone|ContentManagement" );

			var svc = new ShowConfigService( queryString );
			var xml = svc.GetConfiguration();

			// not overridden setting
			Assert.AreEqual( "test", GetAttribute( xml, "//sitecore/settings/setting[@name='stringSetting']", "value" ) );
			// overridden setting
			Assert.AreEqual( "app-value", GetAttribute( xml, "//sitecore/settings/setting[@name='overridenSetting']", "value" ) );
			Assert.AreEqual( "Runtime Override", GetAttribute( xml, "//sitecore/settings/setting[@name='overridenSetting']", "source" ) );

		}

		private void SetupConfugrationRules()
		{
			var rulesContext = new ConfigurationRulesContext(
									new Dictionary<string, string[]>
									{
										{ "role", new string[]{ "Standalone" } },
										{ "search", new string[]{ "Azure" } },
										{ "eds", new string[]{ "CustomSmtp" } }
									}
								);

			typeof(global::Sitecore.Context).GetField( "_rulesContext", BindingFlags.Static | BindingFlags.NonPublic ).SetValue( null, rulesContext );
		}

		private string GetAttribute( XmlDocument xml, string selector, string attributeName )
		{
			return xml.SelectSingleNode( selector ).Attributes[attributeName].Value;
		}
	}
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PING.Feature.SitecoreSettingsRedirector.Services;
using Sitecore.Abstractions;

namespace PING.Feature.SitecoreSettingsRedirector.Test
{
	[TestClass]
	public class OverridableSettingsTest
	{
		private BaseLog Log;
		private BaseFactory Factory;

		[TestInitialize]
		public void Setup()
		{
			var mockLog = new Mock<BaseLog>();
			Log = mockLog.Object;

			var mockFactory = new Mock<BaseFactory>();
			Factory = mockFactory.Object;
		}


		[TestMethod]
		public void OverridableSettings_Inheritance()
		{
			var svc = new OverridableSettings( Factory, Log );

			Assert.IsInstanceOfType( svc, typeof( BaseSettings ) );
		}

		[TestMethod]
		public void OverridableSettings_NonOverriddenSettings()
		{
			// AppConfig has the settings configured
			Assert.AreEqual( 1,			Sitecore.Configuration.Settings.GetIntSetting( "intSetting", 0 ) );
			Assert.AreEqual( "test",	Sitecore.Configuration.Settings.GetSetting( "stringSetting" ) );
			Assert.AreEqual( "default",	Sitecore.Configuration.Settings.GetSetting( "not-a-setting", "default" ));
			Assert.AreEqual( "",		Sitecore.Configuration.Settings.GetSetting( "also-not-a-setting" ) );
		}

		[TestMethod]
		public void OverridableSettings_AppConfigOverriddenSettings()
		{
			Assert.AreEqual( "app-value", Sitecore.Configuration.Settings.GetSetting( "overridenSetting" ) );
		}
	}
}

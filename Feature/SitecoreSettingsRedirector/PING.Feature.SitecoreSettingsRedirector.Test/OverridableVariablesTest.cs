using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PING.Feature.SitecoreSettingsRedirector.Test
{
    [TestClass]
	public class OverridableVariablesTest
    {
		[TestMethod]
		public void OverridableSettings_NonOverriddenVariableViaSettings()
		{
			Assert.AreEqual("normalVariableSettingValue",		global::Sitecore.Configuration.Settings.GetSetting("normalVariableSetting", "" ) ); 
		}

		[TestMethod]
		public void OverridableSettings_AppConfigOverriddenVariableSettings()
		{
			Assert.AreEqual("overrideValueForVariable",	global::Sitecore.Configuration.Settings.GetSetting("overrideVariableSetting") );
		}
	}
}

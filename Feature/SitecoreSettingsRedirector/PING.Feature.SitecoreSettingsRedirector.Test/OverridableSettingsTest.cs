using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PING.Feature.SitecoreSettingsRedirector.Test
{
    [TestClass]
    public class OverridableSettingsTest
    {

        [TestMethod]
        public void OverridableSettings_NonOverriddenSettings()
        {
            Assert.AreEqual(1, global::Sitecore.Configuration.Settings.GetIntSetting("intSetting", 0));
            Assert.AreEqual(true, global::Sitecore.Configuration.Settings.GetBoolSetting("boolSetting", false));
            Assert.AreEqual(5D, global::Sitecore.Configuration.Settings.GetDoubleSetting("doubleSetting", 0));

            Assert.AreEqual("test", global::Sitecore.Configuration.Settings.GetSetting("stringSetting"));
            Assert.AreEqual("default", global::Sitecore.Configuration.Settings.GetSetting("not-a-setting", "default"));
            Assert.AreEqual("", global::Sitecore.Configuration.Settings.GetSetting("also-not-a-setting"));
        }

        [TestMethod]
        public void OverridableSettings_AppConfigOverriddenSettings_String()
        {
            Assert.AreEqual("app-value", global::Sitecore.Configuration.Settings.GetSetting("overridenStringSetting"));
        }

        [TestMethod]
        public void OverridableSettings_AppConfigOverriddenSettings_Bool()
        {
            Assert.AreEqual(true, global::Sitecore.Configuration.Settings.GetBoolSetting("overridenBoolSetting", false));
        }

        [TestMethod]
        public void OverridableSettings_AppConfigOverriddenSettings_IntLongDouble()
        {
            Assert.AreEqual(11, global::Sitecore.Configuration.Settings.GetIntSetting("overridenIntSetting1", 0));
            Assert.AreEqual(22 * 1024, global::Sitecore.Configuration.Settings.GetIntSetting("overridenIntSetting2", 0));
            Assert.AreEqual(0, global::Sitecore.Configuration.Settings.GetIntSetting("overridenIntSetting3", 0));

            Assert.AreEqual(11L, global::Sitecore.Configuration.Settings.GetLongSetting("overridenLongSetting1", 0));
            Assert.AreEqual(22 * 1024L, global::Sitecore.Configuration.Settings.GetLongSetting("overridenLongSetting2", 0));
            Assert.AreEqual(0, global::Sitecore.Configuration.Settings.GetLongSetting("overridenLongSetting3", 0));

            Assert.AreEqual(50D, global::Sitecore.Configuration.Settings.GetDoubleSetting("overridenDoubleSetting", 0));
        }
    }
}

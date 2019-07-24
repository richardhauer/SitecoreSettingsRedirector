using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PING.Feature.SitecoreConfigurationOverrideSystem.Test
{
    [TestClass]
    public class OverrideIncludeFielsSettingTest
    {
        [TestMethod]
        [Ignore]
        public void ShouldGetNormalIncludeValueFromSitecore()
        {
            //this doesn't work in unit tests since it's from include files
            Assert.AreEqual("NewValueOfsetupFromIncludeZzz", global::Sitecore.Configuration.Settings.GetSetting("setupFromIncludeZzz"));
        }

        [TestMethod]
        [Ignore]
        public void ShouldGetOverWrittenIncludeValueFromSitecore()
        {
            //this doesn't work in unit tests since it's from include files
            Assert.AreEqual("overrideValueForAppConfig", global::Sitecore.Configuration.Settings.GetSetting("setupFromIncludeZzzOverwrite"));
        }
    }

}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sitecore.Configuration;
using System.Configuration;

namespace PING.Feature.SitecoreConfigurationOverrideSystem.Test
{
    [TestClass]
    public class TypeInheritanceTest
    {
        [TestMethod]
        public void ShouldNotInheritFromSitecore()
        {
            Assert.IsInstanceOfType(new OverrideRuleBasedConfigReader(), (typeof(IConfigurationSectionHandler)));
        }

        [TestMethod]
        public void ShouldGetNormalIncludeVlaueFromSitecore()
        {
            Assert.AreEqual("NewValueOfsetupFromIncludeZzz", global::Sitecore.Configuration.Settings.GetSetting("setupFromIncludeZzz"));
        }

        [TestMethod]
        public void ShouldGetOverWrittenIncludeVlaueFromSitecore()
        {
            Assert.AreEqual("overrideValueForAppConfig", global::Sitecore.Configuration.Settings.GetSetting("setupFromIncludeZzzOverwrite"));
        }
    }

}

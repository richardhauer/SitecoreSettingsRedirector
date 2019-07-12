using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sitecore.Configuration;

namespace PING.Feature.SitecoreConfigurationOverrideSystem.Test
{
    [TestClass]
    public class HandleErrorsTest
    {
        [TestMethod]
        public void OverrideXPath_ShouldRestoreWhenError()
        {
            
            Assert.AreEqual("app-value", global::Sitecore.Configuration.Settings.GetSetting("overridenStringSetting"));
            var to3 = Factory.CreateObject("xpathtest/TestObject3", true) as TestObject;
            Assert.IsInstanceOfType(to3, typeof(TestObject));
            Assert.AreEqual("prop2", to3.Property2);
        }
    }
 
}

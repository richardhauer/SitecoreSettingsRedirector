using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sitecore.Configuration;

namespace PING.Feature.SitecoreSettingsRedirector.Test
{
    [TestClass]
    public class OverrideSitecorePatchesTest
    {
        [TestMethod]
        public void OverrideXPath_NormalObjectCreation()
        {
            var to1 = Factory.CreateObject("xpathtest/TestObject1", true) as TestObject;
            Assert.IsInstanceOfType(to1, typeof(TestObject));
            Assert.AreEqual("prop1", to1.Property1);
            Assert.AreEqual("prop2", to1.Property2);
            Assert.AreEqual("Constructor mapping", to1.CtorProp1);
            Assert.AreEqual("Sitecore.Data.DefaultDatabase, Sitecore.Kernel", to1.CtorProp2);
            Assert.AreEqual("some1", to1.To2.CtorProp1);
            Assert.AreEqual("something_123", to1.To2.CtorProp2);
        }

        [TestMethod]
        public void OverrideXPath_AddAction()
        {
            Assert.AreEqual("patchAddOverrideValue", global::Sitecore.Configuration.Settings.GetSetting("patchAddValue"));
        }

        [TestMethod]
        public void OverrideXPath_UpdateTextAction()
        {
            var to3 = Factory.CreateObject("xpathtest/TestObject3", true) as TestObject;
            Assert.IsInstanceOfType(to3, typeof(TestObject));
            Assert.AreEqual("patchUpdatenOverrideValue", to3.CtorProp1);
        }

        [TestMethod]
        public void OverrideXPath_UpdatAttributeAction()
        {
            var to3 = Factory.CreateObject("xpathtest/TestObject3", true) as TestObject;
            Assert.IsInstanceOfType(to3, typeof(TestObject));
            Assert.AreEqual("patchUpdatenAttributeOverrideValue", to3.To2.CtorProp1);
        }

        [TestMethod]
        public void OverrideXPath_RemoveAction()
        {
            var to3 = Factory.CreateObject("xpathtest/TestObject3", true) as TestObject;
            Assert.IsInstanceOfType(to3, typeof(TestObject));
            Assert.AreEqual(null, to3.To2.Property2);
        }


    }


    public class TestObject
    {
        public string CtorProp1 { get; set; }
        public string CtorProp2 { get; set; }
        public TestObject2 To2 { get; set; }
        public TestObject(string ctorProp1, string ctorProp2, TestObject2 to2)
        {
            CtorProp1 = ctorProp1;
            CtorProp2 = ctorProp2;
            To2 = to2;
        }
        public string Property1 { get; set; }
        public string Property2 { get; set; }
    }

    public class TestObject2
    {
        public string CtorProp1 { get; set; }
        public string CtorProp2 { get; set; }
        public TestObject2(string ctorProp1, string ctorProp2)
        {
            CtorProp1 = ctorProp1;
            CtorProp2 = ctorProp2;
        }
        public string Property1 { get; set; }
        public string Property2 { get; set; }
    }
}

using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PING.Feature.SitecoreSettingsRedirector.Services;
using Sitecore.Abstractions;
using Sitecore.Configuration;

namespace PING.Feature.SitecoreSettingsRedirector.Test
{
    [TestClass]
    public class OverrideXPath
    {
        [TestMethod]
        public void OverrideXPath_NormalObjectCreation()
        {
            var to1 = Factory.CreateObject("xpathtest/TestObject1", true) as TestObject;
            Assert.IsInstanceOfType(to1, typeof(TestObject));
            Assert.AreEqual(to1.Property1, "prop1");
            Assert.AreEqual(to1.Property2, "prop2");
            Assert.AreEqual(to1.CtorProp1, "Constructor mapping");
            Assert.AreEqual(to1.CtorProp2, "Sitecore.Data.DefaultDatabase, Sitecore.Kernel");
            Assert.AreEqual(to1.To2.CtorProp1, "some1");
        }

        [TestMethod]
        public void OverrideXPath_OverrideObjectCreation()
        {
            var to3 = Factory.CreateObject("xpathtest/TestObject3", true) as TestObject;
            Assert.IsInstanceOfType(to3, typeof(TestObject));
            Assert.AreEqual(to3.Property1, "prop1");
            Assert.AreEqual(to3.Property2, "prop2");
            Assert.AreEqual(to3.CtorProp1, "t8758t8");
            Assert.AreEqual(to3.CtorProp2, "Sitecore.Data.DefaultDatabase, Sitecore.Kernel");
            Assert.AreEqual(to3.To2.CtorProp1, "some1");
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

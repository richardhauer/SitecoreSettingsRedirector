using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace PING.Feature.SitecoreConfigurationOverrideSystem.Test
{
    [TestClass]
    public class DynamicTypeTest
    {
        private PingSitecoreConfigReader _pingSitecoreConfigReader;
        [TestInitialize]
        public void Setup()
        {
            _pingSitecoreConfigReader = new PingSitecoreConfigReader();
        }

        [TestMethod]
        public void ShouldCreateMirrowDimensionType()
        {
            var sitecoreNode = Sitecore.Xml.XmlUtil.LoadXml("<sitecore baseType=\"Sitecore.Configuration.RuleBasedConfigReader, Sitecore.Kernel\" />").DocumentElement;
            var typeResult = _pingSitecoreConfigReader.Create(null, null, sitecoreNode);

            Assert.IsNotNull(typeResult);
            Assert.AreEqual("PING.Feature.SitecoreConfigurationOverrideSystemMirrorDimension.PingSitecoreConfigReader", typeResult.GetType().FullName);
        }

        [TestMethod]
        public void ShouldComplaintWhenBaseTypeDoesNotExist()
        {
            var sitecoreNode = Sitecore.Xml.XmlUtil.LoadXml("<sitecore baseType=\"Sitecore.Configuration.RuleBasedConfigReaderx, Sitecore.Kernel\" />").DocumentElement;
            var ex = Assert.ThrowsException<Exception>(() => _pingSitecoreConfigReader.Create(null, null, sitecoreNode));
            Assert.AreEqual("Please ensure baseType on Sitecore node is defined correctly.", ex.Message);
        }

        [TestMethod]
        public void ShouldComplaintWhenBaseTypeNotImplmentIConfigurationSectionHandler()
        {
            var sitecoreNode = Sitecore.Xml.XmlUtil.LoadXml("<sitecore baseType=\"PING.Feature.SitecoreConfigurationOverrideSystem.Test.DynamicTypeTest, PING.Feature.SitecoreConfigurationOverrideSystem.Test\" />").DocumentElement;
            var ex = Assert.ThrowsException<Exception>(() => _pingSitecoreConfigReader.Create(null, null, sitecoreNode));
            Assert.AreEqual("Please ensure baseType implements System.Configuration.IConfigurationSectionHandler interface.", ex.Message);
        }

        [TestMethod]
        public void ShouldComplaintWhenBaseTypeMissingMethodsToOverride()
        {
            var sitecoreNode = Sitecore.Xml.XmlUtil.LoadXml("<sitecore baseType=\"Sitecore.FakeDb.Configuration.ConfigReader, Sitecore.FakeDb\" />").DocumentElement;
            var ex = Assert.ThrowsException<Exception>(() => _pingSitecoreConfigReader.Create(null, null, sitecoreNode));
            Assert.AreEqual("Please ensure baseType has overrideable ReplaceEnvironmentVariables and ReplaceGlobalVariables methods.", ex.Message);
        }
    }

}

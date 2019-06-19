using Sitecore;
using Sitecore.Configuration;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Xml;

namespace PING.Feature.SitecoreSettingsRedirector.Configuration
{
    [UsedImplicitly]
    public class OverrideRuleBasedConfigReader : RuleBasedConfigReader
    {
        const string SettingPrefixKey = "SitecoreSetting.";
        const string XpathSettingPrefixKey = "SitecoreSetting.xPath.";
        const string XpathSettingEndWithKey = ".Value";
        const string SitecoreSettingXpathTemplate = "/sitecore/settings/setting[@name=\"{0}\"]";

        protected override void ReplaceEnvironmentVariables(XmlNode rootNode)
        {
            base.ReplaceEnvironmentVariables(rootNode);

            HandleSettingsOverride(rootNode);

            HanldeXpathOverride(rootNode);
        }

        private void HandleSettingsOverride(XmlNode rootNode)
        {
            var overrideSettings = GetOverrideSettingsOnly();

            if (overrideSettings.Any())
            {
                var settingReplacements = new List<XpathReplacement>();

                foreach (var os in overrideSettings)
                {
                    var settingValue = ConfigurationManager.AppSettings[os];
                    var settingKey = os.Replace(SettingPrefixKey, "");
                    var settingXPath = string.Format(SitecoreSettingXpathTemplate, settingKey);
                    settingReplacements.Add(new XpathReplacement
                    {
                        Xpath = settingXPath,
                        AttributenName = "value",
                        NewValue = settingValue
                    });
                }
                ProcessReplacementsOnRootnode(rootNode, settingReplacements);
            }
        }

        private IEnumerable<string> GetOverrideSettingsOnly()
        {
            return ConfigurationManager.AppSettings.AllKeys.Where(x => x.StartsWith(SettingPrefixKey) && !x.StartsWith(XpathSettingPrefixKey));
        }

        private IEnumerable<string> GetOverrideXpathSettingsOnly()
        {
            return ConfigurationManager.AppSettings.AllKeys.Where(x => x.StartsWith(XpathSettingPrefixKey) && !x.EndsWith(XpathSettingEndWithKey));
        }

        private void HanldeXpathOverride(XmlNode rootNode)
        {
            var overrideXPathSettings = GetOverrideXpathSettingsOnly();

            if (overrideXPathSettings.Any())
            {
                var settingXPathCollection = new List<XpathReplacement>();

                foreach (var oxs in overrideXPathSettings)
                {

                    var xpathPath = ConfigurationManager.AppSettings[oxs];
                    var rawXpathValue = ConfigurationManager.AppSettings[oxs + XpathSettingEndWithKey];
                    if (rawXpathValue.IndexOf(':') >= 0)
                    {
                        var splitArray = rawXpathValue.Split(':');
                        if (splitArray.Count() == 2)
                        {
                            settingXPathCollection.Add(new XpathReplacement
                            {
                                Xpath = xpathPath,
                                AttributenName = splitArray[0],
                                NewValue = splitArray[1]
                            });
                        }
                    }
                    else
                    {
                        settingXPathCollection.Add(new XpathReplacement
                        {
                            Xpath = xpathPath,
                            NewValue = rawXpathValue
                        });
                    }

                }
                ProcessReplacementsOnRootnode(rootNode, settingXPathCollection);

            }
        }


        private void ProcessReplacementsOnRootnode(XmlNode rootNode, IEnumerable<XpathReplacement> replacements)
        {
            foreach (var replacement in replacements)
            {
                var nodeToChange = rootNode.SelectSingleNode(replacement.Xpath);
                if (nodeToChange != null)
                {
                    if (string.IsNullOrEmpty(replacement.AttributenName))
                    {
                        nodeToChange.InnerText = replacement.NewValue;
                    }
                    else
                    {
                        if (nodeToChange.Attributes[replacement.AttributenName] != null)
                        {
                            nodeToChange.Attributes[replacement.AttributenName].InnerText = replacement.NewValue;
                        }
                    }
                }

            }
        }

    }

    public class XpathReplacement
    {
        public string Xpath { get; set; }
        public string AttributenName { get; set; }
        public string NewValue { get; set; }
    }
}
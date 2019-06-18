using Sitecore;
using Sitecore.Configuration;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Xml;
namespace PING.Feature.SitecoreSettingsRedirector.Configuration
{
    [UsedImplicitly]
    public class OverrideRuleBasedConfigReader : RuleBasedConfigReader
    {
        protected override void ReplaceEnvironmentVariables(XmlNode rootNode)
        {
            base.ReplaceEnvironmentVariables(rootNode);

            ReplaceOverrideSettings(rootNode);
        }

        private void ReplaceOverrideSettings(XmlNode rootNode)
        {
            var overrideSettings = GetOverrideSettings();

            if (overrideSettings.Any())
            {
                var settingCollection = new NameValueCollection();

                foreach (var os in overrideSettings)
                {
                    var settingValue = ConfigurationManager.AppSettings[os];
                    var settingKey = os.Replace("SitecoreSetting.", "");
                    settingCollection[settingKey] = settingValue;
                }

                ReplaceOverrideSetting(rootNode, settingCollection);

            }
        }

        private void ReplaceOverrideSetting(XmlNode rootNode, NameValueCollection settingCollection)
        {
            XmlNodeList settingsNodes = rootNode.SelectNodes("/sitecore/settings/setting");

            for (int index = 0; index < settingsNodes.Count; index++)
            {
                var settingNode = settingsNodes[index];
                var name = settingNode?.Attributes["name"]?.InnerText;
                var match = settingCollection.AllKeys.FirstOrDefault(x => !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(x) && x.ToUpperInvariant() == name.ToUpperInvariant());
                var overrideValue = settingCollection[match];
                if (!string.IsNullOrEmpty(match) && !string.IsNullOrEmpty(overrideValue))
                {
                    settingNode.Attributes["value"].InnerText = overrideValue;
                }
            }

        }

        private IEnumerable<string> GetOverrideSettings()
        {
            return ConfigurationManager.AppSettings.AllKeys.Where(x => x.StartsWith("SitecoreSetting.")); //todo make constants
        }

        //private IEnumerable<string> GetOverrideNodeValue()
        //{
        //    return ConfigurationManager.AppSettings.AllKeys.Where(x => x.StartsWith("SitecoreNode."));
        //}
    }
}
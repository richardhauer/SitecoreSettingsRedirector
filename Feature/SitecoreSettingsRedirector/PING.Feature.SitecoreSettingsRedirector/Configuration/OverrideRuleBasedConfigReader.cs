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
        const string SitecoreSettingXpathTemplate = "/sitecore/settings/setting[@name='{0}']";

        const string VariablePrefixKey = "SitecoreVariable.";
        const string SitecoreVariableXpathTemplate = "/sitecore/sc.variable[@name='{0}']";

        const string PatchPrefixKey = "SitecorePatch.";
        const string XpathSettingEndWithKey = ".xPath";
        const string ActionSettingEndWithKey = ".Action";

        const string ActionPrefixAdd = "[add]";
        const string ActionPrefixUpdatetext = "[update]text()=";
        const string ActionPrefixUpdateAttribute = "[update]@";
        const char ActionPrefixUpdateAttributeSpliter = ':';
        const string ActionPrefixRemove = "[remove]";

        #region Sc.Variables
        protected override void ReplaceGlobalVariables(XmlNode rootNode)
        {
            if (rootNode.Name == "sitecore")
            {
                ReplaceVariblesFromAppSettings(rootNode);
            }
            base.ReplaceGlobalVariables(rootNode);
        }

        private void ReplaceVariblesFromAppSettings(XmlNode rootNode)
        {
            var overrideVariables = GetRawSitecoreVariableConfigs();
            if (overrideVariables.Any())
            {
                var variableReplacements = ParseSitecoreSettingsAndVariables(overrideVariables, VariablePrefixKey, SitecoreVariableXpathTemplate);
                ProcessReplacements(rootNode, variableReplacements);
            }
        }
        private IEnumerable<string> GetRawSitecoreVariableConfigs()
        {
            return ConfigurationManager.AppSettings.AllKeys.Where(x => x.StartsWith(VariablePrefixKey));
        }
        #endregion

        protected override void ReplaceEnvironmentVariables(XmlNode rootNode)
        {
            base.ReplaceEnvironmentVariables(rootNode);
            if (rootNode.Name == "sitecore")
            {
                ReplaceSettingsFromAppSettings(rootNode);
                ReplaceAnyValuesFromAppSettings(rootNode);
            }
        }

        private void ReplaceSettingsFromAppSettings(XmlNode rootNode)
        {
            var overrideSettings = GetRawSitecoreSettingConfigs();

            if (overrideSettings.Any())
            {
                var settingReplacements = ParseSitecoreSettingsAndVariables(overrideSettings, SettingPrefixKey, SitecoreSettingXpathTemplate);

                ProcessReplacements(rootNode, settingReplacements);
            }
        }

        private IEnumerable<XpathReplacement> ParseSitecoreSettingsAndVariables(IEnumerable<string> overrideSettings, string prefixKey, string xpathTemplate)
        {
            var settingReplacements = new List<XpathReplacement>();

            foreach (var os in overrideSettings)
            {
                var settingValue = ConfigurationManager.AppSettings[os];
                var settingKey = os.Replace(prefixKey, "");
                var settingXPath = string.Format(xpathTemplate, settingKey);
                settingReplacements.Add(new XpathReplacement
                {
                    Xpath = settingXPath,
                    Action = XpathReplacement.ActionType.UpdateAttribute,
                    AttributenName = "value",
                    NewValue = settingValue
                });
            }

            return settingReplacements;
        }

        private IEnumerable<string> GetRawSitecoreSettingConfigs()
        {
            return ConfigurationManager.AppSettings.AllKeys.Where(x => x.StartsWith(SettingPrefixKey));
        }

        private IEnumerable<string> GetRawSitecorePatchConfigs()
        {
            return ConfigurationManager.AppSettings.AllKeys.Where(x => x.StartsWith(PatchPrefixKey) && x.EndsWith(XpathSettingEndWithKey) && !x.EndsWith(ActionSettingEndWithKey));
        }

        private void ReplaceAnyValuesFromAppSettings(XmlNode rootNode)
        {
            var overrideXPathSettings = GetRawSitecorePatchConfigs();
            var settingXPathCollection = new List<XpathReplacement>();
            if (overrideXPathSettings.Any())
            {
                foreach (var oxs in overrideXPathSettings)
                {
                    var xpathPath = ConfigurationManager.AppSettings[oxs];
                    var actionKey = oxs.Replace(XpathSettingEndWithKey, "") + ActionSettingEndWithKey;
                    var rawActionString = ConfigurationManager.AppSettings[actionKey];
                    if (!string.IsNullOrEmpty(rawActionString))
                    {
                        var newReplacement = new XpathReplacement();

                        if (rawActionString.StartsWith(ActionPrefixAdd))
                        {
                            newReplacement = ProcessAddAction(xpathPath, rawActionString);
                        }
                        else if (rawActionString.StartsWith(ActionPrefixUpdatetext))
                        {
                            newReplacement = ProcessUpdateTextAction(xpathPath, rawActionString);
                        }
                        else if (rawActionString.StartsWith(ActionPrefixUpdateAttribute))
                        {
                            newReplacement = ProcessUpdateAttributeAction(xpathPath, rawActionString, newReplacement);
                        }
                        else if (rawActionString.StartsWith(ActionPrefixRemove))
                        {
                            newReplacement = ProcessRemoveAction(xpathPath);
                        }

                        settingXPathCollection.Add(newReplacement);
                    }
                }
                ProcessReplacements(rootNode, settingXPathCollection);
            }
        }

        private static XpathReplacement ProcessRemoveAction(string xpathPath)
        {
            return new XpathReplacement
            {
                Xpath = xpathPath,
                Action = XpathReplacement.ActionType.Remove
            };
        }

        private static XpathReplacement ProcessUpdateAttributeAction(string xpathPath, string rawActionString, XpathReplacement newReplacement)
        {
            var splitArray = rawActionString.Replace(ActionPrefixUpdateAttribute, "").Split(ActionPrefixUpdateAttributeSpliter);
            if (splitArray.Count() == 2)
            {
                newReplacement = new XpathReplacement
                {
                    Xpath = xpathPath,
                    Action = XpathReplacement.ActionType.UpdateAttribute,
                    AttributenName = splitArray[0],
                    NewValue = splitArray[1]
                };
            }

            return newReplacement;
        }

        private static XpathReplacement ProcessUpdateTextAction(string xpathPath, string rawActionString)
        {
            return new XpathReplacement
            {
                Xpath = xpathPath,
                Action = XpathReplacement.ActionType.UpdateText,
                NewValue = rawActionString.Replace(ActionPrefixUpdatetext, "")
            };
        }

        private static XpathReplacement ProcessAddAction(string xpathPath, string rawActionString)
        {
            XpathReplacement newReplacement;
            var newNodeXml = rawActionString.Replace(ActionPrefixAdd, "");
            newReplacement = new XpathReplacement
            {
                Xpath = xpathPath,
                Action = XpathReplacement.ActionType.Add,
                NewValue = newNodeXml
            };
            return newReplacement;
        }

        private void ProcessReplacements(XmlNode rootNode, IEnumerable<XpathReplacement> replacements)
        {
            foreach (var replacement in replacements)
            {
                var nodeToChange = rootNode.SelectSingleNode(replacement.Xpath);
                if (nodeToChange != null)
                {
                    switch (replacement.Action)
                    {
                        case XpathReplacement.ActionType.Add:
                            ProcessReplacementAdd(replacement.NewValue, nodeToChange);
                            break;
                        case XpathReplacement.ActionType.UpdateText:
                            nodeToChange.InnerText = replacement.NewValue;
                            break;
                        case XpathReplacement.ActionType.UpdateAttribute:
                            if (nodeToChange.Attributes[replacement.AttributenName] != null)
                            {
                                nodeToChange.Attributes[replacement.AttributenName].InnerText = replacement.NewValue;
                            }
                            break;
                        case XpathReplacement.ActionType.Remove:
                            nodeToChange.ParentNode.RemoveChild(nodeToChange);
                            break;
                        default:
                            break;
                    }
                    AddRuntimeMarker(nodeToChange);
                }
            }
        }

        private static void ProcessReplacementAdd(string rawNewXml, XmlNode nodeToChange)
        {
            var doc = new XmlDocument();
            doc.LoadXml(rawNewXml);
            var newNode = nodeToChange.OwnerDocument?.ImportNode(doc.DocumentElement, true);
            if (newNode != null)
            {
                nodeToChange.AppendChild(newNode);
            }
        }

        private void AddRuntimeMarker(XmlNode nodeToChange)
        {
            var mark = new XmlDocument();
            var marker = mark.CreateAttribute("source");
            marker.Value = "Runtime Override";
            nodeToChange.Attributes.SetNamedItem(marker);
        }
    }

    public class XpathReplacement
    {
        public string Xpath { get; set; }
        public ActionType Action { get; set; }
        public string AttributenName { get; set; }
        public string NewValue { get; set; }

        public enum ActionType
        {
            Add,
            UpdateText,
            UpdateAttribute,
            Remove
        }
    }
}
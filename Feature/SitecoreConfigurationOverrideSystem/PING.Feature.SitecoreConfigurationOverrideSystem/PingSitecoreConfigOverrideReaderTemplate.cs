using Sitecore.Configuration;
using Sitecore.Diagnostics;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Xml;

// This type is not being used in runtime directly, it's a build validation for the code template that's being used for creating types during runtime in PingSitecoreConfigOverrideReaderTemplate.txt
// Note: Build Action is set to 'None' - this is just a helper
namespace PING.Feature.SitecoreConfigurationOverrideSystem
{
	public class PingSitecoreConfigOverrideReader : ConfigReader // public class PingSitecoreConfigOverrideReader : $BaseTypeToken
	{
		private const string SettingPrefixKey = "SitecoreSetting.";
		private const string SitecoreSettingXpathTemplate = "/sitecore/settings/setting[@name='{0}']";

		private const string VariablePrefixKey = "SitecoreVariable.";
		private const string SitecoreVariableXpathTemplate = "/sitecore/sc.variable[@name='{0}']";

		private const string PatchPrefixKey = "SitecorePatch.";
		private const string XpathSettingEndWithKey = ".xPath";
		private const string ActionSettingEndWithKey = ".Action";

		private const string ActionPrefixAdd = "[add]";
		private const string ActionPrefixUpdatetext = "[update]text()=";
		private const string ActionPrefixUpdateAttribute = "[update]@";
		private const char ActionPrefixUpdateAttributeSpliter = ':';
		private const string ActionPrefixRemove = "[remove]";

		private const string RootSitecoreNodeName = "sitecore";
		private const string ValueAttributeName = "value";

		private const string SitecorePatchNamespace = "http://www.sitecore.net/xmlconfig/";
		private const string RuntimeMarkerAttributeName = "source";
		private const string RuntimeMarkerAttributeValue = "AppSettings Override";

		protected override void ReplaceGlobalVariables(XmlNode rootNode)
		{
			try
			{
				if (rootNode.Name == RootSitecoreNodeName)
				{
					ReplaceVariblesFromAppSettings(rootNode);
				}
			}
			catch (System.Exception ex)
			{
				Log.Warn("Error replacing variables, error message: {0}" + ex.Message, this);
			}

			base.ReplaceGlobalVariables(rootNode);
		}

		public void ReplaceVariblesFromAppSettings(XmlNode rootNode)
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

		protected override void ReplaceEnvironmentVariables(XmlNode rootNode)
		{
			base.ReplaceEnvironmentVariables(rootNode);

			try
			{
				if (rootNode.Name == RootSitecoreNodeName)
				{
					ReplaceSettingsFromAppSettings(rootNode);
					ReplaceAnyValuesFromAppSettings(rootNode);
				}
			}
			catch (System.Exception ex)
			{
				Log.Warn("Error replacing settings or patches, error message: {0}" + ex.Message, this);
			}
		}

		public void ReplaceSettingsFromAppSettings(XmlNode rootNode)
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
				settingReplacements.Add(new XpathReplacement
				{
					Xpath = string.Format(xpathTemplate, os.Replace(prefixKey, "")),
					Action = XpathReplacement.ActionType.UpdateAttribute,
					AttributenName = ValueAttributeName,
					NewValue = ConfigurationManager.AppSettings[os]
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

		public void ReplaceAnyValuesFromAppSettings(XmlNode rootNode)
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

						if (newReplacement.IsValid())
						{
							settingXPathCollection.Add(newReplacement);
						}
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
			newReplacement = new XpathReplacement
			{
				Xpath = xpathPath,
				Action = XpathReplacement.ActionType.Add,
				NewValue = rawActionString.Replace(ActionPrefixAdd, "")
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
							nodeToChange.InnerXml = replacement.NewValue;
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
			var newNode = nodeToChange.OwnerDocument.ImportNode(doc.DocumentElement, true);
			if (newNode != null)
			{
				nodeToChange.AppendChild(newNode);
			}
		}

		private void AddRuntimeMarker(XmlNode nodeToChange)
		{
			var mark = new XmlDocument( nodeToChange.OwnerDocument.NameTable );
			var marker = mark.CreateAttribute( RuntimeMarkerAttributeName, SitecorePatchNamespace );
			marker.Value = RuntimeMarkerAttributeValue;
			nodeToChange.Attributes.SetNamedItem( marker );
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

		public bool IsValid()
		{
			return !string.IsNullOrEmpty(Xpath)
					&&
					(
						(
							Action == ActionType.UpdateAttribute && !string.IsNullOrEmpty(AttributenName) && !string.IsNullOrEmpty(NewValue)
						)
						||
						(
							Action == ActionType.UpdateText && !string.IsNullOrEmpty(NewValue)
						)
						||
						(
							Action == ActionType.Add || Action == ActionType.Remove
						)
					);
		}
	}
}
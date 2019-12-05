

# PING Works - Sitecore Configuration Override System

A service that allows AppSettings to override any Sitecore configuration.

The intent of the module is to be used in Azure Web Apps to manage all per-environment settings. This reduces the burden of the release pipelines to correctly configure and repackage config files.

It allows triage and support teams ready access to the settings that require management per-environment, and importantly, keeps build artefacts immutable as they move from one environment to another.

### Setup

1. Drop PING.Feature.SitecoreConfigurationOverrideSystem.dll into website bin folder.
2. Move the value of "type" attribute of /configuration/configSections/section[name="sitecore"] in web.config to (new) "configReaderBaseType" attribute of "Sitecore" node in the configuration (Likely Sitecore.config under App_Config folder).
3. Setup the "type" attribute value to "PING.Feature.SitecoreConfigurationOverrideSystem.PingSitecoreConfigReader, PING.Feature.SitecoreConfigurationOverrideSystem" on node /configuration/configSections/section[name="sitecore"] in web.config .


#### Setup Example in Sitecore 9 and Later

Before:

     <configuration>
      <configSections>
        <section name="sitecore" type="Sitecore.Configuration.RuleBasedConfigReader, Sitecore.Kernel"/>
        ...
      </configSections>
      <sitecore /> <!--This is actually from Sitecore.config-->
      ...
    </configuration>

After:

     <configuration>
      <configSections>
        <section name="sitecore" type="PING.Feature.SitecoreConfigurationOverrideSystem.PingSitecoreConfigReader, PING.Feature.SitecoreConfigurationOverrideSystem"/>
        ...
      </configSections>
      <sitecore configReaderBaseType="Sitecore.Configuration.RuleBasedConfigReader, Sitecore.Kernel"/> <!--This is actually from Sitecore.config-->
      ...
    </configuration>

#### Setup Example Before Sitecore 9

Before:

     <configuration>
      <configSections>
        <section name="sitecore" type="Sitecore.Configuration.ConfigReader, Sitecore.Kernel"/>
        ...
      </configSections>
      <sitecore /> <!--This is actually from Sitecore.config-->
      ...
    </configuration>

After:

     <configuration>
      <configSections>
        <section name="sitecore" type="PING.Feature.SitecoreConfigurationOverrideSystem.PingSitecoreConfigReader, PING.Feature.SitecoreConfigurationOverrideSystem"/>
        ...
      </configSections>
      <sitecore configReaderBaseType="Sitecore.Configuration.ConfigReader, Sitecore.Kernel"/> <!--This is actually from Sitecore.config-->
      ...
    </configuration>

If existing type of sitecore section node is any other types such as Sitecore Support types, it should be moved over to configReaderBaseType on sitecore node just the same as above.

### Usages

In Azure AppSettings, or in the `web.config`, you can override values as follows:

#### Change Sitecore Settings

Before:

    <sitecore>
      <settings>
        <setting name="Analytics.Robots.IgnoreRobots" value="true" />
        ...
      </settings>
    </sitecore>
    
Setup:

    <configuration>
      <appSettings>
        <add key="SitecoreSetting.Analytics.Robots.IgnoreRobots" value="false" />
      </appSettings>
    </configuration>

After:

    <sitecore>
      <settings>
        <setting name="Analytics.Robots.IgnoreRobots" value="false" />
        ...
      </settings>
    </sitecore>
   
#### Change Sitecore Variables

Before:
  
    <sitecore>
        <sc.variable name="datafolder" value="somepath"/>
        ...
    </sitecore>
  
Setup:
  
    <configuration>
        <appSettings>
          <add key="SitecoreVariable.datafolder" value="new path value" />
        </appSettings>
      </configuration>

After:
  
    <sitecore>
        <sc.variable name="datafolder" value="new path value"/>
        ...
    </sitecore>
    
#### Add, Update or Remove in Sitecore configruation using xPath

Actions are always defined with a pair of app settings: 

1. SitecorePatch.[UniqeName].xPath 
2. SitecorePatch.[UniqeName].Action

[UniqeName] should match and is unique within all app settings. 

The 2 keys define the place of the changes and the action details. Examples can be found below:

##### Add 

This adds a new xml node as the last child of the matching xPath selector node

Before:

    <sitecore>
      <settings>
        <setting name="AliasesActive" value="true" />
        ...
        <setting name="ContentSearch.DateFormat" value="yyyy-MM-dd'T'HH:mm:ss'Z'" />
      </settings>
    </sitecore>

Setup:

    <configuration>
      <appSettings>
        <add key="SitecorePatch.TestAdd.xPath" value="/sitecore/settings"/>
        <add key="SitecorePatch.TestAdd.Action" value="[add]&lt;setting name='patchAddValue' value='patchAddOverrideValue' /&gt;"/>
      </appSettings>
    </configuration>

After:

    <sitecore>
      <settings>
        <setting name="AliasesActive" value="true" />
        ...
        <setting name="ContentSearch.DateFormat" value="yyyy-MM-dd'T'HH:mm:ss'Z'" />
        <setting name="patchAddValue" value="patchAddOverrideValue" />
      </settings>
    </sitecore>
    
##### Update

This updates either the text value or attribute value of the node that matches the xPath selector

**Update text value:**

Before:

    <sitecore>
        <xpathtest>
            <TestObject3 type="PING.Feature.SitecoreConfigurationOverrideSystem.Test.TestObject, PING.Feature.SitecoreConfigurationOverrideSystem.Test">
                <param name="ctorProp1">I should be overwritten</param>
                <param name="ctorProp2">$(databaseType)</param>
                <param name="to2" ref="xpathtest/TestObject4" param1="some1" param2="some2"></param>
                <Property1>prop1</Property1>
                <Property2>prop2</Property2>
            </TestObject3>
        </xpathtest>
    </sitecore>

Setup:

    <add key="SitecorePatch.TestUpdateText.xPath" value="/sitecore/xpathtest/TestObject3/param[1]"/>
    <add key="SitecorePatch.TestUpdateText.Action" value="[update]text()=patchUpdatenOverrideValue"/>

After:

    <sitecore>
        <xpathtest>
            <TestObject3 type="PING.Feature.SitecoreConfigurationOverrideSystem.Test.TestObject, PING.Feature.SitecoreConfigurationOverrideSystem.Test">
                <param name="ctorProp1">patchUpdatenOverrideValue</param>
                <param name="ctorProp2">$(databaseType)</param>
                <param name="to2" ref="xpathtest/TestObject4" param1="some1" param2="some2"></param>
                <Property1>prop1</Property1>
                <Property2>prop2</Property2>
            </TestObject3>
        </xpathtest>
    </sitecore>

Note: patchUpdatenOverrideValue can be text or valid xml
    
**Update attribute value:**

Before:

    <sitecore>
        <xpathtest>
            <TestObject3 type="PING.Feature.SitecoreConfigurationOverrideSystem.Test.TestObject, PING.Feature.SitecoreConfigurationOverrideSystem.Test">
                <param name="ctorProp1">I should be overwritten</param>
                <param name="ctorProp2">$(databaseType)</param>
                <param name="to2" ref="xpathtest/TestObject4" param1="some1" param2="some2"></param>
                <Property1>prop1</Property1>
                <Property2>prop2</Property2>
            </TestObject3>
        </xpathtest>
    </sitecore>

Setup:

    <add key="SitecorePatch.TestUpdateAttribute.xPath" value="/sitecore/xpathtest/TestObject3/param[3]"/>
    <add key="SitecorePatch.TestUpdateAttribute.Action" value="[update]@param1:patchUpdatenAttributeOverrideValue"/>

After:

    <sitecore>
        <xpathtest>
            <TestObject3 type="PING.Feature.SitecoreConfigurationOverrideSystem.Test.TestObject, PING.Feature.SitecoreConfigurationOverrideSystem.Test">
                <param name="ctorProp1">I should be overwritten</param>
                <param name="ctorProp2">$(databaseType)</param>
                <param name="to2" ref="xpathtest/TestObject4" param1="patchUpdatenAttributeOverrideValue" param2="some2"></param>
                <Property1>prop1</Property1>
                <Property2>prop2</Property2>
            </TestObject3>
        </xpathtest>
    </sitecore>

##### Remove

This will remove the node that matches the xPath selector

Before:

    <sitecore>
        <xpathtest>
            <TestObject4 type="PING.Feature.SitecoreConfigurationOverrideSystem.Test.TestObject2, PING.Feature.SitecoreConfigurationOverrideSystem.Test">
                <param name="ctorProp1">$(1)</param>
                <param name="ctorProp2">something_123</param>
                <Property1>prop3</Property1>
                <Property2>prop4</Property2>
            </TestObject4>
        </xpathtest>
    </sitecore>

Setup:

    <add key="SitecorePatch.TestRemove.xPath" value="/sitecore/xpathtest/TestObject4/Property2"/>
    <add key="SitecorePatch.TestRemove.Action" value="[remove]"/>
    
After:

    <sitecore>
        <xpathtest>
            <TestObject4 type="PING.Feature.SitecoreConfigurationOverrideSystem.Test.TestObject2, PING.Feature.SitecoreConfigurationOverrideSystem.Test">
                <param name="ctorProp1">$(1)</param>
                <param name="ctorProp2">something_123</param>
                <Property1>prop3</Property1>
            </TestObject4>
        </xpathtest>
    </sitecore>

### Troubleshoot

In /sitecore/admin/showconfig.aspx, all settings that are being overwritten are marked with source="Runtime Override" attribute in the xml node. For example:

    <sitecore xmlns:patch="http://www.sitecore.net/xmlconfig/" database="SqlServer" configReaderBaseType="Sitecore.Configuration.ConfigReader, Sitecore.Kernel">
        <sc.variable name="dataFolder" patch:source="zzDataFolder.config" value="D:\home\site\wwwroot\App_Data"/>
        <sc.variable name="mediaFolder" value="/upload"/>
        <sc.variable name="tempFolder" value="/overridevaluehere" source="Runtime Override"/>
        ...
    </sitecore>
# PING.Feature.SitecoreConfigurationOverrideSystem
A service that allows AppSettings to override any Sitecore configuration.

The intent of the module is to be used in Azure Web Apps to manage all per-environment settings. This reduces the burden of the Release pipeline to correctly configure and repackage config files.

It allows triage and support teams ready access to the settings that require management per-environment, and importantly, keeps build artefacts immutable as they move from one environment to another.

### Setup

1. Drop PING.Feature.SitecoreConfigurationOverrideSystem.dll into your website bin folder.
2. Move value of "type" attribute of /configuration/configSections/section[name="sitecore"] in web.config to (new) "configReaderBaseType" attribute of "Sitecore" nodex in the configuration.
3. Setup the "type" attribute value to "PING.Feature.SitecoreConfigurationOverrideSystem.PingSitecoreConfigReader, PING.Feature.SitecoreConfigurationOverrideSystem".


#### In Sitecore 9 and later

Before:

     <configuration>
      <configSections>
        <section name="sitecore" type="Sitecore.Configuration.RuleBasedConfigReader, Sitecore.Kernel"/>
        ...
      </configSections>
	  <sitecore />
      ...
    </configuration>

After:

     <configuration>
      <configSections>
        <section name="sitecore" type="PING.Feature.SitecoreConfigurationOverrideSystem.PingSitecoreConfigReader, PING.Feature.SitecoreConfigurationOverrideSystem"/>
        ...
      </configSections>
	  <sitecore configReaderBaseType="Sitecore.Configuration.RuleBasedConfigReader, Sitecore.Kernel"/>
      ...
    </configuration>

#### Before Sitecore 9

Before:

     <configuration>
      <configSections>
        <section name="sitecore" type="Sitecore.Configuration.ConfigReader, Sitecore.Kernel"/>
        ...
      </configSections>
	  <sitecore />
      ...
    </configuration>

After:

     <configuration>
      <configSections>
        <section name="sitecore" type="PING.Feature.SitecoreConfigurationOverrideSystem.PingSitecoreConfigReader, PING.Feature.SitecoreConfigurationOverrideSystem"/>
        ...
      </configSections>
	  <sitecore configReaderBaseType="Sitecore.Configuration.ConfigReader, Sitecore.Kernel"/>
      ...
    </configuration>

If existing type of sitecore section node is any other types such as Sitecore Support types, it should be moved over to configReaderBaseType on sitecore node similar to above.

### Basic Use

In Azure AppSettings, or in the `web.config`, you can override this value as follows:

#### Change Sitecore Settings

Target on:

    <sitecore>
      <settings>
        <setting name="Analytics.Robots.IgnoreRobots" value="true" />
      </settings>
    </sitecore>
    
Setup:

    <configuration>
      <appSettings>
      	<add key="SitecoreSetting.Analytics.Robots.IgnoreRobots" value="false" />
      </appSettings>
    </configuration>
   
#### Change Sitecore Variables

  Target on:
  
    <sitecore>
        <sc.variable name="datafolder" value="somepath"/>
    </sitecore>
  
  Setup:
  
    <configuration>
        <appSettings>
          <add key="SitecoreVariable.datafolder" value="new path value" />
        </appSettings>
      </configuration>
    
#### Add, Update, Remove any place in the xml configruation using xPath

1. Add

This adds a new xml node as the last child of the matching xPath selector node
    
    <configuration>
      <appSettings>
        <add key="SitecorePatch.TestAdd.xPath" value="/sitecore/settings"/>
        <add key="SitecorePatch.TestAdd.Action" value="[add]&lt;setting name='patchAddValue' value='patchAddOverrideValue' /&gt;"/>
      </appSettings>
    </configuration>
    
2. Update

This updates either the text value or attribute value of the node that matches the xPath selector

Update text value:

    <add key="SitecorePatch.TestUpdateText.xPath" value="/sitecore/xpathtest/TestObject3/param[1]"/>
    <add key="SitecorePatch.TestUpdateText.Action" value="[update]text()=patchUpdatenOverrideValue"/>
    
Update attribute value:

    <add key="SitecorePatch.TestUpdateAttribute.xPath" value="/sitecore/xpathtest/TestObject3/param[3]"/>
    <add key="SitecorePatch.TestUpdateAttribute.Action" value="[update]@param1:patchUpdatenAttributeOverrideValue"/>

3. Remove

This will remove the node that matches the xPath selector

    <add key="SitecorePatch.TestRemove.xPath" value="/sitecore/xpathtest/TestObject4/Property2"/>
    <add key="SitecorePatch.TestRemove.Action" value="[remove]"/>
    
### Advanced Use

In Sitecore configuration, we are able to redirect the values of elements and properties using the special attribute `ref`.
This allows us to redirect objects and properties that aren't usually found in Sitecore Settings.  This is particularly useful
for Solr Core names, where we might share a Solr across a number of environments:

- sitecore_web_index_prod
- sitecore_web_index_uat
- sitecore_web_index_dev

##### Standard Sitecore config


    <index id="sitecore_web_index" type="Sitecore.ContentSearch.SolrProvider.SolrSearchIndex, Sitecore.ContentSearch.SolrProvider" patch:source="Sitecore.ContentSearch.Solr.Index.Web.config">
      <param desc="name">$(id)</param>
      <param desc="core">$(id)</param>
      <param ref="contentSearch/indexConfigurations/databasePropertyStore" param1="$(id)" desc="propertyStore"/>
      <configuration ref="contentSearch/indexConfigurations/defaultSolrIndexConfiguration"/>
      <strategies hint="list:AddStrategy">
        <strategy ref="contentSearch/indexConfigurations/indexUpdateStrategies/onPublishEndAsyncSingleInstance"/>
      </strategies>
      <locations hint="list:AddCrawler">
        <crawler type="Sitecore.ContentSearch.SitecoreItemCrawler, Sitecore.ContentSearch">
          <Database>web</Database>
          <Root>/sitecore</Root>
        </crawler>
      </locations>
      <enableItemLanguageFallback>false</enableItemLanguageFallback>
      <enableFieldLanguageFallback>false</enableFieldLanguageFallback>
    </index>

##### Amended Sitecore config

The `ref` notation allows us to redirect the property, or the whole object, to another location in the config...

    <index id="sitecore_web_index" type="Sitecore.ContentSearch.SolrProvider.SolrSearchIndex, Sitecore.ContentSearch.SolrProvider" patch:source="Sitecore.ContentSearch.Solr.Index.Web.config">
      <param desc="name">$(id)</param>
      <param desc="core" ref="settings/setting[@name='Customer.SolrCoreNames.SitecoreWebIndex']/@value" />
      ...
    </index>

We provide a default in the Sitecore config settings.

    <sitecore>
      <settings>
        <setting name="Customer.SolrCoreNames.SitecoreWebIndex" value="sitecore_web_index" />
      </settings>
    </sitecore>

##### AppSettings Override

In the app settings, we can override as before.

    <configuration>
      <appSettings>
        <add key="SitecoreSetting.Customer.SolrCoreNames.SitecoreWebIndex" value="sitecore_web_index_prod" />
      </appSettings>
    </configuration>


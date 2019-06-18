# PING.Feature.SitecoreSettingsRedirector
A service that allows AppSettings to override Sitecore settings.

The service also alters the `ShowConfig.aspx` page, setting the `patch:source` attribute to RuntimeOverride, to allow easy identification of the settings that are affected by the module.

The intent of the module is to allow Azure Web Apps to manage all per-environment settings. This reduces the burden of the Release pipeline to correctly configure and repackage config files.
It allows triage and support teams ready access to the settings that require management per-environment, and importantly, keeps 
build artefacts immutable as they move from one environment to another.

### Setup

Open the Web.config in your Website folder and change the <section name="sitecore"> nodes type from "Sitecore.Configuration.RuleBasedConfigReader, Sitecore.Kernel" to "PING.Feature.SitecoreSettingsRedirector.Configuration.OverrideRuleBasedConfigReader, PING.Feature.SitecoreSettingsRedirector".

### Basic Use

    <sitecore>
      <settings>
        <setting name="Analytics.Robots.IgnoreRobots" value="true" />
      </settings>
    </sitecore>

In Azure AppSettings, or in the `web.config`, you can override this value as follows:

    <configuration>
      <appSettings>
        <add key="SitecoreSetting.Analytics.Robots.IgnoreRobots" value="false" />
      </appSettings>
    </configuration>


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


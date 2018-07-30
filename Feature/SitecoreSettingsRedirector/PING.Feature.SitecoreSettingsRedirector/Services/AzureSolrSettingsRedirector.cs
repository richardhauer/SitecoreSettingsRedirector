using Sitecore.ContentSearch.Abstractions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Sitecore.SecurityModel.Cryptography;
using System.Web.Configuration;
using PING.Feature.SitecoreSettingsRedirector.Abstractions;
using Sitecore.Collections;
using Sitecore.Web;
using System.Collections.Specialized;

namespace PING.Feature.SitecoreSettingsRedirector.Services
{
	public class AzureSolrSettingsRedirector : ISettings
	{
		private readonly ISitecoreStaticSettings SitecoreSettings;
		private readonly NameValueCollection AppSettings;

		public AzureSolrSettingsRedirector( ISitecoreStaticSettings sitecoreSettings, NameValueCollection appSettings )
		{
			SitecoreSettings = sitecoreSettings;
			AppSettings = appSettings;
		}

		#region Overridden methods
		public virtual string GetSetting( string name )
		{
			if ( AppSettings.AllKeys.Contains( name ) )
				return AppSettings[name];

			return SitecoreSettings.GetSetting( name );
		}
		public virtual string GetSetting( string name, string defaultValue )
		{
			if ( AppSettings.AllKeys.Contains( name ) )
				return AppSettings[name];

			return SitecoreSettings.GetSetting( name, defaultValue );
		}
		#endregion

		#region Passthrough methods
		public virtual bool AliasesActive() => SitecoreSettings.AliasesActive;
		public virtual bool AllowLogoutOfAllUsers() => SitecoreSettings.AllowLogoutOfAllUsers;
		public virtual bool AppendQSToUrlRendering() => SitecoreSettings.AppendQSToUrlRendering;
		public virtual bool AutomaticDataBind() => SitecoreSettings.AutomaticDataBind;
		public virtual bool AutomaticLockOnSave() => SitecoreSettings.AutomaticLockOnSave;
		public virtual bool AutomaticUnlockOnSaved() => SitecoreSettings.AutomaticUnlockOnSaved;
		public virtual bool AutoScheduleSmartPublish() => SitecoreSettings.AutoScheduleSmartPublish;
		public virtual bool CheckSecurity() => SitecoreSettings.CheckSecurity;
		public virtual bool CheckSecurityOnLanguages() => SitecoreSettings.CheckSecurityOnLanguages;
		public virtual string ClientLanguage() => SitecoreSettings.ClientLanguage;
		public virtual TimeSpan ClientPersistentLoginDuration() => SitecoreSettings.ClientPersistentLoginDuration;
		public virtual TimeSpan ClientSessionTimeout() => SitecoreSettings.ClientSessionTimeout;
		public virtual bool CompareRevisions() => SitecoreSettings.CompareRevisions;
		public virtual bool ConfigurationIsSet() => SitecoreSettings.ConfigurationIsSet;
		public virtual bool ConnectionStringExists( string connectionStringName ) => SitecoreSettings.ConnectionStringExists( connectionStringName );
		public virtual CustomErrorsMode CustomErrorsMode() => SitecoreSettings.CustomErrorsMode;
		public virtual string DataFolder() => SitecoreSettings.DataFolder;
		public virtual string DebugBorderColor() => SitecoreSettings.DebugBorderColor;
		public virtual string DebugBorderTag() => SitecoreSettings.DebugBorderTag;
		public virtual bool DebugEnabled() => SitecoreSettings.DebugEnabled;
		public virtual string DebugFolder() => SitecoreSettings.DebugFolder;
		public virtual string DefaultBaseTemplate() => SitecoreSettings.DefaultBaseTemplate;
		public virtual string DefaultDesktop() => SitecoreSettings.DefaultDesktop;
		public virtual string DefaultDomainName() => SitecoreSettings.DefaultDomainName;
		public virtual string DefaultIcon() => SitecoreSettings.DefaultIcon;
		public virtual string DefaultItem() => SitecoreSettings.DefaultItem;
		public virtual string DefaultLanguage() => SitecoreSettings.DefaultLanguage;
		public virtual string DefaultLayoutFile() => SitecoreSettings.DefaultLayoutFile;
		public virtual string DefaultMembershipProviderWildcard() => SitecoreSettings.DefaultMembershipProviderWildcard;
		public virtual string DefaultPageName() => SitecoreSettings.DefaultPageName;
		public virtual string DefaultPublishingTargets() => SitecoreSettings.DefaultPublishingTargets;
		public virtual string DefaultRegionalIsoCode() => SitecoreSettings.DefaultRegionalIsoCode;
		public virtual string DefaultShellControl() => SitecoreSettings.DefaultShellControl;
		public virtual int DefaultSortOrder() => SitecoreSettings.DefaultSortOrder;
		public virtual TimeSpan DefaultSQLTimeout() => SitecoreSettings.DefaultSQLTimeout;
		public virtual string DefaultTheme() => SitecoreSettings.DefaultTheme;
		public virtual string DefaultThumbnail() => SitecoreSettings.DefaultThumbnail;
		public virtual bool DisableBrowserCaching() => SitecoreSettings.DisableBrowserCaching;
		public virtual string EmailValidation() => SitecoreSettings.EmailValidation;
		public virtual bool EnableEventQueues() => SitecoreSettings.EnableEventQueues;
		public virtual bool EnableSiteConfigFiles() => SitecoreSettings.EnableSiteConfigFiles;
		public virtual bool EnableXslDocumentFunction() => SitecoreSettings.EnableXslDocumentFunction;
		public virtual bool EnableXslScripts() => SitecoreSettings.EnableXslScripts;
		public virtual string ErrorPage() => SitecoreSettings.ErrorPage;
		public virtual bool FastPublishing() => SitecoreSettings.FastPublishing;
		public virtual bool FastQueryDescendantsDisabled() => SitecoreSettings.FastQueryDescendantsDisabled;
		public virtual bool GenerateThumbnails() => SitecoreSettings.GenerateThumbnails;
		public virtual string GetAppSetting( string name ) => SitecoreSettings.GetAppSetting( name );
		public virtual string GetAppSetting( string name, string defaultValue ) => SitecoreSettings.GetAppSetting( name, defaultValue );
		public virtual bool GetBoolSetting( string name, bool defaultValue ) => SitecoreSettings.GetBoolSetting( name, defaultValue );
		public virtual string GetConnectionString( string connectionStringName ) => SitecoreSettings.GetConnectionString( connectionStringName );
		public virtual double GetDoubleSetting( string name, double defaultValue ) => SitecoreSettings.GetDoubleSetting( name, defaultValue );
		public virtual string GetFileSetting( string name ) => SitecoreSettings.GetFileSetting( name );
		public virtual string GetFileSetting( string name, string defaultValue ) => SitecoreSettings.GetFileSetting( name, defaultValue );
		public virtual int GetIntSetting( string name, int defaultValue ) => SitecoreSettings.GetIntSetting( name, defaultValue );
		public virtual long GetLongSetting( string name, long defaultValue ) => SitecoreSettings.GetLongSetting( name, defaultValue );
		public virtual int GetNumberOfSettings() => SitecoreSettings.GetNumberOfSettings;
		public virtual object GetProviderObject( string implementationPath, Type expectedType ) => SitecoreSettings.GetProviderObject( implementationPath, expectedType );
		public virtual object GetProviderObject( string referencePath, string implementationPath, Type expectedType ) => SitecoreSettings.GetProviderObject( referencePath, implementationPath, expectedType );
		public virtual object[] GetProviderObjects( string referencePath, string implementationPath, Type expectedType ) => SitecoreSettings.GetProviderObjects( referencePath, implementationPath, expectedType );
		public virtual T GetSystemSection<T>( string sectionName ) where T : class => SitecoreSettings.GetSystemSection<T>( sectionName );
		public virtual TimeSpan GetTimeSpanSetting( string name, string defaultValue ) => SitecoreSettings.GetTimeSpanSetting( name, defaultValue );
		public virtual TimeSpan GetTimeSpanSetting( string name, TimeSpan defaultValue ) => SitecoreSettings.GetTimeSpanSetting( name, defaultValue );
		public virtual TimeSpan GetTimeSpanSetting( string name, TimeSpan defaultValue, CultureInfo cultureInfo ) => SitecoreSettings.GetTimeSpanSetting( name, defaultValue, cultureInfo );
		public virtual int GridPageSize() => SitecoreSettings.GridPageSize;
		public virtual HashEncryption.EncryptionProvider HashEncryptionProvider() => SitecoreSettings.HashEncryptionProvider;
		public virtual TimeSpan HealthMonitorInterval() => SitecoreSettings.HealthMonitorInterval;
		public virtual TimeSpan HeartbeatInterval() => SitecoreSettings.HeartbeatInterval;
		public virtual string[] IgnoreUrlPrefixes() => SitecoreSettings.IgnoreUrlPrefixes;
		public virtual bool ImagesAsXhtml() => SitecoreSettings.ImagesAsXhtml;
		public virtual string ImageTypes() => SitecoreSettings.ImageTypes;
		public virtual string IndexFolder() => SitecoreSettings.IndexFolder;
		public virtual int IndexMergeFactor() => SitecoreSettings.IndexMergeFactor;
		public virtual string InstanceName() => SitecoreSettings.InstanceName;
		public virtual char[] InvalidItemNameChars() => SitecoreSettings.InvalidItemNameChars;
		public virtual string ItemNameValidation() => SitecoreSettings.ItemNameValidation;
		public virtual string ItemNotFoundUrl() => SitecoreSettings.ItemNotFoundUrl;
		public virtual bool KeepLockAfterSaveForAdminUsers() => SitecoreSettings.KeepLockAfterSaveForAdminUsers;
		public virtual string LayoutFolder() => SitecoreSettings.LayoutFolder;
		public virtual string LayoutNotFoundUrl() => SitecoreSettings.LayoutNotFoundUrl;
		public virtual string LayoutPageEvent() => SitecoreSettings.LayoutPageEvent;
		public virtual string LicenseFile() => SitecoreSettings.LicenseFile;
		public virtual string LinkItemNotFoundUrl() => SitecoreSettings.LinkItemNotFoundUrl;
		public virtual string LogFolder() => SitecoreSettings.LogFolder;
		public virtual string LoginLayout() => SitecoreSettings.LoginLayout;
		public virtual string LoginPage() => SitecoreSettings.LoginPage;
		public virtual int LogInterval() => SitecoreSettings.LogInterval;
		public virtual string MailServer() => SitecoreSettings.MailServer;
		public virtual string MailServerPassword() => SitecoreSettings.MailServerPassword;
		public virtual int MailServerPort() => SitecoreSettings.MailServerPort;
		public virtual string MailServerUserName() => SitecoreSettings.MailServerUserName;
		public virtual string MasterVariablesReplacer() => SitecoreSettings.MasterVariablesReplacer;
		public virtual int MaxCallLevel() => SitecoreSettings.MaxCallLevel;
		public virtual int MaxDocumentBufferSize() => SitecoreSettings.MaxDocumentBufferSize;
		public virtual int MaxFacets() => SitecoreSettings.MaxFacets;
		public virtual int MaxItemNameLength() => SitecoreSettings.MaxItemNameLength;
		public virtual int MaxSqlBatchStatements() => SitecoreSettings.MaxSqlBatchStatements;
		public virtual int MaxTreeDepth() => SitecoreSettings.MaxTreeDepth;
		public virtual int MaxWorkerThreads() => SitecoreSettings.MaxWorkerThreads;
		public virtual string MediaFolder() => SitecoreSettings.MediaFolder;
		public virtual string NoAccessUrl() => SitecoreSettings.NoAccessUrl;
		public virtual string NoLicenseUrl() => SitecoreSettings.NoLicenseUrl;
		public virtual string PackagePath() => SitecoreSettings.PackagePath;
		public virtual string PageStateStore() => SitecoreSettings.PageStateStore;
		public virtual bool PipelineProfilingEnabled() => SitecoreSettings.PipelineProfilingEnabled;
		public virtual bool PipelineProfilingMeasureCpuTime() => SitecoreSettings.PipelineProfilingMeasureCpuTime;
		public virtual string PortalPrincipalResolver() => SitecoreSettings.PortalPrincipalResolver;
		public virtual string PortalStorage() => SitecoreSettings.PortalStorage;
		public virtual bool ProcessDuplicatePlaceholders() => SitecoreSettings.ProcessDuplicatePlaceholders;
		public virtual int ProcessHistoryCount() => SitecoreSettings.ProcessHistoryCount;
		public virtual string ProfileItemDatabase() => SitecoreSettings.ProfileItemDatabase;
		public virtual bool ProtectedSite() => SitecoreSettings.ProtectedSite;
		public virtual int PublishDialogPollingInterval() => SitecoreSettings.PublishDialogPollingInterval;
		public virtual bool PublishEmptyItems() => SitecoreSettings.PublishEmptyItems;
		public virtual string PublishingInstance() => SitecoreSettings.PublishingInstance;
		public virtual int QueryMaxItems() => SitecoreSettings.QueryMaxItems;
		public virtual int RamBufferSize() => SitecoreSettings.RamBufferSize;
		public virtual bool RecycleBinActive() => SitecoreSettings.RecycleBinActive;
		public virtual IEnumerable<string> RedirectUrlPrefixes() => SitecoreSettings.RedirectUrlPrefixes;
		public virtual bool RequireLockBeforeEditing() => SitecoreSettings.RequireLockBeforeEditing;
		public virtual bool RequireTargetDeleteRightWhenCheckingSecurity() => SitecoreSettings.RequireTargetDeleteRightWhenCheckingSecurity;
		public virtual void Reset() => SitecoreSettings.Reset();
		public virtual bool SaveRawUrl() => SitecoreSettings.SaveRawUrl;
		public virtual string SerializationFolder() => SitecoreSettings.SerializationFolder;
		public virtual string SerializationPassword() => SitecoreSettings.SerializationPassword;
		public virtual bool SiteResolving() => SitecoreSettings.SiteResolving;
		public virtual bool SiteResolvingMatchCurrentLanguage() => SitecoreSettings.SiteResolvingMatchCurrentLanguage;
		public virtual bool SiteResolvingMatchCurrentSite() => SitecoreSettings.SiteResolvingMatchCurrentSite;
		public virtual List<SiteInfo> Sites() => SitecoreSettings.Sites;
		public virtual TimeSpan StatusUpdateInterval() => SitecoreSettings.StatusUpdateInterval;
		public virtual string SystemBlockedUrl() => SitecoreSettings.SystemBlockedUrl;
		public virtual string TempFolderPath() => SitecoreSettings.TempFolderPath;
		public virtual ThreadPriority ThreadPriority() => SitecoreSettings.ThreadPriority;
		public virtual int ThumbnailHeight() => SitecoreSettings.ThumbnailHeight;
		public virtual string ThumbnailsBackgroundColor() => SitecoreSettings.ThumbnailsBackgroundColor;
		public virtual int ThumbnailWidth() => SitecoreSettings.ThumbnailWidth;
		public virtual TimeSpan TimeBeforeStatusExpires() => SitecoreSettings.TimeBeforeStatusExpires;
		public virtual Set<string> TypesThatShouldNotBeExpanded() => SitecoreSettings.TypesThatShouldNotBeExpanded;
		public virtual bool UnlockAfterCopy() => SitecoreSettings.UnlockAfterCopy;
		public virtual string VersionFilePath() => SitecoreSettings.VersionFilePath;
		public virtual string ViewStateStore() => SitecoreSettings.ViewStateStore;
		public virtual string VirtualMembershipWildcard() => SitecoreSettings.VirtualMembershipWildcard;
		public virtual string Wallpaper() => SitecoreSettings.Wallpaper;
		public virtual string WallpapersPath() => SitecoreSettings.WallpapersPath;
		public virtual string WebStylesheet() => SitecoreSettings.WebStylesheet;
		public virtual string WelcomeTitle() => SitecoreSettings.WelcomeTitle;
		public virtual bool WriteSetting( string key, string value, bool overwrite ) => SitecoreSettings.WriteSetting( key, value, overwrite );
		public virtual bool WriteSetting( string path, string match, string content, bool overwrite ) => SitecoreSettings.WriteSetting( path, match, content, overwrite );
		public virtual string XHtmlSchemaFile() => SitecoreSettings.XHtmlSchemaFile;
		public virtual string XmlControlAspxFile() => SitecoreSettings.XmlControlAspxFile;
		public virtual string XmlControlExtension() => SitecoreSettings.XmlControlExtension;
		#endregion
	}
}

using PING.Feature.SitecoreSettingsRedirector.Abstractions;
using Sitecore.Collections;
using Sitecore.Configuration;
using Sitecore.SecurityModel.Cryptography;
using Sitecore.Web;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Web.Configuration;

#pragma warning disable CS0618 // Type or member is obsolete
namespace PING.Feature.SitecoreSettingsRedirector.Services
{
	public class SitecoreSettingsAdapter : ISitecoreStaticSettings
	{
		#region CoreSettings
		public bool AliasesActive => Settings.AliasesActive;
		public bool AllowLogoutOfAllUsers => Settings.AllowLogoutOfAllUsers;
		public bool AppendQSToUrlRendering => Settings.AppendQSToUrlRendering;
		public bool AutomaticDataBind => Settings.AutomaticDataBind;
		public bool AutomaticLockOnSave => Settings.AutomaticLockOnSave;
		public bool AutomaticUnlockOnSaved => Settings.AutomaticUnlockOnSaved;
		public bool CheckSecurityOnLanguages => Settings.CheckSecurityOnLanguages;
		public string ClientLanguage => Settings.ClientLanguage;
		public string DataFolder => Settings.DataFolder;
		public string DebugBorderColor => Settings.DebugBorderColor;
		public string DebugBorderTag => Settings.DebugBorderTag;
		public string DebugFolder => Settings.DebugFolder;
		public string DefaultBaseTemplate => Settings.DefaultBaseTemplate;
		public string DefaultDesktop => Settings.DefaultDesktop;
		public string DefaultIcon => Settings.DefaultIcon;
		public string DefaultItem => Settings.DefaultItem;
		public string DefaultLanguage => Settings.DefaultLanguage;
		public string DefaultLayoutFile => Settings.DefaultLayoutFile;
		public string DefaultPageName => Settings.DefaultPageName;
		public string DefaultRegionalIsoCode => Settings.DefaultRegionalIsoCode;
		public string DefaultShellControl => Settings.DefaultShellControl;
		public int DefaultSortOrder => Settings.DefaultSortOrder;
		public TimeSpan DefaultSQLTimeout => Settings.DefaultSQLTimeout;
		public string DefaultTheme => Settings.DefaultTheme;
		public string DefaultThumbnail => Settings.DefaultThumbnail;
		public bool DisableBrowserCaching => Settings.DisableBrowserCaching;
		public string EmailValidation => Settings.EmailValidation;
		public bool EnableEventQueues => Settings.EnableEventQueues;
		public bool EnableSiteConfigFiles => Settings.EnableSiteConfigFiles;
		public bool EnableXslDocumentFunction => Settings.EnableXslDocumentFunction;
		public bool EnableXslScripts => Settings.EnableXslScripts;
		public string ErrorPage => Settings.ErrorPage;
		public bool FastPublishing => Settings.FastPublishing;
		public bool FastQueryDescendantsDisabled => Settings.FastQueryDescendantsDisabled;
		public bool GenerateThumbnails => Settings.GenerateThumbnails;
		public int GridPageSize => Settings.GridPageSize;
		public HashEncryption.EncryptionProvider HashEncryptionProvider => Settings.HashEncryptionProvider;
		public TimeSpan HealthMonitorInterval => Settings.HealthMonitorInterval;
		public TimeSpan HeartbeatInterval => Settings.HeartbeatInterval;
		public string ImageTypes => Settings.ImageTypes;
		public string IndexFolder => Settings.IndexFolder;
		public int IndexMergeFactor => Settings.IndexMergeFactor;
		public string InstanceName => Settings.InstanceName;
		public string ItemNameValidation => Settings.ItemNameValidation;
		public string ItemNotFoundUrl => Settings.ItemNotFoundUrl;
		public bool KeepLockAfterSaveForAdminUsers => Settings.KeepLockAfterSaveForAdminUsers;
		public string LayoutFolder => Settings.LayoutFolder;
		public string LayoutNotFoundUrl => Settings.LayoutNotFoundUrl;
		public string LayoutPageEvent => Settings.LayoutPageEvent;
		public string LicenseFile => Settings.LicenseFile;
		public string LinkItemNotFoundUrl => Settings.LinkItemNotFoundUrl;
		public string LogFolder => Settings.LogFolder;
		public string LoginLayout => Settings.LoginLayout;
		public string LoginPage => Settings.LoginPage;
		public string MailServer => Settings.MailServer;
		public string MailServerPassword => Settings.MailServerPassword;
		public int MailServerPort => Settings.MailServerPort;
		public string MailServerUserName => Settings.MailServerUserName;
		public string MasterVariablesReplacer => Settings.MasterVariablesReplacer;
		public int MaxCallLevel => Settings.MaxCallLevel;
		public int MaxDocumentBufferSize => Settings.MaxDocumentBufferSize;
		public int MaxFacets => Settings.MaxFacets;
		public int MaxItemNameLength => Settings.MaxItemNameLength;
		public int MaxSqlBatchStatements => Settings.MaxSqlBatchStatements;
		public int MaxTreeDepth => Settings.MaxTreeDepth;
		public int MaxWorkerThreads => Settings.MaxWorkerThreads;
		public string MediaFolder => Settings.MediaFolder;
		public string NoAccessUrl => Settings.NoAccessUrl;
		public string NoLicenseUrl => Settings.NoLicenseUrl;
		public string PackagePath => Settings.PackagePath;
		public string PageStateStore => Settings.PageStateStore;
		public string PortalPrincipalResolver => Settings.PortalPrincipalResolver;
		public string PortalStorage => Settings.PortalStorage;
		public int ProcessHistoryCount => Settings.ProcessHistoryCount;
		public string ProfileItemDatabase => Settings.ProfileItemDatabase;
		public bool ProtectedSite => Settings.ProtectedSite;
		public int RamBufferSize => Settings.RamBufferSize;
		public bool RecycleBinActive => Settings.RecycleBinActive;
		public IEnumerable<string> RedirectUrlPrefixes => Settings.RedirectUrlPrefixes;
		public bool RequireLockBeforeEditing => Settings.RequireLockBeforeEditing;
		public string SerializationFolder => Settings.SerializationFolder;
		public string SerializationPassword => Settings.SerializationPassword;
		public string SystemBlockedUrl => Settings.SystemBlockedUrl;
		public string TempFolderPath => Settings.TempFolderPath;
		public int ThumbnailHeight => Settings.ThumbnailHeight;
		public string ThumbnailsBackgroundColor => Settings.ThumbnailsBackgroundColor;
		public int ThumbnailWidth => Settings.ThumbnailWidth;
		public bool UnlockAfterCopy => Settings.UnlockAfterCopy;
		public string VersionFilePath => Settings.VersionFilePath;
		public string ViewStateStore => Settings.ViewStateStore;
		public string Wallpaper => Settings.Wallpaper;
		public string WallpapersPath => Settings.WallpapersPath;
		public string WebStylesheet => Settings.WebStylesheet;
		public string WelcomeTitle => Settings.WelcomeTitle;
		public string XHtmlSchemaFile => Settings.XHtmlSchemaFile;
		public string XmlControlAspxFile => Settings.XmlControlAspxFile;
		public string XmlControlExtension => Settings.XmlControlExtension;
		#endregion

		#region PublishingSettings
		public bool AutoScheduleSmartPublish => Settings.Publishing.AutoScheduleSmartPublish;
		public bool CheckSecurity => Settings.Publishing.CheckSecurity;
		public bool CompareRevisions => Settings.Publishing.CompareRevisions;
		public int LogInterval => Settings.Publishing.LogInterval;
		public int PublishDialogPollingInterval => Settings.Publishing.PublishDialogPollingInterval;
		public bool PublishEmptyItems => Settings.Publishing.PublishEmptyItems;
		public string PublishingInstance => Settings.Publishing.PublishingInstance;
		public bool RequireTargetDeleteRightWhenCheckingSecurity => Settings.Publishing.RequireTargetDeleteRightWhenCheckingSecurity;
		public TimeSpan StatusUpdateInterval => Settings.Publishing.StatusUpdateInterval;
		public ThreadPriority ThreadPriority => Settings.Publishing.ThreadPriority;
		public TimeSpan TimeBeforeStatusExpires => Settings.Publishing.TimeBeforeStatusExpires;
		#endregion

		#region AuthSettings
		public TimeSpan ClientPersistentLoginDuration => Settings.Authentication.ClientPersistentLoginDuration;
		public TimeSpan ClientSessionTimeout => Settings.Authentication.ClientSessionTimeout;
		public string DefaultDomainName => Settings.Authentication.DefaultDomainName;
		public string DefaultMembershipProviderWildcard => Settings.Authentication.DefaultMembershipProviderWildcard;
		public bool SaveRawUrl => Settings.Authentication.SaveRawUrl;
		public string VirtualMembershipWildcard => Settings.Authentication.VirtualMembershipWildcard;
		#endregion

		#region RenderingSettings
		public bool ImagesAsXhtml => Settings.Rendering.ImagesAsXhtml;
		public bool ProcessDuplicatePlaceholders => Settings.Rendering.ProcessDuplicatePlaceholders;
		public bool SiteResolving => Settings.Rendering.SiteResolving;
		public bool SiteResolvingMatchCurrentLanguage => Settings.Rendering.SiteResolvingMatchCurrentLanguage;
		public bool SiteResolvingMatchCurrentSite => Settings.Rendering.SiteResolvingMatchCurrentSite;
		public Set<string> TypesThatShouldNotBeExpanded => Settings.Rendering.TypesThatShouldNotBeExpanded;
		#endregion

		#region Pipeline Settings
		public bool PipelineProfilingEnabled => Settings.Pipelines.Profiling.Enabled;
		public bool PipelineProfilingMeasureCpuTime => Settings.Pipelines.Profiling.MeasureCpuTime;
		#endregion

		#region Query Settings
		public int QueryMaxItems => Settings.Query.MaxItems;
		#endregion

		#region Other Settings
		public bool ConfigurationIsSet => Settings.ConfigurationIsSet;
		public bool ConnectionStringExists( string connectionStringName ) => Settings.ConnectionStringExists( connectionStringName );
		public CustomErrorsMode CustomErrorsMode => Settings.CustomErrorsMode;
		public bool DebugEnabled => Settings.DebugEnabled;
		public int GetNumberOfSettings => Settings.GetNumberOfSettings();
		public string DefaultPublishingTargets => Settings.DefaultPublishingTargets;
		public string[] IgnoreUrlPrefixes => Settings.IgnoreUrlPrefixes;
		public char[] InvalidItemNameChars => Settings.InvalidItemNameChars;
		public List<SiteInfo> Sites => Settings.Sites;
		#endregion

		#region Typed retrieval methods
		public string GetAppSetting( string name ) => Settings.GetAppSetting( name );
		public string GetAppSetting( string name, string defaultValue ) => Settings.GetAppSetting( name, defaultValue );
		public bool GetBoolSetting( string name, bool defaultValue ) => Settings.GetBoolSetting( name, defaultValue );
		public string GetConnectionString( string connectionStringName ) => Settings.GetConnectionString( connectionStringName );
		public double GetDoubleSetting( string name, double defaultValue ) => Settings.GetDoubleSetting( name, defaultValue );
		public string GetFileSetting( string name ) => Settings.GetFileSetting( name );
		public string GetFileSetting( string name, string defaultValue ) => Settings.GetFileSetting( name, defaultValue );
		public int GetIntSetting( string name, int defaultValue ) => Settings.GetIntSetting( name, defaultValue );
		public long GetLongSetting( string name, long defaultValue ) => Settings.GetLongSetting( name, defaultValue );
		public TimeSpan GetTimeSpanSetting( string name, string defaultValue ) => Settings.GetTimeSpanSetting( name, defaultValue );
		public TimeSpan GetTimeSpanSetting( string name, TimeSpan defaultValue ) => Settings.GetTimeSpanSetting( name, defaultValue );
		public TimeSpan GetTimeSpanSetting( string name, TimeSpan defaultValue, CultureInfo cultureInfo ) => Settings.GetTimeSpanSetting( name, defaultValue, cultureInfo );
		#endregion

		#region Untyped retrieval methods
		public object GetProviderObject( string implementationPath, Type expectedType ) => Settings.GetProviderObject( implementationPath, expectedType );
		public object GetProviderObject( string referencePath, string implementationPath, Type expectedType ) => Settings.GetProviderObject( referencePath, implementationPath, expectedType );
		public object[] GetProviderObjects( string referencePath, string implementationPath, Type expectedType ) => Settings.GetProviderObjects( referencePath, implementationPath, expectedType );
		public string GetSetting( string name ) => Settings.GetSetting( name );
		public string GetSetting( string name, string defaultValue ) => Settings.GetSetting( name, defaultValue );
		#endregion

		#region Settings Methods
		public T GetSystemSection<T>( string sectionName ) where T : class => Settings.GetSystemSection<T>( sectionName );
		public void Reset() => Settings.Reset();
		public bool WriteSetting( string key, string value, bool overwrite ) => Settings.WriteSetting( key, value, overwrite );
		public bool WriteSetting( string path, string match, string content, bool overwrite ) => Settings.WriteSetting( path, match, content, overwrite );
		#endregion
	}
}
#pragma warning restore CS0618 // Type or member is obsolete
using Sitecore.Collections;
using Sitecore.SecurityModel.Cryptography;
using Sitecore.Web;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace PING.Feature.SitecoreSettingsRedirector.Abstractions
{
	public interface ISitecoreStaticSettings
	{
		bool AliasesActive { get; }
		bool AllowLogoutOfAllUsers { get; }
		bool AppendQSToUrlRendering { get; }
		bool AutomaticDataBind { get; }
		bool AutomaticLockOnSave { get; }
		bool AutomaticUnlockOnSaved { get; }
		bool AutoScheduleSmartPublish { get; }
		bool CheckSecurity { get; }
		bool CheckSecurityOnLanguages { get; }
		string ClientLanguage { get; }
		TimeSpan ClientPersistentLoginDuration { get; }
		TimeSpan ClientSessionTimeout { get; }
		bool CompareRevisions { get; }
		bool ConfigurationIsSet { get; }
		CustomErrorsMode CustomErrorsMode { get; }
		string DataFolder { get; }
		string DebugBorderColor { get; }
		string DebugBorderTag { get; }
		bool DebugEnabled { get; }
		string DebugFolder { get; }
		string DefaultBaseTemplate { get; }
		string DefaultDesktop { get; }
		string DefaultDomainName { get; }
		string DefaultIcon { get; }
		string DefaultItem { get; }
		string DefaultLanguage { get; }
		string DefaultLayoutFile { get; }
		string DefaultMembershipProviderWildcard { get; }
		string DefaultPageName { get; }
		string DefaultPublishingTargets { get; }
		string DefaultRegionalIsoCode { get; }
		string DefaultShellControl { get; }
		int DefaultSortOrder { get; }
		TimeSpan DefaultSQLTimeout { get; }
		string DefaultTheme { get; }
		string DefaultThumbnail { get; }
		bool DisableBrowserCaching { get; }
		string EmailValidation { get; }
		bool EnableEventQueues { get; }
		bool EnableSiteConfigFiles { get; }
		bool EnableXslDocumentFunction { get; }
		bool EnableXslScripts { get; }
		string ErrorPage { get; }
		bool FastPublishing { get; }
		bool FastQueryDescendantsDisabled { get; }
		bool GenerateThumbnails { get; }
		int GetNumberOfSettings { get; }
		int GridPageSize { get; }
		HashEncryption.EncryptionProvider HashEncryptionProvider { get; }
		TimeSpan HealthMonitorInterval { get; }
		TimeSpan HeartbeatInterval { get; }
		string[] IgnoreUrlPrefixes { get; }
		bool ImagesAsXhtml { get; }
		string ImageTypes { get; }
		string IndexFolder { get; }
		int IndexMergeFactor { get; }
		string InstanceName { get; }
		char[] InvalidItemNameChars { get; }
		string ItemNameValidation { get; }
		string ItemNotFoundUrl { get; }
		bool KeepLockAfterSaveForAdminUsers { get; }
		string LayoutFolder { get; }
		string LayoutNotFoundUrl { get; }
		string LayoutPageEvent { get; }
		string LicenseFile { get; }
		string LinkItemNotFoundUrl { get; }
		string LogFolder { get; }
		string LoginLayout { get; }
		string LoginPage { get; }
		int LogInterval { get; }
		string MailServer { get; }
		string MailServerPassword { get; }
		int MailServerPort { get; }
		string MailServerUserName { get; }
		string MasterVariablesReplacer { get; }
		int MaxCallLevel { get; }
		int MaxDocumentBufferSize { get; }
		int MaxFacets { get; }
		int MaxItemNameLength { get; }
		int MaxSqlBatchStatements { get; }
		int MaxTreeDepth { get; }
		int MaxWorkerThreads { get; }
		string MediaFolder { get; }
		string NoAccessUrl { get; }
		string NoLicenseUrl { get; }
		string PackagePath { get; }
		string PageStateStore { get; }
		bool PipelineProfilingEnabled { get; }
		bool PipelineProfilingMeasureCpuTime { get; }
		string PortalPrincipalResolver { get; }
		string PortalStorage { get; }
		bool ProcessDuplicatePlaceholders { get; }
		int ProcessHistoryCount { get; }
		string ProfileItemDatabase { get; }
		bool ProtectedSite { get; }
		int PublishDialogPollingInterval { get; }
		bool PublishEmptyItems { get; }
		string PublishingInstance { get; }
		int QueryMaxItems { get; }
		int RamBufferSize { get; }
		bool RecycleBinActive { get; }
		IEnumerable<string> RedirectUrlPrefixes { get; }
		bool RequireLockBeforeEditing { get; }
		bool RequireTargetDeleteRightWhenCheckingSecurity { get; }
		bool SaveRawUrl { get; }
		string SerializationFolder { get; }
		string SerializationPassword { get; }
		bool SiteResolving { get; }
		bool SiteResolvingMatchCurrentLanguage { get; }
		bool SiteResolvingMatchCurrentSite { get; }
		List<SiteInfo> Sites { get; }
		TimeSpan StatusUpdateInterval { get; }
		string SystemBlockedUrl { get; }
		string TempFolderPath { get; }
		ThreadPriority ThreadPriority { get; }
		int ThumbnailHeight { get; }
		string ThumbnailsBackgroundColor { get; }
		int ThumbnailWidth { get; }
		TimeSpan TimeBeforeStatusExpires { get; }
		Set<string> TypesThatShouldNotBeExpanded { get; }
		bool UnlockAfterCopy { get; }
		string VersionFilePath { get; }
		string ViewStateStore { get; }
		string VirtualMembershipWildcard { get; }
		string Wallpaper { get; }
		string WallpapersPath { get; }
		string WebStylesheet { get; }
		string WelcomeTitle { get; }
		string XHtmlSchemaFile { get; }
		string XmlControlAspxFile { get; }
		string XmlControlExtension { get; }

		bool ConnectionStringExists( string connectionStringName );
		string GetAppSetting( string name );
		string GetAppSetting( string name, string defaultValue );
		bool GetBoolSetting( string name, bool defaultValue );
		string GetConnectionString( string connectionStringName );
		double GetDoubleSetting( string name, double defaultValue );
		string GetFileSetting( string name );
		string GetFileSetting( string name, string defaultValue );
		int GetIntSetting( string name, int defaultValue );
		long GetLongSetting( string name, long defaultValue );
		object GetProviderObject( string referencePath, string implementationPath, Type expectedType );
		object GetProviderObject( string implementationPath, Type expectedType );
		object[] GetProviderObjects( string referencePath, string implementationPath, Type expectedType );
		string GetSetting( string name );
		string GetSetting( string name, string defaultValue );
		T GetSystemSection<T>( string sectionName ) where T : class;
		TimeSpan GetTimeSpanSetting( string name, string defaultValue );
		TimeSpan GetTimeSpanSetting( string name, TimeSpan defaultValue );
		TimeSpan GetTimeSpanSetting( string name, TimeSpan defaultValue, CultureInfo cultureInfo );
		void Reset();
		bool WriteSetting( string key, string value, bool overwrite );
		bool WriteSetting( string path, string match, string content, bool overwrite );

	}
}

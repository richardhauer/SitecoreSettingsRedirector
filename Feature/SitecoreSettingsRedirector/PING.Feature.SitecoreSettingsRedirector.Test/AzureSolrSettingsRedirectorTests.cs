using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PING.Feature.SitecoreSettingsRedirector.Abstractions;
using PING.Feature.SitecoreSettingsRedirector.Services;
using Sitecore.ContentSearch.Abstractions;
using Sitecore.SecurityModel.Cryptography;

namespace PING.Feature.SitecoreSettingsRedirector.Test
{
	[TestClass]
	public class AzureSolrSettingsRedirectorTests
	{
		private class SettingClass { }

		private const string OverrideSettingKey = "test1";
		private const string OverrideSettingValue = "app settings value";
		private const string SitecoreSettingValue = "sitecore value";
		private const string SitecoreSettingKey = "test2";
		private Dictionary<string,bool> CallbackTable;
		private AzureSolrSettingsRedirector Service;

		[TestInitialize]
		public void Setup()
		{
			CallbackTable = new Dictionary<string, bool>();

			var mockSitecoreSettings = new Mock<ISitecoreStaticSettings>();
			SetupMockPropertiesAndMethodsWithCallbackTracking<SettingClass>( mockSitecoreSettings );

			var overrideSettings = new NameValueCollection{ { OverrideSettingKey, OverrideSettingValue } };

			Service = new AzureSolrSettingsRedirector( mockSitecoreSettings.Object, overrideSettings );
		}
		private void SetupMockPropertiesAndMethodsWithCallbackTracking<T>( Mock<ISitecoreStaticSettings> mockSitecoreSettings ) where T : class
		{
			// have to setup in the order: least specific -> most specific
			mockSitecoreSettings.Setup( s => s.GetSetting( It.IsAny<string>() ) ).Callback( () => { CallbackTable["GetSetting_string"] = true; } );
			mockSitecoreSettings.Setup( s => s.GetSetting( It.IsAny<string>(), It.IsAny<string>() ) ).Callback( () => { CallbackTable["GetSetting_string_string"] = true; } ).Returns<string, string>( ( key, def ) => def );

			mockSitecoreSettings.Setup( s => s.GetSetting( OverrideSettingKey ) ).Returns( SitecoreSettingValue );
			mockSitecoreSettings.Setup( s => s.GetSetting( OverrideSettingKey, It.IsAny<string>() ) ).Returns( SitecoreSettingValue );

			mockSitecoreSettings.Setup( s => s.GetSetting( SitecoreSettingKey ) ).Returns( SitecoreSettingValue );
			mockSitecoreSettings.Setup( s => s.GetSetting( SitecoreSettingKey, It.IsAny<string>() ) ).Returns( SitecoreSettingValue );


			// simple passthrough
			mockSitecoreSettings.SetupGet( s => s.AliasesActive ).Callback( () => { CallbackTable["AliasesActive"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.AllowLogoutOfAllUsers ).Callback( () => { CallbackTable["AllowLogoutOfAllUsers"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.AppendQSToUrlRendering ).Callback( () => { CallbackTable["AppendQSToUrlRendering"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.AutomaticDataBind ).Callback( () => { CallbackTable["AutomaticDataBind"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.AutomaticLockOnSave ).Callback( () => { CallbackTable["AutomaticLockOnSave"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.AutomaticUnlockOnSaved ).Callback( () => { CallbackTable["AutomaticUnlockOnSaved"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.AutoScheduleSmartPublish ).Callback( () => { CallbackTable["AutoScheduleSmartPublish"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.CheckSecurity ).Callback( () => { CallbackTable["CheckSecurity"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.CheckSecurityOnLanguages ).Callback( () => { CallbackTable["CheckSecurityOnLanguages"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.ClientLanguage ).Callback( () => { CallbackTable["ClientLanguage"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.ClientPersistentLoginDuration ).Callback( () => { CallbackTable["ClientPersistentLoginDuration"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.ClientSessionTimeout ).Callback( () => { CallbackTable["ClientSessionTimeout"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.CompareRevisions ).Callback( () => { CallbackTable["CompareRevisions"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.ConfigurationIsSet ).Callback( () => { CallbackTable["ConfigurationIsSet"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.CustomErrorsMode ).Callback( () => { CallbackTable["CustomErrorsMode"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.DataFolder ).Callback( () => { CallbackTable["DataFolder"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.DebugBorderColor ).Callback( () => { CallbackTable["DebugBorderColor"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.DebugBorderTag ).Callback( () => { CallbackTable["DebugBorderTag"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.DebugEnabled ).Callback( () => { CallbackTable["DebugEnabled"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.DebugFolder ).Callback( () => { CallbackTable["DebugFolder"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.DefaultBaseTemplate ).Callback( () => { CallbackTable["DefaultBaseTemplate"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.DefaultDesktop ).Callback( () => { CallbackTable["DefaultDesktop"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.DefaultDomainName ).Callback( () => { CallbackTable["DefaultDomainName"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.DefaultIcon ).Callback( () => { CallbackTable["DefaultIcon"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.DefaultItem ).Callback( () => { CallbackTable["DefaultItem"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.DefaultLanguage ).Callback( () => { CallbackTable["DefaultLanguage"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.DefaultLayoutFile ).Callback( () => { CallbackTable["DefaultLayoutFile"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.DefaultMembershipProviderWildcard ).Callback( () => { CallbackTable["DefaultMembershipProviderWildcard"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.DefaultPageName ).Callback( () => { CallbackTable["DefaultPageName"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.DefaultPublishingTargets ).Callback( () => { CallbackTable["DefaultPublishingTargets"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.DefaultRegionalIsoCode ).Callback( () => { CallbackTable["DefaultRegionalIsoCode"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.DefaultShellControl ).Callback( () => { CallbackTable["DefaultShellControl"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.DefaultSortOrder ).Callback( () => { CallbackTable["DefaultSortOrder"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.DefaultSQLTimeout ).Callback( () => { CallbackTable["DefaultSQLTimeout"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.DefaultTheme ).Callback( () => { CallbackTable["DefaultTheme"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.DefaultThumbnail ).Callback( () => { CallbackTable["DefaultThumbnail"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.DisableBrowserCaching ).Callback( () => { CallbackTable["DisableBrowserCaching"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.EmailValidation ).Callback( () => { CallbackTable["EmailValidation"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.EnableEventQueues ).Callback( () => { CallbackTable["EnableEventQueues"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.EnableSiteConfigFiles ).Callback( () => { CallbackTable["EnableSiteConfigFiles"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.EnableXslDocumentFunction ).Callback( () => { CallbackTable["EnableXslDocumentFunction"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.EnableXslScripts ).Callback( () => { CallbackTable["EnableXslScripts"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.ErrorPage ).Callback( () => { CallbackTable["ErrorPage"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.FastPublishing ).Callback( () => { CallbackTable["FastPublishing"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.FastQueryDescendantsDisabled ).Callback( () => { CallbackTable["FastQueryDescendantsDisabled"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.GenerateThumbnails ).Callback( () => { CallbackTable["GenerateThumbnails"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.GetNumberOfSettings ).Callback( () => { CallbackTable["GetNumberOfSettings"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.GridPageSize ).Callback( () => { CallbackTable["GridPageSize"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.HashEncryptionProvider ).Callback( () => { CallbackTable["HashEncryptionProvider"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.HealthMonitorInterval ).Callback( () => { CallbackTable["HealthMonitorInterval"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.HeartbeatInterval ).Callback( () => { CallbackTable["HeartbeatInterval"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.IgnoreUrlPrefixes ).Callback( () => { CallbackTable["IgnoreUrlPrefixes"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.ImagesAsXhtml ).Callback( () => { CallbackTable["ImagesAsXhtml"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.ImageTypes ).Callback( () => { CallbackTable["ImageTypes"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.IndexFolder ).Callback( () => { CallbackTable["IndexFolder"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.IndexMergeFactor ).Callback( () => { CallbackTable["IndexMergeFactor"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.InstanceName ).Callback( () => { CallbackTable["InstanceName"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.InvalidItemNameChars ).Callback( () => { CallbackTable["InvalidItemNameChars"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.ItemNameValidation ).Callback( () => { CallbackTable["ItemNameValidation"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.ItemNotFoundUrl ).Callback( () => { CallbackTable["ItemNotFoundUrl"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.KeepLockAfterSaveForAdminUsers ).Callback( () => { CallbackTable["KeepLockAfterSaveForAdminUsers"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.LayoutFolder ).Callback( () => { CallbackTable["LayoutFolder"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.LayoutNotFoundUrl ).Callback( () => { CallbackTable["LayoutNotFoundUrl"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.LayoutPageEvent ).Callback( () => { CallbackTable["LayoutPageEvent"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.LicenseFile ).Callback( () => { CallbackTable["LicenseFile"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.LinkItemNotFoundUrl ).Callback( () => { CallbackTable["LinkItemNotFoundUrl"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.LogFolder ).Callback( () => { CallbackTable["LogFolder"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.LoginLayout ).Callback( () => { CallbackTable["LoginLayout"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.LoginPage ).Callback( () => { CallbackTable["LoginPage"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.LogInterval ).Callback( () => { CallbackTable["LogInterval"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.MailServer ).Callback( () => { CallbackTable["MailServer"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.MailServerPassword ).Callback( () => { CallbackTable["MailServerPassword"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.MailServerPort ).Callback( () => { CallbackTable["MailServerPort"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.MailServerUserName ).Callback( () => { CallbackTable["MailServerUserName"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.MasterVariablesReplacer ).Callback( () => { CallbackTable["MasterVariablesReplacer"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.MaxCallLevel ).Callback( () => { CallbackTable["MaxCallLevel"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.MaxDocumentBufferSize ).Callback( () => { CallbackTable["MaxDocumentBufferSize"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.MaxFacets ).Callback( () => { CallbackTable["MaxFacets"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.MaxItemNameLength ).Callback( () => { CallbackTable["MaxItemNameLength"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.MaxSqlBatchStatements ).Callback( () => { CallbackTable["MaxSqlBatchStatements"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.MaxTreeDepth ).Callback( () => { CallbackTable["MaxTreeDepth"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.MaxWorkerThreads ).Callback( () => { CallbackTable["MaxWorkerThreads"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.MediaFolder ).Callback( () => { CallbackTable["MediaFolder"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.NoAccessUrl ).Callback( () => { CallbackTable["NoAccessUrl"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.NoLicenseUrl ).Callback( () => { CallbackTable["NoLicenseUrl"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.PackagePath ).Callback( () => { CallbackTable["PackagePath"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.PageStateStore ).Callback( () => { CallbackTable["PageStateStore"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.PipelineProfilingEnabled ).Callback( () => { CallbackTable["PipelineProfilingEnabled"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.PipelineProfilingMeasureCpuTime ).Callback( () => { CallbackTable["PipelineProfilingMeasureCpuTime"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.PortalPrincipalResolver ).Callback( () => { CallbackTable["PortalPrincipalResolver"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.PortalStorage ).Callback( () => { CallbackTable["PortalStorage"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.ProcessDuplicatePlaceholders ).Callback( () => { CallbackTable["ProcessDuplicatePlaceholders"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.ProcessHistoryCount ).Callback( () => { CallbackTable["ProcessHistoryCount"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.ProfileItemDatabase ).Callback( () => { CallbackTable["ProfileItemDatabase"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.ProtectedSite ).Callback( () => { CallbackTable["ProtectedSite"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.PublishDialogPollingInterval ).Callback( () => { CallbackTable["PublishDialogPollingInterval"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.PublishEmptyItems ).Callback( () => { CallbackTable["PublishEmptyItems"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.PublishingInstance ).Callback( () => { CallbackTable["PublishingInstance"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.QueryMaxItems ).Callback( () => { CallbackTable["QueryMaxItems"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.RamBufferSize ).Callback( () => { CallbackTable["RamBufferSize"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.RecycleBinActive ).Callback( () => { CallbackTable["RecycleBinActive"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.RedirectUrlPrefixes ).Callback( () => { CallbackTable["RedirectUrlPrefixes"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.RequireLockBeforeEditing ).Callback( () => { CallbackTable["RequireLockBeforeEditing"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.RequireTargetDeleteRightWhenCheckingSecurity ).Callback( () => { CallbackTable["RequireTargetDeleteRightWhenCheckingSecurity"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.SaveRawUrl ).Callback( () => { CallbackTable["SaveRawUrl"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.SerializationFolder ).Callback( () => { CallbackTable["SerializationFolder"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.SerializationPassword ).Callback( () => { CallbackTable["SerializationPassword"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.SiteResolving ).Callback( () => { CallbackTable["SiteResolving"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.SiteResolvingMatchCurrentLanguage ).Callback( () => { CallbackTable["SiteResolvingMatchCurrentLanguage"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.SiteResolvingMatchCurrentSite ).Callback( () => { CallbackTable["SiteResolvingMatchCurrentSite"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.Sites ).Callback( () => { CallbackTable["Sites"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.StatusUpdateInterval ).Callback( () => { CallbackTable["StatusUpdateInterval"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.SystemBlockedUrl ).Callback( () => { CallbackTable["SystemBlockedUrl"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.TempFolderPath ).Callback( () => { CallbackTable["TempFolderPath"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.ThreadPriority ).Callback( () => { CallbackTable["ThreadPriority"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.ThumbnailHeight ).Callback( () => { CallbackTable["ThumbnailHeight"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.ThumbnailsBackgroundColor ).Callback( () => { CallbackTable["ThumbnailsBackgroundColor"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.ThumbnailWidth ).Callback( () => { CallbackTable["ThumbnailWidth"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.TimeBeforeStatusExpires ).Callback( () => { CallbackTable["TimeBeforeStatusExpires"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.TypesThatShouldNotBeExpanded ).Callback( () => { CallbackTable["TypesThatShouldNotBeExpanded"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.UnlockAfterCopy ).Callback( () => { CallbackTable["UnlockAfterCopy"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.VersionFilePath ).Callback( () => { CallbackTable["VersionFilePath"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.ViewStateStore ).Callback( () => { CallbackTable["ViewStateStore"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.VirtualMembershipWildcard ).Callback( () => { CallbackTable["VirtualMembershipWildcard"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.Wallpaper ).Callback( () => { CallbackTable["Wallpaper"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.WallpapersPath ).Callback( () => { CallbackTable["WallpapersPath"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.WebStylesheet ).Callback( () => { CallbackTable["WebStylesheet"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.WelcomeTitle ).Callback( () => { CallbackTable["WelcomeTitle"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.XHtmlSchemaFile ).Callback( () => { CallbackTable["XHtmlSchemaFile"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.XmlControlAspxFile ).Callback( () => { CallbackTable["XmlControlAspxFile"] = true; } );
			mockSitecoreSettings.SetupGet( s => s.XmlControlExtension ).Callback( () => { CallbackTable["XmlControlExtension"] = true; } );

			mockSitecoreSettings.Setup( s => s.ConnectionStringExists( It.IsAny<string>() ) ).Callback( () => { CallbackTable["ConnectionStringExists_string"] = true; } );
			mockSitecoreSettings.Setup( s => s.GetAppSetting( It.IsAny<string>() ) ).Callback( () => { CallbackTable["GetAppSetting_string"] = true; } );
			mockSitecoreSettings.Setup( s => s.GetAppSetting( It.IsAny<string>(), It.IsAny<string>() ) ).Callback( () => { CallbackTable["GetAppSetting_string_string"] = true; } );
			mockSitecoreSettings.Setup( s => s.GetBoolSetting( It.IsAny<string>(), It.IsAny<bool>() ) ).Callback( () => { CallbackTable["GetBoolSetting_string_bool"] = true; } );
			mockSitecoreSettings.Setup( s => s.GetConnectionString( It.IsAny<string>() ) ).Callback( () => { CallbackTable["GetConnectionString_string"] = true; } );
			mockSitecoreSettings.Setup( s => s.GetDoubleSetting( It.IsAny<string>(), It.IsAny<double>() ) ).Callback( () => { CallbackTable["GetDoubleSetting_string_double"] = true; } );
			mockSitecoreSettings.Setup( s => s.GetFileSetting( It.IsAny<string>() ) ).Callback( () => { CallbackTable["GetFileSetting_string"] = true; } );
			mockSitecoreSettings.Setup( s => s.GetFileSetting( It.IsAny<string>(), It.IsAny<string>() ) ).Callback( () => { CallbackTable["GetFileSetting_string_string"] = true; } );
			mockSitecoreSettings.Setup( s => s.GetIntSetting( It.IsAny<string>(), It.IsAny<int>() ) ).Callback( () => { CallbackTable["GetIntSetting_string_int"] = true; } );
			mockSitecoreSettings.Setup( s => s.GetLongSetting( It.IsAny<string>(), It.IsAny<long>() ) ).Callback( () => { CallbackTable["GetLongSetting_string_long"] = true; } );
			mockSitecoreSettings.Setup( s => s.GetProviderObject( It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Type>() ) ).Callback( () => { CallbackTable["GetProviderObject_string_string_type"] = true; } );
			mockSitecoreSettings.Setup( s => s.GetProviderObject( It.IsAny<string>(), It.IsAny<Type>() ) ).Callback( () => { CallbackTable["GetProviderObject_string_type"] = true; } );
			mockSitecoreSettings.Setup( s => s.GetProviderObjects( It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Type>() ) ).Callback( () => { CallbackTable["GetProviderObjects_string_string_type"] = true; } );
			mockSitecoreSettings.Setup( s => s.GetSystemSection<T>( It.IsAny<string>() ) ).Callback( () => { CallbackTable["GetSystemSection_string"] = true; } );
			mockSitecoreSettings.Setup( s => s.GetTimeSpanSetting( It.IsAny<string>(), It.IsAny<string>() ) ).Callback( () => { CallbackTable["GetTimeSpanSetting_string_string"] = true; } );
			mockSitecoreSettings.Setup( s => s.GetTimeSpanSetting( It.IsAny<string>(), It.IsAny<TimeSpan>() ) ).Callback( () => { CallbackTable["GetTimeSpanSetting_string_timespan"] = true; } );
			mockSitecoreSettings.Setup( s => s.GetTimeSpanSetting( It.IsAny<string>(), It.IsAny<TimeSpan>(), It.IsAny<CultureInfo>() ) ).Callback( () => { CallbackTable["GetTimeSpanSetting_string_timespan_cultureinfo"] = true; } );
			mockSitecoreSettings.Setup( s => s.Reset() ).Callback( () => { CallbackTable["Reset"] = true; } );
			mockSitecoreSettings.Setup( s => s.WriteSetting( It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>() ) ).Callback( () => { CallbackTable["WriteSetting_string_string_bool"] = true; } );
			mockSitecoreSettings.Setup( s => s.WriteSetting( It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>() ) ).Callback( () => { CallbackTable["WriteSetting_string_string_string_bool"] = true; } );
		}
		private bool IsCallbackCalled( string memberName )
		{
			return CallbackTable.ContainsKey( memberName ) && CallbackTable[memberName];
		}

		[TestMethod]
		public void AzureSolrSettingsRedirector_Interfaces()
		{
			Assert.IsInstanceOfType( Service, typeof( ISettings ) );
		}

		[TestMethod]
		public void AzureSolrSettingsRedirector_PassthroughMethods()
		{
			Service.AliasesActive();
			Service.AllowLogoutOfAllUsers();
			Service.AppendQSToUrlRendering();
			Service.AutoScheduleSmartPublish();
			Service.AutomaticDataBind();
			Service.AutomaticLockOnSave();
			Service.AutomaticUnlockOnSaved();
			Service.CheckSecurity();
			Service.CheckSecurityOnLanguages();
			Service.ClientLanguage();
			Service.ClientPersistentLoginDuration();
			Service.ClientSessionTimeout();
			Service.CompareRevisions();
			Service.ConfigurationIsSet();
			Service.CustomErrorsMode();
			Service.DataFolder();
			Service.DebugBorderColor();
			Service.DebugBorderTag();
			Service.DebugEnabled();
			Service.DebugFolder();
			Service.DefaultBaseTemplate();
			Service.DefaultDesktop();
			Service.DefaultDomainName();
			Service.DefaultIcon();
			Service.DefaultItem();
			Service.DefaultLanguage();
			Service.DefaultLayoutFile();
			Service.DefaultMembershipProviderWildcard();
			Service.DefaultPageName();
			Service.DefaultPublishingTargets();
			Service.DefaultRegionalIsoCode();
			Service.DefaultSQLTimeout();
			Service.DefaultShellControl();
			Service.DefaultSortOrder();
			Service.DefaultTheme();
			Service.DefaultThumbnail();
			Service.DisableBrowserCaching();
			Service.EmailValidation();
			Service.EnableEventQueues();
			Service.EnableSiteConfigFiles();
			Service.EnableXslDocumentFunction();
			Service.EnableXslScripts();
			Service.ErrorPage();
			Service.FastPublishing();
			Service.FastQueryDescendantsDisabled();
			Service.GenerateThumbnails();
			Service.GetNumberOfSettings();
			Service.GridPageSize();
			Service.HashEncryptionProvider();
			Service.HealthMonitorInterval();
			Service.HeartbeatInterval();
			Service.IgnoreUrlPrefixes();
			Service.ImageTypes();
			Service.ImagesAsXhtml();
			Service.IndexFolder();
			Service.IndexMergeFactor();
			Service.InstanceName();
			Service.InvalidItemNameChars();
			Service.ItemNameValidation();
			Service.ItemNotFoundUrl();
			Service.KeepLockAfterSaveForAdminUsers();
			Service.LayoutFolder();
			Service.LayoutNotFoundUrl();
			Service.LayoutPageEvent();
			Service.LicenseFile();
			Service.LinkItemNotFoundUrl();
			Service.LogFolder();
			Service.LogInterval();
			Service.LoginLayout();
			Service.LoginPage();
			Service.MailServer();
			Service.MailServerPassword();
			Service.MailServerPort();
			Service.MailServerUserName();
			Service.MasterVariablesReplacer();
			Service.MaxCallLevel();
			Service.MaxDocumentBufferSize();
			Service.MaxFacets();
			Service.MaxItemNameLength();
			Service.MaxSqlBatchStatements();
			Service.MaxTreeDepth();
			Service.MaxWorkerThreads();
			Service.MediaFolder();
			Service.NoAccessUrl();
			Service.NoLicenseUrl();
			Service.PackagePath();
			Service.PageStateStore();
			Service.PipelineProfilingEnabled();
			Service.PipelineProfilingMeasureCpuTime();
			Service.PortalPrincipalResolver();
			Service.PortalStorage();
			Service.ProcessDuplicatePlaceholders();
			Service.ProcessHistoryCount();
			Service.ProfileItemDatabase();
			Service.ProtectedSite();
			Service.PublishDialogPollingInterval();
			Service.PublishEmptyItems();
			Service.PublishingInstance();
			Service.QueryMaxItems();
			Service.RamBufferSize();
			Service.RecycleBinActive();
			Service.RedirectUrlPrefixes();
			Service.RequireLockBeforeEditing();
			Service.RequireTargetDeleteRightWhenCheckingSecurity();
			Service.SaveRawUrl();
			Service.SerializationFolder();
			Service.SerializationPassword();
			Service.SiteResolving();
			Service.SiteResolvingMatchCurrentLanguage();
			Service.SiteResolvingMatchCurrentSite();
			Service.Sites();
			Service.StatusUpdateInterval();
			Service.SystemBlockedUrl();
			Service.TempFolderPath();
			Service.ThreadPriority();
			Service.ThumbnailHeight();
			Service.ThumbnailWidth();
			Service.ThumbnailsBackgroundColor();
			Service.TimeBeforeStatusExpires();
			Service.TypesThatShouldNotBeExpanded();
			Service.UnlockAfterCopy();
			Service.VersionFilePath();
			Service.ViewStateStore();
			Service.VirtualMembershipWildcard();
			Service.Wallpaper();
			Service.WallpapersPath();
			Service.WebStylesheet();
			Service.WelcomeTitle();
			Service.XHtmlSchemaFile();
			Service.XmlControlAspxFile();
			Service.XmlControlExtension();

			Service.ConnectionStringExists( "" );
			Service.GetAppSetting( "" );
			Service.GetAppSetting( "", "" );
			Service.GetBoolSetting( "", true );
			Service.GetConnectionString( "" );
			Service.GetDoubleSetting( "", 0 );
			Service.GetFileSetting( "" );
			Service.GetFileSetting( "", "" );
			Service.GetIntSetting( "", 0 );
			Service.GetLongSetting( "", 0 );
			Service.GetProviderObject( "", typeof( object ) );
			Service.GetProviderObject( "", "", typeof( object ) );
			Service.GetProviderObjects( "", "", typeof( object ) );
			Service.GetSetting( "" );
			Service.GetSetting( "", "" );
			Service.GetSystemSection<SettingClass>( "" );
			Service.GetTimeSpanSetting( "", TimeSpan.FromSeconds( 0 ) );
			Service.GetTimeSpanSetting( "", TimeSpan.FromSeconds( 0 ), null );
			Service.GetTimeSpanSetting( "", "" );
			Service.Reset();
			Service.WriteSetting( "", "", true );
			Service.WriteSetting( "", "", "", true );

			Assert.IsTrue( IsCallbackCalled( "AliasesActive" ) );
			Assert.IsTrue( IsCallbackCalled( "AllowLogoutOfAllUsers" ) );
			Assert.IsTrue( IsCallbackCalled( "AppendQSToUrlRendering" ) );
			Assert.IsTrue( IsCallbackCalled( "AutoScheduleSmartPublish" ) );
			Assert.IsTrue( IsCallbackCalled( "AutomaticDataBind" ) );
			Assert.IsTrue( IsCallbackCalled( "AutomaticLockOnSave" ) );
			Assert.IsTrue( IsCallbackCalled( "AutomaticUnlockOnSaved" ) );
			Assert.IsTrue( IsCallbackCalled( "CheckSecurity" ) );
			Assert.IsTrue( IsCallbackCalled( "CheckSecurityOnLanguages" ) );
			Assert.IsTrue( IsCallbackCalled( "ClientLanguage" ) );
			Assert.IsTrue( IsCallbackCalled( "ClientPersistentLoginDuration" ) );
			Assert.IsTrue( IsCallbackCalled( "ClientSessionTimeout" ) );
			Assert.IsTrue( IsCallbackCalled( "CompareRevisions" ) );
			Assert.IsTrue( IsCallbackCalled( "ConfigurationIsSet" ) );
			Assert.IsTrue( IsCallbackCalled( "CustomErrorsMode" ) );
			Assert.IsTrue( IsCallbackCalled( "DataFolder" ) );
			Assert.IsTrue( IsCallbackCalled( "DebugBorderColor" ) );
			Assert.IsTrue( IsCallbackCalled( "DebugBorderTag" ) );
			Assert.IsTrue( IsCallbackCalled( "DebugEnabled" ) );
			Assert.IsTrue( IsCallbackCalled( "DebugFolder" ) );
			Assert.IsTrue( IsCallbackCalled( "DefaultBaseTemplate" ) );
			Assert.IsTrue( IsCallbackCalled( "DefaultDesktop" ) );
			Assert.IsTrue( IsCallbackCalled( "DefaultDomainName" ) );
			Assert.IsTrue( IsCallbackCalled( "DefaultIcon" ) );
			Assert.IsTrue( IsCallbackCalled( "DefaultItem" ) );
			Assert.IsTrue( IsCallbackCalled( "DefaultLanguage" ) );
			Assert.IsTrue( IsCallbackCalled( "DefaultLayoutFile" ) );
			Assert.IsTrue( IsCallbackCalled( "DefaultMembershipProviderWildcard" ) );
			Assert.IsTrue( IsCallbackCalled( "DefaultPageName" ) );
			Assert.IsTrue( IsCallbackCalled( "DefaultPublishingTargets" ) );
			Assert.IsTrue( IsCallbackCalled( "DefaultRegionalIsoCode" ) );
			Assert.IsTrue( IsCallbackCalled( "DefaultSQLTimeout" ) );
			Assert.IsTrue( IsCallbackCalled( "DefaultShellControl" ) );
			Assert.IsTrue( IsCallbackCalled( "DefaultSortOrder" ) );
			Assert.IsTrue( IsCallbackCalled( "DefaultTheme" ) );
			Assert.IsTrue( IsCallbackCalled( "DefaultThumbnail" ) );
			Assert.IsTrue( IsCallbackCalled( "DisableBrowserCaching" ) );
			Assert.IsTrue( IsCallbackCalled( "EmailValidation" ) );
			Assert.IsTrue( IsCallbackCalled( "EnableEventQueues" ) );
			Assert.IsTrue( IsCallbackCalled( "EnableSiteConfigFiles" ) );
			Assert.IsTrue( IsCallbackCalled( "EnableXslDocumentFunction" ) );
			Assert.IsTrue( IsCallbackCalled( "EnableXslScripts" ) );
			Assert.IsTrue( IsCallbackCalled( "ErrorPage" ) );
			Assert.IsTrue( IsCallbackCalled( "FastPublishing" ) );
			Assert.IsTrue( IsCallbackCalled( "FastQueryDescendantsDisabled" ) );
			Assert.IsTrue( IsCallbackCalled( "GenerateThumbnails" ) );
			Assert.IsTrue( IsCallbackCalled( "GetNumberOfSettings" ) );
			Assert.IsTrue( IsCallbackCalled( "GridPageSize" ) );
			Assert.IsTrue( IsCallbackCalled( "HashEncryptionProvider" ) );
			Assert.IsTrue( IsCallbackCalled( "HealthMonitorInterval" ) );
			Assert.IsTrue( IsCallbackCalled( "HeartbeatInterval" ) );
			Assert.IsTrue( IsCallbackCalled( "IgnoreUrlPrefixes" ) );
			Assert.IsTrue( IsCallbackCalled( "ImageTypes" ) );
			Assert.IsTrue( IsCallbackCalled( "ImagesAsXhtml" ) );
			Assert.IsTrue( IsCallbackCalled( "IndexFolder" ) );
			Assert.IsTrue( IsCallbackCalled( "IndexMergeFactor" ) );
			Assert.IsTrue( IsCallbackCalled( "InstanceName" ) );
			Assert.IsTrue( IsCallbackCalled( "InvalidItemNameChars" ) );
			Assert.IsTrue( IsCallbackCalled( "ItemNameValidation" ) );
			Assert.IsTrue( IsCallbackCalled( "ItemNotFoundUrl" ) );
			Assert.IsTrue( IsCallbackCalled( "KeepLockAfterSaveForAdminUsers" ) );
			Assert.IsTrue( IsCallbackCalled( "LayoutFolder" ) );
			Assert.IsTrue( IsCallbackCalled( "LayoutNotFoundUrl" ) );
			Assert.IsTrue( IsCallbackCalled( "LayoutPageEvent" ) );
			Assert.IsTrue( IsCallbackCalled( "LicenseFile" ) );
			Assert.IsTrue( IsCallbackCalled( "LinkItemNotFoundUrl" ) );
			Assert.IsTrue( IsCallbackCalled( "LogFolder" ) );
			Assert.IsTrue( IsCallbackCalled( "LogInterval" ) );
			Assert.IsTrue( IsCallbackCalled( "LoginLayout" ) );
			Assert.IsTrue( IsCallbackCalled( "LoginPage" ) );
			Assert.IsTrue( IsCallbackCalled( "MailServer" ) );
			Assert.IsTrue( IsCallbackCalled( "MailServerPassword" ) );
			Assert.IsTrue( IsCallbackCalled( "MailServerPort" ) );
			Assert.IsTrue( IsCallbackCalled( "MailServerUserName" ) );
			Assert.IsTrue( IsCallbackCalled( "MasterVariablesReplacer" ) );
			Assert.IsTrue( IsCallbackCalled( "MaxCallLevel" ) );
			Assert.IsTrue( IsCallbackCalled( "MaxDocumentBufferSize" ) );
			Assert.IsTrue( IsCallbackCalled( "MaxFacets" ) );
			Assert.IsTrue( IsCallbackCalled( "MaxItemNameLength" ) );
			Assert.IsTrue( IsCallbackCalled( "MaxSqlBatchStatements" ) );
			Assert.IsTrue( IsCallbackCalled( "MaxTreeDepth" ) );
			Assert.IsTrue( IsCallbackCalled( "MaxWorkerThreads" ) );
			Assert.IsTrue( IsCallbackCalled( "MediaFolder" ) );
			Assert.IsTrue( IsCallbackCalled( "NoAccessUrl" ) );
			Assert.IsTrue( IsCallbackCalled( "NoLicenseUrl" ) );
			Assert.IsTrue( IsCallbackCalled( "PackagePath" ) );
			Assert.IsTrue( IsCallbackCalled( "PageStateStore" ) );
			Assert.IsTrue( IsCallbackCalled( "PipelineProfilingEnabled" ) );
			Assert.IsTrue( IsCallbackCalled( "PipelineProfilingMeasureCpuTime" ) );
			Assert.IsTrue( IsCallbackCalled( "PortalPrincipalResolver" ) );
			Assert.IsTrue( IsCallbackCalled( "PortalStorage" ) );
			Assert.IsTrue( IsCallbackCalled( "ProcessDuplicatePlaceholders" ) );
			Assert.IsTrue( IsCallbackCalled( "ProcessHistoryCount" ) );
			Assert.IsTrue( IsCallbackCalled( "ProfileItemDatabase" ) );
			Assert.IsTrue( IsCallbackCalled( "ProtectedSite" ) );
			Assert.IsTrue( IsCallbackCalled( "PublishDialogPollingInterval" ) );
			Assert.IsTrue( IsCallbackCalled( "PublishEmptyItems" ) );
			Assert.IsTrue( IsCallbackCalled( "PublishingInstance" ) );
			Assert.IsTrue( IsCallbackCalled( "QueryMaxItems" ) );
			Assert.IsTrue( IsCallbackCalled( "RamBufferSize" ) );
			Assert.IsTrue( IsCallbackCalled( "RecycleBinActive" ) );
			Assert.IsTrue( IsCallbackCalled( "RedirectUrlPrefixes" ) );
			Assert.IsTrue( IsCallbackCalled( "RequireLockBeforeEditing" ) );
			Assert.IsTrue( IsCallbackCalled( "RequireTargetDeleteRightWhenCheckingSecurity" ) );
			Assert.IsTrue( IsCallbackCalled( "SaveRawUrl" ) );
			Assert.IsTrue( IsCallbackCalled( "SerializationFolder" ) );
			Assert.IsTrue( IsCallbackCalled( "SerializationPassword" ) );
			Assert.IsTrue( IsCallbackCalled( "SiteResolving" ) );
			Assert.IsTrue( IsCallbackCalled( "SiteResolvingMatchCurrentLanguage" ) );
			Assert.IsTrue( IsCallbackCalled( "SiteResolvingMatchCurrentSite" ) );
			Assert.IsTrue( IsCallbackCalled( "Sites" ) );
			Assert.IsTrue( IsCallbackCalled( "StatusUpdateInterval" ) );
			Assert.IsTrue( IsCallbackCalled( "SystemBlockedUrl" ) );
			Assert.IsTrue( IsCallbackCalled( "TempFolderPath" ) );
			Assert.IsTrue( IsCallbackCalled( "ThreadPriority" ) );
			Assert.IsTrue( IsCallbackCalled( "ThumbnailHeight" ) );
			Assert.IsTrue( IsCallbackCalled( "ThumbnailWidth" ) );
			Assert.IsTrue( IsCallbackCalled( "ThumbnailsBackgroundColor" ) );
			Assert.IsTrue( IsCallbackCalled( "TimeBeforeStatusExpires" ) );
			Assert.IsTrue( IsCallbackCalled( "TypesThatShouldNotBeExpanded" ) );
			Assert.IsTrue( IsCallbackCalled( "UnlockAfterCopy" ) );
			Assert.IsTrue( IsCallbackCalled( "VersionFilePath" ) );
			Assert.IsTrue( IsCallbackCalled( "ViewStateStore" ) );
			Assert.IsTrue( IsCallbackCalled( "VirtualMembershipWildcard" ) );
			Assert.IsTrue( IsCallbackCalled( "Wallpaper" ) );
			Assert.IsTrue( IsCallbackCalled( "WallpapersPath" ) );
			Assert.IsTrue( IsCallbackCalled( "WebStylesheet" ) );
			Assert.IsTrue( IsCallbackCalled( "WelcomeTitle" ) );
			Assert.IsTrue( IsCallbackCalled( "XHtmlSchemaFile" ) );
			Assert.IsTrue( IsCallbackCalled( "XmlControlAspxFile" ) );
			Assert.IsTrue( IsCallbackCalled( "XmlControlExtension" ) );

			Assert.IsTrue( IsCallbackCalled( "ConnectionStringExists_string" ) );
			Assert.IsTrue( IsCallbackCalled( "GetAppSetting_string" ) );
			Assert.IsTrue( IsCallbackCalled( "GetAppSetting_string_string" ) );
			Assert.IsTrue( IsCallbackCalled( "GetBoolSetting_string_bool" ) );
			Assert.IsTrue( IsCallbackCalled( "GetConnectionString_string" ) );
			Assert.IsTrue( IsCallbackCalled( "GetDoubleSetting_string_double" ) );
			Assert.IsTrue( IsCallbackCalled( "GetFileSetting_string" ) );
			Assert.IsTrue( IsCallbackCalled( "GetFileSetting_string_string" ) );
			Assert.IsTrue( IsCallbackCalled( "GetIntSetting_string_int" ) );
			Assert.IsTrue( IsCallbackCalled( "GetLongSetting_string_long" ) );
			Assert.IsTrue( IsCallbackCalled( "GetProviderObject_string_type" ) );
			Assert.IsTrue( IsCallbackCalled( "GetProviderObject_string_string_type" ) );
			Assert.IsTrue( IsCallbackCalled( "GetProviderObjects_string_string_type" ) );
			Assert.IsTrue( IsCallbackCalled( "GetSetting_string" ) );
			Assert.IsTrue( IsCallbackCalled( "GetSetting_string_string" ) );
			Assert.IsTrue( IsCallbackCalled( "GetSystemSection_string" ) );
			Assert.IsTrue( IsCallbackCalled( "GetTimeSpanSetting_string_timespan" ) );
			Assert.IsTrue( IsCallbackCalled( "GetTimeSpanSetting_string_timespan_cultureinfo" ) );
			Assert.IsTrue( IsCallbackCalled( "GetTimeSpanSetting_string_string" ) );
			Assert.IsTrue( IsCallbackCalled( "WriteSetting_string_string_bool" ) );
			Assert.IsTrue( IsCallbackCalled( "WriteSetting_string_string_string_bool" ) );
		}

		[TestMethod]
		public void AzureSolrSettingsRedirector_OverrideSettingFromAppSettings()
		{
			var val = Service.GetSetting( OverrideSettingKey );

			Assert.AreEqual( OverrideSettingValue, val );
		}

		[TestMethod]
		public void AzureSolrSettingsRedirector_OverrideSettingFromAppSettings_WithDefault()
		{
			var val = Service.GetSetting( OverrideSettingKey, "default" );

			Assert.AreEqual( OverrideSettingValue, val );
		}

		[TestMethod]
		public void AzureSolrSettingsRedirector_SitecoreOnly()
		{
			var val = Service.GetSetting( SitecoreSettingKey );

			Assert.AreEqual( SitecoreSettingValue, val );
		}

		[TestMethod]
		public void AzureSolrSettingsRedirector_SitecoreOnly_WithDefault()
		{
			var val = Service.GetSetting( SitecoreSettingKey, "default" );

			Assert.AreEqual( SitecoreSettingValue, val );
		}


		[TestMethod]
		public void AzureSolrSettingsRedirector_NotInEither()
		{
			var val = Service.GetSetting( "not a value" );

			Assert.IsNull( val );
		}

		[TestMethod]
		public void AzureSolrSettingsRedirector_NotInEither_WithDefault()
		{
			var val = Service.GetSetting( "not a value", "default" );

			Assert.AreEqual( "default", val );
		}
	}
}

namespace office_online_wopi_server.Models
{
    public class CheckFileInfo
    {
        public string BaseFileName { get; set; }

        public string OwnerId { get; set; }

        public long Size { get; set; }

        public string UserId { get; set; }

        public string Version { get; set; }

        public string LastModifiedTime { get; set; }

        public string FileExtension { get; set; }

        public int FileNameMaxLength { get; set; }

        public string BreadcrumbBrandName { get; set; }

        public string BreadcrumbBrandUrl { get; set; }

        public string BreadcrumbDocName { get; set; }

        public string BreadcrumbDocUrl { get; set; }

        public string BreadcrumbFolderName { get; set; }

        public string BreadcrumbFolderUrl { get; set; }

        public string ClientUrl { get; set; }

        public bool CloseButtonClosesWindow { get; set; }
        public bool ClosePostMessage { get; set; }

        public string CloseUrl { get; set; }

        public bool DisableBrowserCachingOfUserContent { get; set; }

        public bool AllowAdditionalMicrosoftServices { get; set; }

        public bool AllowExternalMarketplace { get; set; }

        public bool DisablePrint { get; set; }

        public bool DisableTranslation { get; set; }

        public string DownloadUrl { get; set; }

        public bool EditModePostMessage { get; set; }

        public bool EditNotificationPostMessage { get; set; }

        public string FileSharingUrl { get; set; }

        public string FileUrl { get; set; }

        public string HostAuthenticationId { get; set; }

        public string HostEditUrl { get; set; }

        public string HostEmbeddedEditUrl { get; set; }

        public string HostEmbeddedViewUrl { get; set; }

        public string HostName { get; set; }

        public string HostNotes { get; set; }

        public string HostRestUrl { get; set; }

        public string HostViewUrl { get; set; }

        public string IrmPolicyDescription { get; set; }

        public string IrmPolicyTitle { get; set; }

        public string PresenceProvider { get; set; }

        public string PresenceUserId { get; set; }

        public string PrivacyUrl { get; set; }

        public bool ProtectInClient { get; set; }

        public string SignInUrl { get; set; }

        public bool ReadOnly { get; set; }

        public bool RestrictedWebViewOnly { get; set; }

        public string SHA256 { get; set; }

        public string UniqueContentId { get; set; }

        public string SignoutUrl { get; set; }

        public bool SupportsCoauth { get; set; }

        public bool SupportsCobalt { get; set; }

        public bool SupportsFolders { get { return SupportsContainers; } set { SupportsContainers = value; } }

        public bool SupportsContainers { get; set; }

        public bool SupportsLocks { get; set; }

        public bool SupportsGetLock { get; set; }

        public bool SupportsExtendedLockLength { get; set; }

        public bool SupportsEcosystem { get; set; }

        public bool SupportsGetFileWopiSrc { get; set; }

        public string[] SupportedShareUrlTypes { get; set; }

        public bool SupportsScenarioLinks { get; set; }

        public bool SupportsSecureStore { get; set; }

        public bool SupportsFileCreation { get; set; }

        public bool SupportsUpdate { get; set; }

        public bool SupportsRename { get; set; }

        public bool SupportsDeleteFile { get; set; }

        public bool SupportsUserInfo { get; set; }

        public string UserInfo { get; set; }

        public string TenantId { get; set; }

        public string TermsOfUseUrl { get; set; }

        public string TimeZone { get; set; }

        public bool IsAnonymousUser { get; set; }

        public bool IsEduUser { get; set; }

        public bool LicenseCheckForEditIsEnabled { get; set; }

        public bool UserCanAttend { get; set; }

        public bool UserCanNotWriteRelative { get; set; }

        public bool UserCanPresent { get; set; }

        public bool UserCanWrite { get; set; }

        public bool UserCanRename { get; set; }

        public string UserFriendlyName { get; set; }

        public string UserPrincipalName { get; set; }

        public bool WebEditingDisabled { get; set; }

        public bool WorkflowPostMessage { get; set; }

        public CheckFileInfo()
        {
            AllowAdditionalMicrosoftServices = false;
            AllowExternalMarketplace = false;
            BreadcrumbBrandName = "ConceptOne";
            CloseButtonClosesWindow = false;
            ClosePostMessage = true;
            EditModePostMessage = true;
            EditNotificationPostMessage = true;
            HostName = "ConceptOne";
            LicenseCheckForEditIsEnabled = false;
            OwnerId = "EpicPremier";
            ReadOnly = false;
            RestrictedWebViewOnly = false;
            SupportsCoauth = false;
            SupportsCobalt = false;
            SupportsExtendedLockLength = false;
            SupportsFileCreation = true;
            SupportsContainers = true;
            SupportsGetLock = false;
            SupportsLocks = false;
            SupportsRename = false;
            SupportsScenarioLinks = false;
            SupportsSecureStore = true;
            SupportsUpdate = true;
            SupportsUserInfo = false;
            UserCanAttend = false;
            UserCanNotWriteRelative = true;
            UserCanPresent = false;
            UserCanRename = false;
            UserCanWrite = true;
            WebEditingDisabled = false;
            WorkflowPostMessage = true;

        }
    }
}
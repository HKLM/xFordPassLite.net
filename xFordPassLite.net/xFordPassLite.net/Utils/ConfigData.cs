namespace xFordPassLite.net.Utils
{
    public static class ConfigData
    {
        private static string regionCode = "US";

        /// <summary>
        /// applicationId per region
        /// </summary>
        private static readonly string us = "71A3AD0A-CF46-4CCF-B473-FC7FE5BC4592";
        private static readonly string eu = "1E8C7794-FF5F-49BC-9596-A1E0C86C5B19";
        private static readonly string au = "5C80A6BB-CF0D-4A30-BDBF-FC804B5C1A98";
        public static string DefaultRegion => "US";

        /// <summary>
        /// At app startup, Force a full refresh of data from vehicle or use the quicker data from Ford API servers
        /// </summary>
        public static bool LongRefreshAtStartup { get; set; } = false;
        public static bool DebugMode { get; set; } = false;
        public static string USERNAME { get; set; } = "";
        public static string PW { get; set; } = "";
        public static string VIN { get; set; } = "";
        public static string REGION
        {
            get => regionCode;
            set
            {
                if (IsValidRegion(value))
                    regionCode = value;
            }
        }

        public static string BaseEndpoint { get; } = "https://usapi.cv.ford.com/";
        public static string IdpEndpoint { get; } = "https://sso.ci.ford.com/oidc/endpoint/default/token";
        public static string OAuthEndpoint { get; } = "https://api.mps.ford.com/api/oauth2/v1";
        public static string RefreshTokenEndpoint { get; } = "https://api.mps.ford.com/api/oauth2/v1/refresh";
        public static string ClientId { get; } = "9fb503e0-715b-47e8-adfd-ad4b7770f73b";
        public static string UserAgentString { get; } = "FordPass/5 CFNetwork/1327.0.4 Darwin/21.2.0";


        /// <summary>
        /// Convert country code to ApplicationId for that region
        /// </summary>
        /// <returns>ApplicationID guid</returns>
        public static string RegionToAppID()
        {
            if (REGION == "US")
                return us;
            else if (REGION == "EU")
                return eu;
            else if (REGION == "AU")
                return au;
            else
                return us;
        }

        public static bool IsValidRegion(string stringToTest)
        {
            return stringToTest == "US" || stringToTest == "EU" || stringToTest == "AU";
        }
    }
}

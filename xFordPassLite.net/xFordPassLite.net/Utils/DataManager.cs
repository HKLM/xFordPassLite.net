using Xamarin.Essentials;

namespace xFordPassLite.net.Utils
{
    public class DataManager
    {
        public DataManager() { }

        public string USERNAME
        {
            get => ConfigData.USERNAME;
            set
            {
                if (ConfigData.USERNAME != value)
                {
                    ConfigData.USERNAME = value;
                    Preferences.Set("USERNAME", value);
                }
            }
        }

        public string PW
        {
            get => ConfigData.PW;
            set
            {
                if (ConfigData.PW != value)
                {
                    ConfigData.PW = value;
                    Preferences.Set("PW", value);
                }
            }
        }

        public string VIN
        {
            get => ConfigData.VIN;
            set
            {
                if (ConfigData.VIN != value)
                {
                    ConfigData.VIN = value;
                    Preferences.Set("VIN", value);
                }
            }
        }

        public string REGION
        {
            get => ConfigData.REGION;
            set
            {
                if (ConfigData.REGION != value)
                {
                    ConfigData.REGION = value;
                    Preferences.Set("RegionCode", value);
                }
            }
        }
    }
}

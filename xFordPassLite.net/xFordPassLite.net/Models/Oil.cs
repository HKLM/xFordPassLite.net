using Newtonsoft.Json;

using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;

namespace xFordPassLite.net.Models
{
    public class Oil : ObservableObject
    {
        [JsonIgnore]
        private string _OilLife;
        [JsonProperty("oilLife")]
        public string OilLife
        {
            get => _OilLife;
            set => SetProperty(ref _OilLife, value);
        }

        [JsonIgnore]
        private int _OilLifePercentage;
        [JsonProperty("oilLifeActual")]
        public int OilLifePercentage
        {
            get => _OilLifePercentage;
            set => SetProperty(ref _OilLifePercentage, value);
        }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        /// <summary>
        /// Is this displayed to the user
        /// </summary>
        [JsonIgnore]
        private bool _Visible;

        [JsonIgnore]
        public bool Visible
        {
            get => _Visible;
            set
            {
                SetProperty(ref _Visible, value);
                Preferences.Set("V_Oil", _Visible);
            }
        }

        /// <summary>
        /// Title
        /// </summary>
        [JsonIgnore]
        public string Name { get; set; }

        public Oil()
        {
            _Visible = Preferences.Get("V_Oil", false);
        }
    }
}

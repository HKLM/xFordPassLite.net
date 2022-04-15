using Newtonsoft.Json;

using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;

namespace xFordPassLite.net.Models
{
    public class TirePressure : ObservableObject
    {
        [JsonProperty("value")]
        public string Value { get; set; }

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
                Preferences.Set("V_TirePressure", _Visible);
            }
        }

        /// <summary>
        /// Title
        /// </summary>
        [JsonIgnore]
        public readonly string Name = "TirePressure";

        public TirePressure()
        {
            _Visible = Preferences.Get("V_TirePressure", false);
        }
    }
}

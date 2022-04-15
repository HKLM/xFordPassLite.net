using Newtonsoft.Json;

using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;

namespace xFordPassLite.net.Models
{
    public class Alarm : ObservableObject
    {
        [JsonIgnore]
        private string _Value;
        [JsonProperty("value")]
        public string Value
        {
            get => _Value;
            set => SetProperty(ref _Value, value);
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
                Preferences.Set("V_Alarm", _Visible);
            }
        }

        /// <summary>
        /// Title
        /// </summary>
        [JsonIgnore]
        public readonly string Name = "Alarm";

        public Alarm()
        {
            _Visible = Preferences.Get("V_Alarm", true);
        }
    }
}
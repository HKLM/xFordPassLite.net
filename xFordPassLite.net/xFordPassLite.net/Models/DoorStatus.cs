using Newtonsoft.Json;

using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;

namespace xFordPassLite.net.Models
{
    public class DoorStatus : ObservableObject
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
                if (Name != null || Name != "")
                    Preferences.Set("V_" + Name, _Visible);
            }
        }

        /// <summary>
        /// Title
        /// </summary>
        [JsonIgnore]
        public string Name { get; set; }

        public DoorStatus(string name)
        {
            Name = name;
            string vname = "V_" + name;
            _Visible = Preferences.Get(vname, true);
        }
    }
}

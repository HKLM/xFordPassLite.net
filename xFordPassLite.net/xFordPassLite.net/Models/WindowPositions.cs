using Newtonsoft.Json;

using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;

namespace xFordPassLite.net.Models
{
    public class WindowPositions : ObservableObject
    {
        [JsonProperty("driverWindowPosition")]
        public WindowPosition DriverWindowPosition { get; set; }
        [JsonProperty("passWindowPosition")]
        public WindowPosition PassWindowPosition { get; set; }
        [JsonProperty("rearDriverWindowPos")]
        public WindowPosition RearDriverWindowPos { get; set; }
        [JsonProperty("rearPassWindowPos")]
        public WindowPosition RearPassWindowPos { get; set; }

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
                Preferences.Set("V_WindowPositions", _Visible);
            }
        }

        public WindowPositions()
        {
            _Visible = Preferences.Get("V_WindowPositions", true);
        }
    }
}

using System;

using Newtonsoft.Json;

using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;

namespace xFordPassLite.net.Models
{
    public class Fuel : ObservableObject
    {
        [JsonIgnore]
        private decimal _fuelLevel;
        [JsonProperty("fuelLevel")]
        public decimal FuelLevel
        {
            get => Math.Round(_fuelLevel, 1);
            set => SetProperty(ref _fuelLevel, value);
        }

        [JsonIgnore]
        private decimal _distanceToEmpty;
        [JsonProperty("distanceToEmpty")]
        public decimal DistanceToEmpty
        {
            get => Math.Round(_distanceToEmpty, 1);
            set => SetProperty(ref _distanceToEmpty, value);
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
                Preferences.Set("V_Fuel", _Visible);
            }
        }

        /// <summary>
        /// Title
        /// </summary>
        [JsonIgnore]
        public readonly string Name = "Fuel";

        public Fuel()
        {
            _Visible = Preferences.Get("V_Fuel", false);
        }
    }
}

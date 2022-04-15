using System;

using Newtonsoft.Json;

using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;

namespace xFordPassLite.net.Models
{
    public class Odometer : ObservableObject
    {
        [JsonIgnore]
        private decimal _Value;
        [JsonProperty("value")]
        public decimal Value
        {
            get => Math.Round(_Value * (decimal)0.621, 1);
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
                Preferences.Set("V_Odometer", _Visible);
            }
        }

        /// <summary>
        /// Title
        /// </summary>
        [JsonIgnore]
        public readonly string Name = "Odometer";

        public Odometer()
        {
            _Visible = Preferences.Get("V_Odometer", true);
        }
    }
}

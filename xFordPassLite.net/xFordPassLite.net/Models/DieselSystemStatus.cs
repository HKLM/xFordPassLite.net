using Newtonsoft.Json;

using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;

namespace xFordPassLite.net.Models
{
    public class DieselSystemStatus : ObservableObject
    {
        [JsonIgnore]
        private string _ExhaustFluidLevel;
        [JsonProperty("exhaustFluidLevel")]
        public string ExhaustFluidLevel
        {
            get => _ExhaustFluidLevel;
            set => SetProperty(ref _ExhaustFluidLevel, value);
        }

        [JsonIgnore]
        private string _FilterSoot;
        [JsonProperty("filterSoot")]
        public string FilterSoot
        {
            get => _FilterSoot;
            set => SetProperty(ref _FilterSoot, value);
        }

        [JsonIgnore]
        private string _UreaRange;
        [JsonProperty("ureaRange")]
        public string UreaRange
        {
            get => _UreaRange;
            set => SetProperty(ref _UreaRange, value);
        }

        [JsonIgnore]
        private MetricType _MetricType;
        [JsonProperty("metricType")]
        public MetricType MetricType
        {
            get => _MetricType;
            set => SetProperty(ref _MetricType, value);
        }

        [JsonIgnore]
        private string _FilterRegenerationStatus;
        [JsonProperty("filterRegenerationStatus")]
        public string FilterRegenerationStatus
        {
            get => _FilterRegenerationStatus;
            set => SetProperty(ref _FilterRegenerationStatus, value);
        }

        /// <summary>
        /// Is this displayed to the user
        /// </summary>
        [JsonIgnore]
        private bool _Visible;
        [JsonIgnore]
        public bool Visible
        {
            get => ExhaustFluidLevel == "null" && FilterRegenerationStatus == "null" ? false : _Visible;
            set
            {
                SetProperty(ref _Visible, value);
                Preferences.Set("V_DieselSystemStatus", _Visible);
            }
        }

        /// <summary>
        /// Title
        /// </summary>
        [JsonIgnore]
        public readonly string Name = "Diesel System Status";

        public DieselSystemStatus()
        {
            MetricType = new MetricType();
            _Visible = Preferences.Get("V_DieselSystemStatus", false);
        }
    }
}

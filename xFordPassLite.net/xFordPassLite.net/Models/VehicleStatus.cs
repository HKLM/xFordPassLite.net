using Newtonsoft.Json;

using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;

namespace xFordPassLite.net.Models
{
    public class VehicleStatus : ObservableObject
    {
        public VehicleStatus()
        {
            _lastRefresh_Visible = Preferences.Get("V_lastRefresh_Visible", true);
            _lastModifiedDate_Visible = Preferences.Get("V_lastModifiedDate_Visible", true);
            _serverTime_Visible = Preferences.Get("V_serverTime_Visible", true);
        }

        [JsonProperty("vin")]
        public string VehicleIdentificationNumber { get; set; }

        [JsonProperty("lockStatus")]
        public LockStatus LockStatus { get; set; }

        [JsonProperty("alarm")]
        public Alarm Alarm { get; set; }
        [JsonProperty("PrmtAlarmEvent")]
        public AlarmEvent AlarmEvent { get; set; }
        [JsonProperty("odometer")]
        public Odometer Odometer { get; set; }
        [JsonProperty("fuel")]
        public Fuel Fuel { get; set; }
        [JsonProperty("gps")]
        public GlobalPositioningSystem GlobalPositioningSystem { get; set; }
        [JsonProperty("remoteStart")]
        public RemoteStart RemoteStart { get; set; }
        [JsonProperty("remoteStartStatus")]
        public RemoteStartStatus RemoteStartStatus { get; set; }
        [JsonProperty("battery")]
        public Battery Battery { get; set; }
        [JsonProperty("oil")]
        public Oil Oil { get; set; }
        [JsonProperty("tirePressure")]
        public TirePressure TirePressure { get; set; }
        [JsonProperty("authorization")]
        public string Authorization { get; set; }
        [JsonProperty("TPMS")]
        public TirePressureManagementSystem TirePressureManagementSystem { get; set; }
        [JsonProperty("firmwareUpgInProgress")]
        public FirmwareUpgradeInProgress FirmwareUpgradeInProgress { get; set; }
        [JsonProperty("deepSleepInProgress")]
        public DeepSleepInProgress DeepSleepInProgress { get; set; }
        [JsonProperty("ccsSettings")]
        public CombinedChargingSystemSettings CombinedChargingSystemSettings { get; set; }

        #region LastRefresh
        [JsonIgnore]
        private string _lastRefresh;
        [JsonProperty("lastRefresh")]
        public string LastRefresh
        {
            get => _lastRefresh;
            set => SetProperty(ref _lastRefresh, value);
        }

        [JsonIgnore]
        private bool _lastRefresh_Visible;

        [JsonIgnore]
        public bool LastRefresh_Visible
        {
            get => _lastRefresh_Visible;
            set
            {
                SetProperty(ref _lastRefresh_Visible, value);
                Preferences.Set("V_lastRefresh_Visible", _lastRefresh_Visible);
            }
        }
        #endregion LastRefresh

        #region LastModifiedDate
        [JsonIgnore]
        private string _lastModifiedDate;
        [JsonProperty("lastModifiedDate")]
        public string LastModifiedDate
        {
            get => _lastModifiedDate;
            set => SetProperty(ref _lastModifiedDate, value);
        }
        [JsonIgnore]
        private bool _lastModifiedDate_Visible;

        [JsonIgnore]
        public bool LastModifiedDate_Visible
        {
            get => _lastModifiedDate_Visible;
            set
            {
                SetProperty(ref _lastModifiedDate_Visible, value);
                Preferences.Set("V_lastModifiedDate_Visible", _lastModifiedDate_Visible);
            }
        }
        #endregion LastModifiedDate

        #region ServerTime
        [JsonIgnore]
        private string _serverTime;
        [JsonProperty("serverTime")]
        public string ServerTime
        {
            get => _serverTime;
            set => SetProperty(ref _serverTime, value);
        }
        [JsonIgnore]
        private bool _serverTime_Visible;

        [JsonIgnore]
        public bool ServerTime_Visible
        {
            get => _serverTime_Visible;
            set
            {
                SetProperty(ref _serverTime_Visible, value);
                Preferences.Set("V_serverTime_Visible", _serverTime_Visible);
            }
        }
        #endregion ServerTime

        [JsonProperty("batteryFillLevel")]
        public object BatteryFillLevel { get; set; }
        [JsonProperty("elVehDTE")]
        public object elVehDTE { get; set; }
        [JsonProperty("hybridModeStatus")]
        public object hybridModeStatus { get; set; }
        [JsonProperty("chargingStatus")]
        public object chargingStatus { get; set; }
        [JsonProperty("plugStatus")]
        public object plugStatus { get; set; }
        [JsonProperty("chargeStartTime")]
        public object chargeStartTime { get; set; }
        [JsonProperty("chargeEndTime")]
        public object chargeEndTime { get; set; }
        [JsonProperty("preCondStatusDsply")]
        public object preCondStatusDsply { get; set; }
        [JsonProperty("chargerPowertype")]
        public object chargerPowertype { get; set; }
        [JsonProperty("batteryPerfStatus")]
        public object batteryPerfStatus { get; set; }
        [JsonProperty("outandAbout")]
        public OutAndAbout OutAndAbout { get; set; }
        [JsonProperty("batteryChargeStatus")]
        public object batteryChargeStatus { get; set; }
        [JsonProperty("dcFastChargeData")]
        public object dcFastChargeData { get; set; }
        [JsonProperty("windowPosition")]
        public WindowPositions WindowPosition { get; set; }
        [JsonProperty("doorStatus")]
        public DoorStatuses DoorStatus { get; set; }
        [JsonProperty("ignitionStatus")]
        public IgnitionStatus IgnitionStatus { get; set; }
        [JsonProperty("batteryTracLowChargeThreshold")]
        public object batteryTracLowChargeThreshold { get; set; }
        [JsonProperty("battTracLoSocDDsply")]
        public object battTracLoSocDDsply { get; set; }
        [JsonProperty("dieselSystemStatus")]
        public DieselSystemStatus DieselSystemStatus { get; set; }
    }
}

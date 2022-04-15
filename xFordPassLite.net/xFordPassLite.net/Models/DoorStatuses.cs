using Newtonsoft.Json;

using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;

namespace xFordPassLite.net.Models
{
    public class DoorStatuses : ObservableObject
    {
        public DoorStatuses()
        {
            _Visible = Preferences.Get("V_DoorStatuses", true);

            DriverDoor = new DoorStatus("DriverDoor");
            PassengerDoor = new DoorStatus("PassengerDoor");
            RightRearDoor = new DoorStatus("RightRearDoor");
            LeftRearDoor = new DoorStatus("LeftRearDoor");
            HoodDoor = new DoorStatus("HoodDoor");
            TailgateDoor = new DoorStatus("TailgateDoor");
            InnerTailgateDoor = new DoorStatus("InnerTailgateDoor");
        }

        [JsonProperty("rightRearDoor")]
        public DoorStatus RightRearDoor { get; set; }
        [JsonProperty("leftRearDoor")]
        public DoorStatus LeftRearDoor { get; set; }
        [JsonProperty("driverDoor")]
        public DoorStatus DriverDoor { get; set; }
        [JsonProperty("passengerDoor")]
        public DoorStatus PassengerDoor { get; set; }
        [JsonProperty("hoodDoor")]
        public DoorStatus HoodDoor { get; set; }
        [JsonProperty("tailgateDoor")]
        public DoorStatus TailgateDoor { get; set; }
        [JsonProperty("innerTailgateDoor")]
        public DoorStatus InnerTailgateDoor { get; set; }

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
                Preferences.Set("V_DoorStatuses", _Visible);
            }
        }
    }
}

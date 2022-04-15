using System;

using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

using xFordPassLite.net.Models;
using xFordPassLite.net.Services;
using xFordPassLite.net.Utils;

namespace xFordPassLite.net.ViewModels
{
    public class BaseViewModel : ObservableObject, IDisposable
    {
        public IDataStore<FordInfo> FordXDataStore => DependencyService.Get<IDataStore<FordInfo>>();
        public virtual void Dispose() { }

        private bool isBusy = false;
        public bool IsBusy
        {
            get => isBusy;
            set => SetProperty(ref isBusy, value);
        }

        private string title = string.Empty;
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        private VehicleStatus _Vehicle;
        public VehicleStatus Vehicle
        {
            get => _Vehicle;
            set => SetProperty(ref _Vehicle, value);
        }

        /// <summary>
        /// Shows logging data
        /// </summary>
        private bool _bdebug;
        public bool DebugMode
        {
            get => _bdebug;
            set
            {
                SetProperty(ref _bdebug, value);
                if (ConfigData.DebugMode != value)
                {
                    ConfigData.DebugMode = value;
                    Preferences.Set("DebugMode", value);
                }
            }
        }

        private string _errorCode;
        public string ErrorCode
        {
            get => _errorCode;
            set => SetProperty(ref _errorCode, value);
        }

        public BaseViewModel()
        {
            Vehicle = new VehicleStatus
            {
                Alarm = new Alarm(),
                AlarmEvent = new AlarmEvent(),
                Battery = new Models.Battery
                {
                    Health = new BatteryHealth(),
                    Status = new BatteryStatusActual()
                },
                DoorStatus = new DoorStatuses
                {
                    DriverDoor = new DoorStatus("DriverDoor"),
                    PassengerDoor = new DoorStatus("PassengerDoor"),
                    RightRearDoor = new DoorStatus("RightRearDoor"),
                    LeftRearDoor = new DoorStatus("LeftRearDoor")
                },
                IgnitionStatus = new IgnitionStatus(),
                LockStatus = new LockStatus(),
                Odometer = new Odometer(),
                Oil = new Oil(),
                Fuel = new Fuel(),
                WindowPosition = new WindowPositions
                {
                    DriverWindowPosition = new WindowPosition(),
                    PassWindowPosition = new WindowPosition()
                },
                DieselSystemStatus = new DieselSystemStatus()
            };
            DebugMode = ConfigData.DebugMode;
            ErrorCode = FordXDataStore.GetErrorCode();
        }
    }
}

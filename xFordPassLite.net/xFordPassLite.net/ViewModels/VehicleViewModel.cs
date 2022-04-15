using System;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

using xFordPassLite.net.Models;
using xFordPassLite.net.Utils;
using xFordPassLite.net.Views;

namespace xFordPassLite.net.ViewModels
{
    public class VehicleViewModel : BaseViewModel
    {
        public override void Dispose()
        {
            this.Dispose();
        }

        private string _logMessage;
        public string LogMessage
        {
            get => _logMessage;
            set => SetProperty(ref _logMessage, value);
        }

        private string rawJSON;
        public string RawJSON
        {
            get => rawJSON;
            set => SetProperty(ref rawJSON, value);
        }

        private bool _busyBee;
        public bool BusyBee
        {
            get => _busyBee;
            set => SetProperty(ref _busyBee, value);
        }

        public bool SecuritySection => Vehicle.Alarm.Visible || Vehicle.AlarmEvent.Visible || Vehicle.LockStatus.Visible;

        public Command LoadItemsCommand { get; }

        public IAsyncCommand RefreshCommand { get; private set; }
        public IAsyncCommand UpdateCommand { get; private set; }
        public IAsyncCommand LockCommand { get; private set; }
        public IAsyncCommand UnlockCommand { get; private set; }
        public IAsyncCommand StartCommand { get; private set; }
        public IAsyncCommand StopCommand { get; private set; }

        public VehicleViewModel()
        {
            if (ConfigData.USERNAME == "" || ConfigData.USERNAME == null || ConfigData.PW == "" || ConfigData.PW == null || ConfigData.VIN == "" || ConfigData.VIN == null)
            {
                LogMessage = "FORDPASS USERID, PASSWORD, AND VIN ARE REQUIRED";
                LoadItemsCommand = new Command(async () => await OnGoLoginAsync());
                RefreshCommand = new AsyncCommand(OnGoLoginAsync);
                UpdateCommand = new AsyncCommand(OnGoLoginAsync);
                LockCommand = new AsyncCommand(OnGoLoginAsync);
                UnlockCommand = new AsyncCommand(OnGoLoginAsync);
                StartCommand = new AsyncCommand(OnGoLoginAsync);
                StopCommand = new AsyncCommand(OnGoLoginAsync);
                return;
            }
            _ = ExecuteInitViewCommand();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItem());
            RefreshCommand = new AsyncCommand(OnRefresh);
            UpdateCommand = new AsyncCommand(ExecuteGetUpdateCommand);
            LockCommand = new AsyncCommand(ExecuteLockCommand);
            UnlockCommand = new AsyncCommand(ExecuteUnlockCommand);
            StartCommand = new AsyncCommand(ExecuteStartCommand);
            StopCommand = new AsyncCommand(ExecuteStopCommand);
        }

        async Task ExecuteLoadItem()
        {
            IsBusy = true;
            LogMessage = "";
            if (ConfigData.USERNAME == "" || ConfigData.USERNAME == null || ConfigData.PW == "" || ConfigData.PW == null || ConfigData.VIN == "" || ConfigData.VIN == null)
            {
                LogMessage = "FORDPASS USERID, PASSWORD, AND VIN ARE REQUIRED";
                return;
            }

            try
            {
                FordInfo ford = await FordXDataStore.GetFordInfoAsync();
                Vehicle = ford.Vehicle;
                RawJSON = ford.JSONstring;
            }
            catch (Exception ex)
            {
                Debugger.Log(1, "VehicleViewModel", "ExecuteLoadItemsCommand() " + ex.Message + "\n");
                LogMessage = "VehicleViewModel.ExecuteLoadItemsCommand " + ex.Message;
            }
            finally
            {
                IsBusy = false;
            }
            ErrorCode = FordXDataStore.GetErrorCode();
        }

        async Task OnRefresh()
        {
            LogMessage = "";
            if (ConfigData.USERNAME == "" || ConfigData.USERNAME == null || ConfigData.PW == "" || ConfigData.PW == null || ConfigData.VIN == "" || ConfigData.VIN == null)
            {
                LogMessage = "FORDPASS USERID, PASSWORD, AND VIN ARE REQUIRED";
                return;
            }
            try
            {
                FordInfo ford = await FordXDataStore.GetFordInfoAsync();
                Vehicle = ford.Vehicle;
                RawJSON = ford.JSONstring;
            }
            catch (Exception ex)
            {
                Debugger.Log(1, "VehicleViewModel", "OnRefresh() " + ex.Message + "\n");
                LogMessage = "VehicleViewModel.OnRefresh " + ex.Message;
            }
            ErrorCode = FordXDataStore.GetErrorCode();
        }
        private async Task ExecuteInitViewCommand()
        {
            IsBusy = true;
            BusyBee = true;
            LogMessage = "";
            try
            {
                int i = await FordXDataStore.SendCommandAsync(FordCommand.Refresh);
                if (i == 200 || i == 0)
                    LogMessage = "";
                else
                {
                    LogMessage = "ERROR CODE: " + i.ToString();
#if DEBUG
                    Debugger.Log(1, "VehicleViewModel", "ExecuteInitViewCommand ERROR CODE: " + i.ToString() + "\n");
#endif
                }
            }
            catch (Exception ex)
            {
                Debugger.Log(1, "VehicleViewModel", "ExecuteInitViewCommand() " + ex.Message + "\n");
                LogMessage += "VehicleViewModel.ExecuteInitViewCommand " + ex.Message;
            }
            finally
            {
                IsBusy = false;
                BusyBee = false;
            }
            ErrorCode += FordXDataStore.GetErrorCode();
        }

        public async Task ExecuteGetUpdateCommand()
        {
            IsBusy = true;
            BusyBee = true;
            LogMessage = "";
            try
            {
                int i = await FordXDataStore.SendCommandAsync(FordCommand.Refresh);
                if (i == 200 || i == 0)
                    LogMessage = "GetUpdate Command Sent";
                else
                {
                    LogMessage = "ERROR CODE: " + i.ToString();
#if DEBUG
                    Debugger.Log(1, "VehicleViewModel", "GetUpdate ERROR CODE: " + i.ToString() + "\n");
#endif
                }
            }
            catch (Exception ex)
            {
                Debugger.Log(1, "VehicleViewModel", "ExecuteGetUpdateCommand() " + ex.Message + "\n");
                LogMessage += "VehicleViewModel.ExecuteGetUpdateCommand " + ex.Message;
            }
            finally
            {
                IsBusy = false;
            }
            await Task.Delay(6000);
            await ExecuteLoadItem();
            ErrorCode += FordXDataStore.GetErrorCode();
            BusyBee = false;
        }

        public async Task ExecuteLockCommand()
        {
            IsBusy = true;
            BusyBee = true;
            LogMessage = "";
            try
            {
                int i = await FordXDataStore.SendCommandAsync(FordCommand.Lock);
                if (i == 200)
                    LogMessage = "Lock Command Sent";
                else
                {
                    LogMessage = "ERROR CODE: " + i.ToString();
#if DEBUG
                    Debugger.Log(1, "VehicleViewModel", "Lock ERROR CODE: " + i.ToString() + "\n");
#endif
                }
            }
            catch (Exception ex)
            {
                Debugger.Log(1, "VehicleViewModel", "ExecuteLockCommand() " + ex.Message + "\n");
            }
            finally
            {
                BusyBee = false;
                IsBusy = false;
            }
        }

        public async Task ExecuteUnlockCommand()
        {
            IsBusy = true;
            BusyBee = true;
            LogMessage = "";
            try
            {
                int i = await FordXDataStore.SendCommandAsync(FordCommand.Unlock);
                if (i == 200)
                    LogMessage = "Unlock Command Sent";
                else
                {
                    LogMessage = "ERROR CODE: " + i.ToString();
#if DEBUG
                    Debugger.Log(1, "VehicleViewModel", "Unlock ERROR CODE: " + i.ToString() + "\n");
#endif
                }
            }
            catch (Exception ex)
            {
                Debugger.Log(1, "VehicleViewModel", "ExecuteUnlockCommand() " + ex.Message + "\n");
            }
            finally
            {
                BusyBee = false;
                IsBusy = false;
            }
        }

        public async Task ExecuteStartCommand()
        {
            IsBusy = true;
            BusyBee = true;
            LogMessage = "";
            try
            {
                int i = await FordXDataStore.SendCommandAsync(FordCommand.Start);
                if (i == 200)
                    LogMessage = "Start Command Sent";
                else
                {
                    LogMessage = "ERROR CODE: " + i.ToString();
#if DEBUG
                    Debugger.Log(1, "VehicleViewModel", "Start ERROR CODE: " + i.ToString() + "\n");
#endif
                }
            }
            catch (Exception ex)
            {
                Debugger.Log(1, "VehicleViewModel", "ExecuteStartCommand() " + ex.Message + "\n");
            }
            finally
            {
                BusyBee = false;
                IsBusy = false;
            }
        }

        public async Task ExecuteStopCommand()
        {
            IsBusy = true;
            BusyBee = true;
            LogMessage = "";
            try
            {
                int i = await FordXDataStore.SendCommandAsync(FordCommand.Stop);
                if (i == 200)
                    LogMessage = "Stop Command Sent";
                else
                {
                    LogMessage = "ERROR CODE: " + i.ToString();
#if DEBUG
                    Debugger.Log(1, "VehicleViewModel", "Stop ERROR CODE: " + i.ToString() + "\n");
#endif
                }
            }
            catch (Exception ex)
            {
                Debugger.Log(1, "VehicleViewModel", "ExecuteStopCommand() " + ex.Message + "\n");
            }
            finally
            {
                BusyBee = false;
                IsBusy = false;
            }
        }

        public async void OnAppearing()
        {
            LogMessage = "";
            try
            {
                FordInfo ford = await FordXDataStore.GetFordInfoAsync();
                if (ford.Vehicle != null)
                    Vehicle = ford.Vehicle;
                RawJSON = ford.JSONstring;
            }
            catch (Exception ex)
            {
                Debugger.Log(1, "VehicleViewModel", "OnAppearing() " + ex.Message + "\n");
                LogMessage = "VehicleViewModel.OnAppearing " + ex.Message;
            }
            ErrorCode = FordXDataStore.GetErrorCode();
        }

        private async Task OnGoLoginAsync()
        {
            await Shell.Current.GoToAsync(nameof(SettingsPage));
        }
    }
}
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
    public class ActionViewModel : BaseViewModel
    {
        public override void Dispose() => this.Dispose();

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

        public IAsyncCommand RefreshCommand { get; private set; }
        public IAsyncCommand UpdateCommand { get; private set; }
        public IAsyncCommand LockCommand { get; private set; }
        public IAsyncCommand UnlockCommand { get; private set; }
        public IAsyncCommand StartCommand { get; private set; }
        public IAsyncCommand StopCommand { get; private set; }

        public ActionViewModel()
        {

            if (ConfigData.USERNAME == "" || ConfigData.USERNAME == null || ConfigData.PW == "" || ConfigData.PW == null || ConfigData.VIN == "" || ConfigData.VIN == null)
            {
                LogMessage = "FORDPASS USERID, PASSWORD, AND VIN ARE REQUIRED";
                UpdateCommand = new AsyncCommand(OnGoLoginAsync);
                LockCommand = new AsyncCommand(OnGoLoginAsync);
                UnlockCommand = new AsyncCommand(OnGoLoginAsync);
                StartCommand = new AsyncCommand(OnGoLoginAsync);
                StopCommand = new AsyncCommand(OnGoLoginAsync);
                return;
            }
            Title = "Interact With Vehicle";
            UpdateCommand = new AsyncCommand(ExecuteGetUpdateCommand);
            LockCommand = new AsyncCommand(ExecuteLockCommand);
            UnlockCommand = new AsyncCommand(ExecuteUnlockCommand);
            StartCommand = new AsyncCommand(ExecuteStartCommand);
            StopCommand = new AsyncCommand(ExecuteStopCommand);

            ErrorCode = FordXDataStore.GetErrorCode();

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
                    Debugger.Log(1, "ActionViewModel", "GetUpdate ERROR CODE: " + i.ToString() + "\n");
#endif
                }
            }
            catch (Exception ex)
            {
                Debugger.Log(1, "ActionViewModel", "ExecuteGetUpdateCommand() " + ex.Message + "\n");
                LogMessage += "ActionViewModel.ExecuteGetUpdateCommand " + ex.Message;
            }
            finally
            {
                IsBusy = false;
            }
            await Task.Delay(6000);
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
                    Debugger.Log(1, "ActionViewModel", "Lock ERROR CODE: " + i.ToString() + "\n");
#endif
                }
            }
            catch (Exception ex)
            {
                Debugger.Log(1, "ActionViewModel", "ExecuteLockCommand() " + ex.Message + "\n");
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
                    Debugger.Log(1, "ActionViewModel", "Unlock ERROR CODE: " + i.ToString() + "\n");
#endif
                }
            }
            catch (Exception ex)
            {
                Debugger.Log(1, "ActionViewModel", "ExecuteUnlockCommand() " + ex.Message + "\n");
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
                    Debugger.Log(1, "ActionViewModel", "Start ERROR CODE: " + i.ToString() + "\n");
#endif
                }
            }
            catch (Exception ex)
            {
                Debugger.Log(1, "ActionViewModel", "ExecuteStartCommand() " + ex.Message + "\n");
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
                    Debugger.Log(1, "ActionViewModel", "Stop ERROR CODE: " + i.ToString() + "\n");
#endif
                }
            }
            catch (Exception ex)
            {
                Debugger.Log(1, "ActionViewModel", "ExecuteStopCommand() " + ex.Message + "\n");
            }
            finally
            {
                IsBusy = false;
            }
            BusyBee = false;
        }

        private async Task OnGoLoginAsync()
        {
            await Shell.Current.GoToAsync(nameof(SettingsPage));
        }

    }
}

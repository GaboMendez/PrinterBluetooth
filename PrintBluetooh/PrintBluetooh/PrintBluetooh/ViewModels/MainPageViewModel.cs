using PrintBluetooh.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Prism.Services;
using Xamarin.Forms; // Not Used
using DependencyService = Xamarin.Forms.DependencyService; // Not Used

namespace PrintBluetooh.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private IBluetoothService _blueToothService;
        private Printer _printer;

        private IList<string> _deviceList;
        public IList<string> DeviceList
        {
            get
            {
                if (_deviceList == null)
                    _deviceList = new ObservableCollection<string>();
                return _deviceList;
            }
            set
            {
                _deviceList = value;
            }
        }

        private string _printMessage;
        public string PrintMessage
        {
            get
            {
                return _printMessage;
            }
            set
            {
                _printMessage = value;
            }
        }

        private string _selectedDevice;
        public string SelectedDevice
        {
            get
            {
                return _selectedDevice;
            }
            set
            {
                _selectedDevice = value;
            }
        }

        public DelegateCommand PrintCommand { get; set; }

        public MainPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IBluetoothService bluetoothService)
            : base(navigationService, pageDialogService)
        {
            //_blueToothService = DependencyService.Get<IBluetoothService>();

            Title = "Main Page";
            PrintMessage = "";
            _blueToothService = bluetoothService;
            _printer = new Printer(bluetoothService);

            BindDeviceList();

            PrintCommand = new DelegateCommand(async () =>
            {
                if (String.IsNullOrEmpty(SelectedDevice))
                {
                    await PageDialogService.DisplayAlertAsync("Please select a Bluetooh Printer...", null, "Ok");
                }
                else
                {
                    _printer.MyPrinter = SelectedDevice;
                    if (String.IsNullOrEmpty(PrintMessage))
                    {
                        await PageDialogService.DisplayAlertAsync("Field to Print can not be empty...", null, "Ok");
                    }
                    else
                    {
                        await _printer.Reset();
                        await _printer.SetAlignCenter();
                        await _printer.WriteLine($"Normal: {PrintMessage}");
                        await _printer.LineFeed();
                        await _printer.BoldOn();
                        await _printer.WriteLine($"Bold: {PrintMessage}");
                        await _printer.BoldOff();
                        await _printer.LineFeed();
                        await _printer.WriteLine_Big($"Grande: \n{PrintMessage}");
                        await _printer.LineFeed();
                        await _printer.SetUnderLine($"Subrayado: {PrintMessage}");
                        await _printer.LineFeed();
                        await _printer.SetAlignRight();
                        await _printer.WriteLine_Bold("Negrito en la Derecha:");
                        await _printer.BoldOn();
                        await _printer.WriteLine_Bigger($"G1: {PrintMessage}",1);
                        await _printer.BoldOff();
                        await _printer.WriteLine_Bold("Underline...");
                        await _printer.SetUnderLineOn();
                        await _printer.WriteLine_Bigger($"G2: {PrintMessage}", 2);
                        await _printer.WriteLine_Bigger($"G3: {PrintMessage}", 3);
                        await _printer.SetUnderLineOff();
                        await _printer.LineFeed(2);
                        await _printer.SetAlignCenter();
                        await _printer.WriteLine_Bold("Texto Reverse...");
                        await _printer.SetReverseOn();
                        await _printer.WriteLine_Bigger($"Reverse: {PrintMessage}", 1);
                        await _printer.SetReverseOff();
                        await _printer.WriteLine_Bigger($"Not Reverse: {PrintMessage}", 1);
                        await _printer.LineFeed(3);
                        await _printer.Reset();
                    }
                }
            });
        }

        void BindDeviceList()
        {
            var list = _blueToothService.GetDeviceList();
            DeviceList.Clear();
            foreach (var item in list)
                DeviceList.Add(item);
        }
    }
}

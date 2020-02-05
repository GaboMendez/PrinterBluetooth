using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Util;
using PrintBluetooh.Droid.Services;
using PrintBluetooh.Services;

[assembly: Xamarin.Forms.Dependency(typeof(BluetoothService))]
namespace PrintBluetooh.Droid.Services
{
    public class BluetoothService : IBluetoothService
    {
        public IList<string> GetDeviceList()
        {
            using (BluetoothAdapter bluetoothAdapter = BluetoothAdapter.DefaultAdapter)
            {
                var btdevice = bluetoothAdapter?.BondedDevices.Select(i => i.Name).ToList();
                return btdevice;
            }
        }

        public async Task Print(string deviceName, byte[] buffer)
        {
            using (BluetoothAdapter bluetoothAdapter = BluetoothAdapter.DefaultAdapter)
            {
                BluetoothDevice device = (from bd in bluetoothAdapter?.BondedDevices
                    where bd?.Name == deviceName
                    select bd).FirstOrDefault();
                try
                {
                    using (BluetoothSocket bluetoothSocket = device?.
                        CreateRfcommSocketToServiceRecord(
                            UUID.FromString("00001101-0000-1000-8000-00805f9b34fb")))
                    {
                        bluetoothSocket?.Connect();
                        //byte[] buffer = Encoding.UTF8.GetBytes(text);
                        bluetoothSocket?.OutputStream.Write(buffer, 0, buffer.Length);
                        bluetoothSocket.Close();
                    }
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }
    }
}
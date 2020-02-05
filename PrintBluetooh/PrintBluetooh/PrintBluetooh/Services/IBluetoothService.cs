using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PrintBluetooh.Services
{
    public interface IBluetoothService
    {
        IList<string> GetDeviceList();
        Task Print(string deviceName, byte[] buffer);
    }
}

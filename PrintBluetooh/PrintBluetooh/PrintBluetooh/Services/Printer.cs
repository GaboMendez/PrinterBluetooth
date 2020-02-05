using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PrintBluetooh.Services
{
    public class Printer
    {
        private readonly IBluetoothService _blueToothService;

        public string MyPrinter { get; set; }

        public Printer()
        {
            _blueToothService = DependencyService.Get<IBluetoothService>();
        }
        public async Task LineFeed()
        {
            await _writeByte(10);
        }

        public async Task LineFeed(byte lines)
        {
            await _writeByte(27);
            await _writeByte(100);
            await _writeByte(lines);
        }

        public async Task Reset()
        {
            await _writeByte(27);
            await _writeByte(64);
            Thread.Sleep(50);
        }

        public async Task WriteLine(string text)
        {
            await WriteToBuffer(text);
            await _writeByte(10);
        }

        public async Task Write(string text)
        {
            await WriteToBuffer(text);
        }

        public async Task WriteToBuffer(string text)
        {
            text = text.Trim('\n').Trim('\r');
            byte[] originalBytes = System.Text.Encoding.UTF8.GetBytes(text);
            //byte[] outputBytes = System.Text.Encoding.Convert(System.Text.Encoding.UTF8,
            //    System.Text.Encoding.GetEncoding(this.Encoding), originalBytes);

            await _blueToothService.Print(MyPrinter, originalBytes);
            //_serialPort.Write(outputBytes, 0, outputBytes.Length);
        }

        private async Task _writeByte(byte valueToWrite)
        {
            byte[] tempArray = { valueToWrite };
            await _blueToothService.Print(MyPrinter, tempArray);
            //_serialPort.Write(tempArray, 0, 1);
        }

        public async Task WriteLine_Big(string text)
        {
            const byte DoubleHeight = 1 << 4;
            const byte DoubleWidth = 1 << 5;
            //const byte Bold = 1 << 3;

            //big on
            await _writeByte(27);
            await _writeByte(33);
            await _writeByte(DoubleHeight + DoubleWidth);

            //Sends the text
            await WriteLine(text);

            //big off
            await _writeByte(27);
            await _writeByte(33);
            await _writeByte(0);
        }

        public async Task WriteLine_Bigger(string text, byte n)
        {
            //big on
            await _writeByte(29);
            await _writeByte(33);
            await _writeByte(n);

            //Sends the text
            await WriteLine(text);

            //big off
            await _writeByte(29);
            await _writeByte(33);
            await _writeByte(0);
        }
        
        public async Task WriteLine_Bold(string text)
        {
            //bold on
            await BoldOn();

            //Sends the text
            await WriteLine(text);

            //bold off
            await BoldOff();

            await LineFeed();
        }
        public async Task BoldOn()
        {
            await _writeByte(27);
            await _writeByte(32);
            await _writeByte(1);
            await _writeByte(27);
            await _writeByte(69);
            await _writeByte(1);
        }

        public async Task BoldOff()
        {
            await _writeByte(27);
            await _writeByte(32);
            await _writeByte(0);
            await _writeByte(27);
            await _writeByte(69);
            await _writeByte(0);
        }
        public async Task SetAlignLeft()
        {
            await _writeByte(27);
            await _writeByte(97);
            await _writeByte(0);
        }

        public async Task SetAlignCenter()
        {
            await _writeByte(27);
            await _writeByte(97);
            await _writeByte(1);
        }

        public async Task SetAlignRight()
        {
            await _writeByte(27);
            await _writeByte(97);
            await _writeByte(2);
        }

        public async Task SetUnderLine(string text)
        {
            //underline on
            await _writeByte(27);
            await _writeByte(45);
            await _writeByte(1);

            //Sends the text
            await WriteLine(text);

            //underline off
            await _writeByte(27);
            await _writeByte(45);
            await _writeByte(0);
        }
        public async Task SetUnderLineOn()
        {
            //underline on
            await _writeByte(27);
            await _writeByte(45);
            await _writeByte(1);
        }
        public async Task SetUnderLineOff()
        {
            //underline off
            await _writeByte(27);
            await _writeByte(45);
            await _writeByte(0);
        }
        
        public async Task SetReverseOn()
        {
            await _writeByte(29);
            await _writeByte(66);
            await _writeByte(1);
        }

        public async Task SetReverseOff()
        {
            await _writeByte(29);
            await _writeByte(66);
            await _writeByte(0);
        }
    }
}

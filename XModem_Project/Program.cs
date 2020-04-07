using System;
using System.IO;
using System.IO.Ports;

namespace XModem_Project
{
    class Program
    {
        static void Main(string[] args)
        { 
			int err = 0;
			
			/*SerialPort 연결*/
            XModem xModem = new XModem();
			SerialPort SP = new SerialPort();
			
			/*File 연결*/
			FileStream fs = File.Open(@"D:\maat5_ohcl_obu_etcs_20-02-24_18070285.bin.pkr", FileMode.Open);
			BinaryReader b_reader = new BinaryReader(fs);
			
			SP.PortName = "COM3";
			SP.BaudRate = (int)115200;
			SP.DataBits = (int)8;
			SP.Parity = Parity.None;
			SP.StopBits = StopBits.One;
			SP.ReadTimeout = (int)500;
			SP.WriteTimeout = (int)500;

            err = xModem.init_xmodem(SP);
			if(err == 1)
			{
				Console.WriteLine("xModem init");
			}
			else
			{
				Console.WriteLine("xModem init err");
				return;
			}
			
			
        }
    }
}

using System;

namespace XModem_Project
{
    class Program
    {
        static void Main(string[] args)
        {
            XModem xModem = new XModem();

            xModem.init_xmodem();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace XModem_Project
{
    class XModem
    {
        public void init_xmodem()
        {
            this.wait_c();
            Console.WriteLine("get c");
        }

        /*
         * XModem 시작 
         */
        private void wait_c()
        {
            char readchar;
            while (true)
            {
                readchar = (char)Console.Read();
                Console.WriteLine(readchar);
                if(readchar == 'c')
                {
                    break;
                }
            }
        }
    }
}

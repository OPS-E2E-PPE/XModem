using System;
using System.Collections.Generic;
using System.Text;

namespace XModem_Project
{
    static class Constants
    {
        public const byte SOH = 0x01;
        public const byte EOT = 0x01;
        public const byte ETB = 0x01;

        public const byte ACK = 0x01;
        public const byte NAK = 0x01;
        
        public const byte CAN = 0x01;
    }

    class XModem
    {
        UInt16 Crc_byte = 0;

        byte[] Sender_Packet = new byte[133];

        static byte Sender_Packet_Number = 0;
        byte[] Sender_Data = new byte[128];

        public void init_xmodem()
        {
            this.wait_c();

            for(int i = 0; i < 5; i++)
            {
                Sender_Data[0]++;
                Sender_Data[127]++;
                Send_Packet(Sender_Data);
            }
        }

        /*
         * XModem 시작 문자 대기 'C'
         */
        private void wait_c()
        {
            char readchar;
            while (true)
            {
                readchar = (char)Console.Read();
                //Console.WriteLine(readchar);
                if(readchar == 'C')
                {
                    break;
                }
            }
        }

        /*
         * make packet and send
         */
        private void Send_Packet(byte[] data)
        {
            Sender_Packet[0] = Constants.SOH;

            /*255 넘으면 알아서 0 되겠지*/
            Sender_Packet_Number++;
            Sender_Packet[1] = Sender_Packet_Number;

            /*byte -> int 형변환 때문에 더하기 빼기도 어렵네...*/
            Sender_Packet[2] = BitConverter.GetBytes(255 - Sender_Packet_Number)[0];

            /*C 라면 [3]주소에다가 그냥 memcpy 하면 되는데... Buffer.BlockCopy 가 있네*/
            Buffer.BlockCopy(data, 0, Sender_Packet, 3, 128);

            Crc_byte = crc16.ComputeCrc(data);
            Buffer.BlockCopy(BitConverter.GetBytes(Crc_byte), 0, Sender_Packet, 131, 2);

            Console.WriteLine(BitConverter.ToString(Sender_Packet));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Ports;

namespace XModem_Project
{
    static class Constants
    {
        public const byte SOH = 0x01;
		public const byte SOX = 0x02;
        public const byte EOT = 0x01;
        public const byte ETB = 0x01;

        public const byte ACK = 0x01;
        public const byte NAK = 0x01;
        
        public const byte CAN = 0x01;
    }

    class XModem
    {
        //UInt16 Crc_byte = 0;

        byte[] Sender_Packet = new byte[3];
		byte[] Sender_Data = new byte[1024];
		UInt16 Sender_Crc = new UInt16();
		
		SerialPort SPort;
		FileStream FStream;

        byte Sender_Packet_Number = 1;
		
		/*
		 * 수정 시작 : init 할 때 시리얼 포트 정보를 받는다.
		 */
        public int init_xmodem(SerialPort Port)
        {
			SPort = Port;
			
			SPort.Open();
			if(SPort.IsOpen)
			{
				return 1;
			}
			else
			{
				return 0;
			}
        }
		
		/*
		 * 파일을 받아서 송신하는 것을 전부 관장함.
		 */
		public void xmodem_send()
		{
			this.wait_c();
			//Send_Packet();
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

        /********************************************************
		 * send 형식 바꿀 때는 이 곳을 변경
         * make packet and send
         */
        private void Send_Packet(byte[] data, byte SPN)
        {
            Sender_Packet[0] = Constants.SOX;

            /*255 넘으면 알아서 0 되겠지*/
            Sender_Packet[1] = SPN;

            /*byte -> int 형변환 때문에 더하기 빼기도 어렵네...*/
            Sender_Packet[2] = BitConverter.GetBytes(255 - SPN)[0];

            /*C 라면 [3]주소에다가 그냥 memcpy 하면 되는데... Buffer.BlockCopy 가 있네. 잘만들었네 좋다.*/
            //Buffer.BlockCopy(data, 0, Sender_Packet, 3, 128);  //따로 보낼 것임.
			
			Console.WriteLine(BitConverter.ToString(Sender_Packet));
			Console.WriteLine(BitConverter.ToString(data));
			
            Sender_Crc = crc16.ComputeCrc(data);
			Console.WriteLine(Sender_Crc);
        }

        private int Wait_ACK_NAK()
        {
			int Console_read;

            while(true)
            {
                Console_read = Console.Read();
			
                if(Console_read == 0x30) 		//Constants.NAK
                {
                    return 0;
                }
                else if(Console_read == 0x31) 	//Constants.ACK
                {
                    return 1;
                }
                else
                {;}
            }
        }

        //private byte 
    }
}

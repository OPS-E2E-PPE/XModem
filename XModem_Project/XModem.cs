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
        public const byte EOT = 0x04;
        public const byte ETB = 0x17;

        public const byte ACK = 0x06;
        public const byte NAK = 0x15;
        
        public const byte CAN = 0x18;
    }

    class XModem
    {
        //UInt16 Crc_byte = 0;

        byte[] Sender_Packet = new byte[3];
		byte[] Sender_Data = new byte[1024];
		UInt16 Sender_Crc = new UInt16();
		
		SerialPort SPort;

        byte Sender_Packet_Number = 0;
		
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
		
		/************************************************
		 * 파일을 받아서 송신하는 것을 전부 관장함.
		 */
		public void xmodem_send(BinaryReader B_reader)
		{
			int err = 0;
			Sender_Packet_Number = 1;
			this.wait_c();
			
			Sender_Data = B_reader.ReadBytes(128);
			err = Send_Packet(Sender_Data, Sender_Packet_Number, 128);
			while(true)
			{
				if(err == 1)
				{
					Sender_Data = B_reader.ReadBytes(1024);
					
					if(Sender_Data.Length == 0)
					{
						break;
					}
					else if(Sender_Data.Length != 1024)
					{
						byte[] full_stream = new byte[1024];
						byte[] zero_ary = new byte[1024 - Sender_Data.Length];
						Array.Clear(zero_ary, 0, zero_ary.Length);
						
						Console.WriteLine(Sender_Data.Length);
						
						Array.Copy(Sender_Data, 0, full_stream, 0, Sender_Data.Length);
						Array.Copy(zero_ary, 0, full_stream, Sender_Data.Length, zero_ary.Length);
						
						Sender_Data = full_stream;
					}
					
					Sender_Packet_Number++;
					err = Send_Packet(Sender_Data, Sender_Packet_Number, 1024);
				}
				else
				{
					err = Send_Packet(Sender_Data, Sender_Packet_Number, err);
				}
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
                readchar = (char)SPort.ReadChar();
                Console.WriteLine(readchar);
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
        private int Send_Packet(byte[] data, byte SPN, int Length)
        {
			if(Length == 128)
			{
				Sender_Packet[0] = Constants.SOH;
			}
			else if(Length == 1024)
			{
				Sender_Packet[0] = Constants.SOX;
			}
            Sender_Packet[1] = SPN;

            /*byte -> int 형변환 때문에 더하기 빼기도 어렵네...*/
            Sender_Packet[2] = BitConverter.GetBytes(255 - SPN)[0];

			Console.WriteLine(BitConverter.ToString(Sender_Packet));
			SPort.Write(Sender_Packet, 0, 3);
			
			Console.WriteLine(BitConverter.ToString(data));
			SPort.Write(data, 0, Length);

            Sender_Crc = crc16.ComputeCrc(data);
			Console.WriteLine(Sender_Crc);
			SPort.Write(BitConverter.GetBytes(Sender_Crc), 0, 2);
			
			return Wait_ACK_NAK(Length);
        }

        private int Wait_ACK_NAK(int Length)
        {
			int SPort_read;

            while(true)
            {
				Console.WriteLine("Wait_ACK_NAK");
                SPort_read = SPort.ReadChar();//Console.Read();

                if(SPort_read == Constants.NAK) 		//Constants.NAK
                {
					Console.WriteLine("NAK");
                    return Length;
                }
                else if(SPort_read == Constants.ACK) 	//Constants.ACK
                {
					Console.WriteLine("ACK");
                    return 1;
                }
                else
                {
					Console.WriteLine(SPort_read);
				}
            }
        }

        //private byte 
    }
}

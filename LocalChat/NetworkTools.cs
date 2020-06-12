using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Threading;

namespace LocalChat
{
    static class NetworkTools
    {

        public static List<Thread> FindOtherServer()
        {
            var threads = new List<Thread>();

            // доступно ли сетевое подключение
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable()) return threads;

            foreach (var ip in Dns.GetHostAddresses(Dns.GetHostName()).Where(ha => ha.AddressFamily == AddressFamily.InterNetwork))
            {
                //each ip-v4
                
                string IP = ip.ToString();
                string mask = GetSubnetMask(ip).ToString();
                string[] arrayIP = NetworkTools.generateAllLocalAdress(IP, mask); //IP.Split('.');

                for (int i = 0; i < arrayIP.Length; i++)
                {
                    if (arrayIP[i] == "" || arrayIP[i] == IP) continue;
                    Thread tr = new Thread(new ParameterizedThreadStart(CheckIP));
                    tr.Start(arrayIP[i]);
                    threads.Add(tr);
                }
            }
            return threads;
        }

        private static string[] generateAllLocalAdress(string IP, string Mask)
        {
            uint adress = NetworkTools.stringToint(IP), mask = NetworkTools.stringToint(Mask);
            uint baseAdress = adress & mask;
            uint maxNumber = ~mask;

            string[] allAdress = new string[maxNumber - 1];

            for (uint i = 1; i < maxNumber; i++)
                allAdress[i - 1] = NetworkTools.intToString(baseAdress + i);

            return allAdress;
        }

        private static uint stringToint(string adress)
        {
            string[] buffer = adress.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
            if (buffer.Length == 4)
            {
                byte[] buff_ip = new byte[buffer.Length];
                for (int i = buffer.Length - 1; i >= 0; i--)
                    buff_ip[i] = Convert.ToByte(buffer[buffer.Length - 1 - i]);

                return BitConverter.ToUInt32(buff_ip, 0);
            }
            return 0;
        }

        private static string intToString(uint adress)
        {
            byte[] buffer = BitConverter.GetBytes(adress);
            if (buffer.Length == 4)
            {
                string IP = Convert.ToString(buffer[3]);
                for (int i = 2; i >= 0; i--)
                    IP += "." + Convert.ToString(buffer[i]);

                return IP;
            }
            return "";
        }

        private static IPAddress GetSubnetMask(IPAddress address)
        {
            foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                foreach (UnicastIPAddressInformation unicastIPAddressInformation in adapter.GetIPProperties().UnicastAddresses)
                {
                    if (unicastIPAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        if (address.Equals(unicastIPAddressInformation.Address))
                        {
                            return unicastIPAddressInformation.IPv4Mask;
                        }
                    }
                }
            }
            throw new ArgumentException("Can't find subnetmask for IP address '{address}'");
        }

        public static void CheckIP(object obj)
        {
            string ip = (string)obj;

            try
            {
                if (Dns.GetHostEntry(ip).HostName != null)
                {
                    TcpClient any = new TcpClient(ip, Server.getPort());
                    Server.Connect(any);
                }
            }
            catch{ }
        }
    }
}

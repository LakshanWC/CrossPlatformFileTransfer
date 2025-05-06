using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace FileTransferSoftware.Service_Layer
{
    public class UDPBroadcast
    {
        private const int discoveryPort = 8888;

        public static List<string> discoverDevices()
        {
            List<string> discoveredDevices = new List<string>();

            try
            {
                using (UdpClient udpClient = new UdpClient())
                {
                    udpClient.EnableBroadcast = true;
                    IPEndPoint endPoint = new IPEndPoint(IPAddress.Broadcast, discoveryPort);

                    // Send discovery message
                    string discoveryMessage = "DISCOVER_FILETRANSFER";
                    byte[] data = Encoding.UTF8.GetBytes(discoveryMessage);
                    udpClient.Send(data, data.Length, endPoint);

                    Console.WriteLine("Discovery message sent. Waiting for responses...");

                    udpClient.Client.ReceiveTimeout = 5000;

                    DateTime start = DateTime.Now;
                    while ((DateTime.Now - start).TotalMilliseconds < 5000)
                    {
                        try
                        {
                            IPEndPoint receiveEndPoint = new IPEndPoint(IPAddress.Any, 0);
                            byte[] responseData = udpClient.Receive(ref receiveEndPoint);
                            string responseMessage = Encoding.UTF8.GetString(responseData);

                            Console.WriteLine($"Received response: {responseMessage}");

                            if (!discoveredDevices.Contains(responseMessage))
                                discoveredDevices.Add(responseMessage);
                        }
                        catch (SocketException ex) when (ex.SocketErrorCode == SocketError.TimedOut)
                        {
                            break;
                        }
                    }

                    Console.WriteLine("Discovery complete.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during discovery: {ex.Message}");
            }

            return discoveredDevices;
        }
    }
}

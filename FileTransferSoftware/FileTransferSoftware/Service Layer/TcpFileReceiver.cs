using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace FileTransferSoftware.Service_Layer
{
    class TcpFileReceiver
    {
        private const int Port = 5000;

        public static void Start()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, Port);
            listener.Start();
            Console.WriteLine($"TCP File Receiver listening on port {Port}...");

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("Client connected.");
                Task.Run(() => HandleClient(client));
            }
        }

        private static void HandleClient(TcpClient client)
        {
            NetworkStream networkStream = null;
            FileStream fs = null;

            try
            {
                networkStream = client.GetStream();

                // Read 2 bytes filename length (big-endian)
                int len1 = networkStream.ReadByte();
                int len2 = networkStream.ReadByte();
                if (len1 == -1 || len2 == -1)
                    throw new Exception("Failed to read filename length.");

                int fileNameLength = (len1 << 8) | len2;

                // Read filename bytes
                byte[] fileNameBytes = new byte[fileNameLength];
                int read = 0;
                while (read < fileNameLength)
                {
                    int bytesRead = networkStream.Read(fileNameBytes, read, fileNameLength - read);
                    if (bytesRead == 0)
                        throw new Exception("Disconnected while reading filename.");
                    read += bytesRead;
                }
                string fileName = System.Text.Encoding.UTF8.GetString(fileNameBytes);
                Console.WriteLine($"Receiving file: {fileName}");

                // Prepare save path (Downloads folder)
                string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
                if (!Directory.Exists(downloadsPath))
                    Directory.CreateDirectory(downloadsPath);

                string savePath = Path.Combine(downloadsPath, fileName);

                // Read file data and write to file
                fs = new FileStream(savePath, FileMode.Create, FileAccess.Write);

                byte[] buffer = new byte[4096];
                int bytesReadFile;
                long totalBytes = 0;

                while ((bytesReadFile = networkStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    fs.Write(buffer, 0, bytesReadFile);
                    totalBytes += bytesReadFile;
                }

                Console.WriteLine($"File saved: {savePath} ({totalBytes} bytes)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                if (fs != null)
                    fs.Close();

                if (networkStream != null)
                    networkStream.Close();

                client.Close();
            }
        }

        public static void Start(CancellationToken cancellationToken)
        {
            TcpListener listener = new TcpListener(IPAddress.Any, Port);
            listener.Start();
            Console.WriteLine($"TCP File Receiver listening on port {Port}...");

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    if (listener.Pending())
                    {
                        TcpClient client = listener.AcceptTcpClient();
                        Console.WriteLine("Client connected.");
                        Task.Run(() => HandleClient(client));
                    }
                    else
                    {
                        Thread.Sleep(100); // Avoid busy waiting
                    }
                }
            }
            finally
            {
                listener.Stop();
                Console.WriteLine("TCP Server stopped.");
            }
        }
    }
}

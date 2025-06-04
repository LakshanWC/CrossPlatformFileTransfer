using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public class FileSender
{
    public static async Task<double> SendFileOverTcpAsync(
     string deviceIp, int port, string filePath,
     Action<int> progressCallback = null,
     Action<double> speedCallback = null)
    {
        Stopwatch totalStopwatch = Stopwatch.StartNew();
        try
        {
            if (!File.Exists(filePath))
            {
                MessageBox.Show("File not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }

            using (TcpClient client = new TcpClient())
            {
                await client.ConnectAsync(deviceIp, port);

                using (NetworkStream networkStream = client.GetStream())
                {
                    // Send file name
                    string fileName = Path.GetFileName(filePath);
                    byte[] fileNameBytes = Encoding.UTF8.GetBytes(fileName);
                    byte[] lengthBytes = BitConverter.GetBytes((ushort)fileNameBytes.Length);
                    if (BitConverter.IsLittleEndian) Array.Reverse(lengthBytes);

                    await networkStream.WriteAsync(lengthBytes, 0, 2);
                    await networkStream.WriteAsync(fileNameBytes, 0, fileNameBytes.Length);

                    using (FileStream fileStream = File.OpenRead(filePath))
                    {
                        long fileLength = fileStream.Length;
                        byte[] buffer = new byte[1024 * 1024]; // 64 KB
                        long totalBytesRead = 0;
                        long bytesSinceLastCheck = 0;
                        int bytesRead;

                        Stopwatch stopwatch = Stopwatch.StartNew();

                        while ((bytesRead = await fileStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                        {
                            await networkStream.WriteAsync(buffer, 0, bytesRead);
                            totalBytesRead += bytesRead;
                            bytesSinceLastCheck += bytesRead;

                            // Progress
                            int progress = (int)((totalBytesRead * 100) / fileLength);
                            progressCallback?.Invoke(progress);

                            // Report speed every second
                            if (stopwatch.ElapsedMilliseconds >= 1000)
                            {
                                double seconds = stopwatch.ElapsedMilliseconds / 1000.0;
                                double speedMBps = (bytesSinceLastCheck / 1024.0 / 1024.0) / seconds;

                                speedCallback?.Invoke(speedMBps);

                                stopwatch.Restart();
                                bytesSinceLastCheck = 0;
                            }
                        }

                        // Final speed update (if needed)
                        stopwatch.Stop();
                        if (stopwatch.ElapsedMilliseconds > 0 && bytesSinceLastCheck > 0)
                        {
                            double seconds = stopwatch.ElapsedMilliseconds / 1000.0;
                            double finalSpeedMBps = (bytesSinceLastCheck / 1024.0 / 1024.0) / seconds;
                            speedCallback?.Invoke(finalSpeedMBps);
                        }
                    }
                }

                progressCallback?.Invoke(100);
                totalStopwatch.Stop();
                double totalSeconds = totalStopwatch.Elapsed.TotalSeconds;

                DialogResult result = MessageBox.Show(
                    $"File transferred successfully!\nTime taken: {totalSeconds:F2} seconds",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                if (result == DialogResult.OK)
                {
                    progressCallback?.Invoke(0);
                }
                return 0;

            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error sending file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            progressCallback?.Invoke(-1);
            return -1;
        }
    }
}

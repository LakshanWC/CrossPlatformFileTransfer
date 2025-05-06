using FileTransferSoftware.Session;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileTransferSoftware.Service_Layer
{
    public class HomeService
    {
        private List<string> messageList = new List<string>();
        private List<string> encryptedDeviceIpList;
        private List<string> decryptedDeviceIpList;
        private List<string> fileDetailsList = new List<string>();

        public bool StartHttpServer(string[] prefixes)
        {
            try
            {
                HttpListenerService.StartHttpListener(prefixes);
                var sessionSettings = SessionSettings.getSessionSettingsInstent();
                sessionSettings.isHttpListenerOn = true;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<string> getFileDetails(String path,bool isFolder)
        {
            long flieSize;
            try
            {
                if (isFolder)
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(path);
                    flieSize = getDirectorySize(directoryInfo);
                    fileDetailsList.Add(directoryInfo.Name);

                    if (flieSize == -1)
                    {
                        fileDetailsList.Add("0.0 Bytes");
                    }
                    else
                    {
                        fileDetailsList.Add(formatSize(flieSize));
                    }

                    fileDetailsList.Add(Convert.ToString(directoryInfo.CreationTime));
                    return fileDetailsList;
                }
                else
                {
                    FileInfo fileInfo = new FileInfo(path);
                    fileDetailsList.Add(fileInfo.Name);
                    fileDetailsList.Add(formatSize((int)fileInfo.Length));
                    fileDetailsList.Add(Convert.ToString(fileInfo.CreationTime));

                    return fileDetailsList;
                }
            }
            catch (Exception e) 
            { 
                Console.WriteLine("Error Getting FileDetails"+e.ToString());
                return null;
            }
        }

        private long getDirectorySize(DirectoryInfo directoryInfo)
        {
            try
            {
                long size = 0;
                if (directoryInfo.Exists)
                {
                    foreach (FileInfo file in directoryInfo.GetFiles()) { size += file.Length; }

                    DirectoryInfo[] subDirectories = directoryInfo.GetDirectories();
                    foreach (var subDirectory in subDirectories)
                    {
                        size += getDirectorySize(subDirectory);
                    }
                    return size;
                }
                return 0;
            }
            catch (Exception ex) 
            {
                Console.WriteLine("Error getting DirectorySize: "+ex.ToString());
                return -1;
            }
        }

        private string formatSize(long bytes)
        {
            if (bytes >= 1024 * 1024 * 1024)
                return (bytes / (1024.0 * 1024 * 1024)).ToString("0.00") + " GB";
            else if (bytes >= 1024 * 1024)
                return (bytes / (1024.0 * 1024)).ToString("0.00") + " MB";
            else if (bytes >= 1024)
                return (bytes / 1024.0).ToString("0.00") + " KB";
            else
                return bytes + " Bytes";
        }

        /*
        private void decryptIp(List<string> message)
        {
            try
            {
                if(encryptedDeviceIpList != null) { encryptedDeviceIpList.Clear();}
                encryptedDeviceIpList = UDPBroadcast.discoverDevices();
                foreach(var mess in message)
                {
                    var encryptedPart = mess.Substring(mess.LastIndexOf("|") + 1);
                    encryptedDeviceIpList.Add(encryptedPart.ToString());
                }

                if (decryptedDeviceIpList != null) { decryptedDeviceIpList.Clear(); }
                foreach (var ip in encryptedDeviceIpList)
                {
                    decryptedDeviceIpList.Add(EncryptionHelper.Decrypt(ip).ToString());
                }

            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.ToString());
            }
        }*/

        public async void sendFlie(string encryptedIp,string selectedFilePath)
        {
            try
            {
                Console.WriteLine("sending file " +encryptedIp);
               

                string deviceIp = EncryptionHelper.Decrypt(encryptedIp).ToString();

                Console.WriteLine("i found this ip form here: "+deviceIp);
                string filePath = selectedFilePath;

                using (var client = new HttpClient())
                {
                    var content = new MultipartFormDataContent();
                    var fileContent = new ByteArrayContent(File.ReadAllBytes(filePath));
                    fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                    content.Add(fileContent, "uploadedFile", Path.GetFileName(filePath));

                    // Replace with actual IP and endpoint
                    HttpResponseMessage response = await client.PutAsync($"http://{deviceIp}:8080", content);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("File sent successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Failed to send file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}

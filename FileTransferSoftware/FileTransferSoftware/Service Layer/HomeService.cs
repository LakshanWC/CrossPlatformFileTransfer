using FileTransferSoftware.Session;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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

        public async void sendFile(string encryptedIp, string selectedFilePath)
        {
            try
            {
                string deviceIp = EncryptionHelper.Decrypt(encryptedIp).ToString();
                string filePath = selectedFilePath;

                if (!File.Exists(filePath))
                {
                    MessageBox.Show("File not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (var client = new HttpClient())
                {
                    var content = new MultipartFormDataContent("UploadBoundary");

                    // Read file
                    byte[] fileBytes = File.ReadAllBytes(filePath);
                    var fileContent = new ByteArrayContent(fileBytes);
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                    // Add with correct key and filename
                    string fileName = Path.GetFileName(filePath);
                    content.Add(fileContent, "uploadedFile", fileName);

                    Console.WriteLine($"Sending to: http://{deviceIp}:8080");
                    Console.WriteLine($"Key: uploadedFile | Filename: {fileName}");

                    // Send request
                    var response = await client.PostAsync($"http://{deviceIp}:8080", content);
                    var responseText = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("File sent successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Server rejected: {responseText}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

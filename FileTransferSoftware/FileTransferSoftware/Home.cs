using FileTransferSoftware.Service_Layer;
using FileTransferSoftware.Session;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileTransferSoftware
{
    public partial class Home : Form
    {
        private SessionSettings sessionSettings;
        private HomeService homeService = new HomeService();
        private bool isFolder;
        private List<string> fileDetails;
        private List<string> devices;
        private Panel panelFill = new Panel();
        private bool isServerOn = false;

        public Home()
        {
            InitializeComponent();
            init();
        }

        private void btn_start_server_Click(object sender, EventArgs e)
        {
            if (!isServerOn) 
            {
                btn_server_stat.Text = "Server Status: Online";
                isServerOn = true;
                Task.Run(() =>
                {
                    try
                    {
                        HttpListenerService.StartHttpListener(new[] { "http://localhost:5000/" });
                        this.Invoke((MethodInvoker)delegate
                        {
                            btn_start_server.BackColor = Color.GreenYellow;
                            MessageBox.Show("Server started and ready to go", "Server info",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        });

                        UDPBroadcast.startListning();
                        Console.WriteLine("Start Listing");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        this.Invoke((MethodInvoker)delegate
                        {
                            btn_start_server.BackColor = Color.Red;
                        });
                    }
                });
            } else if (isServerOn)
            {
                btn_server_stat.Text = "Server Status: Offline";
                HttpListenerService.StopHttpListener();
                btn_start_server.BackColor = SystemColors.Control;
                isServerOn=false;
                MessageBox.Show("Server stopped", "Server info",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btn_choose_file_Click(object sender, EventArgs e)
        {
            if(isFolder)
            {
                using(FolderBrowserDialog folderDialog = new FolderBrowserDialog())
                {
                    if (folderDialog.ShowDialog() == DialogResult.OK)
                    {
                        txt_file_path.Text = folderDialog.SelectedPath;
                        if (homeService.getFileDetails(txt_file_path.Text, isFolder) == null) { }
                        else
                        {
                            //fileDetails = homeService.getFileDetails(txt_file_path.Text,isFolder);
                            btn_send.Text = "Send File";
                        }
                    }
                }
            }
            else
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "All Files (*.*)|*.*";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        txt_file_path.Text = openFileDialog.FileName;
                        if (homeService.getFileDetails(txt_file_path.Text, isFolder) == null) { }
                        else
                        {
                            // fileDetails = homeService.getFileDetails(txt_file_path.Text, isFolder);
                            btn_send.Text ="Send File";
                        }
                    }
                }
            }
        }

        private void init()
        {
            btn_choose_file.Enabled = false;
            homeService.setRoundConners(custom_pgb,8);

            panelFill.Parent = custom_pgb;
            panelFill.BackColor = ColorTranslator.FromHtml("#408EE0");
            panelFill.Location = new Point(0, 0);
            panelFill.Height = custom_pgb.Height;
            panelFill.Width = 0; // initial

            progressBarStartUp();
        }

        private async void btn_send_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_file_path.Text))
            {
                int selected = cmb_devices.SelectedIndex;

                // Ensure devices list is populated
                devices = UDPBroadcast.discoverDevices();

                if (selected >= 0 && selected < devices.Count)
                {
                    var selectedDevice = devices[selected];
                    Console.WriteLine("i got this: " + selectedDevice);

                    // Extract the encrypted IP part after "|"
                    string encrypted = selectedDevice.Substring(selectedDevice.LastIndexOf("|") + 1);
                    Console.WriteLine("Extracted this: " + encrypted);

                    string decryptedIp = EncryptionHelper.Decrypt(encrypted);  
                    string selectedFilePath = txt_file_path.Text.Trim();  


                    if (string.IsNullOrEmpty(decryptedIp) || string.IsNullOrEmpty(selectedFilePath))
                    {
                        MessageBox.Show("Please select a device and file.");
                        return;
                    }

                    int port = 5000; // same port your Android ServerSocket will listen on

                    double result = await FileSender.SendFileOverTcpAsync(decryptedIp, port, selectedFilePath,
                        progress =>
                        {
                            //pb_file_upload_progress.Invoke((MethodInvoker)(() => pb_file_upload_progress.Value = progress));
                            homeService.fillProgressBar(custom_pgb,panelFill,lbl_progressCount,progress);
                        },
                        speed =>
                        {
                            Console.WriteLine(speed);
                            lbl_speed.Invoke((MethodInvoker)(() => lbl_speed.Text = $"Speed: {speed:F2} MB/s"));
                        });
                }
                else
                {
                    Console.WriteLine("No device selected or invalid index");
                }
            }
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                cmb_devices.Items.Clear();

                    devices = UDPBroadcast.discoverDevices();
                    foreach (var device in devices)
                    {
                        if (!string.IsNullOrWhiteSpace(device) && !cmb_devices.Items.Contains(device))
                        {
                            
                            cmb_devices.Items.Add(device.Substring(0,device.LastIndexOf("|")));
                        }

                        Console.WriteLine("i got this: "+device);
                        string myString = device.ToString();
                        Console.WriteLine("Extracted this " +device.Substring(device.LastIndexOf("|")+1));
                    }

                    cmb_devices.SelectedIndex = 0;
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void rbtn_file_CheckedChanged(object sender, EventArgs e)
        {
            isFolder = false;
            btn_choose_file.Enabled = true;
        }

        private void rbtn_folder_CheckedChanged(object sender, EventArgs e)
        {
            isFolder = true;
            btn_choose_file.Enabled=true;
        }

        private async void progressBarStartUp()
        {
            for (int i = 0; i <=100; i++)
            {
                homeService.fillProgressBar(custom_pgb, panelFill,lbl_progressCount, i);
                await Task.Delay(10);
            }
            for (int i = 100; i >=0; i--)
            {
                homeService.fillProgressBar(custom_pgb, panelFill,lbl_progressCount, i);
                await Task.Delay(10);
            }
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            txt_file_path.Clear();
            rbtn_file.Checked = false;
            rbtn_folder.Checked = false;
            isFolder = false;
            btn_choose_file.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

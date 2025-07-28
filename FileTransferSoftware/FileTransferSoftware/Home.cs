using FileTransferSoftware.Service_Layer;
using FileTransferSoftware.Session;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileTransferSoftware
{
    public partial class Home : Form
    {
        private SessionSettings sessionSettings;
        private HomeService homeService = new HomeService();
        private Queue fileTransferQueue = new Queue();
        private bool isFolder;
        private List<string> devices;
        private Panel panelFill = new Panel();
        private bool isServerOn = false;
        private int port = 5000;

        public Home()
        {
            InitializeComponent();
            init();
        }

        private CancellationTokenSource tcpServerCancellationTokenSource;

        private void btn_start_server_Click(object sender, EventArgs e)
        {
        }


        private void btn_choose_file_Click(object sender, EventArgs e)
        {
            if(isFolder)
            {
                using(FolderBrowserDialog folderDialog = new FolderBrowserDialog())
                {
                    if (folderDialog.ShowDialog() == DialogResult.OK)
                    {
                        txt_file_path.Text = Path.GetFileName(folderDialog.SelectedPath);
                        if (homeService.getFileDetails(folderDialog.SelectedPath, isFolder) == null) { }
                        else
                        {
                            //fileDetails = homeService.getFileDetails(txt_file_path.Text,isFolder);
                            fileTransferQueue.enqueue(folderDialog.SelectedPath, "Pending");
                            showQueue(fileTransferQueue.getQueue());
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

                        txt_file_path.Text = ((openFileDialog.FileName.Substring(openFileDialog.FileName.LastIndexOf('\\')+1)));


                        if (homeService.getFileDetails(openFileDialog.FileName, isFolder) == null) { }
                        else
                        {
                            // fileDetails = homeService.getFileDetails(txt_file_path.Text, isFolder);
                            fileTransferQueue.enqueue(openFileDialog.FileName, "Pending");
                            showQueue(fileTransferQueue.getQueue());
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
            fileTransferQueue.clearQueue();
            panel_files.Controls.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)
        {

            if (!isServerOn)
            {
                btn_server_stat.Text = "Listing to Discovery Message";
                isServerOn = true;

                tcpServerCancellationTokenSource = new CancellationTokenSource();

                Task.Run(() =>
                {
                    try
                    {
                        // Start TCP server with cancellation token to allow stopping
                        TcpFileReceiver.Start(tcpServerCancellationTokenSource.Token);
                        UDPBroadcast.startListning();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                });
            }
            else if (isServerOn)
            {
                btn_server_stat.Text = "Stoped Listing to Discovery Message";

                // Stop the TCP server by cancelling
                tcpServerCancellationTokenSource?.Cancel();
            }
        }

        private void btn_onClick(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null && btn.Parent != null)
            {
                panel_files.Controls.Remove(btn.Parent); // Remove the whole row 
            }
        }


        private void showQueue(List<QueueItems> queueItems)
        {
            int yOffSet = 0;
            panel_files.Controls.Clear();

            if (queueItems == null || queueItems.Count == 0)
            {
                Console.WriteLine("No File Available to Display");
            }
            else
            {
                foreach (QueueItems item in queueItems)
                {

                    Panel rowPanel = new Panel();
                    rowPanel.Size = new Size(panel_files.Width - 8, 25);
                    rowPanel.Location = new Point(0, yOffSet);

                    // Button
                    Button btn = new Button();
                    btn.Text = "X";
                    btn.Size = new Size(20, 20);
                    btn.Location = new Point(5, 2);
                    btn.Click += btn_onClick; // Will remove row for the corresponding button

                    // File path label
                    Label lbl_fileName = new Label();
                    lbl_fileName.Text = item.filePath.ToString();
                    lbl_fileName.AutoSize = true;
                    lbl_fileName.Location = new Point(40, 7);

                    // Status label
                    Label lbl_status = new Label();
                    lbl_status.Text = item.transferStatus.ToString();
                    lbl_status.AutoSize = true;
                    lbl_status.Location = new Point(330, 7);

                    rowPanel.Controls.Add(btn);
                    rowPanel.Controls.Add(lbl_fileName);
                    rowPanel.Controls.Add(lbl_status);

                    // Add row panel to main panel
                    panel_files.Controls.Add(rowPanel);

                    yOffSet += 20; // row spacing
                }
            }
        }
    }
}

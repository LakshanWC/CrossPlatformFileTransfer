namespace FileTransferSoftware
{
    partial class Home
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Home));
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btn_choose_file = new System.Windows.Forms.Button();
            this.txt_file_path = new System.Windows.Forms.TextBox();
            this.btn_send = new System.Windows.Forms.Button();
            this.cmb_devices = new System.Windows.Forms.ComboBox();
            this.btn_search = new System.Windows.Forms.Button();
            this.lbl_speed = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.rbtn_file = new System.Windows.Forms.RadioButton();
            this.rbtn_folder = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.custom_pgb = new System.Windows.Forms.Panel();
            this.lbl_progressCount = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btn_clear = new System.Windows.Forms.Button();
            this.btn_server_stat = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "fld_brows_file";
            // 
            // btn_choose_file
            // 
            this.btn_choose_file.Location = new System.Drawing.Point(349, 102);
            this.btn_choose_file.Name = "btn_choose_file";
            this.btn_choose_file.Size = new System.Drawing.Size(75, 23);
            this.btn_choose_file.TabIndex = 1;
            this.btn_choose_file.Text = "Choose File";
            this.btn_choose_file.UseVisualStyleBackColor = true;
            this.btn_choose_file.Click += new System.EventHandler(this.btn_choose_file_Click);
            // 
            // txt_file_path
            // 
            this.txt_file_path.Location = new System.Drawing.Point(107, 102);
            this.txt_file_path.Name = "txt_file_path";
            this.txt_file_path.Size = new System.Drawing.Size(220, 20);
            this.txt_file_path.TabIndex = 2;
            // 
            // btn_send
            // 
            this.btn_send.Location = new System.Drawing.Point(107, 148);
            this.btn_send.Name = "btn_send";
            this.btn_send.Size = new System.Drawing.Size(107, 23);
            this.btn_send.TabIndex = 10;
            this.btn_send.Text = "Send File/Folder";
            this.btn_send.UseVisualStyleBackColor = true;
            this.btn_send.Click += new System.EventHandler(this.btn_send_Click);
            // 
            // cmb_devices
            // 
            this.cmb_devices.FormattingEnabled = true;
            this.cmb_devices.Location = new System.Drawing.Point(107, 54);
            this.cmb_devices.Name = "cmb_devices";
            this.cmb_devices.Size = new System.Drawing.Size(220, 21);
            this.cmb_devices.TabIndex = 12;
            // 
            // btn_search
            // 
            this.btn_search.Location = new System.Drawing.Point(353, 52);
            this.btn_search.Name = "btn_search";
            this.btn_search.Size = new System.Drawing.Size(71, 23);
            this.btn_search.TabIndex = 14;
            this.btn_search.Text = "Refresh";
            this.btn_search.UseVisualStyleBackColor = true;
            this.btn_search.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // lbl_speed
            // 
            this.lbl_speed.AutoSize = true;
            this.lbl_speed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_speed.Location = new System.Drawing.Point(17, 305);
            this.lbl_speed.Name = "lbl_speed";
            this.lbl_speed.Size = new System.Drawing.Size(64, 13);
            this.lbl_speed.TabIndex = 18;
            this.lbl_speed.Text = "Progress :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(7, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "Activity Monitor";
            // 
            // rbtn_file
            // 
            this.rbtn_file.AutoSize = true;
            this.rbtn_file.Location = new System.Drawing.Point(20, 128);
            this.rbtn_file.Name = "rbtn_file";
            this.rbtn_file.Size = new System.Drawing.Size(41, 17);
            this.rbtn_file.TabIndex = 22;
            this.rbtn_file.TabStop = true;
            this.rbtn_file.Text = "File";
            this.rbtn_file.UseVisualStyleBackColor = true;
            this.rbtn_file.CheckedChanged += new System.EventHandler(this.rbtn_file_CheckedChanged);
            // 
            // rbtn_folder
            // 
            this.rbtn_folder.AutoSize = true;
            this.rbtn_folder.Location = new System.Drawing.Point(20, 154);
            this.rbtn_folder.Name = "rbtn_folder";
            this.rbtn_folder.Size = new System.Drawing.Size(54, 17);
            this.rbtn_folder.TabIndex = 23;
            this.rbtn_folder.TabStop = true;
            this.rbtn_folder.Text = "Folder";
            this.rbtn_folder.UseVisualStyleBackColor = true;
            this.rbtn_folder.CheckedChanged += new System.EventHandler(this.rbtn_folder_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(16, 102);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 13);
            this.label4.TabIndex = 24;
            this.label4.Text = "Select Type";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Location = new System.Drawing.Point(15, 189);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(408, 100);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Transfer Log";
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Location = new System.Drawing.Point(9, 20);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(387, 74);
            this.panel1.TabIndex = 0;
            // 
            // custom_pgb
            // 
            this.custom_pgb.Location = new System.Drawing.Point(103, 305);
            this.custom_pgb.Name = "custom_pgb";
            this.custom_pgb.Size = new System.Drawing.Size(268, 13);
            this.custom_pgb.TabIndex = 28;
            // 
            // lbl_progressCount
            // 
            this.lbl_progressCount.AutoSize = true;
            this.lbl_progressCount.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_progressCount.Location = new System.Drawing.Point(394, 303);
            this.lbl_progressCount.Name = "lbl_progressCount";
            this.lbl_progressCount.Size = new System.Drawing.Size(26, 15);
            this.lbl_progressCount.TabIndex = 29;
            this.lbl_progressCount.Text = "0 %";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Location = new System.Drawing.Point(16, 326);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(404, 57);
            this.groupBox2.TabIndex = 30;
            this.groupBox2.TabStop = false;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(14, 24);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(71, 23);
            this.button3.TabIndex = 32;
            this.button3.Text = "Settings";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(306, 24);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(71, 23);
            this.button1.TabIndex = 31;
            this.button1.Text = "Exit";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Image = ((System.Drawing.Image)(resources.GetObject("button2.Image")));
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point(167, 24);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(74, 23);
            this.button2.TabIndex = 31;
            this.button2.Text = "Help";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // btn_clear
            // 
            this.btn_clear.Location = new System.Drawing.Point(219, 148);
            this.btn_clear.Name = "btn_clear";
            this.btn_clear.Size = new System.Drawing.Size(92, 23);
            this.btn_clear.TabIndex = 25;
            this.btn_clear.Text = "Clear Selection ";
            this.btn_clear.UseVisualStyleBackColor = true;
            this.btn_clear.Click += new System.EventHandler(this.btn_clear_Click);
            // 
            // btn_server_stat
            // 
            this.btn_server_stat.Location = new System.Drawing.Point(107, 17);
            this.btn_server_stat.Name = "btn_server_stat";
            this.btn_server_stat.Size = new System.Drawing.Size(316, 23);
            this.btn_server_stat.TabIndex = 31;
            this.btn_server_stat.Text = "Server Status: Offline";
            this.btn_server_stat.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(322, 148);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(102, 23);
            this.button4.TabIndex = 32;
            this.button4.Text = "Recive File/Folder";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 33;
            this.label1.Text = "Devices";
            // 
            // Home
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(439, 395);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.btn_server_stat);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.lbl_progressCount);
            this.Controls.Add(this.custom_pgb);
            this.Controls.Add(this.lbl_speed);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btn_clear);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.rbtn_folder);
            this.Controls.Add(this.rbtn_file);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btn_search);
            this.Controls.Add(this.cmb_devices);
            this.Controls.Add(this.btn_send);
            this.Controls.Add(this.txt_file_path);
            this.Controls.Add(this.btn_choose_file);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Home";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btn_choose_file;
        private System.Windows.Forms.TextBox txt_file_path;
        private System.Windows.Forms.Button btn_send;
        private System.Windows.Forms.ComboBox cmb_devices;
        private System.Windows.Forms.Button btn_search;
        private System.Windows.Forms.Label lbl_speed;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton rbtn_file;
        private System.Windows.Forms.RadioButton rbtn_folder;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel custom_pgb;
        private System.Windows.Forms.Label lbl_progressCount;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btn_clear;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btn_server_stat;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
    }
}


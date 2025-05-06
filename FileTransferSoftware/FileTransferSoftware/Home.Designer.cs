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
            this.btn_start_server = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btn_choose_file = new System.Windows.Forms.Button();
            this.txt_file_path = new System.Windows.Forms.TextBox();
            this.btn_select_file = new System.Windows.Forms.Button();
            this.btn_select_folder = new System.Windows.Forms.Button();
            this.pb_selected_flie_icon = new System.Windows.Forms.PictureBox();
            this.txt_flie_name = new System.Windows.Forms.TextBox();
            this.txt_flie_size = new System.Windows.Forms.TextBox();
            this.txt_flie_create_date = new System.Windows.Forms.TextBox();
            this.btn_send = new System.Windows.Forms.Button();
            this.btn_stop_server = new System.Windows.Forms.Button();
            this.cmb_devices = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_search = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pb_selected_flie_icon)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_start_server
            // 
            this.btn_start_server.Location = new System.Drawing.Point(12, 12);
            this.btn_start_server.Name = "btn_start_server";
            this.btn_start_server.Size = new System.Drawing.Size(75, 23);
            this.btn_start_server.TabIndex = 0;
            this.btn_start_server.Text = "Start server";
            this.btn_start_server.UseVisualStyleBackColor = true;
            this.btn_start_server.Click += new System.EventHandler(this.btn_start_server_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "fld_brows_file";
            // 
            // btn_choose_file
            // 
            this.btn_choose_file.Location = new System.Drawing.Point(496, 207);
            this.btn_choose_file.Name = "btn_choose_file";
            this.btn_choose_file.Size = new System.Drawing.Size(75, 23);
            this.btn_choose_file.TabIndex = 1;
            this.btn_choose_file.Text = "Choose File";
            this.btn_choose_file.UseVisualStyleBackColor = true;
            this.btn_choose_file.Click += new System.EventHandler(this.btn_choose_file_Click);
            // 
            // txt_file_path
            // 
            this.txt_file_path.Location = new System.Drawing.Point(222, 209);
            this.txt_file_path.Name = "txt_file_path";
            this.txt_file_path.Size = new System.Drawing.Size(268, 20);
            this.txt_file_path.TabIndex = 2;
            // 
            // btn_select_file
            // 
            this.btn_select_file.Location = new System.Drawing.Point(12, 93);
            this.btn_select_file.Name = "btn_select_file";
            this.btn_select_file.Size = new System.Drawing.Size(75, 23);
            this.btn_select_file.TabIndex = 4;
            this.btn_select_file.Text = "File";
            this.btn_select_file.UseVisualStyleBackColor = true;
            this.btn_select_file.Click += new System.EventHandler(this.btn_select_file_Click);
            // 
            // btn_select_folder
            // 
            this.btn_select_folder.Location = new System.Drawing.Point(12, 122);
            this.btn_select_folder.Name = "btn_select_folder";
            this.btn_select_folder.Size = new System.Drawing.Size(75, 23);
            this.btn_select_folder.TabIndex = 5;
            this.btn_select_folder.Text = "Folder";
            this.btn_select_folder.UseVisualStyleBackColor = true;
            this.btn_select_folder.Click += new System.EventHandler(this.btn_select_folder_Click);
            // 
            // pb_selected_flie_icon
            // 
            this.pb_selected_flie_icon.Location = new System.Drawing.Point(222, 73);
            this.pb_selected_flie_icon.Name = "pb_selected_flie_icon";
            this.pb_selected_flie_icon.Size = new System.Drawing.Size(125, 105);
            this.pb_selected_flie_icon.TabIndex = 6;
            this.pb_selected_flie_icon.TabStop = false;
            // 
            // txt_flie_name
            // 
            this.txt_flie_name.Location = new System.Drawing.Point(369, 73);
            this.txt_flie_name.Name = "txt_flie_name";
            this.txt_flie_name.Size = new System.Drawing.Size(121, 20);
            this.txt_flie_name.TabIndex = 7;
            // 
            // txt_flie_size
            // 
            this.txt_flie_size.Location = new System.Drawing.Point(369, 118);
            this.txt_flie_size.Name = "txt_flie_size";
            this.txt_flie_size.Size = new System.Drawing.Size(121, 20);
            this.txt_flie_size.TabIndex = 9;
            // 
            // txt_flie_create_date
            // 
            this.txt_flie_create_date.Location = new System.Drawing.Point(369, 158);
            this.txt_flie_create_date.Name = "txt_flie_create_date";
            this.txt_flie_create_date.Size = new System.Drawing.Size(121, 20);
            this.txt_flie_create_date.TabIndex = 8;
            // 
            // btn_send
            // 
            this.btn_send.Location = new System.Drawing.Point(12, 209);
            this.btn_send.Name = "btn_send";
            this.btn_send.Size = new System.Drawing.Size(75, 23);
            this.btn_send.TabIndex = 10;
            this.btn_send.Text = "Send";
            this.btn_send.UseVisualStyleBackColor = true;
            this.btn_send.Click += new System.EventHandler(this.btn_send_Click);
            // 
            // btn_stop_server
            // 
            this.btn_stop_server.Location = new System.Drawing.Point(12, 41);
            this.btn_stop_server.Name = "btn_stop_server";
            this.btn_stop_server.Size = new System.Drawing.Size(75, 23);
            this.btn_stop_server.TabIndex = 11;
            this.btn_stop_server.Text = "Stop server";
            this.btn_stop_server.UseVisualStyleBackColor = true;
            this.btn_stop_server.Click += new System.EventHandler(this.btn_stop_server_Click);
            // 
            // cmb_devices
            // 
            this.cmb_devices.FormattingEnabled = true;
            this.cmb_devices.Location = new System.Drawing.Point(318, 18);
            this.cmb_devices.Name = "cmb_devices";
            this.cmb_devices.Size = new System.Drawing.Size(253, 21);
            this.cmb_devices.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(214, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Available Devices";
            // 
            // btn_search
            // 
            this.btn_search.Location = new System.Drawing.Point(593, 16);
            this.btn_search.Name = "btn_search";
            this.btn_search.Size = new System.Drawing.Size(75, 23);
            this.btn_search.TabIndex = 14;
            this.btn_search.Text = "Search Devices";
            this.btn_search.UseVisualStyleBackColor = true;
            this.btn_search.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // Home
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(698, 412);
            this.Controls.Add(this.btn_search);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmb_devices);
            this.Controls.Add(this.btn_stop_server);
            this.Controls.Add(this.btn_send);
            this.Controls.Add(this.txt_flie_size);
            this.Controls.Add(this.txt_flie_create_date);
            this.Controls.Add(this.txt_flie_name);
            this.Controls.Add(this.pb_selected_flie_icon);
            this.Controls.Add(this.btn_select_folder);
            this.Controls.Add(this.btn_select_file);
            this.Controls.Add(this.txt_file_path);
            this.Controls.Add(this.btn_choose_file);
            this.Controls.Add(this.btn_start_server);
            this.Name = "Home";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pb_selected_flie_icon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_start_server;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btn_choose_file;
        private System.Windows.Forms.TextBox txt_file_path;
        private System.Windows.Forms.Button btn_select_file;
        private System.Windows.Forms.Button btn_select_folder;
        private System.Windows.Forms.PictureBox pb_selected_flie_icon;
        private System.Windows.Forms.TextBox txt_flie_name;
        private System.Windows.Forms.TextBox txt_flie_size;
        private System.Windows.Forms.TextBox txt_flie_create_date;
        private System.Windows.Forms.Button btn_send;
        private System.Windows.Forms.Button btn_stop_server;
        private System.Windows.Forms.ComboBox cmb_devices;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_search;
    }
}


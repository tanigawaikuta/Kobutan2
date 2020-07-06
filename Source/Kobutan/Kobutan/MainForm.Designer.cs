namespace Kobutan
{
    partial class MainForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.Menu_File = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_File_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Edit = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Edit_Load = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Edit_Load_App = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Edit_Load_Serial = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Edit_Load_Gamepad = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_View = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_View_SelectApp = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_View_InstanceList = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_View_StripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.Menu_View_Devices = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_View_Server = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_View_StripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.Menu_View_Console = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Tool = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Tool_DeviceManager = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Tool_DeviceAndPrinter = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Tool_DirectX = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Tool_StripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.Menu_Tool_Option = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Help = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Help_Version = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Help_Debug = new System.Windows.Forms.ToolStripMenuItem();
            this.MainStatusBar = new System.Windows.Forms.StatusStrip();
            this.DummyToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.Menu_Tool_TextEditor = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Tool_StripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.MainMenu.SuspendLayout();
            this.MainStatusBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainMenu
            // 
            this.MainMenu.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.MainMenu.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_File,
            this.Menu_Edit,
            this.Menu_View,
            this.Menu_Tool,
            this.Menu_Help});
            resources.ApplyResources(this.MainMenu, "MainMenu");
            this.MainMenu.Name = "MainMenu";
            // 
            // Menu_File
            // 
            this.Menu_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_File_Exit});
            this.Menu_File.Name = "Menu_File";
            resources.ApplyResources(this.Menu_File, "Menu_File");
            // 
            // Menu_File_Exit
            // 
            this.Menu_File_Exit.Name = "Menu_File_Exit";
            resources.ApplyResources(this.Menu_File_Exit, "Menu_File_Exit");
            this.Menu_File_Exit.Click += new System.EventHandler(this.Menu_File_Exit_Click);
            // 
            // Menu_Edit
            // 
            this.Menu_Edit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_Edit_Load});
            this.Menu_Edit.Name = "Menu_Edit";
            resources.ApplyResources(this.Menu_Edit, "Menu_Edit");
            // 
            // Menu_Edit_Load
            // 
            this.Menu_Edit_Load.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_Edit_Load_App,
            this.Menu_Edit_Load_Serial,
            this.Menu_Edit_Load_Gamepad});
            this.Menu_Edit_Load.Name = "Menu_Edit_Load";
            resources.ApplyResources(this.Menu_Edit_Load, "Menu_Edit_Load");
            // 
            // Menu_Edit_Load_App
            // 
            this.Menu_Edit_Load_App.Name = "Menu_Edit_Load_App";
            resources.ApplyResources(this.Menu_Edit_Load_App, "Menu_Edit_Load_App");
            this.Menu_Edit_Load_App.Click += new System.EventHandler(this.Menu_Edit_Load_App_Click);
            // 
            // Menu_Edit_Load_Serial
            // 
            this.Menu_Edit_Load_Serial.Name = "Menu_Edit_Load_Serial";
            resources.ApplyResources(this.Menu_Edit_Load_Serial, "Menu_Edit_Load_Serial");
            this.Menu_Edit_Load_Serial.Click += new System.EventHandler(this.Menu_Edit_Load_Serial_Click);
            // 
            // Menu_Edit_Load_Gamepad
            // 
            this.Menu_Edit_Load_Gamepad.Name = "Menu_Edit_Load_Gamepad";
            resources.ApplyResources(this.Menu_Edit_Load_Gamepad, "Menu_Edit_Load_Gamepad");
            this.Menu_Edit_Load_Gamepad.Click += new System.EventHandler(this.Menu_Edit_Load_Gamepad_Click);
            // 
            // Menu_View
            // 
            this.Menu_View.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_View_SelectApp,
            this.Menu_View_InstanceList,
            this.Menu_View_StripSeparator1,
            this.Menu_View_Devices,
            this.Menu_View_Server,
            this.Menu_View_StripSeparator2,
            this.Menu_View_Console});
            this.Menu_View.Name = "Menu_View";
            resources.ApplyResources(this.Menu_View, "Menu_View");
            // 
            // Menu_View_SelectApp
            // 
            resources.ApplyResources(this.Menu_View_SelectApp, "Menu_View_SelectApp");
            this.Menu_View_SelectApp.Name = "Menu_View_SelectApp";
            this.Menu_View_SelectApp.Click += new System.EventHandler(this.Menu_View_SelectApp_Click);
            // 
            // Menu_View_InstanceList
            // 
            this.Menu_View_InstanceList.Image = global::Kobutan.Properties.Resources.WindowListImage;
            this.Menu_View_InstanceList.Name = "Menu_View_InstanceList";
            resources.ApplyResources(this.Menu_View_InstanceList, "Menu_View_InstanceList");
            this.Menu_View_InstanceList.Click += new System.EventHandler(this.Menu_View_InstanceList_Click);
            // 
            // Menu_View_StripSeparator1
            // 
            this.Menu_View_StripSeparator1.Name = "Menu_View_StripSeparator1";
            resources.ApplyResources(this.Menu_View_StripSeparator1, "Menu_View_StripSeparator1");
            // 
            // Menu_View_Devices
            // 
            this.Menu_View_Devices.Image = global::Kobutan.Properties.Resources.GamepadImage;
            this.Menu_View_Devices.Name = "Menu_View_Devices";
            resources.ApplyResources(this.Menu_View_Devices, "Menu_View_Devices");
            this.Menu_View_Devices.Click += new System.EventHandler(this.Menu_View_Devices_Click);
            // 
            // Menu_View_Server
            // 
            this.Menu_View_Server.Image = global::Kobutan.Properties.Resources.ServerImage;
            this.Menu_View_Server.Name = "Menu_View_Server";
            resources.ApplyResources(this.Menu_View_Server, "Menu_View_Server");
            // 
            // Menu_View_StripSeparator2
            // 
            this.Menu_View_StripSeparator2.Name = "Menu_View_StripSeparator2";
            resources.ApplyResources(this.Menu_View_StripSeparator2, "Menu_View_StripSeparator2");
            // 
            // Menu_View_Console
            // 
            this.Menu_View_Console.Image = global::Kobutan.Properties.Resources.ConsoleImage;
            this.Menu_View_Console.Name = "Menu_View_Console";
            resources.ApplyResources(this.Menu_View_Console, "Menu_View_Console");
            // 
            // Menu_Tool
            // 
            this.Menu_Tool.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_Tool_TextEditor,
            this.Menu_Tool_StripSeparator1,
            this.Menu_Tool_DeviceManager,
            this.Menu_Tool_DeviceAndPrinter,
            this.Menu_Tool_DirectX,
            this.Menu_Tool_StripSeparator2,
            this.Menu_Tool_Option});
            this.Menu_Tool.Name = "Menu_Tool";
            resources.ApplyResources(this.Menu_Tool, "Menu_Tool");
            // 
            // Menu_Tool_DeviceManager
            // 
            this.Menu_Tool_DeviceManager.Image = global::Kobutan.Properties.Resources.DeviceManagerImage;
            this.Menu_Tool_DeviceManager.Name = "Menu_Tool_DeviceManager";
            resources.ApplyResources(this.Menu_Tool_DeviceManager, "Menu_Tool_DeviceManager");
            this.Menu_Tool_DeviceManager.Click += new System.EventHandler(this.Menu_Tool_DeviceManager_Click);
            // 
            // Menu_Tool_DeviceAndPrinter
            // 
            this.Menu_Tool_DeviceAndPrinter.Image = global::Kobutan.Properties.Resources.DevicePrinterImage;
            this.Menu_Tool_DeviceAndPrinter.Name = "Menu_Tool_DeviceAndPrinter";
            resources.ApplyResources(this.Menu_Tool_DeviceAndPrinter, "Menu_Tool_DeviceAndPrinter");
            this.Menu_Tool_DeviceAndPrinter.Click += new System.EventHandler(this.Menu_Tool_DeviceAndPrinter_Click);
            // 
            // Menu_Tool_DirectX
            // 
            this.Menu_Tool_DirectX.Image = global::Kobutan.Properties.Resources.dxdiagImage;
            this.Menu_Tool_DirectX.Name = "Menu_Tool_DirectX";
            resources.ApplyResources(this.Menu_Tool_DirectX, "Menu_Tool_DirectX");
            this.Menu_Tool_DirectX.Click += new System.EventHandler(this.Menu_Tool_DirectX_Click);
            // 
            // Menu_Tool_StripSeparator1
            // 
            this.Menu_Tool_StripSeparator1.Name = "Menu_Tool_StripSeparator1";
            resources.ApplyResources(this.Menu_Tool_StripSeparator1, "Menu_Tool_StripSeparator1");
            // 
            // Menu_Tool_Option
            // 
            this.Menu_Tool_Option.Image = global::Kobutan.Properties.Resources.ConfigImage;
            this.Menu_Tool_Option.Name = "Menu_Tool_Option";
            resources.ApplyResources(this.Menu_Tool_Option, "Menu_Tool_Option");
            this.Menu_Tool_Option.Click += new System.EventHandler(this.Menu_Tool_Option_Click);
            // 
            // Menu_Help
            // 
            this.Menu_Help.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_Help_Version,
            this.Menu_Help_Debug});
            this.Menu_Help.Name = "Menu_Help";
            resources.ApplyResources(this.Menu_Help, "Menu_Help");
            // 
            // Menu_Help_Version
            // 
            this.Menu_Help_Version.Name = "Menu_Help_Version";
            resources.ApplyResources(this.Menu_Help_Version, "Menu_Help_Version");
            this.Menu_Help_Version.Click += new System.EventHandler(this.Menu_Help_Version_Click);
            // 
            // Menu_Help_Debug
            // 
            this.Menu_Help_Debug.Name = "Menu_Help_Debug";
            resources.ApplyResources(this.Menu_Help_Debug, "Menu_Help_Debug");
            this.Menu_Help_Debug.Click += new System.EventHandler(this.Menu_Help_Debug_Click);
            // 
            // MainStatusBar
            // 
            this.MainStatusBar.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.MainStatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DummyToolStripStatusLabel});
            resources.ApplyResources(this.MainStatusBar, "MainStatusBar");
            this.MainStatusBar.Name = "MainStatusBar";
            // 
            // DummyToolStripStatusLabel
            // 
            resources.ApplyResources(this.DummyToolStripStatusLabel, "DummyToolStripStatusLabel");
            this.DummyToolStripStatusLabel.Name = "DummyToolStripStatusLabel";
            // 
            // Menu_Tool_TextEditor
            // 
            this.Menu_Tool_TextEditor.Name = "Menu_Tool_TextEditor";
            resources.ApplyResources(this.Menu_Tool_TextEditor, "Menu_Tool_TextEditor");
            this.Menu_Tool_TextEditor.Click += new System.EventHandler(this.Menu_Tool_TextEditor_Click);
            // 
            // Menu_Tool_StripSeparator2
            // 
            this.Menu_Tool_StripSeparator2.Name = "Menu_Tool_StripSeparator2";
            resources.ApplyResources(this.Menu_Tool_StripSeparator2, "Menu_Tool_StripSeparator2");
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.MainStatusBar);
            this.Controls.Add(this.MainMenu);
            this.Icon = global::Kobutan.Properties.Resources.KobutanICon;
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.MainMenu;
            this.Name = "MainForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.MainStatusBar.ResumeLayout(false);
            this.MainStatusBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem Menu_File;
        private System.Windows.Forms.ToolStripMenuItem Menu_File_Exit;
        private System.Windows.Forms.ToolStripMenuItem Menu_Edit;
        private System.Windows.Forms.ToolStripMenuItem Menu_View;
        private System.Windows.Forms.ToolStripMenuItem Menu_View_SelectApp;
        private System.Windows.Forms.ToolStripMenuItem Menu_View_InstanceList;
        private System.Windows.Forms.ToolStripMenuItem Menu_View_Console;
        private System.Windows.Forms.ToolStripMenuItem Menu_Tool;
        private System.Windows.Forms.ToolStripMenuItem Menu_Tool_DeviceManager;
        private System.Windows.Forms.ToolStripSeparator Menu_Tool_StripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem Menu_Tool_Option;
        private System.Windows.Forms.ToolStripMenuItem Menu_Help;
        private System.Windows.Forms.ToolStripMenuItem Menu_Help_Version;
        private System.Windows.Forms.ToolStripSeparator Menu_View_StripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem Menu_View_Devices;
        private System.Windows.Forms.ToolStripMenuItem Menu_View_Server;
        private System.Windows.Forms.ToolStripSeparator Menu_View_StripSeparator2;
        private System.Windows.Forms.StatusStrip MainStatusBar;
        private System.Windows.Forms.ToolStripMenuItem Menu_Tool_DeviceAndPrinter;
        private System.Windows.Forms.ToolStripMenuItem Menu_Tool_DirectX;
        private System.Windows.Forms.ToolStripMenuItem Menu_Help_Debug;
        private System.Windows.Forms.ToolStripMenuItem Menu_Edit_Load;
        private System.Windows.Forms.ToolStripMenuItem Menu_Edit_Load_App;
        private System.Windows.Forms.ToolStripMenuItem Menu_Edit_Load_Serial;
        private System.Windows.Forms.ToolStripMenuItem Menu_Edit_Load_Gamepad;
        private System.Windows.Forms.ToolStripStatusLabel DummyToolStripStatusLabel;
        private System.Windows.Forms.ToolStripMenuItem Menu_Tool_TextEditor;
        private System.Windows.Forms.ToolStripSeparator Menu_Tool_StripSeparator2;
    }
}


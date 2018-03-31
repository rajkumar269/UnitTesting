namespace TriggerUnitTesting
{
    partial class FileWatcherService
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.FSWatcherTest = new System.IO.FileSystemWatcher();
            ((System.ComponentModel.ISupportInitialize)(this.FSWatcherTest)).BeginInit();
            // 
            // FSWatcherTest
            // 
            this.FSWatcherTest.EnableRaisingEvents = true;
            this.FSWatcherTest.Filter = "*.cs*";
            this.FSWatcherTest.IncludeSubdirectories = true;
            this.FSWatcherTest.Changed += new System.IO.FileSystemEventHandler(this.FSWatcherTest_Changed);
            // 
            // FileWatcherService
            // 
            this.ServiceName = "FileWatcherService";
            ((System.ComponentModel.ISupportInitialize)(this.FSWatcherTest)).EndInit();

        }

        #endregion

        private System.IO.FileSystemWatcher FSWatcherTest;
    }
}

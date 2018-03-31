using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TriggerUnitTesting
{
    public partial class FileWatcherService : ServiceBase
    {
        DateTime lastRead = DateTime.MinValue;
        string OuputFilePath;
        string MSBuildExePath;
        string NunitConsoleExePath;
        string ReportUnitExePath;
        string XMLReportFilePath;
        string HTMLReportFilePath;
        string ProjectFilePath;
        string UnitTestLibraryPath;
        string HTMLReportFolderPath;
        string UTProjectFilePath;
        string ArchiveFolderPath;
        string SendToEmailId;

        public FileWatcherService()
        {            
            InitializeComponent();          
        }

        protected override void OnStart(string[] args)
        {
            FSWatcherTest.Path = ConfigurationManager.AppSettings["WatchPath"];
            FSWatcherTest.Filter = ConfigurationManager.AppSettings["WatchFileType"];
            OuputFilePath = ConfigurationManager.AppSettings["OuputFilePath"];
            MSBuildExePath = ConfigurationManager.AppSettings["MSBuildExePath"];
            NunitConsoleExePath = ConfigurationManager.AppSettings["NunitConsoleExePath"];
            UnitTestLibraryPath = ConfigurationManager.AppSettings["UnitTestLibraryPath"];
            ReportUnitExePath = ConfigurationManager.AppSettings["ReportUnitExePath"];
            XMLReportFilePath = ConfigurationManager.AppSettings["XMLReportFilePath"];
            HTMLReportFolderPath = ConfigurationManager.AppSettings["HTMLReportFolderPath"];
            ProjectFilePath = ConfigurationManager.AppSettings["ProjectFilePath"];
            UTProjectFilePath = ConfigurationManager.AppSettings["UTProjectFilePath"];
            ArchiveFolderPath = ConfigurationManager.AppSettings["ArchiveFolderPath"];
            SendToEmailId = ConfigurationManager.AppSettings["SendToEmailId"];
        }

        protected override void OnStop()
        {
        }

        private void FSWatcherTest_Changed(object sender, System.IO.FileSystemEventArgs e)
        {
            DateTime lastWriteTime = File.GetLastWriteTime(e.FullPath);            
            if (lastWriteTime != lastRead)
            {
                if (File.Exists(ArchiveFolderPath + e.Name))
                {
                    byte[] ArchiveFileHash = GetFileHashData(ArchiveFolderPath + e.Name);
                    byte[] ChangedFileHash = GetFileHashData(e.FullPath);
                    if (!ArchiveFileHash.SequenceEqual(ChangedFileHash))
                    {
                        File.Copy(e.FullPath, ArchiveFolderPath + e.Name,true);
                        PerformUnitTest();
                    }
                }
                else
                {
                    File.Copy(e.FullPath, ArchiveFolderPath + e.Name,true);
                    PerformUnitTest();
                }                
                lastRead = File.GetLastWriteTime(e.FullPath);
            }            
        }

        private void PerformUnitTest()
        {
            System.IO.FileInfo fiBefore = null;
            fiBefore = new System.IO.FileInfo(OuputFilePath);
            fiBefore.Refresh();

            Process BuildProcess = new Process();
            ProcessStartInfo BuildStartInfo = new ProcessStartInfo();
            BuildStartInfo.CreateNoWindow = false;
            BuildStartInfo.UseShellExecute = false;
            BuildStartInfo.FileName = MSBuildExePath + " ";
            BuildStartInfo.Arguments = ProjectFilePath;
            BuildProcess.StartInfo = BuildStartInfo;
            BuildProcess.Start();
            BuildProcess.WaitForExit();

            System.IO.FileInfo fiAfter = null;
            fiAfter = new System.IO.FileInfo(OuputFilePath);
            fiAfter.Refresh();

            if (fiBefore.LastWriteTime != fiAfter.LastWriteTime)
            {
                Process BuildUTProcess = new Process();
                ProcessStartInfo BuildUTStartInfo = new ProcessStartInfo();
                BuildUTStartInfo.CreateNoWindow = false;
                BuildUTStartInfo.UseShellExecute = false;
                BuildUTStartInfo.FileName = MSBuildExePath + " ";
                BuildUTStartInfo.Arguments = UTProjectFilePath;
                BuildUTProcess.StartInfo = BuildUTStartInfo;
                BuildUTProcess.Start();
                BuildUTProcess.WaitForExit();


                Process UnitTestProcess = new Process();
                ProcessStartInfo UnitTestStartInfo = new ProcessStartInfo();
                UnitTestStartInfo.CreateNoWindow = false;
                UnitTestStartInfo.UseShellExecute = false;
                UnitTestStartInfo.FileName = NunitConsoleExePath + " ";
                UnitTestStartInfo.Arguments = " /xml:" + XMLReportFilePath + " " + UnitTestLibraryPath;
                UnitTestProcess.StartInfo = UnitTestStartInfo;
                UnitTestProcess.Start();
                UnitTestProcess.WaitForExit();

                Process UnitTestReportProcess = new Process();
                ProcessStartInfo UnitTestReportStartInfo = new ProcessStartInfo();
                UnitTestReportStartInfo.CreateNoWindow = false;
                UnitTestReportStartInfo.UseShellExecute = false;
                UnitTestReportStartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                UnitTestReportStartInfo.FileName = ReportUnitExePath + " ";
                HTMLReportFilePath = HTMLReportFolderPath + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".html";
                UnitTestReportStartInfo.Arguments = XMLReportFilePath + " " + HTMLReportFilePath;
                UnitTestReportProcess.StartInfo = UnitTestReportStartInfo;
                UnitTestReportProcess.Start();
                UnitTestReportProcess.WaitForExit();

                UnitTestReportProcess.Dispose();
                UnitTestReportProcess.Close();
                Send("Kindly check the unit test report attached.", "Unit Test Report", "davidrajkumar.j@dsrc.co.in", HTMLReportFilePath);
            }
        }

        private static byte[] GetFileHashData(string FilePath)
        {
            byte[] FileData = File.ReadAllBytes(FilePath);
            byte[] FileHash = MD5.Create().ComputeHash(FileData);
            return FileHash;
        }

        private void Send(string emailbody, string emailSub, string ToAddress, string FileAttachement)
        {
            MailMessage mail = new MailMessage();
            SmtpClient client = new SmtpClient();
            client.Port = 2525;
            client.Host = "192.168.4.101";
            client.Credentials = new System.Net.NetworkCredential("arunkumar.s.g@dsrc.co.in", "dsrc4.");
            mail.From = new MailAddress(SendToEmailId);
            mail.To.Add(ToAddress);
            mail.Body = emailbody;
            mail.Subject = emailSub;
            System.Net.Mail.Attachment attachment;
            attachment = new System.Net.Mail.Attachment(FileAttachement);
            mail.Attachments.Add(attachment);
            mail.IsBodyHtml = true;
            client.Send(mail);
        }
    }
}

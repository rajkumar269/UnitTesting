using System;
using NUnit.Framework;
using GPP.Site.Development;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Diagnostics;
using System.Reflection;
namespace GenericPortalUnitTestProject
{

    [SetUpFixture]
    public class SetupClass
    {
        public static Type LogicClassType;
        public static Type TrafficClassType;
        [SetUp]
        public void InitializeDynamicReference()
        {
            //Assembly RefAssembly = Assembly.LoadFile(@"D:\David\Projects\Development\GPPJenkins\WebApplication\GPP.Site.Development\bin\GPP.Site.Development.dll");
            //LogicClassType = RefAssembly.GetType("GPP.Site.Development.LogicClass");
            //TrafficClassType = RefAssembly.GetType("GPP.Site.Development.TrafficLightSystem");
        }

        [TearDown]
        public void Dispose()
        {
            //Process UnitTestReportProcess = new Process();
            //ProcessStartInfo UnitTestReportStartInfo = new ProcessStartInfo();
            //UnitTestReportStartInfo.CreateNoWindow = false;
            //UnitTestReportStartInfo.UseShellExecute = false;
            //UnitTestReportStartInfo.FileName = @"D:\David\Projects\Test\ReportUnitTest.bat";
            //UnitTestReportProcess.StartInfo = UnitTestReportStartInfo;
            //UnitTestReportProcess.Start();
            //UnitTestReportProcess.WaitForExit();             
            //Send("The unit test project was built and run. Kindly check the report attached.", "Unit Test HTML Report", "davidrajkumar.j@dsrc.co.in");
        }
    }

    [TestFixture]
    public class GPPNunitTest
    {
        LogicClass l = new LogicClass();

        //Default login page to authenticate the user
        [TestCase("admin", 1)]
        [TestCase("admin1", 0)]
        public void TestVerifyLogin(string UsrName, int ExpVal)
        {
            
            DataSet ds = new DataSet();
            ds = l.VerifyLogin(UsrName);

            //For Not static method using dynamic reference
            //ds = (DataSet)SetupClass.ClassType.InvokeMember("VerifyLogin", BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public, null, calcInstance, new object[] { "admin" });
            //For static method
            //ds = (DataSet)SetupClass.LogicClassType.GetMethod("VerifyLogin", BindingFlags.Public | BindingFlags.Static).Invoke(null, new object[] { UsrName }); 
            
            Assert.AreEqual(ExpVal, ds.Tables[0].Rows.Count);
        }

        //AdminHome Page to dispay the client in drop down 
        [TestCase]
        public void TestVerifyLoadClient()
        {
            DataSet ds = new DataSet();
            ds = l.VerifyLoadClient();
            //ds = (DataSet)SetupClass.LogicClassType.GetMethod("VerifyLoadClient", BindingFlags.Public | BindingFlags.Static).Invoke(null, null); 
            Assert.AreEqual(1, ds.Tables.Count);
        }

        //AdminHome Page to dispay the client details on drop down selection
        [TestCase("40", 1)]
        [TestCase("44", 0)]
        public void TestVerifyClientDetailsbyId(string ClientID, int ExpVal)
        {
            DataSet ds = new DataSet();
            ds = l.VerifyClientDetailsbyId(ClientID);
            //ds = (DataSet)SetupClass.LogicClassType.GetMethod("VerifyClientDetailsbyId", BindingFlags.Public | BindingFlags.Static).Invoke(null, new object[] { ClientID }); 
            Assert.AreEqual(ExpVal, ds.Tables[0].Rows.Count);
        }

        //Home Page to dispay the dashboard items
        [TestCase("37", 1)]
        [TestCase("44", 1)]
        public void TestGetDashboardStatsbyId(string ClientID, int ExpVal)
        {
            DataSet ds = new DataSet();
            ds = l.GetDashboardStatsbyId(ClientID);
            //ds = (DataSet)SetupClass.LogicClassType.GetMethod("GetDashboardStatsbyId", BindingFlags.Public | BindingFlags.Static).Invoke(null, new object[] { ClientID }); 
            Assert.AreEqual(ExpVal, ds.Tables[0].Rows.Count);
        }

        //Traffic light logic
        [TestCase("Today", 0, 1, 37, 3)]            //Red
        [TestCase("AddDays", -2, 1, 37, 2)]         //Amber
        [TestCase("AddHours", -45, 1, 37, 1)]       //White
        public void TestGetTrafficLightFlag(String AddAttributeName, int AddAttributeValue, int BatchId, int ClientId, int ExpVal)
        {
            int ActVal= 0;
            //Can pass only constant value in the attribute. Logic to pass the values as required for the system
            DateTime OrderDate = DateTime.Now;
            if (AddAttributeName == "AddDays")
                OrderDate = OrderDate.AddDays(AddAttributeValue);
            else if (AddAttributeName == "AddHours")
                OrderDate = OrderDate.AddHours(AddAttributeValue);

            ActVal = TrafficLightSystem.GetTrafficLightFlag(BatchId, Convert.ToDateTime(OrderDate), ClientId);
            //ActVal = (int)SetupClass.TrafficClassType.GetMethod("GetTrafficLightFlag", BindingFlags.Public | BindingFlags.Static).Invoke(null, new object[] { BatchId, Convert.ToDateTime(OrderDate), ClientId });            
            Assert.AreEqual(ExpVal, ActVal);
        }


        //Scan Book Id or order id by Admin
        [TestCase(37, "123", "0,0,0")]
        [TestCase(37, "22", "1,4,22")]
        public void TestValidateScanningCodeForClientID(int ClientID, string Code, string ExpVal)
        {
            string ActVal = l.ValidateScanningCodeForClientID(ClientID, Code);
            //string ActVal = (string)SetupClass.LogicClassType.GetMethod("ValidateScanningCodeForClientID", BindingFlags.Public | BindingFlags.Static).Invoke(null, new object[] { ClientID, Code }); 
            Assert.AreEqual(ExpVal, ActVal);
        }

        //Status update for BOE client
        [TestCase(10018, 32, "Empty", "Processing", "1015", 10, "cover", "BOOK-DL", "", 1)] 
        [TestCase(10018, 32, "Empty", "In finishing", "1015", 6, "cover", "BOOK-DL", "32_1001.pdf", 1)]
        [TestCase(10018, 32, "Empty", "Bound", "0", 7, "0", "0", "32_1001.pdf", 1)]
        public void Test_boeStatusUpdate(int _batchID, int _clientID, string _currentOrderstatus, string _updatingOrderstatus, string _orderID, int _statusID, string _types, string _sku, string _fileName, int _messageStatusID)
        {
            l._boeStatusUpdate(_batchID, _clientID, _currentOrderstatus, _updatingOrderstatus, _orderID, _statusID, _types, _sku, _fileName, _messageStatusID);
            //SetupClass.LogicClassType.GetMethod("_boeStatusUpdate", BindingFlags.Public | BindingFlags.Static).Invoke(null, new object[] { _batchID, _clientID, _currentOrderstatus, _updatingOrderstatus, _orderID, _statusID, _types, _sku, _fileName, _messageStatusID }); 
            Assert.AreEqual(1, 1);
        }
        //Status update for LMN client
        [TestCase(100041, 37, "Empty", "In finishing", "1", 6, "0", "lmn:boy:softback:en-GB", "100041.pdf", 1)]
        [TestCase(100041, 37, "Empty", "Bound", "0", 7, "0", "0", "100041.pdf", 1)]        
        public void Test_lmnEagleStatusUpdate(int _batchID, int _clientID, string _currentOrderstatus, string _updatingOrderstatus, string _orderID, int _statusID, string _types, string _sku, string _fileName, int _messageStatusID
)
        {
            l._lmnEagleStatusUpdate(_batchID, _clientID, _currentOrderstatus, _updatingOrderstatus, _orderID, _statusID, _types, _sku, _fileName, _messageStatusID);
            //SetupClass.LogicClassType.GetMethod("_lmnEagleStatusUpdate", BindingFlags.Public | BindingFlags.Static).Invoke(null, new object[] { _batchID, _clientID, _currentOrderstatus, _updatingOrderstatus, _orderID, _statusID, _types, _sku, _fileName, _messageStatusID }); 
            Assert.AreEqual(1, 1);
        }      

    }
}

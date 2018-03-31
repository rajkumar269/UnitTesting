using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using MiddleTraining.Business;
using MiddleTraining;
namespace TrainingSite.Test
{
    [TestFixture]
    public class TestTrainingSite
    {
        User uInfo = null;
        ImplentUserRole IR = null;
        public TestTrainingSite()
        {
            uInfo = new User()
            {
                UserName = "User1",
                PassWord = "**1234**",
                RoleID  = 1,
                EmailId = "Test@dsrc.co.in"
            };
        }

        [TestCase]   //Passes
        public void CreateUser()
        {
            IR = new ImplentUserRole();
            Assert.AreEqual(true, IR.CreateUser(uInfo));
        }
        [TestCase]
        public void GetUserList()
        {
            IR = new ImplentUserRole();
            var userCount = IR.GetUserList();
            Assert.IsNotEmpty(userCount);
        }
        [TestCase(1)]
        public void GetUserInfo(int UserID)
        {
            IR = new ImplentUserRole();
            Assert.IsNull(IR.GetUserInfo(UserID));
        }
        [TestCase(1)]
        public void DeleteUser(int UserID)
        {            
            IR = new ImplentUserRole();
            Assert.AreEqual(true, IR.DeleteUser(UserID));
        }
    }
}

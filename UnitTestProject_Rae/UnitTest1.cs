using Microsoft.VisualStudio.TestTools.UnitTesting;
using RaeClass.Helper;
using RaeClass.Models;
using System;
using System.Security.Cryptography;
using System.Text;

namespace UnitTestProject_Rae
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestCreateJson()
        {
            Read read = new Read();
            read._id = "W92eXqu9e31Z7Ks-";
            read._openid = "oT_iO4tBijzyXT91iOW6wmGF801Q";
            read.fcreateBy = "choc";
            read.fcreateTime = DateTime.Now.ToString();
            read.fdocStatus = "A";
            read.flevel = "1";
            read.fcnContent = "<p style=\"text - align:left; line - height:150 % \">         < span style = \"font-family:宋体;color:black;background:yellow;background:yellow\" > 自动生成应收单的任务设置为定时重复执行，如设置每天晚上 </ span >< span style = \"color:black;background:yellow;background:yellow\" > 12 </ span >< span style = \"font-family:宋体;color:black;background:yellow;background:yellow\" > 点执行一次。在执行时，检查数据库中未生成应收单的数据：</ span >                         </ p > <p style=\"margin - left:24px; text - align:left; line - height:150 % \">            < span style = \";color:black;background:yellow;background:yellow\" > 1.< span style = \"font:9px &#39;Times New Roman&#39;\" > &nbsp; &nbsp; &nbsp; &nbsp; </ span ></ span >< span style = \"font-family:宋体;color:black;background:yellow;background:yellow\" > 该部分数据没有对应合同号，则根据不同客户生成对应的应收单，每个客户一张；如果有部分记录找不到默认价目表，则该记录对应的客户名下的所有 </ span >< span style = \"font-family:宋体;color:black;background:red;background:red\" > 没有销售订单的数据 </ span >< span style = \"font-family:宋体;color:black;background:yellow;background:yellow\" > 都不生成应收单，同时应该有报错信息给到用户。</ span >                                              </ p > ";
            read.fenContent = "<p style=\"text - align:left; line - height:150 % \">         < span style = \"font-family:宋体;color:black;background:yellow;background:yellow\" > 自动生成应收单的任务设置为定时重复执行，如设置每天晚上 </ span >< span style = \"color:black;background:yellow;background:yellow\" > 12 </ span >< span style = \"font-family:宋体;color:black;background:yellow;background:yellow\" > 点执行一次。在执行时，检查数据库中未生成应收单的数据：</ span >                         </ p > ";
            read.frecordFileId1 = "cloud://reaclass-dev-b66f08.7265-reaclass-dev-b66f08/read/xty.mp3";
            read.frecordFileId2 = "cloud://reaclass-dev-b66f08.7265-reaclass-dev-b66f08/read/xty.mp3";
            read.fnumber = "read1201811030001";
            read.fname = "TT";
            read.fmodifyBy = "choc";
            read.fmodifyTime = DateTime.Now.ToString();
            string json = JsonHelper.SerializeObject(read);

        }

        [TestMethod]
        public void TestSqliteConn()
        {

        }

        [TestMethod]
        public void TestGetDateTimeSerial()
        {
            //日期流水：20180909
            var dt = DateTime.Now;
            StringBuilder sb = new StringBuilder();
            sb.Append(dt.Year.ToString());
            sb.Append(dt.Month>10?dt.Month.ToString():"0" + dt.Month);
            sb.Append(dt.Day>10?dt.Month.ToString():"0" + dt.Day);
            byte[] randomBytes = new byte[4];
            RNGCryptoServiceProvider rngServiceProvider = new RNGCryptoServiceProvider();
            rngServiceProvider.GetBytes(randomBytes);
            int result = BitConverter.ToInt32(randomBytes, 0);
        }

        [TestMethod]
        public void TestGetDateTimeFormat()
        {
            //日期流水：20180909
            var dt = DateTime.Now;
            string str = dt.ToString("yyyy-MM-dd");
        }


    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using RaeClass.Helper;
using RaeClass.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace UnitTestProject_Rae
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestCreateJson()
        {
            //Read read = new Read();
            //read._id = "W92eXqu9e31Z7Ks-";
            //read._openid = "oT_iO4tBijzyXT91iOW6wmGF801Q";
            //read.fcreateBy = "choc";
            //read.fcreateTime = DateTime.Now.ToString();
            //read.fdocStatus = "A";
            //read.flevel = "1";
            //read.fcnContent = "<p style=\"text - align:left; line - height:150 % \">         < span style = \"font-family:����;color:black;background:yellow;background:yellow\" > �Զ�����Ӧ�յ�����������Ϊ��ʱ�ظ�ִ�У�������ÿ������ </ span >< span style = \"color:black;background:yellow;background:yellow\" > 12 </ span >< span style = \"font-family:����;color:black;background:yellow;background:yellow\" > ��ִ��һ�Ρ���ִ��ʱ��������ݿ���δ����Ӧ�յ������ݣ�</ span >                         </ p > <p style=\"margin - left:24px; text - align:left; line - height:150 % \">            < span style = \";color:black;background:yellow;background:yellow\" > 1.< span style = \"font:9px &#39;Times New Roman&#39;\" > &nbsp; &nbsp; &nbsp; &nbsp; </ span ></ span >< span style = \"font-family:����;color:black;background:yellow;background:yellow\" > �ò�������û�ж�Ӧ��ͬ�ţ�����ݲ�ͬ�ͻ����ɶ�Ӧ��Ӧ�յ���ÿ���ͻ�һ�ţ�����в��ּ�¼�Ҳ���Ĭ�ϼ�Ŀ����ü�¼��Ӧ�Ŀͻ����µ����� </ span >< span style = \"font-family:����;color:black;background:red;background:red\" > û�����۶��������� </ span >< span style = \"font-family:����;color:black;background:yellow;background:yellow\" > ��������Ӧ�յ���ͬʱӦ���б�����Ϣ�����û���</ span >                                              </ p > ";
            //read.fenContent = "<p style=\"text - align:left; line - height:150 % \">         < span style = \"font-family:����;color:black;background:yellow;background:yellow\" > �Զ�����Ӧ�յ�����������Ϊ��ʱ�ظ�ִ�У�������ÿ������ </ span >< span style = \"color:black;background:yellow;background:yellow\" > 12 </ span >< span style = \"font-family:����;color:black;background:yellow;background:yellow\" > ��ִ��һ�Ρ���ִ��ʱ��������ݿ���δ����Ӧ�յ������ݣ�</ span >                         </ p > ";
            //read.frecordFileId1 = "cloud://reaclass-dev-b66f08.7265-reaclass-dev-b66f08/read/xty.mp3";
            //read.frecordFileId2 = "cloud://reaclass-dev-b66f08.7265-reaclass-dev-b66f08/read/xty.mp3";
            //read.fnumber = "read1201811030001";
            //read.fname = "TT";
            //read.fmodifyBy = "choc";
            //read.fmodifyTime = DateTime.Now.ToString();
            //string json = JsonHelper.SerializeObject(read);

        }

        [TestMethod]
        public void TestSqliteConn()
        {

        }

        [TestMethod]
        public void TestGetDateTimeSerial()
        {
            //������ˮ��20180909
            var dt = DateTime.Now;
            StringBuilder sb = new StringBuilder();
            sb.Append(dt.Year.ToString());
            sb.Append(dt.Month > 10 ? dt.Month.ToString() : "0" + dt.Month);
            sb.Append(dt.Day > 10 ? dt.Month.ToString() : "0" + dt.Day);
            byte[] randomBytes = new byte[4];
            RNGCryptoServiceProvider rngServiceProvider = new RNGCryptoServiceProvider();
            rngServiceProvider.GetBytes(randomBytes);
            int result = BitConverter.ToInt32(randomBytes, 0);
        }

        [TestMethod]
        public void TestGetDateTimeFormat()
        {
            //������ˮ��20180909
            var dt = DateTime.Now;
            string str = dt.ToString("yyyy-MM-dd");
        }

        [TestMethod]
        public void TestConvert()
        {
            bool flag1 = Regex.IsMatch("56", @"^[+-]?\d*[.]?\d*$");
            bool flag2 = Regex.IsMatch("56.56", @"^[+-]?\d*[.]?\d*$");
            bool flag3 = Regex.IsMatch("0", @"^[+-]?\d*[.]?\d*$");
            bool flag4 = Regex.IsMatch("0.123", @"^[+-]?\d*[.]?\d*$");
            bool flag5 = Regex.IsMatch("-0.123", @"^[+-]?\d*[.]?\d*$");
            bool flag6 = Regex.IsMatch("-0.000", @"^[+-]?\d*[.]?\d*$");
            bool flag7 = Regex.IsMatch("-123.333", @"^[+-]?\d*[.]?\d*$");
            bool flag8 = Regex.IsMatch("ttt", @"^[+-]?\d*[.]?\d*$");
            //bool flag9 = Regex.IsMatch(null, @"^[+-]?\d*[.]?\d*$");
        }

        [TestMethod]
        public void TestGroupByDate()
        {
            List<TestModel> list = new List<TestModel>();
            list.Add(new TestModel { FDate = DateTime.Now ,FLevel = 0});
            list.Add(new TestModel { FDate = DateTime.Now.AddDays(1) ,FLevel = 0});
            list.Add(new TestModel { FDate = DateTime.Now.AddDays(1) ,FLevel = 0});
            list.Add(new TestModel { FDate = DateTime.Now.AddDays(1) ,FLevel = 1});
            list.Add(new TestModel { FDate = DateTime.Now.AddDays(2), FLevel = 0 });
            list.Add(new TestModel { FDate = DateTime.Now.AddDays(2), FLevel = 0 });
            list.Add(new TestModel { FDate = DateTime.Now.AddDays(2), FLevel = 1 });
            list.Add(new TestModel { FDate = DateTime.Now ,FLevel = 1});
            var res = list.Select(x=> new {x.FDate,x.FLevel }).GroupBy(x=>new { Date = x.FDate.ToString("yyyy-MM-dd"),Level = x.FLevel})
                .Select(x=>new {
                    Date = x.Key.Date,
                    Level = x.Key.Level,
                    Count = x.Count(),
            });
            List<TestGroupModel> _list = new List<TestGroupModel>();
            foreach (var item in res)
            {
                var _item = new TestGroupModel();
                _item.Date = item.Date;
                _item.Level = item.Level;
                _item.Count = item.Count;
                _list.Add(_item);
            }
        }
    }

    public class TestModel
    {
        public DateTime FDate { set; get; }
        public int FLevel { set; get; }
    }

    public class TestGroupModel
    {
        public string Date { set; get; }
        public int Level { set; get; }
        public int Count { set; get; }
    }



}


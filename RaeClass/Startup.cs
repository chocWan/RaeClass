using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RaeClass.Helper;
using RaeClass.Models;
using RaeClass.Repository;

namespace RaeClass
{
    public class Startup
    {

        public static ILoggerRepository logRepository { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            //log4net
            logRepository = LogManager.CreateRepository("NETCoreRepository");
            //指定配置文件
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //生成当前项目根目录下
            var connection = @"Filename=raeclass.db";
            services.AddDbContext<RaeClassContext>(options => options.UseSqlite(connection));
            services.AddMvc();
            services.AddScoped<IReadRepository, ReadRepository>();
            services.AddScoped<ISerialNumberRepository, SerialNumberRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            InitData(app.ApplicationServices);
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

        }

        private void InitData(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                //获取注入的RaeClassContext
                var db = serviceScope.ServiceProvider.GetService<RaeClassContext>();
                db.Database.EnsureCreated();//如果数据库不存在则创建，存在不做操作。
                if (db.SerialNumberSet.Count() == 0)
                {
                    var serialNumbers = new List<SerialNumber>{
                        new SerialNumber{ FContentType="Read",FCurrentGeneratedIndex = 1000,FModifyTime = DateTime.Now},
                        new SerialNumber{ FContentType="Listen",FCurrentGeneratedIndex = 1000,FModifyTime = DateTime.Now},
                        new SerialNumber{ FContentType="Oral",FCurrentGeneratedIndex = 1000,FModifyTime = DateTime.Now},
                        new SerialNumber{ FContentType="Tech",FCurrentGeneratedIndex = 1000,FModifyTime = DateTime.Now},
                    };
                    db.SerialNumberSet.AddRange(serialNumbers);
                    db.SaveChanges();
                }
                if (db.ReadContentSet.Count() == 0)
                {
                    var readContents = GetTestInitReadContentData();
                    db.ReadContentSet.AddRange(readContents);
                    db.SaveChanges();
                }
            }
        }

        private List<ReadContent> GetTestInitReadContentData()
        {
            List<ReadContent> readContents = new List<ReadContent>();
            for (int i = 0; i < 5; i++)
            {
                Read read = new Read();
                read._id = "";
                read._openid = "oT_iO4tBijzyXT91iOW6wmGF801Q";
                read.fcreateBy = "choc";
                read.fcreateTime = DateTime.Now.ToString();
                read.fdocStatus = "C";
                read.flevel = "1";
                read.fcnContent = "<p style=\"text - align:left; line - height:150 % \">         < span style = \"font-family:宋体;color:black;background:yellow;background:yellow\" > 自动生成应收单的任务设置为定时重复执行，如设置每天晚上 </ span >< span style = \"color:black;background:yellow;background:yellow\" > 12 </ span >< span style = \"font-family:宋体;color:black;background:yellow;background:yellow\" > 点执行一次。在执行时，检查数据库中未生成应收单的数据：</ span >                         </ p > <p style=\"margin - left:24px; text - align:left; line - height:150 % \">            < span style = \";color:black;background:yellow;background:yellow\" > 1.< span style = \"font:9px &#39;Times New Roman&#39;\" > &nbsp; &nbsp; &nbsp; &nbsp; </ span ></ span >< span style = \"font-family:宋体;color:black;background:yellow;background:yellow\" > 该部分数据没有对应合同号，则根据不同客户生成对应的应收单，每个客户一张；如果有部分记录找不到默认价目表，则该记录对应的客户名下的所有 </ span >< span style = \"font-family:宋体;color:black;background:red;background:red\" > 没有销售订单的数据 </ span >< span style = \"font-family:宋体;color:black;background:yellow;background:yellow\" > 都不生成应收单，同时应该有报错信息给到用户。</ span >                                              </ p > ";
                read.fenContent = "<p style=\"text - align:left; line - height:150 % \">         < span style = \"font-family:宋体;color:black;background:yellow;background:yellow\" > 自动生成应收单的任务设置为定时重复执行，如设置每天晚上 </ span >< span style = \"color:black;background:yellow;background:yellow\" > 12 </ span >< span style = \"font-family:宋体;color:black;background:yellow;background:yellow\" > 点执行一次。在执行时，检查数据库中未生成应收单的数据：</ span >                         </ p > ";
                read.frecordFileId1 = "cloud://reaclass-dev-b66f08.7265-reaclass-dev-b66f08/read/xty.mp3";
                read.frecordFileId2 = "cloud://reaclass-dev-b66f08.7265-reaclass-dev-b66f08/read/xty.mp3";
                read.fnumber = "Read20181124001"+i;
                read.fname = "choc test init" +i;
                read.fmodifyBy = "choc";
                read.fmodifyTime = DateTime.Now.ToString();

                ReadContent readContent = new ReadContent();
                readContent.FNumber = read.fnumber;
                readContent.FName = read.fname;
                readContent.FLevel = read.flevel;
                readContent.FJsonData = JsonHelper.SerializeObject(read);
                readContent.FCreateTime = DateTime.Now;
                readContent.FModifyTime = DateTime.Now;
                readContents.Add(readContent);
            }
            return readContents;
        }

    }
}

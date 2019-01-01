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
using RaeClass.Config;
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
            services.AddScoped<IFormContentRepository, FormContentRepository>();
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
            app.UseMvcWithDefaultRoute();

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
                if (db.BaseFormContentSet.Count() == 0)
                {
                    var readContents = GetTestInitReadContentData();
                    db.BaseFormContentSet.AddRange(readContents);
                    db.SaveChanges();
                }
            }
        }

        private List<BaseFormContent> GetTestInitReadContentData()
        {
            List<BaseFormContent> readContents = new List<BaseFormContent>();
            for (int i = 0; i < 10; i++)
            {
                FormContent read = new FormContent();
                read.id = Guid.NewGuid().ToString();
                read._openid = "oT_iO4tBijzyXT91iOW6wmGF801Q";
                read.fcreateBy = "choc";
                read.fcreateTime = DateTime.Now.ToString();
                read.fdocStatus = "C";
                read.flevel = "1";
                read.fcnContent = "<p><br/></p><article><p><span style=\"color: rgb(79, 129, 189); \"><strong>版权声明：如果该文章对你有帮助,请为我打c</strong></span>all	https://blog.csdn.net/voke_/article/details/76418116</p><p style=\"box - sizing: border - box; outline: 0px; margin - top: 0px; margin - bottom: 16px; padding: 0px; font - size: 16px; color: rgb(79, 79, 79); line - height: 26px; overflow - wrap: break-word; \"><br/></p></article><p><br/></p><p><br/></p>";
                read.fenContent = "<p><br/></p><article><p><span style=\"color: rgb(79, 129, 189); \"><strong>版权声明：如果该文章对你有帮助,请为我打c</strong></span>all	https://blog.csdn.net/voke_/article/details/76418116</p><p style=\"box - sizing: border - box; outline: 0px; margin - top: 0px; margin - bottom: 16px; padding: 0px; font - size: 16px; color: rgb(79, 79, 79); line - height: 26px; overflow - wrap: break-word; \"><br/></p></article><p><br/></p><p><br/></p>";
                read.frecordFileId1 = "xty.mp3";
                read.frecordFileId2 = "xty.mp3";
                read.fnumber = "Read20181124001"+i;
                read.fname = "choc test init" +i;
                read.fmodifyBy = "choc";
                read.fmodifyTime = DateTime.Now.ToString();

                BaseFormContent readContent = new BaseFormContent();
                readContent.FContentType = RaeClassContentType.Read.ToString();
                readContent.FNumber = read.fnumber;
                readContent.FDocStatus = DocStatus.AUDIT;
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

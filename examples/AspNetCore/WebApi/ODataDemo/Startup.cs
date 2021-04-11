using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ODataDemo.Models;
using System;

namespace ODataDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // ��� in-memory ���ݿ�
            var databaseName = Guid.NewGuid().ToString();
            services
                .AddEntityFrameworkInMemoryDatabase()
                .AddDbContext<AppDbContext>((sp, options) => options.UseInMemoryDatabase(databaseName).UseInternalServiceProvider(sp));

            // ��� OData��
            services.AddOData();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                // �� OData 6.0.0 �����ϵİ汾��Ĭ���޷�ʹ����Щ���ܣ���Ҫ�ڴ˴�ָ������
                endpoints.Select().Expand().Filter().Count().OrderBy();
                // ���� OData ��·��ǰ׺���� http://*:5000/odata/[controller] ���� OData ��������
                endpoints.MapODataRoute("odata", "odata", AppEdmModel.GetModel());
            });

            // ��ʼ��ʾ������
            using (var scpoe = app.ApplicationServices.CreateScope())
            {
                DataGenerator.InitSampleData(scpoe.ServiceProvider);
            }
        }
    }
}

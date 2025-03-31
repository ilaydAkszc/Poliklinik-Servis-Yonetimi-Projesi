using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Owin;
using Hangfire;
using Hangfire.SqlServer;
using ProjectPoliklinik_Servis_Final.Models.Services;

namespace ProjectPoliklinik_Servis_Final
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Hangfire yapılandırması
            GlobalConfiguration.Configuration
                .UseSqlServerStorage("HangfireDb"); // Veritabanı bağlantısını düzenleyin

            app.UseHangfireDashboard(); // Hangfire Dashboard
            app.UseHangfireServer();    // Hangfire Server

            // Günlük görev zamanlaması
            RecurringJob.AddOrUpdate(
                "GuncelleMüsaitlikDurumu",
                () => new GuncelleMüsaitlikServisi().GuncelleMüsaitlikDurumu(),
                Cron.Daily); // Her gün çalıştır
        }
    }
}
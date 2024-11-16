using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

namespace Aprojectbackend.Models;

public partial class AprojectContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            //用config變數拿到設定檔
            IConfiguration Config = new ConfigurationBuilder() //用來讀取應用程式的設定
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory) //所有.NET程式都是跑在AppDomain裡面，所以透過AppDomain獲取當前應用程式的執行目錄
                .AddJsonFile("appsettings.json")
                .Build();
            optionsBuilder.UseSqlServer(Config.GetConnectionString("Aproject")); //透過參數讀設定檔的連線
        }
    }


}

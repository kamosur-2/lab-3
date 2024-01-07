using lab_2.DictionaryComponents;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace lab_3;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        var dbPath = Path.Join(path, "blogging.db");
        // устанавливаем контекст данных
        services.AddDbContext<StorageContext>(options => options.UseSqlite($"Data Source={dbPath}"));
 
        services.AddControllers(); // используем контроллеры без представлений
    }
 
    public void Configure(IApplicationBuilder app)
    {
        app.UseDeveloperExceptionPage();
 
        app.UseRouting();
 
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers(); // подключаем маршрутизацию на контроллеры
        });
    }
}
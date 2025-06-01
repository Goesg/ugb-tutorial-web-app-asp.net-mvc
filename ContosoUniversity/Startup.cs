using System;
using ContosoUniversity.Data;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        ConectarNoBancoViaDocker(services);

        //Filtro de exceção de banco de dados
        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddControllersWithViews();
    }

    //Utilizar essa conexao caso esteja rodando o projeto no Visual Studio
    internal void ConectarNoBancoIntegradoAoVisualStudio(IServiceCollection services)
    {
        services.AddDbContext<SchoolContext>(options =>
       options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
    }

    internal void ConectarNoBancoViaDocker(IServiceCollection services)
    {
        services.AddDbContext<SchoolContext>(options =>
       options.UseSqlServer(Configuration.GetConnectionString("DockerDb")));
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
    }

}

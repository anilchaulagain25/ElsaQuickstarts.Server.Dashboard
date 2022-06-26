using Elsa;
using Elsa.Persistence.EntityFramework.Core.Extensions;
using Elsa.Persistence.EntityFramework.Sqlite;


//create namespace



public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    private IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        var elsaSection = Configuration.GetSection("Elsa");

        // Elsa services.
        services
            .AddElsa(elsa => elsa
                .UseEntityFrameworkPersistence(ef => ef.UseSqlite())
                .AddConsoleActivities()
                .AddHttpActivities(elsaSection.GetSection("Server").Bind)
                .AddQuartzTemporalActivities()
                .AddWorkflowsFrom<Startup>()
            );

        // Elsa API endpoints.
        services.AddElsaApiEndpoints();

        // For Dashboard.
        services.AddRazorPages();
    }



    public void Configure(IApplicationBuilder app)
    {
        if (true)
        {
            app.UseDeveloperExceptionPage();
        }

        app
            .UseStaticFiles() // For Dashboard.
            .UseHttpActivities()
            .UseRouting()
            .UseEndpoints(endpoints =>
            {
                // Elsa API Endpoints are implemented as regular ASP.NET Core API controllers.
                endpoints.MapControllers();

                // For Dashboard.
                endpoints.MapFallbackToPage("/_Host");
            });
    }
}
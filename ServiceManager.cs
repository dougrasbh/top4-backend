using BryophytaClassifier.Database;
using Microsoft.EntityFrameworkCore;

namespace BryophytaClassifier;

public static class ServiceManager {
    public static void ConfigureServices(IServiceCollection services, ConfigurationManager config) {
        AddCustomServices(services);

        services.AddDbContext<AppDbContext>((svcProvider, options) =>
            options.UseNpgsql(config.GetConnectionString("BryophytaClassifierDatabase"))
                .AddInterceptors(svcProvider.GetRequiredService<AuditableEntitiesInterceptor>())
        );
        services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    private static void AddCustomServices(IServiceCollection services) {
        services.AddSingleton<AuditableEntitiesInterceptor>();
    }
}

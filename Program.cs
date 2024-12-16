using BryophytaClassifier.Database;
using Microsoft.EntityFrameworkCore;

namespace BryophytaClassifier;

public static class Program {
    public static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);

        ServiceManager.ConfigureServices(builder.Services, builder.Configuration);

        var app = builder.Build();
        
        using var scope = app.Services.CreateScope();

        scope.ServiceProvider.GetService<AppDbContext>()!.Database.Migrate();

        // Configure the HTTP request pipeline.
        //if (app.Environment.IsDevelopment()) {
            app.UseSwagger();
            app.UseSwaggerUI();
        //}

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}

using InboundApi.Data;
using InboundApi.Data.Repositories;
using InboundApi.Models;
using InboundApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        var rabbitMqSettings = Configuration.GetSection("RabbitMqSettings").Get<RabbitMqSettings>();
        var fileSettings = Configuration.GetSection("FileSettings").Get<FileSettings>();
        services.AddSingleton(rabbitMqSettings);
        services.AddSingleton(fileSettings);
        services.AddSingleton<RabbitMqConnectionFactory>();

        // Register IConnection using the RabbitMqConnectionFactory
        services.AddSingleton<IConnection>(sp =>
        {
            var connectionFactory = sp.GetRequiredService<RabbitMqConnectionFactory>();
            return connectionFactory.CreateRabbitMqConnection();
        });

        services.AddDbContext<AppDbContext>(options =>
           options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IRabbitMqService, RabbitMqService>();
        services.AddScoped<IMyLoggerService, MyLoggerService>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IMyLoggerRepository, MyLoggerRepository>();
        services.AddHostedService<RabbitMqFileProcessorService>();

        services.AddControllers();

        services.AddEndpointsApiExplorer(); 
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Inbound API",
            });
        });

    }

    public void Configure(IApplicationBuilder app, IHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Inbound API");
            });
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers(); 
        });
    }
}

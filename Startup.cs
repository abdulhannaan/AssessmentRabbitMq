using InboundApi.Data;
using InboundApi.Data.Repositories;
using InboundApi.Models;
using InboundApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IConfiguration _configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")));

        services.Configure<FileSettings>(_configuration.GetSection("FileSettings"));

        var rabbitMqSettings = _configuration.GetSection("RabbitMqSettings").Get<RabbitMqSettings>();
        services.AddSingleton(rabbitMqSettings);

        services.AddSingleton<RabbitMqConnectionFactory>();

        // Register IConnection using the RabbitMqConnectionFactory
        services.AddSingleton<IConnection>(sp =>
        {
            var connectionFactory = sp.GetRequiredService<RabbitMqConnectionFactory>();
            return connectionFactory.CreateRabbitMqConnection();
        });

      

        services.AddSingleton<IRabbitMqService, RabbitMqService>();
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

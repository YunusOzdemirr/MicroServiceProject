using System;
using Consul;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;

namespace IdentityService.API.Extensions
{
    public static class ConsulRegistration
    {
        public static IServiceCollection ConfigureConsul(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
            {
                var address = configuration["ConsulConfig:Address"];
                consulConfig.Address = new Uri(address);
            }));

            return services;
        }

        public static IApplicationBuilder RegisterWithConsul(this IApplicationBuilder app, IHostApplicationLifetime lifetime, IConfiguration configuration)
        {
            var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();

            var loggingFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();

            var logger = loggingFactory.CreateLogger<IApplicationBuilder>();

            //var features = app.Properties["server.Features"] as FeatureCollection;
           // var addresses = features.Get<IServerAddressesFeature>();
           // var address = addresses.Addresses.First();
            //var uri = configuration.GetValue<Uri>("ConsulConfig:Address");
            //var serviceName = configuration.GetValue<string>("ConsulConfig:ServiceName");
            //var serviceId = configuration.GetValue<string>("ConsulConfig:ServiceId");

           // var uri = new Uri(address);

            var registration = new AgentServiceRegistration
            {
                ID = "IdentityService",
                Name = "IdentityService",
                Address = "localhost",//$"{uri.Host}",
                Port = 5005,//uri.Port,
                Tags = new[] { "Identity Service", "Identity", "Token", "JWT" }
            };

            //var registration = new AgentServiceRegistration()
            //{
            //    ID = serviceId ?? "IdentityService",
            //    Name = serviceName ?? "IdentityService",
            //    Address = $"{uri.Host}",
            //    Port = uri.Port,
            //    Tags = new[] { serviceName, serviceId }
            //};

            logger.LogInformation("Registering with Consul");
            consulClient.Agent.ServiceDeregister(registration.ID).Wait();
            consulClient.Agent.ServiceRegister(registration).Wait();

            lifetime.ApplicationStopping.Register(() =>
            {
                logger.LogInformation("Deregistering from Consul");
                consulClient.Agent.ServiceDeregister(registration.ID).Wait();
            });

            return app;
        }
    }
}


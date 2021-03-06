using MediaValet.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SupervisorAPI.Logging;
using SupervisorAPI.Service.BusinessLogic;
using SupervisorAPI.Service.Contract;

namespace SupervisorAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSingleton<ILog, Log>();

            QueueCreator.CreateAzureQueues(AzureStorageConnection.ConnectionString, StorageEntity.OrderStorageQueue);

            services.AddSingleton<IOrderQueue, OrderQueue>();

            TableCreator.CreateAzureTables(AzureStorageConnection.ConnectionString, StorageEntity.ConfirmationStorageTable);

            TableCreator.CreateAzureTables(AzureStorageConnection.ConnectionString, StorageEntity.OrderCountStorageTable);

            services.AddSingleton<IConfirmationTable, ConfirmationTable>();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILog logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.ConfigureExceptionHandler(logger);

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MedicalOfficeCore.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TitanInformationSolutions.Data;

namespace TitanInformationSolutions
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<TitanInformationSolutionsContext>();
                    context.Database.Migrate();
                    BGCSeedData.Initialize(services);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
                //try
                //{
                //    var context = services.GetRequiredService<ApplicationDbContext>();
                //    context.Database.Migrate();
                //    ApplicationSeedData.Initialize(services);
                //}
                //catch (Exception ex)
                //{
                //    var logger = services.GetRequiredService<ILogger<Program>>();
                //    logger.LogError(ex, "An error occurred seeding the DB.");
                //}
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}

//-----------------------------------------------------------------------
// <copyright file="AppSetup.cs" company="Undersoft">
//     Author: Dariusz Hanc
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;

namespace UltimatR
{
    public partial class AppSetup : IAppSetup
    {
        public static bool usedExternal;

        IApplicationBuilder app;
        IWebHostEnvironment env;

        public AppSetup(IApplicationBuilder application) { app = application; }

        public AppSetup(IApplicationBuilder application, IWebHostEnvironment environment)
        {
            app = application;
            env = environment;
            UseStandardSetup(new string[] { "1" });
        }

        public AppSetup(IApplicationBuilder application, IWebHostEnvironment environment, string[] apiVersions)
        {
            app = application;
            env = environment;
            UseStandardSetup(apiVersions);
        }

        public IAppSetup RebuildProviders()
        {
            if(usedExternal)
            {
                UseExternalProvider();
            } else
            {
                UseInternalProvider();
            }

            return this;
        }

        public IAppSetup UseDataClients()
        {
            RepositoryManager.LoadClientEdms(app).ConfigureAwait(true);
            return this;
        }

        public IAppSetup UseDataMigrations()
        {
            using(IServiceScope scope = ServiceManager.GetProvider().CreateScope())
            {
                IUltimateService us = scope.ServiceProvider.GetRequiredService<IUltimateService>();
                us.GetEndpoints().ForEach(e => ((DbContext)e.Context).Database.Migrate());
            }

            return this;
        }

        public IAppSetup UseExternalProvider()
        {
            IServiceManager sm = ServiceManager.GetManager();
            sm.Registry.MergeServices();
            ServiceManager.SetProvider(app.ApplicationServices);
            usedExternal = true;
            return this;
        }

        public IAppSetup UseInternalProvider()
        {
            IServiceManager sm = ServiceManager.GetManager();
            sm.Registry.MergeServices();
            ServiceManager.BuildInternalProvider();
            app.ApplicationServices = ServiceManager.GetProvider();
            usedExternal = false;
            return this;
        }

        public IAppSetup UseStandardSetup(string[] apiVersions)
        {
            if(env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseODataBatching();
            app.UseODataQueryRequest();

            app.UseRouting();
            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            UseSwaggerSetup(apiVersions);

            app.UseEndpoints(endpoints => endpoints.MapControllers());

            return this;
        }

        public IAppSetup UseSwaggerSetup(string[] apiVersions)
        {
            if(app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            app.UseSwagger();
            //List<string> apiVersions = new List<string>() { "1", "2" };
            //app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1"); });
            app.UseSwaggerUI(
                s =>
                {
                    foreach(string apiVersion in apiVersions)
                    {
                        s.SwaggerEndpoint($"/swagger/{apiVersion}/swagger.json", $" v{apiVersion}");
                    }
                });
            return this;
        }
    }
}
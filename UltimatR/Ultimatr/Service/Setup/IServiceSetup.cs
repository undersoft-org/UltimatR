using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace UltimatR
{
    public interface IServiceSetup
    {
        IServiceSetup AddMapper(IDataMapper mapper);
        IServiceSetup AddMapper(params Profile[] profiles);
        IServiceSetup AddMapper<TProfile>() where TProfile : Profile;
        IServiceSetup AddDataService(IMvcBuilder mvc, Type contextType, string routePrefix = null, int? pageLimit = null);
        IServiceSetup AddDataService<TContext>(IMvcBuilder mvc, string routePrefix = null, int? pageLimit = null) where TContext : DbContext;
        IServiceSetup ConfigureDataServices(IMvcBuilder mvc, int? pageLimit = null);
        IServiceSetup AddCaching();
        IServiceSetup ConfigureServices(Assembly[] assemblies = null);
        IServiceSetup ConfigureEndpoints(Assembly[] assemblies = null);
        IServiceSetup ConfigureClients(Assembly[] assemblies = null);
        IServiceSetup AddImplementations(Assembly[] assemblies = null);
        IServiceSetup ConfigureApiVersions(string[] apiVersions);
    }
}
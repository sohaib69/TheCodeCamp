using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using AutoMapper;
using TheCodeCamp.Data;

namespace TheCodeCamp
{
    public class AutofacConfig
    {
        public static void Register()
        {
            var bldr = new ContainerBuilder();
            var config = GlobalConfiguration.Configuration;
            bldr.RegisterApiControllers(Assembly.GetExecutingAssembly());
            RegisterServices(bldr);
            bldr.RegisterWebApiFilterProvider(config);
            bldr.RegisterWebApiModelBinderProvider();
            var container = bldr.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static void RegisterServices(ContainerBuilder bldr)
        {
            // Custom Implimentation for AutoMapper.
            bldr.Register(c => new MapperConfiguration(cfg => { cfg.AddProfile(new CampMappingProfile()); })).AsSelf().SingleInstance();
            bldr.Register(c => c.Resolve<MapperConfiguration>().CreateMapper(c.Resolve)).As<IMapper>().InstancePerLifetimeScope();

            bldr.RegisterType<CampContext>()
              .InstancePerRequest();

            bldr.RegisterType<CampRepository>()
              .As<ICampRepository>()
              .InstancePerRequest();
        }
    }
}

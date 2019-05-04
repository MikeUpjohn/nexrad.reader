using Autofac;
using System.Web.Http;
using Autofac.Integration.WebApi;
using System.Reflection;
using nexrad.api.Controllers;
using nexrad.reader.Level2;
using nexrad.reader.Level2.IndividualMessages;

namespace nexrad.api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<HomeController>().InstancePerRequest();
            builder.RegisterType<Level2RadarReader>().As<ILevel2RadarReader>().InstancePerRequest();
            builder.RegisterType<Level2RecordReader>().As<ILevel2RecordReader>().InstancePerRequest();
            builder.RegisterType<Message31Reader>().As<IMessage31Reader>().InstancePerRequest();
            builder.RegisterType<MessageHeaderReader>().As<IMessageHeaderReader>().InstancePerRequest();
            builder.RegisterType<ByteReader>().As<IByteReader>().InstancePerRequest();
            builder.RegisterType<DataLogger>().As<IDataLogger>().InstancePerRequest();
            builder.RegisterType<Level2MessageReader>().As<ILevel2MessageReader>().InstancePerRequest();

            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}

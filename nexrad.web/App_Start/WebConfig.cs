using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using nexrad.api.Controllers;
using nexrad.reader.Level2;
using nexrad.reader.Level2.IndividualMessages;
using nexrad.web.Controllers;

namespace nexrad.web.App_Start {
    public static class WebApiConfig {
        public static void Register() {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            //builder.RegisterType<HomeController>().InstancePerRequest();
            builder.RegisterType<Level2RadarReader>().As<ILevel2RadarReader>().InstancePerRequest();
            builder.RegisterType<Level2RecordReader>().As<ILevel2RecordReader>().InstancePerRequest();
            builder.RegisterType<Message31Reader>().As<IMessage31Reader>().InstancePerRequest();
            builder.RegisterType<MessageHeaderReader>().As<IMessageHeaderReader>().InstancePerRequest();
            builder.RegisterType<ByteReader>().As<IByteReader>().InstancePerRequest();
            builder.RegisterType<DataLogger>().As<IDataLogger>().InstancePerRequest();
            builder.RegisterType<Level2MessageReader>().As<ILevel2MessageReader>().InstancePerRequest();

            var container = builder.Build();
            //config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
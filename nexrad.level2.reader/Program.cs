using Autofac;
using nexrad.reader.Autofac;

namespace nexrad.reader
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var container = IocContainerBuilder.BuildContainer();

            using (var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<IApplicationService>();
                app.Run();
            }
        }
    }
}

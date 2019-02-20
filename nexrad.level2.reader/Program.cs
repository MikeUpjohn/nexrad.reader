using Autofac;
using nexrad.level2.reader.Autofac;

namespace nexrad.level2.reader
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

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using TagCloudBuilder.Infrastructure;
using Autofac;
using NUnit.Framework.Internal;
using TagCloudBuilder.Domain;

namespace TagCloudBuilder.App
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            var container = SetContainer();
            Application.Run(new AppTagCloud(container.GetValueOrThrow()));
        }
        

        private static Result<IContainer> SetContainer() => Result.Of(() =>
        {
            var builder = new ContainerBuilder();
            var assm = Assembly.Load("TagCloudBuilder");
            foreach (var type in assm.GetTypes().Where(t => t.IsInterface))
                builder.RegisterAssemblyTypes(assm)
                    .Named(t => t.Name, type)
                    .AsImplementedInterfaces();
            builder.RegisterType<PngDrawer>().Named<ITextRectanglesDrawer>("PngDrawer");
            builder.RegisterInstance(new Logger("CloudLogger", InternalTraceLevel.Debug, TextWriter.Null))
                .As<ILogger>();
            return builder.Build();
        });
    }
}
using System;
using System.Windows.Forms;
using FractalPainting.App.Fractals;
using FractalPainting.Infrastructure.Common;
using FractalPainting.Infrastructure.UiActions;
using Ninject;
using Ninject.Extensions.Factory;
using Ninject.Extensions.Conventions;

namespace FractalPainting.App
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            try
            {
                var container = new StandardKernel();
                container.Bind(kernel =>
                    kernel
                        .FromThisAssembly()
                        .SelectAllClasses()
                        .InheritedFrom<IUiAction>()
                        .BindAllInterfaces());

                container.Bind<IImageHolder, PictureBoxImageHolder>()
                    .To<PictureBoxImageHolder>().InSingletonScope();
                container.Bind<Palette>().ToSelf().InSingletonScope();
                container.Bind<KochPainter>().ToSelf().InSingletonScope();
                container.Bind<IDragonPainterFactory>().ToFactory();
                container.Bind<MenuStrip>().ToSelf();

                container.Bind<IObjectSerializer>().To<XmlObjectSerializer>();
                container.Bind<IBlobStorage>().To<FileBlobStorage>();
                container.Bind<AppSettings, IImageSettingsProvider, IImageDirectoryProvider>()
                    .ToMethod(kernel => kernel.Kernel.Get<SettingsManager>().Load()).InSingletonScope();
                
                container.Bind<Form>().To<MainForm>();
                

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                var mainForm = container.Get<Form>();
                Application.Run(mainForm);
                
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
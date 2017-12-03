using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Autofac;
using TagsCloudContainer.Interfaces;
using TagsCloudContiner;

namespace TagsCloudContainer
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.Run(new AppTagsCloud());
        }
    }
}
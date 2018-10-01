using System.Windows;
using BIF.SWE2.Interfaces;

namespace PicDB
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, IApplication
    {
        void IApplication.HelloWorld()
        {
            // I'm here, that's fine
        }
    }
}

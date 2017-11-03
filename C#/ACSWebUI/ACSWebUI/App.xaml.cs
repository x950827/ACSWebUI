using ACSWebUI.Locators;
using CefSharp.Wpf;
using GalaSoft.MvvmLight.Threading;
using Microsoft.Practices.ServiceLocation;

namespace ACSWebUI {
    public partial class App {
        public App() {
            DispatcherHelper.Initialize();

            Locator.SetAndReg();

            ServiceLocator.Current.GetInstance<ChromiumWebBrowser>();
        }
    }
}
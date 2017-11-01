using System;
using System.IO;
using ACSWebUI.ViewModel;
using CefSharp;
using CefSharp.Wpf;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace ACSWebUI.Locator {
    public class Locator {
        static Locator() {
        }

        public static void SetAndReg() {
            var settings = new CefSettings() {
                RemoteDebuggingPort = 8088,
                CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CefSharp\\Cache")
            };
            Cef.Initialize(settings, true, null);
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<ChromiumWebBrowser>();
            SimpleIoc.Default.Register<ViewModel.ViewModel>();
            SimpleIoc.Default.Register<IWorkerEditor, WorkerEditor>();
            SimpleIoc.Default.Register<IPassageEditor, PassageEditor>();
            SimpleIoc.Default.Register<IWorkerReader, WorkerReader>();
            SimpleIoc.Default.Register<IPassageReader, PassageReader>();
            SimpleIoc.Default.Register<AuthorizationViewModel>();
            SimpleIoc.Default.Register<AccessDatabase>();
        }

        public static ViewModel.ViewModel MainViewModel => ServiceLocator.Current.GetInstance<ViewModel.ViewModel>();

        public static ChromiumWebBrowser Browser => ServiceLocator.Current.GetInstance<ChromiumWebBrowser>();

        public static AuthorizationViewModel AuthorizationViewModel => ServiceLocator.Current.GetInstance<AuthorizationViewModel>();

        public static void Cleanup() {
            // TODO Clear the ViewModels
        }
    }
}
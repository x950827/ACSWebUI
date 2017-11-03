using System;
using System.IO;
using ACSWebUI.Cards.Writers;
using ACSWebUI.Common.Functions.Cards.Readers;
using ACSWebUI.Common.Functions.Cards.Writers;
using ACSWebUI.Common.Functions.Editors;
using ACSWebUI.Common.Functions.Readers;
using ACSWebUI.Database;
using ACSWebUI.Database.Functions.Editors;
using ACSWebUI.Database.Functions.Readers;
using ACSWebUI.Devices.Cards;
using ACSWebUI.Devices.Cards.Readers;
using ACSWebUI.ViewModel.Flyouts;
using CefSharp;
using CefSharp.Wpf;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace ACSWebUI.Locators {
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
            SimpleIoc.Default.Register<IKmoonReader, KmoonReader>();
            SimpleIoc.Default.Register<IKmoonWriter, KmoonWriter>();

            SimpleIoc.Default.Register<CardCommon>();
            SimpleIoc.Default.Register<AuthorizationViewModel>();
            SimpleIoc.Default.Register<WorkersDatabase>();
            SimpleIoc.Default.Register<PassageDatabase>();
        }

        public static ViewModel.ViewModel ViewModel => ServiceLocator.Current.GetInstance<ViewModel.ViewModel>();

        public static ChromiumWebBrowser Browser => ServiceLocator.Current.GetInstance<ChromiumWebBrowser>();

        public static AuthorizationViewModel AuthorizationViewModel => ServiceLocator.Current.GetInstance<AuthorizationViewModel>();


        public static void Cleanup() {
            // TODO Clear the ViewModels
        }
    }
}
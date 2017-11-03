using System;
using System.ComponentModel;
using System.Windows;
using ACSWebUI.Locators;
using CefSharp;
using MahApps.Metro.Controls.Dialogs;

namespace ACSWebUI {
    public partial class MainWindow {
        private bool shutdown;
        public MainWindow() {
            InitializeComponent();
            Locator.Browser.BrowserSettings = new BrowserSettings { DefaultEncoding = "UTF-8" };
            Locator.Browser.RegisterJsObject("viewModel", Locator.ViewModel);
            Locator.Browser.Name = "browser";
            MainGrid.Children.Add(Locator.Browser);
            Closing += OnClosing;
            Closed += OnClosed;
        }
        private void OnClosed(object sender, EventArgs eventArgs) {
            Locator.ViewModel.StopCapture();
            Locator.ViewModel.getHistory();
        }

        private async void OnClosing(object sender, CancelEventArgs e) {
            if (e.Cancel)
                return;
            e.Cancel = !shutdown;
            if (shutdown)
                return;

            var settings = new MetroDialogSettings {
                AffirmativeButtonText = "Выход",
                NegativeButtonText = "Отмена",
                AnimateShow = true,
                AnimateHide = false,
            };

            var result = await this.ShowMessageAsync("Quit application?",
                "Вы действительно хотите закрыть Access Control System?",
                MessageDialogStyle.AffirmativeAndNegative, settings);

            shutdown = result == MessageDialogResult.Affirmative;

            if (!shutdown)
                return;
            Locator.ViewModel.StopCapture();
            Locator.Cleanup();
            Application.Current.Shutdown();
        }
    }
}
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ACSWebUI.AdditionalObjects;
using ACSWebUI.Common.Extensions;
using ACSWebUI.Common.Functions.Generator;
using ACSWebUI.Locators;
using CefSharp;
using Newtonsoft.Json;

namespace ACSWebUI.ViewModel.Flyouts {
    public class AuthorizationViewModel : AutoViewModelBase {
        private string address = "http://osora.ru/scanner/external/index/";
        private WebUIConfiguration configuration;

        public AuthorizationViewModel() {
            configuration = WebUIConfiguration.ReadConfiguration();
        }

        private bool isFlyoutOpen;
        public bool IsFlyoutOpen {
            get => isFlyoutOpen;
            private set => Set(() => IsFlyoutOpen, ref isFlyoutOpen, value);
        }

        private string uniqueId;
        public string UniqueId {
            get => uniqueId;
            set => Set(() => UniqueId, ref uniqueId, value);
        }

        private Visibility isAuthorizationResponceVisibile;
        public Visibility IsAuthorizationResponceVisibile {
            get => isAuthorizationResponceVisibile;
            set => Set(() => IsAuthorizationResponceVisibile, ref isAuthorizationResponceVisibile, value);
        }

        private Visibility isAuthorizationVisible;
        public Visibility IsAuthorizationVisible {
            get => isAuthorizationVisible;
            set => Set(() => IsAuthorizationVisible, ref isAuthorizationVisible, value);
        }

        private bool isProgressRingActive;
        public bool IsProgressRingActive {
            get => isProgressRingActive;
            set => Set(() => IsProgressRingActive, ref isProgressRingActive, value);
        }

        private string authorizationResponce;
        public string AuthorizationResponce {
            get => authorizationResponce;
            set => Set(() => AuthorizationResponce, ref authorizationResponce, value);
        }

        public ICommand OnLoadedCommand { get; } = new AutoRelayCommand(nameof(OnLoaded));
        private void OnLoaded() {
            IsFlyoutOpen = true;
            IsProgressRingActive = false;
            IsAuthorizationVisible = Visibility.Visible;
            IsAuthorizationResponceVisibile = Visibility.Hidden;
            UniqueId = UniqueIdGenerator.GetId();
            if (configuration.isCheckedBefore)
                Check();
        }

        public ICommand CheckCommand { get; } = new AutoRelayCommand(nameof(Check));
        private async void Check() {
            IsProgressRingActive = true;
            IsAuthorizationVisible = Visibility.Hidden;
            var response = uniqueId.ToHttpGetRequest("http://osora.ru/scanner/API/auth/?code=");
            var obj = new {
                success = "",
                data = new {
                    html = "",
                    deviceType = ""
                }
            };
            var json = JsonConvert.DeserializeAnonymousType(response, obj);

            if (json.success == "false") {
                await Task.Delay(1000);
                AuthorizationResponce = "Авторизация не пройдена. Попробуйте еще раз или обратитесь к администратору для идентефикации устройства";
                IsAuthorizationResponceVisibile = Visibility.Visible;
                IsProgressRingActive = false;
                await Task.Delay(1000);
                IsAuthorizationResponceVisibile = Visibility.Hidden;
                IsAuthorizationVisible = Visibility.Visible;
                return;
            }
            if (json.success != "true")
                return;

            Locator.Browser.LoadHtml(json.data.html, "http://osora.ru/scanner");

            if (json.data.deviceType == "office_kpp")
                Locator.ViewModel.RunCaupture();
            await Task.Delay(1000);
            IsAuthorizationResponceVisibile = Visibility.Visible;
            IsProgressRingActive = false;
            AuthorizationResponce = "Устройство успешно распознано";
            await Task.Delay(2000);
            IsAuthorizationResponceVisibile = Visibility.Hidden;
            IsAuthorizationVisible = Visibility.Visible;
            IsFlyoutOpen = false;
        }

        public ICommand ExitCommand { get; } = new AutoRelayCommand(nameof(Exit));
        private void Exit() {
            if (Application.Current.MainWindow != null)
                Application.Current.MainWindow.Close();
        }
    }
}
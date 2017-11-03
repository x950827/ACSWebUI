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
        private readonly WebUIConfiguration configuration;

        public AuthorizationViewModel() {
            configuration = WebUIConfiguration.ReadConfiguration();
            GetRequestAddress = configuration.getRequestAddress;
            DomenAddress = configuration.domenAddress;
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

        private string getReauestAddress;
        public string GetRequestAddress {
            get => getReauestAddress;
            set => Set(() => GetRequestAddress, ref getReauestAddress, value);
        }

        private string domenAddress;
        public string DomenAddress {
            get => domenAddress;
            set => Set(() => DomenAddress, ref domenAddress, value);
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

        private Visibility isSettingVisibility = Visibility.Hidden;
        public Visibility IsSettingsVisibility {
            get => isSettingVisibility;
            set => Set(() => IsSettingsVisibility, ref isSettingVisibility, value);
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
            if (string.IsNullOrEmpty(configuration.getRequestAddress)) {
                configuration.getRequestAddress = "http://osora.ru/scanner/API/auth/?code=";
                configuration.WriteConfiguration();
            }
            var response = uniqueId.ToHttpGetRequest(configuration.getRequestAddress);
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

            //uniqueId.ToHttpGetRequest("http://osora.ru/scanner/API/auth/?code=");
            if (string.IsNullOrEmpty(configuration.domenAddress)) {
                configuration.domenAddress = "http://osora.ru/scanner";
                configuration.WriteConfiguration();
            }
            Locator.Browser.LoadHtml(json.data.html, configuration.domenAddress);
            if (json.data.deviceType == "office_kpp") {
                Locator.ViewModel.RunCaupture();
                Locator.ViewModel.TestVisibility = Visibility.Visible;
            }
            Locator.Browser.FrameLoadEnd += BrowserOnFrameLoadEnd;
            
        }

        public ICommand OpenSettingCommand { get; } = new AutoRelayCommand(nameof(OpenSettings));
        private void OpenSettings() {
            if (IsSettingsVisibility == Visibility.Hidden)
                IsSettingsVisibility = Visibility.Visible;
        }

        public ICommand SaveChahgesCommand { get; } = new AutoRelayCommand(nameof(SaveChanges));
        private void SaveChanges() {
            configuration.getRequestAddress = GetRequestAddress;
            configuration.domenAddress = DomenAddress;
            configuration.WriteConfiguration();
            IsSettingsVisibility = Visibility.Hidden;
        }


        private async void BrowserOnFrameLoadEnd(object sender, FrameLoadEndEventArgs frameLoadEndEventArgs) {
            await Task.Delay(1000);

            if (!configuration.isCheckedBefore)
            {
                configuration.isCheckedBefore = true;
                configuration.WriteConfiguration();
            }

            IsAuthorizationResponceVisibile = Visibility.Visible;
            IsProgressRingActive = false;
            AuthorizationResponce = "Устройство успешно распознано";
            await Task.Delay(2000);
            IsAuthorizationResponceVisibile = Visibility.Hidden;
            IsAuthorizationVisible = Visibility.Visible;
            Locator.Browser.FrameLoadEnd -= BrowserOnFrameLoadEnd;
            IsFlyoutOpen = false;
        }

        public ICommand ExitCommand { get; } = new AutoRelayCommand(nameof(Exit));
        private void Exit() {
            if (Application.Current.MainWindow != null)
                Application.Current.MainWindow.Close();
        }
    }
}
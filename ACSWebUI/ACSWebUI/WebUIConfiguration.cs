using ACSWebUI.Common;

namespace ACSWebUI {
    public class WebUIConfiguration : ConfigurationFile<WebUIConfiguration> {
        protected override string ConfigurationFileName => "WebUiConfigurationFile";

        public bool isCheckedBefore;
        public string getRequestAddress;
        public string domenAddress;
    }
}
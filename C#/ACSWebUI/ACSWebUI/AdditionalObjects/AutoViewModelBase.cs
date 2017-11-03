using System.Linq;
using System.Windows.Input;
using ACSWebUI.Extensions;
using GalaSoft.MvvmLight;

namespace ACSWebUI.AdditionalObjects {
    public class AutoViewModelBase : ViewModelBase {
        protected AutoViewModelBase() {
            SetCommands();
        }

        private void SetCommands() {
            if (IsInDesignMode)
                return;

            GetType().GetProperties()
                .Where(propertyInfo => propertyInfo.PropertyType == typeof(ICommand))
                .ForEach(propertyInfo => (propertyInfo.GetValue(this) as AutoRelayCommand)?.SetObject(this));
        }
    }
}
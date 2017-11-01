using System;
using System.Reflection;
using System.Threading;
using System.Windows.Input;
using GalaSoft.MvvmLight.Helpers;

namespace ACSWebUI.AdditionalObjects {
    public class AutoRelayCommand : ICommand {
        private readonly string executeActionName;
        private readonly string canExecuteActionName;
        private MethodInfo executeMethodInfo;
        private MethodInfo canExecuteMethodInfo;
        private object parentObject;

        private readonly WeakAction execute;
        private readonly WeakFunc<bool> canExecute;
        private EventHandler requerySuggestedLocal;

        public event EventHandler CanExecuteChanged {
            add {
                if (canExecute == null)
                    return;
                var eventHandler = requerySuggestedLocal;
                EventHandler comparand;
                do {
                    comparand = eventHandler;
                    eventHandler = Interlocked.CompareExchange(ref requerySuggestedLocal, comparand + value, comparand);
                }
                while (eventHandler != comparand);
                CommandManager.RequerySuggested += value;
            }
            remove {
                if (canExecute == null)
                    return;
                var eventHandler = requerySuggestedLocal;
                EventHandler comparand;
                do {
                    comparand = eventHandler;
                    eventHandler = Interlocked.CompareExchange(ref requerySuggestedLocal, comparand - value, comparand);
                }
                while (eventHandler != comparand);
                CommandManager.RequerySuggested -= value;
            }
        }

        public AutoRelayCommand(string executeActionName = null, string canExecuteActionName = null) {
            this.executeActionName = executeActionName;
            this.canExecuteActionName = canExecuteActionName;

            execute = new WeakAction(() => executeMethodInfo.Invoke(parentObject, null));
            canExecute = new WeakFunc<bool>(() => (bool?)canExecuteMethodInfo.Invoke(parentObject, null) != false);
        }

        public void SetObject(object parentObject) {
            this.parentObject = parentObject;
            executeMethodInfo = GetMethodInfo(this.parentObject, executeActionName);
            canExecuteMethodInfo = GetMethodInfo(this.parentObject, canExecuteActionName);
        }
        private MethodInfo GetMethodInfo(object parentObject, string methodName) {
            return string.IsNullOrEmpty(methodName)
                ? null
                : (parentObject.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static)
                    ?? throw new ArgumentException($"Class {parentObject.GetType().Name} not contains method with name {methodName}"));
        }

        public void Execute(object parameter) {
            if (CanExecute(parameter) && executeMethodInfo != null)
                execute.Execute();
        }

        public bool CanExecute(object parameter) {
            return canExecuteMethodInfo == null || canExecute.Execute();
        }
    }
}
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

using System;
using System.Windows.Input;
using ACSWebUI.AdditionalObjects;
using ACSWebUI.Common.Functions.Editors;
using ACSWebUI.Common.Functions.Readers;
using ACSWebUI.Common.Model;
using ACSWebUI.Devices;
using ACSWebUI.Locators;
using CefSharp;
using Newtonsoft.Json;


namespace ACSWebUI.ViewModel {
    public class ViewModel : AutoViewModelBase {
        private readonly IPassageEditor passageEditor;
        private readonly IPassageReader passageReader;
        private readonly IWorkerEditor workerEditor;
        private readonly IWorkerReader workerReader;
        private readonly IpCamers ipCamera = new IpCamers();

        public ViewModel(IWorkerReader workerReader, IWorkerEditor workerEditor, IPassageReader passageReader, IPassageEditor passageEditor) {
            this.workerReader = workerReader;
            this.workerEditor = workerEditor;
            this.passageReader = passageReader;
            this.passageEditor = passageEditor;
        }

        public void RunCaupture() {
            ipCamera.StartTest("http://192.168.11.134:80/?camid=1" /*new[] {"http://192.168.0.105:80/?camid=1", "http://192.168.0.105:80/?camid=2", "http://192.168.0.105:80/?camid=0" }*/);
        }

        public void StopCapture() {
            ipCamera.StopCapture();
        }

        public void getHistory() {
            var message = passageReader.GetAllInJson().Replace("[", string.Empty).Replace("]", string.Empty);
            var script = @"ReceiveHistory('" + message + @"');";
            Locator.Browser.ExecuteScriptAsync(script);
        }

        public void receiveWorkers(string jsonWorker) {
            var workers = JsonConvert.DeserializeObject<Worker[]>(jsonWorker);
            workerEditor.AddMany(workers);
            Locator.Browser.ExecuteScriptAsync(@"SynchronizationSet();");
        }

        public void statusHistory(string jsonMessage) {
            var obj = new { Message = "" };
            var response = JsonConvert.DeserializeAnonymousType(jsonMessage, obj);
            switch (response.Message) {
                case "sucsess":
                    break;
                case "error":
                    return;
                case "Error internet disconnected":
                    Locator.Browser.ExecuteScriptAsync(@"SynchronizationSet();");
                    return;
                case "OK":
                    return;
            }
        }

        public void open() {
        }

        public void ignore() {
        }

        public void deleteWorker(string jsonMessage) {
            var obj = new { id = "" };
            var response = JsonConvert.DeserializeAnonymousType(jsonMessage, obj);
            workerEditor.Delete(Convert.ToInt32(response.id));
        }

        public void print() {
        }

        public void createSkip(string jsonMessage) {
            var obj = new {
                id_worker = "",
                fio = "",
                work_position = "",
                temporary = "",
                date_end = ""
            };
            var response = JsonConvert.DeserializeAnonymousType(jsonMessage, obj);
        }

        public void encodeCard(string jsonMessage) {
            var obj = new { id = "" };
            var resp = JsonConvert.DeserializeAnonymousType(jsonMessage, obj);
        }

        public void changeWorker(string jsonMessage) {
            var obj = new {
                id = "",
                fio = "",
                work_position = "",
                active = "",
                temporary = "",
                date_end = ""
            };
            var resp = JsonConvert.DeserializeAnonymousType(jsonMessage, obj);
            var worker = new Worker {
                id_worker = Convert.ToInt32(resp.id),
                work_position = resp.work_position,
                active = Convert.ToInt32(resp.active),
                temporary = Convert.ToInt32(resp.temporary),
                date_end = resp.date_end,
                fio = resp.fio
            };
            workerEditor.Change(worker);
        }

        public void FindWorkerAfterDecoding(string code) {
            var result = workerReader.Get(code);
            if (result == null) {
                Locator.Browser.ExecuteScriptAsync(@"InputWorkerUnknown(0);");
                return;
            }

            var passage = new Passage {
                id_worker = result.id_worker,
                date = DateTime.Now.ToShortDateString()
            };
            passageEditor.Add(passage);

            var datetime = Convert.ToDateTime(result.date_end);
            if (datetime <= DateTime.Now) {
                var script = @"InputWorkerExpired(1, '" + result.fio + @"', '" + result.work_position + @"', '" + result.foto + @"'); InputAction('1');";
                Locator.Browser.ExecuteScriptAsync(script);
                return;
            }
            if (datetime < DateTime.Now)
                return;
            {
                var script = @"InputWorkerSuccess(1, '" + result.fio + @"', '" + result.work_position + @"', '" + result.date_end + @"', '" + result.foto + @"'); InputAction('1');";
                Locator.Browser.ExecuteScriptAsync(script);
            }
        }

        public ICommand OpenDebugCommand { get; } = new AutoRelayCommand(nameof(OpenDebug));
        private void OpenDebug() {
            Locator.Browser.ShowDevTools();
        }
    }
}
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

using System;
using System.Windows;
using System.Windows.Input;
using ACSWebUI.AdditionalObjects;
using ACSWebUI.Common.Functions.Cards.Readers;
using ACSWebUI.Common.Functions.Cards.Writers;
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
        private readonly IKmoonWriter kmoonWriter;
        private IKmoonReader kmoonReader;
        private readonly IpCamers ipCamera = new IpCamers();

        public ViewModel(IWorkerReader workerReader, IWorkerEditor workerEditor, IPassageReader passageReader, IPassageEditor passageEditor, IKmoonWriter kmoonWriter, IKmoonReader kmoonReader) {
            this.workerReader = workerReader;
            this.workerEditor = workerEditor;
            this.passageReader = passageReader;
            this.passageEditor = passageEditor;
            this.kmoonWriter = kmoonWriter;
            this.kmoonReader = kmoonReader;
        }

        private Visibility testVisibility = Visibility.Hidden;
        public Visibility TestVisibility {
            get => testVisibility;
            set => Set(() => TestVisibility, ref testVisibility, value);
        }

        public void RunCaupture() {
            //ipCamera.StartTest("http://192.168.11.134:80/?camid=1" /*new[] {"http://192.168.0.105:80/?camid=1", "http://192.168.0.105:80/?camid=2", "http://192.168.0.105:80/?camid=0" }*/);
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
            Locator.Browser.ExecuteScriptAsync(@"synchronizationSet();");
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
                    Locator.Browser.ExecuteScriptAsync(@"synchronizationSet();");
                    return;
                case "OK":
                    return;
            }
        }

        public void statusAuth(string message) {
            if (message == "Not authorized")
                Locator.Browser.ExecuteScriptAsync(@"auth('" + Locator.AuthorizationViewModel.UniqueId + @"');");
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
            //var obj = new {
            //    info = new {
            //        fio = "",
            //        work_position = "",
            //        temporary = "",
            //        date_end = ""
            //    }
            //};
            //var response = JsonConvert.DeserializeAnonymousType(jsonMessage, obj);
            Locator.Browser.ExecuteScriptAsync(@"receiveWorkers();");
        }

        public void encodeCard(string jsonMessage) {
            var obj = new { id_worker = "", code = "" };
            var resp = JsonConvert.DeserializeAnonymousType(jsonMessage, obj);

            var cardResponse = kmoonWriter.WriteData(resp.code);
            Locator.Browser.ExecuteScriptAsync(cardResponse ? @"encodeCardSucces();" : @"encodeCardError();");
        }

        public void changeWorker(string jsonMessage) {
            //var obj = new {
            //    info = new {
            //        id_worker = "",
            //        update = new {
            //            fio = "",
            //            work_position = "",
            //            active = "",
            //            temporary = "",
            //            date_end = ""
            //        }
            //    }
            //};
            //var resp = JsonConvert.DeserializeAnonymousType(jsonMessage, obj);
            //var worker = new Worker {
            //    id_worker = Convert.ToInt32(resp.info.id_worker),
            //    work_position = resp.info.update.work_position,
            //    active = Convert.ToInt32(resp.info.update.active),
            //    temporary = Convert.ToInt32(resp.info.update.temporary),
            //    date_end = resp.info.update.date_end,
            //    fio = resp.info.update.fio
            //};
            //workerEditor.Change(worker);
            Locator.Browser.ExecuteScriptAsync(@"receiveWorkers();");
        }

        public void FindWorkerAfterDecoding(string code) {
            var result = workerReader.Get(code);
            if (result == null) {
                Locator.Browser.ExecuteScriptAsync(@"InputWorkerUnknown(0);");
                return;
            }

            var passage = new Passage {
                skip_id = result.id_worker,
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

        public ICommand StartCardReadingCommand { get; } = new AutoRelayCommand(nameof(StartCardReading));
        private void StartCardReading() {
            var code = kmoonReader.ReadFullData();
            if (code.Contains("Fail")) {
                MessageBox.Show("Card reading error");
                return;
            }
            var result = workerReader.Get(code);
            if (result == null)
            {
                Locator.Browser.ExecuteScriptAsync(@"InputWorkerUnknown(0);");
                return;
            }

            var passage = new Passage
            {
                skip_id = result.id_worker,
                date = DateTime.Now.ToShortDateString()
            };
            passageEditor.Add(passage);

            var datetime = Convert.ToDateTime(result.date_end);
            if (datetime <= DateTime.Now)
            {
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
    }
}
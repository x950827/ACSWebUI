using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using ACSWebUI.Common.Extensions;
using ACSWebUI.Locators;
using AForge.Video;
using GalaSoft.MvvmLight;
using ZXing;

namespace ACSWebUI.Devices {
    public class IpCamers : ViewModelBase {
        private Bitmap bitmap;
        private DispatcherTimer timer;
        private readonly BarcodeReader barcodeReader = new BarcodeReader {
            Options = {
                PossibleFormats = new List<BarcodeFormat> {
                    BarcodeFormat.QR_CODE
                }
            }
        };

        private string previousResult;

        public IpCamers() {
            timer = new DispatcherTimer();
        }

        private ObservableCollection<MJPEGStream> streams = new ObservableCollection<MJPEGStream>();
        public ObservableCollection<MJPEGStream> Streams {
            get => streams;
            set => Set(() => Streams, ref streams, value);
        }

        private ObservableCollection<string> addresses = new ObservableCollection<string>();
        public ObservableCollection<string> Addresses {
            get => addresses;
            set => Set(() => Addresses, ref addresses, value);
        }

        public void StartCaptire() {
            foreach (var stream in Addresses.Select(address => new MJPEGStream(address))) {
                stream.NewFrame += StreamOnNewFrame;
                stream.VideoSourceError += (sender, args) => {
                    args.Description.ToLog();
                    MessageBox.Show(args.Description);
                };
                Streams.Add(stream);
                stream.Start();
            }
        }

        private MJPEGStream stream;
        public void StartTest(string adr) {
            stream = new MJPEGStream(adr);
            stream.VideoSourceError += (sender, args) => {
                args.Description.ToLog();
                MessageBox.Show(args.Description);
            };
            stream.NewFrame += StreamOnNewFrame;
            stream.Start();
        }

        public void StartTest(string[] adr) {
            foreach (var s in adr) {
                var str = new MJPEGStream(s);
                str.NewFrame += StreamOnNewFrame;
                str.Start();
            }
        }

        public void StopCapture() {
            stream?.SignalToStop();
        }

        private void StreamOnNewFrame(object sender, NewFrameEventArgs eventArgs) {
            try {
                //bitmap?.Dispose();
                //bitmap = eventArgs.Frame.Clone() as Bitmap;
                //var ms = new System.IO.MemoryStream();
                //bitmap?.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                //var sigBase64 = "data:image/bmp;base64," + Convert.ToBase64String(ms.ToArray());
                //var script = @"ShowSecImage('" + sigBase64 + @"');";
                //MainLocator.Browser.EvaluateScriptAsync(script);
                //ms.Dispose();

                var result = barcodeReader.Decode(eventArgs.Frame.Clone() as Bitmap);
                if (result == null)
                    return;
                if (result.Text == previousResult)
                    return;

                previousResult = result.Text;

                if (!timer.IsEnabled) {
                    timer.Tick += (o, args) => {
                        previousResult = null;
                        timer.Stop();
                    };
                    timer.Interval = new TimeSpan(0, 0, 0, 5, 0);
                    timer.Start();
                }
                Locator.ViewModel.FindWorkerAfterDecoding(result.Text);
            }
            catch (Exception e) {
                e.ToLog();
                MessageBox.Show(e.Message);
            }
        }
    }
}
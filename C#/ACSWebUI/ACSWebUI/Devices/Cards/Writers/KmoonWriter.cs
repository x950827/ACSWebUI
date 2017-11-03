using ACSWebUI.Common.Functions.Cards.Readers;
using ACSWebUI.Common.Functions.Cards.Writers;
using ACSWebUI.Devices.Cards;

namespace ACSWebUI.Cards.Writers {
    public class KmoonWriter : IKmoonWriter {
        private readonly CardCommon cardCommon;
        private readonly IKmoonReader kmoonReader;
        public KmoonWriter(CardCommon cardCommon, IKmoonReader kmoonReader) {
            this.cardCommon = cardCommon;
            this.kmoonReader = kmoonReader;
        }

        public bool WriteData(string data) {
            cardCommon.SelectDevice();
            cardCommon.EstablishContext();
            if (!cardCommon.ConnectCard())
                return false;
            var datas = data.Replace("-", "");
            string[] code = {
                datas.Substring(0, 5),
                datas.Substring(5, 5),
                datas.Substring(10, 5),
                datas.Substring(15, 5),
                datas.Substring(20, 5)
            };

            cardCommon.SubmitText(code[0], "4");
            cardCommon.SubmitText(code[1], "5");
            cardCommon.SubmitText(code[2], "6");
            cardCommon.SubmitText(code[3], "8");
            cardCommon.SubmitText(code[4], "9");
            cardCommon.Close();


            var readAgain = kmoonReader.ReadData("4") + "-" +
                kmoonReader.ReadData("5") + "-" +
                kmoonReader.ReadData("6") + "-" +
                kmoonReader.ReadData("8") + "-" +
                kmoonReader.ReadData("9");
            return readAgain != "error" && readAgain == data;
        }
    }
}
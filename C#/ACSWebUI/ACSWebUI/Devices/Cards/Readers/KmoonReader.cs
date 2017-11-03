using ACSWebUI.Common.Functions.Cards.Readers;

namespace ACSWebUI.Devices.Cards.Readers {
    public class KmoonReader : IKmoonReader {
        private readonly CardCommon cardCommon;
        public KmoonReader(CardCommon cardCommon) {
            this.cardCommon = cardCommon;
        }

        public string ReadData(string block) {
            cardCommon.SelectDevice();
            cardCommon.EstablishContext();
            var response = cardCommon.ConnectingToCard();
            return response == "error" ? response : cardCommon.VerifyCard(block);
        }

        public string ReadFullData() {
            cardCommon.SelectDevice();
            cardCommon.EstablishContext();
            return cardCommon.VerifyCard("4") + "-" +
                cardCommon.VerifyCard("5") + "-" +
                cardCommon.VerifyCard("6") + "-" +
                cardCommon.VerifyCard("8") + "-" +
                cardCommon.VerifyCard("9");
        }
    }
}
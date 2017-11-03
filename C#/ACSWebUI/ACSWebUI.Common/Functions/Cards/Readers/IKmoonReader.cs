namespace ACSWebUI.Common.Functions.Cards.Readers {
    public interface IKmoonReader {
        string ReadData(string block);
        string ReadFullData();
    }
}
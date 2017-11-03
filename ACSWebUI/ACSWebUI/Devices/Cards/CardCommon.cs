using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace ACSWebUI.Devices.Cards {
    public class CardCommon {
        int retCode;
        int hCard;
        int hContext;
        int Protocol;
        public bool connActive = false;
        string readername = "ACS ACR122 0"; // change depending on reader
        public byte[] SendBuff = new byte[263];
        public byte[] RecvBuff = new byte[263];
        public int SendLen, RecvLen, nBytesRet, reqType, Aprotocol, dwProtocol, cbPciLength;
        public Card.ScardReaderstate RdrState;
        public Card.ScardIoRequest pioSendRequest;

        public CardCommon() {
            
            //SelectDevice();
            //EstablishContext();
        }

        public string ConnectingToCard() {
            if (ConnectCard()) {
                return GetcardUID();
            }
            return "error";
        }

        public bool ConnectCard() {
            connActive = true;

            retCode = Card.SCardConnect(hContext, readername, Card.ScardShareShared,
                Card.ScardProtocolT0 | Card.ScardProtocolT1, ref hCard, ref Protocol);

            if (retCode == Card.ScardSSuccess)
                return true;

            connActive = false;
            return false;
        }


        public string VerifyCard(String Block) {
            string value = "";
            if (ConnectCard()) {
                value = ReadBlock(Block);
            }

            value = value.Split(new char[] { '\0' }, 2, StringSplitOptions.None)[0].ToString();
            return value;
        }


        public string ReadBlock(String Block) {
            string tmpStr = "";
            int indx;

            if (AuthenticateBlock(Block)) {
                ClearBuffers();
                SendBuff[0] = 0xFF; // CLA 
                SendBuff[1] = 0xB0; // INS
                SendBuff[2] = 0x00; // P1
                SendBuff[3] = (byte)int.Parse(Block); // P2 : Block No.
                SendBuff[4] = (byte)int.Parse("16"); // Le

                SendLen = 5;
                RecvLen = SendBuff[4] + 2;

                retCode = SendAPDUandDisplay(2);

                if (retCode == -200) {
                    return "outofrangeexception";
                }

                if (retCode == -202) {
                    return "BytesNotAcceptable";
                }

                if (retCode != Card.ScardSSuccess) {
                    return "FailRead";
                }

                // Display data in text format
                for (indx = 0; indx <= RecvLen - 1; indx++) {
                    tmpStr = tmpStr + Convert.ToChar(RecvBuff[indx]);
                }

                return (tmpStr);
            }
            else {
                return "FailAuthentication";
            }
        }

        private bool AuthenticateBlock(String block) {
            ClearBuffers();
            SendBuff[0] = 0xFF; // CLA
            SendBuff[2] = 0x00; // P1: same for all source types 
            SendBuff[1] = 0x86; // INS: for stored key input
            SendBuff[3] = 0x00; // P2 : Memory location;  P2: for stored key input
            SendBuff[4] = 0x05; // P3: for stored key input
            SendBuff[5] = 0x01; // Byte 1: version number
            SendBuff[6] = 0x00; // Byte 2
            SendBuff[7] = (byte)int.Parse(block); // Byte 3: sectore no. for stored key input
            SendBuff[8] = 0x60; // Byte 4 : Key A for stored key input
            SendBuff[9] = (byte)int.Parse("1"); // Byte 5 : Session key for non-volatile memory

            SendLen = 0x0A;
            RecvLen = 0x02;

            retCode = SendAPDUandDisplay(0);

            if (retCode != Card.ScardSSuccess) {
                //MessageBox.Show("FAIL Authentication!");
                return false;
            }

            return true;
        }

        // clear memory buffers
        private void ClearBuffers() {
            long indx;

            for (indx = 0; indx <= 262; indx++) {
                RecvBuff[indx] = 0;
                SendBuff[indx] = 0;
            }
        }

        // send application protocol data unit : communication unit between a smart card reader and a smart card
        private int SendAPDUandDisplay(int reqType) {
            int indx;
            string tmpStr = "";

            pioSendRequest.dwProtocol = Aprotocol;
            pioSendRequest.cbPciLength = 8;

            //Display Apdu In
            for (indx = 0; indx <= SendLen - 1; indx++) {
                tmpStr = tmpStr + " " + string.Format("{0:X2}", SendBuff[indx]);
            }

            retCode = Card.SCardTransmit(hCard, ref pioSendRequest, ref SendBuff[0],
                SendLen, ref pioSendRequest, ref RecvBuff[0], ref RecvLen);

            if (retCode != Card.ScardSSuccess) {
                return retCode;
            }

            try {
                tmpStr = "";
                switch (reqType) {
                    case 0:
                        for (indx = (RecvLen - 2); indx <= (RecvLen - 1); indx++) {
                            tmpStr = tmpStr + " " + string.Format("{0:X2}", RecvBuff[indx]);
                        }

                        if ((tmpStr).Trim() != "90 00") {
                            //MessageBox.Show("Return bytes are not acceptable.");
                            return -202;
                        }

                        break;

                    case 1:

                        for (indx = (RecvLen - 2); indx <= (RecvLen - 1); indx++) {
                            tmpStr = tmpStr + string.Format("{0:X2}", RecvBuff[indx]);
                        }

                        if (tmpStr.Trim() != "90 00") {
                            tmpStr = tmpStr + " " + string.Format("{0:X2}", RecvBuff[indx]);
                        }

                        else {
                            tmpStr = "ATR : ";
                            for (indx = 0; indx <= (RecvLen - 3); indx++) {
                                tmpStr = tmpStr + " " + string.Format("{0:X2}", RecvBuff[indx]);
                            }
                        }

                        break;

                    case 2:

                        for (indx = 0; indx <= (RecvLen - 1); indx++) {
                            tmpStr = tmpStr + " " + string.Format("{0:X2}", RecvBuff[indx]);
                        }

                        break;
                }
            }
            catch (IndexOutOfRangeException) {
                return -200;
            }
            return retCode;
        }

        public void SelectDevice() {
            List<string> availableReaders = this.ListReaders();
            this.RdrState = new Card.ScardReaderstate();
            readername = availableReaders[0].ToString(); //selecting first device
            this.RdrState.RdrName = readername;
        }

        public List<string> ListReaders() {
            int ReaderCount = 0;
            List<string> AvailableReaderList = new List<string>();

            //Make sure a context has been established before 
            //retrieving the list of smartcard readers.
            retCode = Card.SCardListReaders(hContext, null, null, ref ReaderCount);
            if (retCode != Card.ScardSSuccess) {
                MessageBox.Show(Card.GetScardErrMsg(retCode));
                //connActive = false;
            }

            byte[] ReadersList = new byte[ReaderCount];

            //Get the list of reader present again but this time add sReaderGroup, retData as 2rd & 3rd parameter respectively.
            retCode = Card.SCardListReaders(hContext, null, ReadersList, ref ReaderCount);
            if (retCode != Card.ScardSSuccess) {
                MessageBox.Show(Card.GetScardErrMsg(retCode));
            }

            string rName = "";
            int indx = 0;
            if (ReaderCount > 0) {
                // Convert reader buffer to string
                while (ReadersList[indx] != 0) {
                    while (ReadersList[indx] != 0) {
                        rName = rName + (char)ReadersList[indx];
                        indx = indx + 1;
                    }

                    //Add reader name to list
                    AvailableReaderList.Add(rName);
                    rName = "";
                    indx = indx + 1;
                }
            }
            return AvailableReaderList;
        }

        internal void EstablishContext() {
            retCode = Card.SCardEstablishContext(Card.ScardScopeSystem, 0, 0, ref hContext);
            if (retCode == Card.ScardSSuccess)
                return;

            MessageBox.Show("Check your device and please restart again", "Reader not connected", MessageBoxButton.OK, MessageBoxImage.Warning);
            connActive = false;
        }


        private string GetcardUID() //only for mifare 1k cards
        {
            string cardUID = "";
            byte[] receivedUID = new byte[256];
            Card.ScardIoRequest request = new Card.ScardIoRequest();
            request.dwProtocol = Card.ScardProtocolT1;
            request.cbPciLength = System.Runtime.InteropServices.Marshal.SizeOf(typeof(Card.ScardIoRequest));
            byte[] sendBytes = new byte[] { 0xFF, 0xCA, 0x00, 0x00, 0x00 }; //get UID command      for Mifare cards
            int outBytes = receivedUID.Length;
            int status = Card.SCardTransmit(hCard, ref request, ref sendBytes[0], sendBytes.Length, ref request, ref receivedUID[0], ref outBytes);

            if (status != Card.ScardSSuccess) {
                cardUID = "Error";
            }
            else {
                cardUID = BitConverter.ToString(receivedUID.Take(4).ToArray()).Replace("-", string.Empty).ToLower();
            }

            return cardUID;
        }

        public void Close() {
            if (connActive) {
                retCode = Card.SCardDisconnect(hCard, Card.ScardUnpowerCard);
            }
            //retCode = Card.SCardReleaseContext(hCard);
        }

        public void SubmitText(String Text, String Block) {
            String tmpStr = Text;
            int indx;
            if (AuthenticateBlock(Block)) {
                ClearBuffers();
                SendBuff[0] = 0xFF; // CLA
                SendBuff[1] = 0xD6; // INS
                SendBuff[2] = 0x00; // P1
                SendBuff[3] = (byte)int.Parse(Block); // P2 : Starting Block No.
                SendBuff[4] = (byte)int.Parse("16"); // P3 : Data length

                for (indx = 0; indx <= (tmpStr).Length - 1; indx++) {
                    SendBuff[indx + 5] = (byte)tmpStr[indx];
                }
                SendLen = SendBuff[4] + 5;
                RecvLen = 0x02;

                retCode = SendAPDUandDisplay(2);

                //if (retCode != Card.SCARD_S_SUCCESS) {
                //    MessageBox.Show("fail write");
                //}
                //else {
                //    MessageBox.Show("write success");
                //}
            }
            else {
                MessageBox.Show("FailAuthentication");
            }
        }
    }
}
using System;
using System.Runtime.InteropServices;

namespace ACSWebUI.Devices.Cards {
    public class Card {
        [StructLayout(LayoutKind.Sequential)]
        public struct ScardIoRequest {
            public int dwProtocol;
            public int cbPciLength;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ApduRec {
            public byte bCLA;
            public byte bINS;
            public byte bP1;
            public byte bP2;
            public byte bP3;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public byte[] Data;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public byte[] SW;
            public bool IsSend;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ScardReaderstate {
            public string RdrName;
            public int UserData;
            public int RdrCurrState;
            public int RdrEventState;
            public int ATRLength;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 37)]
            public byte[] ATRValue;
        }

        public const int ScardSSuccess = 0;
        public const int ScardAtrLength = 33;

        public const int CtMcu = 0x00;
        public const int CtIicAuto = 0x01;
        public const int CtIic_1K = 0x02;
        public const int CtIic_2K = 0x03;
        public const int CtIic_4K = 0x04;
        public const int CtIic_8K = 0x05;
        public const int CtIic16K = 0x06;
        public const int CtIic32K = 0x07;
        public const int CtIic64K = 0x08;
        public const int CtIic128K = 0x09;
        public const int CtIic256K = 0x0A;
        public const int CtIic512K = 0x0B;
        public const int CtIic1024K = 0x0C;
        public const int CtAt88Sc153 = 0x0D;
        public const int CtAt88Sc1608 = 0x0E;
        public const int CtSle4418 = 0x0F;
        public const int CtSle4428 = 0x10;
        public const int CtSle4432 = 0x11;
        public const int CtSle4442 = 0x12;
        public const int CtSle4406 = 0x13;
        public const int CtSle4436 = 0x14;
        public const int CtSle5536 = 0x15;
        public const int CtMcut0 = 0x16;
        public const int CtMcut1 = 0x17;
        public const int CtMcuAuto = 0x18;

        public const int ScardScopeUser = 0;
        public const int ScardScopeTerminal = 1;
        public const int ScardScopeSystem = 2;
        public const int ScardStateUnaware = 0x00;
        public const int ScardStateIgnore = 0x01;
        public const int ScardStateChanged = 0x02;
        public const int ScardStateUnknown = 0x04;
        public const int ScardStateUnavailable = 0x08;
        public const int ScardStateEmpty = 0x10;
        public const int ScardStatePresent = 0x20;
        public const int ScardStateAtrmatch = 0x40;
        public const int ScardStateExclusive = 0x80;
        public const int ScardStateInuse = 0x100;
        public const int ScardStateMute = 0x200;
        public const int ScardStateUnpowered = 0x400;
        public const int ScardShareExclusive = 1;
        public const int ScardShareShared = 2;
        public const int ScardShareDirect = 3;
        public const int ScardLeaveCard = 0;
        public const int ScardResetCard = 1;
        public const int ScardUnpowerCard = 2;
        public const int ScardEjectCard = 3;


        public const long FileDeviceSmartcard = 0x310000;

        public const long IoctlSmartcardDirect = FileDeviceSmartcard + 2050 * 4;
        public const long IoctlSmartcardSelectSlot = FileDeviceSmartcard + 2051 * 4;
        public const long IoctlSmartcardDrawLcdbmp = FileDeviceSmartcard + 2052 * 4;
        public const long IoctlSmartcardDisplayLcd = FileDeviceSmartcard + 2053 * 4;
        public const long IoctlSmartcardClrLcd = FileDeviceSmartcard + 2054 * 4;
        public const long IoctlSmartcardReadKeypad = FileDeviceSmartcard + 2055 * 4;
        public const long IoctlSmartcardReadRtc = FileDeviceSmartcard + 2057 * 4;
        public const long IoctlSmartcardSetRtc = FileDeviceSmartcard + 2058 * 4;
        public const long IoctlSmartcardSetOption = FileDeviceSmartcard + 2059 * 4;
        public const long IoctlSmartcardSetLed = FileDeviceSmartcard + 2060 * 4;
        public const long IoctlSmartcardLoadKey = FileDeviceSmartcard + 2062 * 4;
        public const long IoctlSmartcardReadEeprom = FileDeviceSmartcard + 2065 * 4;
        public const long IoctlSmartcardWriteEeprom = FileDeviceSmartcard + 2066 * 4;
        public const long IoctlSmartcardGetVersion = FileDeviceSmartcard + 2067 * 4;
        public const long IoctlSmartcardGetReaderInfo = FileDeviceSmartcard + 2051 * 4;
        public const long IoctlSmartcardSetCardType = FileDeviceSmartcard + 2060 * 4;
        public const long IoctlSmartcardAcr128EscapeCommand = FileDeviceSmartcard + 2079 * 4;

        public const int ScardFInternalError = -2146435071;
        public const int ScardECancelled = -2146435070;
        public const int ScardEInvalidHandle = -2146435069;
        public const int ScardEInvalidParameter = -2146435068;
        public const int ScardEInvalidTarget = -2146435067;
        public const int ScardENoMemory = -2146435066;
        public const int ScardFWaitedTooLong = -2146435065;
        public const int ScardEInsufficientBuffer = -2146435064;
        public const int ScardEUnknownReader = -2146435063;


        public const int ScardETimeout = -2146435062;
        public const int ScardESharingViolation = -2146435061;
        public const int ScardENoSmartcard = -2146435060;
        public const int ScardEUnknownCard = -2146435059;
        public const int ScardECantDispose = -2146435058;
        public const int ScardEProtoMismatch = -2146435057;


        public const int ScardENotReady = -2146435056;
        public const int ScardEInvalidValue = -2146435055;
        public const int ScardESystemCancelled = -2146435054;
        public const int ScardFCommError = -2146435053;
        public const int ScardFUnknownError = -2146435052;
        public const int ScardEInvalidAtr = -2146435051;
        public const int ScardENotTransacted = -2146435050;
        public const int ScardEReaderUnavailable = -2146435049;
        public const int ScardPShutdown = -2146435048;
        public const int ScardEPciTooSmall = -2146435047;

        public const int ScardEReaderUnsupported = -2146435046;
        public const int ScardEDuplicateReader = -2146435045;
        public const int ScardECardUnsupported = -2146435044;
        public const int ScardENoService = -2146435043;
        public const int ScardEServiceStopped = -2146435042;

        public const int ScardWUnsupportedCard = -2146435041;
        public const int ScardWUnresponsiveCard = -2146435040;
        public const int ScardWUnpoweredCard = -2146435039;
        public const int ScardWResetCard = -2146435038;
        public const int ScardWRemovedCard = -2146435037;

        public const int ScardProtocolUndefined = 0x00;
        public const int ScardProtocolT0 = 0x01;
        public const int ScardProtocolT1 = 0x02;
        public const int ScardProtocolRaw = 0x10000;
        public const int ScardUnknown = 0;
        public const int ScardAbsent = 1;
        public const int ScardPresent = 2;
        public const int ScardSwallowed = 3;
        public const int ScardPowered = 4;
        public const int ScardNegotiable = 5;
        public const int ScardSpecific = 6;

        [DllImport("winscard.dll")]
        public static extern int SCardEstablishContext(int dwScope, int pvReserved1, int pvReserved2, ref int phContext);

        [DllImport("winscard.dll")]
        public static extern int SCardReleaseContext(int phContext);

        [DllImport("winscard.dll")]
        public static extern int SCardConnect(int hContext, string szReaderName, int dwShareMode, int dwPrefProtocol, ref int phCard, ref int activeProtocol);

        [DllImport("winscard.dll")]
        public static extern int SCardBeginTransaction(int hCard);

        [DllImport("winscard.dll")]
        public static extern int SCardDisconnect(int hCard, int disposition);

        [DllImport("winscard.dll")]
        public static extern int SCardListReaderGroups(int hContext, ref string mzGroups, ref int pcchGroups);

        [DllImport("winscard.DLL", EntryPoint = "SCardListReadersA", CharSet = CharSet.Ansi)]
        public static extern int SCardListReaders(
            int hContext,
            byte[] groups,
            byte[] readers,
            ref int pcchReaders
        );


        [DllImport("winscard.dll")]
        public static extern int SCardStatus(int hCard, string szReaderName, ref int pcchReaderLen, ref int state, ref int protocol, ref byte atr, ref int atrLen);

        [DllImport("winscard.dll")]
        public static extern int SCardEndTransaction(int hCard, int disposition);

        [DllImport("winscard.dll")]
        public static extern int SCardState(int hCard, ref uint state, ref uint protocol, ref byte atr, ref uint atrLen);

        [DllImport("WinScard.dll")]
        public static extern int SCardTransmit(IntPtr hCard,
                                               ref ScardIoRequest pioSendPci,
                                               ref byte[] pbSendBuffer,
                                               int cbSendLength,
                                               ref ScardIoRequest pioRecvPci,
                                               ref byte[] pbRecvBuffer,
                                               ref int pcbRecvLength);

        [DllImport("winscard.dll")]
        public static extern int SCardTransmit(int hCard, ref ScardIoRequest pioSendRequest, ref byte sendBuff, int sendBuffLen, ref ScardIoRequest pioRecvRequest, ref byte recvBuff, ref int recvBuffLen);

        [DllImport("winscard.dll")]
        public static extern int SCardTransmit(int hCard, ref ScardIoRequest pioSendRequest, ref byte[] sendBuff, int sendBuffLen, ref ScardIoRequest pioRecvRequest, ref byte[] recvBuff, ref int recvBuffLen);

        [DllImport("winscard.dll")]
        public static extern int SCardControl(int hCard, uint dwControlCode, ref byte sendBuff, int sendBuffLen, ref byte recvBuff, int recvBuffLen, ref int pcbBytesReturned);

        [DllImport("winscard.dll")]
        public static extern int SCardGetStatusChange(int hContext, int timeOut, ref ScardReaderstate readerState, int readerCount);

        public static string GetScardErrMsg(int returnCode) {
            switch (returnCode) {
                case ScardECancelled:
                    return ("The action was canceled by an SCardCancel request.");
                case ScardECantDispose:
                    return ("The system could not dispose of the media in the requested manner.");
                case ScardECardUnsupported:
                    return ("The smart card does not meet minimal requirements for support.");
                case ScardEDuplicateReader:
                    return ("The reader driver didn't produce a unique reader name.");
                case ScardEInsufficientBuffer:
                    return ("The data buffer for returned data is too small for the returned data.");
                case ScardEInvalidAtr:
                    return ("An ATR string obtained from the registry is not a valid ATR string.");
                case ScardEInvalidHandle:
                    return ("The supplied handle was invalid.");
                case ScardEInvalidParameter:
                    return ("One or more of the supplied parameters could not be properly interpreted.");
                case ScardEInvalidTarget:
                    return ("Registry startup information is missing or invalid.");
                case ScardEInvalidValue:
                    return ("One or more of the supplied parameter values could not be properly interpreted.");
                case ScardENotReady:
                    return ("The reader or card is not ready to accept commands.");
                case ScardENotTransacted:
                    return ("An attempt was made to end a non-existent transaction.");
                case ScardENoMemory:
                    return ("Not enough memory available to complete this command.");
                case ScardENoService:
                    return ("The smart card resource manager is not running.");
                case ScardENoSmartcard:
                    return ("The operation requires a smart card, but no smart card is currently in the device.");
                case ScardEPciTooSmall:
                    return ("The PCI receive buffer was too small.");
                case ScardEProtoMismatch:
                    return ("The requested protocols are incompatible with the protocol currently in use with the card.");
                case ScardEReaderUnavailable:
                    return ("The specified reader is not currently available for use.");
                case ScardEReaderUnsupported:
                    return ("The reader driver does not meet minimal requirements for support.");
                case ScardEServiceStopped:
                    return ("The smart card resource manager has shut down.");
                case ScardESharingViolation:
                    return ("The smart card cannot be accessed because of other outstanding connections.");
                case ScardESystemCancelled:
                    return ("The action was canceled by the system, presumably to log off or shut down.");
                case ScardETimeout:
                    return ("The user-specified timeout value has expired.");
                case ScardEUnknownCard:
                    return ("The specified smart card name is not recognized.");
                case ScardEUnknownReader:
                    return ("The specified reader name is not recognized.");
                case ScardFCommError:
                    return ("An internal communications error has been detected.");
                case ScardFInternalError:
                    return ("An internal consistency check failed.");
                case ScardFUnknownError:
                    return ("An internal error has been detected, but the source is unknown.");
                case ScardFWaitedTooLong:
                    return ("An internal consistency timer has expired.");
                case ScardSSuccess:
                    return ("No error was encountered.");
                case ScardWRemovedCard:
                    return ("The smart card has been removed, so that further communication is not possible.");
                case ScardWResetCard:
                    return ("The smart card has been reset, so any shared state information is invalid.");
                case ScardWUnpoweredCard:
                    return ("Power has been removed from the smart card, so that further communication is not possible.");
                case ScardWUnresponsiveCard:
                    return ("The smart card is not responding to a reset.");
                case ScardWUnsupportedCard:
                    return ("The reader cannot communicate with the card, due to ATR string configuration conflicts.");
                default:
                    return ("?");
            }
        }
    }
}
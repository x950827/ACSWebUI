using ACSWebUI.Common.Serializers;

namespace ACSWebUI.Common.Extensions {
    public static class ObjectExtensions {
        public static byte[] ToXml(this object obj) {
            return XmlSerializer.Serializing(obj);
        }
    }
}
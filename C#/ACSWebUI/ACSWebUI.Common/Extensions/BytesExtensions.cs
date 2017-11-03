using ACSWebUI.Common.Serializers;

namespace ACSWebUI.Common.Extensions {
    public static class BytesExtensions {
        public static TKey FromXml<TKey>(this byte[] bytes) {
            return XmlSerializer.Deserializing<TKey>(bytes);
        }
    }
}
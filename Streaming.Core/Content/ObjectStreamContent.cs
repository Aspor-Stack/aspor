using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace Aspor.Streaming.Core.Content
{
    public class ObjectStreamContent<E> : IStreamContent where E : class
    {

        private static JsonSerializer SERIALIZER = new JsonSerializer
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        };

        public ObjectStreamContent() {}

        public ObjectStreamContent(E content)
        {
            Value = content;
        }

        public E Value { get; private set; }

        public byte[] Encode()
        {
            return Encoding.UTF8.GetBytes(JToken.FromObject(Value, SERIALIZER).ToString());
        }

        public void Decode(byte[] data)
        {
            Value = JToken.Parse(Encoding.UTF8.GetString(data)).ToObject<E>();
        }

    }
}

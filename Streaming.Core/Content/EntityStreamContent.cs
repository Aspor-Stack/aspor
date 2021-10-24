using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Text;

namespace Aspor.Streaming.Core.Content
{
    public class EntityStreamContent<E> : IStreamContent where E : class
    {

        public EntityStreamContent() {}

        public EntityStreamContent(E content)
        {
            Instance = content;
            AffectedProperties = new List<string>();
        }

        public EntityStreamContent(E content, IEnumerable<string> affectedProperties)
        {
            Instance = content;
            AffectedProperties = affectedProperties;
        }

        public E Instance { get; private set; }

        public IEnumerable<string> AffectedProperties { get; private set; }

        public byte[] Encode()
        {
            JObject raw = new JObject();
            raw["instance"] = JObject.FromObject(Instance);
            raw["affectedProperties"] = JArray.FromObject(AffectedProperties);
            return Encoding.UTF8.GetBytes(JToken.FromObject(raw).ToString());
        }

        public void Decode(byte[] data)
        {
            JToken raw = JToken.Parse(Encoding.UTF8.GetString(data));
            Instance = raw["instance"].ToObject<E>();
            AffectedProperties = raw["affectedProperties"].ToObject<List<string>>();
        }

    }
}

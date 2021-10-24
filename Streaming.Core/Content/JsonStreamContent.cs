using Newtonsoft.Json.Linq;
using System.Text;

namespace Aspor.Streaming.Core.Content
{
    public class JsonStreamContent : IStreamContent
    {

        public JsonStreamContent() {}

        public JsonStreamContent(JToken content)
        {
            Root = content;
        }

        public JToken Root { get; private set; }

        public byte[] Encode()
        {
            return Encoding.UTF8.GetBytes(Root.ToString());
        }

        public void Decode(byte[] data)
        {
            Root = JToken.Parse(Encoding.UTF8.GetString(data));
        }

    }
}

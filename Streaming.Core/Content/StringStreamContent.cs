using System.Text;

namespace Aspor.Streaming.Core.Content
{
    public class StringStreamContent : IStreamContent
    {

        public StringStreamContent() {}

        public StringStreamContent(string content)
        {
            Value = content;
        }

        public string Value { get; private set; }

        public byte[] Encode()
        {
            return Encoding.UTF8.GetBytes(Value);
        }

        public void Decode(byte[] data)
        {
            Value = Encoding.UTF8.GetString(data);
        }

    }
}

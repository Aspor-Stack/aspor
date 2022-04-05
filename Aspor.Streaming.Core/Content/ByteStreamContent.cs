
namespace Aspor.Streaming.Core.Content
{
    public class ByteStreamContent : IStreamContent
    {

        public ByteStreamContent() {}

        public ByteStreamContent(byte[] content)
        {
            Value = content;
        }

        public byte[] Value { get; private set; }

        public byte[] Encode()
        {
            return Value;
        }

        public void Decode(byte[] data)
        {
            Value = data;
        }

    }
}

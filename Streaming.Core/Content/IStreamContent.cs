
using System;

namespace Aspor.Streaming.Core.Content
{
    public interface IStreamContent
    {

        byte[] Encode();

        void Decode(byte[] data);

        public static T CreateAndDecode<T>(byte[] data) where T : IStreamContent, new()
        {
            T instance = new T();
            instance.Decode(data);
            return instance;
        }
    }
}

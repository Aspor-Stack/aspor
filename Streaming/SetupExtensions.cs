using Microsoft.Extensions.DependencyInjection;
using Streaming;

namespace Aspor.Streaming
{

    public static class SetupExtensions
    {

        public static IMvcBuilder AddAsporODataStreaming(this IMvcBuilder mvc, StreamMode mode)
        {
            mvc.AddMvcOptions(config =>
            {
                if(mode == StreamMode.AUTO) config.Filters.Add<AsporODataAutoStreamingFilter>();
                else config.Filters.Add<AsporODataStreamingFilter>();
            });
            return mvc;
        }
    }

}

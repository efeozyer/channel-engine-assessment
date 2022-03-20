namespace ChannelEngine.Assessment.Infrastructure.Integrations.ChannelEngine
{
    public class ChannelEngineClient : IChannelEngineClient
    {
        private readonly ChannelEngineClientConfig _config;

        public ChannelEngineClient(ChannelEngineClientConfig config)
        {
            _config = config;
        }
    }
}

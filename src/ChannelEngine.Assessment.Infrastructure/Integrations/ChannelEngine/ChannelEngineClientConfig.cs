namespace ChannelEngine.Assessment.Infrastructure.Integrations.ChannelEngine
{
    public class ChannelEngineClientConfig : IntegrationClientConfig
    {
        public string ApiKey { get; set; }

        public override string Name => "ChannelEngine";
    }
}

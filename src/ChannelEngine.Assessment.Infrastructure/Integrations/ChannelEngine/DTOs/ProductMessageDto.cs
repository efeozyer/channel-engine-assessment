using System.Collections.Generic;

namespace ChannelEngine.Assessment.Infrastructure.Integrations.ChannelEngine
{
    public class ProductMessageDto
    {
        public string Name { get; set; }
        public string Reference { get; set; }
        public List<string> Warnings { get; set; }
        public List<string> Errors { get; set; }
    }
}

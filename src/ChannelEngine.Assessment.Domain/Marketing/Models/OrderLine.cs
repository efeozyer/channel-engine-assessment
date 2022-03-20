namespace ChannelEngine.Assessment.Domain.Marketing.Models
{
    public class OrderLine
    {
        public int Quantity { get; set; }

        public string ProductId { get; set; }
        public string Gtin { get; set; }
    }
}

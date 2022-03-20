namespace ChannelEngine.Assessment.Infrastructure.Integrations.ChannelEngine
{
    public class PagingResponse<TModel> : ApiResponse
    {
        public TModel[] Content { get; set; }

        public long Count { get; set; }

        public long TotalCount { get; set; }

        public long ItemsPerPage { get; set; }
    }
}

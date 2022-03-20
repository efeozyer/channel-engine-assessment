using System;
using System.Collections.Generic;

namespace ChannelEngine.Assessment.Infrastructure.Integrations.ChannelEngine.Messages
{
    /// <summary>
    /// Get orders request
    /// </summary>
    public class GetOrdersByFilterRequest : PagingRequest
    {
        /// <summary>
        /// Order status(es) to filter on. AWAITING_PAYMENT orders will be excluded if it is not included in this Statuses filter.
        /// </summary>
        public List<string> Statuses { get; set; }

        /// <summary>
        /// Client emailaddresses to filter on.
        /// </summary>
        public List<string> EmailAddresses { get; set; }

        /// <summary>
        /// Filter on unique order reference used by the merchant.
        /// </summary>
        public List<string> MerchantOrderNos { get; set; }

        /// <summary>
        /// Filter on unique order reference used by the channel.
        /// </summary>
        public List<string> ChannelOrderNos { get; set; }

        /// <summary>
        /// Filter on the order date, starting from this date. This date is inclusive.
        /// The order date is based on the data we got from a channel.
        /// </summary>
        public DateTime? FromDate { get; set; }

        /// <summary>
        /// Filter on the order date, until this date. This date is exclusive.
        /// The order date is based on the data we got from a channel
        /// </summary>
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// Filter on the created at date in ChannelEngine, starting from this date. This date is inclusive.
        /// The created date is set on the date and time when the order is created
        /// </summary>
        public DateTime? FromCreatedAtDate { get; set; }

        /// <summary>
        /// Filter on the created at date in ChannelEngine, until this date. This date is exclusive.
        /// The created date is set on the date and time when the order is created
        /// </summary>
        public DateTime? ToCreatedAtDate { get; set; }

        /// <summary>
        /// Exclude order (lines) fulfilled by the marketplace (amazon:FBA, bol:LVB, etc.)
        /// </summary>
        public bool? ExcludeMarketplaceFulfilledOrdersAndLines { get; set; }

        /// <summary>
        /// Filter orders on fulfillment type. This will include all orders lines, even if they are partially fulfilled by the marketplace.
        /// To exclude orders and lines that are fulfilled by the marketplace from the response, set ExcludeMarketplaceFulfilledOrdersAndLines to true.
        /// </summary>
        public FulfillmentEnum FulfillmentType { get; set; }

        /// <summary>
        /// Filter on orders containing cancellation requests.
        /// Some channels allow a customer to cancel an order until it has been shipped.
        /// When an order has already been acknowledged in ChannelEngine, it can only be cancelled by creating a cancellation.
        /// </summary>
        public bool? OnlyWithCancellationRequests { get; set; }

        /// <summary>
        /// Filter orders on channel(s).
        /// </summary>
        public List<int> ChannelIds { get; set; }

        /// <summary>
        /// Filter on stock locations
        /// </summary>
        public List<int> StockLocations { get; set; }

        /// Filter on acknowledged value
        public bool? IsAcknowledged { get; set; }

        // Filter on the order update date, starting from this date. This date is inclusive.
        public DateTime? FromUpdatedAtDate { get; set; }

        // Filter on the order update date, unitl from this date. This date is exclusive.
        public DateTime? ToUpdatedAtDate { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ChannelEngine.Assessment.Domain.Marketing.Models;
using ChannelEngine.Assessment.Infrastructure.Integrations.ChannelEngine;
using ChannelEngine.Assessment.Infrastructure.Integrations.ChannelEngine.Messages;

namespace ChannelEngine.Assessment.Acl.ChannelEngine
{
    public class OrderAdapter : IOrderAdapter
    {
        private readonly IChannelEngineClient _channelEngineClient;

        public OrderAdapter(IChannelEngineClient channelEngineClient)
        {
            _channelEngineClient = channelEngineClient;
        }

        public async Task<List<Order>> GetOrdersByStatusesAsync(OrderStatus[] orderStatuses, CancellationToken cancellationToken = default)
        {
            var request = new GetOrdersByFilterRequest
            {
                Statuses = orderStatuses.Select(x => Map(x)).ToList()
            };

            var response = await _channelEngineClient.GetOrdersByFilterAsync(request, cancellationToken);

            return response.Content.Select(x => Map(x)).ToList();
        }

        #region Private methods
        private string Map(OrderStatus orderStatus)
        {
            switch (orderStatus)
            {
                case OrderStatus.Pending:
                    return Enum.GetName(typeof(OrderStatusEnum), OrderStatusEnum.AWAITING_PAYMENT);
                case OrderStatus.Returned:
                    return Enum.GetName(typeof(OrderStatusEnum), OrderStatusEnum.RETURNED);
                case OrderStatus.Shipped:
                    return Enum.GetName(typeof(OrderStatusEnum), OrderStatusEnum.SHIPPED);
                case OrderStatus.InProgress:
                    return Enum.GetName(typeof(OrderStatusEnum), OrderStatusEnum.IN_PROGRESS);

                case OrderStatus.None:
                default:
                    throw new ArgumentOutOfRangeException(nameof(orderStatus));
            }
        }

        private OrderStatus Map(string orderStatus)
        {
            switch (orderStatus)
            {
                case "AWAITING_PAYMENT":
                    return OrderStatus.Pending;

                case "RETURNED":
                    return OrderStatus.Returned;

                case "SHIPPED":
                    return OrderStatus.Shipped;

                default:
                    return OrderStatus.None;
            }
        }

        private Order Map(OrderDto order)
        {
            return new Order
            {
                Id = order.Id,
                Lines = order.Lines.Select(x => Map(x)).ToList(),
                Status = Map(order.Status)
            };
        }

        private OrderLine Map(LineDto line)
        {
            return new OrderLine { 
                ProductId = line.MerchantProductNo,
                Quantity = line.Quantity,
                Gtin = line.Gtin
            };
        }
        #endregion
    }
}

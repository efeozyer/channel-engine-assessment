using ChannelEngine.Assessment.Domain.Marketing.Models;
using ChannelEngine.Assessment.Domain.Marketing.Services;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace ChannelEngine.Assessment.Tests
{
    public class MarketingService_Unit_Tests
    {
        private readonly List<Order> _mockOrders;
        private readonly MarketingService _marketingService;

        public MarketingService_Unit_Tests()
        {
            _mockOrders = new List<Order>();
            _marketingService = new MarketingService();
        }


        [Fact]
        public void GetBestSellerProducts_Should_Return_ExpectedResult()
        {
            // Arrnage
            _mockOrders.Add(new Order
            {
                Id = 1,
                Lines = new List<OrderLine>()
                {
                    new OrderLine
                    {
                        Gtin = "1234",
                        ProductId = "1",
                        Quantity = 1
                    }
                }
            });

            _mockOrders.Add(new Order
            {
                Id = 2,
                Lines = new List<OrderLine>()
                {
                    new OrderLine
                    {
                        Gtin = "1234",
                        ProductId = "1",
                        Quantity = 3
                    }
                }
            });

            _mockOrders.Add(new Order
            {
                Id = 3,
                Lines = new List<OrderLine>()
                {
                    new OrderLine
                    {
                        Gtin = "1234",
                        ProductId = "2",
                        Quantity = 5
                    }
                }
            });

            // Act
            var result = _marketingService.GetBestSellerProducts(_mockOrders, 5);

            // Assert
            result[0].ProductId.Should().Be("2");
            result[0].Quantity.Should().Be(5);
            result.Should().OnlyHaveUniqueItems(x => x.ProductId);
            result.Count.Should().BeLessThanOrEqualTo(5);
        }
    }
}

using System;
using System.Collections.Generic;

namespace ChannelEngine.Assessment.Acl.ChannelEngine
{
    public class ChannelEngineAdapterProvider : IAdapterProvider
    {
        private readonly List<KeyValuePair<Type, Type>> adapters = new List<KeyValuePair<Type, Type>>()
        {
            { new KeyValuePair<Type, Type>(typeof(IOrderAdapter), typeof(OrderAdapter)) },
            { new KeyValuePair<Type, Type>(typeof(IProductAdapter), typeof(ProductAdapter)) }
        };

        public List<KeyValuePair<Type, Type>> GetAll()
        {
            return adapters;
        }
    }
}

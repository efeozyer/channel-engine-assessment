using System;
using System.Collections.Generic;

namespace ChannelEngine.Assessment.Acl
{
    public interface IAdapterProvider
    {
        List<KeyValuePair<Type, Type>> GetAll();
    }
}

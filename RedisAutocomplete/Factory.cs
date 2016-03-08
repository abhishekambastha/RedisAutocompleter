using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisAutocomplete
{
    internal class Factory
    {

        public static IRedisAutocompleteManager<T> getAutocompleter<T>() {
            return new RedisAutocompleteManager<T>();
        }

        public static IRedisAutocompleteManager<T> getAutocompleter<T>(string setName)
        {
            return new RedisAutocompleteManager<T>(setName);
        }
    }
}

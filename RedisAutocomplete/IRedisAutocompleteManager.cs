using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisAutocomplete
{
    interface IRedisAutocompleteManager<T>
    {
        Task SetValuesAsync(List<string> input);
        void SetValues(List<string> input);
        Task<List<string>> GetValuesAsync(string key); 
        Task<List<string>> GetValuesAsync(string key,int limit);
        List<string> GetValues(string key);
        List<string> GetValues(string key,int limit);
    }
}

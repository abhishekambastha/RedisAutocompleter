using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisAutocomplete
{
    interface IRedisAutocompleter<T>
    {
        Task SetValuesAsync(IEnumerable<string> input);
        void SetValues(IEnumerable<string> input);
        Task<IEnumerable<string>> GetValuesAsync(string key); 
        Task<IEnumerable<string>> GetValuesAsync(string key,int limit);
        IEnumerable<string> GetValues(string key); 
        IEnumerable<string> GetValues(string key,int limit);
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RedisAutocomplete;

namespace RedisAutocomplete
{
    public class RedisAutoComplete<T> 
    {
        private IRedisAutocompleteManager<T> _manager;

        public RedisAutoComplete()
        {
            _manager = Factory.getAutocompleter<T>();
        }

        public RedisAutoComplete(string redisConnectionString)
        {
            _manager = Factory.getAutocompleter<T>(redisConnectionString);
        }

        public List<string> GetValues(string key)
        {
            return _manager.GetValues(key);
        }

        public List<string> GetValues(string key, int limit)
        {
            return _manager.GetValues(key,limit);
        }

        public async Task<List<string>> GetValuesAsync(string key)
        {
            return await _manager.GetValuesAsync(key);
        }

        public async Task<List<string>> GetValuesAsync(string key, int limit)
        {
            return await _manager.GetValuesAsync(key,limit);
        }

        public void SetValues(List<string> input)
        {
             _manager.SetValues(input);
        }

        public async Task SetValuesAsync(List<string> input)
        {
            await _manager.SetValuesAsync(input);
        }
    }
}

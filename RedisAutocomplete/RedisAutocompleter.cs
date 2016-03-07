using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StackExchange.Redis;
using Newtonsoft.Json;
using System.Linq;

namespace RedisAutocomplete
{
    public class RedisAutocompleter<T> : IRedisAutocompleter<T>
    {
        private static ConnectionMultiplexer _redis;
        private IDatabase _db;

        private string _setName;

        public string RedisSetName
        {
            get { return _setName; }
            set { _setName = value; }
        }

        #region ctor

        public RedisAutocompleter()
        {
            if (_redis == null)
            {
                _redis = ConnectionMultiplexer.Connect("localhost:6379");
            }
            _db = _redis.GetDatabase();
            RedisSetName = "MainAutoComplete";
        }

        public RedisAutocompleter(string redisSetName) : this()
        {
            RedisSetName = redisSetName;
        }

        #endregion ctor

        #region Interface Methods

        #region Set Methods

        public void SetValues(IEnumerable<string> input)
        {
            foreach (var item in input)
            {
                InsertValue(item, default(T)).Wait();
            }
        }

        public async Task SetValuesAsync(IEnumerable<string> input)
        {
            foreach (var item in input)
            {
                await InsertValue(item, default(T));
            }
        }

        #endregion Set Methods

        #region Get Methods

        public IEnumerable<string> GetValues(string key)
        {
            var getTask = GetValuesAsync(key);
            getTask.Wait();
            return getTask.Result;
        }

        public IEnumerable<string> GetValues(string key, int limit)
        {
            var getTask = GetValuesAsync(key, limit);
            getTask.Wait();
            return getTask.Result;
        }

        public async Task<IEnumerable<string>> GetValuesAsync(string prefix)
        {
            prefix = prefix.ToLower();


            var autoCompleteResult = new List<string>();

            var start = await GetRank(prefix);
            var end = await GetRank(GetNextString(prefix));
            if (start == 0)
                return new List<string>();
            end = end > 0 ? end : -1;
            var result = await GetRange(start, end);
            var filteredResult = result.Where(x => x.EndsWith("*#*"));
            foreach (var item in filteredResult)
            {
                var autores = item.Split(new string[] { "*#*" }, StringSplitOptions.RemoveEmptyEntries);
                if (autores != null && autores.Count() == 2)
                {
                    var criteria = JsonConvert.DeserializeObject<T>(autores[1]);
                    autoCompleteResult.Add(autores[0]);
                    //autoCompleteResult.Add(new AutoCompleteResult()
                    //{
                    //    value = autores[0],
                    //    criteria = criteria
                    //});
                }
            }

            return autoCompleteResult;
        }

        public async Task<IEnumerable<string>> GetValuesAsync(string key, int limit)
        {
            var result = await GetValuesAsync(key);

            return result.Take(limit);
        }

        #endregion Get Methods

        #endregion Interface Methods

        #region private Methods

        private async Task InsertValue(string key, T value)
        {
            key = key.ToLower();
            var stringvalue = value == null ? key : JsonConvert.SerializeObject(value);
            var keyvalue = "";
            for (int i = 0; i < key.Length; i++)
            {
                keyvalue += key[i];
                var nextkey = GetNextString(keyvalue);
                if (i == key.Length - 1)
                {
                    keyvalue += "*#*" + stringvalue + "*#*";
                }
                await InsertToSortedSet(keyvalue);
                await InsertToSortedSet(nextkey);
            }
        }

        private async Task InsertToSortedSet(string keyvalue)
        {
            var entry = new SortedSetEntry(keyvalue, 0.0);
            var entryArray = new SortedSetEntry[] { entry };
            await _db.SortedSetAddAsync(_setName, entryArray);
        }

        

        private string GetNextString(string input)
        {
            var lastchar = input[input.Length - 1];
            var op = input.TrimEnd(lastchar);
            ++lastchar;
            return op + lastchar;
        }

        private async Task<long> GetRank(string prefix)
        {
            var start = await _db.SortedSetRankAsync(_setName, prefix);
            return start ?? 0;
        }

        private async Task<string[]> GetRange(long start, long end)
        {
            var result = await _db.SortedSetRangeByRankAsync(_setName, start, end);
            var arrayresult = from r in result select r.HasValue ? r.ToString() : "";
            return arrayresult.ToArray();
        }

        #endregion private Methods
    }
}

# RedisAutocompleter
Autocomplete with Redis , C#  
Implementation of Autocomplete as described in blog  post [by creater of Redis]  in  in C#  
Implemented by using ZRANK and ZRANGE on sorted sets . Complexity is O(log(N)) .More details can be found on the blog post.
### Installation

	Install-Package RedisAutocomplete  
	
#### Config settings	
     <connectionStrings>  
        <add name="DefaultRedisAutocomplete" connectionString="localhost:6379"/>  
        <add name="AlternateRedisAutocomplete" connectionString="localhost:6378"/>  
      </connectionStrings>
  
### Usage
	       var autocompleter = new RedisAutoComplete<string>();/*var autocompleter = new RedisAutoComplete<string>("AlternateRedisAutocomplete"); */
            var input = new List<string>();
            input.Add("foo");
            input.Add("foobar");
            input.Add("bar");
            input.Add("icecream");
            autocompleter.SetValues(input);
            var result = autocompleter.GetValues("foo");
            Assert.AreEqual(result.Count, 2);
            Assert.AreEqual(result[0], "foo");
            Assert.AreEqual(result[1], "foobar");
  
 
 [by creater of Redis]: <http://oldblog.antirez.com/post/autocomplete-with-redis.html>

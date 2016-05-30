using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Configuration;

namespace RedisAutocomplete.Tests
{

    //Scenario 1 validate connection . if no redis then throw Exception else continue
    //Insert values
    //get autocomplete with values
    //get autocomplete with values delimited results
    //set name default override with values in config
    //should use asyn and sync as well
    // case sensitive tests
    [TestClass]
    public class RedisAutocompleteTests
    {
        [TestMethod]
        public void InsertValuesAndQuery()
        {
            var autocompleter = new RedisAutoComplete<string>(); /*var autocompleter = new RedisAutoComplete<string>(ConfigurationManager.ConnectionStrings["AlternateRedisAutocomplete"].ConnectionString);*/
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
        }


        [TestMethod]
        public void InsertValuesAndQueryWithLimitedResults()
        {
            var autocompleter = new RedisAutoComplete<string>();
            var input = new List<string>();
            input.Add("foo");
            input.Add("foobar");
            input.Add("bar");
            input.Add("icecream");
            autocompleter.SetValues(input);
            var result = autocompleter.GetValues("foo",1);
            Assert.AreEqual(result.Count, 1);
            Assert.AreEqual(result[0], "foo");
        }

        [TestMethod]
        public void InsertValuesAndQueryDiffConnectionString()
        {
            var autocompleter = new RedisAutoComplete<string>(ConfigurationManager.ConnectionStrings["AlternateRedisAutocomplete"].ConnectionString);
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
        }

        [TestMethod]
        public void InsertValuesAndQueryWrongConnectionString()
        {
            try
            {
                var autocompleter = new RedisAutoComplete<string>(ConfigurationManager.ConnectionStrings["WrongConnectionString"].ConnectionString);
                Assert.Fail("Expected Exception not thrown");
            }
            catch (Exception ex)
            {

                Assert.AreEqual(ex.Message.Contains("Failed to connect to Redis Database.Please check your connection string. Default ConnectionString is 'DefaultRedisAutocomplete'"), true);
            }
        }
    }
}

using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwitterAPIApplication;
using TwitterAPIApplication.Controllers;
using TwitterAPIApplication.Proxylayer;
using System.Collections.Generic;
using TwitterAPIApplication.Models;
using TwitterAPIApplication.RestAPIClient;
using System.Net.Http;
using System;
using System.Net;
using Newtonsoft.Json;

namespace TwitterAPIApplication.Tests.RestAPIClient
{
    [TestClass]
    public class RestClientTest
    {
        //This method is to test the sucess scenario
        [TestMethod]
        public void SendRequest_Success()
        {
            // Arrange
            RestClient instance = new RestClient();
            string url = "1.1/statuses/user_timeline.json";
            string querystring = "?count=10&screen_name=salesforce&tweet_mode=extended";
            url = url + querystring;

            APIResponse response = instance.SendRequest(HttpMethod.Get, url).Result;
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            List<TwitterAPIResponse> twiiterResponse = JsonConvert.DeserializeObject<List<TwitterAPIResponse>>(response.Content);
            // Assert
            Assert.AreEqual(10, twiiterResponse.Count);
        }


        [TestMethod]
        public void SendRequest_InvalidUrl()
        {

            RestClient instance = new RestClient();
            string url = "/statuses/user_timeline.json";
            string querystring = "?count=10&screen_name=salesforce&tweet_mode=extended";
            url = url + querystring;

            APIResponse response = instance.SendRequest(HttpMethod.Get, url).Result;

            Assert.AreNotEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }


        [TestMethod]
        public void SendRequest_NoTweets()
        {
            RestClient instance = new RestClient();
            string url = "1.1/statuses/user_timeline.json";
            string querystring = "?count=10&screen_name=salesfce&tweet_mode=extended";
            url = url + querystring;

            APIResponse response = instance.SendRequest(HttpMethod.Get, url).Result;
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
            // Assert
        }

    }
}

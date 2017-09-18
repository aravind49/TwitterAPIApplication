using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwitterAPIApplication.Proxylayer;
using System.Collections.Generic;
using TwitterAPIApplication.Models;

namespace TwitterAPIApplication.Tests.ProxyLayer
{
    [TestClass]
    public class TwitterAPIProxyTest
    {
        //This method is to test the sucess scenario
        [TestMethod]
        public void Get_Success()
        {
            // Arrange
            TwitterAPIProxy controller = new TwitterAPIProxy();

            Dictionary<Enum_SearchCriteria, string> search = new Dictionary<Enum_SearchCriteria, string>();
            search.Add(Enum_SearchCriteria.screen_name, "salesforce");
            search.Add(Enum_SearchCriteria.count, "10");
            TwitterViewModel viewModel = controller.Get(search).Result;

            // Assert
            Assert.IsTrue(viewModel.IsSuccess);
            Assert.AreEqual(10, viewModel.Tweets.Count);
        }


        [TestMethod]
        public void Get_InvalidSearchCriteria()
        {
            // Arrange
            TwitterAPIProxy controller = new TwitterAPIProxy();

            Dictionary<Enum_SearchCriteria, string> search = new Dictionary<Enum_SearchCriteria, string>();
            search.Add(Enum_SearchCriteria.text, "salesforce");
            search.Add(Enum_SearchCriteria.count, "10");
            TwitterViewModel viewModel = controller.Get(search).Result;

            // Assert
            Assert.IsFalse(viewModel.IsSuccess);
            Assert.IsTrue(viewModel.ErrorDetails.Description.ToUpper().Contains("AUTHORIZATION REQUIRED"));
        }


        [TestMethod]
        public void Get_NoTweets()
        {
            // Arrange
            TwitterAPIProxy controller = new TwitterAPIProxy();

            Dictionary<Enum_SearchCriteria, string> search = new Dictionary<Enum_SearchCriteria, string>();
            search.Add(Enum_SearchCriteria.screen_name, "lesrce");
            search.Add(Enum_SearchCriteria.count, "10");
            TwitterViewModel viewModel = controller.Get(search).Result;

            // Assert
            Assert.IsTrue(viewModel.IsSuccess);
            Assert.AreEqual(0, viewModel.Tweets.Count);
        }



    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwitterAPIApplication.Models
{
    //this model is used to display the items on the page
    public class TwitterViewModel :BaseViewModel
    {
        public TwitterViewModel()
        {
            
            Tweets = new List<Tweet>();
        }
        public List<Tweet> Tweets { set; get; }
    }
    public class Tweet
    {
        public string UserName { set; get; }
        public string UserScreenName { set; get; }
        public string UserProfileImage { set; get; }
        public string TweetContent  { set; get; }
        public int CountOfRetweet { set; get; }
        public string TweetDate  { set; get; }
    }
}
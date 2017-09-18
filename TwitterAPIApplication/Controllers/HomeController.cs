using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using TwitterAPIApplication.Models;
using TwitterAPIApplication.Proxylayer;

namespace TwitterAPIApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly string str_userName = "username";
        private readonly string str_tweetCount = "tweetcount";

        private readonly string userName;
        private readonly int tweetCount;

        TwitterAPIProxy twitterAPIProxy;
        public HomeController()
        {
            userName = ConfigurationManager.AppSettings[str_userName];
            tweetCount = Convert.ToInt32(ConfigurationManager.AppSettings[str_tweetCount]);
            twitterAPIProxy = new TwitterAPIProxy();
        }

        //this is loading page.
        public async Task<ActionResult> Index()
        {
            return View();
        }

        //to populate the Twitter view
        public async Task<ActionResult> Twitter(Dictionary<Enum_SearchCriteria, string> searchCriteria = null)
        {
            TwitterViewModel viewModel = new TwitterViewModel();
            
            try
            {
                //since the requirement has the search criteria as username and tweetcount,building the serach criteria with those values.

                Dictionary<Enum_SearchCriteria, string> search = new Dictionary<Enum_SearchCriteria, string>();
                search.Add(Enum_SearchCriteria.screen_name, userName);
                search.Add(Enum_SearchCriteria.count, tweetCount.ToString());

                viewModel = await twitterAPIProxy.Get(search);                
            }
            catch (Exception e)
            {
                viewModel.IsSuccess = false;
                //Log the error to DB or Flatfile
            }
            return PartialView("Twitter", viewModel);
        }

    }
}

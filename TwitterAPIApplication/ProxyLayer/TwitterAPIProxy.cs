using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Newtonsoft.Json.Linq;
using System.Net;
using Newtonsoft.Json;
using TwitterAPIApplication.RestAPIClient;
using TwitterAPIApplication.Models;
using System.Globalization;

namespace TwitterAPIApplication.Proxylayer
{
    public class TwitterAPIProxy
    {
        //Consumer key and secret are constants and should never be set to a different value
        private static readonly string TimelineUrl;

        private const string str_TimelineUrl = "TimeLineUrl";

        const string format = "ddd MMM dd HH:mm:ss zzzz yyyy";

        RestClient restClient;

        static TwitterAPIProxy()
        {
            TimelineUrl = ConfigurationManager.AppSettings[str_TimelineUrl];
        }

        public TwitterAPIProxy()
        {
            restClient = new RestClient();
        }


        /// <summary>
        /// This method returns the tweets of a user
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="numofTweets"></param>
        public async Task<TwitterViewModel> Get(Dictionary<Enum_SearchCriteria,string> serachCriteria)
        {
            TwitterViewModel viewModel = new TwitterViewModel();
            try
            {
                string url = TimelineUrl+buildUrl(serachCriteria);                   

                Task<APIResponse> getTweets = restClient.SendRequest(HttpMethod.Get, url);

                viewModel = await populateTwitterViewModel(getTweets);
            }
            catch (Exception e)
            {
                viewModel.IsSuccess = false;
                viewModel.ErrorDetails = new Error() { Description = e.Message, StackTrace = e.StackTrace };
            }
            return viewModel;
        }

        /// <summary>
        /// Not Implemented
        /// </summary>
        /// <returns></returns>
        public TwitterViewModel Post()
        {
            TwitterViewModel res = new TwitterViewModel();
            res.ErrorDetails = new Error() { Description = "this method is not implemented" };
            return res;
        }

        public async Task<TwitterViewModel> Delete()
        {
            throw new NotImplementedException();
        }

        public async Task<TwitterViewModel> Like()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// to loas the twitter view model
        /// </summary>
        /// <param name="taskGetTweets"></param>
        /// <returns></returns>
        private async Task<TwitterViewModel> populateTwitterViewModel(Task<APIResponse> taskGetTweets)
        {
            try
            {
                TwitterViewModel tvm = new TwitterViewModel();

                APIResponse apiResponse = await taskGetTweets;
                if (apiResponse.StatusCode == HttpStatusCode.OK)
                {
                    List<TwitterAPIResponse> twitterResponse = (JsonConvert.DeserializeObject<List<TwitterAPIResponse>>(apiResponse.Content));
                    
                    foreach (var res in twitterResponse)
                    {
                        Tweet tweet = new Tweet();
                        tweet.UserName = res.user.name;
                        tweet.UserScreenName = res.user.screen_name;
                        tweet.CountOfRetweet = res.retweet_count;

                        tweet.TweetDate = string.Format("{0:dd MMM yyyy}", DateTime.ParseExact(res.created_at, format, CultureInfo.InvariantCulture));
                        List<string> images = new List<string>();
                        List<string> links = new List<string>();
                        res.entities?.media?.ForEach(x => images.Add(x.media_url));
                        //res.
                        tweet.TweetContent = res.full_text;
                        if (res.entities?.urls != null)
                        {
                            foreach (var url in res.entities?.urls)
                            {
                                tweet.TweetContent = tweet.TweetContent.Replace(url.url, "<a href=\"" + url.expanded_url + "\">" + url.display_url + "</a>");
                            }
                        }
                        if (res.entities?.media != null)
                        {
                            foreach (var media in res.entities?.media)
                            {
                                tweet.TweetContent = tweet.TweetContent.Replace(media.url, "<br /><img src=\"" + media.media_url_https + "\"/><br />");
                            }
                        }
                        tweet.UserProfileImage = res.user.profile_image_url_https;
                        tvm.Tweets.Add(tweet);
                    }
                    tvm.IsSuccess = true;
                }
                else
                {
                    tvm.IsSuccess = false;
                    tvm.ErrorDetails.Description = apiResponse.Reason;
                    //Log the error to DB ot flat file
                }
                return tvm;
            }
            catch (Exception e)
            {
                throw;
            }

        }
        
        private string buildUrl(Dictionary<Enum_SearchCriteria, string> serachCriteria)
        {
            string url = string.Empty;
            try
            {
                //"?count={0}&screen_name={1}&tweet_mode=extended", numofTweets, userName
                url = "?";
                foreach (System.Collections.Generic.KeyValuePair<Enum_SearchCriteria,string> kvp in serachCriteria)
                {
                    switch (kvp.Key)
                    {
                        case Enum_SearchCriteria.count:
                            url = url + "count="+ kvp.Value + "&";
                            break;
                        case Enum_SearchCriteria.screen_name:
                            url = url + "screen_name=" + kvp.Value + "&";
                            break;
                            //this is just an example
                        case Enum_SearchCriteria.retweet:
                            url = url + "retweet=" + kvp.Value + "&";
                            break;
                        case Enum_SearchCriteria.text:
                            url = url + "text=" + kvp.Value + "&";
                            break;
                        default:
                            break;
                    }
                    //to avoid the truncate of the tweets
                    
                }
                url += "tweet_mode=extended";

            }
            catch(Exception e)
            {
                throw;
            }
            return url;
        }
    }
}

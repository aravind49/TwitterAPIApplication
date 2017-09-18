using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TwitterAPIApplication.Models;

namespace TwitterAPIApplication.RestAPIClient
{
    public class RestClient
    {
        private const string str_ConsumerKey = "ConsumerKey";
        private const string str_ConsumerSecret = "ConsumerSecret";
        private const string str_TwitterBaseUrl = "TwitterBaseUrl";
        private const string str_AuthUrl = "AuthUrl";
        private const string str_Basic = "Basic";
        private const string str_Authorization = "Authorization";
        private const string str_GrantType = "grant_type=client_credentials";


        // private const string getUrl = "GetUrl";

        private static readonly string ConsumerKey;
        private static readonly string ConsumerSecret;

        private static readonly string BaseUrl;
        private static readonly string AuthUrl;
        //private static readonly string GetUrl;

        //Token which is create for the firsttime can be used in all the subsequent calls.If the restapi responds with an invalid token message new token should be created.
        public static string AccessToken { set; get; }

        static RestClient()
        {
            ConsumerKey = ConfigurationManager.AppSettings[str_ConsumerKey];
            ConsumerSecret = ConfigurationManager.AppSettings[str_ConsumerSecret];
            BaseUrl = ConfigurationManager.AppSettings[str_TwitterBaseUrl];
            AuthUrl = ConfigurationManager.AppSettings[str_AuthUrl];
        }
        /// <summary>
        /// this method calls the api method converts the response object to required class object.Here
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<APIResponse> SendRequest(HttpMethod method, string relativeUrl, object content = null)
        {
            APIResponse response;
            try
            {
                using (HttpRequestMessage request = await buildRequest(method, relativeUrl, content))
                {
                    response = await SendRequest(request);
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return response;
        }



        /// <summary>
        /// this method creates a request foreach rest call
        /// </summary>  
        /// <param name="method"></param>
        /// <param name="content">to pass the any object as body in the request</param>
        /// <param name="relativeUrl"></param>
        /// <returns></returns>
        private async Task<HttpRequestMessage> buildRequest(HttpMethod method, string relativeUrl, object content = null)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.Method = method;

            //To call the api, accesstoken is needed. So if there is no access token, call the getaccesstoken method to get it.
            if (string.IsNullOrWhiteSpace(AccessToken) && relativeUrl != AuthUrl)
            {
                AccessToken = await getAccessToken();
            }
            request.RequestUri = new Uri(BaseUrl + relativeUrl);

            string authorizationHeader = "Bearer " + AccessToken;

            HttpContent httpContent = null;

            if (method == HttpMethod.Post || method == HttpMethod.Put)
            {
                httpContent = content == null ? httpContent : new StringContent(JsonConvert.SerializeObject(content));
            }

            request.Headers.Add("Authorization", authorizationHeader);
            request.Content = httpContent;

            return request;
        }

        /// <summary>
        ///// this method creates a request foreach rest call
        ///// </summary>
        ///// <param name="method"></param>
        ///// <param name="content">to pass the any object as body in the request</param>
        ///// <param name="relativeUrl"></param>
        ///// <returns></returns>
        //private async Task<HttpRequestMessage> buildRequest(HttpMethod method, string relativeUrl, object content = null)
        //{
        //    HttpRequestMessage request = new HttpRequestMessage();
        //    request.Method = method;

        //    //To call the api, accesstoken is needed. So if there is no access token, call the getaccesstoken method to get it.
        //    if (string.IsNullOrWhiteSpace(AccessToken) && relativeUrl != AuthUrl)
        //    {
        //        AccessToken = await getAccessToken();
        //    }
        //    request.RequestUri = new Uri(BaseUrl + relativeUrl);

        //    string authorizationHeader = (string.IsNullOrWhiteSpace(AccessToken)) ? "Basic " + Convert.ToBase64String(new UTF8Encoding().GetBytes(ConsumerKey + ":" + ConsumerSecret)) : "Bearer " + AccessToken;

        //    HttpContent httpContent = null;

        //    if (method == HttpMethod.Post)
        //    {
        //        if (string.IsNullOrWhiteSpace(AccessToken))
        //        {
        //            httpContent = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");
        //        }
        //        else
        //        {
        //            //content object should be created.This application does not need this. and 
        //        }
        //    }

        //    request.Headers.Add("Authorization", authorizationHeader);
        //    request.Content = httpContent;

        //    return request;
        //}

        /// <summary>
        /// this method generates the access token
        /// </summary>
        /// <returns></returns>
        private async Task<string> getAccessToken()
        {
            string token = string.Empty;
            try
            {
                using (HttpRequestMessage request = await buildRequestToGenerateAccessToken())
                {
                    APIResponse response = await SendRequest(request);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        token = (JsonConvert.DeserializeObject<AccessToken>(response.Content)).access_token;
                    }
                    else
                    {
                        //Log the error to db or flat file.
                        throw new Exception(response.StatusCode.ToString() + response.Reason);
                    }
                }
                return token;
            }
            catch (Exception e)
            {
                //throw: stacktrace will be sent to the parent call. 
                //throw ex :does not carry the stack trace to parent class.
                throw;
            }


        }
        /// <summary>
        /// This method builds the request to generate the access token. This can be merged in the buildrequest method.but buildrequest method is becoming complex.
        /// </summary>
        /// <returns></returns>
        private async Task<HttpRequestMessage> buildRequestToGenerateAccessToken()
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.Method = HttpMethod.Post;
            request.RequestUri = new Uri(BaseUrl + AuthUrl);
            string authorizationHeader = Models.Enum_Authorization.Basic + " " + Convert.ToBase64String(new UTF8Encoding().GetBytes(ConsumerKey + ":" + ConsumerSecret));
            request.Headers.Add(str_Authorization, authorizationHeader);
            request.Content = new StringContent(str_GrantType, Encoding.UTF8, "application/x-www-form-urlencoded");
            return request;
        }

        /// <summary>
        /// this method call the api to get the response
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<APIResponse> SendRequest(HttpRequestMessage request)
        {
            APIResponse response = new APIResponse();
            try
            {
                using (var httpClient = new HttpClient())
                {
                    HttpResponseMessage httpResponse = await httpClient.SendAsync(request);
                    string httpcontent = await httpResponse.Content.ReadAsStringAsync();
                    response.StatusCode = httpResponse.StatusCode;
                    if (httpResponse.IsSuccessStatusCode)
                    {                        
                        response.Content = httpcontent;
                    }
                    else
                    {
                        response.Reason = httpResponse.ReasonPhrase;
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }


            return response;
        }
    }
    
}
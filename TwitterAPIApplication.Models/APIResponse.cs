using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TwitterAPIApplication.Models
{
        public class APIResponse
        {
            public HttpStatusCode StatusCode { set; get; }
            public string Reason { set; get; }
            public string Description { set; get; }

            public string Content { set; get; }

        }
   
}

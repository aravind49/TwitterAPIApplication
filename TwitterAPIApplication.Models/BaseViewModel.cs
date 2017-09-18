using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwitterAPIApplication.Models
{
    public class BaseViewModel
    {

        public BaseViewModel()
        {
            ErrorDetails = new Error();
        }
        public bool IsSuccess { set; get; }        
        public Error ErrorDetails { set; get; }
    }

    public class Error
    {
        public string ErrorCd { set; get; }
        public string Description { set; get; }
        public string StackTrace { set; get; }
    }
}
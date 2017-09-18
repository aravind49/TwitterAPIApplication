using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace TwitterAPIApplication.Models
{
    public enum Enum_Authorization
    {
        Basic,
        Bearer
    }
    public enum Enum_MediaType
    {
        [Description("application/x-www-form-urlencoded")]
        application_x_www_form_urlencoded,
        [Description("application/json")]
        application_json
    }

    public enum Enum_SearchCriteria
    {
        //only added few for testing purpose
        screen_name,
        count,
        retweet,
        text,

    }
}
using Steganografia.Security.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Steganografia
{
    public class FilterConfig
    {
        public static void RegisterFilters(GlobalFilterCollection filters)
        {
            filters.Add(new CustomAuthenticationFilter());
            filters.Add(new CustomAuthorizationFilter());
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Steganografia
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterFilters(GlobalFilters.Filters);
        }

		void Application_Error(object sender, EventArgs e)
		{
			Exception ex = Server.GetLastError();

			if (ex is HttpRequestValidationException)
			{
				Response.Clear();
				Response.StatusCode = 200;
				Response.Write(@"
<html><head><title>HTML Not Allowed</title>
<script language='JavaScript'><!--
function back() { history.go(-1); } //--></script></head>
<body style='font-family: Arial, Sans-serif;'>
<h1>Oops!</h1>
<p>I'm sorry, but HTML entry is not allowed on that page.</p>
<p>Please make sure that your entries do not contain 
any angle brackets like &lt; or &gt;.</p>
<p><a href='javascript:back()'>Go back</a></p>
</body></html>
");
				Response.End();
			}
		}
	}
}

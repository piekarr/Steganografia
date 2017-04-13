using Steganografia.Security.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Steganografia.Controllers
{
    public abstract class BaseController : Controller
    {
        protected new AppPrincipal User => base.User as AppPrincipal;
    }
}
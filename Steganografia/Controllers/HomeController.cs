﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Steganografia.Controllers
{
    public class HomeController : BaseController
    {

        public ActionResult Index()
        {
            var asd = User.Identity.Id;
            return View();
        }
    }
}
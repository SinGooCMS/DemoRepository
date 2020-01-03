using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using WebApp.Code;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        public IPersonRepository Repository { get; set; }

        // GET: Default
        public ActionResult Index()
        {
            var data = Repository.GetAll();
            ViewBag.Users = data;

            return View();
        }
    }
}
using MapHacks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MapHacks.Controllers
{
	public class HomeController : Controller
	{
		//
		// GET: /Home/
		public ActionResult Index()
		{
			var viewModel = new AppViewModel
			{
				UserDetails = Session["UserDetails"] as UserDetailsViewModel
			};

			return View(viewModel);
		}
	}
}
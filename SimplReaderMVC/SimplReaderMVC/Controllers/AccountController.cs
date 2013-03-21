using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimplReaderMVC.Models.Account;

namespace SimplReaderMVC.Controllers {
	public class AccountController : Controller {
		//
		// GET: /Account/

		public ActionResult LogOn() {
			return PartialView();
		}

		[HttpPost]
		public ActionResult LogOn(LogOnVM model) {
			return Content("OK");
		}

	}
}

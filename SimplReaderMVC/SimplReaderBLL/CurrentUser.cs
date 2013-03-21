using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using SimplReaderBLL.BLL.Concrete;
using SimplReaderBLL.BLL.Membership;
using SimplReaderBLL.Enumerators;

namespace SimplReaderBLL {
	public class CurrentUser {
		/// <summary>
		/// If user is authenticated returns instance of the user with all the data
		/// </summary>
		public static User Instance {
			get {
				if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated == false) return null;

				var data = System.Web.HttpContext.Current.Session["CurrentUser"] as User;
				if (data == null) {
					var kernel = new StandardKernel();
					var accountProvider = kernel.Get<AccountProvider>();
					var user = accountProvider.Get(Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.Name));
					System.Web.HttpContext.Current.Session["CurrentUser"] = user;
					return user;
				}
				data = System.Web.HttpContext.Current.Session["CurrentUser"] as User;
				return data;
			}
		}


		public static int UserID {
			get {
				if (Instance == null) throw new Exception("No logged in user");
				return Instance.UserID;
			}
		}

		/// <summary>
		/// checks if user is authenticated based on current tenant
		/// </summary>
		public static bool IsAuthenticated {
			get {
				if (System.Web.HttpContext.Current.User == null || System.Web.HttpContext.Current.User.Identity.IsAuthenticated == false) return false;
				if (Instance == null) {
					return false;
				}
				//Does a user have any role in this tenant or is a global user
				return true;
			}
		}


		public static bool IsAdmin {
			get {
				return IsAuthenticated && Instance.UserType == (int)UserTypeEnum.Administrator;
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SimplReaderBLL
{
    public class Request
    {
        public static string AppDataFolder
        {
            get { return HttpContext.Current.Server.MapPath("~/AppData"); }
        }
    }
}

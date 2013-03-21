using System;
using System.Net;
using System.Web;

namespace SimplReaderBLL.Helpers {
	public class IPHelper {


		/// <summary>
		/// Returns client ip address
		/// Works if proxy or load balancer is in the middle of request
		/// </summary>
		/// <returns></returns>
		public static string GetClientIP4Address() {

			var request = HttpContext.Current.Request;
			string szRemoteAddr = request.UserHostAddress;
			string szXForwardedFor = request.ServerVariables["X_FORWARDED_FOR"];
			string szIP = "";

            // Localhost check
            //bool isLocal = new HttpApplication().Request.IsLocal;
            if (HttpContext.Current.Request.IsLocal)
            {
                return "127.0.0.1";
            }

            //not forwarded
			if (szXForwardedFor == null) {
                string IP4Address = String.Empty;

                foreach (IPAddress IPA in Dns.GetHostAddresses(HttpContext.Current.Request.UserHostAddress))
                {
                    if (IPA.AddressFamily.ToString() == "InterNetwork")
                    {
                        IP4Address = IPA.ToString();
                        break;
                    }
                }

                if (IP4Address != String.Empty)
                {
                    return IP4Address;
                }

                foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
                {
                    if (IPA.AddressFamily.ToString() == "InterNetwork")
                    {
                        IP4Address = IPA.ToString();
                        break;
                    }
                }

                return IP4Address;

			} else {
				szIP = szXForwardedFor;
				if (szIP.IndexOf(",") > 0) {
					string[] arIPs = szIP.Split(',');

					foreach (string item in arIPs) {
						if (!IsPrivateIpAddress(item)) {
							return item;
						}
					}
				}
			}
			return szIP;
		}

		private static bool IsPrivateIpAddress(string ipAddress) {
			// http://en.wikipedia.org/wiki/Private_network
			// Private IP Addresses are: 
			//  24-bit block: 10.0.0.0 through 10.255.255.255
			//  20-bit block: 172.16.0.0 through 172.31.255.255
			//  16-bit block: 192.168.0.0 through 192.168.255.255
			//  Link-local addresses: 169.254.0.0 through 169.254.255.255 (http://en.wikipedia.org/wiki/Link-local_address)

			var ip = IPAddress.Parse(ipAddress);
			var octets = ip.GetAddressBytes();

			var is24BitBlock = octets[0] == 10;
			if (is24BitBlock) return true; // Return to prevent further processing

			var is20BitBlock = octets[0] == 172 && octets[1] >= 16 && octets[1] <= 31;
			if (is20BitBlock) return true; // Return to prevent further processing

			var is16BitBlock = octets[0] == 192 && octets[1] == 168;
			if (is16BitBlock) return true; // Return to prevent further processing

			var isLinkLocalAddress = octets[0] == 169 && octets[1] == 254;
			return isLinkLocalAddress;
		}

	}


}

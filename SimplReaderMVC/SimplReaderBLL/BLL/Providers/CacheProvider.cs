using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Web;
using CacheItemPriority = System.Web.Caching.CacheItemPriority;

namespace SimplReaderBLL.BLL.Providers {
	public class CacheProvider {
		private static CacheProvider _instance;
		public static CacheProvider Default {
			get { return _instance ?? (_instance = new CacheProvider()); }
		}

		/// <summary>
		/// Gets the object from memory cache
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public object Get(string key) {
			return HttpContext.Current.Cache.Get(key);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <param name="cacheItemPolicy">Default 5 min cache</param>
		/// 
		public void Add(string key, object value, CacheItemPolicy cacheItemPolicy = null) {
			if (value == null)
				throw new ArgumentNullException("value", String.Format("Value cannot be null for key {0}", key));
			if (cacheItemPolicy == null) {
				HttpContext.Current.Cache.Add(key, value, null, System.Web.Caching.Cache.NoAbsoluteExpiration,
														TimeSpan.FromMinutes(5), CacheItemPriority.Normal, null);
			} else {
				double minutes = 5;
				if (cacheItemPolicy.AbsoluteExpiration == DateTimeOffset.MinValue && cacheItemPolicy.SlidingExpiration != TimeSpan.MinValue)
					minutes = cacheItemPolicy.SlidingExpiration.TotalMinutes;

				HttpContext.Current.Cache.Add(key, value, null, System.Web.Caching.Cache.NoAbsoluteExpiration,
														TimeSpan.FromMinutes(minutes), CacheItemPriority.Normal, null);
			}
		}


		public bool Contains(string key) {
			return Get(key) != null;
		}

		public void Remove(string key) {
			HttpContext.Current.Cache.Remove(key);
		}

		public List<string> GetItems() {
			List<string> keys = new List<string>();
			IDictionaryEnumerator enumerator = HttpContext.Current.Cache.GetEnumerator();
			while (enumerator.MoveNext()) {
				if (!String.IsNullOrEmpty(enumerator.Key.ToString())) {
					keys.Add(enumerator.Key.ToString());
				}

			}
			return keys;
		}


		public List<string> Clear() {
			List<string> keys = new List<string>();
			IDictionaryEnumerator enumerator = HttpContext.Current.Cache.GetEnumerator();
			while (enumerator.MoveNext()) {
				if (!String.IsNullOrEmpty(enumerator.Key.ToString())) {
					keys.Add(enumerator.Key.ToString());
					Remove(enumerator.Key.ToString());
				}

			}
			return keys;
		}
	}
}

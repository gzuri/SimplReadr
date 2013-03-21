using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Extensions.Logging;
using SimplReaderBLL.BLL.Concrete;
using SimplReaderBLL.BLL.Providers;
using SimplReaderBLL.Enumerators;

namespace SimplReaderBLL.BLL.Membership {
	public class AccountProvider {
		private readonly Concrete.DbContext coreContext;
		private readonly ILogger logger;

		public AccountProvider(Concrete.DbContext coreContext, ILogger logger) {
			this.coreContext = coreContext;
			this.logger = logger;
		}

		public User Get(int userID, bool cached = true) {
			if (!cached)
				return coreContext.Users.FirstOrDefault(x => x.UserID == userID);

			if (!CacheProvider.Default.Contains("Core_User" + userID)) {
				var dbData = coreContext.Users.FirstOrDefault(x => x.UserID == userID);
				CacheProvider.Default.Add("Core_User" + userID, dbData);
			}
			return CacheProvider.Default.Get("Core_User" + userID) as User;
		}

		/// <summary>
		/// Encrypts system password, very important to be in one place for creating and reading
		/// </summary>
		/// <param name="plainText"></param>
		/// <returns></returns>
		public static string EncryptPassword(string plainText) {
			return System.Web.Helpers.Crypto.SHA1(plainText).ToLowerInvariant();
		}

		/// <summary>
		/// Tries to get global user with provided credentials
		/// </summary>
		/// <param name="email"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		public User GetUserByCredentials(string email, string password) {
			email = email.ToLowerInvariant();
			password = EncryptPassword(password);
			return coreContext.Users.FirstOrDefault(x => x.Email == email && x.Password == password);
		}

		/// <summary>
		/// Creates most basic user in the system
		/// no roles, no keys, nothing just a user
		/// </summary>
		/// <param name="email"></param>
		/// <param name="password"></param>
		/// <returns>created user</returns>
		public User CreateUser(string email, string password) {
			var newUser = new User {
				Username = email,
				Email = email,
				CreatedDate = DateTime.UtcNow,
				IsBlocked = false,
				IsActivated = false,
				Password = EncryptPassword(password)
			};
			coreContext.Users.Add(newUser);
			coreContext.SaveChanges();
			logger.Info("Created user <id={0}>", newUser.UserID);
			return newUser;
		}

		/// <summary>
		/// Activates system user
		/// </summary>
		/// <param name="userID"></param>
		/// <returns> true if successfull, false if user is not found </returns>
		public bool ActivateUser(int userID) {
			User user = coreContext.Users.Single(x => x.UserID == userID);
			if (user == null)
				return false;
			user.IsActivated = true;
			coreContext.SaveChanges();
			logger.Info("User activated <id={0})>", userID);
			return true;
		}

		#region user keys
		public Guid GenerateLostPasswordUserKey(int userID) {
			return GenerateUserKey(userID, UserKeyTypeEnum.LostPassword).Key;
		}

		/// <summary>
		/// Gets user from userKey 
		/// </summary>
		/// <param name="userKey"></param>
		/// <param name="immediateDeleteKey"> </param>
		/// <returns></returns>
		public int? GetUserFromKey(string userKey, bool immediateDeleteKey = false) {
			Guid gKey;
			if (!Guid.TryParse(userKey, out gKey)) return null;

			UserKey dbKey = coreContext.UserKeys.FirstOrDefault(x => x.Key == gKey && x.IsDeleted != true);
			if (dbKey == null) return null;

			if (immediateDeleteKey) {
				dbKey.IsDeleted = true;
				coreContext.SaveChanges();
			}

			return dbKey.UserID;
		}
		/// <summary>
		/// Generates UserKey and stores it to database, if there is a already a valid key it returnes it
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="keyType"></param>
		/// <param name="daysValid"></param>
		/// <returns></returns>
		private UserKey GenerateUserKey(int userId, UserKeyTypeEnum keyType, int daysValid = 15) {
			Guid key = Guid.Empty;

			UserKey userKey =
				 coreContext.UserKeys.FirstOrDefault(
					  x => x.UserID == userId && x.ExpireDate >= DateTime.Now && x.KeyType == (int)keyType && x.IsDeleted != true);

			if (userKey != null) {
				return userKey;
			} else {
				userKey = new UserKey {

					UserID = userId,
					KeyType = Convert.ToInt32(keyType),
					CreateDate = DateTime.UtcNow,
					ExpireDate = DateTime.UtcNow.AddDays(daysValid),
					Key = Guid.NewGuid()
				};

				coreContext.UserKeys.Add(userKey);
				coreContext.SaveChanges();
				logger.Info("Created user key <id={0}>", userKey.ID);
			}
			return userKey;
		}
		#endregion
	}
}

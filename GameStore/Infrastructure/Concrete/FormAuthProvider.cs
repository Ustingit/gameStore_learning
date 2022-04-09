using System;
using System.Web.Security;
using GameStore.Infrastructure.Abstract;

namespace GameStore.Infrastructure.Concrete
{
	public class FormAuthProvider : IAuthProvider
	{
		public bool Authenticate(string username, string password)
		{
			var valid = FormsAuthentication.Authenticate(username, password);

			if (valid)
			{
				FormsAuthentication.SetAuthCookie(username, false);
			}

			return valid;
		}
	}
}
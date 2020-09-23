
// FocusNewest.cs
// Tiago Togores - 2011

using System;
using System.Collections.Generic;

namespace Vitruvius.Users.Focus
{
	/// <summary>
	/// Algoritmo de foco de atenção que foca no usuário mais novo.
	/// </summary>
	public class FocusNewest : IFocus
	{
		/// <summary>
		/// Instancia um objeto da classe FocusNewest.
		/// </summary>
		public FocusNewest()
			: base()
		{
		}

		/// <summary>
		/// Devolve o usuário criado mais recentemente.
		/// </summary>
		override public User GetMainUser(Dictionary<int, User> users)
		{
			User newestUser = null;
			DateTime newestDate = DateTime.MinValue;
			foreach (User user in users.Values) {
				if (user.Valid) {
					if (newestDate.CompareTo(user.CreationDate) < 0) {
						newestUser = user;
						newestDate = user.CreationDate;
					}
				}
			}
			return newestUser;
		}
	}
}

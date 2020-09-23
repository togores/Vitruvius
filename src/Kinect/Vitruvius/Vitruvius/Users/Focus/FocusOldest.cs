
// FocusOldest.cs
// Tiago Togores - 2011

using System;
using System.Collections.Generic;

namespace Vitruvius.Users.Focus
{
	/// <summary>
	/// Algoritmo de foco de atenção que foca no usuário mais antigo.
	/// </summary>
	public class FocusOldest : IFocus
	{
		/// <summary>
		/// Instancia um objeto da classe FocusOldest.
		/// </summary>
		public FocusOldest()
			: base()
		{
		}

		/// <summary>
		/// Devolve o usuário criado menos recentemente.
		/// </summary>
		override public User GetMainUser(Dictionary<int, User> users)
		{
			User oldestUser = null;
			DateTime oldestDate = DateTime.MaxValue;
			foreach (User user in users.Values) {
				if (user.Valid) {
					if (oldestDate.CompareTo(user.CreationDate) > 0) {
						oldestUser = user;
						oldestDate = user.CreationDate;
					}
				}
			}
			return oldestUser;
		}
	}
}

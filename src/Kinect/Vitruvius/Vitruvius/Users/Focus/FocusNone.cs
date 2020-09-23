
// FocusNone.cs
// Tiago Togores - 2011

using System.Collections.Generic;

namespace Vitruvius.Users.Focus
{
	/// <summary>
	/// Algoritmo de foco de atenção que não foca em nenhum usuário.
	/// </summary>
	public class FocusNone : IFocus
	{
		/// <summary>
		/// Instancia um objeto da classe FocusNone.
		/// </summary>
		public FocusNone()
			: base()
		{
		}

		/// <summary>
		/// Devolve null.
		/// </summary>
		override public User GetMainUser(Dictionary<int, User> users)
		{
			return null;
		}
	}
}

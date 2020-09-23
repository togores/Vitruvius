
// FocusFarthest.cs
// Tiago Togores - 2011

using System;
using System.Collections.Generic;

namespace Vitruvius.Users.Focus
{
	/// <summary>
	/// Algoritmo de foco de atenção que foca no usuário mais distante.
	/// </summary>
	public class FocusFarthest : IFocus
	{
		/// <summary>
		/// Instancia um objeto da classe FocusFarthest.
		/// </summary>
		public FocusFarthest()
			: base()
		{
		}

		/// <summary>
		/// Devolve o usuário mais distante.
		/// </summary>
		override public User GetMainUser(Dictionary<int, User> users)
		{
			User farthestUser = null;
			double farthestDistance = Double.NegativeInfinity;
			foreach (User user in users.Values) {
				if (user.Valid) {
					double z = user.JointsPosition[ReferenceJoint].Position.Z;
					if (z > farthestDistance) {
						farthestDistance = z;
						farthestUser = user;
					}
				}
			}
			return farthestUser;
		}
	}
}

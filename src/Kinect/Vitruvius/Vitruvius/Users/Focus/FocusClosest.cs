
// FocusClosest.cs
// Tiago Togores - 2011

using System;
using System.Collections.Generic;

namespace Vitruvius.Users.Focus
{
	/// <summary>
	/// Algoritmo de foco de atenção que foca no usuário mais próximo.
	/// </summary>
	public class FocusClosest : IFocus
	{
		/// <summary>
		/// Instancia um objeto da classe FocusClosest.
		/// </summary>
		public FocusClosest()
			: base()
		{
		}

		/// <summary>
		/// Devolve o usuário mais próximo.
		/// </summary>
		public override User GetMainUser(Dictionary<int, User> users)
		{
			User closestUser = null;
			double closestDistance = Double.PositiveInfinity;
			foreach (User user in users.Values) {
				if (user.Valid) {
					double z = user.JointsPosition[ReferenceJoint].Position.Z;
					if (z < closestDistance) {
						closestDistance = z;
						closestUser = user;
					}
				}
			}
			return closestUser;
		}
	}
}

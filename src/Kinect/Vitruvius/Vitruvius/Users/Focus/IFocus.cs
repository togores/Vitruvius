
// IFocus.cs
// Tiago Togores - 2011

using System.Collections.Generic;
using OpenNI;

namespace Vitruvius.Users.Focus
{
	/// <summary>
	/// Classe que escolhe o usuário que merece o foco de atenção (usuário principal). 
	/// </summary>
	public abstract class IFocus
	{
		/// <summary>
		/// Usuário principal.
		/// </summary>
		protected User mainUser;

		/// <summary>
		/// O tempo restante para que o usuário deixe de ser principal.
		/// </summary>
		protected int mainUserTimeout;

		/// <summary>
		/// A articulação que é tomada como referência para a determinação do foco.
		/// </summary>
		public SkeletonJoint ReferenceJoint { get; set; }

		/// <summary>
		/// O tempo máximo necessário para que o usuário deixe de ser principal.
		/// </summary>
		public int FocusTimeout { get; set; }

		/// <summary>
		/// Construtor da classe IFocus que atribui valores padrões.
		/// </summary>
		public IFocus()
		{
			ReferenceJoint = SkeletonJoint.Torso;
			FocusTimeout = Controller.Fps;
			Reset();
		}

		/// <summary>
		/// Limpa o usuário principal.
		/// </summary>
		public void Reset()
		{
			mainUser = null;
			mainUserTimeout = 0;
		}

		/// <summary>
		/// Devolve o usuário principal.
		/// </summary>
		public abstract User GetMainUser(Dictionary<int, User> users);

		/// <summary>
		/// Devolve uma string que descreve as condições dos usuários: {principal}, [válido], (inválido)
		/// </summary>
		public string GetUsersLabel(Dictionary<int, User> users)
		{
			string text = users.Keys.Count.ToString() + " users: ";
			int mainUserId = (mainUser == null ? -1 : mainUser.ID);
			foreach (int id in users.Keys) {
				if (id == mainUserId)
					text += "{" + id.ToString() + "}, ";
				else if (users[id].Valid)
					text += "[" + id.ToString() + "], ";
				else
					text += "(" + id.ToString() + "), ";
			}
			return text;
		}

		/// <summary>
		/// Atualiza o usuário principal.
		/// Devolve o usuário que merece a atenção do sistema.
		/// Devolve null se não há nenhum usuário ou se ninguém merece atenção.
		/// </summary>
		public User UpdateMainUser(Dictionary<int, User> users)
		{
			User currentUser = GetMainUser(users);
			if (currentUser != mainUser) { // usuário principal mudou
				mainUserTimeout--;
				if (mainUserTimeout > 0) // se ainda resta tempo, espera por ele
					currentUser = null;
				else {// troca
					if (currentUser != null)
						mainUserTimeout = FocusTimeout;
					mainUser = currentUser;
				}
			}
			else {// se o usuário principal ainda continua no comando
				if (mainUser == null)
					mainUserTimeout = 0;
				else
					mainUserTimeout = FocusTimeout;
			}
			return currentUser;
		}
	}
}

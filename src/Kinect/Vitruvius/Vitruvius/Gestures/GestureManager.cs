
// GestureManager.cs
// Tiago Togores - 2011

using System;
using System.Collections.Generic;
using System.IO;

namespace Vitruvius.Gestures
{
	/// <summary>
	/// Gerenciador de gestos armazenados. 
	/// </summary>
	public class GestureManager
	{
		private Dictionary<string, Gesture> gestures;
		/// <summary>
		/// Coleção de nomes de gestos e suas representações.
		/// </summary>
		public Dictionary<string, Gesture> Gestures
		{
			get
			{
				return gestures;
			}
		}

		/// <summary>
		/// Instancia um novo objeto da classe GestureManager.
		/// </summary>
		public GestureManager()
		{
			gestures = new Dictionary<string, Gesture>();
		}

		/// <summary>
		/// Devolve o gesto com o nome passado.
		/// Se não existe, cria e insere um novo gesto.
		/// </summary>
		/// <param name="name">Nome do gesto.</param>
		public Gesture GetGesture(string name)
		{
			if (name == null)
				throw new ArgumentNullException();
			if (!gestures.ContainsKey(name))
				gestures[name] = new Gesture(name);
			return gestures[name];
		}

		/// <summary>
		/// Remove o gesto com o nome passado.
		/// </summary>
		/// <param name="name">Nome do gesto.</param>
		public void RemoveGesture(string name)
		{
			if (name == null)
				throw new ArgumentNullException();
			gestures.Remove(name);
		}

		#region Import/Export

		/// <summary>
		/// Importa gestos dos arquivos.
		/// </summary>
		public void ImportGestures(params string[] filenames)
		{
			foreach (string filename in filenames) {
				Gesture gesture = Gesture.Import(filename);
				if (gesture == null) {
					Controller.Instance.Print("Gesture not imported: invalid file content");
				}
				else if (gestures.ContainsKey(gesture.Name)) {
					Controller.Instance.Print("Gesture not imported: gesture already exists");
				}
				else {
					gestures.Add(gesture.Name, gesture);
					Controller.Instance.Print("Gesture imported: " + gesture.Name);
				}
			}
		}

		/// <summary>
		/// Exporta o gesto para um arquivo.
		/// </summary>
		public void ExportGesture(string gesture, string file)
		{
			if (!gestures.ContainsKey(gesture))
				Controller.Instance.Print("Gesture not exported: invalid gesture");
			else if (!gestures[gesture].Export(file))
				Controller.Instance.Print("Gesture not exported: failed");
			else
				Controller.Instance.Print("Gesture exported: " + gesture);
		}

		#endregion
	}
}

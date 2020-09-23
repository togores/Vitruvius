
// Persister.cs
// Tiago Togores - 2011

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Vitruvius
{
	/// <summary>
	/// Faz a serialização de um objeto para carregar/salvar em um arquivo.
	/// </summary>
	public class Persister
	{
		/// <summary>
		/// Importa um objeto serializado de um arquivo.
		/// </summary>
		/// <typeparam name="T">O tipo do objeto que será importado</typeparam>
		/// <param name="fileName">O nome do arquivo de onde será lido o estado do objeto</param>
		/// <returns>Se importado corretamente, o objeto. Caso contrário, null</returns>
		public static T Import<T>(string fileName)
		{
			Stream stream = null;
			T data = default(T);
			IFormatter formatter = new BinaryFormatter();
			try {
				stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
				stream.Position = 0;
				data = (T)formatter.Deserialize(stream);
			}
			catch (Exception e) {
				Console.WriteLine(e.Message);
			}
			finally {
				if (stream != null)
					stream.Close();
			}
			return data;
		}

		/// <summary>
		/// Exporta um objeto, serializando-o para um arquivo.
		/// </summary>
		/// <typeparam name="T">O tipo do objeto que será exportado</typeparam>
		/// <param name="data">O objeto a ser exportado</param>
		/// <param name="fileName">O nome do arquivo onde será salvo o estado do objeto</param>
		/// <returns>Se exportado corretamente, true. Caso contrário, false</returns>
		public static bool Export<T>(T data, string fileName)
		{
			Stream stream = null;
			IFormatter formatter = new BinaryFormatter();
			bool successful = true;
			try {
				stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
				stream.Position = 0;
				formatter.Serialize(stream, data);
			}
			catch (Exception e) {
				successful = false;
				Console.WriteLine(e.Message);
			}
			finally {
				if (stream != null)
					stream.Close();
			}
			return successful;
		}
	}
}

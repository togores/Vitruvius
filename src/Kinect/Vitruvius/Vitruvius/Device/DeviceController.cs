
// DeviceController.cs
// Tiago Togores - 2011

using OpenNI;

namespace Vitruvius.Device
{
	/// <summary>
	/// Classe abstrata que controla a leitura de dados de um dispositivo.
	/// </summary>
	public abstract class DeviceController
	{
		/// <summary>
		/// Gerador de dados.
		/// </summary>
		protected MapGenerator generator;
		
		/// <summary>
		/// Dados gerados.
		/// </summary>
		protected MapMetaData metadata;

		/// <summary>
		/// Atualiza os dados.
		/// </summary>
		abstract public void UpdateMetadata();
	}
}

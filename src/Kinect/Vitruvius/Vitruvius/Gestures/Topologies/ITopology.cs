
// ITopology.cs
// Tiago Togores - 2011

namespace Vitruvius.Gestures.Topologies
{
	/// <summary>
	/// Topologia de um modelo oculto de Markov.
	/// </summary>
	public interface ITopology
	{
		/// <summary>
		/// Inicializa as probabilidades de transição e emissão de acordo com a topologia.
		/// </summary>
		void Init(HiddenMarkovModel hmm);
	}
}

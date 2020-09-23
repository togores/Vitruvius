
// ErgodicTopology.cs
// Tiago Togores - 2011

namespace Vitruvius.Gestures.Topologies
{
	/// <summary>
	/// Topologia ergódica: cada estado é alcançável por outro em um passo.
	/// </summary>
	public class ErgodicTopology : ITopology
	{
		/// <summary>
		/// Inicializa as probabilidades de transição e emissão de acordo com a topologia.
		/// </summary>
		public void Init(HiddenMarkovModel hmm)
		{
			for (int i = 0; i < hmm.States; i++) {
				// probabilidades de início
				hmm.Pi[i] = 1.0/hmm.States;
				// probabilidades de transição
				for (int j = 0; j < hmm.States; j++)
					hmm.A[i, j] = 1.0/hmm.States;
				// probabilidades de emissão
				for (int j = 0; j < hmm.Observations; j++)
					hmm.B[i, j] = 1.0/hmm.Observations;
			}
		}
	}
}

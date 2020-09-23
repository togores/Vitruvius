
// BakisTopology.cs
// Tiago Togores - 2011

using System;

namespace Vitruvius.Gestures.Topologies
{
	/// <summary>
	/// Topologia de Bakis: um estado j é alcançado por um estado i em um passo
	/// se j >= i e j - i é menor ou igual a delta.
	/// </summary>
	public class BakisTopology : ITopology
	{
		/// <summary>
		/// número máximo de estados seguintes em que há probabilidade
		/// de transição não nula.
		/// </summary>
		private int delta;

		/// <summary>
		/// Instancia um objeto do tipo BakisTopology.
		/// </summary>
		public BakisTopology(int delta)
		{
			if (delta <= 0)
				throw new ArgumentOutOfRangeException("delta should be greater than zero");
			this.delta = delta;
		}

		/// <summary>
		/// Inicializa as probabilidades de transição e emissão de acordo com a topologia.
		/// </summary>
		public void Init(HiddenMarkovModel hmm)
		{
			for (int i = 0; i < hmm.States; i++) {
				// probabilidades de início
				hmm.Pi[i] = 0.0;
				// probabilidades de transição
				int j;
				for (j = 0; j < i; j++)
					hmm.A[i, j] = 0.0;
				int end = Math.Min(i + delta, hmm.States - 1);
				for (j = i; j <= end; j++)
					hmm.A[i, j] = 1.0/(end - i + 1);
				for (; j < hmm.States; j++)
					hmm.A[i, j] = 0.0;
				// probabilidades de emissão
				for (j = 0; j < hmm.Observations; j++)
					hmm.B[i, j] = 1.0/hmm.Observations;
			}
			hmm.Pi[0] = 1.0;
		}
	}
}


// HiddenMarkovModel.cs
// Tiago Togores - 2011

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vitruvius.Gestures.Topologies;

namespace Vitruvius.Gestures
{
	/// <summary>
	/// Modelo oculto de markov.
	/// </summary>
	[Serializable]
	public class HiddenMarkovModel
	{
		/// <summary>
		/// número de estados.
		/// </summary>
		public int States { get; set; }

		/// <summary>
		/// número de observações.
		/// </summary>
		public int Observations { get; set; }

		/// <summary>
		/// matriz de transição.
		/// </summary>
		public double[,] A { get; set; }

		/// <summary>
		/// matriz de emissão.
		/// </summary>
		public double[,] B { get; set; }

		/// <summary>
		/// distribuição inicial.
		/// </summary>
		public double[] Pi { get; set; }

		/// <summary>
		/// Instancia um objeto do tipo HiddenMarkovModel.
		/// </summary>
		/// <param name="states">número de estados</param>
		/// <param name="observations">número de observações</param>
		/// <param name="topology">topologia do modelo</param>
		public HiddenMarkovModel(int states, int observations, ITopology topology)
		{
			States = states;
			Observations = observations;
			A = new double[states, states];
			B = new double[states, observations];
			Pi = new double[states];
			topology.Init(this);
		}

		/// <summary>
		/// Devolve as probabilidades forward do modelo, com relação às observações de o.
		/// Guarda em c os coeficientes de escala, que podem ser seus inversos se inverted for true.
		/// </summary>
		private double[,] forward(int[] o, out double[] c, bool inverted)
		{
			int N = o.Length;
			double[,] forw = new double[N, States];
			c = new double[N];

			// base da indução
			c[0] = 0;
			for (int j = 0; j < States; j++) {
				forw[0, j] = Pi[j]*B[j, o[0]];
				c[0] += forw[0, j];
			}
			if (!inverted)
				c[0] = 1.0/c[0];
			for (int j = 0; j < States; j++) {
				if (inverted)
					forw[0, j] /= c[0];
				else
					forw[0, j] *= c[0];
			}

			// passo da indução
			for (int n = 1; n < N; n++) {
				c[n] = 0;
				for (int j = 0; j < States; j++) {
					forw[n, j] = 0;
					for (int i = 0; i < States; i++)
						forw[n, j] += forw[n - 1, i]*A[i, j];
					forw[n, j] *= B[j, o[n]];
					c[n] += forw[n, j];
				}
				if (!inverted)
					c[n] = 1.0/c[n];
				for (int j = 0; j < States; j++) {
					if (inverted)
						forw[n, j] /= c[n];
					else
						forw[n, j] *= c[n];
				}
			}

			return forw;
		}

		/// <summary>
		/// Devolve as probabilidades backward, com relação às observações de o.
		/// Recebe c, os coeficientes de escala, que podem estar invertidos se inverted for true.
		/// </summary>
		private double[,] backward(int[] o, double[] c, bool inverted)
		{
			int N = o.Length;
			double[,] back = new double[N, States];

			// base da indução
			for (int j = 0; j < States; j++) {
				if (inverted)
					back[N - 1, j] = 1/c[N - 1];
				else
					back[N - 1, j] = c[N - 1];
			}

			// passo da indução
			for (int n = N - 2; n >= 0; n--) {
				for (int j = 0; j < States; j++) {
					back[n, j] = 0;
					for (int i = 0; i < States; i++)
						back[n, j] += A[j, i]*B[i, o[n + 1]]*back[n + 1, i];
					if (inverted)
						back[n, j] /= c[n];
					else
						back[n, j] *= c[n];
				}
			}

			return back;
		}

		/// <summary>
		/// Recebe os coeficientes de escala no cálculo das variáveis forward invertidos ou não.
		/// Devolve o probabilidade do problema da estimação.
		/// </summary>
		private double Likelihood(double[] c, bool inverted)
		{
			double prob = 0.0;
			for (int i = 0; i < c.Length; i++)
				prob += Math.Log(c[i]);
			if (inverted)
				prob = -prob;
			return Math.Exp(-prob);
		}

		/// <summary>
		/// Devolve a probabilidade P(O|HMM).
		/// </summary>
		public double Evaluate(int[] o)
		{
			return Evaluate(o, false);
		}

		/// <summary>
		/// Devolve a probabilidade P(O|HMM).
		/// inverted indica se os coeficientes de escala devem ser invertidos.
		/// </summary>
		public double Evaluate(int[] o, bool inverted)
		{
			if (o == null || o.Length == 0)
				return 0.0;
			double[] c;
			forward(o, out c, inverted);
			return Likelihood(c, inverted);
		}

		/// <summary>
		/// Realiza o aprendizado do modelo com o conjunto de sequências de observações o.
		/// Aplica o algoritmo de Baum-Welch com escala.
		/// </summary>
		public void Learn(int[][] o)
		{
			Learn(o, true);
		}

		/// <summary>
		/// Realiza o aprendizado do modelo com o conjunto de sequências de observações o.
		/// Aplica o algoritmo de Baum-Welch com escala.
		/// inverted indica se os coeficientes de escala devem ser invertidos.
		/// </summary>
		public void Learn(int[][] o, bool inverted)
		{
			if (o == null || o.Length == 0)
				return;
			int N = o.Length;
			double oldprob = 0;
			double newprob;

			// a cada iteração, reestima as probabilidades do modelo
			while (true) {
				double[][,] forw = new double[N][,];
				double[][,] back = new double[N][,];
				double[][] c = new double[N][];
				double[] prob = new double[N];

				// calcula a probabilidade média do modelo emitir as sequências
				newprob = 0;
				for (int l = 0; l < N; l++) {
					forw[l] = forward(o[l], out c[l], inverted);
					back[l] = backward(o[l], c[l], inverted);
					prob[l] = Likelihood(c[l], inverted);
					newprob += prob[l];
				}
				newprob /= N;

				// erro na conta
				if (double.IsNaN(newprob) || double.IsInfinity(newprob) || newprob < 0 || newprob > 1)
					break;
				
				// convergiu, isto é, valor só variou no máximo 0.1% desde a última iteração 
				if (Util.AreClose(oldprob, newprob, oldprob*1.001))
					break;

				oldprob = newprob;

				//calcula gamma
				double[][,] gamma = new double[N][,];
				for (int l = 0; l < N; l++) {
					int T = o[l].Length;
					gamma[l] = new double[T, States];
					for (int t = 0; t < T; t++) {
						double scale = 0;
						for (int i = 0; i < States; i++) {
							gamma[l][t, i] = forw[l][t, i]*back[l][t, i];
							scale += gamma[l][t, i];
						}
						for (int i = 0; i < States; i++)
							gamma[l][t, i] /= scale;
					}
				}

				//calcula ksi
				double[][, ,] ksi = new double[N][, ,];
				for (int l = 0; l < N; l++) {
					int T = o[l].Length;
					ksi[l] = new double[T, States, States];
					for (int t = 0; t < T - 1; t++) {
						double scale = 0;
						for (int i = 0; i < States; i++) {
							for (int j = 0; j < States; j++) {
								ksi[l][t, i, j] = forw[l][t, i]*A[i, j]*B[j, o[l][t + 1]]*back[l][t + 1, j] / prob[l];
								scale += ksi[l][t, i, j];
							}
						}
						for (int i = 0; i < States; i++) {
							for (int j = 0; j < States; j++) {
								ksi[l][t, i, j] /= scale;
							}
						}
					}
				}

				// calcula as novas probabilidades inicias
				for (int i = 0; i < States; i++) {
					Pi[i] = 0;
					for (int l = 0; l < N; l++)
						Pi[i] += gamma[l][0, i];
					Pi[i] /= N;
				}

				// calcula as novas probabilidades de transição
				for (int i = 0; i < States; i++) {
					for (int j = 0; j < States; j++) {
						double num = 0, den = 0;
						for (int l = 0; l < N; l++) {
							for (int t = 0; t < o[l].Length - 1; t++) {
								num += ksi[l][t, i, j];
								den += gamma[l][t, i];
							}
						}
						A[i, j] = 0;
						if (den != 0)
							A[i, j] = num/den;
					}
				}

				// calcula as novas probabilidades de emissão
				for (int j = 0; j < States; j++) {
					for (int k = 0; k < Observations; k++) {
						double num = 0, den = 0;
						for (int l = 0; l < N; l++) {
							for (int t = 0; t < o[l].Length; t++) {
								if (o[l][t] == k)
									num += gamma[l][t, j];
								den += gamma[l][t, j];
							}
						}
						B[j, k] = 0;
						if (den != 0)
							B[j, k] = num/den;
					}
				}
			}
		}
	}
}

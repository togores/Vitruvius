
// Quantizer.cs
// Tiago Togores - 2011

using System;
using System.Collections.Generic;

namespace Vitruvius.Gestures
{
	/// <summary>
	/// Responsável por quantizar os vetores característicos.
	/// </summary>
	[Serializable]
	public class Quantizer
	{
		/// <summary>
		/// Os centróides de cada partição
		/// </summary>
		private Dictionary<int, Feature> centroids;
		// Cada entrada do dicionário é <partição, centróide>

		/// <summary>
		/// número de partições
		/// </summary>
		public int Clusters
		{
			get
			{
				return centroids.Count;
			}
		}

		/// <summary>
		/// Devolve uma nova instância de um Quantizer,
		/// inicializado com partições aleatórias.
		/// </summary>
		/// <param name="dimension">dimensão do espaço vetorial</param>
		/// <param name="numClusters">número de partições do espaço</param>
		public Quantizer(int dimension, int numClusters)
		{
			centroids = new Dictionary<int, Feature>();
			Random random = new Random();
			for (int i = 0; i < numClusters; i++) {
				double[] vector = new double[dimension];
				for (int j = 0; j < vector.Length; j++)
					vector[j] = 2*random.NextDouble() - 1; // [-1,1]
				centroids.Add(i, new Feature(vector));
			}
		}

		/// <summary>
		/// Devolve uma nova instância de um Quantizer,
		/// com representantes de partição pré-definidos e fixos.
		/// </summary>
		public Quantizer(int dimension)
		{
			List<double[]> set1 = new List<double[]>();
			double s = Math.Sqrt(2)/2;
			set1.Add(new double[] { 1, 0 });
			set1.Add(new double[] { s, s });
			set1.Add(new double[] { 0, 1 });
			set1.Add(new double[] { -s, s });
			set1.Add(new double[] { -1, 0 });
			set1.Add(new double[] { -s, -s });
			set1.Add(new double[] { 0, -1 });
			set1.Add(new double[] { s, -s });

			for (int n = 3; n <= dimension; n++) {
				List<double[]> set2 = new List<double[]>();
				foreach (double[] v1 in set1) { // para cada vetor no conjunto
					for (int p = 0; p < n; p++) {
						// calcula uma "intercalação" dele com 0 na posição p
						double[] v2 = new double[n];
						int k = 0;
						for (; k < p; k++)
							v2[k] = v1[k];
						v2[k++] = 0;
						for (; k < n; k++)
							v2[k] = v1[k - 1];

						// insere no conjunto se não há nenhum elemento semelhante
						bool equals = false;
						foreach (double[] v in set2) {
							equals = true;
							for (int i = 0; i < v.Length; i++) {
								if (!Util.AreClose(v[i], v2[i])) {
									equals = false;
									break;
								}
							}
							if (equals)
								break;
						}
						if (!equals)
							set2.Add(v2);
					}
				}
				set1 = set2;
			}

			// define os centróides como os vetores gerados pelo conjunto
			centroids = new Dictionary<int, Feature>();
			int j = 0;
			foreach (double[] vector in set1)
				centroids.Add(j++, new Feature(vector));
		}

		/// <summary>
		/// Devolve o índice da partição à qual o vetor pertence.
		/// </summary>
		private int Quantize(Feature feature)
		{
			int label = -1;
			double distance, minDistance = Double.PositiveInfinity;
			foreach (KeyValuePair<int, Feature> entry in centroids) {
				distance = feature.CalculateDistance(entry.Value);
				if (distance < minDistance) {
					minDistance = distance;
					label = entry.Key;
				}
			}
			return label;
		}

		/// <summary>
		/// Quantiza cada vetor característico da lista para um valor discreto.
		/// </summary>
		public int[] Quantize(List<Feature> features)
		{
			if (features == null || features.Count == 0)
				return null;
			int[] labels = new int[features.Count];
			for (int i = 0; i < labels.Length; i++)
				labels[i] = Quantize(features[i]);
			return labels;
		}

		/// <summary>
		/// Atualiza os centróides das partições dados os vetores.
		/// Utiliza o algoritmo k-means.
		/// </summary>
		public void Update(List<Feature> features)
		{
			if (features == null || features.Count == 0)
				return;

			// inicialmente, os vetores não estão em nenhuma partição
			int[] labels = new int[features.Count];
			for (int i = 0; i < labels.Length; i++)
				labels[i] = -1;

			bool changed; // se mudou algum centróide
			do {
				double[] count = new double[centroids.Count]; // número de vetores em cada partição
				for (int i = 0; i < count.Length; i++)
					count[i] = 0;

				Feature[] newCentroids = new Feature[centroids.Count];
				// quantiza os vetores, atribuindo-os a partições
				for (int i = 0; i < features.Count; i++) {
					labels[i] = Quantize(features[i]);
					count[labels[i]]++;
					newCentroids[labels[i]] += features[i];
				}

				// atualiza os centróides de cada partição
				changed = false;
				for (int i = 0; i < centroids.Count; i++) {
					newCentroids[i] /= count[i];
					if (!Util.Equals(newCentroids[i], centroids[i]))
						changed = true;
					centroids[i] = newCentroids[i];
				}
			} while (changed);
		}
	}
}

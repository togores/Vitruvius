
// Gesture.cs
// Tiago Togores - 2011

using System;
using System.Collections.Generic;
using Vitruvius.Gestures.Topologies;

namespace Vitruvius.Gestures
{
	/// <summary>
	/// Representa um gesto.
	/// </summary>
	[Serializable]
	public class Gesture
	{
		#region Fields and Properties

		/// <summary>
		/// Nome do gesto.
		/// </summary>
		public string Name
		{
			get;
			set;
		}

		/// <summary>
		/// Modelo oculto de Markov que representa o gesto.
		/// </summary>
		public HiddenMarkovModel Hmm
		{
			get;
			set;
		}

		/// <summary>
		/// Quantas vezes um gesto foi treinado.
		/// </summary>
		public int TrainingCounter
		{
			get
			{
				return trainingSet.Count;
			}
		}

		/// <summary>
		/// Conjunto de observações utilizada para treinamento.
		/// </summary>
		private List<int[]> trainingSet;

		/// <summary>
		/// Quantizador.
		/// </summary>
		private Quantizer quantizer;

		#endregion

		#region Constructors

		/// <summary>
		/// Cria um gesto desconhecido.
		/// </summary>
		/// <param name="name">Nome do gesto.</param>
		public Gesture(string name)
		{
			Name = name;
			quantizer = Parameterization.Quantizer;
			Hmm = new HiddenMarkovModel(Parameterization.NumStates, Parameterization.NumObservations, Parameterization.Topology);
			trainingSet = new List<int[]>();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Devolve a probabilidade das observações terem sido geradas por esse modelo.
		/// </summary>
		public double Evaluate(List<Feature> observations)
		{
			var q = quantizer.Quantize(observations);
			return Hmm.Evaluate(q);
		}

		/// <summary>
		/// Acrescenta as observações ao conjunto de treinamento
		/// e adapta o modelo.
		/// </summary>
		public void Train(List<Feature> observations)
		{
			var o = quantizer.Quantize(observations);
			if (o != null) {
				trainingSet.Add(o);
				if (Parameterization.IncrementalTraining || trainingSet.Count == Parameterization.TrainingSetSize)
					Hmm.Learn(trainingSet.ToArray());
			}
		}

		/// <summary>
		/// Importa um objeto Gesture que foi salvo num arquivo
		/// </summary>
		/// <param name="filePath">nome do arquivo</param>
		/// <returns>o objeto importado</returns>
		public static Gesture Import(string filePath)
		{
			return Persister.Import<Gesture>(filePath);
		}

		/// <summary>
		/// Exporta o objeto para um arquivo
		/// </summary>
		/// <param name="filePath">nome do arquivo</param>
		public bool Export(string filePath)
		{
			return Persister.Export<Gesture>(this, filePath);
		}

		#endregion
	}
}

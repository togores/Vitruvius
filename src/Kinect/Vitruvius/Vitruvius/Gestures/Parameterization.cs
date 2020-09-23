
// Parameterization.cs
// Tiago Togores - 2011

using System;
using OpenNI;
using Vitruvius.Gestures.Extractors;
using Vitruvius.Gestures.Filters;
using Vitruvius.Gestures.Topologies;

namespace Vitruvius.Gestures
{
	/// <summary>
	/// Parametrizações dos módulos que fazem parte das etapas
	/// do processo de reconhecimento de gestos.
	/// </summary>
	class Parameterization
	{
		/// <summary>
		/// A articulação responsável pelo gesto.
		/// </summary>
		public static SkeletonJoint Joint = SkeletonJoint.RightHand;

		/// <summary>
		/// Extrator de vetores característicos.
		/// </summary>
		public static IExtractor Extractor = new OrientationExtractor(Joint);

		/// <summary>
		/// Filtro de dados.
		/// </summary>
		public static IFilter Filter = new SimilarityFilter(0.25);

		/// <summary>
		/// Quantizador.
		/// </summary>
		public static Quantizer Quantizer = new Quantizer(Extractor.Dimension);

		/// <summary>
		/// Número de estados ocultos dos HMMs.
		/// </summary>
		public static int NumStates = 10;

		/// <summary>
		/// Número de estados observáveis dos HMMs.
		/// </summary>
		public static int NumObservations = Quantizer.Clusters;

		/// <summary>
		/// Topologia dos HMMs.
		/// </summary>
		public static ITopology Topology = new BakisTopology(1);

		/// <summary>
		/// Se cada gesto é treinado cada vez que um novo elemento é adicionado ao conjunto de treinamento.
		/// </summary>
		public static bool IncrementalTraining = false;

		/// <summary>
		/// Tamanho do conjunto de treinamento esperado para realizar um treinamento se ele não for incremental.
		/// </summary>
		public static int TrainingSetSize = 80;
	}
}

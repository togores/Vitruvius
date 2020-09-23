
// Observer.cs
// Tiago Togores - 2011

using System.Collections.Generic;
using Vitruvius.Gestures.Extractors;
using Vitruvius.Gestures.Filters;
using Vitruvius.Users;

namespace Vitruvius.Gestures
{
	/// <summary>
	/// Realiza o treinameto/reconhecimento de um gesto.
	/// </summary>
	public class Observer
	{
		/// <summary>
		/// Filtro que será aplicado na entrada.
		/// </summary>
		private IFilter filter;

		/// <summary>
		/// Extrator de características.
		/// </summary>
		private IExtractor extractor;

		/// <summary>
		/// Observações que serão usadas no treinamento/reconhecimento.
		/// </summary>
		private List<Feature> observations;

		/// <summary>
		/// Gerenciador dos gestos.
		/// </summary>
		public GestureManager GestureManager { get; set; }

		/// <summary>
		/// Gesto para treinamento.
		/// </summary>
		public string GestureName { get; set; }

		/// <summary>
		/// Instancia um novo objeto da classe Observer.
		/// </summary>
		public Observer(GestureManager manager, IExtractor extractor, IFilter filter)
		{
			GestureManager = manager;
			this.filter = filter;
			this.extractor = extractor;
			observations = new List<Feature>();
		}

		#region Methods

		/// <summary>
		/// Inicia uma nova observação.
		/// </summary>
		public void StartObserving()
		{
			filter.Reset();
			extractor.Reset();
			observations.Clear();
		}

		/// <summary>
		/// Obtém informações instantâneas do gesto sendo executado pelo usuário.
		/// </summary>
		/// <param name="User">Usuário de quem o gesto é extraído.</param>
		public void Capture(User user)
		{
			if (user != null) {
				Feature feature = extractor.Extract(user);
				if (feature != null)
					feature = filter.ApplyFilter(feature);
				if (feature != null) {
					observations.Add(feature);
				}
			}
		}

		/// <summary>
		/// Treina o gesto.
		/// </summary>
		public void Train()
		{
			GestureManager.GetGesture(GestureName).Train(observations);
		}

		/// <summary>
		/// Devolve o gesto mais provável de representar as observações.
		/// </summary>
		public Gesture Recognize()
		{
			double prob, maxProb = 0;
			Gesture maxGesture = null;
			foreach (Gesture gesture in GestureManager.Gestures.Values) {
				prob = gesture.Evaluate(this.observations);
				if (prob > maxProb) {
					maxProb = prob;
					maxGesture = gesture;
				}
			}
			return maxGesture;
		}

		#endregion
	}
}

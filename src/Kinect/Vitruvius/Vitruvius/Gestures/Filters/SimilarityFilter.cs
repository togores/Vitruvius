
// SimilarityFilter.cs
// Tiago Togores - 2011

using System;

namespace Vitruvius.Gestures.Filters
{
	/// <summary>
	/// Filtra os dados similares.
	/// </summary>
	public class SimilarityFilter : IFilter
	{
		/// <summary>
		/// Última característica observada.
		/// </summary>
		Feature last;

		/// <summary>
		/// coeficiente de similaridade
		/// </summary>
		double similarity;

		/// <summary>
		/// Instancia um objeto de tipo SimilarityFilter.
		/// </summary>
		/// <param name="similarity">coeficiente de similaridade</param>
		public SimilarityFilter(double similarity)
		{
			if (similarity < 0)
				throw new ArgumentException();
			this.similarity = similarity;
			last = null;
		}

		/// <summary>
		/// Aplica o filtro e devolve o feature filtrado, se for relevante.
		/// Devolve null, caso contrário.
		/// </summary>
		public Feature ApplyFilter(Feature feature)
		{
			if (feature != null && !feature.Similar(last, similarity)) {
				last = feature;
				return feature;
			}
			return null;
		}

		/// <summary>
		/// Apaga as condições temporais/espaciais para a aplicação do filtro.
		/// </summary>
		public void Reset()
		{
			last = null;
		}
	}
}


// IFilter.cs
// Tiago Togores - 2011

namespace Vitruvius.Gestures.Filters
{
	/// <summary>
	/// Filtro que só deixa passar dados relevantes.
	/// </summary>
	public interface IFilter
	{
		/// <summary>
		/// Aplica o filtro e devolve o feature filtrado, se for relevante.
		/// Devolve null, caso contrário.
		/// </summary>
		Feature ApplyFilter(Feature feature);

		/// <summary>
		/// Apaga as condições temporais/espaciais para a aplicação do filtro.
		/// </summary>
		void Reset();
	}
}

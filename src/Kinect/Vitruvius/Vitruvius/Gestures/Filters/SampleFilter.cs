
// SampleFilter.cs
// Tiago Togores - 2011

namespace Vitruvius.Gestures.Filters
{
	/// <summary>
	/// Filtro que só deixa passar dados após uma certa contagem de frames.
	/// </summary>
	public class SampleFilter : IFilter
	{
		/// <summary>
		/// quantos frames já se passarem desde o último válido
		/// </summary>
		private int count;

		/// <summary>
		/// quantos precisam passar para que se consiga um válido
		/// </summary>
		public int Limit { get; set; }

		/// <summary>
		/// Devolve uma instância do filtro que tem o limite de filtragem especificado.
		/// </summary>
		/// <param name="limit">Quantidade de frames necessários para que um deles seja válido.</param>
		public SampleFilter(int limit)
		{
			Limit = limit;
			count = 0;
		}

		/// <summary>
		/// Aplica o filtro e devolve o feature filtrado, se for relevante.
		/// Devolve null, caso contrário.
		/// </summary>
		public Feature ApplyFilter(Feature feature)
		{
			if (++count == Limit) {
				count = 0;
				return feature;
			}
			return null;
		}

		/// <summary>
		/// Apaga as condições temporais/espaciais para a aplicação do filtro.
		/// </summary>
		public void Reset()
		{
			count = 0;
		}
	}
}

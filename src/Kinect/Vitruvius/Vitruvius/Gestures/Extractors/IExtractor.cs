
// IExtractor.cs
// Tiago Togores - 2011

using System;
using OpenNI;
using Vitruvius.Users;

namespace Vitruvius.Gestures.Extractors
{
	/// <summary>
	/// Extrator de vetores característicos de um gesto.
	/// </summary>
	public abstract class IExtractor
	{
		protected int dimension = 0;
		/// <summary>
		/// Dimensão do espaço do vetor extraído.
		/// </summary>
		public int Dimension
		{
			get
			{
				return dimension;
			}
		}

		#region Abstract

		/// <summary>
		/// Extrai o vetor característico de um usuário realizando um gesto. 
		/// </summary>
		abstract public Feature Extract(User user);

		/// <summary>
		/// Apaga as dependências temporais para a extração dos vetores característicos.
		/// </summary>
		abstract public void Reset();

		#endregion
	}
}

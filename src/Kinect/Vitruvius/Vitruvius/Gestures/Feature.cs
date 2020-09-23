
// Feature.cs
// Tiago Togores - 2011

using System;

namespace Vitruvius.Gestures
{
	[Serializable]
	/// <summary>
	/// Representa um vetor característico de dimensão arbitrária.
	/// </summary>
	public class Feature
	{
		private double[] vector;
		/// <summary>
		/// O vetor característico.
		/// </summary>
		public double[] Vector
		{
			get
			{
				return vector;
			}
		}

		/// <summary>
		/// Cria uma nova instância da classe Feature.
		/// </summary>
		public Feature(params double[] vector)
		{
			this.vector = vector;
		}

		/// <summary>
		/// Combina os dois vetores, concatenando o feature.
		/// </summary>
		public Feature Combine(Feature feature)
		{
			double[] vector1 = this.Vector;
			double[] vector2 = feature.Vector;
			int length1 = vector1.Length;
			int length2 = vector2.Length;
			int newLength = length1 + length2;
			double[] newVector = new double[newLength];
			int i;
			for (i = 0; i < length1; i++)
				newVector[i] = vector1[i];
			for (; i < newLength; i++)
				newVector[i] = vector2[i - length1];
			return new Feature(newVector);
		}

		/// <summary>
		/// Calcula a distância euclidiana entre os dois vetores, como se fossem pontos.
		/// </summary>
		public double CalculateDistance(Feature feature)
		{
			double[] v1 = this.Vector;
			double[] v2 = feature.Vector;
			if (v1.Length != v2.Length)
				throw new ArgumentException();
			double d = 0.0;
			for (int i = 0; i < v1.Length; i++)
				d += (v1[i] - v2[i]) * (v1[i] - v2[i]);
			return Math.Sqrt(d);
		}

		/// <summary>
		/// Devolve um vetor que é a soma de f1 com f2 coordenada a coordenada.
		/// </summary>
		public static Feature operator +(Feature f1, Feature f2)
		{
			if (f1 == null)
				return f2;
			if (f2 == null)
				return f1;
			int length = f1.vector.Length;
			if (length != f2.vector.Length)
				throw new ArgumentException();
			double[] vector = new double[length];
			for (int i = 0; i < length; i++)
				vector[i] = f1.vector[i] + f2.vector[i];
			return new Feature(vector);
		}

		/// <summary>
		/// Devolve um vetor em que cada coordenada é igual a respectiva
		/// coordenada de f1 dividida por d.
		/// </summary>
		public static Feature operator /(Feature f1, double d)
		{
			if (Util.AreClose(d, 0))
				throw new DivideByZeroException();
			int length = f1.vector.Length;
			double[] vector = new double[length];
			for (int i = 0; i < length; i++)
				vector[i] = f1.vector[i] / d;
			return new Feature(vector);
		}

		/// <summary>
		/// Devolve a norma euclideana do vetor.
		/// </summary>
		public double GetNorm()
		{
			double norm = 0.0;
			foreach (double d in vector)
				norm += d * d;
			return Math.Sqrt(norm);
		}

		/// <summary>
		/// Normaliza o vetor.
		/// </summary>
		public void Normalize()
		{
			double norm = GetNorm();
			if (Util.AreClose(norm, 0))
				return;
			for (int i = 0; i < vector.Length; i++)
				vector[i] /= norm;
		}

		/// <summary>
		/// Devolve true se os dois vetores característicos são similares.
		/// </summary>
		public bool Similar(Feature feature, double similarity)
		{
			if (feature == null)
				return false;
			return this.CalculateDistance(feature) < similarity;
		}

		#region Override

		/// <summary>
		/// Devolve true se os dois vetores são iguais.
		/// Devolve false, caso contrário.
		/// </summary>
		private bool Equals(double[] a, double[] b)
		{
			if (a.Length != b.Length)
				return false;
			for (int i = 0; i < a.Length; i++)
				if (!Util.AreClose(a[i], b[i]))
					return false;
			return true;
		}

		override public bool Equals(Object o)
		{
			if ((o == null) || !(o is Feature))
				return false;
			return Equals(this.vector, ((Feature)o).vector);
		}

		public bool Equals(Feature f)
		{
			if (f == null)
				return false;
			return Equals(this.vector, f.vector);
		}

		public override int GetHashCode()
		{
			return vector.GetHashCode();
		}

		public override string ToString()
		{
			string s = string.Empty;
			foreach (double d in this.vector)
				s += d + ", ";
			return s;
		}

		#endregion
	}
}

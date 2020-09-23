
// Util.cs
// Tiago Togores - 2011

using System;
using OpenNI;

namespace Vitruvius
{
	/// <summary>
	/// Métodos auxiliares. 
	/// </summary>
	class Util
	{
		/// <summary>
		/// Devolve true se os dois valores diferem por no máximo eps.
		/// </summary>
		public static bool AreClose(double a, double b, double eps)
		{
			if (a == b)
				return true;
			return Math.Abs(a - b) <= eps;
		}

		/// <summary>
		/// Devolve true se os dois valores são próximos.
		/// </summary>
		public static bool AreClose(double a, double b)
		{
			return AreClose(a, b, 1e-6 * (Math.Abs(a) + Math.Abs(b) + 10.0));
		}

		// Métodos auxiliares para facilitar o cálculo de todos os extratores
		#region Helper

		/// <summary>
		/// Calcula a adição de dois pontos coordenada a coordenada.
		/// </summary>
		public static Point3D Add(Point3D p, Point3D q)
		{
			return new Point3D(p.X + q.X, p.Y + q.Y, p.Z + q.Z);
		}

		/// <summary>
		/// Calcula a subtração de dois pontos coordenada a coordenada.
		/// </summary>
		public static Point3D Subtract(Point3D p, Point3D q)
		{
			return new Point3D(p.X - q.X, p.Y - q.Y, p.Z - q.Z);
		}

		/// <summary>
		/// Calcula a multiplicação de um ponto por um scalar.
		/// </summary>
		public static Point3D Multiply(Point3D point, float scalar)
		{
			return new Point3D(point.X * scalar, point.Y * scalar, point.Z * scalar);
		}

		/// <summary>
		/// Calcula a divisão de um ponto por um scalar.
		/// </summary>
		public static Point3D? Divide(Point3D point, float scalar)
		{
			if (float.Equals(scalar, 0))
				throw new DivideByZeroException();
			return Multiply(point, 1.0f / scalar);
		}

		/// <summary>
		/// Calcula a distância entre p e q.
		/// </summary>
		public static double Distance(Point3D p, Point3D q)
		{
			return Norm(Subtract(p, q));
		}

		/// <summary>
		/// Calcula a "norma" de um ponto p.
		/// </summary>
		public static double Norm(Point3D p)
		{
			return Math.Sqrt(p.X * p.X + p.Y * p.Y + p.Z * p.Z);
		}

		/// <summary>
		/// "Normaliza" um ponto p.
		/// </summary>
		public static Point3D Normalized(Point3D p)
		{
			Point3D n = new Point3D();
			float norm = (float)Norm(p);
			if (AreClose(norm, 0))
				return p;
			n.X = p.X / norm;
			n.Y = p.Y / norm;
			n.Z = p.Z / norm;
			return n;
		}

		/// <summary>
		/// Converte o ponto para um vetor.
		/// </summary>
		public static double[] PointToArray(Point3D point)
		{
			return new double[] { point.X, point.Y, point.Z };
		}

		/// <summary>
		/// Devolve uma string que representa o vetor.
		/// </summary>
		public static string ArrayToString(int[] q)
		{
			string s = "(";
			int i;
			for (i = 0; i < q.Length - 1; i++)
				s += q[i] + ", ";
			s += i + ")";
			return s;
		}

		#endregion
	}
}

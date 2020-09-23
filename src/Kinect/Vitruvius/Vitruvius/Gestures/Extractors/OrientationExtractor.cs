
// OrientationExtractor.cs
// Tiago Togores - 2011

using OpenNI;
using Vitruvius.Users;

namespace Vitruvius.Gestures.Extractors
{
	/// <summary>
	/// Extrator que observa a orientação do movimento.
	/// </summary>
	class OrientationExtractor : IExtractor
	{
		/// <summary>
		/// articulação que realiza o gesto
		/// </summary>
		private SkeletonJoint joint;

		/// <summary>
		/// última posição observada da articulação.
		/// </summary>
		private Point3D? last;

		/// <summary>
		/// Instancia um objeto do tipo OrientationExtractor.
		/// </summary>
		public OrientationExtractor(SkeletonJoint joint)
		{
			this.joint = joint;
			this.dimension = 3;
			this.last = null;
		}

		/// <summary>
		/// Extrai o vetor característico de um usuário realizando um gesto. 
		/// </summary>
		override public Feature Extract(User user)
		{
			Point3D p = user.JointsPosition[joint].Position;
			if (last == null)
				last = p;
			// filtragem de proximidade: movimentos até 5cm são descartados
			Point3D o = Util.Subtract(p, last.Value);
			if (Util.Norm(o) < 50)
				return null;
			last = p;
			o = Util.Normalized(o);
			return new Feature(Util.PointToArray(o));
		}

		/// <summary>
		/// Apaga as dependências temporais para a extração dos vetores característicos.
		/// </summary>
		override public void Reset()
		{
			last = null;
		}
	}
}

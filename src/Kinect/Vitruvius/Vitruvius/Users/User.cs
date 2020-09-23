
// User.cs
// Tiago Togores - 2011

using System;
using System.Collections.Generic;
using OpenNI;

namespace Vitruvius.Users
{
	/// <summary>
	/// Representa um usuário sendo rastreado pelo Kinect.
	/// </summary>
	public class User
	{
		#region Fields and Properties

		int id;
		/// <summary>
		/// Número de identificação.
		/// </summary>
		public int ID
		{
			get
			{
				return id;
			}
		}

		private Dictionary<SkeletonJoint, SkeletonJointPosition> jointsPosition;
		/// <summary>
		/// Articulações e suas respectivas posições.
		/// </summary>
		public Dictionary<SkeletonJoint, SkeletonJointPosition> JointsPosition
		{
			get
			{
				return jointsPosition;
			}
		}

		private Dictionary<SkeletonJoint, SkeletonJointOrientation> jointsOrientation;
		/// <summary>
		/// Articulações e suas respectivas orientações (vetores do seu sistema de coordenadas).
		/// </summary>
		public Dictionary<SkeletonJoint, SkeletonJointOrientation> JointsOrientation
		{
			get
			{
				return jointsOrientation;
			}
		}

		DateTime creationDate;
		/// <summary>
		/// Data de instanciação do objeto.
		/// </summary>
		public DateTime CreationDate
		{
			get
			{
				return creationDate;
			}
		}

		bool valid;
		/// <summary>
		/// Se usuário está com dados válidos.
		/// </summary>
		public bool Valid
		{
			get
			{
				return valid;
			}
			set
			{
				valid = value;
			}
		}

		Point3D com;
		/// <summary>
		/// Centro de massa.
		/// </summary>
		public Point3D CoM
		{
			get
			{
				return com;
			}
			set
			{
				com = value;
			}
		}

		#endregion

		/// <summary>
		/// Cria um novo usuário com o id dado.
		/// </summary>
		public User(int id)
		{
			this.id = id;
			jointsPosition = new Dictionary<SkeletonJoint, SkeletonJointPosition>();
			jointsOrientation = new Dictionary<SkeletonJoint, SkeletonJointOrientation>();
			creationDate = DateTime.Now;
			valid = false;
		}

		/// <summary>
		/// Atualiza a posição e a orientação da articulação joint desse usuário de acordo com o skeletonCapability.
		/// Frame de coordenadas do mundo real.
		/// </summary>
		private void UpdateJoint(SkeletonCapability skeletonCapability, SkeletonJoint joint)
		{
			jointsPosition[joint] = skeletonCapability.GetSkeletonJointPosition(id, joint);
			jointsOrientation[joint] = skeletonCapability.GetSkeletonJointOrientation(id, joint);
			if (jointsPosition[joint].Position.Z == 0)
				this.valid = false;
		}

		/// <summary>
		/// Atualiza todas as articulações desse usuário de acordo com o skeletonCapability.
		/// </summary>
		public void UpdateJoints(SkeletonCapability skeletonCapability, Skeleton skeleton)
		{
			this.valid = true;
			foreach (SkeletonJoint skeletonJoint in skeleton.AvailableJoints.Values)
				this.UpdateJoint(skeletonCapability, skeletonJoint);
		}
	}
}

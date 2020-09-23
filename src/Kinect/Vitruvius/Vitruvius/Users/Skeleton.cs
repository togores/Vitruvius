
// Skeleton.cs
// Tiago Togores - 2011

using System;
using System.Collections.Generic;
using OpenNI;

namespace Vitruvius.Users
{
	/// <summary>
	/// Guarda as articulações e os ossos que são utilizados para rastreamento de usuários.
	/// </summary>
	public class Skeleton
	{
		private Dictionary<String, SkeletonJoint> availableJoints;
		/// <summary>
		/// Articulações disponíveis para rastreamento.
		/// </summary>
		public Dictionary<String, SkeletonJoint> AvailableJoints
		{
			get
			{
				return availableJoints;
			}
		}

		private List<Tuple<SkeletonJoint, SkeletonJoint>> availableBones;
		/// <summary>
		/// Ossos formados pelas ligações de duas articulações disponíveis.
		/// </summary>
		public List<Tuple<SkeletonJoint, SkeletonJoint>> AvailableBones
		{
			get
			{
				return availableBones;
			}
		}

		/// <summary>
		/// Adiciona a articulação joint como disponível.
		/// </summary>
		private void AddJoint(SkeletonCapability capability, SkeletonJoint joint)
		{
			if (capability.IsJointAvailable(joint))
				availableJoints.Add(joint.ToString(), joint);
		}

		/// <summary>
		/// Adiciona o osso formado pela ligação entre as articulações joint1 e joint2.
		/// </summary>
		private void AddBone(SkeletonCapability capability, SkeletonJoint joint1, SkeletonJoint joint2)
		{
			if (capability.IsJointAvailable(joint1) && capability.IsJointAvailable(joint2))
				availableBones.Add(new Tuple<SkeletonJoint, SkeletonJoint>(joint1, joint2));
		}

		/// <summary>
		/// Devolve uma instância da classe Skeleton.
		/// </summary>
		public Skeleton(SkeletonCapability s)
		{
			// articulações
			availableJoints = new Dictionary<string, SkeletonJoint>();
			AddJoint(s, SkeletonJoint.Head);
			AddJoint(s, SkeletonJoint.Neck);
			AddJoint(s, SkeletonJoint.LeftShoulder);
			AddJoint(s, SkeletonJoint.LeftElbow);
			AddJoint(s, SkeletonJoint.LeftHand);
			AddJoint(s, SkeletonJoint.RightShoulder);
			AddJoint(s, SkeletonJoint.RightElbow);
			AddJoint(s, SkeletonJoint.RightHand);
			AddJoint(s, SkeletonJoint.Torso);
			AddJoint(s, SkeletonJoint.LeftHip);
			AddJoint(s, SkeletonJoint.LeftKnee);
			AddJoint(s, SkeletonJoint.LeftFoot);
			AddJoint(s, SkeletonJoint.RightHip);
			AddJoint(s, SkeletonJoint.RightKnee);
			AddJoint(s, SkeletonJoint.RightFoot);

			availableBones = new List<Tuple<SkeletonJoint, SkeletonJoint>>();
			// cabeça
			AddBone(s, SkeletonJoint.Head, SkeletonJoint.Neck);
			// braços
			AddBone(s, SkeletonJoint.LeftShoulder, SkeletonJoint.LeftElbow);
			AddBone(s, SkeletonJoint.LeftHand, SkeletonJoint.LeftElbow);
			AddBone(s, SkeletonJoint.RightShoulder, SkeletonJoint.RightElbow);
			AddBone(s, SkeletonJoint.RightHand, SkeletonJoint.RightElbow);
			// pernas
			AddBone(s, SkeletonJoint.LeftHip, SkeletonJoint.LeftKnee);
			AddBone(s, SkeletonJoint.LeftFoot, SkeletonJoint.LeftKnee);
			AddBone(s, SkeletonJoint.RightHip, SkeletonJoint.RightKnee);
			AddBone(s, SkeletonJoint.RightFoot, SkeletonJoint.RightKnee);
			// tronco
			AddBone(s, SkeletonJoint.RightShoulder, SkeletonJoint.Torso);
			AddBone(s, SkeletonJoint.LeftShoulder, SkeletonJoint.Torso);
			AddBone(s, SkeletonJoint.LeftShoulder, SkeletonJoint.Neck);
			AddBone(s, SkeletonJoint.RightShoulder, SkeletonJoint.Neck);
			AddBone(s, SkeletonJoint.RightHip, SkeletonJoint.Torso);
			AddBone(s, SkeletonJoint.LeftHip, SkeletonJoint.Torso);
			AddBone(s, SkeletonJoint.RightHip, SkeletonJoint.LeftHip);
		}
	}
}

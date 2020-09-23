
// UserManager.cs
// Tiago Togores - 2011

using System;
using System.Collections.Generic;
using OpenNI;
using Vitruvius.Users.Focus;

namespace Vitruvius.Users
{
	/// <summary>
	/// Gerenciador de usuários.
	/// </summary>
	public class UserManager
	{
		#region Fields and Properties

		private UserGenerator userGenerator;
		/// <summary>
		/// Classe do OpenNI que faz o controle dos eventos relacionados a usuários.
		/// </summary>
		public UserGenerator UserGenerator
		{
			get
			{
				return userGenerator;
			}
		}

		/// <summary>
		/// Classe do OpenNI que controla os dados dos esqueletos dos usuários.
		/// </summary>
		private SkeletonCapability skeletonCapability;

		private Skeleton skeleton;
		/// <summary>
		/// Descrição dos esqueletos dos usuários.
		/// </summary>
		public Skeleton Skeleton
		{
			get
			{
				return skeleton;
			}
			set
			{
				skeleton = value;
			}
		}

		/// <summary>
		/// Classe do OpenNI que controla as poses.
		/// </summary>
		private PoseDetectionCapability poseDetectionCapability;

		private Dictionary<int, User> users;
		/// <summary>
		/// Usuários detectados.
		/// </summary>
		public Dictionary<int, User> Users
		{
			get
			{
				return users;
			}
		}

		private Dictionary<String, IFocus> focus;
		/// <summary>
		/// Algoritmos utilizados para foco de atenção.
		/// </summary>
		public Dictionary<String, IFocus> Focus
		{
			get
			{
				return focus;
			}
			set
			{
				focus = value;
			}
		}

		/// <summary>
		/// Algoritmo de foco sendo usado no momento.
		/// </summary>
		private IFocus currentFocus;

		#endregion

		#region Calibration

		/// <summary>
		/// Callback chamado quando um novo usuário é detectado.
		/// Precisa calibrar.
		/// </summary>
		private void NewUser(object sender, NewUserEventArgs e)
		{
			int id = e.ID;
			Controller.Instance.Print("User Found: " + id + " - Looking for pose");
			this.userGenerator.PoseDetectionCapability.StartPoseDetection(this.userGenerator.SkeletonCapability.CalibrationPose, id);
		}

		/// <summary>
		/// Callback chamado quando a pose de calibrar é detectada.
		/// </summary>
		private void PoseDetected(object sender, PoseDetectedEventArgs e)
		{
			int id = e.ID;
			this.userGenerator.PoseDetectionCapability.StopPoseDetection(id);
			Controller.Instance.Print("Pose detected: " + id + " - Requesting Calibration");
			this.userGenerator.SkeletonCapability.RequestCalibration(id, true);
		}

		/// <summary>
		/// Callback chamado quando a pose de calibrar é desfeita.
		/// </summary>
		private void OutOfPose(object sender, OutOfPoseEventArgs e)
		{
			Controller.Instance.Print("Out of Pose: " + e.ID);
		}

		/// <summary>
		/// Callback chamado quando a calibragem é iniciada.
		/// </summary>
		private void CalibrationStart(object sender, CalibrationStartEventArgs e)
		{
			Controller.Instance.Print("Calibration Started: " + e.ID);
		}

		/// <summary>
		/// Callback chamado quando a calibragem é finalizada.
		/// </summary>
		private void CalibrationComplete(object sender, CalibrationProgressEventArgs e)
		{
			int id = e.ID;
			if (e.Status == CalibrationStatus.OK) {
				Controller.Instance.Print("Calibration Completed: " + id);
				this.userGenerator.SkeletonCapability.StartTracking(id);
				this.users[id] = new User(id);
			}
			else {
				Controller.Instance.Print("Calibration Failed: " + id);
				this.userGenerator.PoseDetectionCapability.StartPoseDetection(this.userGenerator.SkeletonCapability.CalibrationPose, id);
			}
		}

		/// <summary>
		/// Callback chamado quando um usuário existente desaparece da cena.
		/// </summary>
		private void LostUser(object sender, UserLostEventArgs e)
		{
			int id = e.ID;
			Controller.Instance.Print("User lost: " + id);
			this.users.Remove(id);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Instancia um objeto da classe UserManager a partir de context.
		/// </summary>
		public UserManager(Context context)
		{
			userGenerator = new UserGenerator(context);
			poseDetectionCapability = userGenerator.PoseDetectionCapability;
			skeletonCapability = userGenerator.SkeletonCapability;
			skeletonCapability.SetSkeletonProfile(SkeletonProfile.All);
			skeletonCapability.SetSmoothing(0.5f);
			skeleton = new Skeleton(skeletonCapability);
			users = new Dictionary<int, User>();

			userGenerator.NewUser += NewUser;
			poseDetectionCapability.PoseDetected += PoseDetected;
			poseDetectionCapability.OutOfPose += OutOfPose;
			skeletonCapability.CalibrationStart += CalibrationStart;
			skeletonCapability.CalibrationComplete += CalibrationComplete;
			userGenerator.LostUser += LostUser;

			focus = new Dictionary<string, IFocus>();
			focus.Add("Closest", new FocusClosest());
			focus.Add("Farthest", new FocusFarthest());
			focus.Add("Oldest", new FocusOldest());
			focus.Add("Newest", new FocusNewest());
			focus.Add("None", new FocusNone());
			currentFocus = focus["None"];
		}

		#endregion

		#region Methods

		/// <summary>
		/// Atualiza todos os usuários.
		/// </summary>
		public void UpdateAllUsers()
		{
			foreach (int id in users.Keys)
				UpdateUser(id);
		}

		/// <summary>
		/// Atualiza as articulações e o centro de massa do usuário identificado por id.
		/// </summary>
		public void UpdateUser(int id)
		{
			users[id].UpdateJoints(userGenerator.SkeletonCapability, skeleton);
			users[id].CoM = userGenerator.GetCoM(id);
		}

		/// <summary>
		/// Troca o algoritmo de foco para o algoritmo de nome s.
		/// </summary>
		public void SetFocus(String s)
		{
			if (s != null && focus.ContainsKey(s))
				currentFocus = focus[s];
			else
				currentFocus = focus["None"];
			currentFocus.Reset();
		}

		/// <summary>
		/// Atualiza e devolve quem é o usuário principal.
		/// </summary>
		public User UpdateMainUser()
		{
			return currentFocus.UpdateMainUser(users);
		}

		/// <summary>
		/// Devolve uma string que descreve as condições dos usuários (principal, válido, inválido)
		/// </summary>
		public string GetUsersLabel()
		{
			return currentFocus.GetUsersLabel(users);
		}

		#endregion
	}
}

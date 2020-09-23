
// DeviceImage.cs
// Tiago Togores - 2011

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Media.Imaging;
using OpenNI;
using Vitruvius.Users;

namespace Vitruvius.Device
{
	/// <summary>
	/// Transforma dados gerados por um dispositivo visual qualquer em imagem.
	/// </summary>
	public abstract class DeviceImage
	{
		#region Fields and Properties

		/// <summary>
		/// Se deve inverter a imagem horizontalmente.
		/// </summary>
		protected bool mirror;

		/// <summary>
		/// Se deve desenhar o plano de fundo (imagem obtida do dispositivo).
		/// </summary>
		public bool ShouldDrawBackground { get; set; }

		/// <summary>
		/// Se deve imprimir id dos usuários.
		/// </summary>
		public bool ShouldPrintID { get; set; }

		/// <summary>
		/// Se deve imprimir estado dos usuários.
		/// </summary>
		public bool ShouldPrintState { get; set; }

		/// <summary>
		/// Se deve desenhar esqueleto dos usuários.
		/// </summary>
		public bool ShouldDrawSkeleton { get; set; }

		protected static WriteableBitmap writeableBitmap;
		/// <summary>
		/// Bitmap onde a imagem do dispositivo é guardada.
		/// </summary>
		public static WriteableBitmap WriteableBitmap
		{
			get
			{
				return writeableBitmap;
			}
		}

		/// <summary>
		/// Cores para desenho dos labels (id + status) e dos esqueletos.
		/// </summary>
		protected Color[] colors = { Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.Pink, Color.Beige};

		#endregion

		#region Constructors

		static DeviceImage()
		{
			writeableBitmap = new WriteableBitmap(640, 480, 96, 96, System.Windows.Media.PixelFormats.Rgb24, null);
		}

		/// <summary>
		/// Instancia um objeto do tipo DeviceImage com valores padrão.
		/// </summary>
		public DeviceImage()
		{
			this.ShouldDrawBackground = true;
			this.ShouldDrawSkeleton = true;
			this.ShouldPrintID = true;
			this.ShouldPrintState = true;
			this.mirror = true;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Desenha em g uma linha que liga joints[j1] e joints[j2] com cor color.
		/// </summary>
		protected void DrawLine(Graphics g, Color color, Dictionary<SkeletonJoint, SkeletonJointPosition> joints, SkeletonJoint j1, SkeletonJoint j2)
		{
			if (joints.Count == 0 || joints[j1].Confidence == 0 || joints[j2].Confidence == 0)
				return;

			Point3D pos1 = Controller.Instance.ConvertRealWorldToProjective(joints[j1].Position);
			Point3D pos2 = Controller.Instance.ConvertRealWorldToProjective(joints[j2].Position);

			g.DrawLine(new Pen(color),
						new System.Drawing.Point((int)pos1.X, (int)pos1.Y),
						new System.Drawing.Point((int)pos2.X, (int)pos2.Y));
		}

		/// <summary>
		/// Desenha em g o esqueleto formado por joints com cor color.
		/// </summary>
		protected void DrawSkeleton(Graphics g, Color color, Dictionary<SkeletonJoint, SkeletonJointPosition> joints, Skeleton armature)
		{
			foreach (Tuple<SkeletonJoint, SkeletonJoint> bone in armature.AvailableBones)
				DrawLine(g, color, joints, bone.Item1, bone.Item2);
		}

		/// <summary>
		/// Desenha os esqueletos dos usuários.
		/// </summary>
		protected void DrawSkeletons(Graphics drawing, UserManager userController)
		{
			SkeletonCapability skeletonCapability = userController.UserGenerator.SkeletonCapability;

			foreach (int userID in userController.Users.Keys) {
				if (ShouldDrawSkeleton && skeletonCapability.IsTracking(userID)) {
					DrawSkeleton(drawing, colors[userID % colors.Length], userController.Users[userID].JointsPosition, userController.Skeleton);
				}
			}
		}

		/// <summary>
		/// Desenha os labels dos usuários.
		/// </summary>
		protected void DrawLabels(Graphics drawing, UserManager userController)
		{
			SkeletonCapability skeletonCapability = userController.UserGenerator.SkeletonCapability;

			foreach (int userID in userController.Users.Keys) {
				string label = string.Empty;
				Point3D com = userController.Users[userID].CoM;
				com = Controller.Instance.ConvertRealWorldToProjective(com);
				if (ShouldPrintID) {
					label += userID.ToString();
					if (ShouldPrintState)
						label += " - ";
				}
				if (ShouldPrintState) {
					if (skeletonCapability.IsTracking(userID))
						label += "Tracking";
					else if (skeletonCapability.IsCalibrating(userID))
						label += "Calibrating";
					else if (skeletonCapability.IsCalibrated(userID))
						label += "Calibrated";
					else
						label += "Looking for pose";
				}
				if (ShouldPrintID || ShouldPrintState) {
					int x;
					if (mirror)
						x = (int)com.X;
					else
						x = 480 - (int)com.X;
					drawing.DrawString(label, new Font("Arial", 12), new SolidBrush(colors[userID % colors.Length]), x, com.Y);
				}
			}
		}

		/// <summary>
		/// Desenha os esqueletos e labels dos usuários.
		/// </summary>
		private void DrawSkeletonsAndLabels(UserManager userController)
		{
			Bitmap bitmap = new Bitmap(writeableBitmap.PixelWidth, writeableBitmap.PixelHeight,
				writeableBitmap.BackBufferStride, System.Drawing.Imaging.PixelFormat.Format24bppRgb, writeableBitmap.BackBuffer);
			Graphics graphics = Graphics.FromImage(bitmap);
			DrawSkeletons(graphics, userController);
			DrawLabels(graphics, userController);
		}

		/// <summary>
		/// Desenha um frame de imagem de deviceController.
		/// </summary>
		abstract protected void DrawBackground(DeviceController deviceController);

		/// <summary>
		/// Desenha um frame com a imagem de deviceController e sobrepõe os usuários de userController.
		/// </summary>
		public void DrawFrame(DeviceController deviceController, UserManager userController)
		{
			DrawBackground(deviceController);
			DrawSkeletonsAndLabels(userController);
		}

		/// <summary>
		/// Desenha uma imagem vazia (todos os pixels pretos).
		/// </summary>
		public static void DrawFrame()
		{
			writeableBitmap.Lock();
			writeableBitmap.WritePixels(new Int32Rect(0, 0, 640, 480), new int[640 * 480], writeableBitmap.BackBufferStride, 0);
			writeableBitmap.AddDirtyRect(new Int32Rect(0, 0, 640, 480));
			writeableBitmap.Unlock();
		}

		#endregion
	}
}

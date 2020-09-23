
// Recorder.cs
// Tiago Togores - 2011

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using OpenNI;
using Vitruvius.Users;

namespace Vitruvius
{
	/// <summary>
	/// Faz a gravação/leitura de gestos de um arquivo.
	/// </summary>
	class Recorder
	{
		/// <summary>
		/// string que representa uma nova linha no Windows.
		/// </summary>
		private static string newline = "\r\n";

		/// <summary>
		/// dados armazenados para escrita em arquivo.
		/// </summary>
		List<Tuple<string, SkeletonJointPosition>> data;

		/// <summary>
		/// leitor do arquivo.
		/// </summary>
		private StreamReader reader;

		private bool reading;
		/// <summary>
		/// Leitura em andamento.
		/// </summary>
		public bool Reading
		{
			get { return reading; }
		}

		private bool recording;
		/// <summary>
		/// Gravação em andamento.
		/// </summary>
		public bool Recording
		{
			get { return recording; }
		}

		/// <summary>
		/// Instancia um novo objeto do tipo Recorder.
		/// </summary>
		public Recorder()
		{
			reading = false;
			recording = false;
			data = new List<Tuple<string, SkeletonJointPosition>>();
		}

		#region Read

		/// <summary>
		/// Inicia leitura de um arquivo.
		/// </summary>
		public void StartReading(string filename)
		{
			reading = true;
			reader = new StreamReader(filename);
		}

		/// <summary>
		/// Faz a leitura da próxima pose do usuário.
		/// </summary>
		public bool NextPose(User user, Skeleton skeleton)
		{
			if (user != null && reading) {
				if (reader.EndOfStream)
					return false;
				
				var line = reader.ReadLine().Split(' ', '\r');

				SkeletonJoint joint = SkeletonJoint.RightHand;
				var jointPosition = new SkeletonJointPosition();
				var point = new Point3D();
				point.X = float.Parse(line[0]);
				point.Y = float.Parse(line[1]);
				point.Z = float.Parse(line[2]);
				jointPosition.Position =  point;
				user.JointsPosition[joint] = jointPosition;
			}
			return true;
		}

		/// <summary>
		/// Termina a leitura de um arquivo.
		/// </summary>
		public void StopReading()
		{
			if (reading) {
				reading = false;
				reader.Dispose();
			}
		}

		#endregion

		#region Record

		/// <summary>
		/// Inicia a gravação.
		/// </summary>
		public void StartRecord()
		{
			recording = true;
			data.Clear();
		}

		/// <summary>
		/// Captura a pose do usuário no momento e salva na lista.
		/// </summary>
		public void Capture(User user, Skeleton skeleton)
		{
			if (user != null && recording) {
				SkeletonJoint joint = SkeletonJoint.RightHand;
				data.Add(new Tuple<string, SkeletonJointPosition>("RightHand", user.JointsPosition[joint]));
			}
		}

		/// <summary>
		/// Termina a gravação
		/// </summary>
		public void StopRecord()
		{
			recording = false;
			data.Clear();
		}

		/// <summary>
		/// Salva os dados em um arquivo.
		/// </summary>
		public void Save(string filename)
		{
			using (Stream stream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None)) {
				stream.Position = 0;
				foreach (var x in this.data) {
					var p = x.Item2.Position;
					string record = p.X.ToString("R") + " " + p.Y.ToString("R") + " " + p.Z.ToString("R") + newline;
					var bytes = UnicodeEncoding.UTF8.GetBytes(record);
					stream.Write(bytes, 0, bytes.Length);
				}
			}
		}

		#endregion
	}
}

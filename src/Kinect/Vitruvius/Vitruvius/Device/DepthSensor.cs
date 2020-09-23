
// DepthSensor.cs
// Tiago Togores - 2011

using System;
using OpenNI;

namespace Vitruvius.Device
{
	/// <summary>
	/// Controla o sensor de profundidade.
	/// </summary>
	public class DepthSensor : DeviceController
	{
		/// <summary>
		/// Gerador de dados de profundidade.
		/// </summary>
		public DepthGenerator Generator
		{
			get
			{
				return generator as DepthGenerator;
			}
		}

		/// <summary>
		/// Dados de profundidade.
		/// </summary>
		public DepthMetaData Metadata
		{
			get
			{
				return metadata as DepthMetaData;
			}
		}

		private int[] histogram;
		/// <summary>
		/// Histograma acumulado relativo de profundidades.
		/// </summary>
		public int[] Histogram
		{
			get
			{
				return histogram;
			}
		}

		/// <summary>
		/// Instancia um objeto do tipo DepthSensor associado ao Context context.
		/// </summary>
		public DepthSensor(Context context)
		{
			this.generator = new DepthGenerator(context);
			this.generator.MirrorCapability.SetMirror(true); // dados serão espelhados
			MapOutputMode mapOutputMode = new MapOutputMode();
			mapOutputMode.FPS = 30; // frames por segundo
			mapOutputMode.XRes = 640; // resolução
			mapOutputMode.YRes = 480;
			this.generator.MapOutputMode = mapOutputMode;
			this.metadata = new DepthMetaData();
			this.histogram = new int[this.Generator.DeviceMaxDepth];
		}

		/// <summary>
		/// Atualiza os dados enviados pelo sensor.
		/// </summary>
		override public void UpdateMetadata()
		{
			Generator.GetMetaData(Metadata);
		}

		/// <summary>
		/// Atualiza o histograma de profundidades.
		/// </summary>
		public unsafe void UpdateHistogram()
		{
			for (int i = 0; i < histogram.Length; i++)
				histogram[i] = 0;

			ushort* pDepth = (ushort*)Metadata.DepthMapPtr.ToPointer();

			// calcula histograma
			int validPixels = 0;
			for (int y = 0; y < Metadata.YRes; y++) {
				for (int x = 0; x < Metadata.XRes; x++, pDepth++) {
					ushort depthVal = *pDepth;
					if (depthVal != 0) {
						histogram[depthVal]++;
						validPixels++;
					}
				}
			}

			// calcula histograma acumulado
			for (int i = 1; i < histogram.Length; i++)
				histogram[i] += histogram[i - 1];

			// calcula histograma acumulado relativo
			if (validPixels > 0) {
				for (int i = 1; i < histogram.Length; i++)
					histogram[i] = (int)(256 * (1.0f - (histogram[i] / (float)validPixels)));
			}
		}
	}
}

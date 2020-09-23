
// RGBCamera.cs
// Tiago Togores - 2011

using OpenNI;

namespace Vitruvius.Device
{
	/// <summary>
	/// Controla a câmera RGB.
	/// </summary>
	public class RGBCamera : DeviceController
	{
		/// <summary>
		/// Gerador de dados de cor.
		/// </summary>
		public ImageGenerator Generator
		{
			get
			{
				return generator as ImageGenerator;
			}
		}

		/// <summary>
		/// Dados de cor.
		/// </summary>
		public ImageMetaData Metadata
		{
			get
			{
				return metadata as ImageMetaData;
			}
		}

		/// <summary>
		/// Instancia um objeto do tipo RGBCamera associado ao Context context.
		/// </summary>
		public RGBCamera(Context context)
		{
			this.generator = new ImageGenerator(context);
			this.generator.MirrorCapability.SetMirror(true); // dados serão espelhados
			MapOutputMode mapOutputMode = new MapOutputMode();
			mapOutputMode.FPS = 30; // frames por segundo
			mapOutputMode.XRes = 640; // resolução
			mapOutputMode.YRes = 480;
			this.generator.MapOutputMode = mapOutputMode;
			this.metadata = new ImageMetaData();
		}

		/// <summary>
		/// Atualiza os dados enviados pela câmera.
		/// </summary>
		override public void UpdateMetadata()
		{
			Generator.GetMetaData(Metadata);
		}
	}
}

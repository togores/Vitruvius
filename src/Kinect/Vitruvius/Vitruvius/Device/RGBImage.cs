
// RGBImage.cs
// Tiago Togores - 2011

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;
using OpenNI;

namespace Vitruvius.Device
{
	/// <summary>
	/// Transforma dados da câmera RGB em imagem.
	/// </summary>
	public class RGBImage : DeviceImage
	{
		/// <summary>
		/// Instancia um objeto do tipo RGBImage.
		/// </summary>
		public RGBImage()
			: base()
		{
		}

		/// <summary>
		/// Desenha um frame de imagem de deviceController
		/// se for possível fazer o casting para RGBCamera.
		/// </summary>
		override protected void DrawBackground(DeviceController deviceController)
		{
			RGBCamera rgbCamera = deviceController as RGBCamera;
			if (rgbCamera == null)
				return;
			ImageMetaData md = rgbCamera.Metadata;
			writeableBitmap.Lock();
			if (ShouldDrawBackground)
				writeableBitmap.WritePixels(new Int32Rect(0, 0, md.XRes, md.YRes), md.ImageMapPtr, (int)md.DataSize, writeableBitmap.BackBufferStride);
			else
				writeableBitmap.WritePixels(new Int32Rect(0, 0, 640, 480), new int[640 * 480], writeableBitmap.BackBufferStride, 0);
			writeableBitmap.AddDirtyRect(new Int32Rect(0, 0, md.XRes, md.YRes));
			writeableBitmap.Unlock();
		}
	}
}


// DepthImage.cs
// Tiago Togores - 2011

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;
using OpenNI;

namespace Vitruvius.Device
{
	/// <summary>
	/// Transforma dados do sensor de profundidade em imagem.
	/// </summary>
	public class DepthImage : DeviceImage
	{
		/// <summary>
		/// Instancia um objeto do tipo DepthImage.
		/// </summary>
		public DepthImage()
			: base()
		{
		}

		/// <summary>
		/// Desenha um frame de imagem de deviceController
		/// se for possível fazer o casting para DepthSensor.
		/// </summary>
		override protected void DrawBackground(DeviceController cameraController)
		{
			DepthSensor depthSensor = cameraController as DepthSensor;
			if (depthSensor == null)
				return;
			writeableBitmap.Lock();
			if (ShouldDrawBackground) {
				unsafe {
					ushort* pDepth = (ushort*)depthSensor.Generator.DepthMapPtr.ToPointer();
					for (int y = 0; y < depthSensor.Metadata.YRes; y++) {
						byte* pDest = (byte*)writeableBitmap.BackBuffer.ToPointer() + y * writeableBitmap.BackBufferStride;
						for (int x = 0; x < depthSensor.Metadata.XRes; x++, pDepth++, pDest += 3) {
							pDest[0] = pDest[1] = pDest[2] = 0;
							byte pixel = (byte)(depthSensor.Histogram[*pDepth]);
							Color labelColor = Color.White;
							pDest[0] = (byte)(pixel * (labelColor.B / 256.0));
							pDest[1] = (byte)(pixel * (labelColor.G / 256.0));
							pDest[2] = (byte)(pixel * (labelColor.R / 256.0));
						}
					}
				}
			}
			else
				writeableBitmap.WritePixels(new Int32Rect(0, 0, 640, 480), new int[640 * 480], writeableBitmap.BackBufferStride, 0);
			writeableBitmap.AddDirtyRect(new Int32Rect(0, 0, 640, 480));
			writeableBitmap.Unlock();
		}
	}
}

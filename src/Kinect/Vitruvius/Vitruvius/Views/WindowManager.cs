
// WindowManager.cs
// Tiago Togores - 2011

using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace Vitruvius.Views
{
	/// <summary>
	/// Gerenciador de janelas.
	/// </summary>
	public class WindowManager
	{
		private MainWindow mainWindow;
		/// <summary>
		/// Janela principal do programa.
		/// </summary>
		public MainWindow MainWindow
		{
			get
			{
				if (mainWindow == null) {
					mainWindow = new MainWindow();
					Icon icon = Vitruvius.Properties.Resources.vitruvius;
					mainWindow.Icon = Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
				}
				return mainWindow;
			}
		}
	}
}

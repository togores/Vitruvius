
// App.xaml.cs
// Tiago Togores - 2011

using System.Windows;

namespace Vitruvius.Views
{
	/// <summary>
	/// Classe que inicia a execução do programa.
	/// </summary>
	public partial class App : Application
	{
		[System.STAThreadAttribute()]
		public static void Main()
		{
			App app = new App();
			Controller controller = Controller.Instance;
			if (controller != null) {
				MainWindow w = controller.WindowManager.MainWindow;
				w.Show();
			}
			app.InitializeComponent();
			app.Run();
		}
	}
}

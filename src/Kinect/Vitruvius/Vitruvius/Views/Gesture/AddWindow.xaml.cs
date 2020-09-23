using System.Windows;

namespace Vitruvius.Views.Gesture
{
	/// <summary>
	/// Interaction logic for AddWindow.xaml
	/// </summary>
	public partial class AddWindow : Window
	{
		public AddWindow()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Adiciona um gesto e fecha a janela.
		/// </summary>
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			if (!string.IsNullOrWhiteSpace(box.Text)) {
				Controller.Instance.GestureManager.GetGesture(box.Text);
				this.Close();
			}
		}
	}
}

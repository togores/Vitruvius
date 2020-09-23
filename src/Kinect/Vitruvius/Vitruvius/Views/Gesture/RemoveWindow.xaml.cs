using System.Windows;

namespace Vitruvius.Views.Gesture
{
	/// <summary>
	/// Interaction logic for RemoveWindow.xaml
	/// </summary>
	public partial class RemoveWindow : Window
	{
		public RemoveWindow()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Remove um gesto e fecha a janela.
		/// </summary>
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			if (!string.IsNullOrWhiteSpace(box.Text)) {
				Controller.Instance.GestureManager.RemoveGesture(box.Text);
				this.Close();
			}
		}
	}
}

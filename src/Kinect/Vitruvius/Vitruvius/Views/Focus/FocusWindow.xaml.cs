
// FocusWindow.xaml.cs
// Tiago Togores - 2011

using System.Windows;
using System.Windows.Controls;

namespace Vitruvius.Views
{
	/// <summary>
	/// Janela das configurações do foco de atenção.
	/// </summary>
	public partial class FocusWindow : Window
	{
		private Controller controller;

		public FocusWindow()
		{
			this.controller = Controller.Instance;
			InitializeComponent();
		}

		/// <summary>
		/// Atualiza o valor do timeout do foco.
		/// </summary>
		private void TimeoutChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			controller.setUserTimeout((((Slider)sender).Name).Replace("TimeoutSlider", ""),
				(int)((Slider)sender).Value);
		}

		/// <summary>
		/// Atualiza o valor do joint de referência.
		/// </summary>
		private void ReferenceJointChanged(object sender, SelectionChangedEventArgs e)
		{
			controller.SetReferenceJoint((((ComboBox)sender).Name).Replace("ReferenceJointComboBox", ""),
				((string)((ComboBoxItem)(((ComboBox)sender).SelectedValue)).Content).Replace(" ", ""));
		}
	}
}

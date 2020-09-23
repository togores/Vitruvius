using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Vitruvius.Views.Gesture
{
	/// <summary>
	/// Interaction logic for SelectWindow.xaml
	/// </summary>
	public partial class SelectWindow : Window
	{
		public SelectWindow()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Seleciona um gesto para treinamento e fecha a janela.
		/// </summary>
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			if (!string.IsNullOrWhiteSpace(box.Text)) {
				Controller.Instance.SetTraining(box.Text);
				this.DialogResult = true;
				this.Close();
			}
		}
	}
}

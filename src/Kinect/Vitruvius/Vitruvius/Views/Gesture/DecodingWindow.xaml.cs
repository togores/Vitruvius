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
	/// Interaction logic for DecodingWindow.xaml
	/// </summary>
	public partial class DecodingWindow : Window
	{
		Controller controller;

		public DecodingWindow()
		{
			controller = Controller.Instance;
			InitializeComponent();
		}

		/// <summary>
		/// Inicia o reconhecimento de um gesto.
		/// </summary>
		private void startButton_Click(object sender, RoutedEventArgs e)
		{
			recordStatusTextBlock.Text = "Recognizing...";
			this.startButton.IsEnabled = false;
			this.stopButton.IsEnabled = true;
			controller.StartRecognizing();
		}

		/// <summary>
		/// Termina o reconhecimento de um gesto.
		/// </summary>
		private void stopButton_Click(object sender, RoutedEventArgs e)
		{
			recordStatusTextBlock.Text = "Idle";
			this.startButton.IsEnabled = true;
			this.stopButton.IsEnabled = false;
			controller.StopDecoding();
		}
	}
}

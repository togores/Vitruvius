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
	/// Interaction logic for RecordWindow.xaml
	/// </summary>
	public partial class RecordWindow : Window
	{
		Controller controller;

		public RecordWindow()
		{
			controller = Controller.Instance;
			InitializeComponent();
		}

		/// <summary>
		/// Inicia a gravação de um gesto.
		/// </summary>
		private void startButton_Click(object sender, RoutedEventArgs e)
		{
			recordStatusTextBlock.Text = "Recording...";
			this.startButton.IsEnabled = false;
			this.stopButton.IsEnabled = true;
			controller.StartRecording();
		}

		/// <summary>
		/// Termina a gravação de um gesto.
		/// </summary>
		private void stopButton_Click(object sender, RoutedEventArgs e)
		{
			recordStatusTextBlock.Text = "Idle";
			this.startButton.IsEnabled = true;
			this.stopButton.IsEnabled = false;
			controller.StopRecording();
		}
	}
}


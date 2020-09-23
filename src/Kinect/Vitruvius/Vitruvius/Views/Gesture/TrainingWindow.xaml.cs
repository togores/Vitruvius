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
	/// Interaction logic for TrainingWindow.xaml
	/// </summary>
	public partial class TrainingWindow : Window
	{
		Controller controller;

		public TrainingWindow()
		{
			controller = Controller.Instance;
			InitializeComponent();
		}

		/// <summary>
		/// Inicia o treinamento de um gesto.
		/// </summary>
		private void startButton_Click(object sender, RoutedEventArgs e)
		{
			recordStatusTextBlock.Text = "Training...";
			this.startButton.IsEnabled = false;
			this.stopButton.IsEnabled = true;
			controller.StartTraining();
		}

		/// <summary>
		/// Finaliza o treinamento de um gesto.
		/// </summary>
		private void stopButton_Click(object sender, RoutedEventArgs e)
		{
			recordStatusTextBlock.Text = "Idle";
			this.startButton.IsEnabled = true;
			this.stopButton.IsEnabled = false;
			controller.StopTraining();
		}
	}
}


// GestureWindow.cs
// Tiago Togores - 2011

using System.Windows;
using System.Windows.Controls;
using Vitruvius.Gestures;

namespace Vitruvius.Views
{
	/// <summary>
	/// Janela que mostra os gestos e quantas vezes foram treinados.
	/// </summary>
	public partial class GestureWindow : Window
	{
		public GestureWindow()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Quando a janela é carregada, a lista dos gestos é adicionada a ela.
		/// </summary>
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			ItemCollection nameItems = this.nameListBox.Items;
			ItemCollection trainingSet = this.trainingListBox.Items;
			nameItems.Add("Name");
			trainingSet.Add("Training Counter");
			foreach (Vitruvius.Gestures.Gesture gesture in Controller.Instance.GestureManager.Gestures.Values) {
				nameItems.Add(gesture.Name);
				trainingSet.Add(gesture.TrainingCounter);
			}
			nameItems.Refresh();
			trainingSet.Refresh();
		}
	}
}

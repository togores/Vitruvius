
// MainWindow.xaml.cs
// Tiago Togores - 2011

using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using Vitruvius.Gestures;
using Vitruvius.Views.Gesture;

namespace Vitruvius.Views
{
	/// <summary>
	/// Janela principal do programa, onde ocorre a maior parte das interações.
	/// </summary>
	public partial class MainWindow : Window
	{
		private Controller controller;

		/// <summary>
		/// Contrutor padrão que inicializa os componentes da interface.
		/// </summary>
		public MainWindow()
		{
			this.controller = Controller.Instance;
			InitializeComponent();
		}

		#region Window Events
		// eventos associados à janela

		/// <summary>
		/// Callback chamado quando a janela é carregada.
		/// Inicia a execução do controller.
		/// </summary>
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			controller.Start();
		}

		/// <summary>
		/// Fecha todas as janelas pertencentes à aplicação atual.
		/// </summary>
		private void CloseAllWindows()
		{
			for (int intCounter = App.Current.Windows.Count - 1; intCounter > 0; intCounter--)
				App.Current.Windows[intCounter].Close();
		}

		/// <summary>
		/// Callback chamado quando a janela está sendo fechada.
		/// Termina a execução do programa se o usuário não cancelar.
		/// </summary>
		private void Window_Closing(object sender, CancelEventArgs e)
		{
			if (MessageBox.Show("Are you sure?", "Quit", MessageBoxButton.YesNo) == MessageBoxResult.Yes) {
				e.Cancel = false;
				CloseAllWindows();
				controller.Stop();
				Application.Current.Shutdown();
			}
			else {
				e.Cancel = true;
			}
		}

		#endregion

		/// <summary>
		/// Abre a janela como um dialog.
		/// </summary>
		private bool? NewDialog(Window window)
		{
			if (window != null) {
				window.Owner = this;
				return window.ShowDialog();
			}
			return false;
		}

		#region File Events

		/// <summary>
		/// Importa gestos de arquivos.
		/// </summary>
		private void importMenuItem_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.Title = "Import Gesture";
			dialog.ValidateNames = true;
			dialog.AddExtension = true;
			dialog.DefaultExt = ".gst";
			dialog.CheckPathExists = true;
			dialog.Multiselect = true;
			if (dialog.ShowDialog() == true)
				controller.ImportGesture(dialog.FileNames);
		}

		/// <summary>
		/// Exporta um gesto.
		/// </summary>
		private void exportMenuItem_Click(object sender, RoutedEventArgs e)
		{
			var w = new ExportWindow();
			if (NewDialog(w) == true) {
				string name = (string)w.Tag;
				SaveFileDialog dialog = new SaveFileDialog();
				dialog.Title = "Export Gesture";
				dialog.ValidateNames = true;
				dialog.AddExtension = true;
				dialog.DefaultExt = ".gst";
				dialog.CheckPathExists = true;
				dialog.CreatePrompt = false;
				dialog.OverwritePrompt = true;
				if (dialog.ShowDialog() == true) {
					controller.ExportGesture(name, dialog.FileName);
				}
			}
		}

		/// <summary>
		/// Fecha a janela.
		/// </summary>
		private void quitMenuItem_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		#endregion

		#region View Events
		// eventos associados ao menu View

		private void rgbRadioButton_Checked(object sender, RoutedEventArgs e)
		{
			controller.ViewRGBImage();
		}

		private void depthRadioButton_Checked(object sender, RoutedEventArgs e)
		{
			controller.ViewDepthImage();
		}

		private void noneRadioButton_Checked(object sender, RoutedEventArgs e)
		{
			controller.ViewEmptyImage();
		}

		private void backgroundMenuItem_Checked(object sender, RoutedEventArgs e)
		{
			controller.ShowBackground();
		}

		private void backgroundMenuItem_Unchecked(object sender, RoutedEventArgs e)
		{
			controller.HideBackground();
		}

		private void idMenuItem_Checked(object sender, RoutedEventArgs e)
		{
			controller.ShowID();
		}

		private void idMenuItem_Unchecked(object sender, RoutedEventArgs e)
		{
			controller.HideID();
		}

		private void skeletonMenuItem_Checked(object sender, RoutedEventArgs e)
		{
			controller.ShowSkeleton();
		}

		private void skeletonMenuItem_Unchecked(object sender, RoutedEventArgs e)
		{
			controller.HideSkeleton();
		}

		private void stateMenuItem_Checked(object sender, RoutedEventArgs e)
		{
			controller.ShowState();
		}

		private void stateMenuItem_Unchecked(object sender, RoutedEventArgs e)
		{
			controller.HideState();
		}

		#endregion

		#region Focus Events
		// eventos associados ao menu Focus

		/// <summary>
		/// Troca o algoritmo de foco.
		/// </summary>
		private void FocusChanged(object sender, RoutedEventArgs e)
		{
			String content = (string)((RadioButton)sender).Content;
			this.controller.ChangeAttentionFocus(content);
		}

		/// <summary>
		/// Abre a janela de configuração do algoritmo de foco.
		/// </summary>
		private void OpenFocusWindow(object sender, RoutedEventArgs e)
		{
			NewDialog(new FocusWindow());
		}

		#endregion

		#region Gesture Events
		// eventos associados ao menu Gesture

		/// <summary>
		/// Ativa/desativa o submenu.
		/// </summary>
		private void SetMenuItemEnabled(MenuItem menuItem, bool boolean)
		{
			if (menuItem != null && menuItem.IsLoaded)
				menuItem.IsEnabled = boolean;
		}

		/// <summary>
		/// Ativa a opção de gravação quando os gestos são tempo real.
		/// </summary>
		private void onlineRadioButton_Checked(object sender, RoutedEventArgs e)
		{
			SetMenuItemEnabled(gestureRecordSubmenu, true);
		}

		/// <summary>
		/// Desativa a opção de gravação quando os gestos são lidos de arquivos.
		/// </summary>
		private void offlineRadioButton_Checked(object sender, RoutedEventArgs e)
		{
			SetMenuItemEnabled(gestureRecordSubmenu, false);
		}

		/// <summary>
		/// Abre a janela de adicionar um gesto.
		/// </summary>
		private void addSubmenu_Click(object sender, RoutedEventArgs e)
		{
			NewDialog(new AddWindow());
		}

		/// <summary>
		/// Abre a janela de remover um gesto.
		/// </summary>
		private void removeSubmenu_Click(object sender, RoutedEventArgs e)
		{
			NewDialog(new RemoveWindow());
		}

		/// <summary>
		/// Abre a janela de gravar um gesto.
		/// </summary>
		private void gestureRecordSubmenu_Click(object sender, RoutedEventArgs e)
		{
			NewDialog(new RecordWindow());
		}

		/// <summary>
		/// Abre a janela para treinar um gesto.
		/// </summary>
		private void gestureTrainSubmenu_Click(object sender, RoutedEventArgs e)
		{
			if (NewDialog(new SelectWindow()) == true) {
				if (this.offlineRadioButton.IsChecked == true) {
					OpenFileDialog dialog = new OpenFileDialog();
					dialog.Title = "Open Stream";
					dialog.ValidateNames = true;
					dialog.AddExtension = true;
					dialog.DefaultExt = ".txt";
					dialog.CheckPathExists = true;
					dialog.Multiselect = true;
					if (dialog.ShowDialog() == true) {
						foreach (string filename in dialog.FileNames)
							controller.StartTraining(filename);
					}
				}
				else {
					NewDialog(new TrainingWindow());
				}
			}
		}

		/// <summary>
		/// Abre a janela para reconhecer um gesto.
		/// </summary>
		private void gestureRecognizeSubmenu_Click(object sender, RoutedEventArgs e)
		{
			if (this.offlineRadioButton.IsChecked == true) {
				OpenFileDialog dialog = new OpenFileDialog();
				dialog.Title = "Open Stream";
				dialog.ValidateNames = true;
				dialog.AddExtension = true;
				dialog.DefaultExt = ".txt";
				dialog.CheckPathExists = true;
				dialog.Multiselect = true;
				if (dialog.ShowDialog() == true) {
					foreach (string filename in dialog.FileNames)
						controller.StartRecognizing(filename);
				}
			}
			else {
				NewDialog(new DecodingWindow());
			}
		}

		/// <summary>
		/// Abre a janela para listar os gestos.
		/// </summary>
		private void gestureViewSubmenu_Click(object sender, RoutedEventArgs e)
		{
			NewDialog(new GestureWindow());
		}

		#endregion
	}
}

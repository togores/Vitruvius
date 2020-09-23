
// Controller.cs
// Tiago Togores - 2011

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Win32;
using OpenNI;
using Vitruvius.Device;
using Vitruvius.Gestures;
using Vitruvius.Users;
using Vitruvius.Views;

namespace Vitruvius
{
	/// <summary>
	/// Controla a lógica do programa e faz a ponte entre o WPF e os dados.
	/// Faz o papel do Presenter no Design Pattern Model-View-Presenter com Passive View.
	/// </summary>
	public class Controller
	{
		#region Fields and Properties

		private static Controller instance; // singleton
		private WindowManager windowManager;

		private const int fps = 30;
		private DispatcherTimer dispatcher;
		private DateTime time;

		private Context context;
		private RGBCamera rgbCamera;
		private DepthSensor depthSensor;
		private UserManager userManager;

		private DepthImage depthImage;
		private RGBImage rgbImage;
		private bool drawDepthImage; // se deve desenhar a imagem de profundidade
		private bool drawRGBImage; // se deve desenhar a imagem da camera

		private User mainUser; // usuário principal, o único que pode realizar gestos
		private Observer observer;

		private Recorder recorder;
		private bool realtime; // se gestos são executados em tempo-real ou lidos de arquivos.

		#endregion

		#region Properties

		/// <summary>
		/// Taxa de atualização em quadros por segundo.
		/// </summary>
		public static int Fps
		{
			get
			{
				return Controller.fps;
			}
		}

		/// <summary>
		/// Gerenciador das janelas do programa.
		/// </summary>
		public WindowManager WindowManager
		{
			get
			{
				return windowManager;
			}
		}

		/// <summary>
		/// Gerenciador dos gestos.
		/// </summary>
		public GestureManager GestureManager
		{
			get
			{
				return observer.GestureManager;
			}
		}

		/// <summary>
		/// Devolve a única instância da Classe Controller.
		/// </summary>
		public static Controller Instance
		{
			get
			{
				if (instance == null)
					instance = new Controller();
				return instance;
			}
		}

		#endregion

		/// <summary>
		/// Construtor padrão.
		/// </summary>
		private Controller()
		{
			context = new Context();
			while (true) {// espera que o Kinect esteja corretamente conectado
				try {
					userManager = new UserManager(context);
					depthSensor = new DepthSensor(context);
					rgbCamera = new RGBCamera(context);
				}
				catch (Exception) {
					if (MessageBox.Show("Kinect is not connected. Connect and retry.", "Error", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
						continue;
					Application.Current.Shutdown();
				}
				break;
			}

			depthImage = new DepthImage();
			rgbImage = new RGBImage();
			drawDepthImage = false;
			drawRGBImage = false;

			time = DateTime.Now;
			windowManager = new WindowManager();

			observer = new Observer(new GestureManager(), Parameterization.Extractor, Parameterization.Filter);
			recorder = new Recorder();
			realtime = true;
		}

		#region Logical Methods

		/// <summary>
		/// Converte um ponto no sistema de coordenadas do mundo real para um ponto projetado na tela.
		/// </summary>
		public Point3D ConvertRealWorldToProjective(Point3D worldPoint)
		{
			return depthSensor.Generator.ConvertRealWorldToProjective(worldPoint);
		}

		/// <summary>
		/// Converte um ponto no sistema de coordenadas projetado na tela para um ponto do mundo real.
		/// </summary>
		public Point3D ConvertProjectiveToRealWorld(Point3D worldPoint)
		{
			return depthSensor.Generator.ConvertProjectiveToRealWorld(worldPoint);
		}

		/// <summary>
		/// Inicia a execução.
		/// </summary>
		public void Start()
		{
			context.StartGeneratingAll();
			userManager.UserGenerator.StartGenerating();
			dispatcher = new DispatcherTimer();
			dispatcher.Tick += new EventHandler(WorkerThread);
			dispatcher.Interval = new TimeSpan(0, 0, 0, 0, 0);
			dispatcher.Start();
		}

		/// <summary>
		/// Thread que faz o processamento.
		/// </summary>
		private void WorkerThread(object sender, EventArgs e)
		{
			// atualiza os dados dos dispositivos
			try {
				context.WaitAnyUpdateAll();
			}
			catch (Exception ex) {
				Console.WriteLine(ex.Message);
			}

			#region Drawing

			if (drawRGBImage) {
				rgbCamera.UpdateMetadata();
				rgbImage.DrawFrame(rgbCamera, userManager);
			}
			else if (drawDepthImage) {
				depthSensor.UpdateMetadata();
				depthSensor.UpdateHistogram();
				depthImage.DrawFrame(depthSensor, userManager);
			}
			else {
				DeviceImage.DrawFrame();
			}
			// desenha
			WindowManager.MainWindow.image.Source = DeviceImage.WriteableBitmap;

			#endregion

			// atualiza os usuários
			userManager.UpdateAllUsers();
			mainUser = userManager.UpdateMainUser();
			windowManager.MainWindow.usersLabel.Text = userManager.GetUsersLabel();
			lock (this) {
				if (realtime) {
					recorder.Capture(mainUser, userManager.Skeleton);
					observer.Capture(mainUser);
				}
			}

			// calcula o fps do programa
			DateTime now = DateTime.Now;
			double timePassed = 0.0;
			timePassed += now.Millisecond + 1000 * (now.Second + 60 * (now.Minute + 60 * now.Hour));
			timePassed -= time.Millisecond + 1000 * (time.Second + 60 * (time.Minute + 60 * time.Hour));
			time = now;
			windowManager.MainWindow.fpsTextBlock.Text = (int)(1000/timePassed) + " FPS";
		}

		/// <summary>
		/// Para a execução
		/// </summary>
		public void Stop()
		{
			dispatcher.Stop();
			context.StopGeneratingAll();
			context.Release();
		}

		#endregion

		#region Interface Methods
		// Callbacks dos eventos da interface

		#region Focus Menu

		/// <summary>
		/// Troca o foco de atenção para o item selecionado
		/// </summary>
		public void ChangeAttentionFocus(String name)
		{
			userManager.SetFocus(name);
		}

		/// <summary>
		/// Troca o timeout de focusAlgorithm
		/// </summary>
		public void setUserTimeout(string focusAlgorithm, int timeout)
		{
			userManager.Focus[focusAlgorithm].FocusTimeout = timeout * fps;
		}

		/// <summary>
		/// Coloca joint como joint de referência do algoritmo de foco focusAlgorithm
		/// </summary>
		public void SetReferenceJoint(string focusAlgorithm, string joint)
		{
			userManager.Focus[focusAlgorithm].ReferenceJoint = userManager.Skeleton.AvailableJoints[joint];
		}

		#endregion

		#region Views Menu

		#region Show

		/// <summary>
		/// Mostra a imagem da camera de cor.
		/// </summary>
		public void ViewRGBImage()
		{
			drawRGBImage = true;
			drawDepthImage = false;
		}

		/// <summary>
		/// Mostra uma imagem do sensor de profundidade.
		/// </summary>
		public void ViewDepthImage()
		{
			drawRGBImage = false;
			drawDepthImage = true;
		}

		/// <summary>
		/// Mostra uma imagem vazia (toda preta).
		/// </summary>
		public void ViewEmptyImage()
		{
			drawRGBImage = false;
			drawDepthImage = false;
		}

		/// <summary>
		/// Mostra os pixels das imagens das cameras.
		/// </summary>
		public void ShowBackground()
		{
			depthImage.ShouldDrawBackground = true;
			rgbImage.ShouldDrawBackground = true;
		}

		/// <summary>
		/// Não mostra os pixels das imagens das cameras.
		/// </summary>
		public void HideBackground()
		{
			depthImage.ShouldDrawBackground = false;
			rgbImage.ShouldDrawBackground = false;
		}

		/// <summary>
		/// Mostra os esqueletos dos usuários sobre as imagens das cameras.
		/// </summary>
		public void ShowSkeleton()
		{
			depthImage.ShouldDrawSkeleton = true;
			rgbImage.ShouldDrawSkeleton = true;
		}

		/// <summary>
		/// Não mostra os esqueletos dos usuários sobre as imagens das cameras.
		/// </summary>
		public void HideSkeleton()
		{
			depthImage.ShouldDrawSkeleton = false;
			rgbImage.ShouldDrawSkeleton = false;
		}

		/// <summary>
		/// Mostra os IDs dos usuários sobre as imagens das cameras.
		/// </summary>
		public void ShowID()
		{
			depthImage.ShouldPrintID = true;
			rgbImage.ShouldPrintID = true;
		}

		/// <summary>
		/// Não mostra os IDs dos usuários sobre as imagens das cameras.
		/// </summary>
		public void HideID()
		{
			depthImage.ShouldPrintID = false;
			rgbImage.ShouldPrintID = false;
		}

		/// <summary>
		/// Mostra o estado dos usuários sobre as imagens das cameras.
		/// </summary>
		public void ShowState()
		{
			depthImage.ShouldPrintState = true;
			rgbImage.ShouldPrintState = true;
		}

		/// <summary>
		/// Não mostra o estado dos usuários sobre as imagens das cameras.
		/// </summary>
		public void HideState()
		{
			depthImage.ShouldPrintState = false;
			rgbImage.ShouldPrintState = false;
		}

		#endregion

		#endregion

		#region Gesture Menu

		/// <summary>
		/// Devolve uma lista com os nomes dos gestos disponíveis
		/// </summary>
		public List<string> GetGesturesList()
		{
			return new List<string>(GestureManager.Gestures.Keys);
		}

		/// <summary>
		/// Atribui o gesto de nome passado como o alvo do treinamento.
		/// </summary>
		public void SetTraining(string gestureName)
		{
			observer.GestureName = gestureName;
		}

		/// <summary>
		/// Inicia um treinamento em tempo real.
		/// </summary>
		public void StartTraining()
		{
			observer.StartObserving();
		}

		/// <summary>
		/// Inicia um treinamento lendo de um arquivo.
		/// </summary>
		public void StartTraining(string filename)
		{
			realtime = false;
			observer.StartObserving();
			recorder.StartReading(filename);
			User user = new User(-1);
			while (recorder.NextPose(user, userManager.Skeleton))
				observer.Capture(user);
			StopTraining();
			realtime = true;
		}

		/// <summary>
		/// Finaliza com sucesso o treinamento.
		/// </summary>
		public void StopTraining()
		{
			observer.Train();
		}

		/// <summary>
		/// Inicia um reconhecimento em tempo real de um gesto.
		/// </summary>
		public void StartRecognizing()
		{
			observer.StartObserving();
		}

		/// <summary>
		/// Inicia um reconhecimento de um gesto a partir de um arquivo.
		/// </summary>
		public void StartRecognizing(string filename)
		{
			realtime = false;
			observer.StartObserving();
			recorder.StartReading(filename);
			User user = new User(-1);
			while (recorder.NextPose(user, userManager.Skeleton))
				observer.Capture(user);
			StopDecoding();
			realtime = true;
		}

		/// <summary>
		/// Finaliza o reconhecimento de um gesto.
		/// </summary>
		public void StopDecoding()
		{
			Gesture gesture = observer.Recognize();
			if (gesture != null)
				Controller.Instance.Print("Gesture recognized: " + gesture.Name);
			else
				Controller.Instance.Print("Gesture not recognized");
		}

		/// <summary>
		/// Importa os dados dos gestos de arquivos.
		/// </summary>
		public void ImportGesture(params string[] files)
		{
			GestureManager.ImportGestures(files);
		}

		/// <summary>
		/// Exporta os dados do gesto para arquivo.
		/// </summary>
		public void ExportGesture(string name, string file)
		{
			GestureManager.ExportGesture(name, file);
		}

		/// <summary>
		/// Inicia a gravação do movimento de um usuário.
		/// </summary>
		public void StartRecording()
		{
			recorder.StartRecord();
		}

		/// <summary>
		/// Finaliza a gravação.
		/// </summary>
		public void StopRecording()
		{
			SaveFileDialog dialog = new SaveFileDialog();
			dialog.Title = "Save Stream";
			dialog.ValidateNames = true;
			dialog.AddExtension = true;
			dialog.DefaultExt = ".txt";
			dialog.CheckPathExists = true;
			dialog.CreatePrompt = false;
			dialog.OverwritePrompt = true;
			if (dialog.ShowDialog() == true) {
				recorder.Save(dialog.FileName);
				Print("stream saved: " + dialog.FileName);
			}
			recorder.StopRecord();
		}

		#endregion

		#region Console

		/// <summary>
		/// Imprime no console do programa
		/// </summary>
		public void Print(Object o)
		{
			if (o != null)
				windowManager.MainWindow.outputConsole.Text += o.ToString() +"\n";
			windowManager.MainWindow.outputConsole.ScrollToEnd();
		}

		#endregion

		#endregion
	}
}

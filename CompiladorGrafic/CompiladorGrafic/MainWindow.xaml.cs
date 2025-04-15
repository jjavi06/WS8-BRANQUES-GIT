using System.IO;
using Microsoft.Win32;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ProvaTaula;

namespace CompiladorGrafic
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Compilador compilador = new Compilador();
        List<object> datos = new List<object>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void t1_btn_validar_Click(object sender, RoutedEventArgs e)
        {
            string expresion = t1_tbox_expresion.Text;

            ValidarYResolver(expresion);

            t1_tbox_expresion.Clear();
        }

        private void t1_btn_SelectDoc_Click(object sender, RoutedEventArgs e)
        {
            string ruta;
            TaulaLlista<string> expresiones = new TaulaLlista<string>();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivos de texto (*.txt)|*.txt";
            openFileDialog.Title = "Selecciona un archivo de texto";

            if (openFileDialog.ShowDialog() == true)
            {
                ruta = openFileDialog.FileName;
                expresiones = CargarListaExpresiones(ruta);
                foreach (string str in expresiones)
                    ValidarYResolver(str);
            }
        }
        #region Métodos Internos
        private void ValidarYResolver(string expresion)
        {
            string valido;
            int resultado = 0;

            if (compilador.Validar(expresion))
            {
                valido = "Válido";
                resultado = compilador.Resolver(expresion);
            }
            else
                valido = "Inválido";

            AgregarLineaDG(expresion, valido, resultado);

        }
        private TaulaLlista<string> CargarListaExpresiones(string ruta)
        {
            TaulaLlista<string> expresiones = new TaulaLlista<string>();
            StreamReader sr = new StreamReader(ruta);
            string linia = sr.ReadLine();
            while (linia != null)
            {
                expresiones.Add(linia);
                linia = sr.ReadLine();
            }
            return expresiones;
        }
        private void AgregarLineaDG(string expresion, string valido, int resultado)
        {
            datos.Add(new { Expresion = expresion, Validacion = valido, Resultado = resultado });
            //Limpiar Data Grid
            t1_dg_datos.ItemsSource = null;
            //Poner datos en el Data Grid
            t1_dg_datos.ItemsSource = datos;

        }
        #endregion
    }
}
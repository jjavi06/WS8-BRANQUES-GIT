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
            string valido;
            int resultado=0;

            if (compilador.Validar(expresion))
            {
                valido = "Válido";
                resultado = compilador.Resolver(expresion);
            }
            else
                valido = "Inválido";
            t1_tbox_expresion.Clear();

            AgregarLineaDG(expresion, valido, resultado);
        }
        private void AgregarLineaDG(string expresion, string valido, int resultado)
        {
            datos.Add(new { Expresion = expresion, Validacion = valido, Resultado = resultado });
            //Limpiar Data Grid
            t1_dg_datos.ItemsSource = null;
            //Poner datos en el Data Grid
            t1_dg_datos.ItemsSource = datos;

        }
    }
}
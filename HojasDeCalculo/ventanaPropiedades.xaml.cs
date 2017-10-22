using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HojasDeCalculo
{
    /// <summary>
    /// Lógica de interacción para ventanaPropiedades.xaml
    /// </summary>
    public partial class ventanaPropiedades : Window
    {
        public Color colorConfigurado { get; set; }
        public double tamanoTrazo
        {
            get;set;
        }
        public ventanaPropiedades()
        {
            InitializeComponent();
            sliderTrazo.Value = tamanoTrazo;
        }

        private void cancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void aceptar_Click(object sender, RoutedEventArgs e)
        {

            DialogResult = true;
        }

        private void sliderTrazo_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            tamanoTrazo = sliderTrazo.Value;
        }

        private void sliderColor_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            colorConfigurado = Color.FromRgb((byte)slider1.Value, (byte)slider2.Value, (byte)slider3.Value);

        }
    }
}

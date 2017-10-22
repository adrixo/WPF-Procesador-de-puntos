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
    public partial class ventanaRenombrar : Window
    {
        public string nombreARenombrar { get; set; }
        public ventanaRenombrar()
        {
            InitializeComponent();
        }

        private void cancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void aceptar_Click(object sender, RoutedEventArgs e)
        {
            if (textbox.Text!="")
            {
                nombreARenombrar = textbox.Text;
                DialogResult = true;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HojasDeCalculo
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        //Lista de listas que contendrá las listas abiertas
        System.Collections.ObjectModel.ObservableCollection<Lista> Listas = new System.Collections.ObjectModel.ObservableCollection<Lista>();
        //Collección de puntos donde se almacenarán las coordenadas a ser representadas
        PointCollection coleccionPantalla;

        public MainWindow()
        {
            if (iniciarCarpeta()) {
                leerDatosGuardados();
            }

            InitializeComponent();
            dialogoListas.ItemsSource = Listas;
        }

        //Clases empleadas al inicio para cargar carpeta y archivos contenidos
        public bool iniciarCarpeta() 
        {
            //Comprueba si hay creada una carpeta de guardado y si no la crea

            // Carpeta donde se guardan las listas
            // Obtenemos la ruta acutal
            string directorioHojas = System.IO.Directory.GetCurrentDirectory();
            // Add directorio guardado
            directorioHojas = System.IO.Path.Combine(directorioHojas, "Hojas");
            // Comprobamos si esta creado
            if (System.IO.Directory.Exists(directorioHojas))
            {
                return true;
            }
            else
            {
                System.IO.Directory.CreateDirectory(directorioHojas);
                return false;
            }
        }

        public void leerDatosGuardados()
        {
            //Primero obtenemos la ruta
            string directorioHojas = System.IO.Directory.GetCurrentDirectory();
            directorioHojas = System.IO.Path.Combine(directorioHojas, "Hojas");

            string[] ArrayHojas = Directory.GetFiles(directorioHojas, "*.HC").Select(System.IO.Path.GetFileName).ToArray();

            DirectoryInfo directorio = new DirectoryInfo(directorioHojas);
            int numHojas = directorio.GetFiles("*.HC", SearchOption.AllDirectories).Length;
            //leemos fichero a fichero
            for (int i=0; i < numHojas; i++){

                //Obtenemos Extension del archivo
                string extension = System.IO.Path.GetExtension(ArrayHojas[i]);
                //obtenemos nombre del archivo desde 0 hasta antes de su extension
                string archivoSinExtension = ArrayHojas[i].Substring(0, ArrayHojas[i].Length - extension.Length);

                //Creamos un elemento lista con el nombre del archivo
                Listas.Add(new Lista() { Nombre = archivoSinExtension });

                int elemento = -1;
                string linea;
                // Vamos leyendo las lineas del archivo y clasificando sus coordenadas
                System.IO.StreamReader hojaActual = new System.IO.StreamReader(System.IO.Path.Combine(directorioHojas, ArrayHojas[i]));
                
                while ((linea = hojaActual.ReadLine()) != null)
                {
                    if (elemento==-1)
                    {
                        System.Console.WriteLine(linea);
                        Listas[i].tipo = linea;
                        elemento++;
                    }
                    else
                    {
                        System.Console.WriteLine(linea);
                        string[] coordenadas = linea.Split(' ');
                        Listas[i].addPunto(int.Parse(coordenadas[0]), int.Parse(coordenadas[1]), int.Parse(coordenadas[2]));
                        elemento++;
                    }
                }
                hojaActual.Close();
                System.Console.ReadLine();
            }
            //
            // Cargamos las listas que se encuentren en el directorio hojas
            //  mientras haya ficheros
            //  añadir a la lista(nombre del fichero)
            //  a elemento de la lista añadir puntos dentro del fichero / sumar numero de puntos
            //  cerrar el fichero
        }

        //Evento encargado de delimitar el funcionamiento de los dos cuadros de dialogo cuando el tamaño de la ventana cambia
        //tambien instancia ejes
        private void ventanaPrincipal_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            dialogoPuntos.MaxHeight = datos.ActualWidth - 115; //procurando dejar tanto el boton de nueva lista como punto en la pantalla
            dialogoListas.MaxHeight = hojas.ActualHeight - 25;
            dibujarEjes();
            actualizarPuntos();
        }

        public void dibujarEjes()
        {
            //Dibujamos los ejes de forma relativa respecto al tamaño actual de la ventana
            ejex.Stroke = Brushes.Black;
            ejex.Name = "ejex";
            ejex.X1 = 0;
            ejex.X2 = grafico.ActualWidth;
            ejex.Y1 = grafico.ActualHeight / 2;
            ejex.Y2 = grafico.ActualHeight / 2;
            ejex.StrokeThickness = 1;

            ejey.Stroke = Brushes.Black;
            ejey.Name = "ejex";
            ejey.X1 = grafico.ActualWidth / 2;
            ejey.X2 = grafico.ActualWidth / 2;
            ejey.Y1 = 0;
            ejey.Y2 = grafico.ActualHeight;
            ejey.StrokeThickness = 1;

        }

        //eventos para los botones del menu
        private void menuAbrir_Click(object sender, RoutedEventArgs e)
        {
            //Preparamos una nueva instancia de openfiledialog
            Microsoft.Win32.OpenFileDialog dialogoCargar = new Microsoft.Win32.OpenFileDialog();
            dialogoCargar.InitialDirectory = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Hojas");
            dialogoCargar.Filter = "Documentos Hoja de Calculo (.HC)|*.HC";

            Nullable<bool> result = dialogoCargar.ShowDialog();

            if (result == true)
            {
                Listas.Add(new Lista() { Nombre = System.IO.Path.GetFileNameWithoutExtension(dialogoCargar.Fi‌​leName) });

                int elemento = -1;
                string linea;
                // Vamos leyendo las lineas del archivo y clasificando sus coordenadas
                System.IO.StreamReader hojaActual = new System.IO.StreamReader(dialogoCargar.FileName);
                while ((linea = hojaActual.ReadLine()) != null)
                {
                    if (elemento == -1) //el primer elemento constituye el tipo
                    {
                        System.Console.WriteLine(linea);
                        Listas[Listas.Count - 1].tipo = linea;
                        elemento++;
                    }
                    else //agregamos uno a uno los puntos guardados.
                    {
                        System.Console.WriteLine(linea);
                        string[] coordenadas = linea.Split(' ');
                        Listas[Listas.Count - 1].addPunto(int.Parse(coordenadas[0]), int.Parse(coordenadas[1]), int.Parse(coordenadas[2]));
                        elemento++;
                    }
                }
                hojaActual.Close();
            }

        }

        private void menuGuardar_Click(object sender, RoutedEventArgs e)
        {
            //Preparamos una instancia de savefiledialog, solo si hay un item seleccionado
            if (dialogoListas.SelectedIndex >= 0)
            {
                Microsoft.Win32.SaveFileDialog dialogoGuardar = new Microsoft.Win32.SaveFileDialog();
                dialogoGuardar.InitialDirectory = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Hojas");
                dialogoGuardar.FileName = Listas[dialogoListas.SelectedIndex].Nombre;
                dialogoGuardar.DefaultExt = ".HC";
                dialogoGuardar.Filter = "Documentos Hoja de Calculo (.HC)|*.HC";

                Nullable<bool> result = dialogoGuardar.ShowDialog();

                if (result == true)
                {
                    //La clase lista nos prepara un string para guardar
                    File.WriteAllText(dialogoGuardar.FileName, Listas[dialogoListas.SelectedIndex].obtenerPuntosParaGuardar());
                }
            }
            else
            {
                mensajeAlertaHojaNoSeleccionada();
            }
        }

        private void menuNueva_Click(object sender, RoutedEventArgs e)
        {
            //añadimos un nuevo item mediante una ventana creada por mi
            ventanaRenombrar vrenombrar = new ventanaRenombrar();
            vrenombrar.Owner = this;
            vrenombrar.nombreARenombrar = "Nueva lista:";
            vrenombrar.informacion.Content = vrenombrar.nombreARenombrar;
            vrenombrar.ShowDialog();
            if (vrenombrar.DialogResult == true)
            {
                Listas.Add(new Lista() { Nombre = vrenombrar.nombreARenombrar });
            }
        }

        private void Configuracion_Click(object sender, RoutedEventArgs e)
        {
            //accedemos al menu configuración definido en ventanaPropiedades
            ventanaPropiedades vpropiedades = new ventanaPropiedades();
            vpropiedades.Owner = this;
            vpropiedades.sliderTrazo.Value = grafica.StrokeThickness;
            SolidColorBrush antiguo = (SolidColorBrush)grafica.Fill;
            vpropiedades.slider1.Value = antiguo.Color.R;
            vpropiedades.slider2.Value = antiguo.Color.G;
            vpropiedades.slider3.Value = antiguo.Color.B;
            vpropiedades.ShowDialog();
            if (vpropiedades.DialogResult == true)
            {
                grafica.StrokeThickness = vpropiedades.tamanoTrazo;
                grafica.Fill = new SolidColorBrush(vpropiedades.colorConfigurado);
            }

        }

        private void Manual_Click(object sender, RoutedEventArgs e)
        {
            string msg = "Manual\n\n" +
                "Este programa permite el manejo de las hojas de cálculo \n" +
                "situadas en el diálogo izquierdo donde puede:\n" +
                "\t - Añadir nuevas mediante el botón.\n" +
                "\t - Manejar las existentes mediante la pestaña ARCHIVO.\n" +
                "\t - Eliminar y renombrar mediante click derecho.\n\n"+
                "Una vez seleccionada una lista <doble click> puede: \n"
                + "\t - Añadir y modificar puntos.\n"
                + "\t - Cambiar el modo de representación <click en nombre>.\n"
                + "\t - Borrar un elemento <click derecho>.\n"
                + "\t * No se olvide de refrescar la vista cuando termine de \n"
                + "\t   manipular los puntos.\n\n"
                + "Si lo desea configure el estilo del gráfico desde"
                + "Configuracion.";
            string titulo = "Manual";
            MessageBoxButton botones = MessageBoxButton.OK;
            MessageBoxImage icono = MessageBoxImage.Information;
            MessageBox.Show(msg, titulo, botones, icono);
        }

        private void Autor_Click(object sender, RoutedEventArgs e)
        {
            string msg = "Interfaces gráficas 2017\n\nAutor: Adrián Valera Román\n";
            string titulo = "Sobre el autor";
            MessageBoxButton botones = MessageBoxButton.OK;
            MessageBoxImage icono = MessageBoxImage.Information;
            MessageBox.Show(msg, titulo, botones, icono);
        }

        //funciones encargadas de añadir items a las listas
        private void dialogoHojas_Nuevo_Click(object sender, RoutedEventArgs e)
        {
            Listas.Add(new Lista() { Nombre = "nuevo"});
        }

        private void aniadirPunto_Click(object sender, RoutedEventArgs e)
        {
            if (dialogoPuntos.ItemsSource != null) {
                Listas[dialogoListas.SelectedIndex].addPunto(0, 0);
            }
            
        }

        //funcion encargada de seleccionar item de la lista principal
        private void dialogoListas_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dialogoListas.SelectedIndex>=0) {
                dialogoPuntos.ItemsSource = Listas[dialogoListas.SelectedIndex].listaPuntos;
                nombreHojaAbierta.Content = Listas[dialogoListas.SelectedIndex].Nombre + "\n" + Listas[dialogoListas.SelectedIndex].tipo;
                actualizarPuntos();
            }
        }

        private void dialogoListas_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void listasRenombrar_Click(object sender, RoutedEventArgs e)
        {
            //En caso de tener seleccionado un item, podremos renombrarlo mediante click derecho
            if (dialogoListas.SelectedIndex >= 0)
            {
                ventanaRenombrar vrenombrar = new ventanaRenombrar();
                vrenombrar.Owner = this;
                vrenombrar.nombreARenombrar = Listas[dialogoListas.SelectedIndex].Nombre;
                vrenombrar.informacion.Content = vrenombrar.nombreARenombrar;
                vrenombrar.ShowDialog();
                if (vrenombrar.DialogResult == true)
                {
                    Listas[dialogoListas.SelectedIndex].Nombre = vrenombrar.nombreARenombrar;
                }
            }
            else
            {
                mensajeAlertaHojaNoSeleccionada();
            }
        }

        private void listasBorrar_Click(object sender, RoutedEventArgs e)
        {
            //Eliminamos elemento seleccionado
            if (dialogoListas.SelectedIndex >= 0)
            {
                Listas.RemoveAt(dialogoListas.SelectedIndex);
            }
            else
            {
                mensajeAlertaHojaNoSeleccionada();
            }
        }

        private void puntoBorrar_Click(object sender, RoutedEventArgs e)
        {
            //Eliminamos punto seleccionado
            if (dialogoPuntos.SelectedIndex >= 0)
            {
                Listas[dialogoListas.SelectedIndex].listaPuntos.RemoveAt(dialogoPuntos.SelectedIndex);
            }
            else
            {
                mensajeAlertaHojaNoSeleccionada();
            }
        }

        public void actualizarPuntos()
        {
            //si tenemos una lista seleccionada, actualizamos la lista de puntos en la grafica
            if (dialogoListas.SelectedIndex >= 0)
            {
                if (Listas[dialogoListas.SelectedIndex].listaPuntos.Count >=2)
                {
                    coleccionPantalla = Listas[dialogoListas.SelectedIndex].ObtenerColeccionPuntos((int)grafico.ActualHeight, (int)grafico.ActualWidth);
                    grafica.Points = coleccionPantalla;
                }

            }
        }

        public void mensajeAlertaHojaNoSeleccionada()
        {
            //Mensaje por defecto ejecutado cuando no hay item seleccionado en la lista principal
            string msg = "No hay elemento seleccionado";
            string titulo = "Accion ilegal";
            MessageBoxButton botones = MessageBoxButton.OK;
            MessageBoxImage icono = MessageBoxImage.Exclamation;
            MessageBox.Show(msg, titulo, botones, icono);
        }

        private void actualizarPunto_Click(object sender, RoutedEventArgs e)
        {
            //controlador del evento actualizar
            if (dialogoListas.SelectedIndex >= 0 )
            {
                if (Listas[dialogoListas.SelectedIndex].listaPuntos.Count > 0)
                {
                    Listas[dialogoListas.SelectedIndex].ordenarParaFuncion();
                    actualizarPuntos();
                }
            }
        }

        private void nombreHojaAbierta_Click(object sender, RoutedEventArgs e)
        {
            //Controlador del boton de nombre en el dialogo de puntos.
            if (dialogoListas.SelectedIndex >= 0)
            {
                Listas[dialogoListas.SelectedIndex].cambiarTipo();
                nombreHojaAbierta.Content = Listas[dialogoListas.SelectedIndex].Nombre + "\n" + Listas[dialogoListas.SelectedIndex].tipo;
                if (Listas[dialogoListas.SelectedIndex].listaPuntos.Count > 0)
                {
                    Listas[dialogoListas.SelectedIndex].ordenarParaFuncion();
                    actualizarPuntos();
                }
            }
        }

        private void dialogoPuntos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}

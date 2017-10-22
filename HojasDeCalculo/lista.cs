using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace HojasDeCalculo
{


    public partial class Lista : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged( String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        //Atributo nombre de la lista
        private String atributoNombre;
        public String Nombre
        {
            get
            {
                return this.atributoNombre;
            }
            set
            {
                this.atributoNombre = value;
                NotifyPropertyChanged();
            }
        }

        //tipo de grafica a representar
        private String atributoTipo="polilinea";
        public String tipo
        {
            get
            {
                return this.atributoTipo;
            }
            set
            {
                this.atributoTipo = value;
                NotifyPropertyChanged();
            }
        }

        //variable donde se almacena el numero de puntos
        private int numeroPuntos;
        public int nPuntos
        {
            get
            {
                return this.numeroPuntos;
            } 
            set
            {
                this.numeroPuntos = value;
                NotifyPropertyChanged();

            }
        }

        //lista observable de puntos 
        public System.Collections.ObjectModel.ObservableCollection<Puntos> listaPuntos = new System.Collections.ObjectModel.ObservableCollection<Puntos>();

        public void addPunto(int xNuevo, int yNuevo)
        {
            listaPuntos.Add(new Puntos() { x = xNuevo, y = yNuevo, posicionPoligono=listaPuntos.Count });
            nPuntos += 1;
        }

        public void addPunto(int xNuevo, int yNuevo, int posicionNueva)
        {
            listaPuntos.Add(new Puntos() { x = xNuevo, y = yNuevo, posicionPoligono = posicionNueva });
            nPuntos += 1;
        }

        public PointCollection ObtenerColeccionPuntos(int altura, int anchura)
        {
            //anchura/2 es 0 en x
            int zero= altura / 2;
            //altura/2 es 0 en y
            PointCollection nuevaColeccionPuntos = new PointCollection();
            int xTemp;
            int yTemp;
            int contador=0;

            if (tipo == "polilinea")
            {
                foreach (var coordenada in listaPuntos)
                {
                        if (contador==0)
                        {
                             xTemp = coordenada.x + anchura / 2;
                             yTemp = altura / 2;
                             nuevaColeccionPuntos.Add(new System.Windows.Point(xTemp, yTemp));
                        }

                    xTemp = coordenada.x + anchura / 2;
                    yTemp = altura / 2 - coordenada.y;
                    nuevaColeccionPuntos.Add(new System.Windows.Point(xTemp, yTemp));

                        if ( (contador+1) == listaPuntos.Count)
                        {
                            xTemp = coordenada.x + anchura / 2;
                            yTemp = altura / 2;
                            nuevaColeccionPuntos.Add(new System.Windows.Point(xTemp, yTemp));
                            nuevaColeccionPuntos.Add(new System.Windows.Point(nuevaColeccionPuntos[0].X, nuevaColeccionPuntos[0].Y));
                        }
                    contador++;
                }
                return nuevaColeccionPuntos;
            }

            if (tipo == "barras")
            {
                int xdistanciaConAnterior;
                int xDistanciaConSiguiente;
                foreach (var coordenada in listaPuntos)
                {
                    if (contador == 0)
                    {
                        xDistanciaConSiguiente = (listaPuntos[contador + 1].x - coordenada.x) / 2;
                        xTemp = coordenada.x + anchura / 2 - xDistanciaConSiguiente;
                        nuevaColeccionPuntos.Add(new System.Windows.Point(xTemp, zero)); //primera linea descendiente
                        yTemp = altura / 2 - coordenada.y;
                        nuevaColeccionPuntos.Add(new System.Windows.Point(xTemp, yTemp));
                        xTemp = coordenada.x + anchura / 2 + xDistanciaConSiguiente;
                        nuevaColeccionPuntos.Add(new System.Windows.Point(xTemp, yTemp));
                        nuevaColeccionPuntos.Add(new System.Windows.Point(xTemp, zero));
                    }
                    else if ((contador + 1) == listaPuntos.Count)
                    {
                        xdistanciaConAnterior = (coordenada.x - listaPuntos[contador - 1].x) / 2;
                        xTemp = coordenada.x + anchura / 2 - xdistanciaConAnterior;
                        nuevaColeccionPuntos.Add(new System.Windows.Point(xTemp, zero));
                        yTemp = altura / 2 - coordenada.y;
                        nuevaColeccionPuntos.Add(new System.Windows.Point(xTemp, yTemp));
                        xTemp = coordenada.x + anchura / 2 + xdistanciaConAnterior;
                        nuevaColeccionPuntos.Add(new System.Windows.Point(xTemp, yTemp));
                        nuevaColeccionPuntos.Add(new System.Windows.Point(xTemp, zero));
                        //unimos la parte de abajo
                        nuevaColeccionPuntos.Add(new System.Windows.Point((listaPuntos[0].x + anchura / 2 + (listaPuntos[0].x - listaPuntos[1].x) / 2), zero));
                    }
                    else
                    {
                        xDistanciaConSiguiente = (listaPuntos[contador + 1].x - coordenada.x) / 2;
                        xdistanciaConAnterior = (coordenada.x - listaPuntos[contador - 1].x) / 2;
                        xTemp = coordenada.x + anchura / 2 - xdistanciaConAnterior;
                        nuevaColeccionPuntos.Add(new System.Windows.Point(xTemp, zero));
                        yTemp = altura / 2 - coordenada.y;
                        nuevaColeccionPuntos.Add(new System.Windows.Point(xTemp, yTemp));
                        xTemp = coordenada.x + anchura / 2 + xDistanciaConSiguiente;
                        nuevaColeccionPuntos.Add(new System.Windows.Point(xTemp, yTemp));
                        nuevaColeccionPuntos.Add(new System.Windows.Point(xTemp, zero));
                    }
                    contador++;
                }
                return nuevaColeccionPuntos;
            }

            else
            {
                foreach (var coordenada in listaPuntos)
                {
                    xTemp = coordenada.x + anchura / 2;
                    yTemp = altura / 2 - coordenada.y;
                    nuevaColeccionPuntos.Add(new System.Windows.Point(xTemp, yTemp));
                }
                //cerramos el poligono
                nuevaColeccionPuntos.Add(new System.Windows.Point(listaPuntos[0].x + anchura / 2, altura / 2 - listaPuntos[0].y)); 

                return nuevaColeccionPuntos;
            }
        }

        public string obtenerPuntosParaGuardar()
        { 
            string listaResultado="" ;
            listaResultado += tipo;
            listaResultado += Environment.NewLine;
            foreach (var coordenada in listaPuntos)
            { 
                listaResultado += coordenada.x.ToString();
                listaResultado += " ";
                listaResultado += coordenada.y.ToString();
                listaResultado += " ";
                listaResultado += coordenada.posicionPoligono.ToString();
                listaResultado += Environment.NewLine;
            }
            return listaResultado;
        }

        public void cambiarTipo()
        {
            if ( tipo== "polilinea" )
            {
                tipo = "barras" ;
            }
                else if (tipo == "barras")
                {
                    tipo = "poligono";
                }
                    else if (tipo == "poligono")
                    {
                        tipo = "polilinea";
                    }
        }

        public void ordenarParaFuncion()
        {
            Puntos temp;
            if (tipo == "barras" | tipo == "polilinea")
            {
                for (int i=1;i<listaPuntos.Count;i++)
                {
                    for (int j=0;j< listaPuntos.Count - 1; j++)
                    {
                        if(listaPuntos[j].x > listaPuntos[j+1].x)
                        {
                            temp = listaPuntos[j];
                            listaPuntos[j] = listaPuntos[j + 1];
                            listaPuntos[j + 1] = temp;
                        }
                    }

                }
            }
            else
            {
                for (int i = 1; i < listaPuntos.Count; i++)
                {
                    for (int j = 0; j < listaPuntos.Count - 1; j++)
                    {
                        if (listaPuntos[j].posicionPoligono > listaPuntos[j + 1].posicionPoligono)
                        {
                            temp = listaPuntos[j];
                            listaPuntos[j] = listaPuntos[j + 1];
                            listaPuntos[j + 1] = temp;
                        }
                    }

                }
            }
        }
    }
}

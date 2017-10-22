using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HojasDeCalculo
{

    public class Puntos : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private int coordenadax;
        public int x
        {
            get
            {
                return this.coordenadax;
            }
            set
            {
                this.coordenadax = value;
                NotifyPropertyChanged();
            }
        }

        private int coordenaday;
        public int y
        {
            get
            {
                return this.coordenaday;
            }
            set
            {
                this.coordenaday = value;
                NotifyPropertyChanged();
            }
        }

        public int posicionPoligono { get; set; }
    }
}

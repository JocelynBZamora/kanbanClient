using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kanbanServer.Models
{
    public enum EstadoTareas
    {
        Pendiente, 
        EnProgreso,
        Terminada,
        Eliminar
    }
    public class TareasActivas: TareasDTO, INotifyPropertyChanged
    {
        
        
        private DateTime? fechaCreacion;
        public DateTime? FechaCreacion
        {
            get { return fechaCreacion; }
            set
            {
                fechaCreacion = value;
                PropertyChanged?.Invoke(this, new(nameof(FechaCreacion)));
            }
        }
       
        private EstadoTareas estado;
        public EstadoTareas Estado
        {
            get 
            {
               
                return estado;
            }
            set
            {
                estado = value;
                PropertyChanged?.Invoke(this, new(nameof(Estado)));
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;

    }
}


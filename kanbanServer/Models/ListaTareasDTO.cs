using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kanbanServer.Models
{
    public enum EstadoTarea
    {
        Pendiente,
        EnProgreso,
        Terminada
    }
    public class ListaTareasDTO: Usuario, INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Descrip { get; set; } = "";
        public DateTime FechaCreacion { get; set; }
        private EstadoTarea estado;
        public EstadoTarea Estado
        {
            get { return estado; }
            set
            {
                estado = value;
                PropertyChanged?.Invoke(this, new(nameof(Estado)));
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}

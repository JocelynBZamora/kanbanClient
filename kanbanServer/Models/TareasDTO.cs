using CommunityToolkit.Mvvm.ComponentModel;
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
        TareaEnviada
    }
    public  class TareasDTO:Usuario, INotifyPropertyChanged
    {
        public int Id { get; set; }
        private string? descrip;
        public string? Descrip
        {
            get { return descrip; }
            set
            {
                descrip = value;
                PropertyChanged?.Invoke(this, new(nameof(Descrip)));
            }
        }
        private DateTime fechaCreacion;
        public DateTime FechaCreacion
        {
            get { return fechaCreacion; }
            set
            {
                fechaCreacion = value;
                PropertyChanged?.Invoke(this, new(nameof(FechaCreacion)));
            }
        }
        private string? titulo;
        public string? Titulo
        {
            get { return titulo; }
            set
            {
                titulo = value;
                PropertyChanged?.Invoke(this, new(nameof(Titulo)));
            }
        }
        private EstadoTareas estado;
        public EstadoTareas Estado
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

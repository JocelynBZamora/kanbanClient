using kanbanServer.Helper;
using kanbanServer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kanbanServer.ViewModels
{
    public class ListaTareasViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ListaTareasDTO> Tareas { get; set; }
        public ListaTareasViewModel()
        {
            Tareas = new();
            ComunicacionHelper.Tarearesibida += (tarea) =>
            {
                if (tarea != null && !string.IsNullOrEmpty(tarea.Titulo))
                {
                    Tareas.Add(tarea);
                }
            };
            ComunicacionHelper.TareaProseso += (tarea) =>
            {
                var tareaExistente = Tareas.FirstOrDefault(x => x.Titulo == tarea.Titulo);
                if (tareaExistente != null)
                {
                    tareaExistente.Estado = (EstadoTarea)1;
                }
            };

            ComunicacionHelper.eliminar += (tarea) =>
            {
                try
                {
                    var tareaAEliminar = Tareas.FirstOrDefault(x => x.Titulo == tarea.Titulo);
                    if (tareaAEliminar != null)
                    {
                        Tareas.Remove(tareaAEliminar);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            };
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}


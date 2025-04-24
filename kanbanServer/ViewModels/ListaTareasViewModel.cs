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
                Tareas.Add(tarea);
            };

            ComunicacionHelper.TareaProseso += (tarea) =>
            {

                Tareas.Where(x => x.Titulo == tarea.Titulo).First().Estado = (EstadoTarea)1;

            };
            ComunicacionHelper.eliminar += (tarea) =>
            {
                try
                {
                    Tareas.Remove(Tareas.Where(x => x.Titulo == tarea.Titulo).First());

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


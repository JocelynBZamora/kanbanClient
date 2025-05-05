using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kanbanServer.Services;
using kanbanServer.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Text.Json;

namespace kanbanServer.ViewModels
{
    public class KanbanViewModel : ObservableObject
    {
        TareaServer serverbb = new();
        public ObservableCollection<TareasDTO> Encargado { get; set; } = new();
        public ObservableCollection<TareasActivas> Tareas { get; set; } = new();
        ListaTareasDTO tarealista = new();
         
        public KanbanViewModel()
        {
            serverbb.Tarearesibida += Serverbb_Tarearesibida;
            serverbb.TareaTerminada += Serverbb_TareaTerminada;
            serverbb.TareaProseso += Serverbb_TareaProseso;
            serverbb.eliminar += Serverbb_Eliminar;
            serverbb.TareaPendiente += Serverbb_Pendiente;

            if (File.Exists("assest/listatareas.json"))
            {
                var json = File.ReadAllText("assest/listatareas.json");
                var lista= JsonSerializer.Deserialize<ListaTareasDTO>(json);
                if (lista != null)
                {
                    tarealista= lista;
                    Encargado.Clear();
                    tarealista.Tareas.ForEach(t => {Encargado.Add(t); });
                }
            }
        }
        private void Serverbb_Tarearesibida(TareasActivas obj)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var tareaExistente = Tareas.FirstOrDefault(t => t.Titulo == obj.Titulo && t.IP == obj.IP);
                if (tareaExistente == null)
                {
                    Tareas.Add(obj);
                    File.WriteAllText("assest/listatareas.json", JsonSerializer.Serialize(tarealista));

                }
                else
                {
                    tareaExistente.Estado = EstadoTareas.Pendiente;
                    tareaExistente.FechaCreacion = obj.FechaCreacion;
                    tareaExistente.Encargado = obj.Encargado;
                    tareaExistente.Descrip = obj.Descrip;
                    tareaExistente.Titulo = obj.Titulo;
                    tareaExistente.IP = obj.IP;
                    tareaExistente.Id= obj.Id;
                }
            });
        }

        private void Serverbb_Pendiente(TareasActivas dTO)
        {
            Application.Current.Dispatcher.Invoke(() => 
            {
                var tareaExistente = Tareas.FirstOrDefault(x=> x.Titulo == dTO.Titulo);
                if(tareaExistente != null)
                {
                    tareaExistente.Estado=EstadoTareas.Pendiente;
                    File.WriteAllText("assest/listatareas.json", JsonSerializer.Serialize(tarealista));
                }
            });
        }

        private void Serverbb_TareaProseso(TareasActivas dTO)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var tareaExistente = Tareas.FirstOrDefault(x => x.Titulo == dTO.Titulo);
                if (tareaExistente != null)
                {
                    tareaExistente.Estado =EstadoTareas.EnProgreso;
                    File.WriteAllText("assest/listatareas.json", JsonSerializer.Serialize(tarealista));

                }
            });
        }

        private void Serverbb_TareaTerminada(TareasActivas dTO)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var tareaExistente = Tareas.FirstOrDefault(x=> x.Titulo == dTO.Titulo);
                if (tareaExistente != null)
                {
                    tareaExistente.Estado = EstadoTareas.Terminada;
                    tareaExistente.FechaCreacion=dTO.FechaCreacion;
                    tareaExistente.Encargado=dTO.Encargado;
                    tareaExistente.Descrip=dTO.Descrip;
                    tareaExistente.Titulo=dTO.Titulo;
                    tareaExistente.IP=dTO.IP;
                    tarealista.Tareas.Add(dTO);
                    tarealista.Tareas=tarealista.Tareas.OrderBy(x=> x.Titulo).ToList();
                    Encargado.Clear();
                    tarealista.Tareas.ForEach(x => Encargado.Add(x));

                }
            });
        }

        private void Serverbb_Eliminar(TareasActivas dTO)
        {
            Application.Current.Dispatcher.Invoke(() => 
            {
                var tareaExistente = Tareas.FirstOrDefault(x => x.Id == dTO.Id);
                if (tareaExistente != null)
                {
                    Tareas.Remove(tareaExistente);
                }
            });
        }

        
    }

}
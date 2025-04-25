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
        private readonly TareaServer serverbb = new();
        public ObservableCollection<TareasDTO> Tareas { get; set; } = new();
        private ListaTareasDTO listaTareas = new();

        public KanbanViewModel()
        {
            serverbb.Tarearesibida += Serverbb_Tarearesibida;
            serverbb.TareaTerminada += Serverbb_TareaTerminada;
            serverbb.TareaProseso += Serverbb_TareaProseso;
            serverbb.eliminar += Serverbb_Eliminar;
            serverbb.actualizar += Serverbb_Actualizar;

            if (!File.Exists("assest/ListaTareas.json"))
            {
                Directory.CreateDirectory("assest");
                var defaultLista = new ListaTareasDTO { Tareas = new List<TareasDTO>() };
                var defaultJson = JsonSerializer.Serialize(defaultLista, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText("assest/ListaTareas.json", defaultJson);
            }
            var json = File.ReadAllText("assest/ListaTareas.json");
            if (string.IsNullOrWhiteSpace(json))
            {
                var defaultLista = new ListaTareasDTO { Tareas = new List<TareasDTO>() };
                json = JsonSerializer.Serialize(defaultLista, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText("assest/ListaTareas.json", json);
            }
            var lista = JsonSerializer.Deserialize<ListaTareasDTO>(json);
            if (lista != null)
            {
                listaTareas = lista;
                Tareas.Clear();
                listaTareas.Tareas.ForEach(t => Tareas.Add(t));
            }
        }

        private void Serverbb_Tarearesibida(ListaTareasDTO lista)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                bool cambiosRealizados = false;

                foreach (var nuevaTarea in lista.Tareas)
                {
                    // Permitir agregar tareas incluso si tienen un nombre diferente  
                    listaTareas.Tareas.Add(nuevaTarea);
                    Tareas.Add(nuevaTarea);
                    cambiosRealizados = true;
                }

                // Guardar la lista actualizada en el archivo JSON solo si hubo cambios  
                if (cambiosRealizados)
                {
                    try
                    {
                        var json = JsonSerializer.Serialize(listaTareas, new JsonSerializerOptions { WriteIndented = true });
                        File.WriteAllText("assest/ListaTareas.json", json);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al guardar el archivo JSON: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            });
        }

        private void Serverbb_TareaTerminada(ListaTareasDTO lista)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                foreach (var tarea in lista.Tareas)
                {
                    var tareaExistente = Tareas.FirstOrDefault(t => t.Id == tarea.Id);
                    if (tareaExistente != null)
                    {
                        tareaExistente.Estado = EstadoTareas.Terminada;
                    }
                    else
                    {
                        Tareas.Add(tarea);
                    }
                }
            });
        }

        private void Serverbb_TareaProseso(ListaTareasDTO lista)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                foreach (var tarea in lista.Tareas)
                {
                    var tareaExistente = Tareas.FirstOrDefault(t => t.Id == tarea.Id);
                    if (tareaExistente != null)
                    {
                        tareaExistente.Estado = EstadoTareas.EnProgreso;
                    }
                    else
                    {
                        Tareas.Add(tarea);
                    }
                }
            });
        }

        private void Serverbb_Eliminar(ListaTareasDTO lista)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                listaTareas = lista;
                foreach (var tarea in listaTareas.Tareas)
                {
                    var tareaExistente = Tareas.FirstOrDefault(t => t.Id == tarea.Id);
                    if (tareaExistente != null)
                    {
                        Tareas.Remove(tareaExistente);
                    }
                }

            });
        }

        private void Serverbb_Actualizar(ListaTareasDTO lista)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                listaTareas = lista;
                Tareas.Clear();
                foreach (var item in listaTareas.Tareas)
                {
                    Tareas.Add(item);
                }
            });
        }
    }
}
   




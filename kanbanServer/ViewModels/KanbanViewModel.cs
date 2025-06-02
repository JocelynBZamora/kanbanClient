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
using System.Threading;

namespace kanbanServer.ViewModels
{
    public class KanbanViewModel : ObservableObject
    {
        TareaServer serverbb = new();
        public ObservableCollection<TareasDTO> Encargadobb { get; set; } = new();
        public ObservableCollection<TareasActivas> Tareas { get; set; } = new();
        ListaTareasDTO tarealista = new(); 
        public KanbanViewModel()
        {
            serverbb.Tarearesibida += Serverbb_Tarearesibida;
            serverbb.TareaTerminada += Serverbb_TareaTerminada;
            serverbb.TareaProseso += Serverbb_TareaProseso;
            serverbb.eliminar += Serverbb_Eliminar;
            serverbb.TareaPendiente += Serverbb_Pendiente;

            if (File.Exists("Assest/listatareas.json"))
            {
                var json = File.ReadAllText("Assest/listatareas.json");
                var lista= JsonSerializer.Deserialize<ListaTareasDTO>(json);
                if (lista != null)
                {
                    tarealista= lista;
                    //Encargadobb.Clear();
                    tarealista.Tareas.ForEach(t => {Tareas.Add(t); });
                }
            }
        }
        private void Serverbb_Tarearesibida(TareasActivas obj)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var tareaExistente = Tareas.FirstOrDefault(t => t.Titulo == obj.Titulo);
                if (tareaExistente == null)
                {
                    Tareas.Add(obj);
                    tarealista.Tareas.Add(obj);
                    File.WriteAllText("Assest/listatareas.json", JsonSerializer.Serialize(tarealista));
                }
                else
                {
                    tareaExistente.Estado = EstadoTareas.Pendiente;
                    tareaExistente.FechaCreacion = obj.FechaCreacion;
                    tareaExistente.Encargado = obj.Encargado;
                    tareaExistente.Descrip = obj.Descrip;
                    tareaExistente.Titulo = obj.Titulo;
                    tareaExistente.IP = obj.IP;
                    tareaExistente.Id = obj.Id;
                }
            });
        }
        private void Serverbb_Pendiente(TareasActivas dTO)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                // Buscar la tarea activa con el mismo Título y IP  
                var tareaExistente = Tareas.FirstOrDefault(x => x.Titulo == dTO.Titulo && x.IP == dTO.IP);

                if (tareaExistente != null)
                {
                    try
                    {
                        // Actualizar la lista en tarealista.Tareas si corresponde  
                        var tareaLista = tarealista.Tareas.FirstOrDefault(t => t.IP == dTO.IP && t.Titulo == dTO.Titulo);
                        if (tareaLista != null)
                        {
                            tareaLista.Estado = EstadoTareas.Pendiente;
                        }

                        // Guardar los cambios en el archivo JSON  
                        File.WriteAllText("Assest/listatareas.json", JsonSerializer.Serialize(tarealista));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al guardar el archivo JSON: {ex.Message}");
                    }
                }
            });
        }
        private void Serverbb_TareaProseso(TareasActivas dTO)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                // Buscar la tarea activa con el mismo Título y IP  
                var tareaExistente = Tareas.FirstOrDefault(x => x.Titulo == dTO.Titulo && x.IP == dTO.IP);

                if (tareaExistente != null)
                {
                    try
                    {
                        // Actualizar la lista en tarealista.Tareas si corresponde  
                        var tareaLista = tarealista.Tareas.FirstOrDefault(t => t.IP == dTO.IP && t.Titulo == dTO.Titulo);
                        if (tareaLista != null)
                        {
                            tareaLista.Estado = EstadoTareas.EnProgreso;
                        }

                        // Guardar los cambios en el archivo JSON  
                        File.WriteAllText("Assest/listatareas.json", JsonSerializer.Serialize(tarealista));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al guardar el archivo JSON: {ex.Message}");
                    }
                }
            });
        }
        private void Serverbb_TareaTerminada(TareasActivas dTO)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                // Buscar la tarea activa con el mismo Título y IP  
                var tareaExistente = Tareas.FirstOrDefault(x => x.Titulo == dTO.Titulo && x.IP == dTO.IP);

                if (tareaExistente != null)
                {
                    try
                    {
                        // Actualizar la lista en tarealista.Tareas si corresponde  
                        var tareaLista = tarealista.Tareas.FirstOrDefault(t => t.IP == dTO.IP && t.Titulo == dTO.Titulo);
                        if (tareaLista != null)
                        {
                            tareaLista.Estado = EstadoTareas.Terminada;
                        }

                        // Guardar los cambios en el archivo JSON  
                        File.WriteAllText("Assest/listatareas.json", JsonSerializer.Serialize(tarealista));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al guardar el archivo JSON: {ex.Message}");
                    }
                }
            });
        }
        private void Serverbb_Eliminar(TareasActivas dTO)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                // Buscar la tarea en la colección Tareas
                var tareaExistente = Tareas.FirstOrDefault(t =>  t.IP == dTO.IP);

                if (tareaExistente != null)
                {
                    // Eliminar la tarea de la colección Tareas
                    Tareas.Remove(tareaExistente);

                    // Eliminar la tarea de la lista en tarealista.Tareas
                    var tareaLista = tarealista.Tareas.FirstOrDefault(t => t.IP == dTO.IP);
                    if (tareaLista != null)
                    {
                        tarealista.Tareas.Remove(tareaLista);
                    }

                    // Guardar los cambios en el archivo JSON
                    try
                    {
                        File.WriteAllText("Assest/listatareas.json", JsonSerializer.Serialize(tarealista));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al guardar el archivo JSON: {ex.Message}");
                    }
                }
            });
        }
    }

}
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
                Application.Current.Dispatcher.Invoke(() =>
                {
                    // Buscar todas las tareas activas con el mismo Título
                    var tareasExistentes = Tareas.Where(x => x.Titulo == dTO.Titulo).ToList();

                    if (tareasExistentes.Any())
                    {
                        foreach (var tarea in tareasExistentes)
                        {
                            tarea.Estado = EstadoTareas.Pendiente;
                            try
                            {
                                File.WriteAllText("Assest/listatareas.json", JsonSerializer.Serialize(tarealista));
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error al guardar el archivo JSON: {ex.Message}");
                            }
                        }

                        // También actualiza la lista en tarealista.Tareas si corresponde
                        foreach (var tareaLista in tarealista.Tareas.Where(x => x.Id == dTO.Id))
                        {
                            tareaLista.Estado = EstadoTareas.Pendiente;
                        }

                        // Guardar los cambios en el archivo JSON
                        File.WriteAllText("Assest/listatareas.json", JsonSerializer.Serialize(tarealista));
                    }
                });
            });
        }
        private void Serverbb_TareaProseso(TareasActivas dTO)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    // Buscar todas las tareas activas con el mismo Título
                    var tareasExistentes = Tareas.Where(x => x.Titulo == dTO.Titulo).ToList();

                    if (tareasExistentes.Any())
                    {
                        foreach (var tarea in tareasExistentes)
                        {
                            tarea.Estado = EstadoTareas.EnProgreso;
                            try
                            {
                                File.WriteAllText("Assest/listatareas.json", JsonSerializer.Serialize(tarealista));
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error al guardar el archivo JSON: {ex.Message}");
                            }
                        }

                        // También actualiza la lista en tarealista.Tareas si corresponde
                        foreach (var tareaLista in tarealista.Tareas.Where(x => x.Id == dTO.Id))
                        {
                            tareaLista.Estado = EstadoTareas.EnProgreso;
                        }

                        // Guardar los cambios en el archivo JSON
                        File.WriteAllText("Assest/listatareas.json", JsonSerializer.Serialize(tarealista));
                    }
                });
            });
        }
        private void Serverbb_TareaTerminada(TareasActivas dTO)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                // Buscar todas las tareas activas con el mismo Título
                var tareasExistentes = Tareas.Where(x => x.Titulo == dTO.Titulo).ToList();

                if (tareasExistentes.Any())
                {
                    foreach (var tarea in tareasExistentes)
                    {
                        tarea.Estado = EstadoTareas.Terminada;
                        try
                        {
                            File.WriteAllText("Assest/listatareas.json", JsonSerializer.Serialize(tarealista));
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error al guardar el archivo JSON: {ex.Message}");
                        }
                    }

                    // También actualiza la lista en tarealista.Tareas si corresponde
                    foreach (var tareaLista in tarealista.Tareas.Where(x => x.Id == dTO.Id))
                    {
                        tareaLista.Estado = EstadoTareas.Terminada;
                    }

                    // Guardar los cambios en el archivo JSON
                    File.WriteAllText("Assest/listatareas.json", JsonSerializer.Serialize(tarealista));
                }
            });
        }
        private void Serverbb_Eliminar(TareasActivas dTO)
        {
            Application.Current.Dispatcher.Invoke(() => 
            {
                //Deserializo el json 
                var json = File.ReadAllText("Assest/listatareas.json");
                var lista = JsonSerializer.Deserialize<ListaTareasDTO>(json);
                if (lista != null)
                {
                    // Eliminar la tarea de la lista
                    var tareaAEliminar = lista.Tareas.FirstOrDefault(x => x.Id == dTO.Id);
                    if (tareaAEliminar != null)
                    {
                        lista.Tareas.Remove(tareaAEliminar);
                        File.WriteAllText("Assest/listatareas.json", JsonSerializer.Serialize(lista));
                    }
                }
                // Actualizar la colección Tareas
                var tareaExistente = Tareas.FirstOrDefault(x => x.Id == dTO.Id);
                if (tareaExistente != null)
                {
                    Tareas.Remove(tareaExistente);
                }
            });
        }
    }

}
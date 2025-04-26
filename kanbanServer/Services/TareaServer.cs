using kanbanServer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace kanbanServer.Services
{
    public class TareaServer
    {
        HttpListener serverbb = new();
        byte[] TareaIndex;

        public TareaServer()
        {
            serverbb.Prefixes.Add("http://*:19800/kanban/");
            TareaIndex = File.ReadAllBytes("assest/index.html");
            serverbb.Start();
            new Thread(Escuchar) { IsBackground = true }.Start();
        }

        public event Action<ListaTareasDTO>? Tarearesibida, TareaTerminada, TareaProseso, TareaPendiente, eliminar;

        void Escuchar()
        {
            try
            {
                var context = serverbb.GetContext();
                new Thread(Escuchar) { IsBackground = true }.Start();

                if (context.Request.HttpMethod == "GET")
                {
                    switch (context.Request.RawUrl)
                    {
                        case "/kanban/":
                            EnviarDatos(context, TareaIndex, "text/html");
                            break;
                        case "/kanban/TareaEnviada":
                            var tareaEnviada = File.ReadAllBytes("assest/ListaTareas.json");
                            EnviarDatos(context, tareaEnviada, "application/json");
                            break;
                        default:
                            context.Response.StatusCode = 404;
                            break;
                    }
                }
                else if (context.Request.HttpMethod == "POST")
                {
                    switch (context.Request.RawUrl)
                    {
                        case "/kanban/nuevo":
                            LeerTareas(context, Tarearesibida, null);
                            break;
                        case "/kanban/pendiente":
                            LeerTareas(context, TareaProseso, EstadoTareas.Pendiente);
                            break;
                        case "/kanban/enprogreso":
                            LeerTareas(context, eliminar, EstadoTareas.EnProgreso);
                            break;
                        case "/kanban/terminada":
                            LeerTareas(context, TareaTerminada, EstadoTareas.Terminada);
                            break;
                        case "/kanban/eliminar":
                            LeerTareas(context,eliminar, EstadoTareas.Terminada);
                            break;
                        default:
                            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                            break;
                    }
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                }

                context.Response.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void LeerTareas(HttpListenerContext context, Action<ListaTareasDTO>? action, EstadoTareas? e)
        {
            try
            {
                byte[] buffernombre = new byte[context.Request.ContentLength64];
                context.Request.InputStream.Read(buffernombre, 0, buffernombre.Length);
                string json = Encoding.UTF8.GetString(buffernombre);
                var tarea = JsonSerializer.Deserialize<TareasDTO>(json);

                if (tarea != null)
                {
                    tarea.FechaCreacion = DateTime.Now;
                    tarea.Ip = context.Request.RemoteEndPoint.Address.ToString();
                    if (e != null)
                    {
                        tarea.Estado = e.Value;
                    } 
                    var listaTareas = new ListaTareasDTO
                    {
                        Tareas = new List<TareasDTO> { tarea }
                    };

                    action?.Invoke(listaTareas);
                    context.Response.StatusCode = 200;
                }
                else
                {
                    context.Response.StatusCode = 400;
                }

                context.Response.Close();
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error de deserialización: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error general: {ex.Message}");
            }
        }

        private void EnviarDatos(HttpListenerContext context, byte[] buffer, string tipo)
        {
            context.Response.ContentLength64 = buffer.Length;
            context.Response.ContentType = tipo;
            context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            context.Response.StatusCode = 200;
        }
    }
}

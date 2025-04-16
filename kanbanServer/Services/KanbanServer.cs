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
    public class KanbanServer   
    {
        HttpListener servidorbb = new();
        byte[] TareaIndex;
        public event Action<ListaTareasDTO>? Tarearesibida, TareaTerminada, TareaProseso, TareaEn;
        public KanbanServer() { TareaIndex = File.ReadAllBytes("assest/index.html"); }
        public void Iniciar()
        {
            if (!servidorbb.IsListening)
            {
                servidorbb = new();
                servidorbb.Prefixes.Add("http://*:19800/kanban/");
                servidorbb.Start();
                new Thread(Escuchar) { IsBackground = true }.Start();
            }
        }
        public void Escuchar()
        {
            try
            {
                var context = servidorbb.GetContext();
                new Thread(Escuchar) { IsBackground = true }.Start();
                if (context.Request.HttpMethod == "GET")
                {
                    switch (context.Request.RawUrl)
                    {
                        case "/kanban/":
                            context.Response.ContentLength64 = TareaIndex.Length;
                            context.Response.ContentType = "text/html";
                            context.Response.StatusCode = 200;
                            context.Response.OutputStream.Write(TareaIndex, 0, TareaIndex.Length);
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
                        //me va  a traer el nombre del usuario, ip y su tarea
                        case "/kanban/nuevo":
                            Tareas(context, Tarearesibida, null);
                            break;
                        case "/kanban/pendiente":
                            Tareas(context, TareaProseso, EstadoTarea.Pendiente);
                            break;
                        case "/kanban/en":
                            Tareas(context, TareaEn, EstadoTarea.EnProgreso);
                            break;
                        case "/kanban/terminada":
                            Tareas(context, TareaTerminada, EstadoTarea.Terminada);
                            break;
                        default:
                            context.Response.StatusCode = (int)HttpStatusCode.NotFound; break;
                    }
                }
                context.Response.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void Tareas(HttpListenerContext context, Action<ListaTareasDTO>? tarearesibida, EstadoTarea? e)
        {
            byte[] buffernombre = new byte[context.Request.ContentLength64];
            context.Request.InputStream.Read(buffernombre, 0, buffernombre.Length);
            string json = Encoding.UTF8.GetString(buffernombre);

            var usuario = JsonSerializer.Deserialize<ListaTareasDTO>(json);

            if (usuario != null)
            {
                usuario.FechaCreacion = DateTime.Now;
                usuario.Ip = context.Request.RemoteEndPoint.ToString();
                if (e != null)
                {
                    usuario.Estado = e.Value;
                }
                tarearesibida?.Invoke(usuario);
            }
        }
        public void Detener()
        {
            if (servidorbb.IsListening)
            {
                servidorbb.Stop();
            }
        }
    }
}

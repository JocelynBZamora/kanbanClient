using kanbanServer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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
                        case "/kanban/nuevo":

                            break;
                        case "/kanban/pendiente":
                            break;
                        case "/kanban/en":
                            break;
                        case "/kanban/terminada":
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
        public void Detener()
        {
            if (servidorbb.IsListening)
            {
                servidorbb.Stop();
            }
        }
    }
}

using kanbanServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kanbanServer.Helper
{
    public static class ComunicacionHelper
    {
        public static event Action<ListaTareasDTO>? Tarearesibida,
            TareaTerminada,
            TareaProseso,
            actualizar,
            eliminar;
        public static void NotificarTareaRecibida(ListaTareasDTO tarea)
        {
            Tarearesibida?.Invoke(tarea);
        }
        public static void NotificarTareaTerminada(ListaTareasDTO tarea)
        {
            TareaTerminada?.Invoke(tarea);
        }
        public static void NotificarTareaEnProceso(ListaTareasDTO tarea)
        {
            TareaProseso?.Invoke(tarea);
        }
        public static void NotificarActualizar(ListaTareasDTO tarea)
        {
            actualizar?.Invoke(tarea);
        }
        public static void NotificarEliminar(ListaTareasDTO tarea)
        {
            eliminar?.Invoke(tarea);
        }
    }
}

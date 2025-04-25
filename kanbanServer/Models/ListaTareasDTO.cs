using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kanbanServer.Models
{
    public class ListaTareasDTO
    {
        public List<TareasDTO> Tareas { get; set; } = new();
    }
}

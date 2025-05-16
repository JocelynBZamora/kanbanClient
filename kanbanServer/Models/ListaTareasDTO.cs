using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kanbanServer.Models
{
    public class ListaTareasDTO
    { 
        public List<TareasActivas> Tareas { get; set; } = new();
        public List<TareasDTO> TareasActivas { get; set;} = new();
    }
}

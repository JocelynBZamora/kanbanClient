using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using kanbanServer.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace kanbanServer.ViewModels
{
    public class KanbanViewModels : ObservableObject
    {
        KanbanServer serverbb = new();
        public ICommand IniciarCommand { get; }
        public ICommand DetenerCommand { get; }
        public KanbanViewModels()
        {
            IniciarCommand = new RelayCommand(Iniciar);
            DetenerCommand = new RelayCommand(Detener);
        }
        void Iniciar()
        {
            try
            {
                serverbb.Iniciar();
            }
            catch (HttpListenerException ex)
            {
                if (ex.Message.StartsWith("Acceso no autorizado"))
                {
                    ProcessStartInfo p = new ProcessStartInfo
                    {
                        FileName = "netsh.exe",
                        Arguments = "http add urlacl url=http://*:19800/kanban/ user=Everyone",
                        UseShellExecute = true,
                        CreateNoWindow = false,
                        Verb = "runas"
                    };
                    var res = Process.Start(p);
                    if (res != null)
                    {
                        res.WaitForExit();
                        if (res.ExitCode == 0)
                        {
                            serverbb.Iniciar();
                        }
                    }
                }
            }
        }
        void Detener()
        {

            serverbb.Detener();
        }
    }
}

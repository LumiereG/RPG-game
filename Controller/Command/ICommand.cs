using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2.Command
{
    public interface ICommand
    {
        bool Execute(GameModelServer model);
        int PlayerId { get; set; }
    }
}

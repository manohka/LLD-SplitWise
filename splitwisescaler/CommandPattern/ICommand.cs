using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace splitwisescaler.CommandPattern
{
    public interface ICommand
    {
        void Execute();
        bool CanExecute();
        void Undo();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2
{
    public interface ISubject
    {
        void Attach(IPotionObserver observer);
        void Detach(IPotionObserver observer);
        void Notify();
    }
}

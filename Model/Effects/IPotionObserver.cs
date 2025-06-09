using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;

namespace Game2
{
    public interface IPotionObserver
    {
        void Update(ISubject subject);
    }
}

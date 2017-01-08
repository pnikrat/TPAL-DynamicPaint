using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternalShared
{
    public interface IHost
    {
        void SetCanvasWidth(int width);
        void SetCanvasHeight(int height);
    }
}

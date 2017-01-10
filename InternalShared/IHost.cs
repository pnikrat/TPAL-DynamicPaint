using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace InternalShared
{
    public interface IHost
    {
        void SetCanvasWidth(int width);
        void SetCanvasHeight(int height);
        int GetCanvasWidth();
        int GetCanvasHeight();
        Brush GetCanvasBackground();
        void SetCanvasBackground(Brush background);
    }
}

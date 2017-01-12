using System.Windows.Media;

namespace InternalShared
{
    /// <summary>
    /// Interfejs przez który pluginy oddziałują na główną aplikację
    /// </summary>
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

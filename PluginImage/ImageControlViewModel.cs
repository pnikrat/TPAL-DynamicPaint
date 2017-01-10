using InternalShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Drawing;

namespace PluginImage
{
    public class ImageControlViewModel : ViewModelBase
    {
        public IHost hostInterface { get; set; }

        private int _imageWidth;
        private int _imageHeight;

        RelayCommand _imageOperationCommand;

        public ICommand ImageOperationCommand
        {
            get
            {
                if (_imageOperationCommand == null)
                    _imageOperationCommand = new RelayCommand(param => this.ImageOperation());
                return _imageOperationCommand;
            }
        }

        public void ImageOperation()
        {
            _imageWidth = hostInterface.GetCanvasWidth();
            _imageHeight = hostInterface.GetCanvasHeight();

            //konwersja z Brush do BitmapSource (WPF)
            //'ramka' bitmapy
            RenderTargetBitmap target = new RenderTargetBitmap(_imageWidth, _imageHeight, 96d, 96d, PixelFormats.Default);
            System.Windows.Media.Brush backgroundBrush = hostInterface.GetCanvasBackground();

            //'zawartosc' bitmapy
            DrawingVisual visu = new DrawingVisual();
            DrawingContext cont = visu.RenderOpen();
            cont.DrawRectangle(backgroundBrush, null, new Rect(new System.Windows.Point(0, 0), new System.Windows.Point(_imageWidth, _imageHeight)));
            cont.Close();
            target.Render(visu);
            BitmapSource src = target;

            //konwersja z BitmapSource(WPF) nad Bitmap(GDI)
            Bitmap srcBitmap = ImageProcessing.ConvertToBitmap(src);

            //negatyw
            ImageProcessing.BitmapInvertColors(srcBitmap);

            //konwersja z Bitmap(GDI) na ImageSource(WPF)
            ImageSource srcImage = ImageProcessing.ConvertToImageSource(srcBitmap);

            //z ImageSource tworzony jest Brush wysyłany do głównej aplikacji
            System.Windows.Media.Brush backgroundResultBrush = new ImageBrush(srcImage);

            hostInterface.SetCanvasBackground(backgroundResultBrush);
        }


    }
}

using System;
using System.Collections.Generic;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.WIC;
using SharpDX.Windows;

namespace Miner.DirectX
{
    /// <summary>
    /// Класс, реализующий DirectX. Содержит фабрики, кисти, настройки окна, метод загрузки спрайта и их хранение
    /// </summary>
    public class DirectX2D : IDisposable
    {
        //2D фабрика
        private SharpDX.Direct2D1.Factory _factory;
        public SharpDX.Direct2D1.Factory Factory { get => _factory; }

        //фабрика для изображений
        private ImagingFactory _imgFactory;
        public ImagingFactory ImgFactory { get => _imgFactory; }

        //фабрика для текста
        private SharpDX.DirectWrite.Factory _textFactory;
        public SharpDX.DirectWrite.Factory TextFactory { get => _textFactory; }

        //объект отрисовки
        private WindowRenderTarget _renderTarget;
        public WindowRenderTarget RenderTarget { get => _renderTarget; }

        //формат текста (его параметры (шрифт, размер))
        private TextFormat _textFormatCenter;
        public TextFormat TextFormatCenter { get => _textFormatCenter; }

        private TextFormat _textFormatLeft;
        public TextFormat TextFormatLeft { get => _textFormatLeft; }

        private TextFormat _textFormatRight;
        public TextFormat TextFormatRight { get => _textFormatRight; }

        //кисть для рисования текста (мины, если они есть и для rectangle игровых объектов)
        private Brush _brush;
        public Brush Brush { get => _brush; }

        //кисть для рисования текста (используется при выводе счета мин, если их количество = 0)
        private Brush _brushRed;
        public Brush BrushRed { get => _brushRed; }

        private Brush _brushBlue;
        public Brush BrushBlue { get => _brushBlue; }

        //коллекция спрайтов и картинок
        private List<SharpDX.Direct2D1.Bitmap> _bitmap;
        public List<SharpDX.Direct2D1.Bitmap> Bitmaps { get => _bitmap; }
        
        public DirectX2D(RenderForm form)
        {
            _factory = new SharpDX.Direct2D1.Factory();
            _imgFactory = new ImagingFactory();
            _textFactory = new SharpDX.DirectWrite.Factory();

            //настраиваем наше поле игры
            RenderTargetProperties renderProp = new RenderTargetProperties();
            HwndRenderTargetProperties hwndProp = new HwndRenderTargetProperties()
            {
                Hwnd = form.Handle,
                PixelSize = new Size2(form.ClientSize.Width, form.ClientSize.Height),
                PresentOptions = PresentOptions.None
            };

            _renderTarget = new WindowRenderTarget(_factory, renderProp, hwndProp);

            _textFormatCenter = new TextFormat(_textFactory, "Arial", 24);
            _textFormatCenter.ParagraphAlignment = ParagraphAlignment.Center;
            _textFormatCenter.TextAlignment = TextAlignment.Center;

            _textFormatLeft = new TextFormat(_textFactory, "Arial", 24);
            _textFormatLeft.ParagraphAlignment = ParagraphAlignment.Center;
            _textFormatLeft.TextAlignment = TextAlignment.Justified;

            _textFormatRight = new TextFormat(_textFactory, "Arial", 24);
            _textFormatRight.ParagraphAlignment = ParagraphAlignment.Center;
            _textFormatRight.TextAlignment = TextAlignment.Trailing;

            //кисти (игровое поле, цвет кисти)
            _brush = new SolidColorBrush(_renderTarget, Color.White);
            _brushRed = new SolidColorBrush(_renderTarget, Color.Red);
            _brushBlue = new SolidColorBrush(_renderTarget, Color.Blue);
        }

        /// <summary>
        /// Метод для загрузки изображений, их преоброзования и дальнейшее хранение
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <returns>Возвращает номер загруженного спрайта</returns>
        public int GetImageLoad(string path)
        {
            BitmapDecoder decoder = new BitmapDecoder(_imgFactory, path, DecodeOptions.CacheOnLoad);
            BitmapFrameDecode frame = decoder.GetFrame(0);
            FormatConverter converter = new FormatConverter(_imgFactory);
            converter.Initialize(frame, SharpDX.WIC.PixelFormat.Format32bppPRGBA, BitmapDitherType.Ordered4x4, null, 0.0, BitmapPaletteType.Custom);
            SharpDX.Direct2D1.Bitmap bitmap = SharpDX.Direct2D1.Bitmap.FromWicBitmap(_renderTarget, converter);

            Utilities.Dispose(ref converter);
            Utilities.Dispose(ref frame);
            Utilities.Dispose(ref decoder);


            if (_bitmap == null)
                _bitmap = new List<SharpDX.Direct2D1.Bitmap>();
            _bitmap.Add(bitmap);

            return _bitmap.Count - 1;
        }

         /// <summary>
         /// Освобождение неупровляемых ресурсов
         /// </summary>
        public void Dispose()
        {
            for (int i = _bitmap.Count - 1; i >= 0; i--)
            {
                SharpDX.Direct2D1.Bitmap bitmap = _bitmap[i];
                _bitmap.RemoveAt(i);
                Utilities.Dispose(ref bitmap);
            }

            Utilities.Dispose(ref _brush);
            Utilities.Dispose(ref _textFormatCenter);
            Utilities.Dispose(ref _imgFactory);
            Utilities.Dispose(ref _renderTarget);
            Utilities.Dispose(ref _factory);
        }
    }
}

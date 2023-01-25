#nullable enable
using System;
using Foundation;
using SkiaSharp;
using SkiaSharp.Views.iOS;
using UIKit;
using ImageCaching.Nuke;

namespace Sample
{
    public partial class ViewController : UIViewController
    {
        private SKBitmap _bitmap;
        private readonly NSUrl _bottomImageUrl = new NSUrl("https://placekitten.com/g/1000/1000"); 
        private readonly NSUrl _topImageUrl = new NSUrl("https://placekitten.com/g/300/300");
        private readonly Prefetcher _imagePrefetcher = new Prefetcher();

        private UIButton _streamButton, _placeholderButton, _clearButton, _prefetchButton, _dumpCacheStateButton;
        private UIImageView _imageView;
        private SKCanvasView _skiaCanvasView;
        private UIStackView _bottomStackView;
        private UILabel _cacheStatusLabel;
        
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        private void LogCacheState()
        {
            var msg =
                $"👋 Images in cache: Top->{ImagePipeline.Shared.IsCachedFor(_topImageUrl)}, Bottom->{ImagePipeline.Shared.IsCachedFor(_bottomImageUrl)}";
            System.Console.WriteLine(msg);

            if (_cacheStatusLabel != null)
            {
                _cacheStatusLabel.Text = msg;
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            // Remove to use default, in-memory cache
            ImagePipeline.SetupWithDataCache();

            _bottomStackView = new UIStackView
            {
                Axis = UILayoutConstraintAxis.Vertical,
                Spacing = 8f,
                Alignment = UIStackViewAlignment.Fill,
                Distribution = UIStackViewDistribution.FillEqually,
                TranslatesAutoresizingMaskIntoConstraints = false
            };
            
            Add(_bottomStackView);
            
            _imageView = new UIImageView
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                BackgroundColor = UIColor.Gray
            };
            
            Add(_imageView);

            _placeholderButton = AddButton("Load with placeholder");
            Add(_placeholderButton);
            
            _placeholderButton.TouchUpInside += (s, e) =>
            {
                ImageCache.Shared.RemoveAll();
                
                ImagePipeline.Shared.LoadImageWithUrl(
                    _topImageUrl,
                    UIImage.FromBundle("Placeholder"),
                    null,
                    _imageView);
            };

            _skiaCanvasView = new SKCanvasView
            {
                TranslatesAutoresizingMaskIntoConstraints = false
            };
            _skiaCanvasView.PaintSurface += OnPaintSurface;
            Add(_skiaCanvasView);

            _streamButton = AddButton("Load streamed image");
            Add(_streamButton);
            _streamButton.TouchUpInside += (s, e) =>
            {
                ImagePipeline.Shared.LoadDataWithUrl(_bottomImageUrl, (data, response) =>
                {
                    using var dataStream = data.AsStream();
                    _bitmap = SKBitmap.Decode(dataStream);

                    _skiaCanvasView.LayoutSubviews();
                });
            };

            _clearButton = AddButton("Clear caches & images");
            _clearButton.TouchUpInside += (sender, args) =>
            {
                ClearDiskCache();
                _bitmap = null; 
                _skiaCanvasView.LayoutSubviews();
                 _imageView.Image = null;
            };
            
            _prefetchButton = AddButton("Prefetch above images");
            _prefetchButton.TouchUpInside += (sender, args) =>
            {
                StartPrefetchingImages();
            };

            _dumpCacheStateButton = AddButton("Show cache status");
            _dumpCacheStateButton.TouchUpInside += (sender, args) =>
            {
                LogCacheState();
            };
            
            _cacheStatusLabel = new UILabel(){ Lines = 0, TranslatesAutoresizingMaskIntoConstraints = false};
            
            _bottomStackView.AddArrangedSubview(_clearButton);
            _bottomStackView.AddArrangedSubview(_prefetchButton);
            _bottomStackView.AddArrangedSubview(_dumpCacheStateButton);
            
            Add(_cacheStatusLabel);

            AddConstraints();
        }

        private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            e.Surface.Canvas.Clear(SKColors.Gray);
            
            if (_bitmap == null)
                return;
            
            e.Surface.Canvas.DrawBitmap(_bitmap, SKPoint.Empty);
        }

        private void AddConstraints()
        {
            _imageView.TopAnchor.ConstraintEqualTo(View.SafeAreaLayoutGuide.TopAnchor, 16f).Active = true;
            _imageView.WidthAnchor.ConstraintEqualTo(View.WidthAnchor, 0.5f).Active = true;
            _imageView.HeightAnchor.ConstraintEqualTo(_imageView.WidthAnchor).Active = true;
            _imageView.CenterXAnchor.ConstraintEqualTo(View.CenterXAnchor).Active = true;

            _placeholderButton.TopAnchor.ConstraintEqualTo(_imageView.BottomAnchor, 16f).Active = true;
            _placeholderButton.WidthAnchor.ConstraintEqualTo(View.WidthAnchor, 0.95f).Active = true;
            _placeholderButton.HeightAnchor.ConstraintEqualTo(30f).Active = true;
            _placeholderButton.CenterXAnchor.ConstraintEqualTo(View.CenterXAnchor).Active = true;

            _skiaCanvasView.CenterXAnchor.ConstraintEqualTo(View.CenterXAnchor).Active = true;
            _skiaCanvasView.TopAnchor.ConstraintEqualTo(_placeholderButton.BottomAnchor, 16f).Active = true;
            _skiaCanvasView.WidthAnchor.ConstraintEqualTo(View.WidthAnchor, 0.5f).Active = true;
            _skiaCanvasView.HeightAnchor.ConstraintEqualTo(_skiaCanvasView.WidthAnchor).Active = true;

            _streamButton.TopAnchor.ConstraintEqualTo(_skiaCanvasView.BottomAnchor, 16f).Active = true;
            _streamButton.WidthAnchor.ConstraintEqualTo(View.WidthAnchor, 0.95f).Active = true;
            _streamButton.HeightAnchor.ConstraintEqualTo(30f).Active = true;
            _streamButton.CenterXAnchor.ConstraintEqualTo(View.CenterXAnchor).Active = true;

            _bottomStackView.TopAnchor.ConstraintEqualTo(_streamButton.BottomAnchor, 16f).Active = true;
            _bottomStackView.HeightAnchor.ConstraintEqualTo(106).Active = true;
            _bottomStackView.WidthAnchor.ConstraintEqualTo(View.WidthAnchor, 0.95f).Active = true;
            _bottomStackView.CenterXAnchor.ConstraintEqualTo(View.CenterXAnchor).Active = true;

            _cacheStatusLabel.TopAnchor.ConstraintEqualTo(_bottomStackView.BottomAnchor).Active = true;
            _cacheStatusLabel.WidthAnchor.ConstraintEqualTo(View.WidthAnchor, 0.95f).Active = true;
            _cacheStatusLabel.CenterXAnchor.ConstraintEqualTo(View.CenterXAnchor).Active = true;
            
        }

        private UIButton AddButton(string title)
        {
            var button = new UIButton
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                BackgroundColor = UIColor.Orange
            };
            button.SetTitleColor(UIColor.Black, UIControlState.Normal);
            button.SetTitle(title, UIControlState.Normal);

            return button;
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        private void StartPrefetchingImages()
        {
            _imagePrefetcher.StartPrefetchingWith(new[] {_bottomImageUrl, _topImageUrl});
        }

        private void ClearDiskCache()
        {
            ImagePipeline.Shared.RemoveAllCaches();
        }

        private void RemoveFromCache(NSUrl url)
        {
            ImagePipeline.Shared.RemoveImageFromCacheFor(url);
        }

        private UIImage? GetFromCache(NSUrl url) => ImagePipeline.Shared.GetCachedImageFor(url);
    }
}
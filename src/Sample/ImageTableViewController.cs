using System;
using CoreGraphics;
using Foundation;
using UIKit;
using ImageCaching.Nuke;
using ObjCRuntime;

namespace Sample;

public class ImageTableViewController : UIViewController
{
    public ImageTableViewController()
    {

    }

    public override void LoadView()
    {
        base.LoadView();

        var layout = new UICollectionViewFlowLayout
        {
            ItemSize = new CGSize(UIScreen.MainScreen.Bounds.Width, 100),
            MinimumInteritemSpacing = 8,
            ScrollDirection = UICollectionViewScrollDirection.Vertical
        };
        var collectionView = new UICollectionView(CGRect.Empty, layout)
        {
            TranslatesAutoresizingMaskIntoConstraints = false,
            DataSource = new ImageDataSource()
        };

        collectionView.RegisterClassForCell(typeof(ImageCell), nameof(ImageCell));

        Add(collectionView);

        collectionView.TopAnchor.ConstraintEqualTo(View.TopAnchor).Active = true;
        collectionView.BottomAnchor.ConstraintEqualTo(View.BottomAnchor).Active = true;
        collectionView.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor).Active = true;
        collectionView.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor).Active = true;
    }
}

public class ImageDataSource : UICollectionViewDataSource
{
    private readonly string[] _items;

    public ImageDataSource()
    {
        _items =
        [
            "https://www.dummyimage.com/600x400/e30ce3/8bd61a.png&text=Item1",
            "https://www.dummyimage.com/600x400/e30ce3/8bd61a.png&text=Item1",
            "https://www.dummyimage.com/600x400/e30ce3/8bd61a.png&text=Item7",
            "https://www.dummyimage.com/600x400/e30ce3/8bd61a.png&text=Item7",
            "https://www.dummyimage.com/600x400/e30ce3/8bd61a.png&text=Item2",
            "https://www.dummyimage.com/600x400/e30ce3/8bd61a.png&text=Item1",
            "https://www.dummyimage.com/600x400/e30ce3/8bd61a.png&text=Item1",
            "https://www.dummyimage.com/600x400/e30ce3/8bd61a.png&text=Item7",
            "https://www.dummyimage.com/600x400/e30ce3/8bd61a.png&text=Item7",
            "https://www.dummyimage.com/600x400/e30ce3/8bd61a.png&text=Item3",
            "https://www.dummyimage.com/600x400/e30ce3/8bd61a.png&text=Item1",
            "https://www.dummyimage.com/600x400/e30ce3/8bd61a.png&text=Item1",
            "https://www.dummyimage.com/600x400/e30ce3/8bd61a.png&text=Item3",
            "https://www.dummyimage.com/600x400/e30ce3/8bd61a.png&text=Item4",
            "https://www.dummyimage.com/600x400/e30ce3/8bd61a.png&text=Item5",
            "https://www.dummyimage.com/600x400/e30ce3/8bd61a.png&text=Item1",
            "https://www.dummyimage.com/600x400/e30ce3/8bd61a.png&text=Item1",
            "https://www.dummyimage.com/600x400/e30ce3/8bd61a.png&text=Item6",
            "https://www.dummyimage.com/600x400/e30ce3/8bd61a.png&text=Item7",
            "https://www.dummyimage.com/600x400/e30ce3/8bd61a.png&text=Item7",
            "https://www.dummyimage.com/600x400/e30ce3/8bd61a.png&text=Item1",
            "https://www.dummyimage.com/600x400/e30ce3/8bd61a.png&text=Item3",
            "https://www.dummyimage.com/600x400/e30ce3/8bd61a.png&text=Item4",
            "https://www.dummyimage.com/600x400/e30ce3/8bd61a.png&text=Item5",
            "https://www.dummyimage.com/600x400/e30ce3/8bd61a.png&text=Item1",
            "https://www.dummyimage.com/600x400/e30ce3/8bd61a.png&text=Item1",
            "https://www.dummyimage.com/600x400/e30ce3/8bd61a.png&text=Item6",
            "https://www.dummyimage.com/600x400/e30ce3/8bd61a.png&text=Item7",
            "https://www.dummyimage.com/600x400/e30ce3/8bd61a.png&text=Item7",
            "https://www.dummyimage.com/600x400/e30ce3/8bd61a.png&text=Item2",
            "https://www.dummyimage.com/600x400/e30ce3/8bd61a.png&text=Item1",
            "https://www.dummyimage.com/600x400/e30ce3/8bd61a.png&text=Item1",
            "https://www.dummyimage.com/600x400/e30ce3/8bd61a.png&text=Item7",
            "https://www.dummyimage.com/600x400/e30ce3/8bd61a.png&text=Item7",
            "https://www.dummyimage.com/600x400/e30ce3/8bd61a.png&text=Item2",
            "https://www.dummyimage.com/600x400/e30ce3/8bd61a.png&text=Item1",
            "https://www.dummyimage.com/600x400/e30ce3/8bd61a.png&text=Item1",
            "https://www.dummyimage.com/600x400/e30ce3/8bd61a.png&text=Item7",
        ];
    }

    public override IntPtr GetItemsCount(UICollectionView collectionView, IntPtr section)
    {
        return _items.Length;
    }

    public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
    {
        var cell = collectionView.DequeueReusableCell(nameof(ImageCell), indexPath);
        if (cell is ImageCell imageCell)
        {
            imageCell.LoadImage(_items[indexPath.Row]);
        }

        return (UICollectionViewCell)cell;
    }
}

public sealed class ImageCell : UICollectionViewCell
{
    private readonly UIImageView _imageView;
    private long? _taskId;

    public ImageCell(NativeHandle handle) : base(handle)
    {
        _imageView = new UIImageView
        {
            TranslatesAutoresizingMaskIntoConstraints = false,
            ContentMode = UIViewContentMode.ScaleToFill
        };

        ContentView.Add(_imageView);
        ContentView.BackgroundColor = UIColor.Gray;

        _imageView.TopAnchor.ConstraintEqualTo(ContentView.TopAnchor).Active = true;
        _imageView.BottomAnchor.ConstraintEqualTo(ContentView.BottomAnchor).Active = true;
        _imageView.LeadingAnchor.ConstraintEqualTo(ContentView.LeadingAnchor).Active = true;
        _imageView.TrailingAnchor.ConstraintEqualTo(ContentView.TrailingAnchor).Active = true;
    }

    public void LoadImage(string url)
    {
        _taskId = ImagePipeline.Shared.LoadImageWithUrl(
            new NSUrl(url),
            UIImage.FromBundle("Placeholder"),
            null,
            _imageView);
    }

    public override void PrepareForReuse()
    {
        if (_taskId.HasValue)
            ImagePipeline.Shared.CancelTask(_taskId.Value);

        base.PrepareForReuse();
    }
}

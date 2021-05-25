using Foundation;
using System;
using UIKit;

namespace Xamarin.Nuke.Sample
{
    public partial class ViewController : UIViewController
    {
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.


            var image = new UIImageView
            {
                TranslatesAutoresizingMaskIntoConstraints = false
            };
            Add(image);

            image.WidthAnchor.ConstraintEqualTo(View.WidthAnchor).Active = true;
            image.HeightAnchor.ConstraintEqualTo(View.WidthAnchor).Active = true;
            image.CenterXAnchor.ConstraintEqualTo(View.CenterXAnchor).Active = true;
            image.CenterYAnchor.ConstraintEqualTo(View.CenterYAnchor).Active = true;

            ImagePipeline.Shared.LoadImageWithUrl(
                new NSUrl("https://placekitten.com/g/300/300"),
                (img, url) => image.Image = img);
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}
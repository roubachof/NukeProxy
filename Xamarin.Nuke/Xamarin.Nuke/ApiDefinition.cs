using System;
using Foundation;
using ObjCRuntime;
using UIKit;

namespace Xamarin.Nuke
{
    // @interface DataLoader : NSObject
    [BaseType(typeof(NSObject))]
    interface DataLoader
    {
        // @property (readonly, nonatomic, strong, class) DataLoader * _Nonnull shared;
        [Static]
        [Export("shared", ArgumentSemantic.Strong)]
        DataLoader Shared { get; }

        // -(void)removeAllCachedResponses;
        [Export("removeAllCachedResponses")]
        void RemoveAllCachedResponses();
    }

    // @interface ImageCache : NSObject
    [BaseType(typeof(NSObject))]
    interface ImageCache
    {
        // @property (readonly, nonatomic, strong, class) ImageCache * _Nonnull shared;
        [Static]
        [Export("shared", ArgumentSemantic.Strong)]
        ImageCache Shared { get; }

        // -(void)removeAll;
        [Export("removeAll")]
        void RemoveAll();
    }

    // @interface ImagePipeline : NSObject
    [BaseType(typeof(NSObject))]
    interface ImagePipeline
    {
        // @property (nonatomic, strong, class) ImagePipeline * _Nonnull shared;
        [Static]
        [Export("shared", ArgumentSemantic.Strong)]
        ImagePipeline Shared { get; }

        // -(void)loadImageWithUrl:(NSURL * _Nonnull)url onCompleted:(void (^ _Nonnull)(UIImage * _Nullable, NSString * _Nonnull))onCompleted;
        [Export("loadImageWithUrl:onCompleted:")]
        void LoadImageWithUrl(NSUrl url, Action<UIImage, NSString> onCompleted);
    }
}

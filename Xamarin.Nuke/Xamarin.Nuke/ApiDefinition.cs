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
		// @property (readonly, nonatomic, strong, class) ImagePipeline * _Nonnull shared;
		[Static]
		[Export("shared", ArgumentSemantic.Strong)]
		ImagePipeline Shared { get; }

		// -(void)loadImageWithUrl:(NSURL * _Nonnull)url onCompleted:(void (^ _Nonnull)(UIImage * _Nullable, NSString * _Nonnull))onCompleted;
		[Export("loadImageWithUrl:onCompleted:")]
		void LoadImageWithUrl(NSUrl url, Action<UIImage, NSString> onCompleted);

		// -(void)loadImageWithUrl:(NSURL * _Nonnull)url placeholder:(UIImage * _Nullable)placeholder errorImage:(UIImage * _Nullable)errorImage into:(UIImageView * _Nonnull)into;
		[Export("loadImageWithUrl:placeholder:errorImage:into:")]
		void LoadImageWithUrl(NSUrl url, [NullAllowed] UIImage placeholder, [NullAllowed] UIImage errorImage,
			UIImageView into);

		// -(void)loadImageWithUrl:(NSURL * _Nonnull)url imageIdKey:(NSString * _Nonnull)imageIdKey placeholder:(UIImage * _Nullable)placeholder errorImage:(UIImage * _Nullable)errorImage into:(UIImageView * _Nonnull)into;
		[Export("loadImageWithUrl:imageIdKey:placeholder:errorImage:into:")]
		void LoadImageWithUrl(NSUrl url, string imageIdKey, [NullAllowed] UIImage placeholder,
			[NullAllowed] UIImage errorImage, UIImageView into);

		// -(void)loadDataWithUrl:(NSURL * _Nonnull)url onCompleted:(void (^ _Nonnull)(NSData * _Nullable, NSURLResponse * _Nullable))onCompleted;
		[Export("loadDataWithUrl:onCompleted:")]
		void LoadDataWithUrl(NSUrl url, Action<NSData, NSUrlResponse> onCompleted);

		// -(void)loadDataWithUrl:(NSURL * _Nonnull)url imageIdKey:(NSString * _Nullable)imageIdKey reloadIgnoringCachedData:(BOOL)reloadIgnoringCachedData onCompleted:(void (^ _Nonnull)(NSData * _Nullable, NSURLResponse * _Nullable))onCompleted;
		[Export("loadDataWithUrl:imageIdKey:reloadIgnoringCachedData:onCompleted:")]
		void LoadDataWithUrl(NSUrl url, [NullAllowed] string imageIdKey, bool reloadIgnoringCachedData,
			Action<NSData, NSUrlResponse> onCompleted);
	}

	// @interface Prefetcher : NSObject
	[BaseType(typeof(NSObject))]
	interface Prefetcher
	{
		// -(void)startPrefetchingWith:(NSArray<NSURL *> * _Nonnull)with;
		[Export("startPrefetchingWith:")]
		void StartPrefetchingWith(NSUrl[] with);

		// -(void)stopPrefetchingWith:(NSArray<NSURL *> * _Nonnull)with;
		[Export("stopPrefetchingWith:")]
		void StopPrefetchingWith(NSUrl[] with);

		// -(void)stopPrefetching;
		[Export("stopPrefetching")]
		void StopPrefetching();

		// -(void)pause;
		[Export("pause")]
		void Pause();

		// -(void)unPause;
		[Export("unPause")]
		void UnPause();
	}
}
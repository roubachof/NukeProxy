//
//  NukeProxy.swift
//  NukeProxy
//
//  Created by Jean-Marie Alfonsi on 06/03/2020.
//  Copyright © 2020 Jean-Marie Alfonsi. All rights reserved.
//

import Foundation
import UIKit
import Nuke


@objc(ImagePipeline)
public class ImagePipeline : NSObject {
    
    @objc
    public static let shared = ImagePipeline()
    
    @objc
    public static func setupWithDataCache(){
        Nuke.ImagePipeline.shared = Nuke.ImagePipeline(configuration: .withDataCache)
    }
    
    @objc
    public func isCached(for url: URL) -> Bool {
        return Nuke.ImagePipeline.shared.cache.containsCachedImage(for: ImageRequest(url: url))
    }
    
    @objc
    public func getCachedImage(for url: URL) -> UIImage? {
        guard let cached = Nuke.ImagePipeline.shared.cache.cachedImage(for: ImageRequest(url: url)) else {
            return nil
        }
        
        return cached.image
    }
    
    @objc
    public func removeImageFromCache(for url: URL) {
        Nuke.ImagePipeline.shared.cache.removeCachedData(for: ImageRequest(url: url))
    }
    
    @objc
    public func removeAllCaches() {
        Nuke.ImagePipeline.shared.cache.removeAll()
    }
    
    @objc
    public func loadImage(url: URL, onCompleted: @escaping (UIImage?, String) -> Void) {
        _ = Nuke.ImagePipeline.shared.loadImage(
            with: url,
            progress: nil,
            completion: { result in
                switch result {
                case let .success(response):
                    onCompleted(response.image, "success")
                case let .failure(error):
                    onCompleted(nil, error.localizedDescription)
                }
            }
        )
    }
    
    
    @objc
    public func loadImage(url: URL, placeholder: UIImage?, errorImage: UIImage?, into: UIImageView) {
        let options = ImageLoadingOptions(placeholder:placeholder, failureImage: errorImage)
        Nuke.loadImage(with: url, options: options, into: into)
    }
    
    @objc
    public func loadImage(url: URL, imageIdKey: String, placeholder: UIImage?, errorImage: UIImage?, into: UIImageView) {
        let options = ImageLoadingOptions(placeholder: placeholder, failureImage: errorImage)
        
        Nuke.loadImage(with: ImageRequest(
            url: url,
            userInfo: [.imageIdKey: imageIdKey]
        ), options: options, into: into)
    }
    
    @objc
    public func loadData(url: URL, onCompleted: @escaping (Data?, URLResponse?) -> Void) {
        loadData(url: url, imageIdKey: nil, reloadIgnoringCachedData: false, onCompleted: onCompleted)
    }
    
    @objc
    public func loadData(url: URL, imageIdKey: String?, reloadIgnoringCachedData: Bool, onCompleted: @escaping (Data?, URLResponse?) -> Void) {
        _ = Nuke.ImagePipeline.shared.loadData(
            with: ImageRequest(
                url: url,
                options: reloadIgnoringCachedData ? [.reloadIgnoringCachedData] : [],
                userInfo: [.imageIdKey: imageIdKey]
            ),
            completion: { result in
                switch result {
                case let .success(response):
                    onCompleted(response.data, response.response)
                case .failure(_):
                    onCompleted(nil, nil)
                }
            }
        )
    }
}

@objc(ImageCache)
public final class ImageCache: NSObject {
    
    @objc
    public static let shared = ImageCache()
    
    @objc
    public func removeAll() {
        Nuke.ImageCache.shared.removeAll()
    }
}

@objc(DataLoader)
public final class DataLoader: NSObject {
    
    @objc
    public static let shared = DataLoader()
    
    @objc
    public static let sharedUrlCache = Nuke.DataLoader.sharedUrlCache
}

@objc(Prefetcher)
public final class Prefetcher: NSObject {
 
    private var prefetcher: ImagePrefetcher
    
    @objc
    public override init() {
        prefetcher = ImagePrefetcher()
    }
    
    @objc
    public init(destination: Destination = .memoryCache) {
        prefetcher = ImagePrefetcher(destination: destination == .memoryCache ?
                                        ImagePrefetcher.Destination.memoryCache :
                                        ImagePrefetcher.Destination.diskCache)
    }
    
    @objc
    public init(destination: Destination = .memoryCache,
                maxConcurrentRequestCount: Int = 2) {
        prefetcher = ImagePrefetcher(destination: destination == .memoryCache ?
                                        ImagePrefetcher.Destination.memoryCache :
                                        ImagePrefetcher.Destination.diskCache,
                                     maxConcurrentRequestCount: maxConcurrentRequestCount)
    }
    
    @objc
    public func startPrefetching(with: [URL]) {
        prefetcher.startPrefetching(with: with)
    }
    
    @objc
    public func stopPrefetching(with: [URL]) {
        prefetcher.stopPrefetching(with: with)
    }
    
    @objc
    public func stopPrefetching() {
        prefetcher.stopPrefetching()
    }
    
    @objc
    public func pause() {
        prefetcher.isPaused = true
    }
    
    @objc
    public func unPause() {
        prefetcher.isPaused = false
    }
    
    @objc
    public enum Destination: Int {
        case memoryCache
        case diskCache
    }
}

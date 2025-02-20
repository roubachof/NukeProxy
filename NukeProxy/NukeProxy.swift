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
    
    private var tasks: [ImageTask] = []
    
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
        let imageRequest = ImageRequest(url: url)
        Nuke.ImagePipeline.shared.cache.removeCachedData(for: imageRequest)
        Nuke.ImagePipeline.shared.cache.removeCachedImage(for: imageRequest)
    }
    
    @objc
    public func removeAllCaches() {
        Nuke.ImagePipeline.shared.cache.removeAll()
    }
    
    @objc
    public func loadImage(url: URL, onCompleted: @escaping (UIImage?, String) -> Void) {
        let task = Nuke.ImagePipeline.shared.loadImage(
            with: url,
            progress: nil,
            completion: { [weak self] result in
                switch result {
                case let .success(response):
                    onCompleted(response.image, "success")
                case let .failure(error):
                    onCompleted(nil, error.localizedDescription)
                }
                self?.tasks.removeAll(where: { $0.request.imageId == url.absoluteString })
            }
        )
        
        tasks.append(task)
    }
    
    @objc
    public func loadImage(url: URL, placeholder: UIImage?, errorImage: UIImage?, into: UIImageView) {
        loadImage(url: url, placeholder: placeholder, errorImage: errorImage, into: into, reloadIgnoringCachedData: false)
    }
    
    @objc
    public func loadImage(url: URL, placeholder: UIImage?, errorImage: UIImage?, into: UIImageView, reloadIgnoringCachedData: Bool) {
        let options = ImageLoadingOptions(placeholder: placeholder, failureImage: errorImage)
        let cachePolicy = reloadIgnoringCachedData ? URLRequest.CachePolicy.reloadIgnoringLocalAndRemoteCacheData : URLRequest.CachePolicy.useProtocolCachePolicy
        let urlRequest = URLRequest(url: url, cachePolicy: cachePolicy)
        let request = ImageRequest(urlRequest: urlRequest)
        
        let task = Nuke.loadImage(
            with: request,
            options: options,
            into: into
        )
        
        tasks.append(task!)
    }
    
    @objc
    public func loadImage(url: URL, imageIdKey: String, placeholder: UIImage?, errorImage: UIImage?, into: UIImageView) {
        loadImage(url: url, imageIdKey: imageIdKey, placeholder: placeholder, errorImage: errorImage, into: into, reloadIgnoringCachedData: false)
    }
    
    @objc
    public func loadImage(url: URL, imageIdKey: String, placeholder: UIImage?, errorImage: UIImage?, into: UIImageView, reloadIgnoringCachedData: Bool) {
        let options = ImageLoadingOptions(placeholder: placeholder, failureImage: errorImage)
        let cachePolicy = reloadIgnoringCachedData ? URLRequest.CachePolicy.reloadIgnoringLocalAndRemoteCacheData : URLRequest.CachePolicy.useProtocolCachePolicy
        let urlRequest = URLRequest(url: url, cachePolicy: cachePolicy)
        let request = ImageRequest(urlRequest: urlRequest, userInfo: [.imageIdKey: imageIdKey])
        
        let task = Nuke.loadImage(
            with: request,
            options: options,
            into: into
        )
        
        tasks.append(task!)
    }
    
    @objc
    public func loadData(url: URL, onCompleted: @escaping (Data?, URLResponse?) -> Void) {
        loadData(url: url, imageIdKey: nil, reloadIgnoringCachedData: false, onCompleted: onCompleted)
    }
    
    @objc
    public func loadData(url: URL, imageIdKey: String?, reloadIgnoringCachedData: Bool, onCompleted: @escaping (Data?, URLResponse?) -> Void) {
        let cachePolicy = reloadIgnoringCachedData ? URLRequest.CachePolicy.reloadIgnoringLocalAndRemoteCacheData : URLRequest.CachePolicy.useProtocolCachePolicy
        let urlRequest = URLRequest(url: url, cachePolicy: cachePolicy)
        let request = ImageRequest(urlRequest: urlRequest, userInfo: imageIdKey == nil ? nil : [.imageIdKey: imageIdKey!])
        
        let task = Nuke.ImagePipeline.shared.loadData(
            with: request,
            completion: { [weak self] result in
                switch result {
                case let .success(response):
                    onCompleted(response.data, response.response)
                case .failure(_):
                    onCompleted(nil, nil)
                }
                self?.tasks.removeAll(where: { $0.request.imageId == imageIdKey })
            }
        )
        
        tasks.append(task)
    }
    
    @objc
    public func cancelTask(_ url: String) {
        tasks.forEach { task in 
            if (task.request.imageId == url) {
                task.cancel()
            }
        }
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
    public init(destination: Destination = .memoryCache, maxConcurrentRequestCount: Int = 2) {
        prefetcher = ImagePrefetcher(
            destination: destination == .memoryCache ? ImagePrefetcher.Destination.memoryCache : ImagePrefetcher.Destination.diskCache,
            maxConcurrentRequestCount: maxConcurrentRequestCount
        )
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

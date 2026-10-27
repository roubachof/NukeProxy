using ObjCRuntime;

namespace ImageCaching.Nuke {
	[Native]
	public enum DataCachePolicy : long {
		Automatic = 0,
		StoreOriginalData = 1,
		StoreEncodedImages = 2,
		StoreAll = 3
	}

	[Native]
	public enum Destination : long {
		MemoryCache = 0,
		DiskCache = 1
	}
}

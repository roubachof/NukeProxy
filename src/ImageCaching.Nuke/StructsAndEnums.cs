using ObjCRuntime;

namespace ImageCaching.Nuke {
	[Native]
	public enum Destination : long {
		MemoryCache = 0,
		DiskCache = 1
	}
}

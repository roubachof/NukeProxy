using System;

namespace ImageCaching.Nuke
{
    public enum DataCachePolicy : long
    {
        Automatic,
        StoreOriginalData,
        StoreEncodedImages,
        StoreAll
    }

    public enum Destination : long
    {
        MemoryCache,
        DiskCache
    }
}


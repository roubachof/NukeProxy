# NukeProxy

This is a small Xamarin.iOS binding Proxy for [Nuke][nuke]. Since Nuke is a Swift library, which doesn't expose its code with `@objc` annotations, we cannot bind it directly and need a proxy for this.
This proxy provides enough API for [Xamarin.Forms.Nuke][xformsnuke] to function. If you need more of Nukes API to be surfaced, PRs are welcome.

## Installation

ImageCaching.Nuke supports .NET 9 and .NET 10 for `ios` and `maccatalyst` targets on ARM64, with a minimum iOS/Mac Catalyst version of 15.0.

### .NET 9 / .NET 10 iOS and Mac Catalyst

Nuget:

> Install-Package ImageCaching.Nuke

### MAUI UI framework

NuGet:

> Install-Package https://www.nuget.org/packages/Sharpnado.Maui.Nuke

## Building locally

Install Xcode 27.1 and select it as your active developer directory. Xcode resolves the Nuke dependency through Swift Package Manager automatically.

Install .NET 10.0.403 SDK and ios and maccatalyst workloads:

```sh
dotnet workload install ios maccatalyst
```

Restore .NET tools:

``sh
dotnet tool restore
```

Run the build script to generate xcframeworks and to update ApiDefinitions


```sh
bash build.sh
```

This creates `Output/NukeProxy.xcframework` with ARM64-only slices for iOS devices, iOS Simulator, and Mac Catalyst, targeting iOS/Mac Catalyst 15.0 or later.

> Intel (`x86_64`) Macs and simulators are no longer supported. The script verifies that each generated framework contains only ARM64.

CI uses the same script on the `xcode-27` runner and explicitly selects Xcode 27.1. Xcode upgrades require changing the pinned `XCODE_VERSION` in `.github/workflows/build.yml`.

## Exposing more API

Open the NukeProxy.xcodeproj in Xcode and add more code to `NukeProxy.swift`.

Make sure to annotate correctly with `@objc`. Refer to the other code and [Xamarin.iOS Swift Bindings][bindings]. When done adding more code, ensure project builds:

1. Run `bash build.sh` from the command line to rebuild the ARM64 XCFramework
2. Adjust the definitions and ensure the project builds
3. Create a PR to this repository

[nuke]:https://github.com/kean/Nuke
[xformsnuke]:https://github.com/roubachof/Xamarin.Forms.Nuke
[bindings]:https://docs.microsoft.com/en-us/xamarin/ios/platform/binding-swift/walkthrough
[sharpie]:https://aka.ms/objective-sharpie

# NukeProxy

This is a small Xamarin.iOS binding Proxy for [Nuke][nuke]. Since Nuke is a Swift library, which doesn't expose its code with `@objc` annotations, we cannot bind it directly and need a proxy for this.
This proxy provides enough API for [Xamarin.Forms.Nuke][xformsnuke] to function. If you need more of Nukes API to be surfaced, PRs are welcome.

## Installation

New! There is now a .net 6 version of the proxy targeting `ios` and `mac catalyst`.

### .NET 9 iOS and macOS

Nuget:

> Install-Package ImageCaching.Nuke

### MAUI UI framework

NuGet:

> Install-Package https://www.nuget.org/packages/Sharpnado.Maui.Nuke

## Building locally

Make sure to install carthage first. This can be done through Homebrew:

```
brew install carthage
```

Getting Nuke from carthage:

```
sh carthage.sh update --use-xcframeworks
```

## Exposing more API

Open the NukeProxy.xcodeproj in Xcode and add more code to `NukeProxy.swift`.

Make sure to annotate correctly with `@objc`. Refer to the other code and [Xamarin.iOS Swift Bindings][bindings]. When done adding more code, ensure project builds:

1. Run `sh build.sh` from commandline (make sure you have [the latest Sharpie][sharpie] installed)
2. Copy the new definitions from the `sharpie_output` folder into the `Xamarin.Nuke` C# project
3. Adjust the definitions and ensure the project builds
4. Create a PR to this repository

[nuke]:https://github.com/kean/Nuke
[xformsnuke]:https://github.com/roubachof/Xamarin.Forms.Nuke
[bindings]:https://docs.microsoft.com/en-us/xamarin/ios/platform/binding-swift/walkthrough
[sharpie]:https://aka.ms/objective-sharpie

#!/bin/bash

set -euo pipefail

cd "$(dirname "$0")"

mkdir -p artifacts
rm -rf Output/NukeProxy.xcframework

echo "xcode build (ARM64 only)"
# Set ARCHS explicitly so the framework and its Swift package dependencies never build Intel slices.
xcodebuild archive -sdk iphoneos -destination 'generic/platform=iOS' -project NukeProxy.xcodeproj -scheme NukeProxy -configuration Release -archivePath Output/Output-iphoneos ARCHS=arm64 ONLY_ACTIVE_ARCH=NO SKIP_INSTALL=NO CODE_SIGNING_ALLOWED=NO
xcodebuild archive -sdk iphonesimulator -destination 'generic/platform=iOS Simulator' -project NukeProxy.xcodeproj -scheme NukeProxy -configuration Release -archivePath Output/Output-iphonesimulator ARCHS=arm64 ONLY_ACTIVE_ARCH=NO SKIP_INSTALL=NO CODE_SIGNING_ALLOWED=NO
xcodebuild archive -destination 'generic/platform=macOS,variant=Mac Catalyst' -project NukeProxy.xcodeproj -scheme NukeProxy -configuration Release -archivePath Output/Output-maccatalyst ARCHS=arm64 ONLY_ACTIVE_ARCH=NO SKIP_INSTALL=NO CODE_SIGNING_ALLOWED=NO

echo "create xcframework"
xcodebuild -create-xcframework -framework Output/Output-iphonesimulator.xcarchive/Products/Library/Frameworks/NukeProxy.framework -framework Output/Output-iphoneos.xcarchive/Products/Library/Frameworks/NukeProxy.framework -framework Output/Output-maccatalyst.xcarchive/Products/Library/Frameworks/NukeProxy.framework -output Output/NukeProxy.xcframework

for binary in Output/NukeProxy.xcframework/*/NukeProxy.framework/NukeProxy; do
    architectures="$(xcrun lipo -archs "$binary")"
    if [[ "$architectures" != "arm64" ]]; then
        echo "Unexpected architectures in $binary: $architectures (expected arm64)" >&2
        exit 1
    fi
done

echo "create bindings"
dotnet sharpie bind -o src/ImageCaching.Nuke/ -s iphoneos27.0 -n ImageCaching.Nuke -f Output/NukeProxy.xcframework/ios-arm64/NukeProxy.framework --verbose

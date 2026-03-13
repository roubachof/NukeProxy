#!/bin/bash

mkdir -p artifacts
rm -r Output/NukeProxy.xcframework

echo "xcode build"
xcodebuild archive -sdk iphoneos -project NukeProxy.xcodeproj -scheme NukeProxy -configuration Release -archivePath Output/Output-iphoneos SKIP_INSTALL=NO
xcodebuild archive -sdk iphonesimulator -project NukeProxy.xcodeproj -scheme NukeProxy -configuration Release -archivePath Output/Output-iphonesimulator SKIP_INSTALL=NO

echo "create xcframework"
xcodebuild -create-xcframework -framework Output/Output-iphonesimulator.xcarchive/Products/Library/Frameworks/NukeProxy.framework -framework Output/Output-iphoneos.xcarchive/Products/Library/Frameworks/NukeProxy.framework -output Output/NukeProxy.xcframework

echo "create bindings"
dotnet sharpie bind -o src/ImageCaching.Nuke/ -s iphoneos26.2 -n ImageCaching.Nuke -f Output/NukeProxy.xcframework/ios-arm64/NukeProxy.framework --verbose

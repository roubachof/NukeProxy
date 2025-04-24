#!/bin/bash

mkdir -p artifacts

echo "Update carthage deps"
sh carthage.sh update --use-xcframeworks --platform iOS --log-path artifacts/carthage.log

rm -r Output/NukeProxy.xcframework

echo "xcode build"
xcodebuild archive -sdk iphoneos -project NukeProxy.xcodeproj -scheme NukeProxy -configuration Release -archivePath Output/Output-iphoneos SKIP_INSTALL=NO
xcodebuild archive -sdk iphonesimulator -project NukeProxy.xcodeproj -scheme NukeProxy -configuration Release -archivePath Output/Output-iphonesimulator SKIP_INSTALL=NO

echo "create xcframework"
xcodebuild -create-xcframework -framework Output/Output-iphonesimulator.xcarchive/Products/Library/Frameworks/NukeProxy.framework -framework Output/Output-iphoneos.xcarchive/Products/Library/Frameworks/NukeProxy.framework -output Output/NukeProxy.xcframework

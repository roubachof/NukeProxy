#!/bin/bash

xcodebuild archive -sdk iphoneos -project NukeProxy.xcodeproj -scheme NukeProxy -configuration Release -archivePath Output/Output-iphoneos SKIP_INSTALL=NO
xcodebuild archive -sdk iphonesimulator -project NukeProxy.xcodeproj -scheme NukeProxy -configuration Release -archivePath Output/Output-iphonesimulator SKIP_INSTALL=NO

xcodebuild -create-xcframework -framework Output/Output-iphonesimulator.xcarchive/Products/Library/Frameworks/NukeProxy.framework -framework Output/Output-iphoneos.xcarchive/Products/Library/Frameworks/NukeProxy.framework -output Output/NukeProxy.xcframework

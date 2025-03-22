# Beaver Receiver

![icon](/macOS/images/256.png)

## Menu bar button for macOS

The Beaver Receiver application adds a button to the macOS menu bar, which  starts or stops an audio stream.

The audio source is configurable from the application window, with [Groove Salad](https://somafm.com/groovesalad/) used by default.

A dropdown list is prepopulated with additional streams from [somafm.com](https://somafm.com/), and an input field is available for the user to enter a custom URL. 

![application](/macOS/images/application_window.png)

Click the button on the menu bar to turn the stream on or off:

![flow](/macOS/images/flow.png)   indicates the stream is started

![dam](/macOS/images/dam.png)   indicates the stream is stopped

_____

## Installation

* Minimum version: macOS 12.4 (Monterey)
* Build and testing: macOS 15.3.1 (Sequoia)

### Notes 

* [Install from the App Store](https://apps.apple.com/us/app/beaver-receiver/id6743641943), or download and unzip [Beaver_Receiver.zip](/macOS/Application/Beaver_Receiver.zip) and drag the application to the Applications directory (Command+Shift+A).
* The application remains visible in the dock; however, when set to open at login, the menu bar button is available after reboot without spawning an unnecessary application window, which is good enough for me.
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

* Minimum version: macOS 10.15 (Catalina)
* Build and testing: macOS 15.0.1 (Sequoia)

### Notes 
* The application is currently unsigned, so depending on macOS version, right-clicking on the icon may be required the first time the application is launched.

* The application allows user input, and barely checks the syntax. It does as little as possible to just start or stop a selected stream. I'm not responsible for anything else that may happen on your system. View the source code [here](Source).
 
* The application remains visible in the dock; however, when set to open at login, the menu bar button is available after reboot without spawning an unnecessary application window, which is good enough for me.

Download and unzip [Beaver Receiver.zip](/macOS/Application/Beaver%20Receiver_macOS.zip) and drag the application to the Applications directory (Command+Shift+A).


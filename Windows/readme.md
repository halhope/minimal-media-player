# Beaver Receiver

![icon](/Windows/images/256.png)

## Movable button for Windows

The Beaver Receiver application adds a configurable on/off button, which  starts or stops an audio stream.

The audio source is configurable from the button's context menu, with [Groove Salad](https://somafm.com/groovesalad/) used by default.

A dropdown list is prepopulated with additional streams from [somafm.com](https://somafm.com/), and an input field is available for the user to enter a custom URL. 

Click the button to turn the stream on or off:

![streaming.png](images/streaming.png)   indicates the stream is started

![stopped.png](images/stopped.png)   indicates the stream is stopped

* Click and hold to move the button to a new position on the screen (hold for at least one second to avoid toggling playback).

* Roll the mouse wheel over the button to adjust the volume.

* Right-click to select from a list of sources or define a new stream.

![context_menu.png](images/context_menu.png)

* Hover to see the current selection.

![hover.png](images/hover.png)

* The button position and last-played selection are preserved the next time the application is launched.

### Notes 

* The application is currently unsigned, so clicking "More Info" and choosing "Run anyway" is required the first time the application is launched. View the source code [here](Source).
* The application allows user input, and barely checks the syntax. It does as little as possible to simply start or stop a selected stream. I'm not responsible for anything else that may happen on your system. View the source code [here](Source).
* Since nothing is installed by the application, removal is as simple as deleting the Beaver Receiver folder (no uninstall required).
* That also means it doesn't run in the system tray or prompt to add a shortcut to launch after installing, so you'll need to handle that part of the configuration.
 
Download and unzip [Beaver Receiver.zip](/Windows/Application/Beaver%20Receiver.zip) and put the folder somewhere handy. Find the application inside to launch Beaver Receiver. Once running, it can be pinned to the Taskbar or added to Startup programs, etc.

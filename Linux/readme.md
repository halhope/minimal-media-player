# Groove Salad button for Linux

By default the `groove.sh` shell script simply checks to see if `ffplay` is already running, and kills it if so. 

If `ffplay` is not already running, it's started, playing the [Groove Salad](https://somafm.com/groovesalad/) stream from [somafm.com](https://somafm.com/).

(I don't expect to normally have `ffplay` running for any other reason, so this approach works for me.)

____ 

## Adding a button

I added a [KDE Plasmoid](https://userbase.kde.org/Plasma) to the menu bar by right-clicking it to add/manage widgets, and selecting the [Application Launcher](https://userbase.kde.org/Plasma_application_launchers).

![launcher](/Linux/images/launch_button.png)

Right-click to edit Properties:

![button_context](/Linux/images/button_context.png)

An icon can be set at the first tab:

![general](/Linux/images/general.png)

Check permissions:

![general](/Linux/images/button_permissions.png)

Give the application a title and define the path(s) accordingly.

In my case, it's a hidden file (preceded by a dot) in my home directory.

Note that on most Linux systems, the application path will just be `/bin/bash`.

Provide the file path for the downloaded `groove.sh` at the Application tab:

![application_details](/Linux/images/application_details.png)

___

With the button placed rightmost on my menu bar, I'm able to quickly throw the cursor into the corner and click to quickly stop/resume playback.

![menu_button](/Linux/images/menu_button.png)
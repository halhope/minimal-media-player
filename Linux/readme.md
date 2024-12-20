# Groove Salad button for Linux

With a button placed rightmost on my menu bar, I'm able to throw the cursor into the corner and click to immediately stop/resume playback of an audio stream.

![menu_button](/Linux/images/menu_button.png)

The [groove.sh](groove.sh) shell script simply checks to see if `ffplay` is already running, and kills it if so. 

If `ffplay` is not already running, it's started, playing the [Groove Salad](https://somafm.com/groovesalad/) stream from [somafm.com](https://somafm.com/).

(I don't expect to normally have `ffplay` running for any other reason, so this approach works for me.)

* Ensure the script is executable: `chmod +x /path/to/groove.sh`
* Edit the script to change the path if you don't want to groove. 🤷
  
____ 

## Making a button

I added a [KDE Plasmoid](https://userbase.kde.org/Plasma) to the menu bar by right-clicking it to add/manage widgets, and selecting Quicklaunch.

![launcher](/Linux/images/launch_button.png)

Right-click to edit Properties:

![button_context](/Linux/images/button_context.png)

An icon can be set at the first tab:

![general](/Linux/images/general.png)

Check permissions:

![button_permissions](/Linux/images/button_permissions.png)

Select Advanced Options at the Application tab and uncheck "Run in terminal" to prevent one from opening.

![advanced_permissions](/Linux/images/advanced_permissions.png)

Give the application a title and define the path(s) accordingly.

Provide the file path for the downloaded `groove.sh` at the Application tab:

![application_details](/Linux/images/application_details.png)

**Note** that on most Linux systems, the application path will just be `/bin/bash`.

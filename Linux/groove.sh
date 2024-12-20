#!/bin/bash

# Command to check and run
COMMAND="ffplay"

# Check if ffplay is already running
if pgrep -x "$COMMAND" > /dev/null; then
    # Kill the ffplay process if it's running
    pkill -x "$COMMAND"
else
    # If not running, start ffplay in background - for KDE, add widget Quicklaunch, and uncheck _Run in Terminal_ in Application > Properties > Advanced Options
    curl -s https://ice4.somafm.com/groovesalad-128-aac 2>&1 | ffplay -nodisp -
fi

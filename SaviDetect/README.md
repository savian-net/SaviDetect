# SaviDetect

### Description

Detects any file changes in a specified directory and executes a program when a change is detected.

### Overview

SaviDetect installs as a Windows service. When installed, go into the services section of administrative
tools and set any startup options. All configuration changes are handled in the config.xml file that is in
the programs root directory (typically c:\program files\Savian\SaviDetect).

### Arguments

Arguments are specified in the SaviDetectSettings.json file. 

#### Example
        "ProgramToExecute": "x:\\repos\\publish\\TestSaviDetect.exe",
        "DirectoryToMonitor": "z:\\scratch\\SaviDetectTest",
        "UserArguments":
        [
        ],
        "UsePolling": "true",
        "DelayAfterFileDetection": 1000,
        "AllowMultipleNotifications": "true",
        "ReturnActionTags": "false",
        "Changed": "true",
        "Created": "true",
        "Deleted": "true",
        "Renamed": "true"

#### Argument Descriptions

    PollingDelay can be any number. It is merely a placeholder that 
    executes a delay that is not used.
 
    A large arbitrary number works fine.

| Parm  | Description  |
| --------  | ------------------- |
| ProgramToExecute | The full path to the program to execute when an event is triggered      | 
| DirectoryToMonitor      | The directory to monitor for changes | 
| UserArguments      | The strings to append to the ProgramToExecute to pass in as arguments | 
| DelayAfterFileDetection      | How long to wait, after the event is triggered, before executing the program | 
| AllowMultipleNotifications      | A single file change can result in multiple firings of an application. This flag will indicate whether every action triggers an event or whether 1 event will be fired regardless of the number of flags marked true | 
| Changed      | Trigger an event when a file is CHANGED | 
| Created      | Trigger an event when a file is CREATED | 
| Deleted      | Trigger an event when a file is DELETED | 
| Renamed      | Trigger an event when a file is RENAMED | 

### Install as a service

A cmd file is included called InstallSaviDetect.cmd. This will run the commands necessary to install SaviDetect as a Windows service. 

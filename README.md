[![GitHub release](https://img.shields.io/github/release/SamuelSVD/TrayDir-Lite.svg)](../../releases/latest)
[![GitHub release](https://img.shields.io/github/downloads/SamuelSVD/TrayDir-Lite/total)](../../releases/latest)

# TrayDir-Lite
A windows application that gives the user the ability to access files, folders, and programs from the system tray.

## Download TrayDir-Lite

Download installer from GitHub in the latest release

[http://github.com/samuelSVD/TrayDir-Lite/releases/latest](http://github.com/samuelSVD/TrayDir-Lite/releases/latest)

## Usage

It is very easy to get started with TrayDir. Make use of Files, Folders, and Virtual Folders easily by dragging in files from the Windows File Explorer.

| Description | GIF|
| --- | --- |
| File use in TrayDir | <img src="https://github.com/SamuelSVD/TrayDir/blob/main/Misc/Media/TrayDir_DragDrop_File.gif?raw=true" width="500"/> |
| Folder use in TrayDir | <img src="https://github.com/SamuelSVD/TrayDir/blob/main/Misc/Media/TrayDir_DragDrop_Folder.gif?raw=true" width="500"/> |
| Virtual Folder use in TrayDir | <img src="https://github.com/SamuelSVD/TrayDir/blob/main/Misc/Media/TrayDir_VirtualFolder.gif?raw=true" width="500"/> |

## Supported Platforms

Currently TrayDir is only tested on Windows 10, but also works on Windows 11.

# Getting Started
When you first open up TrayDir you are greeted with the default new instance. At the top you will find five buttons corresponding with the following functions:

- New File: Inserts a new file menu item
- New Folder: Inserts a new folder menu item
- New Virtual Folder: Inserts a new virtual folder menu item
- New Separator: Inserts a separator item that displays as a horizontal bar

![TrayDir_example_instancebuttons](https://user-images.githubusercontent.com/36249705/164873655-de6e4ac9-56f8-497d-95fe-e5197d433e7e.png)

### What is the System Tray?

The system tray is the list of items at the bottom right of your windows task bar. In most cases, this is where the time, date, Wi-Fi connection, and other settings are displayed.

### Modifying TrayDir
You can start modifying TrayDir by clicking on the New File button at the top to add a new file link. This will result in a new file item being added to the list of items, and begins browsing for a file in your file system. When you select a file and apply the changes, this file will appear in the right-click menu of TrayDir in the System Tray

If you right click on the TrayDir icon in the system tray, you will be greeted by the Tray Menu, containing the file item that you added to the list. When clicked, this file will be opened as if you had selected it directly through a File Explorer window.

## Instances

### What is an Instance?

An instance refers to one specific configuration of links. Each instance has its own icon in the system tray, with a unique corresponding menu.

### Adding New Instances

In case you want a new icon in the system tray for a separate set of items, either click on the ```+``` beside the New Instance name or click on the menu ```Instance > New```. From here you should be able to select the new instance in a new named tab near the top of the form, and see the new icon in the system tray.

### Renaming an Instance and Chaning its Icon

If you hover over the TrayDir icon in the system tray, you will notice that it is named ```New Instance```. This name can be modified by clicking on the top menu, ```Instance > Edit Name```.


### Changing the Icon

The icon used in the System Tray can also be modified by clicking the ```Options``` button at the bottom right, then clicking ```Browse``` under the image. You can select any file and TrayDir will use that file's icon for itself.

Want a different icon for your instance? One option is to create an image file (I recommend .png), then run it through [convertico.com](https://convertico.com/) to convert it to a valid icon file (.ico). From there, download the file and browse to the file location through the instance's icon browse button. A good resource for images you could use is [flaticon.com](https://www.flaticon.com/) 

# Example - Link to PC Games!

Below we have an example of an instance named My Games.

| Description | Image |
| --- | --- |
| It contains a link to a folder, and a virtual folder with two file links inside. | ![TrayDir_example_gamesinstance](https://user-images.githubusercontent.com/36249705/164873135-3a2d9d99-8ef6-4828-936c-22ea06e9855d.png)<br/>My Games - Folder Structure | 
| This ```My Games``` instance is configured with a yellow G as the icon to use in the system tray. | ![TrayDir_example_gamesinstanceoptions](https://user-images.githubusercontent.com/36249705/164873145-ee977a89-2e38-4e38-8049-e65f9eecee37.png)<br/>My Games - Instance Options |
| When we hover the cursor over the icon in the system menu, you can see the instance name, ```My Games```. Notice there is another icon in the system tray for the Work instance. | ![TrayDir_example_hover](https://user-images.githubusercontent.com/36249705/164873155-47a638e8-20b0-408f-9a95-eef9542ced97.png)<br/>My Games - Name & Icon |
| When you right-click the tray menu, the same item structure appears as it appeared in the main window. Clicking on the items here will open the files. | ![TrayDir_example_menu](https://user-images.githubusercontent.com/36249705/164873163-c97ebe2b-537d-46a6-a75d-981bf6c198a7.png)<br/>My Games - System Tray Menu |
| The folders can also be expanded, so if you add a folder item TrayDir will automatically expand them for you to have quick access.<br/>All subfolders will be accessible, since they keep on expanding further as required. | ![TrayDir_example_menufolder](https://user-images.githubusercontent.com/36249705/164873168-dc08a9b1-9aba-4f67-beec-cfbbc540b7ef.png)<br/>My Games - System Tray Menu (Folder) |

# Advanced Functionality
## RegEx Filtering

Do you want to point TrayDir to a folder containing a lot of files you don't care about? You can add a RegEx pattern to ignore certain files or folders from being included in the tray menu.

Every new line is treated as a new pattern.

| Pattern | Description |
| --- | --- |
| `$(?<=\.(gif\|png\|jpg))` | This pattern will ignore all files ending with `.gif`, `.png`, or `.jpg` |

For more information on RegEx and creating your own filters see [Regular Expressions](https://docs.microsoft.com/en-us/dotnet/standard/base-types/regular-expression-language-quick-reference)


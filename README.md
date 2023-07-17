Graphics Console Demos for Meadow
=================================

This Visual Studio solution contains three examples of how to use the Belasoft Graphics Console on a [**meadow**](https://www.wildernesslabs.co/developers) board from [*Wilderness Labs*](https://www.wildernesslabs.co/). The **meadow** board must have a graphic display attached for this to work.

![example image](Media/image1.jpg "An example image")

The examples uses the St7789 display, but although the console has not been tested on other graphic displays, it should work as long as the displays are based on the MicroGraphics class.

![example video](Media/Meadow-Console.gif "Example of a console on St7789 display")

You can have the whole display being a console, part of the display being a console, or you can even have more than one console on the same display. 

Examples of Console in action
-----------------------------

[Video showing one Console on St7789 display](https://www.youtube.com/shorts/e0oUps9HY1Y)

[Video showing two Consoles on St7789 display](https://www.youtube.com/shorts/ixaep6Anos4)

[Video showing three Consoles on St7789 display](https://www.youtube.com/watch?v=B-GKpX_ZmJ8)

Getting Started
---------------

1. Download/Clon this project from Github

2. Open the solution file MeadowGraphicsConsoleDemos.sln in Visual Studio 2022

3. Attach a meadow board to your computer, for example The ProjectLab or use a setup [like this](https://www.hackster.io/wilderness-labs/working-with-graphics-on-a-st7789-display-using-meadow-e2295a)

4. Compile the solution in Visual Studio to ensure everything is ok, ie all packages has been downloaded etc.

5. Select your Meadow Device in Visual Studio (View --> Toolbars --> Meadow Device List)

6. Right-click the project you want to run and select Deploy. Choose between the projects:

  * 'OneConsoleDemo'
  * 'TwoConsolesDemo'
  * 'ThreeConsolesDemo'
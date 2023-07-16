Graphics Console Demos for Meadow
=================================

This Visual Studio solution contains three examples of how to use the Belasoft Graphics Console on a [**meadow**](https://www.wildernesslabs.co/developers) board from [*Wilderness Labs*](https://www.wildernesslabs.co/). The **meadow** board must have a graphic display attached for this to work.

The examples uses the St7789 display, but although the console has not been tested on other graphic displays, it should work as long as the displays are based on the MicroGraphics class.

... hertil
 
![example image](Samples/image1.jpg "An example image")

You can have the whole display being a console, part of the display being a console, or you can even have more than one console on the same display. 

Sometimes it's nice to have a console that can show messages, events, commands etc. so that you have an idea of what is going on in your program. A console is like having a small window to a log while it's being written. That can be quite usefull in certain situations and for me, I would have a hard time coding on the meadow without having a console. That's why I made it - but of course it depends on the individual needs and what you are developing. 

Anyway, I hope you will find it usefull - Happy coding :-)

Examples of Console in action
-----------------------------

(example video)

Installation steps
------------------

1. Install this NuGet package in your project file in Visual Studio: 

        Description of package and how to install

2. In the top of your MeadowApp.cs file add a using for the  Belasoft.MeadowLibrary:

        using Belasoft.MeadowLibrary;

3. Setup a display with MicroGraphics, for example the St7789:

        var display = new St7789
        (
            spiBus: MeadowApp.Device.CreateSpiBus(),
            chipSelectPin: Device.Pins.A03,
            dcPin: Device.Pins.A04,
            resetPin: Device.Pins.A05,
            width: 240, height: 240,
            colorMode: ColorMode.Format16bppRgb565
        );

        var graphics = new MicroGraphics(display)
        {
            Stroke = 1,
            CurrentFont = new Font8x12(),
            Rotation = RotationType._270Degrees
        };        

4. Setup a Console:

        GraphicsConsole gc = new GraphicsConsole(graphics, false);
        gc.YTop = 0;
        gc.YBottom = graphics.Height / 2;
        gc.Indent = 0;
        gc.IndentRight = 0;
        gc.BorderColor = Color.Violet;
        gc.SetBorder();        
        Thread t = new Thread(gc.Start);
        t.Start();

5. Begin writing texts to the console:
        
        gc.WriteLine("Initializing", Color.Red, ScaleFactor.X2, true);        
        gc.WriteLine("System starting...", Color.OrangeRed, ScaleFactor.X2, true);        
        gc.WriteLine("Warming up...", Color.Orange, ScaleFactor.X2, true);        
        gc.WriteLine("Comm ready", Color.Yellow, ScaleFactor.X2, true);
        gc.WriteLine("GO", Color.Green, ScaleFactor.X3, true);        


...

2nd paragraph. *Italic*, **bold**, and `monospace`. Itemized lists
look like

  * this one
  * that one
  * the other one

Note that --- not considering the asterisk --- the actual text
content starts at 4-columns in.

> Block quotes are
> written like so.
>
> They can span multiple paragraphs,
> if you like.

Use 3 dashes for an em-dash. Use 2 dashes for ranges (ex., "it's all
in chapters 12--14"). Three dots ... will be converted to an ellipsis.
Unicode is supported. ☺



An h2 header
------------

Here's a numbered list:

 1. first item
 2. second item
 3. third item

Note again how the actual text starts at 4 columns in (4 characters
from the left side). Here's a code sample:
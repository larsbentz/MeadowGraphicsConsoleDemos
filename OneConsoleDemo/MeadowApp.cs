using Belasoft.MeadowLibrary;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Hardware;
using Meadow.Peripherals.Leds;
using MeadowCommonLib.Data;
using MeadowCommonLib.Devices;
using System;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace OneConsoleDemo
{
    // Change F7FeatherV2 to F7FeatherV1 for V1.x boards
    public class MeadowApp : App<F7FeatherV2>
    {
        RgbPwmLed onboardLed;
        IPin chipSelectPin;
        IPin dcPin;
        IPin resetPin;
        RotationType rotationType;

        MicroGraphics graphics;
        GraphicsConsole gc;


        public override Task Initialize()
        {
            Resolver.Log.Info("Initialize...");

            onboardLed = new RgbPwmLed(
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue,
                CommonType.CommonAnode);

            onboardLed.SetColor(Color.Red);

            // -- Settings for Project Lab Board V1 --
            chipSelectPin = Device.Pins.A03;
            dcPin = Device.Pins.A04;
            resetPin = Device.Pins.A05;
            rotationType = RotationType._270Degrees;

            // -- Settings for stanalone Board using digital pins --
            // -- NB: Change pins according to the ones your are using on your breadboard --
            //chipSelectPin = Device.Pins.D02;
            //dcPin = Device.Pins.D01;
            //resetPin = Device.Pins.D00;
            //rotationType = RotationType._270Degrees;

            var st7789Graphices = new St7789Graphics(MeadowApp.Device.CreateSpiBus(), chipSelectPin, dcPin, resetPin, new Font8x12(), ColorMode.Format16bppRgb565, rotationType);
            graphics = st7789Graphices.Graphics;

            return base.Initialize();
        }

        public override async Task Run()
        {
            Resolver.Log.Info("Run...");
            
            await onboardLed.StartPulse(Color.Green, TimeSpan.FromMilliseconds(3000));

            await DemoMonitor();
            //return CycleColors(TimeSpan.FromMilliseconds(1000));
        }

        async Task DemoMonitor()
        {
            InitializeConsole();

            InitializeClock();

            InitializeTextWriter();

            // Start writing to Console            
            gc.WriteLine("Initializing", Color.Red, ScaleFactor.X2, true);
            await Task.Delay(3000);

            gc.WriteLine("System starting...", Color.OrangeRed, ScaleFactor.X2, true);
            await Task.Delay(3000);

            gc.WriteLine("Warming up...", Color.Orange, ScaleFactor.X2, true);
            await Task.Delay(3000);

            gc.WriteLine("Comm ready", Color.Yellow, ScaleFactor.X2, true);
            await Task.Delay(3000);

            gc.WriteLine("GO", Color.Green, ScaleFactor.X3, true);
            await Task.Delay(2000);

            gc.WriteLine("Meadow rules :-)", Color.CornflowerBlue, ScaleFactor.X2, true);
            await Task.Delay(4000);

            var rnd = new Random(DateTime.Now.Millisecond);
            var oldnum = 0;
            while (true)
            {
                var txtnum = rnd.Next(0, DemoData.Demos.Length - 1);
                if (txtnum == oldnum) continue;
                oldnum = txtnum;

                var txt = DemoData.Demos[txtnum].Text;
                var color = DemoData.Demos[txtnum].Color; // colors[colnum];
                var scale = DemoData.Demos[txtnum].Scale;
                if (scale == ScaleFactor.X3)
                    scale = ScaleFactor.X2;                

                //var txtnum = rnd.Next(0, DemoData.Demos.Length - 1);
                //var txt = ((DemoData.Demos[txtnum].Scale == ScaleFactor.X1) ? DateTime.Now.ToString("mm:ss ") : "")
                //            + DemoData.Demos[txtnum].Text;
                if (txt.Contains("CPU temp"))
                {
                    var cpuTemp = Device.PlatformOS.GetCpuTemperature();
                    txt += $" {cpuTemp.Celsius:0.##}";
                }
                else if (txt == "OS")
                {
                    var osVersion = Device.PlatformOS.OSVersion;
                    txt += $" {osVersion}";
                }
                //var color = DemoData.Demos[txtnum].Color; // colors[colnum];
                //var scaleFactor = DemoData.Demos[txtnum].Scale;
                gc.WriteLine(txt, color, scale, true);

                //gc.WriteLine(DateTime.Now.ToString("mm:ss ") + response.StatusCode, true);

                //Thread.Sleep(3000);
                var rndsleep = rnd.Next(0, 10);
                if (rndsleep < 3)
                    await Task.Delay(1000);
                else if (rndsleep < 6) 
                    await Task.Delay(2000);
                else
                    await Task.Delay(3000);
            }
        }


        void InitializeConsole()
        {
            graphics.Clear(true);

            gc = new GraphicsConsole(graphics, false);
            gc.YTop = graphics.Height / 4;            
            gc.Indent = 0;
            gc.IndentRight = 0;
            gc.BorderColor = Color.Violet;
            gc.SetBorder();
            Thread t = new Thread(gc.Start);
            t.Start();
        }

        GraphicsClock InitializeClock()
        {
            GraphicsClock gClock = new GraphicsClock(graphics, 10, 30);
            gClock.ScaleFactor = ScaleFactor.X2;
            Thread tClock = new Thread(gClock.Start);
            tClock.Start();
            return gClock;
        }

        TextWriter InitializeTextWriter()
        {
            graphics.DrawText(110, 0, "Console", Color.Red, ScaleFactor.X2);
            GraphicsWriter gWriter = new GraphicsWriter(graphics);
            TextWriter txtWriter = new TextWriter(gWriter, 5, 0, "Meadow");
            txtWriter.Start();
            //Thread tWriter = new Thread(txtWriter.Start);
            //tWriter.Start();
            return txtWriter;
        }






        async Task CycleColors(TimeSpan duration)
        {
            Resolver.Log.Info("Cycle colors...");

            while (true)
            {
                await ShowColorPulse(Color.Blue, duration);
                await ShowColorPulse(Color.Cyan, duration);
                await ShowColorPulse(Color.Green, duration);
                await ShowColorPulse(Color.GreenYellow, duration);
                await ShowColorPulse(Color.Yellow, duration);
                await ShowColorPulse(Color.Orange, duration);
                await ShowColorPulse(Color.OrangeRed, duration);
                await ShowColorPulse(Color.Red, duration);
                await ShowColorPulse(Color.MediumVioletRed, duration);
                await ShowColorPulse(Color.Purple, duration);
                await ShowColorPulse(Color.Magenta, duration);
                await ShowColorPulse(Color.Pink, duration);
            }
        }

        async Task ShowColorPulse(Color color, TimeSpan duration)
        {
            await onboardLed.StartPulse(color, duration / 2);
            await Task.Delay(duration);
        }
    }
}
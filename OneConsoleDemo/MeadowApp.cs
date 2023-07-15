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

            //await ShowColorPulse(Color.Green, TimeSpan.FromMilliseconds(3000));
            await onboardLed.StartPulse(Color.Green, TimeSpan.FromMilliseconds(3000));

            await DemoMonitor();
            //return CycleColors(TimeSpan.FromMilliseconds(1000));
        }

        async Task DemoMonitor()
        {
            InitializeConsole();

            InitializeClock();

            InitializeTextWriter();

            var rnd = new Random(DateTime.Now.Millisecond);

            while (true)
            {
                var txtnum = rnd.Next(0, DemoData.Demos.Length - 1);
                var txt = ((DemoData.Demos[txtnum].Scale == ScaleFactor.X1) ? DateTime.Now.ToString("mm:ss ") : "")
                            + DemoData.Demos[txtnum].Text;
                if (txt.StartsWith("CPU t"))
                {
                    var cpuTemp = Device.PlatformOS.GetCpuTemperature();
                    txt += $" {cpuTemp.Celsius:0.##}";
                }
                else if (txt.StartsWith("OS v"))
                {
                    var osVersion = Device.PlatformOS.OSVersion;
                    txt += $" {osVersion}";
                }
                var color = DemoData.Demos[txtnum].Color; // colors[colnum];
                var scaleFactor = DemoData.Demos[txtnum].Scale;
                gc.WriteLine(txt, color, scaleFactor, true);

                //gc.WriteLine(DateTime.Now.ToString("mm:ss ") + response.StatusCode, true);

                //Thread.Sleep(3000);
                await Task.Delay(3000);
            }
        }


        void InitializeConsole()
        {
            graphics.Clear(true);

            //graphics.WriteLine("Hello!", 1);
            //graphics.DrawText(indent, y, "Meadow F7 SPI ST7789!!");
            graphics.DrawText(10, 5, "Console", Color.Red, ScaleFactor.X2);

            gc = new GraphicsConsole(graphics, false);
            gc.YTop = graphics.Height / 4;
            //gc.YBottom = graphics.Height - gc.YTop;
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
            GraphicsWriter gWriter = new GraphicsWriter(graphics);
            TextWriter txtWriter = new TextWriter(gWriter, 130, 0, "Meadow");
            Thread tWriter = new Thread(txtWriter.Start);
            tWriter.Start();
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
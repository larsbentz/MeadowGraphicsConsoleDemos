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

            await TwoConsoles();            
        }


        async Task TwoConsoles()
        {
            graphics.Clear(true);

            GraphicsConsole gc = new GraphicsConsole(graphics, false);
            gc.YTop = 0;
            gc.YBottom = graphics.Height / 2;
            gc.Indent = 0;
            gc.IndentRight = 0;
            gc.BorderColor = Color.Violet;
            gc.SetBorder();
            Thread t = new Thread(gc.Start);
            t.Start();

            GraphicsConsole gc2 = new GraphicsConsole(graphics, false);
            gc2.YTop = gc.YBottom + 3;
            gc2.Indent = 0;
            gc2.IndentRight = 0;
            gc2.BorderColor = Color.Red;
            gc2.SetBorder();
            gc2.WriteLineSleep = 200;
            Thread t2 = new Thread(gc2.Start);
            t2.Start();

            var rnd = new Random(123456);
            int cnt = 0;

            int oldRnd = -1;
            while (true)
            {
                cnt++;

                var rndnum = rnd.Next(0, 10);
                if (rndnum == oldRnd)
                    continue;

                if (rndnum < 3)
                {
                    var txtnum = rnd.Next(0, DemoData.Cons1.Length - 1);
                    var txt = DemoData.Cons1[txtnum].Text;
                    var color = DemoData.Cons1[txtnum].Color;
                    var scale = DemoData.Cons1[txtnum].Scale;
                    gc.WriteLine(txt, color, scale, true);

                }
                if (rndnum < 9)
                {
                    var txtnum2 = rnd.Next(0, DemoData.Strings.Length - 1);
                    var colnum = rnd.Next(0, DemoData.Colors.Length - 1);
                    var txt2 = DemoData.Strings[txtnum2];
                    var color2 = DemoData.Colors[colnum];
                    gc2.WriteLine(txt2, color2, true);
                }

                var rndsleep = rnd.Next(0, 10);
                if (rndsleep > 8)
                {
                    Thread.Sleep(1500);
                }

                if (cnt % 25 == 0)
                {
                    gc.Stop();
                }

                if (cnt % 30 == 0 && gc.Stopped)
                {
                    gc.Restart();
                }

                oldRnd = rndnum;
            }
        }



    }
}
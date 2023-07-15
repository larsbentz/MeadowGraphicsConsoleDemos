using Belasoft.MeadowLibrary;
using Meadow.Foundation;
using System;
using System.Threading;

namespace OneConsoleDemo
{
    internal class TextWriter
    {
        readonly Color[] colors = new Color[]
        {
             Color.Red, Color.Green, Color.Blue, Color.Orange, Color.Orchid, Color.Yellow, Color.YellowGreen,
             Color.PaleTurquoise
        };

        GraphicsWriter GW;

        int X, Y;

        string Text;

        public bool Running { get; private set; } = false;


        internal TextWriter(GraphicsWriter graphicsWriter, int x, int y, string text)
        {
            GW = graphicsWriter;
            X = x;
            Y = y;            
            Text = text;
        }

        public void Start()
        {
            var rnd = new Random(DateTime.Now.Millisecond);
            Running = true;

            int cnt = 0;
            int oldcol = 0;
            while (cnt < 16)
            {
                var rndcol = rnd.Next(0, colors.Length - 1);
                if (rndcol == oldcol) continue;
                oldcol = rndcol;

                GW.DrawText(X, Y, Text, colors[rndcol], Meadow.Foundation.Graphics.ScaleFactor.X2);
                Thread.Sleep(200);
                //var rndms = rnd.Next(1, 1000);
                //Thread.Sleep(500 + rndms);
                cnt++;
            }
            GW.DrawText(X, Y, Text, Color.Red, Meadow.Foundation.Graphics.ScaleFactor.X2);
        }

        public void Stop()
        {
            Running = false;
        }

    }
}

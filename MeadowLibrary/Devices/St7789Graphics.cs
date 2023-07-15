using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Hardware;

namespace MeadowCommonLib.Devices
{
    public class St7789Graphics
    {
        MicroGraphics _graphics;
        public MicroGraphics Graphics { get { return _graphics; } set { _graphics = value; } }

        public ISpiBus SpiBus { get; set; }

        public IPin ChipSelectPin { get; set; } = null;

        public IPin DcPin { get; set; }

        public IPin ResetPin { get; set; }

        public ColorMode ColorMode { get; set; }

        public IFont Font { get; set; }

        public RotationType RotationType { get; set; }


        public St7789Graphics(
            ISpiBus spiBus,
            IPin chipSelectPin,
            IPin dcPin,
            IPin resetPin,
            IFont font,
            ColorMode colorMode = ColorMode.Format16bppRgb565,
            RotationType rotationType = RotationType.Normal
            )
        {
            SpiBus = spiBus;
            ChipSelectPin = chipSelectPin;
            DcPin = dcPin;
            ResetPin = resetPin;
            ColorMode = colorMode;
            Font = font;
            RotationType = rotationType;

            Initialize();
        }

        void Initialize()
        {
            var display = new St7789
            (
                spiBus: SpiBus,
                chipSelectPin: ChipSelectPin,
                dcPin: DcPin,
                resetPin: ResetPin,
                width: 240, height: 240,
                colorMode: ColorMode
            );

            _graphics = new MicroGraphics(display)
            {
                Stroke = 1,
                CurrentFont = Font,
                Rotation = RotationType
            };

            _graphics.Clear(true);
            _graphics.Show();
        }

    }
}

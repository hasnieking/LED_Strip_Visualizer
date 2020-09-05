using System;
using System.IO.Ports;
using System.Threading;
using System.Drawing.Imaging;
using System.Drawing;


namespace LED_Strip_Visualizer
{
    class Program
    {

        private static int screenwidth = 1920;
        private static int screenheight = 1080;

        static void Main(string[] args)
        {
            //getScreenres();
            string currentColour = "";
            string newColour;

            //hello world
            Console.WriteLine("Hello World!");

            String[] ports;
            ports = SerialPort.GetPortNames();

            //write usb port names
            for (int i = 0; i < ports.Length; i++)
            {
                Console.WriteLine(ports[i]);
            }

            SerialPort arduino = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One);
            arduino.Open();
            Thread.Sleep(200);

            arduino.Write("000255000\n");

            String input;
            String command;
            input = Console.ReadLine();
            while (input.ToLower() != "stop")
            {
                newColour = getColour();
                if (newColour != currentColour)
                {
                    arduino.Write(newColour + '\n');
                }
                currentColour = newColour;
            }

            arduino.Close();
        }

        static string getColour()
        {
            //initialize string
            string colour = "";
            try
            {
                Bitmap captureBitmap = new Bitmap(screenwidth, screenheight, PixelFormat.Format32bppArgb);

                using (var captureGraphic = Graphics.FromImage(captureBitmap))
                {
                    //make screen capture
                    captureGraphic.CopyFromScreen(0, 0, 0, 0, captureBitmap.Size);
                    //captureBitmap.Save("capture.jpg", ImageFormat.Jpeg);
                    //get colour
                    Color pixelColor = captureBitmap.GetPixel(screenwidth / 2, screenheight / 2);
                    colour = pixelColor.R.ToString().PadLeft(3, '0') + pixelColor.G.ToString().PadLeft(3, '0') + pixelColor.B.ToString().PadLeft(3, '0');

                    Console.WriteLine(colour);
                }
                


            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return colour; //return colour in string format (255000255)
        }

    }

}

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
        private static string portname;

        static void Main(string[] args)
        {
            //variables for colours
            string currentColour = "";
            string newColour;

            //ask user for settings
            getScreenres();
            getPortname();

            try
            {

                SerialPort arduino = new SerialPort(portname, 9600, Parity.None, 8, StopBits.One);
                arduino.Open();
                Thread.Sleep(200);

                while (true)
                {
                    newColour = getColour();
                    if (newColour != currentColour)
                    {
                        arduino.Write(newColour + '\n');
                    }
                    currentColour = newColour;
                }

                //unreachable, but this should be the way to close
                arduino.Close();

            }

            //catch exception from arduino communication
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        static string getColour()
        {
            //initialize string
            string colour = "";
            try
            {
                Bitmap captureBitmap = new Bitmap(1, 1, PixelFormat.Format32bppArgb); //used bitmap
                using (Graphics captureGraphic = Graphics.FromImage(captureBitmap))
                {
                    
                    //make screen capture
                    captureGraphic.CopyFromScreen(960, 540, 0, 0, captureBitmap.Size);
                    
                    //get colour
                    Color pixelColor = captureBitmap.GetPixel(0, 0);
                    colour = pixelColor.R.ToString().PadLeft(3, '0') + pixelColor.G.ToString().PadLeft(3, '0') + pixelColor.B.ToString().PadLeft(3, '0');

                    Console.WriteLine(colour);
                }
            }

            //catch error from getting colour
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return colour; //return colour in string format (255000255)
        }

        //ask user for resolution screen
        private static void getScreenres()
        {
            Console.WriteLine("Screen width: ");
            screenwidth = int.Parse(Console.ReadLine());
            Console.WriteLine("Screen height: ");
            screenheight = int.Parse(Console.ReadLine());
        }

        private static void getPortname()
        {
            String[] ports;
            ports = SerialPort.GetPortNames();

            Console.WriteLine("Which of the following ports do you want to use?");

            //write usb port names
            for (int i = 0; i < ports.Length; i++)
            {
                Console.WriteLine(ports[i]);
            }

            portname = Console.ReadLine();
        }


    }

}

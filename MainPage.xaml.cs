using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using PhoneApp2.Resources;

//Import Microphone
using System.IO;
using System.Windows.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;


//Import Vibrator 
using Microsoft.Devices;
namespace PhoneApp2
{

    public partial class MainPage : PhoneApplicationPage
    {
        int volume = 0;
        int previousVolume = 0;
        Microphone microphone = Microphone.Default;
        byte[] buffer;
        MemoryStream stream = new MemoryStream();

        //initilize vibrator 
        VibrateController vibrator = VibrateController.Default;
        // Constructor
        private double minimumThreshold = 2000;
        private double changeV = 500;

        public MainPage()
        {


            InitializeComponent();


            DispatcherTimer dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromMilliseconds(0.1);
            dt.Tick += delegate { try { FrameworkDispatcher.Update(); } catch { } };
            dt.Start();

            microphone.BufferReady += new EventHandler<EventArgs>(microphone_BufferReady);//System.AccessViolationException 



        }
        private void microphone_BufferReady(object sender, EventArgs e)
        {
            microphone.GetData(buffer);

            // RMS Method
            double rms = 0;
            ushort byte1 = 0;
            ushort byte2 = 0;
            short value = 0;
            previousVolume = volume;

            rms = (short)(byte1 | (byte2 << 8));

            for (int i = 0; i < buffer.Length - 1; i += 2)
            {
                byte1 = buffer[i];
                byte2 = buffer[i + 1];

                value = (short)(byte1 | (byte2 << 8));
                rms += Math.Pow(value, 2);
            }
            rms /= (double)(buffer.Length / 2);
            volume = (int)Math.Floor(Math.Sqrt(rms));



            // vibrator.Start(TimeSpan.FromSeconds(0.2));
            //System.Diagnostics.Debug.WriteLine("Threshold exceeded");
            //System.Diagnostics.Debug.WriteLine("buffer.Length" + buffer.Length + " Volume:" + volume);






            System.Diagnostics.Debug.WriteLine("buffer.Length: " + buffer.Length + " previousVolume:" + previousVolume + " Volume:" + volume);
            if ((volume - previousVolume) > changeV)
            {
                vibrator.Start(TimeSpan.FromSeconds(0.2));


            }

        }

        /*
        private void thisbut_Click(object sender, RoutedEventArgs e)
        {

            ///if (thisbut.Content.Equals("Start"))
            {
                //microphone.GetData(buffer);
                thisbut.Content = "Stop";
                //vibrator.Start(TimeSpan.FromSeconds(0.07));
            }
            else if (thisbut.Content.Equals("Stop"))
            {
                thisbut.Content = "Start";
            }
            if (microphone.State == MicrophoneState.Stopped)
            {
                microphone.BufferDuration = TimeSpan.FromMilliseconds(60);
                buffer = new byte[microphone.GetSampleSizeInBytes(microphone.BufferDuration)];
                microphone.Start();
                System.Diagnostics.Debug.WriteLine("Threshold setted to:" + minimumThreshold);
            }

            while (stopbut.IsPressed==false)
            {
                textbox1.Text = "InsideWhileloop";
            }

        }*/

        private void stopbut_Click(object sender, RoutedEventArgs e)
        {

            if (microphone.State == MicrophoneState.Started)
            {
                microphone.Stop();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            /*if (thisbut.Content.Equals("Start"))
            {
                //microphone.GetData(buffer);
                thisbut.Content = "Stop";
                //vibrator.Start(TimeSpan.FromSeconds(0.07));
            }
            else if (thisbut.Content.Equals("Stop"))
            {
                thisbut.Content = "Start";
            }*/

            if (microphone.State == MicrophoneState.Stopped)
            {
                microphone.BufferDuration = TimeSpan.FromMilliseconds(100);
                buffer = new byte[microphone.GetSampleSizeInBytes(microphone.BufferDuration)];
                microphone.Start();
                System.Diagnostics.Debug.WriteLine("Threshold setted to:" + minimumThreshold);
            }
        }

        private void textbox1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void enter_Click(object sender, RoutedEventArgs e)
        {
            if (textbox1.Text.All(Char.IsDigit))
               changeV = Convert.ToInt32(textbox1.Text);
            else
                textbox1.Text = "Error, enter only number please";
        }





    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace wpf3d
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Window loaded");

            cam.Position = new Point3D(600 * 90, 6000, 600 * 90);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.W)
            {
                Point3D camPos = cam.Position;
                camPos += (cam.LookDirection * 30.0);
                cam.Position = camPos;
            }
            else if (e.Key == Key.A)
            {
                Point3D camPos = cam.Position;
                camPos -= (Vector3D.CrossProduct(cam.LookDirection, cam.UpDirection) * 90.0);
                cam.Position = camPos;
            }
            else if (e.Key == Key.D)
            {
                Point3D camPos = cam.Position;
                camPos += (Vector3D.CrossProduct(cam.LookDirection, cam.UpDirection) * 90.0);
                cam.Position = camPos;
            }
            else if (e.Key == Key.S)
            {
                Point3D camPos = cam.Position;
                camPos -= (cam.LookDirection * 30.0);
                cam.Position = camPos;
            }
            else if (e.Key == Key.Left)
            {
                Rotate(-3.0);
            }
            else if (e.Key == Key.Right)
            {
                Rotate(3.0);
            }
        }
        public void Rotate(double d)
        {
            double angleD = d;
            Vector3D lookDirection = cam.LookDirection;

            var m = new Matrix3D();
            m.Rotate(new Quaternion(cam.UpDirection, -angleD)); // Rotate about the camera's up direction to look left/right
            cam.LookDirection = m.Transform(cam.LookDirection);
        }
    }
}

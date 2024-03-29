﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        // TODO: one for now, will have many
        private TerrainTile tile;
        private GeometryModel3D terrain;
        private int x;
        private int y;
        private Stopwatch timer;
        private int frame = 0;
        private int cameraHeight = 100;
        private int heading = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Window loaded");

            timer = new Stopwatch();
            timer.Start();
            CompositionTarget.Rendering += GameLoop;

            tile = TerrainTile.FromFile(@"C:\Users\alank\git\aykay76\wpf3d\S02W079.hgt");

            terrain = new GeometryModel3D();
            terrain.Geometry = tile.GetMesh(6);
            terrain.Material = new DiffuseMaterial(new SolidColorBrush(Colors.White));
            scene.Children.Add(terrain);

            // adjust camera position to 2m above terrain at current point
            short terrainHeight = tile.GetHeight(600, 600);
            cam.Position = new Point3D(600 * 90, terrainHeight + cameraHeight, 600 * 90);
        }

        private void GameLoop(object sender, EventArgs e)
        {
            frame++;

            if (timer.ElapsedMilliseconds >= 1000)
            {
                fps.Content = frame.ToString();
                Console.WriteLine(frame);
                timer.Restart();
                frame = 0;
            }

            if (Keyboard.IsKeyDown(Key.W))
            {
                Point3D camPos = cam.Position;
                camPos += (cam.LookDirection * 9.0);

                camPos.Y = tile.GetHeight((int)(camPos.X / 90.0), (int)(camPos.Z / 90.0)) + cameraHeight;

                cam.Position = camPos;
            }
            if (Keyboard.IsKeyDown(Key.A))
            {
                Point3D camPos = cam.Position;
                camPos -= (Vector3D.CrossProduct(cam.LookDirection, cam.UpDirection) * 90.0);
                cam.Position = camPos;
            }
            if (Keyboard.IsKeyDown(Key.D))
            {
                Point3D camPos = cam.Position;
                camPos += (Vector3D.CrossProduct(cam.LookDirection, cam.UpDirection) * 90.0);
                cam.Position = camPos;
            }
            if (Keyboard.IsKeyDown(Key.S))
            {
                Point3D camPos = cam.Position;
                camPos -= (cam.LookDirection * 90.0);
                cam.Position = camPos;
            }
            if (Keyboard.IsKeyDown(Key.Left))
            {
                heading -= 3;
                Rotate(-3.0);
                bearing.Content = heading.ToString();
            }
            if (Keyboard.IsKeyDown(Key.Right))
            {
                heading += 3;
                Rotate(3.0);
                bearing.Content = heading.ToString();
            }
            if (Keyboard.IsKeyDown(Key.Up))
            {
                Point3D camPos = cam.Position;
                camPos.Y += 90.0;
                cam.Position = camPos;
            }
            if (Keyboard.IsKeyDown(Key.Down))
            {
                Point3D camPos = cam.Position;
                camPos.Y -= 90.0;
                cam.Position = camPos;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
        }
        public void Rotate(double d)
        {
            double angleD = d;
            
            var m = new Matrix3D();
            m.Rotate(new Quaternion(cam.UpDirection, -angleD)); // Rotate about the camera's up direction to look left/right
            cam.LookDirection = m.Transform(cam.LookDirection);
        }
    }
}

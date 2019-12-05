﻿using BoundaryVisualizer.Data;
using BoundaryVisualizer.Data.DataProviders;
using BoundaryVisualizer.Logic;
using Microsoft.Win32;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Project2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Visualizer Visualizer;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void BrowseGeoJsonFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "OSM Boundary maps|*.GeoJson",
                Title = "Please select a boundary map file",
            };
            if (openFileDialog.ShowDialog() == true)
            {
                FileGeoJsonLoader fileGeoJsonLoader = new FileGeoJsonLoader(openFileDialog.FileName);
                //MockGeoJsonLoader fileGeoJsonLoader = new MockGeoJsonLoader();
                await Task.Run(() =>
                {
                    Visualizer = new Visualizer(fileGeoJsonLoader, this.Dispatcher,new PopulationProviderBelgianProvinces(), 300);
                });
                RenderModel();
            }

        }

        private void RenderModel()
        {
            Model3DGroup modelgroup = Visualizer.CreateModelGroup();

            DirectionalLight dirLight1 = new DirectionalLight
            {
                Color = Colors.White,
                Direction = new Vector3D(0, -1, -8)
            };
            DirectionalLight dirlight2 = new DirectionalLight
            {
                Color = Colors.White,
                Direction = new Vector3D(0, 1, 8)
            };
            DirectionalLight dirlight3 = new DirectionalLight
            {
                Color = Colors.White,
                Direction = new Vector3D(1, 1, 8)
            };
            DirectionalLight dirLight4 = new DirectionalLight
            {
                Color = Colors.White,
                Direction = new Vector3D(1, -1, -8)
            };
            var maxSize = (modelgroup.Bounds.SizeX > modelgroup.Bounds.SizeY) ? modelgroup.Bounds.SizeX : modelgroup.Bounds.SizeY;

            modelgroup.Transform = new TranslateTransform3D(modelgroup.Bounds.SizeX / -2.0, modelgroup.Bounds.SizeY / 2.0, modelgroup.Bounds.SizeZ / -2.0);
            modelgroup.Children.Add(dirLight1);
            modelgroup.Children.Add(dirlight2);
            modelgroup.Children.Add(dirlight3);
            modelgroup.Children.Add(dirLight4);

            ModelVisual3D modelVisual3D = new ModelVisual3D
            {
                Content = (modelgroup)
            };
            Camera.Position = new Point3D(0, 0, maxSize * -6);
            Viewport.Children.Clear();
            Viewport.Children.Add(modelVisual3D);
            Viewport.FixedRotationPoint = new Point3D(modelgroup.Bounds.SizeX / -2.0 + modelgroup.Bounds.X, modelgroup.Bounds.SizeY / 2.0 + modelgroup.Bounds.Y, modelgroup.Bounds.SizeZ / -2.0 + modelgroup.Bounds.Z);
            Viewport.FixedRotationPointEnabled = true;
        }
    }
}

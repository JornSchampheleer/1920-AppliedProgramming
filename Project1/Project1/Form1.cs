﻿using ImageProcessing.Logic;
using ImageProcessing.Logic.Quantizers;
using ImageProcessing.Presentation;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Project1
{
    public partial class Form1 : Form
    {
        private string ImagePath;
        private ImageStore imageStore;
        private SimpleDrawer drawer;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void ButtonLoadImage_Click(object sender, EventArgs e)
        {
            if (OpenFileDialogImageLoader.ShowDialog()==DialogResult.OK)
            {
                ImagePath = OpenFileDialogImageLoader.FileName;
                LabelPath.Text = ImagePath;
                var image = new Bitmap(ImagePath);
                PictureBoxLoadedImage.Image = image;
                imageStore = new ImageStore(image, new SimpleQuantizer());
                drawer = new SimpleDrawer(imageStore);
                imageStore.InitFinished += AfterInit;
                drawer.ProgressUpdate += ProgressUpdate;
                
            }
        }

        private async void SetQuantizedImage()
        {
            PictureBoxQuantized.Image = await drawer.DrawAsync();
        }

        private async void AfterInit(object sender, EventArgs args)
        {
            PictureBoxHistogram.Image = await drawer.VisualizeHistogramAsync(PictureBoxHistogram.Height, PictureBoxHistogram.Width);
            // Quantizer must be populated first
            SetQuantizedImage();
        }

        private void ProgressUpdate(object sender, ProgressEventArgs args)
        {
            System.Diagnostics.Debug.WriteLine("Progress: " + args.Progress);
            ProgressBarQuantization.Invoke(new Action(() =>
            {
                ProgressBarQuantization.Value = args.Progress;
            }));
        }
    }
}

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
using System.Windows.Shapes;

namespace PlaytoztheGame
{
    public partial class ColorPickerDialog : Window
    {
        public Color SelectedColor { get; private set; }
        public Color InitialColor { get; internal set; }

        private Color tempColor;

        public ColorPickerDialog(Color initialColor)
        {
            InitializeComponent();
            tempColor = initialColor;
            UpdateColorPreview();
        }
        private void UpdateColorPreview()
        {
            colorPreview.Fill = new SolidColorBrush(tempColor);
            redSlider.Value = tempColor.R;
            greenSlider.Value = tempColor.G;
            blueSlider.Value = tempColor.B;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            byte r = (byte)redSlider.Value;
            byte g = (byte)greenSlider.Value;
            byte b = (byte)blueSlider.Value;
            tempColor = Color.FromRgb(r, g, b);
            UpdateColorPreview();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedColor = tempColor;
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}

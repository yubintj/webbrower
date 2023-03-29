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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;

namespace webbrower
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private WebView2 webView1, webView2, webView3, webView4;
        private Point startPoint;
        private Border draggingBorder = null;


        private async void MainGrid_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadWebPages();
        }

        public MainWindow()
        {
            InitializeComponent();
            /*
            webView1 = new WebView2();
            webView2 = new WebView2();
            webView3 = new WebView2();
            webView4 = new WebView2();

            MainGrid.Children.Add(webView1);
            MainGrid.Children.Add(webView2);
            MainGrid.Children.Add(webView3);
            MainGrid.Children.Add(webView4);

            Grid.SetRow(webView1, 0);
            Grid.SetColumn(webView1, 0);
            Grid.SetRow(webView2, 0);
            Grid.SetColumn(webView2, 1);
            Grid.SetRow(webView3, 1);
            Grid.SetColumn(webView3, 0);
            Grid.SetRow(webView4, 1);
            Grid.SetColumn(webView4, 1);*/
        }
        private async Task LoadWebPages()
        {
            await webView1.EnsureCoreWebView2Async(null);
            await webView2.EnsureCoreWebView2Async(null);
            await webView3.EnsureCoreWebView2Async(null);
            await webView4.EnsureCoreWebView2Async(null);

            webView1.CoreWebView2.Navigate("https://www.google.com");
            webView2.CoreWebView2.Navigate("https://www.microsoft.com");
            webView3.CoreWebView2.Navigate("https://www.apple.com");
            webView4.CoreWebView2.Navigate("https://www.amazon.com");

            webView1.ZoomFactor = (double)ActualHeight / webView1.ActualHeight;
            webView2.ZoomFactor = (double)ActualHeight / webView2.ActualHeight;
            webView3.ZoomFactor = (double)ActualHeight / webView3.ActualHeight;
            webView4.ZoomFactor = (double)ActualHeight / webView4.ActualHeight;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
            await LoadWebPages();

        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            draggingBorder = sender as Border;
            startPoint = e.GetPosition(draggingBorder);
            draggingBorder.CaptureMouse();
        }

        private void Border_MouseMove(object sender, MouseEventArgs e)
        {
            if (draggingBorder != null && e.LeftButton == MouseButtonState.Pressed)
            {
                Point endPoint = e.GetPosition(this);
                double dx = endPoint.X - startPoint.X;
                double dy = endPoint.Y - startPoint.Y;
                double left = Canvas.GetLeft(draggingBorder) + dx;
                double top = Canvas.GetTop(draggingBorder) + dy;
                Canvas.SetLeft(draggingBorder, left);
                Canvas.SetTop(draggingBorder, top);
            }
        }

        private void Border_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (draggingBorder != null)
            {
                draggingBorder.ReleaseMouseCapture();
                foreach (UIElement element in MainGrid.Children)
                {
                    if (element is Border border && border != draggingBorder)
                    {
                        Rect bounds1 = new Rect(draggingBorder.TranslatePoint(new Point(0, 0), MainGrid), draggingBorder.RenderSize);

                        //Rect bounds2 = border.TranslatePoint(new Point(0, 0), MainGrid);
                        Rect bounds2 = border.TransformToAncestor(MainGrid).TransformBounds(new Rect(0, 0, border.ActualWidth, border.ActualHeight));

                        if (bounds1.IntersectsWith(bounds2))
                        {
                            int draggingColumn = Grid.GetColumn(draggingBorder);
                            int draggingRow = Grid.GetRow(draggingBorder);
                            int column = Grid.GetColumn(border);
                            int row = Grid.GetRow(border);
                            Grid.SetColumn(draggingBorder, column);
                            Grid.SetRow(draggingBorder, row);
                            Grid.SetColumn(border, draggingColumn);
                            Grid.SetRow(border, draggingRow);
                            break;
                        }
                    }
                }
                draggingBorder = null;
            }
        }


    }
}

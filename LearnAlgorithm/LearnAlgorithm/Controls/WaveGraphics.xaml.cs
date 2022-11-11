using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace LearnAlgorithm.Controls
{
    /// <summary>
    /// WaveGraphics.xaml 的交互逻辑
    /// </summary>
    public partial class WaveGraphics : UserControl
    {
        public WaveGraphics()
        {
            InitializeComponent();
        }


        #region 依赖属性

        /// <summary>
        /// 要显示的数据
        /// </summary>
        public WaveGraphicsData WaveGraphicsData
        {
            get { return (WaveGraphicsData)GetValue(WaveGraphicsDataProperty); }
            set { SetValue(WaveGraphicsDataProperty, value); }
        }

        public static readonly DependencyProperty WaveGraphicsDataProperty =
            DependencyProperty.Register("WaveGraphicsData", typeof(WaveGraphicsData), typeof(WaveGraphics), new UIPropertyMetadata(null, WaveGraphicsDataChanged));

        private static void WaveGraphicsDataChanged(DependencyObject obj, DependencyPropertyChangedEventArgs arg)
        {
            WaveGraphics? view = obj as WaveGraphics;
            WaveGraphicsData? data = arg.NewValue as WaveGraphicsData;
            view?.DrawImage(data);
        }

        private void DrawImage(WaveGraphicsData? data)
        {
            DrawingGroup group = new DrawingGroup();
            using (DrawingContext ctx = group.Open())
            {
                DrawByContext(ctx, data);
            }

            DrawingImage drawimage = new DrawingImage(group);
            this.image.Source = drawimage;
        }

        private void DrawByContext(DrawingContext ctx, WaveGraphicsData data)
        {
            int PicWidth = 2000;
            int PicHeight = 800;
            int Margin = 50;          
            int YCenter = 400;

            //绘制外框
            ctx.DrawRectangle(Brushes.Black, new Pen(Brushes.Transparent, 1), new Rect(0, 0, PicWidth + Margin * 2, PicHeight + Margin * 2));
            ctx.DrawRectangle(Brushes.Black, new Pen(Brushes.White, 2), new Rect(Margin, Margin, PicWidth, PicHeight));

            //绘制X坐标
            for (int i = 1; i < 10; i++)
            {
                int X = Margin + i * PicWidth / 10;
                int Y0 = Margin;
                int Y1 = Margin + PicHeight;
                ctx.DrawLine(new Pen(Brushes.Gray, 1), new Point(X, Y0), new Point(X, Y1));
            }

            //绘制Y坐标
            for (int i = -4; i <= 4; i++)
            {
                int X0 = Margin;
                int X1 = Margin + PicWidth;
                int Y = Margin + YCenter - i * 80;
                ctx.DrawLine(new Pen(Brushes.Gray, 1), new Point(X0, Y), new Point(X1, Y));
            }

            if (data != null && data.WaveData != null)
            {
                if (data.GraphicsType == GraphicsType.Wave)
                {
                    //绘制X坐标
                    for (int i = 0; i <= 10; i++)
                    {
                        int X = Margin + i * PicWidth / 10;
                        int Y1 = Margin + PicHeight + 10;
                        FormattedText text = new FormattedText($"{i * 10}ms", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(new FontFamily("宋体"), FontStyles.Normal, FontWeights.Thin, FontStretches.Normal), 20, Brushes.White, VisualTreeHelper.GetDpi(this).PixelsPerDip);
                        ctx.DrawText(text, new Point(X - 20, Y1));
                    }

                    double DataMax = data.WaveData.Max();
                    if (DataMax < 0.1) DataMax = 1;

                    //绘制Y坐标
                    for (int i = -5; i <= 5; i++)
                    {
                        int Y = Margin + YCenter - i * 80;
                        FormattedText textMax = new FormattedText($"{i * DataMax / 5:0.#}", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(new FontFamily("宋体"), FontStyles.Normal, FontWeights.Thin, FontStretches.Normal), 20, Brushes.White);
                        ctx.DrawText(textMax, new Point(5, Y - 10));
                    }

                    ////绘制波形
                    for (int i = 0; i < data.WaveData.Length - 1; i++)
                    {
                        int X1 = Margin + i * 2;
                        int Y1 = Margin + YCenter - (int)(YCenter * data.WaveData[i] / DataMax);
                        int X2 = Margin + (i + 1) * 2;
                        int Y2 = Margin + YCenter - (int)(YCenter * data.WaveData[i + 1] / DataMax);

                        ctx.DrawLine(new Pen(Brushes.LightGreen, 2), new Point(X1, Y1), new Point(X2, Y2));
                    }
                }
                else
                {
                    //绘制X坐标
                    for (int i = 0; i <= 5; i++)
                    {
                        int X = Margin + i * 40;
                        int Y1 = Margin + PicHeight + 10;
                        FormattedText text = new FormattedText($"{i*100}", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(new FontFamily("宋体"), FontStyles.Normal, FontWeights.Thin, FontStretches.Normal), 20, Brushes.White);
                        ctx.DrawText(text, new Point(X - 10, Y1));
                    }
                    for (int i = 2; i <= 10; i++)
                    {
                        int X = Margin + i * 200;
                        int Y1 = Margin + PicHeight + 10;
                        FormattedText text = new FormattedText($"{(double)i / 2:0.0}k", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(new FontFamily("宋体"), FontStyles.Normal, FontWeights.Thin, FontStretches.Normal), 20, Brushes.White);
                        ctx.DrawText(text, new Point(X - 10, Y1));
                    }

                    double DataMax = data.WaveData.Max();
                    if (DataMax == 0) DataMax = 1;

                    //绘制Y坐标
                    for (int i =0; i <= 10; i++)
                    {
                        int Y = Margin + PicHeight - i * 80;
                        FormattedText textMax = new FormattedText($"{i * DataMax / 10:0.#}", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(new FontFamily("宋体"), FontStyles.Normal, FontWeights.Thin, FontStretches.Normal), 20, Brushes.White);
                        ctx.DrawText(textMax, new Point(5, Y - 10));
                    }

                    //绘制波形
                    for (int i = 0; i < data.WaveData.Length - 1; i++)
                    {
                        int X1 = Margin + i * 4;
                        int Y1 = Margin + PicHeight;
                        int X2 = X1;
                        int Y2 = Margin + PicHeight - (int)(PicHeight * data.WaveData[i] / DataMax);

                        ctx.DrawLine(new Pen(Brushes.LightGreen, 2), new Point(X1, Y1), new Point(X2, Y2));
                    }
                }
            }
        }

        #endregion
    }
}

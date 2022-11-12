using MathNet.Numerics.Statistics;
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
    /// ScatterplotNormal.xaml 的交互逻辑
    /// </summary>
    public partial class ScatterplotNormal : UserControl
    {
        public ScatterplotNormal()
        {
            InitializeComponent();
        }

        #region 依赖属性

        /// <summary>
        /// 要显示的数据
        /// </summary>
        public ScatterplotData ScatterplotData
        {
            get { return (ScatterplotData)GetValue(ScatterplotDataProperty); }
            set { SetValue(ScatterplotDataProperty, value); }
        }

        public static readonly DependencyProperty ScatterplotDataProperty =
            DependencyProperty.Register("ScatterplotData", typeof(ScatterplotData), typeof(ScatterplotNormal), new UIPropertyMetadata(null, ScatterplotDataChanged));

        private static void ScatterplotDataChanged(DependencyObject obj, DependencyPropertyChangedEventArgs arg)
        {
            ScatterplotNormal? view = obj as ScatterplotNormal;
            ScatterplotData? data = arg.NewValue as ScatterplotData;
            view?.DrawImage(data);
        }

        private void DrawImage(ScatterplotData? data)
        {
            DrawingGroup group = new DrawingGroup();
            using (DrawingContext ctx = group.Open())
            {
                DrawByContext(ctx, data);
            }

            DrawingImage drawimage = new DrawingImage(group);
            this.image.Source = drawimage;
        }

        private void DrawByContext(DrawingContext ctx, ScatterplotData? data)
        {
            int PicWidth = 2000;
            int PicHeight = 2000;
            int XCenter = 1000;
            int YCenter = 1000;
            int Margin = 100;

            //绘制外框
            ctx.DrawRectangle(Brushes.Black, new Pen(Brushes.Transparent, 1), new Rect(0, 0, PicWidth + Margin * 2, PicHeight + Margin * 2));
            ctx.DrawRectangle(Brushes.Black, new Pen(Brushes.White, 2), new Rect(Margin, Margin, PicWidth, PicHeight));

            //绘制X轴线
            for (int i = 1; i < 20; i++)
            {
                int X = Margin + i * 100;
                int Y0 = Margin;
                int Y1 = Margin + PicHeight;
                ctx.DrawLine(new Pen(Brushes.Gray, 1), new Point(X, Y0), new Point(X, Y1));
            }

            //绘制Y轴线
            for (int i = 1; i <= 20; i++)
            {
                int X0 = Margin;
                int X1 = Margin + PicWidth;
                int Y = Margin + i * 100;
                ctx.DrawLine(new Pen(Brushes.Gray, 1), new Point(X0, Y), new Point(X1, Y));
            }

            ctx.DrawLine(new Pen(Brushes.White, 2), new Point(Margin, Margin + YCenter), new Point(Margin + PicWidth, Margin + YCenter));


            //绘制X坐标
            for (int i = 0; i <= 20; i++)
            {
                int X = Margin + i * 100;
                int Y = Margin + PicHeight + 10;
                FormattedText text = new FormattedText($"{i * 10}", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(new FontFamily("宋体"), FontStyles.Normal, FontWeights.Thin, FontStretches.Normal), 30, Brushes.White, VisualTreeHelper.GetDpi(this).PixelsPerDip);
                ctx.DrawText(text, new Point(X - 30, Y));
            }


            if (data != null)
            {
                double yMax = data.SampleY.MaximumAbsolute() * 1.2;
                if (yMax < 0.01) yMax = 0.01;

                //绘制Y坐标
                FormattedText text = new FormattedText($"{0:0.0}", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(new FontFamily("宋体"), FontStyles.Normal, FontWeights.Thin, FontStretches.Normal), 30, Brushes.White, VisualTreeHelper.GetDpi(this).PixelsPerDip);
                ctx.DrawText(text, new Point(10, Margin + YCenter - 15));
                text = new FormattedText($"{yMax:0.0}", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(new FontFamily("宋体"), FontStyles.Normal, FontWeights.Thin, FontStretches.Normal), 30, Brushes.White, VisualTreeHelper.GetDpi(this).PixelsPerDip);
                ctx.DrawText(text, new Point(10, Margin - 15));
                text = new FormattedText($"{-yMax:0.0}", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(new FontFamily("宋体"), FontStyles.Normal, FontWeights.Thin, FontStretches.Normal), 30, Brushes.White, VisualTreeHelper.GetDpi(this).PixelsPerDip);
                ctx.DrawText(text, new Point(10, Margin + PicHeight - 15));

                //绘点
                int Count = data.SampleX.Length;
                for (int i = 0; i < Count; i++)
                {
                    double X = Margin + 10 * data.SampleX[i];
                    double Y = Margin + YCenter - 1000 * data.SampleY[i] / yMax;

                    ctx.DrawEllipse(Brushes.White, new Pen(Brushes.White, 1), new Point(X, Y), 3, 3);
                }

                //绘制结果               
                if (data.ScatterplotType == ScatterplotType.Function)
                {
                    double[] XPoints = new double[201];
                    double[] YPoints = new double[201];

                    for (int i = 0; i <= 200; i++)
                    {
                        double x = i * 10;
                        double y = data.ResultFunc(i);

                        XPoints[i] = x;
                        YPoints[i] = y;
                    }

                    for (int i = 0; i < 200; i++)
                    {
                        double X1 = Margin + XPoints[i];
                        double Y1 = Margin + YCenter - 1000 * YPoints[i] / yMax;

                        double X2 = Margin + XPoints[i + 1];
                        double Y2 = Margin + YCenter - 1000 * YPoints[i + 1] / yMax;

                        ctx.DrawLine(new Pen(Brushes.Red, 3), new Point(X1, Y1), new Point(X2, Y2));
                    }
                }
            }
        }

        #endregion
    }
}

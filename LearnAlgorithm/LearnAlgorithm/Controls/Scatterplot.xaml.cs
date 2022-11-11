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
    /// Scatterplot.xaml 的交互逻辑
    /// </summary>
    public partial class Scatterplot : UserControl
    {
        public Scatterplot()
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
            DependencyProperty.Register("ScatterplotData", typeof(ScatterplotData), typeof(Scatterplot), new UIPropertyMetadata(null, ScatterplotDataChanged));

        private static void ScatterplotDataChanged(DependencyObject obj, DependencyPropertyChangedEventArgs arg)
        {
            Scatterplot? view = obj as Scatterplot;
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
            int PicWidth = 2400;
            int PicHeight = 2000;
            int XCenter = 1200;
            int YCenter = 1000;
            int Margin = 100;

            //绘制外框
            ctx.DrawRectangle(Brushes.Black, new Pen(Brushes.Transparent, 1), new Rect(0, 0, PicWidth + Margin * 2, PicHeight + Margin * 2));
            ctx.DrawRectangle(Brushes.Black, new Pen(Brushes.White, 2), new Rect(Margin, Margin, PicWidth, PicHeight));

            //绘制X轴线
            for (int i = 1; i < 24; i++)
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
            ctx.DrawLine(new Pen(Brushes.White, 2), new Point(Margin + XCenter, Margin), new Point(Margin + XCenter, Margin + PicHeight));

            //绘制X坐标
            for (int i = -12; i <= 12; i++)
            {
                int X = Margin + XCenter + i * 100;
                int Y = Margin + PicHeight + 10;
                FormattedText text = new FormattedText($"{(double)i / 10:0.0}", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(new FontFamily("宋体"), FontStyles.Normal, FontWeights.Thin, FontStretches.Normal), 30, Brushes.White, VisualTreeHelper.GetDpi(this).PixelsPerDip);
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
                    double X = Margin + XCenter + 1000 * data.SampleX[i];
                    double Y = Margin + YCenter - 1000 * data.SampleY[i] / yMax;

                    ctx.DrawEllipse(Brushes.White, new Pen(Brushes.White, 1), new Point(X, Y), 3, 3);
                }

                //绘制结果
                //LineRegression
                if (data.ScatterplotType == ScatterplotType.LineRegression)
                {
                    double x1 = -1;
                    double y1 = data.LinearRegressionResult.p0 + data.LinearRegressionResult.p1 * x1;
                    double X1 = Margin + XCenter + 1000 * x1;
                    double Y1 = Margin + YCenter - 1000 * y1 / yMax;

                    double x2 = 1;
                    double y2 = data.LinearRegressionResult.p0 + data.LinearRegressionResult.p1 * x2;
                    double X2 = Margin + XCenter + 1000 * x2;
                    double Y2 = Margin + YCenter - 1000 * y2 / yMax;

                    ctx.DrawLine(new Pen(Brushes.Red, 3), new Point(X1, Y1), new Point(X2, Y2));
                }

                //PolynomialRegresion
                if (data.ScatterplotType == ScatterplotType.PolynomialRegresion)
                {
                    double[] YPoints = new double[201];

                    for (int i = 0; i <= 200; i++)
                    {
                        double x = (double)(i - 100) / 100;
                        double y = data.PolynomialRegresioResult[0]
                                 + data.PolynomialRegresioResult[1] * x
                                 + data.PolynomialRegresioResult[2] * Math.Pow(x, 2)
                                 + data.PolynomialRegresioResult[3] * Math.Pow(x, 3)
                                 + data.PolynomialRegresioResult[4] * Math.Pow(x, 4)
                                 + data.PolynomialRegresioResult[5] * Math.Pow(x, 5);
                        YPoints[i] = y;
                    }

                    for (int i = 0; i < 200; i++)
                    {
                        double X1 = Margin + 200 + 10 * i;
                        double Y1 = Margin + YCenter - 1000 * YPoints[i] / yMax;

                        double X2 = Margin + 200 + 10 * (i + 1);
                        double Y2 = Margin + YCenter - 1000 * YPoints[i + 1] / yMax;

                        ctx.DrawLine(new Pen(Brushes.Red, 3), new Point(X1, Y1), new Point(X2, Y2));
                    }
                }

                if (data.ScatterplotType == ScatterplotType.LinearCombination)
                {
                    double[] YPoints = new double[241];

                    for (int i = 0; i <= 240; i++)
                    {
                        double x = (double)(i - 120) / 100;
                        double y = data.ResultFunc(x);
                        YPoints[i] = y;
                    }

                    for (int i = 0; i < 240; i++)
                    {
                        double X1 = Margin  + 10 * i;
                        double Y1 = Margin + YCenter - 1000 * YPoints[i] / yMax;

                        double X2 = Margin  + 10 * (i + 1);
                        double Y2 = Margin + YCenter - 1000 * YPoints[i + 1] / yMax;

                        ctx.DrawLine(new Pen(Brushes.Red, 3), new Point(X1, Y1), new Point(X2, Y2));
                    }
                }
            }
        }

        #endregion
    }
}

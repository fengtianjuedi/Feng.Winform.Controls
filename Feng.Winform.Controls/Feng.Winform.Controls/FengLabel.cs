using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace Feng.Winform.Controls
{
    /// <summary>
    /// 水平对齐方式
    /// </summary>
    public enum HorizontalAlignment
    {
        Left = 1,
        Center = 2,
        Right = 3
    }

    /// <summary>
    /// 垂直对齐方式
    /// </summary>
    public enum VerticalAlignment
    {
        Top = 1,
        Center = 2,
        Bottom = 3
    }

    public partial class FengLabel : Control
    {
        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern IntPtr SetFocus(HandleRef hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, int lParam);
        /// <summary>
        /// 文本样式
        /// </summary>
        private TextFormatFlags textFlags = TextFormatFlags.WordBreak | TextFormatFlags.SingleLine;

        /*
        /// <summary>
        /// 重写控件的默认鼠标样式
        /// </summary>
        protected override Cursor DefaultCursor
        {
            get
            {
                return Cursors.IBeam;
            }
        }

        protected override ImeMode ImeModeBase
        {
            get
            {
                if (DesignMode)
                {
                    return base.ImeModeBase;
                }
                return System.Windows.Forms.ImeMode.NoControl;
            }
            set
            {
                base.ImeModeBase = value;
            }
        }
         * */

        private HorizontalAlignment textHorizontalAlignment = HorizontalAlignment.Left;
        /// <summary>
        /// 水平文本对齐
        /// </summary>
        public HorizontalAlignment TextHorizontalAlignment
        {
            get
            {
                return textHorizontalAlignment;
            }
            set
            {
                this.textHorizontalAlignment = value;
                this.Invalidate();
            }
        }
        private VerticalAlignment textVerticalAlignment = VerticalAlignment.Center;
        /// <summary>
        /// 垂直文本对齐
        /// </summary>
        public VerticalAlignment TextVerticalAlignment
        {
            get
            {
                return textVerticalAlignment;
            }
            set
            {
                this.textVerticalAlignment = value;
                this.Invalidate();
            }
        }
        private Color borderColor = Color.Black;
        /// <summary>
        /// 边框颜色
        /// </summary>
        public Color BorderColor
        {
            get
            {
                return this.borderColor;
            }
            set
            {
                this.borderColor = value;
            }
        }
        private int borderThickness = 1;
        /// <summary>
        /// 边框粗细
        /// </summary>
        public int BorderThickness
        {
            get
            {
                return this.borderThickness;
            }
            set
            {
                this.borderThickness = value;
            }
        }
        private int borderRadius = 0;
        /// <summary>
        /// 边框半径
        /// </summary>
        public int BorderRadius
        {
            get
            {
                return this.borderRadius;
            }
            set
            {
                this.borderRadius = value;
            }
        }
        /// <summary>
        /// 绘制文本的Rectangle
        /// </summary>
        private Rectangle DrawStringRectangle
        {
            get
            {
                if (this.ClientRectangle == new Rectangle())
                {
                    return new Rectangle(0, 0, 1, 1);
                }
                Rectangle drawStringRectangle = new Rectangle(1, 1, this.ClientRectangle.Width - 2, this.ClientRectangle.Height - 2);
                drawStringRectangle.X = drawStringRectangle.X + this.Padding.Left;
                drawStringRectangle.Y = drawStringRectangle.Y + this.Padding.Top;
                drawStringRectangle.Width = drawStringRectangle.Width - this.Padding.Left - this.Padding.Right;
                drawStringRectangle.Height = drawStringRectangle.Height - this.Padding.Top - this.Padding.Bottom;
                return drawStringRectangle;
            }
        }

        public FengLabel()
        {
            InitializeComponent();
            EnableDoubleBuffering();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            //设置bitmap的那种颜色为透明色
            /*
            Bitmap bitMap = new Bitmap(this.Width, this.Height);
            Graphics bitMapGraphics = Graphics.FromImage(bitMap);
            bitMapGraphics.FillRectangle(new SolidBrush(Color.White),0, 0, bitMap.Width, bitMap.Height);
            bitMap.MakeTransparent(Color.Black);
             * */
            Graphics graphics = pe.Graphics;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            //透明色变换
            /*
            float[][] mtxItens = {
            new float[] {1,0,0,0,0},
            new float[] {0,1,0,0,0},
            new float[] {0,0,1,0,0},
            new float[] {0,0,0,1,0},
            new float[] {0,0,0,0,1}};
            ColorMatrix colorMatrix = new ColorMatrix(mtxItens);
            ImageAttributes imageAttributes = new ImageAttributes();
            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            graphics.DrawImage(backgroundImage, ClientRectangle, 0.0f, 0.0f, backgroundImage.Width, backgroundImage.Height, GraphicsUnit.Pixel, imageAttributes);
            backgroundImage.Dispose();
             * */
            DrawBorder(graphics);
            textFlags = TextFormatFlags.WordBreak | TextFormatFlags.SingleLine;
            switch (textHorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    textFlags = textFlags | TextFormatFlags.Left;
                    break;
                case HorizontalAlignment.Center:
                    textFlags = textFlags | TextFormatFlags.HorizontalCenter;
                    break;
                case HorizontalAlignment.Right:
                    textFlags = textFlags | TextFormatFlags.Right;
                    break;
            }
            switch(textVerticalAlignment)
            {
                case VerticalAlignment.Top:
                    textFlags = textFlags | TextFormatFlags.Top;
                    break;
                case VerticalAlignment.Center:
                    textFlags = textFlags | TextFormatFlags.VerticalCenter;
                    break;
                case VerticalAlignment.Bottom:
                    textFlags = textFlags | TextFormatFlags.Bottom;
                    break;
            }
            TextRenderer.DrawText(graphics, this.Text, this.Font, this.DrawStringRectangle, this.ForeColor, textFlags);
        }

        /// <summary>
        /// 开启双缓冲,并使控件支持透明背景
        /// </summary>
        private void EnableDoubleBuffering()
        {
            // Set the value of the double-buffering style bits to true.
            this.SetStyle(ControlStyles.DoubleBuffer |
               ControlStyles.UserPaint |
               ControlStyles.AllPaintingInWmPaint |
               ControlStyles.SupportsTransparentBackColor,
               true);
            this.UpdateStyles();
        }

        /// <summary>
        /// 绘制边框
        /// </summary>
        /// <param name="g"></param>
        private void DrawBorder(Graphics g)
        {
            if (borderThickness <= 0)
                return;
            Pen pen = new Pen(borderColor, borderThickness);
            
            if (borderRadius <= 0)
            {
                g.DrawRectangle(pen, 0, 0, this.Width - 1, this.Height - 1);
                return;
            }

            // 要实现 圆角化的 矩形
            Rectangle rect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
            // 指定图形路径， 有一系列 直线/曲线 组成
            GraphicsPath borderPath = new GraphicsPath();
            borderPath.StartFigure();
            borderPath.AddArc(new Rectangle(new Point(rect.X, rect.Y), new Size(2 * borderRadius, 2 * borderRadius)), 180, 90);
            borderPath.AddLine(new Point(rect.X + borderRadius, rect.Y), new Point(rect.Right - borderRadius, rect.Y));
            borderPath.AddArc(new Rectangle(new Point(rect.Right - 2 * borderRadius, rect.Y), new Size(2 * borderRadius, 2 * borderRadius)), 270, 90);
            borderPath.AddLine(new Point(rect.Right, rect.Y + borderRadius), new Point(rect.Right, rect.Bottom - borderRadius));
            borderPath.AddArc(new Rectangle(new Point(rect.Right - 2 * borderRadius, rect.Bottom - 2 * borderRadius), new Size(2 * borderRadius, 2 * borderRadius)), 0, 90);
            borderPath.AddLine(new Point(rect.Right - borderRadius, rect.Bottom), new Point(rect.X + borderRadius, rect.Bottom));
            borderPath.AddArc(new Rectangle(new Point(rect.X, rect.Bottom - 2 * borderRadius), new Size(2 * borderRadius, 2 * borderRadius)), 90, 90);
            borderPath.AddLine(new Point(rect.X, rect.Bottom - borderRadius), new Point(rect.X, rect.Y + borderRadius));
            borderPath.CloseFigure();
            g.DrawPath(pen, borderPath);
            g.FillPath(new SolidBrush(BackColor), borderPath);
        }
    }
}

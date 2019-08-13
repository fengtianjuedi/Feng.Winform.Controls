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

namespace Feng.Winform.Controls
{
    public partial class FengTextBox : Control
    {
        TextBox textBox = new TextBox();

        public override string Text
        {
            get
            {
                return textBox.Text;
            }
            set
            {
                textBox.Text = value;
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

        public FengTextBox()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.DoubleBuffer |
               ControlStyles.UserPaint |
               ControlStyles.AllPaintingInWmPaint |
               ControlStyles.SupportsTransparentBackColor,
               true);
            this.UpdateStyles();
            textBox.BorderStyle = BorderStyle.None;
            this.Controls.Add(textBox);
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            height = textBox.Height + borderThickness * 2 + 2;
            base.SetBoundsCore(x, y, width, height, specified);
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            pevent.Graphics.Clear(Parent.BackColor); 
            if (Color.Transparent.ToArgb() == BackColor.ToArgb())
                textBox.BackColor = Color.White;
            else
                textBox.BackColor = BackColor;
            textBox.ForeColor = ForeColor;
        }
        
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            Graphics g = pe.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.CompositingQuality = CompositingQuality.HighQuality;
            if (borderThickness <= 0)
                return;
            Pen pen = new Pen(borderColor, borderThickness);

            if (borderRadius <= 0)
            {
                g.DrawRectangle(pen, 0, 0, this.Width - 1, this.Height - 1);
                g.FillRectangle(new SolidBrush(BackColor), 0, 0, this.Width - 1, this.Height - 1);
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

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            int y = Height - textBox.Height - borderThickness;
            textBox.Location = new Point(borderThickness + borderRadius, y);
            textBox.Size = new Size(this.Width - borderThickness * 2 - borderRadius * 2, this.Height - borderThickness);
        }
    }
}

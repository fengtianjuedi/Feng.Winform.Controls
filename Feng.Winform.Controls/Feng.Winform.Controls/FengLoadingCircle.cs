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
    public partial class FengLoadingCircle : Control
    {
        #region 常数

        private const double NumberOfDegreesInCircle = 360;
        private const double NumberOfDegreesInHalfCircle = NumberOfDegreesInCircle / 2;
        private const int DefaultInnerCircleRadius = 70; //默认内圈半径
        private const int DefaultOuterCircleRadius = 90; //默认外圈半径
        private const int DefaultNumberOfSpoke = 20; //默认辐条数
        private const int DefaultSpokeThickness = 10; //默认辐条粗细
        private readonly Color DefaultColor = Color.DeepSkyBlue; //默认颜色

        #endregion

        #region 局部变量

        private Timer m_Timer; //定时器
        private bool m_IsTimerActive; //是否激活定时器
        private int m_NumberOfSpoke; //辐条数
        private int m_SpokeThickness; //辐条粗细
        private int m_ProgressValue; //进度值
        private int m_OuterCircleRadius; //外圈半径
        private int m_InnerCircleRadius; //内圈半径
        private PointF m_CenterPoint; //中心坐标
        private Color m_Color; //控件高亮颜色
        private Color[] m_Colors; //颜色组
        private double[] m_Angles; //弧度组

        #endregion

        #region 属性

        /// <summary>
        /// 获取和设置控件高亮色
        /// </summary>
        /// <value>高亮色</value>
        [TypeConverter("System.Drawing.ColorConverter"),
        Category("LoadingCircle"),
        Description("获取和设置控件高亮色")]
        public Color Color
        {
            get
            {
                return m_Color;
            }
            set
            {
                m_Color = value;

                GenerateColorsPallet();
                Invalidate();
            }
        }

        /// <summary>
        /// 获取和设置外围半径
        /// </summary>
        /// <value>外围半径</value>
        [System.ComponentModel.Description("获取和设置外围半径"),
         System.ComponentModel.Category("LoadingCircle")]
        public int OuterCircleRadius
        {
            get
            {
                if (m_OuterCircleRadius == 0)
                    m_OuterCircleRadius = DefaultOuterCircleRadius;

                return m_OuterCircleRadius;
            }
            set
            {
                m_OuterCircleRadius = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 获取和设置内圆半径
        /// </summary>
        /// <value>内圆半径</value>
        [System.ComponentModel.Description("获取和设置内圆半径"),
         System.ComponentModel.Category("LoadingCircle")]
        public int InnerCircleRadius
        {
            get
            {
                if (m_InnerCircleRadius == 0)
                    m_InnerCircleRadius = DefaultInnerCircleRadius;

                return m_InnerCircleRadius;
            }
            set
            {
                m_InnerCircleRadius = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 获取和设置辐条数量
        /// </summary>
        /// <value>辐条数量</value>
        [System.ComponentModel.Description("获取和设置辐条数量"),
        System.ComponentModel.Category("LoadingCircle")]
        public int NumberSpoke
        {
            get
            {
                if (m_NumberOfSpoke == 0)
                    m_NumberOfSpoke = DefaultNumberOfSpoke;

                return m_NumberOfSpoke;
            }
            set
            {
                if (m_NumberOfSpoke != value && m_NumberOfSpoke > 0)
                {
                    m_NumberOfSpoke = value;
                    GenerateColorsPallet();
                    GetSpokesAngles();
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// 获取和设置一个布尔值，表示当前控件<see cref="T:LoadingCircle"/>是否激活。
        /// </summary>
        /// <value><c>true</c> 表示激活；否则，为<c>false</c>。</value>
        [System.ComponentModel.Description("获取和设置一个布尔值，表示当前控件是否激活。"),
        System.ComponentModel.Category("LoadingCircle")]
        public bool Active
        {
            get
            {
                return m_IsTimerActive;
            }
            set
            {
                m_IsTimerActive = value;
                ActiveTimer();
            }
        }

        /// <summary>
        /// 获取和设置辐条粗细程度。
        /// </summary>
        /// <value>辐条粗细值</value>
        [System.ComponentModel.Description("获取和设置辐条粗细程度。"),
        System.ComponentModel.Category("LoadingCircle")]
        public int SpokeThickness
        {
            get
            {
                if (m_SpokeThickness <= 0)
                    m_SpokeThickness = DefaultSpokeThickness;

                return m_SpokeThickness;
            }
            set
            {
                m_SpokeThickness = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 获取和设置旋转速度。
        /// </summary>
        /// <value>旋转速度</value>
        [System.ComponentModel.Description("获取和设置旋转速度。"),
        System.ComponentModel.Category("LoadingCircle")]
        public int RotationSpeed
        {
            get
            {
                return m_Timer.Interval;
            }
            set
            {
                if (value > 0)
                    m_Timer.Interval = value;
            }
        }

        #endregion
        public FengLoadingCircle()
        {
            InitializeComponent();
            SetStyle(ControlStyles.UserPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw |
            ControlStyles.SupportsTransparentBackColor, true);
            UpdateStyles();

            m_Color = DefaultColor;

            GenerateColorsPallet();
            GetSpokesAngles();
            GetControlCenterPoint();

            m_Timer = new Timer();
            m_Timer.Tick += new EventHandler(aTimer_Tick);
            ActiveTimer();

            this.Resize += new EventHandler(LoadingCircle_Resize);
        }

        void LoadingCircle_Resize(object sender, EventArgs e)
        {
            GetControlCenterPoint();
        }

        /// <summary>
        /// 定时任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void aTimer_Tick(object sender, EventArgs e)
        {
            ++m_ProgressValue;
            if (m_ProgressValue > m_NumberOfSpoke)
                m_ProgressValue = 1;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            if (m_NumberOfSpoke > 0)
            {
                pe.Graphics.SmoothingMode = SmoothingMode.HighQuality;

                int intPosition = m_ProgressValue;
                for (int intCounter = 0; intCounter < m_NumberOfSpoke; intCounter++)
                {
                    intPosition = intPosition % m_NumberOfSpoke;
                    DrawLine(pe.Graphics,
                             GetCoordinate(m_CenterPoint, m_InnerCircleRadius, m_Angles[intPosition]),
                             GetCoordinate(m_CenterPoint, m_OuterCircleRadius, m_Angles[intPosition]),
                             m_Colors[intCounter], m_SpokeThickness);
                    intPosition++;
                }
                TextFormatFlags textFlags = TextFormatFlags.WordBreak | TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;
                TextRenderer.DrawText(pe.Graphics, this.Text, this.Font, Rectangle.Round(this.DrawStringRectangle), this.ForeColor, textFlags);
            }
            base.OnPaint(pe);
        }

        #region 局部方法

        private Color Darken(Color _objColor, int _intPercent)
        {
            int intRed = _objColor.R;
            int intGreen = _objColor.G;
            int intBlue = _objColor.B;
            return Color.FromArgb(_intPercent, Math.Min(intRed, byte.MaxValue), Math.Min(intGreen, byte.MaxValue), Math.Min(intBlue, byte.MaxValue));
        }

        /// <summary>
        /// 生成转动颜色数组
        /// </summary>
        private void GenerateColorsPallet()
        {
            m_Colors = GenerateColorsPallet(m_Color, Active, m_NumberOfSpoke);
        }

        /// <summary>
        /// 根据特定颜色生成渐变的颜色数组
        /// </summary>
        /// <param name="_objColor">初始颜色</param>
        /// <param name="_blnShadeColor">是否激活</param>
        /// <param name="_intNbSpoke">辐条数量</param>
        /// <returns></returns>
        private Color[] GenerateColorsPallet(Color _objColor, bool _blnShadeColor, int _intNbSpoke)
        {
            Color[] objColors = new Color[NumberSpoke];

            byte bytIncrement = (byte)(byte.MaxValue / NumberSpoke);

            byte PERCENTAGE_OF_DARKEN = 0;

            for (int intCursor = 0; intCursor < NumberSpoke; intCursor++)
            {
                if (_blnShadeColor)
                {
                    if (intCursor == 0 || intCursor < NumberSpoke - _intNbSpoke)
                        objColors[intCursor] = _objColor;
                    else
                    {
                        PERCENTAGE_OF_DARKEN += bytIncrement;

                        if (PERCENTAGE_OF_DARKEN > byte.MaxValue)
                            PERCENTAGE_OF_DARKEN = byte.MaxValue;

                        objColors[intCursor] = Darken(_objColor, PERCENTAGE_OF_DARKEN);
                    }
                }
                else
                    objColors[intCursor] = _objColor;
            }
            return objColors;
        }

        /// <summary>
        /// 获取控件中心坐标
        /// </summary>
        private void GetControlCenterPoint()
        {
            m_CenterPoint = GetControlCenterPoint(this);
        }

        private PointF GetControlCenterPoint(Control _objControl)
        {
            return new PointF(_objControl.Width / 2, _objControl.Height / 2 - 1);
        }

        private void DrawLine(Graphics _objGraphics, PointF _objPointOne, PointF _objPointTwo,
                              Color _objColor, int _intLineThickness)
        {
            using (Pen objPen = new Pen(new SolidBrush(_objColor), _intLineThickness))
            {
                objPen.StartCap = LineCap.Round;
                objPen.EndCap = LineCap.Round;
                _objGraphics.DrawLine(objPen, _objPointOne, _objPointTwo);
            }
        }

        private PointF GetCoordinate(PointF _objCircleCenter, int _intRadius, double _dblAngle)
        {
            double dblAngle = Math.PI * _dblAngle / NumberOfDegreesInHalfCircle;

            return new PointF(_objCircleCenter.X + _intRadius * (float)Math.Cos(dblAngle),
                              _objCircleCenter.Y + _intRadius * (float)Math.Sin(dblAngle));
        }

        /// <summary>
        /// 获取辐条角度
        /// </summary>
        private void GetSpokesAngles()
        {
            m_Angles = GetSpokesAngles(NumberSpoke);
        }

        /// <summary>
        /// 生成辐条转动角度数组
        /// </summary>
        /// <param name="_intNumberSpoke"></param>
        /// <returns></returns>
        private double[] GetSpokesAngles(int _intNumberSpoke)
        {
            double[] Angles = new double[_intNumberSpoke];
            double dblAngle = (double)NumberOfDegreesInCircle / _intNumberSpoke;

            for (int shtCounter = 0; shtCounter < _intNumberSpoke; shtCounter++)
                Angles[shtCounter] = (shtCounter == 0 ? dblAngle : Angles[shtCounter - 1] + dblAngle);

            return Angles;
        }

        /// <summary>
        /// 激活定时器
        /// </summary>
        private void ActiveTimer()
        {
            if (m_IsTimerActive)
                m_Timer.Start();
            else
            {
                m_Timer.Stop();
                m_ProgressValue = 0;
            }

            GenerateColorsPallet();
            Invalidate();
        }

        /// <summary>
        /// 绘制文本的Rectangle
        /// </summary>
        private RectangleF DrawStringRectangle
        {
            get
            {
                RectangleF drawStringRectangle = new RectangleF(m_CenterPoint.X, m_CenterPoint.Y, 1, 1);
                drawStringRectangle.X = (float)(m_CenterPoint.X - m_InnerCircleRadius * Math.Cos(Math.PI * 0.25));
                drawStringRectangle.Y = (float)(m_CenterPoint.Y - m_InnerCircleRadius * Math.Sin(Math.PI * 0.25));
                drawStringRectangle.Width = (float)(m_InnerCircleRadius * Math.Cos(Math.PI * 0.25) * 2);
                drawStringRectangle.Height = (float)(m_InnerCircleRadius * Math.Sin(Math.PI * 0.25) * 2);
                return drawStringRectangle;
            }
        }

        #endregion
    }
}

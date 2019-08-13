namespace Feng.Winform.Controls.Demo
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.fengLoadingCircle1 = new Feng.Winform.Controls.FengLoadingCircle();
            this.fengLabel1 = new Feng.Winform.Controls.FengLabel();
            this.fengTextBoxEx1 = new Feng.Winform.Controls.FengTextBox();
            this.SuspendLayout();
            // 
            // fengLoadingCircle1
            // 
            this.fengLoadingCircle1.Active = true;
            this.fengLoadingCircle1.BackColor = System.Drawing.Color.Transparent;
            this.fengLoadingCircle1.Color = System.Drawing.Color.DeepSkyBlue;
            this.fengLoadingCircle1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.fengLoadingCircle1.InnerCircleRadius = 70;
            this.fengLoadingCircle1.Location = new System.Drawing.Point(326, 180);
            this.fengLoadingCircle1.Name = "fengLoadingCircle1";
            this.fengLoadingCircle1.NumberSpoke = 20;
            this.fengLoadingCircle1.OuterCircleRadius = 90;
            this.fengLoadingCircle1.RotationSpeed = 40;
            this.fengLoadingCircle1.Size = new System.Drawing.Size(213, 206);
            this.fengLoadingCircle1.SpokeThickness = 10;
            this.fengLoadingCircle1.TabIndex = 2;
            this.fengLoadingCircle1.Text = "加载中，请稍候";
            // 
            // fengLabel1
            // 
            this.fengLabel1.BorderColor = System.Drawing.Color.Black;
            this.fengLabel1.BorderRadius = 9;
            this.fengLabel1.BorderThickness = 0;
            this.fengLabel1.Location = new System.Drawing.Point(162, 249);
            this.fengLabel1.Name = "fengLabel1";
            this.fengLabel1.Size = new System.Drawing.Size(158, 31);
            this.fengLabel1.TabIndex = 1;
            this.fengLabel1.Text = "fengLabel1";
            this.fengLabel1.TextHorizontalAlignment = Feng.Winform.Controls.HorizontalAlignment.Left;
            this.fengLabel1.TextVerticalAlignment = Feng.Winform.Controls.VerticalAlignment.Center;
            // 
            // fengTextBoxEx1
            // 
            this.fengTextBoxEx1.BackColor = System.Drawing.Color.Tomato;
            this.fengTextBoxEx1.BorderColor = System.Drawing.Color.Black;
            this.fengTextBoxEx1.BorderRadius = 8;
            this.fengTextBoxEx1.BorderThickness = 1;
            this.fengTextBoxEx1.Font = new System.Drawing.Font("宋体", 42F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.fengTextBoxEx1.ForeColor = System.Drawing.Color.Maroon;
            this.fengTextBoxEx1.Location = new System.Drawing.Point(70, 106);
            this.fengTextBoxEx1.Name = "fengTextBoxEx1";
            this.fengTextBoxEx1.Size = new System.Drawing.Size(421, 68);
            this.fengTextBoxEx1.TabIndex = 0;
            this.fengTextBoxEx1.Text = "fengTextBoxEx1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(569, 391);
            this.Controls.Add(this.fengLoadingCircle1);
            this.Controls.Add(this.fengLabel1);
            this.Controls.Add(this.fengTextBoxEx1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private FengTextBox fengTextBoxEx1;
        private FengLabel fengLabel1;
        private FengLoadingCircle fengLoadingCircle1;













    }
}


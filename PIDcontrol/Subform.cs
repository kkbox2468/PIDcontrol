using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PIDcontrol
{
    public partial class Subform : Form
    {
        
        int frameX,frameY,frameW,frameSubW,frameH,frameSubH;
        public static float a,b;
        float y1Label,y2Label;
        public static float xLabel;
        Label[] lblxArray = new System.Windows.Forms.Label[4];
        Label[] lblyArray = new System.Windows.Forms.Label[4];

        
        public void InitPar()
        {
            frameX = 50;
            frameY = 40;
            frameW = 400;
            frameH = 200;
            frameSubW = frameW / 4;
            frameSubH = frameH / 4;
           

           
            
            
            try
            {
                y1Label = Convert.ToSingle(textBox2.Text);
                y2Label = (Convert.ToSingle(textBox1.Text) - y1Label) / 4.0f;
            }
            catch { }
        }
        public Subform()
        {
            InitializeComponent();

            InitPar();

            for (int i = 0; i < 4; i++)
            {
                lblxArray[i] = new System.Windows.Forms.Label();
                lblxArray[i].Font = new Font("Calibri", 9);
                lblxArray[i].Size = new Size(30, 14);
            }
            for (int i = 0; i < 4; i++)
            {
                lblyArray[i] = new System.Windows.Forms.Label();
                lblyArray[i].Font = new Font("Calibri", 9);
                lblyArray[i].Size = new Size(30, 14);
            }

        }
        protected override void OnPaint(PaintEventArgs g)
        {
                     
            Pen pen1 = new Pen(Color.Black, 2);
            Pen pen2 = new Pen(Color.Gray, 1);
            pen2.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

            g.Graphics.DrawRectangle(pen1, frameX, frameY, frameW, frameH);

            for (int i = 1; i < 4; i++)
            {
                g.Graphics.DrawLine(pen2, frameX, frameY + (4 - i) * frameSubH, frameX + frameW, frameY + (4 - i) * frameSubH);
                lblyArray[i].Location = new Point(frameX - 35, frameY + (4 - i) * frameSubH - 8);
                //lblyArray[i].Text = Convert.ToString(y1Label + y2Label * i);
                Controls.Add(lblyArray[i]);
            }
            for (int i = 1; i < 4; i++)
            {
                g.Graphics.DrawLine(pen2, frameX + i * frameSubW, frameY, frameX + i * frameSubW, frameY + frameH);
                lblxArray[i].Location = new Point(frameX + i * frameSubW - 5, frameY + frameH + 10);
                //lblxArray[i].Text = Convert.ToString(xLabel * i);
                Controls.Add(lblxArray[i]);
            }
        }
        public void ShowData(int n,int ii,int index, float[] pvArray, float[] spArray, float[] opArray, float[] tArray)
        {
            if (index > 1)
            {
                InitPar();
                Refresh();
                //Form1 s = new Form1();
                b = Convert.ToSingle(label12.Text);
                if (ii < n+1)
                { xLabel = b / 4.0f; }
                //if (ii> 40.0 | ii== 40)
                else
                {
                    b = Convert.ToSingle(tArray[n-1]/60.0+1.0);
                    a =Convert.ToSingle(tArray[0]/60.0);
                    xLabel = Convert.ToSingle((b-a ) / 4);
                    label12.Text = Convert.ToString(Math.Truncate(b * 100.0) / 100.0);
                    label13.Text = Convert.ToString(Math.Truncate(a * 100.0) / 100.0);
                }
                
             
                for (int i = 1; i < 4; i++)
                {
                   
                    lblyArray[i].Text = Convert.ToString(y1Label + y2Label * i);
                    
                }
                for (int i = 1; i < 4; i++)
                {
                    
                    lblxArray[i].Text = Convert.ToString(Math.Truncate((a+xLabel * i)*100.0)/100.0);
                   
                }


                Graphics g = CreateGraphics();
                g.Clip = new Region(new Rectangle(frameX, frameY, frameW, frameH));

                Point[] points1 = new Point[index];
                Point[] points2 = new Point[index];
                Point[] points3 = new Point[index];
                Pen pen1 = new Pen(Color.Blue, 1);
                Pen pen2 = new Pen(Color.Red, 1);
                Pen pen3 = new Pen(Color.Green, 1);

                for (int i = 0; i < index; i++)
                {
                    try
                    {

                        points1[i].X = 50 + Convert.ToInt32(Convert.ToSingle(frameW) * Convert.ToSingle(tArray[i] - 60.0 * a) / (xLabel * 240f));
                        points1[i].Y = (frameY + frameH) - Convert.ToInt32(Convert.ToSingle(frameH) * (pvArray[i] - y1Label) / (y2Label * 4f));
                        points2[i].X = 50 + Convert.ToInt32(Convert.ToSingle(frameW) * Convert.ToSingle(tArray[i] - 60.0 * a) / (xLabel * 240f));
                        points2[i].Y = (frameY + frameH) - Convert.ToInt32(Convert.ToSingle(frameH) * (spArray[i] - y1Label) / (y2Label * 4f));
                        points3[i].X = 50 + Convert.ToInt32(Convert.ToSingle(frameW) * Convert.ToSingle(tArray[i] - 60.0 * a) / (xLabel * 240f));
                        points3[i].Y = (frameY + frameH) - Convert.ToInt32(Convert.ToSingle(frameH) * (opArray[i]) / (100.0));
                    }
                    catch { }                
                }

                g.DrawLines(pen1, points1);
                g.DrawLines(pen2, points2);
                g.DrawLines(pen3, points3);

            }
        }

        
        private void Subform_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            InitPar();
            Refresh();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            InitPar();
            Refresh();
        }

        

        private void Subform_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        

        

        
        
        

    }
}

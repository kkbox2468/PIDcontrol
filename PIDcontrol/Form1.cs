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
    public partial class Form1 : Form
    {
        Subform subform = new Subform();
        public int ii,index,a;
        public static int n = 500;
        double sp, pv0, pv,op;
        double er0,er,erSum0,erSum,erDiff;
        double Kc, Ti, Td;
        static double Kp = 0.1d;
        static double tc = 8.0d;
        static double flowRate = 0.25d;
        double opss = flowRate * tc / Kp;
        double pointY,H;
        double TowerHeight = 10.0d;
        string p;
        float[] pvArray = new float[n];
        float[] spArray = new float[n];
        float[] opArray = new float[n];
        float[] tArray = new float[n];

       

        public Form1()
        {
            InitializeComponent();
        }
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            Pen MyPen = new Pen(Color.Blue, 2);
            e.Graphics.DrawRectangle(MyPen, 220, 152, 51, 277);
            
            SolidBrush MyBrush1 = new SolidBrush(Color.Gray);
            Rectangle Rect1 = new Rectangle(221, 153, 49, 275);
            e.Graphics.FillRectangle(MyBrush1, Rect1);
          
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            ii = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            switch (comboBox1.Text)
          {
           case "AUTO" :

              try
            {

              sp = Convert.ToDouble(textBox1.Text);

              if(sp>10)
             {
               sp=10;
               textBox1.Text = Convert.ToString(sp);
             }
              if(sp<0)
             {
               sp=0;
               textBox1.Text = Convert.ToString(sp);
             }

              Kc = 100.0F / Convert.ToDouble(textBox3.Text);
              Ti = Convert.ToDouble(textBox4.Text);
              Td = Convert.ToDouble(textBox5.Text);
            }
            catch { }

            if(ii==0)
            {
              er0 = (sp / TowerHeight*100.0)-(pv0/TowerHeight*100.0);
            }
            er = (sp / TowerHeight * 100.0) - (pv / TowerHeight * 100.0 );
            erDiff = er - er0;
            er0 = er;
            erSum = erSum0 + er;
            erSum0 = erSum;

            //op = opss - (Kc * er) - (Td * erDiff) - (erSum / Ti);
            op = opss - (Kc * er) - (Kc * Td * erDiff) - (Kc * erSum / Ti);
            if (op > 100.0F)
            {
                op = 100.0F;
            }
            if (op < 0.0F)
            {
                op = 0.0F;
            }
            pv = pv0 +( flowRate -( Kp * op) / tc);
            if(pv<0)
            { pv = 0.0F; }
            pv0 = pv;
   
           break;

           case "MAN":
            try
            {
             op = Convert.ToDouble(textBox2.Text);

             if (op > 100)
             {
                 op = 100;
                 textBox2.Text = Convert.ToString(op);
             }
             if (op < 0)
             {
                 op = 0;
                 textBox2.Text = Convert.ToString(op);
             }
            }
            catch { }

            pv = pv0 + (flowRate-( Kp * op) / tc);
            if(pv<0)
            { pv = 0.0F; }
            pv0 = pv;

           break;
         }
         ii++;
         
            
         TimeSpan ts1 = TimeSpan.FromSeconds(ii);
         label1.Text = Convert.ToString(ts1);
         if (index == n)
         {
             for (a = 0; a < n-1; a++)
             {
                 pvArray[a] = pvArray[a + 1];
                 tArray[a] = tArray[a + 1];
                 opArray[a] = opArray[a + 1];
                 spArray[a] = spArray[a + 1];
             }
             index = n - 1;
 
         }

         p = Convert.ToString(Math.Truncate(pv*100.0)/100.0);
         pvArray[index] = Convert.ToSingle(p);
         label5.Text = "液位:" + p + "m";
         label7.Text = p + "m";


         if (comboBox1.Text=="AUTO" )       
         {
           p = Convert.ToString(Math.Truncate(op*100.0)/100.0);
           textBox2.Text = p;
         }

         
         Graphics g = CreateGraphics();
         g.Clip = new Region(new Rectangle(220, 152, 51, 277));
         SolidBrush m = new SolidBrush(Color.Blue);

         pointY = (pv) * (-277.0 / 10) + 429;
         H = 429 - pointY;

         Rectangle Rect1 = new Rectangle(220, Convert.ToInt32(pointY), 50, Convert.ToInt32(H)); 
         g.FillRectangle(m, Rect1);

         SolidBrush m1 = new SolidBrush(Color.Gray);
         Rectangle Rect2 = new Rectangle(221, 153, 49, Convert.ToInt32(277.0 - H));
         g.FillRectangle(m1, Rect2);

         tArray[index] = Convert.ToSingle(ii);
         opArray[index] = Convert.ToSingle(op);
         spArray[index] = Convert.ToSingle(sp);

            
             index++; 
         
         subform.ShowData(n,ii,index, pvArray, spArray, opArray, tArray); 

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.Text)
            {
                case "AUTO":

                    textBox2.ReadOnly = true;

                    break;

                case "MAN":

                    textBox2.ReadOnly = false;

                    break;

            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            subform.Show();
        }
    }
}

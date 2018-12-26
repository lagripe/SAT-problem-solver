using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            label12.Text = "Statistics of " + Form1.U_OR_NOT.ToUpper();
            chart1.Series.Add(Form1.U_OR_NOT);
            chart1.Series[Form1.U_OR_NOT].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            chart1.Series[Form1.U_OR_NOT].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Auto;
            chart1.Series[Form1.U_OR_NOT].YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Auto;

            chart1.ChartAreas["0"].AxisY.Minimum = (Form1.Statics.Aggregate((l, r) => l.Value < r.Value ? l : r).Value/325)*100 - 10;
            chart1.ChartAreas["0"].AxisY.Interval = ((Form1.Statics.Aggregate((l, r) => l.Value > r.Value ? l : r).Value/325)*100-(chart1.ChartAreas["0"].AxisY.Minimum))/10;
            chart1.Series[Form1.U_OR_NOT].Color = Color.FromArgb(255, 49, 75);
            chart1.ChartAreas["0"].AxisX.Title = "Instances";
            chart1.ChartAreas["0"].AxisY.Title = "Satified Clauses";
            chart1.ChartAreas["0"].AxisY.LabelStyle.Format = "{0.00} %";

            foreach (KeyValuePair<int,double> Instance in Form1.Statics){
                chart1.Series[Form1.U_OR_NOT].Points.AddXY(Instance.Key, (Instance.Value/325) * 100);
            }
            

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}

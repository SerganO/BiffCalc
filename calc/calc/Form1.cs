using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace calc
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            initTable();
        }

        double m = 0;
        double a = 0;
        double b = 0;
        double k1 = 0;
        double k2 = 0;
        double fi1 = 0;
        double fi2 = 0;
        double left = 0;
        double right = 0;
        double leftBound = 0;
        double rightBound = 0;
        double up = 0;
        double down = 0;
        double h = 0;


        List<double> listX = new List<double>();
        List<double> listY1 = new List<double>();
        List<double> listY2 = new List<double>();
        List<double> listRY1 = new List<double>();
        List<double> listRY2 = new List<double>();
        List<double> listGG = new List<double>();
        List<double> listDGG = new List<double>();

        bool analytic = true;

        void initTable()
        {
            

            functionTable1.Columns.Add("X", "X");
            functionTable1.Columns.Add("Y1", "Y1");
            functionTable1.Columns.Add("Y2", "Y2");
            functionTable1.Columns.Add("RY1", "RY1");
            functionTable1.Columns.Add("RY2", "RY2");
            functionTable1.Columns.Add("GG", "GG");
            functionTable1.Columns.Add("DGG", "DGG");

            //functionTable1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //functionTable1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //functionTable1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            chart.Series[0].Color = Color.Black;
            chart.Series[1].Color = Color.Black;
            chart.Series[2].Color = Color.Black;
            chart.Series[3].Color = Color.Blue;
            chart.Series[4].Color = Color.DarkBlue;
            chart.Series[5].Color = Color.Red;
            chart.Series[6].Color = Color.DarkRed;

           
        }

        double function1(double x)
        {
            double l = a + b;
            double g1 = m * 9.8 * b / l;
            double q1 = k1 / g1;
            double c = q1 * q1 / fi1 / fi1;
            return (q1 * x) / Math.Sqrt(1+ c * x* x);
        }

        double function2(double x)
        {
            double l = a + b; 
            double g2 = m * 9.8 * a / l;  
            double q2 = k2 / g2;

            return q2 * x / Math.Sqrt(1 + (q2 * q2 *( x * x / (fi2 * fi2))));
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            try
            {
                updateGrafProp();
                updateSeriesVisible();
                int n = 3;
                var list = new List<(double, double)>();
                chart.Series[0 + n].Points.Clear();
                chart.Series[1 + n].Points.Clear();
                chart.Series[2 + n].Points.Clear();
                chart.Series[3 + n].Points.Clear();
                chart.Series[4 + n].Points.Clear();
                chart1.Series[0].Points.Clear();

                for (int i = 0; i < functionTable1.Rows.Count - 1; i++)
                {
                    var x = 0.0;
                    var y1 = 0.0;
                    var y2 = 0.0;
                    var ry1 = 0.0;
                    var ry2 = 0.0;
                    var gg = 0.0;
                    var dgg = 0.0;

                    x = listX[i];
                    y1 = listY1[i];
                    y2 = listY2[i];
                    ry1 = listRY1[i];
                    ry2 = listRY2[i];
                    gg = listGG[i];
                    dgg = listDGG[i];


                    if (chart.Series[0 + n].Enabled) chart.Series[0 + n].Points.AddXY(x, y1);
                    if (chart.Series[1 + n].Enabled) chart.Series[1 + n].Points.AddXY(x, ry1);
                    if (chart.Series[2 + n].Enabled) chart.Series[2 + n].Points.AddXY(x, y2);
                    if (chart.Series[3 + n].Enabled) chart.Series[3 + n].Points.AddXY(x, ry2);
                    if (chart.Series[4 + n].Enabled) chart.Series[4 + n].Points.AddXY(x, gg);
                    if (chart1.Series[0].Enabled) chart1.Series[0].Points.AddXY(x * dgg - gg, Math.Sqrt(9.81 * 1 / dgg));

                }
            } catch(Exception exc)
            {
                MessageBox.Show(exc.Message);
            }

        }

        private void setButton_Click(object sender, EventArgs e)
        {
            Double.TryParse(mTextBox.Text, out m);
            Double.TryParse(aTextBox.Text, out a);
            Double.TryParse(bTextBox.Text, out b);
            Double.TryParse(k1TextBox.Text, out k1);
            Double.TryParse(k2TextBox.Text, out k2);
            Double.TryParse(fi1TextBox.Text, out fi1);
            Double.TryParse(fi2TextBox.Text, out fi2);
            Double.TryParse(leftTextBox.Text, out left);
            Double.TryParse(rightTextBox.Text, out right);
            Double.TryParse(boundLeftTextBox.Text, out leftBound);
            Double.TryParse(boundRightTextBox.Text, out rightBound);
            Double.TryParse(hTextBox.Text, out h);

            

            int grade = 0;
            var th = h;
            while (th < 1)
            {
                th *= 10;
                grade++;
            }

            listX = new List<double>();
            listY1 = new List<double>();
            listY2 = new List<double>();
            listRY1 = new List<double>();
            listRY2 = new List<double>();
            listGG = new List<double>();
            listDGG = new List<double>();



            if (analytic)
            {
                functionTable1.Rows.Clear();
                for (double i = leftBound; i <= rightBound; i += h)
                {
                    var x = Math.Round(i, grade);
                    listX.Add(x);
                    listY1.Add(function1(x));
                    listY2.Add(function2(x));
                }
            }
            else
            {
                var spliney1 = new CubeSpline();
                var spliney2 = new CubeSpline();

                for (int i = 0; i < functionTable1.Rows.Count - 1; i++)
                {
                   double x = double.Parse((string)functionTable1.Rows[i].Cells[0].Value);
                   double y1 = double.Parse((string)functionTable1.Rows[i].Cells[1].Value);
                   double y2 = double.Parse((string)functionTable1.Rows[i].Cells[2].Value);

                    listX.Add(x);
                    listY1.Add(y1);
                    listY2.Add(y2);


                }
                functionTable1.Rows.Clear();
                spliney1.build_spline(listX.ToArray(), listY1.ToArray(), listX.Count);
                spliney2.build_spline(listX.ToArray(), listY2.ToArray(), listX.Count);

                listX = new List<double>();
                listY1 = new List<double>();
                listY2 = new List<double>();

                for (double i = leftBound; i <= rightBound; i += h)
                {
                    var x = Math.Round(i, grade);
                    listX.Add(x);
                    listY1.Add(spliney1.f(x));
                    listY2.Add(spliney2.f(x));
                }
            }


            var spline1 = new CubeSpline();
            spline1.build_spline(listY1.ToArray(), listX.ToArray(), listX.Count);

            var spline2 = new CubeSpline();
            spline2.build_spline(listY2.ToArray(), listX.ToArray(), listX.Count);

            for (int i = 0; i < listX.Count; i++)
            {
                listRY1.Add(spline1.f(listX[i]));
                listRY2.Add(spline2.f(listX[i]));

                listGG.Add(listRY2[i] - listRY1[i]);
            }

            for (int i = 0; i < listX.Count - 1; i++)
            {
                listDGG.Add((listGG[i + 1] - listGG[i]) / h);
            }

            listDGG.Add((listGG[listGG.Count - 1] - listGG[listGG.Count - 2]) / h);

            for (int i = 0; i < listX.Count; i++)
            {
                functionTable1.Rows.Add(listX[i],listY1[i],listY2[i],listRY1[i],listRY2[i],listGG[i],listDGG[i]);
            }
        }

        private void updateGraf_Click(object sender, EventArgs e)
        {

            updateGrafProp();
            updateSeriesVisible();
        }

        void updateGrafProp()
        {
            Double.TryParse(leftTextBox.Text, out left);
            Double.TryParse(rightTextBox.Text, out right);
            Double.TryParse(upTextBox.Text, out up);
            Double.TryParse(downTextBox.Text, out down);
            Double.TryParse(hTextBox.Text, out h);

            chart.Series[0].Points.Clear();
            chart.Series[1].Points.Clear();
            chart.Series[2].Points.Clear();
            var intervalX = 0.1;
            var intervalY = 0.1;
            double.TryParse(intervalXTextBox.Text, out intervalX);
            double.TryParse(intervalYTextBox.Text, out intervalY);

            chart.ChartAreas[0].AxisX.Minimum = left;
            chart.ChartAreas[0].AxisX.Maximum = right;
            chart.ChartAreas[0].AxisY.Minimum = down;
            chart.ChartAreas[0].AxisY.Maximum = up;
            chart.ChartAreas[0].AxisX.Interval = intervalX;
            chart.ChartAreas[0].AxisY.Interval = intervalY;


            var hor = Math.Max(Math.Abs(left), Math.Abs(right));
            var ver = Math.Max(Math.Abs(up), Math.Abs(down));

            chart.Series[0].Points.AddXY(-hor, 0);
            chart.Series[0].Points.AddXY(hor, 0);

            chart.Series[1].Points.AddXY(0, ver);
            chart.Series[1].Points.AddXY(0, -ver);

            chart.Series[2].Points.AddXY(-hor, -ver);
            chart.Series[2].Points.AddXY(hor, ver);
        }

        void updateSeriesVisible()
        {
            chart.Series[0].Enabled = checkBox1.Checked;
            chart.Series[1].Enabled = checkBox2.Checked;
            chart.Series[2].Enabled = checkBox3.Checked;
            chart.Series[3].Enabled = checkBox4.Checked;
            chart.Series[4].Enabled = checkBox5.Checked;
            chart.Series[5].Enabled = checkBox6.Checked;
            chart.Series[6].Enabled = checkBox7.Checked;
            chart.Series[7].Enabled = checkBox8.Checked;
            chart1.Series[0].Enabled = checkBox9.Checked;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        public DataPoint GetPointAtMouse(Chart c, MouseEventArgs e)
        {
            try
            {


                var result = c.HitTest(e.X, e.Y);
                // If the mouse if over a data point
                if (result.ChartElementType == ChartElementType.DataPoint)
                {
                    // Find selected data point
                    if(index() == 8)
                    {
                        var point = c.Series[0].Points[result.PointIndex];
                        return point;
                    } else
                    {
                        var point = c.Series[index()].Points[result.PointIndex];
                        return point;
                    }
                    
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        private void chart_MouseClick(object sender, MouseEventArgs e)
        {
            var data = GetPointAtMouse(sender as Chart, e);
            MessageBox.Show(data?.ToString() ?? "Miss click");
        }


        int index()
        {
            if (radioButton1.Checked) return 0;
            else if (radioButton2.Checked) return 1;
            else if (radioButton3.Checked) return 2;
            else if (radioButton4.Checked) return 3;
            else if (radioButton5.Checked) return 4;
            else if (radioButton6.Checked) return 5;
            else if (radioButton7.Checked) return 6;
            else if (radioButton8.Checked) return 7;
            else if (radioButton9.Checked) return 8;
            else return 0;
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            analytic = checkBox10.Checked;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        

        public Form1()
        {
            InitializeComponent();
        }
        public static String U_OR_NOT;
        private void button2_Click(object sender, EventArgs e)
        {
            button3.Enabled = false;
            richTextBox1.Text = "";
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(openFileDialog1.FileName))
            {
                String[] temp = openFileDialog1.FileName.Split('.');
                if (!temp[temp.Length - 1].Equals("cnf"))
                {
                    MessageBox.Show("enter a .cnf file");
                    return;
                }
                else
                {

                    textBox2.Text = openFileDialog1.FileName;
                    String[] ReadFile = File.ReadAllLines(textBox2.Text);
                    int cpt = 0;
                    foreach (String line in ReadFile)
                    {
                        if (string.IsNullOrEmpty(line))
                            continue;
                        if (cpt == 0 && line[0] == 'p')
                        {
                            cpt = 1;
                            try
                            {

                                label7.Text = API.CutWhiteSpace(line).Split(' ')[2];
                                label7.ForeColor = Color.Tomato;
                                label8.Text = API.CutWhiteSpace(line).Split(' ')[3];
                                label8.ForeColor = Color.Tomato;


                            }
                            catch (Exception z)
                            {
                                MessageBox.Show(z.Message);
                                return;
                            }
                        }
                        //dump in original file
                        richTextBox1.Text += line + Environment.NewLine;



                    }
                    button1.Enabled = true;

                }
            }
            String []tempd= textBox2.Text.Split('-')[0].Split('\\'); ;
            U_OR_NOT = tempd[(tempd.Count() - 1)];

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            API.NombreTester = 0;
            richTextBox2.Text = "";
            List<String> data = API.getClause(richTextBox1.Text);
            //Console.WriteLine(data.ElementAt(0));
            foreach (String line in data)
                richTextBox2.Text += line + Environment.NewLine;
            //get the clause table to search occurence in it;
            API.data_clause = API.ClauseTab(int.Parse(label8.Text), richTextBox2.Text);

            

            // Console.WriteLine(API.data_clause.Length);
            button3.Enabled = true;


        }
        Thread thread = null;
        public static DateTime startTime;
        private void button3_Click(object sender, EventArgs e)
        {
            
            API.BestClauseSatisfied = 0;
            button5.Enabled = false;
            button1.Enabled = false;
            button6.Enabled = false;
            if (thread!=null)
                thread.Abort();
            label4.Text = "0" ;
            
            startTime = DateTime.Now;
            groupBox5.Enabled = false;
            if (radioButton1.Checked)
            {
                //width

                timer1.Start();
                button4.Enabled = true;
                 thread = new Thread(() =>
                 {


                     String output = API.ByWidth(int.Parse(label7.Text), int.Parse(label8.Text));
                     timer1.Stop();
                     this.Invoke((MethodInvoker)delegate // To Write the Received data
                     {
                         if (output == null) { 
                             textBox1.Text = "NO Solution Found!";
                             textBox1.ForeColor = Color.Red;
                         }
                         else
                         {
                             textBox1.Text = output;
                             textBox1.ForeColor = Color.Green;
                         }
                         groupBox5.Enabled = true;
                         button4.Enabled = false;
                         timer1.Stop();
                     });


                });
                thread.Start();
               
                



            }
            if (radioButton2.Checked)
            {
                timer1.Start();
                button4.Enabled = true;
                thread = new Thread(() =>
                {


                    String output = API.ByDepth(int.Parse(label7.Text), int.Parse(label8.Text));
                    timer1.Stop();
                    this.Invoke((MethodInvoker)delegate // To Write the Received data
                    {
                        if (output == null)
                        {
                            textBox1.Text = "NO Solution Found!";
                            textBox1.ForeColor = Color.Red;
                        }
                        else
                        {
                            textBox1.Text = output;
                            textBox1.ForeColor = Color.Green;
                        }
                        groupBox5.Enabled = true;
                        button4.Enabled = false;
                        timer1.Stop();
                    });


                });
                thread.Start();
                //Depth

            }
            if (radioButton3.Checked)
            {
                //A*
                timer1.Start();
                button4.Enabled = true;
                thread = new Thread(() =>
                {


                    String output = API.ByFrequency(int.Parse(label7.Text), int.Parse(label8.Text));
                    timer1.Stop();
                    this.Invoke((MethodInvoker)delegate // To Write the Received data
                    {
                        if (output == null)
                        {
                            textBox1.Text = "NO Solution Found!";
                            textBox1.ForeColor = Color.Red;
                        }
                        else
                        {
                            textBox1.Text = output;
                            textBox1.ForeColor = Color.Green;
                        }
                        groupBox5.Enabled = true;
                        button4.Enabled = false;
                        timer1.Stop();
                    });


                });
                thread.Start();

            }
            if (radioButton5.Checked)
            {
                //HeuristicWeight
                timer1.Start();
                button4.Enabled = true;
                thread = new Thread(() =>
                {


                    String output = API.ByHeuristicWeightFrequencyHigh(int.Parse(label7.Text), int.Parse(label8.Text));
                    timer1.Stop();
                    this.Invoke((MethodInvoker)delegate // To Write the Received data
                    {
                        if (output == null)
                        {
                            textBox1.Text = "NO Solution Found!";
                            textBox1.ForeColor = Color.Red;
                        }
                        else
                        {
                            textBox1.Text = output;
                            textBox1.ForeColor = Color.Green;
                        }
                        groupBox5.Enabled = true;
                        button4.Enabled = false;
                        timer1.Stop();
                    });


                });
                thread.Start();

            }

            if (radioButton4.Checked)
            {
                //HeuristicWeight
                timer1.Start();
                button4.Enabled = true;
                thread = new Thread(() =>
                {


                    String output = API.ByHeuristicWeightLow(int.Parse(label7.Text), int.Parse(label8.Text));
                    timer1.Stop();
                    this.Invoke((MethodInvoker)delegate // To Write the Received data
                    {
                        if (output == null)
                        {
                            textBox1.Text = "NO Solution Found!";
                            textBox1.ForeColor = Color.Red;
                        }
                        else
                        {
                            textBox1.Text = output;
                            textBox1.ForeColor = Color.Green;
                        }
                        groupBox5.Enabled = true;
                        button4.Enabled = false;
                        timer1.Stop();
                    });


                });
                thread.Start();

            }

            if (radioButton6.Checked)
            {
                //Weight+Frequnecy that depends on the OLDPATH (OldPath don't interact with the Rest of Litterals)
                timer1.Start();
                button4.Enabled = true;
                thread = new Thread(() =>
                {


                    String output = API.ByHeuristicWeightFrequencyUsingOldPath(int.Parse(label7.Text), int.Parse(label8.Text));
                    timer1.Stop();
                    this.Invoke((MethodInvoker)delegate // To Write the Received data
                    {
                        if (output == null)
                        {
                            textBox1.Text = "NO Solution Found!";
                            textBox1.ForeColor = Color.Red;
                        }
                        else
                        {
                            textBox1.Text = output;
                            textBox1.ForeColor = Color.Green;
                        }
                        groupBox5.Enabled = true;
                        button4.Enabled = false;
                        timer1.Stop();
                    });


                });
                thread.Start();
            }
            
            if (radioButton7.Checked)
            {
                //HeuristicWeight
                timer1.Start();
                button4.Enabled = true;
                thread = new Thread(() =>
                {


                    String output = API.ByHeuristicWeightHigh(int.Parse(label7.Text), int.Parse(label8.Text));
                    timer1.Stop();
                    this.Invoke((MethodInvoker)delegate // To Write the Received data
                    {
                        if (output == null)
                        {
                            textBox1.Text = "NO Solution Found!";
                            textBox1.ForeColor = Color.Red;
                        }
                        else
                        {
                            textBox1.Text = output;
                            textBox1.ForeColor = Color.Green;
                        }
                        groupBox5.Enabled = true;
                        button4.Enabled = false;
                        timer1.Stop();
                    });


                });
                thread.Start();

            }


            if (radioButton8.Checked)
            {
                //HeuristicWeight
                timer1.Start();
                button4.Enabled = true;
                thread = new Thread(() =>
                {


                    String output = API.ByHeuristicWeightFrequencyLow(int.Parse(label7.Text), int.Parse(label8.Text));
                    timer1.Stop();
                    this.Invoke((MethodInvoker)delegate // To Write the Received data
                    {
                        if (output == null)
                        {
                            textBox1.Text = "NO Solution Found!";
                            textBox1.ForeColor = Color.Red;
                        }
                        else
                        {
                            textBox1.Text = output;
                            textBox1.ForeColor = Color.Green;
                        }
                        groupBox5.Enabled = true;
                        button4.Enabled = false;
                        timer1.Stop();
                    });


                });
                thread.Start();

            }

           

            if (radioButton11.Checked)
            {
                

                timer1.Start();
                button4.Enabled = true;
                textBox1.Text = "";
                thread = new Thread(() =>
                {
                    //Max Ietartion, flip,maxlocal, chances
                    BSO b = new BSO(Form3.max_iter, Form3.flip, Form3.max_local_iter, Form3.chances);
                    Bee b1 = b.Bso_search(int.Parse(label8.Text), int.Parse(label7.Text));
                    Console.WriteLine("nbr de clause satisfaites :" + b1.max_sat);
                    String output = "";
                    if (b1.max_sat == int.Parse(label8.Text))
                    {
                       
                        foreach(int value in b1.localarea)
                        {
                            output +=value+" " ;
                        }
                        

                    }
                    else
                    {
                    output="solution not found";
                    }
                    timer1.Stop();
                    this.Invoke((MethodInvoker)delegate // To Write the Received data
                    {
                        label4.Text =""+ b1.max_sat;
                        textBox1.Text = output;
                        groupBox5.Enabled = true;
                        button4.Enabled = false;
                        timer1.Stop();
                    });


                });
                thread.Start();
                //Depth

            }



        }

        private bool mouseDown;
        private Point lastLocation;
        private void Form1_Load(object sender, EventArgs e)
        {
            
            button1.Enabled = false;
            button3.Enabled = false;
            //Thread.Sleep(5000);

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            TimeSpan y = DateTime.Now.Subtract(startTime);
            label2.Text = y.ToString();
            label4.Text = "" + API.BestClauseSatisfied;
            label10.Text = API.NombreTester.ToString();
            if(y.Minutes == 15)
            {
                
                thread.Abort();
                groupBox5.Enabled = true;
                button4.Enabled = false;
                button5.Enabled = true;
                button5.Enabled = true;
                button6.Enabled = true;
                timer1.Stop();
                Statics[int.Parse(textBox2.Text.Trim().Split('-')[1].Split('.')[0])] = API.BestClauseSatisfied;
            }


        }

        private void button4_Click(object sender, EventArgs e)
        {
            
            thread.Abort();
            groupBox5.Enabled = true;
            button4.Enabled = false;
            button5.Enabled = true;
            button1.Enabled = true;
            button6.Enabled = true;
            timer1.Stop();
            Statics[int.Parse(textBox2.Text.Trim().Split('-')[1].Split('.')[0])]= API.BestClauseSatisfied;


        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (thread != null)
                if (thread.IsAlive)
                    thread.Abort();
        }
        public static Dictionary<int, double> Statics = new Dictionary<int, double>();
        private void button5_Click(object sender, EventArgs e)
        {
            if (Statics.Count == 0)
            {
                MessageBox.Show("No Statics Found!");
                return;
            }
            Form2 F2 = new Form2();
            F2.Show();
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (Statics.Count() == 0)
            {
                MessageBox.Show("No Statics Found to reset!");
                return;
            }
            Statics = new Dictionary<int, double>();
            MessageBox.Show(" Statics has been reseted!");
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void radioButton11_CheckedChanged(object sender, EventArgs e)
        {
            
        }
    }  
}

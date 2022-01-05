using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Диплом
{
    public partial class PCBForm : Form
    {
        public bool upDate = false;
        int id;
        public PCB pcb = new PCB();
        PCBDBForm pcbDbForm;

        public PCBForm(ref PCB pcb)
        {
            InitializeComponent();
            this.pcb = pcb;
        }

        private void PCBForm_Load(object sender, EventArgs e)
        {
            this.ActiveControl = label5;
            id = pcb.Id;
            if (pcb.Id != 0)
                FillTextBoxes();
            this.Load += new System.EventHandler(this.PCBForm_Load);
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void FillTextBoxes()
        {
            textBox1.Text = pcb.Name;
            textBox2.Text = pcb.Length.ToString();
            textBox3.Text = pcb.Width.ToString();
            textBox4.Text = pcb.Dencity.ToString();
            textBox5.Text = pcb.HeatSurfaceFilmK.ToString();
            textBox6.Text = pcb.HeatConduction.ToString();
            textBox7.Text = pcb.HeatCapacity.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pcbDbForm = new PCBDBForm();
            pcbDbForm.ShowDialog();
            try
            {
                if (pcbDbForm.pcb.Id != 0)
                {
                    id = pcbDbForm.pcb.Id;
                    textBox1.Text = pcbDbForm.pcb.Name.ToString();
                    textBox2.Text = pcbDbForm.pcb.Length.ToString();
                    textBox3.Text = pcbDbForm.pcb.Width.ToString();
                    textBox4.Text = pcbDbForm.pcb.Dencity.ToString();
                    textBox5.Text = pcbDbForm.pcb.HeatSurfaceFilmK.ToString();
                    textBox6.Text = pcbDbForm.pcb.HeatConduction.ToString();
                    textBox7.Text = pcbDbForm.pcb.HeatCapacity.ToString();
                }
            }
            catch (NullReferenceException) { };
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private int Save()
        {
            try
            {
                pcb.Id = id;
                pcb.Name = textBox1.Text;
                pcb.Length = Convert.ToInt32(textBox2.Text);
                pcb.Width = Convert.ToInt32(textBox3.Text);
                pcb.Height = 0.2;
                pcb.Dencity = Convert.ToDouble(textBox4.Text);
                pcb.HeatSurfaceFilmK = Convert.ToDouble(textBox5.Text);
                pcb.HeatConduction = Convert.ToDouble(textBox6.Text);
                pcb.HeatCapacity = Convert.ToDouble(textBox7.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка при вводе");
                return 1;
            }
            upDate = true;
            return 0;
        }

        private void Form1_FormClosing(Object sender, FormClosingEventArgs e)
        {
            bool isEmpty = false;
            if (textBox1.Text == "" || textBox1.Text == "0" ||
                textBox2.Text == "" || textBox2.Text == "0" ||
                textBox3.Text == "" || textBox3.Text == "0" ||
                textBox4.Text == "" || textBox4.Text == "0" ||
                textBox5.Text == "" || textBox5.Text == "0" ||
                textBox6.Text == "" || textBox6.Text == "0" ||
                textBox7.Text == "" || textBox7.Text == "0")
            {
                isEmpty = true;
                e.Cancel = true;
                MessageBox.Show("Заполните данные!");
            }

            if (!isEmpty)
            {
                if (textBox1.Text != pcb.Name || textBox2.Text != pcb.Length.ToString() || textBox3.Text != pcb.Width.ToString() ||
                    textBox4.Text != pcb.Dencity.ToString() || textBox5.Text != pcb.HeatSurfaceFilmK.ToString() ||
                    textBox6.Text != pcb.HeatConduction.ToString() || textBox7.Text != pcb.HeatCapacity.ToString())
                {
                    SaveForm saveForm = new SaveForm();
                    saveForm.ShowDialog();

                    switch (saveForm.yes)
                    {
                        case 0:
                            e.Cancel = true;
                            break;
                        case 1:
                            if (Save() == 1)
                                e.Cancel = true;
                            this.FormClosing -= new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
                            break;
                        case 2:
                            this.FormClosing -= new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
                            break;
                    }
                    Close();
                    this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
        }
    }
}

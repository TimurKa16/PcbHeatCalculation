using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Диплом
{
    public partial class ElementForm : Form
    {
        int id;
        public bool upDate = false;
        public int elementIndex;
        public int Exit = 0;
        private int lastCoord;
        Project project;
        List<string[]> elementList;
        List<string> lbList;
        Label[] label;
        TextBox[][] textBox;
        ElementDBForm elementDBForm;

        public ElementForm(ref Project project)
        {
            this.project = project;
            InitializeComponent();
        }

        private void ElementForm_Load(object sender, EventArgs e)
        {
            Exit = 0;
            Tab1();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            project.elementList.Add(new Element());
            project.elementList[project.elementList.Count - 1].Id = 0;
            Tab1();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void Create_Labels()
        {
            label = new Label[lbList.Count - 1];
            int a = 15, b = 15;
            for (int i = 0; i < lbList.Count - 1; i++)
            {
                label[i] = new Label();
                label[i].Name = "lb" + i.ToString();
                label[i].Parent = this;
                label[i].BackColor = Color.Orange;
                label[i].Left = a;
                label[i].Top = b;
                label[i].Size = new Size(105, 50);

                label[i].Text = lbList[i + 1];
                label[i].ForeColor = Color.Black;
                label[i].Font = new Font(label[i].Font, FontStyle.Bold);
                label[i].Font = new Font(label[i].Font.Name, 9, label[i].Font.Style);
                label[i].TextAlign = ContentAlignment.MiddleCenter;
                label[i].BorderStyle = BorderStyle.FixedSingle;
                label[i].BringToFront();
                a += 104;
            }
            this.ActiveControl = label[0];
        }

        private void CreateTextBoxes()
        {
            textBox = new TextBox[elementList.Count][];
            for (int i = 0; i < elementList.Count; i++)
                textBox[i] = new TextBox[elementList[0].Length - 1];
            int a = 15, b = 64, k = 0;
            for (int i = 0; i < elementList.Count; i++)
            {
                for (int j = 0; j < elementList[0].Length - 1; j++)
                {
                    textBox[i][j] = new TextBox();
                    textBox[i][j].Name = i.ToString() + " " + j.ToString();
                    textBox[i][j].Parent = this;
                    textBox[i][j].Left = a;
                    textBox[i][j].Top = b;
                    textBox[i][j].Multiline = true;
                    textBox[i][j].Size = new Size(105, 25);
                    textBox[i][j].ForeColor = Color.Black;
                    textBox[i][j].BackColor = Color.White;
                    textBox[i][j].TextAlign = HorizontalAlignment.Center;
                    textBox[i][j].BorderStyle = BorderStyle.FixedSingle;
                    if(j == textBox[0].Length - 1)
                    {
                        textBox[i][j].ReadOnly = true;
                        textBox[i][j].MouseClick += new MouseEventHandler(ButtonMouseClick);
                    }
                    textBox[i][j].MouseEnter += new EventHandler(ButtonMouseEnter);
                    textBox[i][j].MouseLeave += new EventHandler(ButtonMouseLeave);
                    textBox[i][j].MouseDoubleClick += new MouseEventHandler(ButtonMouseDoubleClick);
                    textBox[i][j].BringToFront();
                    a += 104;
                    k++;
                }
                a = 15;
                b += 24;
            }
        }

        private void Delbt()
        {
            if (textBox != null)
                for (int i = 0; i < textBox.Length; i++)
                    for (int j = 0; j < textBox[i].Length; j++)
                        textBox[i][j].Dispose();
        }

        private void Dellb()
        {
            if (label != null)
                for (int i = 0; i < label.Length; i++)
                    label[i].Dispose();
        }

        private void Tab1()
        {
            Delbt();
            Dellb();
            FillElement();
            Create_Labels();
            CreateTextBoxes();

            for (int i = 0; i < elementList.Count; i++)            
                for (int j = 0; j < elementList[0].Length - 1; j++)                
                    textBox[i][j].Text = elementList[i][j + 1];
        }





        private void ButtonMouseEnter(object sender, EventArgs e)
        {
            TextBox bt = sender as TextBox;
            int row = Convert.ToInt32(bt.Name[0].ToString());
            for (int i = 0; i < textBox[row].Length; i++)            
                textBox[row][i].BackColor = Color.Chocolate;            
        }

        private void ButtonMouseLeave(object sender, EventArgs e)
        {
            TextBox bt = sender as TextBox;
            int row = Convert.ToInt32(bt.Name[0].ToString());
            for (int i = 0; i < textBox[row].Length; i++)            
                textBox[row][i].BackColor = Color.White;
            
        }



        private void ButtonMouseClick(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            LocateElement(tb);
        }

        private void ButtonMouseDoubleClick(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            int row = Convert.ToInt32(tb.Name[0].ToString());

            elementDBForm = new ElementDBForm(project);
            elementDBForm.ShowDialog();

            try
            {
                if (elementDBForm.element.Id != 0)
                {
                    id = elementDBForm.element.Id;
                    textBox[row][0].Text = elementDBForm.element.Name.ToString();
                    textBox[row][1].Text = elementDBForm.element.Length.ToString();
                    textBox[row][2].Text = elementDBForm.element.Width.ToString();
                    textBox[row][3].Text = elementDBForm.element.Power.ToString();
                    textBox[row][4].Text = elementDBForm.element.MaxTemperature.ToString();
                }
            }
            catch (NullReferenceException) { };
        }

        private void FillElement()
        {
            lbList = new List<string> { "ID элемента", "Название",
                "Длина", "Ширина", "Мощность тепловыделения", "Допустимая температура", "Координаты" };
            elementList = new List<string[]>();

            for (int i = 0; i < project.elementList.Count; i++)
            {
                elementList.Add(new string[7]);
                elementList[elementList.Count - 1][0] = project.elementList[i].Id.ToString();
                elementList[elementList.Count - 1][1] = project.elementList[i].Name;
                elementList[elementList.Count - 1][2] = project.elementList[i].Length.ToString();
                elementList[elementList.Count - 1][3] = project.elementList[i].Width.ToString();
                elementList[elementList.Count - 1][4] = project.elementList[i].Power.ToString();
                elementList[elementList.Count - 1][5] = project.elementList[i].MaxTemperature.ToString();
                elementList[elementList.Count - 1][6] = project.elementList[i].XCoordinate + "; " + project.elementList[i].YCoordinate;
            }
        }

        private int Save()
        {
            try
            {
                for (int i = 0; i < project.elementList.Count; i++)
                {
                    project.elementList[i].Name = textBox[i][0].Text;
                    project.elementList[i].Length = Convert.ToInt32(textBox[i][1].Text);
                    project.elementList[i].Width = Convert.ToInt32(textBox[i][2].Text);
                    project.elementList[i].Power = Convert.ToDouble(textBox[i][3].Text);
                    project.elementList[i].MaxTemperature = Convert.ToInt32(textBox[i][4].Text);
                }
            }
            catch (Exception)
            {                
                MessageBox.Show("Ошибка при вводе");
                return 1;
            }
            upDate = true;
            return 0;
        }

        private void ElementForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool hasChanged = false;
            bool isEmpty = false;
            for (int i = 0; i < project.elementList.Count; i++)
                if (textBox[i][0].Text == "" || textBox[i][0].Text == "0" ||
                    textBox[i][1].Text == "" || textBox[i][1].Text == "0" ||
                    textBox[i][2].Text == "" || textBox[i][2].Text == "0" ||
                    textBox[i][3].Text == "" || textBox[i][3].Text == "0" ||
                    textBox[i][4].Text == "" || textBox[i][4].Text == "0")
                {
                    isEmpty = true;
                    e.Cancel = true;
                    MessageBox.Show("Заполните данные!");
                }

            if (!isEmpty)
            {


                for (int i = 0; i < project.elementList.Count; i++)
                {
                    if (textBox[i][0].Text != project.elementList[i].Name ||
                        textBox[i][1].Text != project.elementList[i].Length.ToString() ||
                        textBox[i][2].Text != project.elementList[i].Width.ToString() ||
                        textBox[i][3].Text != project.elementList[i].Power.ToString() ||
                        textBox[i][4].Text != project.elementList[i].MaxTemperature.ToString())
                        hasChanged = true;
                }
                if (hasChanged)
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
                            else
                                this.FormClosing -= new System.Windows.Forms.FormClosingEventHandler(this.ElementForm_FormClosing);
                            break;
                        case 2:
                            this.FormClosing -= new System.Windows.Forms.FormClosingEventHandler(this.ElementForm_FormClosing);
                            break;
                    }
                }
            }
        }

        private void LocateElement(TextBox tb)
        {
            string[] buf = tb.Name.Split(' ');
            elementIndex = Convert.ToInt32(buf[0]);
            Exit = 1;
            upDate = true;
            Close();
        }

        private void ContextMenuClick(object sender, EventArgs e)
        {

        }
    }
}


using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Диплом
{
    public partial class ElementDBForm : Form
    {
        Project project;
        public Element element;
        List<string[]> elementList;
        List<string> lbList;        
        Label[] lbHorizontal;
        Button[][] button;
        public ElementDBForm(Project project)
        {
            this.project = project;
            InitializeComponent();
        }

        private void ElementDBForm_Load(object sender, EventArgs e)
        {
            Tab1();
        }

        private void Create_Labels()
        {
            lbHorizontal = new Label[lbList.Count - 1];
            int a = 15, b = 15;
            for (int i = 0; i < lbList.Count - 1; i++)
            {
                lbHorizontal[i] = new Label();
                lbHorizontal[i].Name = "lb" + i.ToString();
                lbHorizontal[i].Parent = this;
                lbHorizontal[i].BackColor = Color.Orange;
                lbHorizontal[i].Left = a;
                lbHorizontal[i].Top = b;
                lbHorizontal[i].Size = new Size(105, 50);

                lbHorizontal[i].Text = lbList[i + 1];
                lbHorizontal[i].ForeColor = Color.Black;
                lbHorizontal[i].Font = new Font(lbHorizontal[i].Font, FontStyle.Bold);
                lbHorizontal[i].Font = new Font(lbHorizontal[i].Font.Name, 9, lbHorizontal[i].Font.Style);
                lbHorizontal[i].TextAlign = ContentAlignment.MiddleCenter;
                lbHorizontal[i].BorderStyle = BorderStyle.FixedSingle;
                lbHorizontal[i].BringToFront();
                a += 104;
            }
            this.ActiveControl = lbHorizontal[0];
        }

        private void Create_buttons()
        {
            button = new Button[elementList.Count][];
            for (int i = 0; i < elementList.Count; i++)
                button[i] = new Button[elementList[0].Length - 1];
            int a = 15, b = 64, k = 0;
            for (int i = 0; i < elementList.Count; i++)
            {
                for (int j = 0; j < elementList[0].Length - 1; j++)
                {
                    button[i][j] = new Button();
                    button[i][j].Name = i.ToString() + j.ToString();
                    button[i][j].Parent = this;
                    button[i][j].Left = a;
                    button[i][j].Top = b;
                    button[i][j].Size = new Size(105, 25);
                    button[i][j].ForeColor = Color.Black;
                    button[i][j].BackColor = Color.White;
                    button[i][j].FlatStyle = FlatStyle.Flat;
                    button[i][j].FlatAppearance.BorderSize = 1;
                    button[i][j].MouseEnter += new EventHandler(ButtonMouseEnter);
                    button[i][j].MouseLeave += new EventHandler(ButtonMouseLeave);
                    button[i][j].MouseClick += new MouseEventHandler(ButtonMouseClick);
                    button[i][j].BringToFront();
                    a += 104;
                    k++;
                }
                a = 15;
                b += 24;
            }
        }

        private void Delbt()
        {
            if (button != null)
                for (int i = 0; i < button.Length; i++)
                    for (int j = 0; j < button[i].Length; j++)
                        button[i][j].Dispose();
        }

        private void Dellb()
        {
            if (lbHorizontal != null)
                for (int i = 0; i < lbHorizontal.Length; i++)
                    lbHorizontal[i].Dispose();
        }

        private void Tab1()
        {
            DBClass.ReadElements(ref lbList, ref elementList);
            Delbt();
            Dellb();
            Create_Labels();
            Create_buttons();

            for (int i = 0; i < elementList.Count; i++)            
                for (int j = 0; j < elementList[0].Length - 1; j++)                
                    button[i][j].Text = elementList[i][j + 1]; 
        }

        private void ButtonMouseEnter(object sender, EventArgs e)
        {
            Button bt = sender as Button;
            int row = Convert.ToInt32(bt.Name[0].ToString());
            for (int i = 0; i < button[row].Length; i++)
                button[row][i].BackColor = Color.Chocolate;            
        }

        private void ButtonMouseLeave(object sender, EventArgs e)
        {
            Button bt = sender as Button;
            int row = Convert.ToInt32(bt.Name[0].ToString());
            for (int i = 0; i < button[row].Length; i++)            
                button[row][i].BackColor = Color.White;            
        }

        private void ButtonMouseClick(object sender, EventArgs e)
        {
            Button bt = sender as Button;
            int row = Convert.ToInt32(bt.Name[0].ToString());
            FillElement(row);
            Close();
        }

        private void FillElement(int row)
        {
            element = new Element();
            element.Id = Convert.ToInt32(elementList[row][0]);
            element.Name = elementList[row][1];
            element.Length = Convert.ToInt32(elementList[row][2]);
            element.Width = Convert.ToInt32(elementList[row][3]);
            element.Power = Convert.ToDouble(elementList[row][4]);
            element.MaxTemperature = Convert.ToDouble(elementList[row][5]);
        }
    }
}

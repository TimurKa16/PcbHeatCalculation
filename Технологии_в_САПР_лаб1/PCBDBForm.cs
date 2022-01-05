using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Диплом
{
    public partial class PCBDBForm : Form
    {
        public PCB pcb;
        public int pcbId;
        List<string[]> pcbList;
        List<string> lbList;
        Label[] lbHorizontal;
        Button[][] button;

        public PCBDBForm()
        {
            InitializeComponent();
        }

        private void PCBDBForm_Load(object sender, EventArgs e)
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
            button = new Button[pcbList.Count][];
            for (int i = 0; i < pcbList.Count; i++)
                button[i] = new Button[pcbList[0].Length - 2];
            int a = 15, b = 64, k = 0;
            for (int i = 0; i < pcbList.Count; i++)
            {
                for (int j = 0; j < pcbList[0].Length - 2; j++)
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
            DBClass.ReadPCBs(ref lbList, ref pcbList);
            Delbt();
            Dellb();
            Create_Labels();
            Create_buttons();

            for (int i = 0; i < pcbList.Count; i++)
            {

                for (int j = 0; j < pcbList[0].Length - 2; j++)
                {
                    if(j < 4)
                        button[i][j].Text = pcbList[i][j + 1];
                    else
                        button[i][j].Text = pcbList[i][j + 2];
                }
            }
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
            FillPCB(row);
            Close();
        }

        private void FillPCB(int row)
        {
            pcb = new PCB();
            pcb.Id = Convert.ToInt32(pcbList[row][0]);
            pcb.Name = pcbList[row][1];
            pcb.Length = Convert.ToInt32(pcbList[row][2]);
            pcb.Width = Convert.ToInt32(pcbList[row][3]);
            pcb.Height = Convert.ToDouble(pcbList[row][4]);
            pcb.Dencity = Convert.ToDouble(pcbList[row][5]);
            pcb.HeatSurfaceFilmK = Convert.ToDouble(pcbList[row][6]);
            pcb.HeatConduction = Convert.ToDouble(pcbList[row][7]);
            pcb.HeatCapacity = Convert.ToDouble(pcbList[row][8]);
        }
    }
}

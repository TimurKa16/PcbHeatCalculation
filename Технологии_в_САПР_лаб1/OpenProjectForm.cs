using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Диплом
{
    public partial class OpenProjectForm : Form
    {
        public bool yes = false;
        public int projectId { set; get; }
        List<string[]> projectList;
        List<string> lbList;
        
        public OpenProjectForm()
        {
            InitializeComponent();
        }

        private void OpenProjectForm_Load(object sender, EventArgs e)
        {
            DBClass.ReadProjects(ref projectList, ref lbList);
            Draw();
        }
        
        Label[] lbHorizontal;
        Button[][] button;

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
                lbHorizontal[i].Size = new Size(100, 50);
                lbHorizontal[i].Text = lbList[i + 1];
                lbHorizontal[i].ForeColor = Color.Black;
                lbHorizontal[i].Font = new Font(lbHorizontal[i].Font, FontStyle.Bold);
                lbHorizontal[i].Font = new Font(lbHorizontal[i].Font.Name, 9, lbHorizontal[i].Font.Style);
                lbHorizontal[i].TextAlign = ContentAlignment.MiddleCenter;
                lbHorizontal[i].BorderStyle = BorderStyle.FixedSingle;
                lbHorizontal[i].BringToFront();
                a += 99;
            }
            this.ActiveControl = lbHorizontal[0];
        }

        private void Create_buttons()
        {
            button = new Button[projectList.Count][];
            for (int i = 0; i < projectList.Count; i++)
                button[i] = new Button[projectList[0].Length - 1];
            int a = 15, b = 64, k = 0;
            for (int i = 0; i < projectList.Count; i++)
            {
                for (int j = 0; j < projectList[0].Length - 1; j++)
                {
                    button[i][j] = new Button();
                    button[i][j].Name = i.ToString() + j.ToString();              
                    button[i][j].Parent = this;
                    button[i][j].Left = a;
                    button[i][j].Top = b;
                    button[i][j].Size = new Size(100, 25);
                    button[i][j].ForeColor = Color.Black;
                    button[i][j].BackColor = Color.White;
                    button[i][j].FlatStyle = FlatStyle.Flat;
                    button[i][j].FlatAppearance.BorderSize = 1;
                    button[i][j].MouseEnter += new EventHandler(ButtonMouseEnter);
                    button[i][j].MouseLeave += new EventHandler(ButtonMouseLeave);
                    button[i][j].MouseClick += new MouseEventHandler(ButtonMouseClick);                    
                    button[i][j].BringToFront();
                    a += 99;
                    k++;
                }
                a = 15;
                b += 24;
            }

        }        

        private void Deltb(Button[][] tb)
        {
            if (tb != null)
                for (int i = 0; i < tb.Length; i++)
                    for (int j = 0; j < tb[i].Length; j++)
                        tb[i][j].Dispose();
        }

        private void Dellb()
        {
            if (lbHorizontal != null)
                for (int i = 0; i < lbHorizontal.Length; i++)
                    lbHorizontal[i].Dispose();
        }

        private void Draw()
        {           
            Deltb(button);
            Dellb();
            Create_Labels();
            Create_buttons();

            for (int i = 0; i < projectList.Count; i++)
            {
                for (int j = 0; j < projectList[0].Length - 1; j++)
                    button[i][j].Text = projectList[i][j + 1];
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
            projectId = Convert.ToInt32(projectList[row][0]);
            Close();
            yes = true;
        }
    }
}

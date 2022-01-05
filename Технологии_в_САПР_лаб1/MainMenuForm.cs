using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Диплом
{
    public partial class MainMenuForm : Form
    {
        bool projectIsCreated = false;
        CreateProjectForm createProjectForm;
        OpenProjectForm openProjectForm;
        ElementForm elementForm;
        PCBForm pcbForm;
        HelpForm helpForm = new HelpForm();
        AnalysisParametersForm analysisParametersForm;
        ModelForm modelForm;
        double grid;

        int[][] pcbMatrix;
        double[][] temperatureField;

        string projectName = "";
        bool yes = false;
        int projectId;
        int elementIndex;
        Element locatingElement;
        Project project;

        public MainMenuForm()
        {
            InitializeComponent();
            toolStripStatusLabel1.Text = "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DBClass.LoadDB();
            
            if (!yes)
            {
                CloseItems();
            }
            else
            {
                OpenItems();           
            }



        }

        private void CloseItems()
        {
            проектToolStripMenuItem.Enabled = false;
            анализToolStripMenuItem.Enabled = false;
            сохранитьToolStripMenuItem.Enabled = false;
            StartModellingBut.Enabled = false;
            SaveBut.Enabled = false;
            UpdateBut.Enabled = false;
        }

        private void OpenItems()
        {
            проектToolStripMenuItem.Enabled = true;
            анализToolStripMenuItem.Enabled = true;
            сохранитьToolStripMenuItem.Enabled = true;
            StartModellingBut.Enabled = true;
            SaveBut.Enabled = true;
            UpdateBut.Enabled = true;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            StartModelling();
        }

        private void AboutProgram(object sender, EventArgs e)
        {
            MessageBox.Show("Моделирование тепловой совместимости блока электроники (c) Камалов Тимур 4413");
        }

        //  Создать проект
        //  тык на кнопку, передает в форму CreateProjectForm, там идет запрос к БД, присылает ID проекта
        private void CreateProject(object sender, EventArgs e)
        {
            
            bool open = true;
            if (openProjectForm != null)
            {
                SaveForm saveForm = new SaveForm();
                saveForm.ShowDialog();

                switch (saveForm.yes)
                {
                    case 0:
                        open = false;
                        break;
                    case 1:
                        Save();
                        open = true;
                        break;
                    case 2:
                        open = true;
                        break;
                }
            }

            if (open)
            {
                project = new Project();
                project.pcb = new PCB();
                project.elementList = new List<Element>();

                projectIsCreated = true;
                createProjectForm = new CreateProjectForm();
                createProjectForm.ShowDialog();

                projectName = createProjectForm.projectName;
                if (projectName != "")
                {
                    toolStripStatusLabel1.Text = "Готово";
                    yes = true;
                    OpenItems();
                    string s = DateTime.Now.ToString();
                    string s1 = "";
                    for (int i = 0; i < 10; i++)
                        s1 += s[i];
                    project.Date = s1;
                    project.Name = projectName;
                }
                if (projectName == "")
                    toolStripStatusLabel1.Text = "Отмена";
            }
        }

        //  Открыть проект
        //  тык на кнопку, передает в форму OpenProjectForm, там идет запрос к БД, присылает ID проекта
        private void OpenProject(object sender, EventArgs e)
        {
            bool open = true;
            if (openProjectForm != null)
            {
                SaveForm saveForm = new SaveForm();
                saveForm.ShowDialog();

                switch (saveForm.yes)
                {
                    case 0:
                        open = false;
                        break;
                    case 1:
                        Save();
                        open = true;
                        break;
                    case 2:
                        open = true;
                        break;
                }
            }

            if (open)
            {
                project = new Project();
                project.pcb = new PCB();
                project.elementList = new List<Element>();

                projectIsCreated = false;
                openProjectForm = new OpenProjectForm();
                openProjectForm.ShowDialog();
                projectId = openProjectForm.projectId;
                if (openProjectForm.yes)
                    OpenItems();
                project.ProjectId = projectId;
                DBClass.ReadProject(ref project);
                DBClass.ReadPCB(ref project);
                DBClass.ReadProjectElements(ref project);
                if(project.pcb.Id != 0 && project.elementList.Count != 0)
                    DrawModel1();
                toolStripStatusLabel1.Text = "Готово";
            }
        }

        private void Exit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void выполнитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Выполнение";
            MessageBox.Show("Идет выполнение расчета перегрева");
        }

        private void AboutGod(object sender, EventArgs e)
        {
            MessageBox.Show("Камалов Тимур 2019г выпуска\n\re-mail: timur_kamalov97@mail.ru");
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            elementForm.ShowDialog();
            toolStripStatusLabel1.Text = "Добавлено";
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Сохранено";
        }

        private void PutPCB()
        {
            pcbForm = new PCBForm(ref project.pcb);
            pcbForm.ShowDialog();

            if (pcbForm.pcb.Id != 0)
            {
                project.pcb.Id = pcbForm.pcb.Id;
                project.pcb.Name = pcbForm.pcb.Name;
                project.pcb.Length = pcbForm.pcb.Length;
                project.pcb.Width = pcbForm.pcb.Width;
                project.pcb.Height = pcbForm.pcb.Height;
                project.pcb.Dencity = pcbForm.pcb.Dencity;
                project.pcb.HeatSurfaceFilmK = pcbForm.pcb.HeatSurfaceFilmK;
                project.pcb.HeatConduction = pcbForm.pcb.HeatConduction;
                project.pcb.HeatCapacity = pcbForm.pcb.HeatCapacity;
            }
            if (pcbForm.upDate)
                DrawModel1();
        }

        private void PCB(object sender, EventArgs e)
        {
            PutPCB();
        }

        private void SaveAs(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Сохранено";
        }

        private void Help(object sender, EventArgs e)
        {
            helpForm.ShowDialog();
        }

        private void оПрограммеToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Хорошая программа");
        }

        private void времяМоделированияToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void добавитьToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void добавитьToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            //ReadPCB();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void StartModelling()
        {
            if (project.pcb != null && project.elementList.Count != 0 && model1 != null)
            {
                double[] Power = new double[project.elementList.Count];
                for (int i = 0; i < Power.Length; i++)
                    Power[i] = project.elementList[i].Power;

                pcbMatrix = new int[model1.Length][];
                for (int i = 0; i < model1.Length; i++)
                    pcbMatrix[i] = new int[model1[0].Length];

                for (int i = 0; i < model1.Length; i++)
                    for (int j = 0; j < model1[0].Length; j++)
                        pcbMatrix[i][j] = Convert.ToInt32(model1[i][j].Text);

                ThermalModelling.Modelling(project.pcb.Dencity, project.pcb.HeatConduction, project.pcb.HeatSurfaceFilmK,
                    project.pcb.HeatCapacity, project.pcb.Length, project.pcb.Width, project.pcb.Height, project.totalTime,
                    ref temperatureField, project.firstOverheat, Power, pcbMatrix);
                DrawModel2();
                toolStripStatusLabel1.Text = "Готово";
            }
            else
            {
                MessageBox.Show("Не все данные введены");
            }
        }

        private void выполнитьToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            StartModelling();
        }

        private void AnalysisParameters(object sender, EventArgs e)
        {
            analysisParametersForm = new AnalysisParametersForm(ref project);
            analysisParametersForm.ShowDialog();
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void проектToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        #region Model1
        Label[][] model1;
        int length1;
        int width1;

        private void CreatePCBLabels1()
        {
            int a = 15, b = 15;
            for (int i = 0; i < width1; i++)
            {
                for (int j = 0; j < length1; j++)
                {
                    model1[i][j] = new Label();
                    model1[i][j].Name = i.ToString() + " " + j.ToString();
                    model1[i][j].Parent = tabPage1;
                    model1[i][j].BackColor = Color.Green;
                    model1[i][j].Left = a;
                    model1[i][j].Top = b;
                    model1[i][j].Size = new Size(50, 50);

                    model1[i][j].Text = "0";
                    model1[i][j].ForeColor = model1[i][j].BackColor;
                    model1[i][j].Font = new Font(model1[i][j].Font.Name, 9, model1[i][j].Font.Style);
                    model1[i][j].TextAlign = ContentAlignment.MiddleCenter;
                    model1[i][j].BorderStyle = BorderStyle.FixedSingle;
                    model1[i][j].BringToFront();
                    a += 49;
                }
                b += 49;
                a = 15;
            }
            if (model1.Length != 0)
                this.ActiveControl = model1[0][0];
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void Dellb1()
        {
            if (model1 != null)
                for (int i = 0; i < model1.Length; i++)
                    for (int j = 0; j < model1[0].Length; j++)
                        model1[i][j].Dispose();
        }

        private void DrawElements1()
        {
            for (int i = 0; i < project.elementList.Count; i++)         // проходим все элементы в проекте
            {
                int x = project.elementList[i].XCoordinate - 1;
                int y = project.elementList[i].YCoordinate - 1;
                int xSize = project.elementList[i].Length;
                int ySize = project.elementList[i].Width;
                for (int ii = y; ii < y + ySize; ii++)
                    for (int jj = x; jj < x + xSize; jj++)
                    {
                        model1[ii][jj].BackColor = Color.Black;
                        model1[ii][jj].ForeColor = Color.White;
                        model1[ii][jj].Text = (i + 1).ToString();
                    }
            }
        }

        private void DrawModel1()
        {
            Dellb1();
            length1 = project.pcb.Length;
            width1 = project.pcb.Width;
            model1 = new Label[width1][];
            for (int i = 0; i < width1; i++)
                model1[i] = new Label[length1];
            CreatePCBLabels1();
            DrawElements1();
        }

        #endregion

        #region Model2

        Label[][] model2;
        int length2;
        int width2;

        private void CreatePCBLabels2()
        {
            int a = 80, b = 15;
            for (int i = 0; i < width2; i++)
            {
                for (int j = 0; j < length2; j++)
                {
                    model2[i][j] = new Label();
                    model2[i][j].Name = i.ToString() + j.ToString();
                    model2[i][j].Parent = tabPage2;                    
                    model2[i][j].Left = a;
                    model2[i][j].Top = b;
                    model2[i][j].Size = new Size(50, 50);
                    

                    model2[i][j].Text = Math.Round(temperatureField[i][j], 2).ToString();

                    if (temperatureField[i][j] >= 0 && temperatureField[i][j] <= 40)
                        model2[i][j].BackColor = Color.FromArgb(255, 255 - Convert.ToInt32(temperatureField[i][j] * 6), 0);
                    else
                    {
                        model2[i][j].BackColor = Color.FromArgb(255, 255 - Convert.ToInt32(temperatureField[i][j] * 6), 0);
                        model2[i][j].ForeColor = Color.White;
                    }
                    
                    model2[i][j].ForeColor = Color.Black;
                    model2[i][j].Font = new Font(model2[i][j].Font.Name, 9, model2[i][j].Font.Style);
                    model2[i][j].TextAlign = ContentAlignment.MiddleCenter;
                    model2[i][j].BorderStyle = BorderStyle.FixedSingle;
                    model2[i][j].BringToFront();
                    a += 49;
                }
                b += 49;
                a = 80;
            }
            this.ActiveControl = model2[0][0];
        }

        private void Dellb2()
        {
            if (model2 != null)
                for (int i = 0; i < model2.Length; i++)
                    for (int j = 0; j < model2[0].Length; j++)
                        model2[i][j].Dispose();
        }

        private void DrawElements2()
        {
            for (int i = 0; i < project.elementList.Count; i++)         // проходим все элементы в проекте
            {
                int x = project.elementList[i].XCoordinate - 1;
                int y = project.elementList[i].YCoordinate - 1;
                int xSize = project.elementList[i].Length;
                int ySize = project.elementList[i].Width;
                for (int ii = y; ii < y + ySize; ii++)
                    for (int jj = x; jj < x + xSize; jj++)
                    {
                        model2[ii][jj].BackColor = Color.White;
                        model2[ii][jj].ForeColor = Color.White;
                        model2[ii][jj].Text = (i + 1).ToString();
                    }
            }
        }

        private void DrawModel2()
        {
            Dellb2();
            length2 = project.pcb.Length;
            width2 = project.pcb.Width;
            model2 = new Label[width2][];
            for (int i = 0; i < width2; i++)
                model2[i] = new Label[length2];
            CreatePCBLabels2();
        }

        #endregion

        private void LabelMouseEnter(object sender, EventArgs e)
        {
            Label label = sender as Label;

            bool isBlack = false;

            string[] buf = label.Name.Split(' ');
            int x = Convert.ToInt32(buf[0]);
            int y = Convert.ToInt32(buf[1]);
            int xSize = locatingElement.Length;
            int ySize = locatingElement.Width;

            for (int i = x; i < x + xSize; i++)
                for (int j = y; j < y + ySize; j++)
                    if (i >= model1.Length || j >= model1[0].Length)
                        isBlack = true;
                    else
                        if (model1[i][j].BackColor == Color.Black)
                        isBlack = true;

            for (int i = x; i < x + xSize; i++)
                for (int j = y; j < y + ySize; j++)
                    if (i < model1.Length && j < model1[0].Length)
                    {
                        if (isBlack)
                        {
                            if (model1[i][j].BackColor != Color.Black)
                            {
                                model1[i][j].BackColor = Color.Red;
                                model1[i][j].ForeColor = Color.Red;
                            }
                        }
                        else
                        {
                            model1[i][j].BackColor = Color.White;
                            model1[i][j].ForeColor = Color.White;
                        }
                    }
        }

        private void LabelMouseLeave(object sender, EventArgs e)
        {
            Label label = sender as Label;

            string[] buf = label.Name.Split(' ');
            int x = Convert.ToInt32(buf[0]);
            int y = Convert.ToInt32(buf[1]);

            int xSize = locatingElement.Length;
            int ySize = locatingElement.Width;

            for (int i = x; i < x + xSize; i++)
                for (int j = y; j < y + ySize; j++)
                    if (i < model1.Length && j < model1[0].Length)
                        if (model1[i][j].BackColor != Color.Black)
                        {
                            model1[i][j].BackColor = Color.Green;
                            model1[i][j].ForeColor = Color.Green;
                        }
        }
                     
        private void LabelMouseClick(object sender, EventArgs e)
        {
            Label label = sender as Label;
            bool isBlack = false;

            string[] buf = label.Name.Split(' ');
            int x = Convert.ToInt32(buf[0]);
            int y = Convert.ToInt32(buf[1]);

            int xSize = locatingElement.Length;
            int ySize = locatingElement.Width;

            for (int i = x; i < x + xSize; i++)
                for (int j = y; j < y + ySize; j++)
                    if (i >= model1.Length || j >= model1[0].Length)
                        isBlack = true;
                    else
                        if (model1[i][j].BackColor == Color.Black)
                        isBlack = true;

            if (!isBlack)
            {
                for (int i = x; i < x + xSize; i++)
                    for (int j = y; j < y + ySize; j++)
                        if (i < model1.Length && j < model1[0].Length)
                        {
                            model1[i][j].BackColor = Color.Black;
                            model1[i][j].ForeColor = Color.White;
                            model1[i][j].Text = (elementIndex + 1).ToString();
                        }

                for (int i = 0; i < model1.Length; i++)
                    for (int j = 0; j < model1[0].Length; j++)

                    {
                        model1[i][j].MouseClick -= new MouseEventHandler(LabelMouseClick);
                        model1[i][j].MouseEnter -= new EventHandler(LabelMouseEnter);
                        model1[i][j].MouseLeave -= new EventHandler(LabelMouseLeave);
                    }

                locatingElement.XCoordinate = y;
                locatingElement.YCoordinate = x;
                project.elementList[elementIndex].XCoordinate = y + 1;
                project.elementList[elementIndex].YCoordinate = x + 1;

                Element(sender, e);

            }
        }

        private void PutElement()
        {
            elementForm = new ElementForm(ref project);
            elementForm.ShowDialog();

            if (elementForm.Exit == 1)
            {
                elementIndex = elementForm.elementIndex;
                PreparePlace();
            }
        }

        private void Element(object sender, EventArgs e)
        {
            PutElement();
        }

        private void PreparePlace()
        {
            //  Дали события
            for (int i = 0; i < model1.Length; i++)
                for (int j = 0; j < model1[0].Length; j++)
                {
                    model1[i][j].MouseClick += new MouseEventHandler(LabelMouseClick);
                    model1[i][j].MouseEnter += new EventHandler(LabelMouseEnter);
                    model1[i][j].MouseLeave += new EventHandler(LabelMouseLeave);
                }

            //  Сделали место того элемента зелёным
            int x = project.elementList[elementIndex].XCoordinate - 1;
            int y = project.elementList[elementIndex].YCoordinate - 1;
            int xSize = project.elementList[elementIndex].Length;
            int ySize = project.elementList[elementIndex].Width;

            for (int i = 0; i < model1.Length; i++)
                for (int j = 0; j < model1[0].Length; j++)
                    if (model1[i][j].Text == (elementIndex + 1).ToString())
                    {
                        model1[i][j].BackColor = Color.Green;
                        model1[i][j].ForeColor = Color.Green;
                        model1[i][j].Text = "0";
                    }
            
            locatingElement = new Element();
            locatingElement.XCoordinate = y;
            locatingElement.YCoordinate = x;
            locatingElement.Length = ySize;
            locatingElement.Width = xSize;
            elementForm.Exit = 0;
        }

        private void UpdateButton(object sender, EventArgs e)
        {
            DrawModel1();
            toolStripStatusLabel1.Text = "Обновление";
        }

        private void StartModellingButton(object sender, EventArgs e)
        {
            StartModelling();
        }

        private void SaveButton(object sender, EventArgs e)
        {
            Save();
        }

        private void Save()
        {
            if (projectIsCreated)
                DBClass.CreateProject(ref project);
            DBClass.SaveAnalysisParameters(project);
            DBClass.SavePCB(project);
            DBClass.SaveElements(project);

            toolStripStatusLabel1.Text = "Сохранено";
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void MainMenuForm_FormClosing(Object sender, FormClosingEventArgs e)
        {
            if (yes)
            {
                SaveForm saveForm = new SaveForm();
                saveForm.ShowDialog();

                switch (saveForm.yes)
                {
                    case 0:
                        e.Cancel = true;
                        break;
                    case 1:
                        if (yes)
                            Save();
                        this.FormClosing -= new FormClosingEventHandler(this.MainMenuForm_FormClosing);
                        Close();
                        break;
                    case 2:
                        this.FormClosing -= new FormClosingEventHandler(this.MainMenuForm_FormClosing);
                        Close();
                        break;
                }
            }
            
        }

        private void сеткаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            modelForm = new ModelForm(ref grid);
            modelForm.ShowDialog();
        }
    }
}

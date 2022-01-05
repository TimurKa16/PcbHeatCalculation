using System;
using System.Windows.Forms;

namespace Диплом
{
    public partial class AnalysisParametersForm : Form
    {
        Project project;

        public AnalysisParametersForm(ref Project project)
        {
            InitializeComponent();
            this.project = project;
        }

        private void AnalysisParameters_Load(object sender, EventArgs e)
        {
            textBox1.Text = project.totalTime.ToString();
            textBox3.Text = project.firstOverheat.ToString();
            //comboBox1.Items.Add("1 см");
            //comboBox1.Items.Add("5 мм");
            //comboBox1.Items.Add("2.5 мм");
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private int Save()
        {
            try
            {
                project.totalTime = Convert.ToInt32(textBox1.Text);
                project.firstOverheat = Convert.ToInt32(textBox3.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка при вводе");
                return 1;
            }
            return 0;
        }

        private void AnalysisParametersForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool hasChanged = false;
            bool isEmpty = false;
            for (int i = 0; i < project.elementList.Count; i++)
                if (textBox1.Text == "" ||
                    textBox3.Text == "")
                {
                    isEmpty = true;
                    e.Cancel = true;
                    MessageBox.Show("Заполните данные!");
                }

            if (!isEmpty)
            {
                for (int i = 0; i < project.elementList.Count; i++)
                {
                    if (textBox1.Text != project.totalTime.ToString() ||
                        textBox3.Text != project.firstOverheat.ToString())
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
                                this.FormClosing -= new System.Windows.Forms.FormClosingEventHandler(this.AnalysisParametersForm_FormClosing);
                            break;
                        case 2:
                            this.FormClosing -= new System.Windows.Forms.FormClosingEventHandler(this.AnalysisParametersForm_FormClosing);
                            break;
                    }
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

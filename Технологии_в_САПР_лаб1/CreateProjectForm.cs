using System;
using System.Windows.Forms;

namespace Диплом
{
    public partial class CreateProjectForm : Form
    {
        public string projectName = null;
        public bool yes = false;

        public CreateProjectForm()
        {
            InitializeComponent();            
        }

        private void CreateProjectForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            projectName = textBox1.Text;
            if (projectName == "")
                MessageBox.Show("Ошибка! Поле \"Имя проекта\" не заполнено!");
            if (projectName != "")
            {
                yes = true;
                MessageBox.Show("Проект создан успешно!");
                Close();
            }            
        }        
    }
}

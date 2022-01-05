using System;
using System.Windows.Forms;

namespace Диплом
{
    public partial class SaveForm : Form
    {
        public int yes = 0;     // 0 - cancel, 1= yes, 2 = no

        public SaveForm()
        {
            InitializeComponent();
        }

        private void SaveForm_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            yes = 1;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            yes = 2;
            Close();
        }
        

        private void button3_Click(object sender, EventArgs e)
        {

            yes = 0;
            Close();
        }

        private void SaveForm_Load_1(object sender, EventArgs e)
        {

        }
    }    
}

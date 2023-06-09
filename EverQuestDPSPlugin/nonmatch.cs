using System;
using System.Windows.Forms;

namespace EverQuestDPSPlugin
{
    public partial class nonmatch : Form
    {
        public nonmatch()
        {
            InitializeComponent();
        }

        public void addLogLineToForm(String logline)
        {
            this.nonMatchList.Items.Add(logline);
        }
    }
}

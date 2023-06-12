using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace EverQuestDPSPlugin
{

    public partial class nonmatch : Form
    {
        List<String> logLine = new List<string>();

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

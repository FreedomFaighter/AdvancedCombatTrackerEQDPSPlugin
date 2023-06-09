using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Advanced_Combat_Tracker;

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

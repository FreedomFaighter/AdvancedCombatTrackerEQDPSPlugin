using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace EverQuestDPSPlugin
{

    public partial class nonmatch : Form
    {
        List<String> logLine = new List<string>();
        EverQuestDPSPlugin parentPlugin;
        public nonmatch(EverQuestDPSPlugin eqdpsp)
        {
            InitializeComponent();
            parentPlugin = eqdpsp;

            FormClosed += new FormClosedEventHandler(new Action<object, FormClosedEventArgs>((o, f) =>
            {
                parentPlugin.ChangeNonmatchFormCheckBox(false);
            }));
        }



        public void addLogLineToForm(String logline)
        {
            if (nonMatchList.InvokeRequired)
                nonMatchList.Invoke(new Action(() =>
                {
                    nonMatchList.Items.Add(logline);
                }
            ));
            else
                nonMatchList.Items.Add(logline);
        }
    }
}

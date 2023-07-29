using System;
using System.Windows.Forms;
using System.Collections.Generic;
using EverQuestDPSPlugin.Interfaces;

namespace EverQuestDPSPlugin
{

    public partial class nonmatch : Form
    {
        List<String> logLine = new List<string>();
        IEverQuestDPSPlugin pluginControl;
        public nonmatch(EverQuestDPSPlugin eqdpsp)
        {
            InitializeComponent();
            pluginControl = eqdpsp;

            FormClosed += new FormClosedEventHandler(new Action<object, FormClosedEventArgs>((o, f) =>
            {
                pluginControl.ChangeNonmatchFormCheckBox(false);
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

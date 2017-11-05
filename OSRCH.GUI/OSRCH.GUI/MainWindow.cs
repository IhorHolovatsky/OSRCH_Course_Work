using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OSRCH.BLL;
using OSRCH.BLL.Models;

namespace OSRCH.GUI
{
    public partial class MainWindow : Form
    {
        private readonly SynchronizationContext _synchronizationContext;

        public MainWindow()
        {
            InitializeComponent();
            _synchronizationContext = SynchronizationContext.Current;
        }

        private void importButton_Click(object sender, EventArgs e)
        {
            var openFileDialog1 = new OpenFileDialog
            {
                Filter = "Txt files|*.txt",
                Title = "Select an instruction"
            };

            if (openFileDialog1.ShowDialog() != DialogResult.OK) return;

            labelInstructionName.Text = openFileDialog1.SafeFileName;

            var sr = new
                System.IO.StreamReader(openFileDialog1.FileName);

            var instructions = new List<string>();

            while (!sr.EndOfStream)
            {
                instructions.Add(sr.ReadLine());
            }
            
            sr.Close();

            SimulatorBusinessLogic.CurrentInstructionsStrings = instructions;
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            startButton.IsAccessible = false;

            SimulatorBusinessLogic.RunSimulation(new CraneObject(),_synchronizationContext,logsTextBox,
                delegate(SimulationResponseModel model)
                {
                    MessageBox.Show(this, model.Message, (model.IsSuccess) ? "Simulation end" : "Error happened",
                        MessageBoxButtons.OK);
                    startButton.IsAccessible = true;
                });

            

        }
    }
}

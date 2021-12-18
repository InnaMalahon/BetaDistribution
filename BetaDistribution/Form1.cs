using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace BetaDistribution
{
    public partial class Form1 : Form
    {
        private double n, a, b;
        private bool paused1, paused2, paused3 = false;
        private bool canceled;
        
        public Form1()
        {
            InitializeComponent();
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker2.WorkerReportsProgress = true;
            backgroundWorker2.WorkerSupportsCancellation = true;
            backgroundWorker3.WorkerReportsProgress = true;
            backgroundWorker3.WorkerSupportsCancellation = true;
            canceled = false;
            button2.Enabled = false;
        }

        private void проПрограмуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (paused1 || paused2 || paused3)
            {
                paused1 = false;
                paused2 = false;
                paused3 = false;
                canceled = true;
            }
            if (canceled)
            {               
                progressBar1.Value = 0;
                progressBar2.Value = 0;
                progressBar3.Value = 0;
                canceled = false;
            }
            if (backgroundWorker1.IsBusy != true && backgroundWorker2.IsBusy != true && backgroundWorker3.IsBusy != true)
            {               
                //GUIStateChange(false);
                lock (this)
                {                   
                    //prepareStartData();
                    this.generator.PlusExperiment();
                    this.button2.Enabled = false;
                    generator.generateAnalytical();
                    generator.MakeSomeMetropolysSteps(1000);
                    backgroundWorker1.RunWorkerAsync();
                    backgroundWorker2.RunWorkerAsync();
                    backgroundWorker3.RunWorkerAsync();
                }
            }
            else if (backgroundWorker1.WorkerSupportsCancellation == true)
            {
                //GUIStateChange(true);
                canceled = true;
                lock (this)
                {
                    backgroundWorker1.CancelAsync();
                    backgroundWorker2.CancelAsync();
                    backgroundWorker3.CancelAsync();
                }
            }
        }
        
        private void prepareTables()
        {
            var column1 = new DataGridViewColumn();
            column1.HeaderText = "Інтервал  ";
            column1.ReadOnly = true;
            column1.Frozen = true;
            column1.CellTemplate = new DataGridViewTextBoxCell();

            var column2 = new DataGridViewColumn();
            column2.HeaderText = "Зворотньої функції   ";
            column2.CellTemplate = new DataGridViewTextBoxCell();

            dataGridView1.Columns.Add(column1);
            dataGridView1.Columns.Add(column2);

            var column4 = new DataGridViewColumn();
            column4.HeaderText = "Неймана    ";
            column4.CellTemplate = new DataGridViewTextBoxCell();

            dataGridView1.Columns.Add(column4);

            var column6 = new DataGridViewColumn();
            column6.HeaderText = "Метрополісу";
            column6.CellTemplate = new DataGridViewTextBoxCell();

            var column9 = new DataGridViewColumn();
            column9.HeaderText = "Аналітика    ";
            column9.CellTemplate = new DataGridViewTextBoxCell();

            dataGridView1.Columns.Add(column6);
            dataGridView1.Columns.Add(column9);
        }
    }
}
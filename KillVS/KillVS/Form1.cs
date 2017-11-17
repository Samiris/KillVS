using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace KillVS
{
    public partial class Form1 : Form
    {
        private List<Process> processos = null;

        public Form1()
        {
            InitializeComponent();
            lbxItens.DoubleClick += new EventHandler(lbxItens_DoubleClick);
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsIconic(IntPtr hWnd);
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                processos = Process.GetProcesses().Where(p => p.MainWindowTitle.Contains("Microsoft Visual Studio")).ToList();

                if (processos != null)
                {
                    if (processos.Count > 1)
                    {
                        foreach (var processo in processos)
                        {
                            //IntPtr wHnd = processo.MainWindowHandle;
                            //IsIconic(wHnd);
                            lbxItens.Items.Add(processo.Id + "_" + processo.MainWindowTitle);
                        }
                    }
                    else
                    {
                        DialogResult result = MessageBox.Show("Certeza?", "Confirmar!", MessageBoxButtons.YesNo);

                        if (result == DialogResult.Yes)
                        {
                            processos[0].Kill();
                        }

                        this.Close();
                    }
                }
            }
            catch (Exception)
            {
                this.Close();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnTodos_Click(object sender, EventArgs e)
        {
            foreach (var processo in processos)
            {
                processo.Kill();
            }

            this.Close();
        }

        private void lbxItens_DoubleClick(object sender, EventArgs e)
        {
            if (lbxItens.SelectedItem != null)
            {
                int idProcesso = Convert.ToInt32(lbxItens.SelectedItem.ToString().Split('_')[0]);
                var processo = processos.Where(x => x.Id == idProcesso).SingleOrDefault();

                if (processo != null)
                {
                    processo.Kill();
                    lbxItens.Items.Remove(lbxItens.SelectedItem);
                }
            }
        }
    }
}

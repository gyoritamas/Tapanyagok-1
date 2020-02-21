using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tapanyagok.Models;
using Tapanyagok.Presenters;
using Tapanyagok.ViewInterfaces;

namespace Tapanyagok.Views
{
    public partial class TablazatForm : Form, ITablazatView
    {
        private TablazatPresenter presenter;
        // Oldaltördelés
        private int pageCount;
        private int sortIndex;

        public TablazatForm()
        {
            InitializeComponent();
            presenter = new TablazatPresenter(this);
            Init();
        }

        public void Init()
        {
            pageNumber = 1;
            itemsPerPage = 25;
            sortBy = "Id";
            sortIndex = 0;
            ascending = true;
        }

        public BindingList<tapanyag> bindingList
        {
            get => (BindingList<tapanyag>)dataGridView1.DataSource;
            set => dataGridView1.DataSource = value;
        }
        public int pageNumber { get; set; }
        public int itemsPerPage { get; set; }
        public string search { get => keresestoolStripTextBox.Text; }
        public string sortBy { get; set; }
        public bool ascending { get; set; }
        public int totalItems
        {
            set
            {
                pageCount = (value - 1) / itemsPerPage + 1;
                label1.Text = pageNumber.ToString() + "/" + pageCount.ToString();
            }
        }

        private void TablazatPresenter_Load(object sender, EventArgs e)
        {
            presenter.LoadData();
        }

        private void mentestoolStripButton_Click(object sender, EventArgs e)
        {
            presenter.Save();
        }

        private void KeresestoolStripButton_Click(object sender, EventArgs e)
        {
            presenter.LoadData();
        }

        private void FirstButton_Click(object sender, EventArgs e)
        {
            pageNumber = 1;
            presenter.LoadData();
        }

        private void PrevButton_Click(object sender, EventArgs e)
        {
            if (pageNumber > 1)
            {
                pageNumber--;
                presenter.LoadData();
            }
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            if (pageNumber < pageCount)
            {
                pageNumber++;
                presenter.LoadData();
            }
        }

        private void LastButton_Click(object sender, EventArgs e)
        {
            pageNumber = pageCount;
            presenter.LoadData();
        }

        private void NewDGRow()
        {
            //using (var szerkForm = new JarmuKategoriaSzerkForm())
            //{
            //    DialogResult dr = szerkForm.ShowDialog(this);
            //    if (dr == DialogResult.OK)
            //    {
            //        presenter.Add(szerkForm.jarmukategoria);
            //        szerkForm.Close();
            //    }
            //}
        }
        private void EditDGRow(int index)
        {
            //var jk = (jarmukategoria)dataGridView1.Rows[index].DataBoundItem;

            //if (jk != null)
            //{
            //    using (var modForm = new JarmuKategoriaSzerkForm())
            //    {
            //        modForm.jarmukategoria = jk;
            //        DialogResult dr = modForm.ShowDialog(this);
            //        if (dr == DialogResult.OK)
            //        {
            //            presenter.Modify(modForm.jarmukategoria);
            //            modForm.Close();
            //        }
            //    }
            //}
        }
        private void DelDGRow()
        {
            while (dataGridView1.SelectedRows.Count > 0)
            {
                presenter.Remove(dataGridView1.SelectedRows[0].Index);
            }
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (sortIndex == e.ColumnIndex)
            {
                ascending = !ascending;
            }
            switch (e.ColumnIndex)
            {
                case 1:
                    sortBy = "nev";
                    break;
                case 2:
                    sortBy = "energia";
                    break;
                case 3:
                    sortBy = "feherje";
                    break;
                case 4:
                    sortBy = "nev";
                    break;
                case 5:
                    sortBy = "zsir";
                    break;
                case 6:
                    sortBy = "szenhidrat";
                    break;
                default:
                    sortBy = "id";
                    break;
            }

            sortIndex = e.ColumnIndex;

            presenter.LoadData();
        }

        private void UjtoolStripButton_Click(object sender, EventArgs e)
        {
            NewDGRow();
        }

        private void TorlestoolStripButton_Click(object sender, EventArgs e)
        {
            DelDGRow();
        }

        private void UjToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewDGRow();
        }

        private void SzerkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows != null)
            {
                var sorIndex = dataGridView1.SelectedCells[0].RowIndex;
                dataGridView1.ClearSelection();
                dataGridView1.Rows[sorIndex].Selected = true;
            }

            EditDGRow(dataGridView1.SelectedRows[0].Index);
        }

        private void TorlesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DelDGRow();
        }
    }
}

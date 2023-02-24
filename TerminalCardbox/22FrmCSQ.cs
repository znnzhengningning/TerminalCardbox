using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SHBSS
{
    public partial class FrmCSQ : Form
    {
        Global gb = Global.GetInstance();
        DataTable dtArea = new DataTable();
        DataTable dtTown = new DataTable();
        DataTable dtVillage = new DataTable();
        public FrmCSQ()
        {
            InitializeComponent();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }
        private void FrmCSQ_Load(object sender, EventArgs e)
        {
            LoadTableData();
            checkedListBox3.DataSource = dtArea;
            checkedListBox3.ValueMember = "AreaCode";
            checkedListBox3.DisplayMember = "AreaName";
            checkedListBox1.DataSource = dtTown;
            checkedListBox1.ValueMember = "TownCode";
            checkedListBox1.DisplayMember = "TownName";
            checkedListBox2.DataSource = dtVillage;
            checkedListBox2.ValueMember = "VillageNo";
            checkedListBox2.DisplayMember = "VillageName";
        }

        void LoadTableData()
        {
            SSCSQL.getAreaTable(gb.CityName, ref dtArea);
            SSCSQL.getTownTable(dtArea.Rows[0][0].ToString(), ref dtTown);
            SSCSQL.getSqbhTable(dtTown.Rows[0][0].ToString(), ref dtVillage);
        }
        private void btn_ok_Click(object sender, EventArgs e)
        {
            FrmApplyCheck.b_select_sqbh = true;
            FrmApplyCheck.strSqbh = checkedListBox2.SelectedValue.ToString();
            FrmApplyCheck.strSqcmc = checkedListBox2.GetItemText(checkedListBox2.Items[checkedListBox2.SelectedIndex]);
            this.Hide();
        }

        private void checkedListBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox3.Items.Count; i++) {
                checkedListBox3.SetItemChecked(i, false);
            }
            if (checkedListBox3.CheckedItems == null) {
                checkedListBox3.SetItemChecked(checkedListBox3.SelectedIndex, false);
            }
            else {
                if (checkedListBox3.SelectedIndex != -1)
                {
                    checkedListBox3.SetItemChecked(checkedListBox3.SelectedIndex, true);
                    string AreaCode = checkedListBox3.SelectedValue.ToString();
                    if (AreaCode.IndexOf("System") < 0)
                    {
                        if (SSCSQL.getTownTable(AreaCode, ref dtTown))
                        {
                            SSCSQL.getSqbhTable(dtTown.Rows[0][0].ToString(), ref dtVillage);
                        }
                    }
                }
            }
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++) {
                checkedListBox1.SetItemChecked(i, false);
            }
            if (checkedListBox1.CheckedItems == null) {
                checkedListBox1.SetItemChecked(checkedListBox1.SelectedIndex, false);
            }
            else {
                if (checkedListBox1.SelectedIndex != -1)
                {
                    checkedListBox1.SetItemChecked(checkedListBox1.SelectedIndex, true);
                    string TownCode = checkedListBox1.SelectedValue.ToString();
                    if (TownCode.IndexOf("System") < 0)
                    {
                        SSCSQL.getSqbhTable(TownCode, ref dtVillage);
                    }
                }
            }
        }

        private void checkedListBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox2.Items.Count; i++) {
                checkedListBox2.SetItemChecked(i, false);
            }
            if (checkedListBox2.CheckedItems == null) {
                checkedListBox2.SetItemChecked(checkedListBox2.SelectedIndex, false);
            }
            else {
                if(checkedListBox2.SelectedIndex != -1)
                    checkedListBox2.SetItemChecked(checkedListBox2.SelectedIndex, true);
            }
        }
    }
}

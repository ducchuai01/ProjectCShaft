using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectCShaft
{
    public partial class frmMain : MetroFramework.Forms.MetroForm
    {
        bool edit = true;

        ProjectCShaftDataContext db = new ProjectCShaftDataContext();
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
           
            ShowAllMenu();
            ShowDetail();
        }

        private void ShowAllMenu()
        {
            var menus = from m in db.Menus select new { m.idMenu, m.nameMenu,m.unitMenu,m.priceMenu,m.descriptionMenu,m.status };
            dgMenu.DataSource = menus;
        }

        private void ShowDetail()
        {
            if (dgMenu.CurrentRow != null)
            {
                var row = dgMenu.CurrentRow;
                //hiển thị lên form
                txtIdMenu.Text = row.Cells[0].Value.ToString();
                txtNameMenu.Text = row.Cells[1].Value.ToString();
                cboUnit.Text = row.Cells[2].Value.ToString();
                txtPrice.Value = decimal.Parse(row.Cells[3].Value.ToString());
                txtDescriptionMenu.Text = row.Cells[4].Value.ToString();
                chkStatus.Checked = bool.Parse(row.Cells[5].Value.ToString());
                edit = true;
            }
        }

      


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void metroSetSetTabPage2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            edit = false;
            txtIdMenu.Text = txtNameMenu.Text = cboUnit.Text = txtDescriptionMenu.Text = "";
            txtIdMenu.ReadOnly = false;
            txtPrice.Value = 0;
            txtIdMenu.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!edit) //insert
            {
                //tạo đối tượng product
                var m = new Menu();
                m.nameMenu = txtNameMenu.Text;
                m.unitMenu = cboUnit.Text;
                m.priceMenu = (float)txtPrice.Value;
                m.descriptionMenu = txtDescriptionMenu.Text;
                m.status = chkStatus.Checked;
                //insert
                db.Menus.InsertOnSubmit(m);
                //ghi
                db.SubmitChanges();
                MessageBox.Show("Thêm mới thành công","Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ShowAllMenu();
                ShowDetail();
            }
            else //update
            {
                //tìm bản ghi cần sửa
                var m = db.Menus.FirstOrDefault(x => x.idMenu == int.Parse(txtIdMenu.Text));
                m.nameMenu = txtNameMenu.Text;
                m.unitMenu = cboUnit.Text;
                m.priceMenu = (float)txtPrice.Value;
                m.descriptionMenu = txtDescriptionMenu.Text;
                m.status = chkStatus.Checked;
                //ghi
                db.SubmitChanges();
                MessageBox.Show("Sửa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ShowAllMenu();
                ShowDetail();
            }
        }

        private void dgMenu_Click(object sender, EventArgs e)
        {
            ShowDetail();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
    
                if (MessageBox.Show("Bạn có muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    //tìm bản ghi cần xóa
                    var p = db.Menus.FirstOrDefault(x => x.idMenu == int.Parse(txtIdMenu.Text));
                    //xóa
                    db.Menus.DeleteOnSubmit(p);
                    //ghi
                    db.SubmitChanges();
                    ShowAllMenu();
                    ShowDetail();
                }
            }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            tabControlMain.SelectTab(tpCLB);
        }

        private void metroSetTabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void btnSkip_Click(object sender, EventArgs e)
        {
            ShowDetail();
        }

        private void metroCheckBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}

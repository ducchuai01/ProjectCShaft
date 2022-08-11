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
            CountTable();
            ShowAllMenu();
            ShowDetailMenu();
            ShowAllTable(); 
            ShowDetailTable();
            ShowMenuInTable();
            ShowMenu();
            SumPrice();
            ShowDetalOrderMenu();
            ShowAllOrderMenu();
            this.dgOrderMenu.DataSource = null;
        }

        private void ShowAllOrderMenu()
        {
            var order_menus = db.OrderMenus;
            dgOrderMenu.DataSource = order_menus.ToList();
        }

        private void ShowDetalOrderMenu()
        {
            if (dgOrderMenu.CurrentRow != null)
            {
                var row = dgOrderMenu.CurrentRow;
                //hiển thị lên form
                txtNMenu.Text = row.Cells[0].Value.ToString();
                txtDV.Text = row.Cells[1].Value.ToString();
                txtPMenu.Text = row.Cells[2].Value.ToString();
                nbQuantity.Value = decimal.Parse(row.Cells[3].Value.ToString());
                txtSumPrice.Text = row.Cells[4].Value.ToString();
                edit = true;
            }
        }

        private void SumPrice()
        {
            if(!string.IsNullOrEmpty(txtPMenu.Text))
            {
                decimal tien = decimal.Parse(txtPMenu.Text);
                decimal sumPrice = tien * nbQuantity.Value;
                txtSumPrice.Text = sumPrice.ToString();
            }
            
        }

        private void ShowMenuInTable()
        {
            var menus = from m in db.Menus where m.status == true select new { m.idMenu, m.nameMenu, m.unitMenu, m.priceMenu, m.descriptionMenu };
            dgMenuTable.DataSource = menus.ToList();
        }

        private void CountTable()
        {
            int i = 0;
            int x = 0;
            int y = 5;
            int z = 1;

            foreach (var item in db.Table_Bidas.OrderByDescending(m => m.idTable))
            {
                if (i >= 13)
                {
                    i = 0;
                    x = 0;
                    y = 90 * z;
                    z++;
                }
                if (i>0)
                {
                    x = i * 30 + 80*i;
                }
                i++;
                Button button = new Button() { Width = 80, Height = 80,Location = new Point(x,y) ,Text = item.nameTable , BackColor = Color.Green,ForeColor = Color.White,Tag = item.idTable.ToString() };
                button.Click += Button_Click;
                if (item.statusTable == false)
                {
                    button.BackColor = Color.Red;
                }
                pnAllTable.Controls.Add(button);
            }
        }

        int numberTable;
        private void Button_Click(object sender, EventArgs e)
        {
            btnNameTable.Text = ((Button)sender).Text;
            numberTable = int.Parse(((Button)sender).Tag.ToString());
            var order_table = db.Order_Tables.FirstOrDefault(i => i.idTable == numberTable && i.status == false);
            if (order_table != null)
            {
                var order_menus = from om in db.OrderMenus where om.idTable == numberTable select new { om.nameMenuOrder, om.unitMenuOrder, om.priceMenuOrder, om.quantity, om.sumPrice };
                dgOrderMenu.DataSource = order_menus.ToList();

                var datetime = order_table.timeStart.ToString();
                lbTimeStart.Text = datetime;
            }
            else
            {
                lbTimeStart.Text = "";
                this.dgOrderMenu.DataSource = null;
            }
        }

        private void ShowAllTable()
        {
            var tables = db.Table_Bidas.Select(t => new {t.idTable,t.nameTable,t.typeTable,t.priceTable,StatusTable = t.statusTable == true ?"Đang sử dụng":"Chưa sử dụng",t.description });
            dgTable.DataSource = tables.ToList();
        }

        private void ShowDetailTable()
        {
            if (dgTable.CurrentRow != null)
            {
                var row = dgTable.CurrentRow;
                //hiển thị lên form
                txtIdTable.Text = row.Cells[0].Value.ToString();
                txtNameTable.Text = row.Cells[1].Value.ToString();
                txtTypeTable.Text = row.Cells[2].Value.ToString();
                txtPriceTable.Value = decimal.Parse(row.Cells[3].Value.ToString());
                if (row.Cells[4].Value.ToString().Equals("Đang sử dụng"))
                {
                    chkStatusTable.Checked = true;

                }
                else
                {
                    chkStatusTable.Checked = false;
                }
                txtDescriptionTable.Text = row.Cells[5].Value.ToString();
                edit = true;
            }
        }
        #region Menu
        private void ShowAllMenu()
        {
            var menus = db.Menus.Select(m => new {m.idMenu,m.nameMenu,m.unitMenu,m.priceMenu,m.descriptionMenu,Status = m.status == true ? "Còn hàng":"Hết hàng" });
            dgMenu.DataSource = menus.ToList();
        }

        private void ShowDetailMenu()
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
                if (row.Cells[5].Value.ToString().Equals("Còn hàng"))
                {
                    chkStatus.Checked = true;

                }
                else
                {
                    chkStatus.Checked = false;
                }
                edit = true;
            }
        }
        private void ShowMenu()
        {
            if (dgMenuTable.CurrentRow != null)
            {
                var row = dgMenuTable.CurrentRow;
                //hiển thị lên form
                txtNMenu.Text = row.Cells[1].Value.ToString();
                txtDV.Text = row.Cells[2].Value.ToString();
                txtPMenu.Text = row.Cells[3].Value.ToString();
                edit = true;
                SumPrice();
            }
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
            if (string.IsNullOrEmpty(txtIdMenu.Text)) //insert
            {
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
                MessageBox.Show("Thêm mới thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ShowAllMenu();
                ShowDetailMenu();
                ShowMenuInTable();
                ShowMenu();
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
                ShowDetailMenu();
                ShowMenuInTable();
                ShowMenu();
            }
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
                ShowDetailMenu();
                ShowMenuInTable();
                ShowMenu();
            }
        }

        private void btnSkip_Click(object sender, EventArgs e)
        {
            ShowDetailMenu();
        }
        #endregion

        private void dgMenu_Click(object sender, EventArgs e)
        {
            ShowDetailMenu();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            tabControlMain.SelectTab(tpCLB);
        }

        private void dgTable_Click(object sender, EventArgs e)
        {
            ShowDetailTable();
        }

        private void btnAddTable_Click(object sender, EventArgs e)
        {
            edit = false;
            txtIdTable.Text = txtNameTable.Text = txtTypeTable.Text = txtDescriptionTable.Text = "";
            txtPriceTable.Value = 0;
            txtIdTable.Focus();
        }

        private void btnSaveTable_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIdTable.Text)) //insert
            {
                //tạo đối tượng product
                var t = new Table_Bida();
                txtIdTable.Enabled = false;
                t.nameTable = txtNameTable.Text;
                t.typeTable = txtTypeTable.Text;
                t.priceTable = (float)txtPriceTable.Value;
                t.statusTable = chkStatusTable.Checked;
                t.description = txtDescriptionTable.Text;
                //insert
                db.Table_Bidas.InsertOnSubmit(t);
                //ghi
                db.SubmitChanges();
                MessageBox.Show("Thêm mới thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ShowAllTable();
                ShowDetailTable();
                pnAllTable.Controls.Clear();
                CountTable();
            }
            else //update
            {
                //tìm bản ghi cần sửa
                    var t = db.Table_Bidas.FirstOrDefault(x => x.idTable == int.Parse(txtIdTable.Text));
                    t.nameTable = txtNameTable.Text;
                    t.typeTable = txtTypeTable.Text;
                    t.priceTable = (float)txtPriceTable.Value;
                    t.statusTable = chkStatusTable.Checked;
                    t.description = txtDescriptionTable.Text;
                    //ghi
                    db.SubmitChanges();
                    MessageBox.Show("Cập nhật thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ShowAllTable();
                    ShowDetailTable();
                    pnAllTable.Controls.Clear();
                    CountTable();
            }
        }

        private void dgMenuTable_Click(object sender, EventArgs e)
        {
            ShowMenu();
        }

        private void nbQuantity_ValueChanged(object sender, EventArgs e)
        {
            SumPrice();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                var o_t = new Order_Table();
                o_t.idTable = numberTable;
                o_t.timeStart = DateTime.Now;
                o_t.status = false;
                var t = db.Table_Bidas.FirstOrDefault(x => x.idTable == numberTable);
                t.statusTable = true;
                ShowAllTable();
                ShowDetailTable();
                pnAllTable.Controls.Clear();
                CountTable();
                db.Order_Tables.InsertOnSubmit(o_t);
                db.SubmitChanges();
                MessageBox.Show("Bàn đã hoạt động", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lbTimeStart.Text = o_t.timeStart.ToString();
            }
            catch (Exception)
            {
                MessageBox.Show("Vui lòng chọn bàn", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            
        }

        private void btnDeleteTable_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                //tìm bản ghi cần xóa
                var p = db.Table_Bidas.FirstOrDefault(x => x.idTable == int.Parse(txtIdTable.Text));
                //xóa
                db.Table_Bidas.DeleteOnSubmit(p);
                //ghi
                db.SubmitChanges();
                ShowAllTable();
                ShowDetailTable();
                pnAllTable.Controls.Clear();
                CountTable();
            }
        }

        private void btnSkip_Table_Click(object sender, EventArgs e)
        {
            ShowDetailTable();
        }

        private void btnCancel_Table_Click(object sender, EventArgs e)
        {
            tabControlMain.SelectTab(tpCLB);
        }

        private List<OrderMenu> GetMenuTableById(int id)
        {
            return db.OrderMenus.Where(i => i.idTable == id).ToList();
        }
        private void btnAddToTable_Click(object sender, EventArgs e)
        {
            try
            {
                var getOrder = db.Order_Tables.ToList().LastOrDefault(x => x.idTable == numberTable && x.status == false);
                if (getOrder != null)
                {
                    int item = (int)dgMenuTable.SelectedRows[0].Cells[0].Value;
                    var om = db.OrderMenus.FirstOrDefault(x => x.idTable == numberTable && x.idMenuOrder == item);
                    var getItem = db.Menus.FirstOrDefault(x => x.idMenu == item);
                    txtIdTable.Enabled = true;
                    if (om == null)
                    {
                        om = new OrderMenu();
                        om.idMenuOrder = item;
                        om.idTable = numberTable;
                        om.idOrder = getOrder.idOrder;
                        om.nameMenuOrder = txtNMenu.Text;
                        om.unitMenuOrder = txtDV.Text;
                        om.priceMenuOrder = double.Parse(txtPMenu.Text);
                        om.quantity = (int)nbQuantity.Value;
                        om.sumPrice = double.Parse(txtSumPrice.Text);
                        //insert
                        db.OrderMenus.InsertOnSubmit(om);
                    }
                    else
                    {
                        om.quantity += (int)nbQuantity.Value;
                        om.sumPrice = om.quantity * getItem.priceMenu;
                    }

                    db.SubmitChanges();
                    var getid = GetMenuTableById(numberTable);
                    dgOrderMenu.DataSource = getid;
                    MessageBox.Show("Thêm mới thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Bàn trống", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Vui lòng chọn bàn", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void dgOrderMenu_Click(object sender, EventArgs e)
        {
            ShowDetalOrderMenu();
        }

        private void btnUpdateOrderMenu_Click(object sender, EventArgs e)
        {
                var item = (int)dgMenuTable.SelectedRows[0].Cells[0].Value;
                var om = db.OrderMenus.FirstOrDefault(x => x.idMenuOrder == item);
                om.idMenuOrder = item;
                om.idTable = numberTable;
                om.nameMenuOrder = txtNMenu.Text;
                om.unitMenuOrder = txtDV.Text;
                om.priceMenuOrder = double.Parse(txtPMenu.Text);
                om.quantity = (int)nbQuantity.Value;
                om.sumPrice = double.Parse(txtSumPrice.Text);
                //ghi
                db.SubmitChanges();
                MessageBox.Show("Cập nhật thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ShowAllOrderMenu();
                ShowDetalOrderMenu();
        }

        private void btnDelOrderMenu_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                //tìm bản ghi cần xóa
                string item = dgOrderMenu.SelectedRows[0].Cells[0].Value.ToString();
                var om = db.OrderMenus.FirstOrDefault(x => x.idTable == numberTable && x.Menu.nameMenu == item);
                //xóa
                if (om != null)
                {
                    db.OrderMenus.DeleteOnSubmit(om);
                    db.SubmitChanges();
                }
                //ghi
                
                ShowAllOrderMenu();
                ShowDetalOrderMenu();
            }
        }

        private void btnBill_Click(object sender, EventArgs e)
        {
            var getOrder = db.Order_Tables.ToList().LastOrDefault(x => x.idTable == numberTable);
            var timeEnd = DateTime.Now;
            var getTotalTime = (int)(timeEnd - getOrder.timeStart).Value.TotalSeconds;
            int totalTimePrice = (int)(getTotalTime * getOrder.Table_Bida.priceTable / 60 / 60);
            var getOrderItem = db.OrderMenus.Where(x => x.idTable == numberTable && x.idOrder == getOrder.idOrder);
            double totalItem = 0;
            foreach (var item in getOrderItem)
            {
                totalItem += (double)item.sumPrice;
            }
            double totalBill = totalTimePrice + totalItem;
            getOrder.status = true;
            getOrder.sumPriceTable = totalBill;
            getOrder.timeEnd = timeEnd;
            getOrder.Table_Bida.statusTable = false;
            db.SubmitChanges();
            MessageBox.Show("Tổng bill " + getOrder.Table_Bida.nameTable + " là: " + totalBill + " VND", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            lbTimeStart.Text = "";
            dgOrderMenu.DataSource = null;
            pnAllTable.Controls.OfType<Button>().FirstOrDefault(x => x.Tag.ToString() == numberTable.ToString()).BackColor = Color.Red;
        }
    }
}

        


using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows.Forms;

namespace BaiTapXuLyChuoi
{
    public partial class Diem : Form
    {
        private string connectionString = @"Data Source=.;Initial Catalog=QLSinhVien_Chung;Integrated Security=True";

        public Diem()
        {
            InitializeComponent();
        }

        private bool KiemTraDuLieu(out decimal diem)
        {
            diem = 0;

            if (string.IsNullOrWhiteSpace(txtMaSV.Text))
            {
                MessageBox.Show("Ma sinh vien khong duoc de trong!");
                txtMaSV.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtMaMH.Text))
            {
                MessageBox.Show("Ma mon hoc khong duoc de trong!");
                txtMaMH.Focus();
                return false;
            }

            string giaTriDiem = txtDiem.Text.Trim();

            if (giaTriDiem == "")
            {
                MessageBox.Show("Vui long nhap diem!");
                txtDiem.Focus();
                return false;
            }

            // Chap nhan ca dau cham va dau phay
            giaTriDiem = giaTriDiem.Replace(',', '.');

            if (!decimal.TryParse(
                giaTriDiem,
                NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture,
                out diem))
            {
                MessageBox.Show("Diem phai la mot so hop le!");
                txtDiem.Focus();
                return false;
            }

            if (diem < 0 || diem > 10)
            {
                MessageBox.Show("Diem phai tu 0 den 10!");
                txtDiem.Focus();
                return false;
            }

            return true;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            decimal diem;

            if (!KiemTraDuLieu(out diem))
                return;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string sql = @"INSERT INTO dbo.Diem (MaSV, MaMH, Diem)
                                   VALUES (@MaSV, @MaMH, @Diem)";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.Add("@MaSV", SqlDbType.VarChar, 20).Value =
                            txtMaSV.Text.Trim();

                        cmd.Parameters.Add("@MaMH", SqlDbType.VarChar, 20).Value =
                            txtMaMH.Text.Trim();

                        cmd.Parameters.Add("@Diem", SqlDbType.Decimal).Value = diem;
                        cmd.Parameters["@Diem"].Precision = 4;
                        cmd.Parameters["@Diem"].Scale = 2;

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Them diem thanh cong!");
                XoaTrang();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                    MessageBox.Show("Sinh vien nay da co diem mon hoc nay!");
                else
                    MessageBox.Show("Loi SQL: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi: " + ex.Message);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaSV.Text))
            {
                MessageBox.Show("Ma sinh vien khong duoc de trong!");
                txtMaSV.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtMaMH.Text))
            {
                MessageBox.Show("Ma mon hoc khong duoc de trong!");
                txtMaMH.Focus();
                return;
            }

            DialogResult xacNhan = MessageBox.Show(
                "Ban co chac muon xoa diem nay?",
                "Xac nhan",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (xacNhan != DialogResult.Yes)
                return;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(
                    @"DELETE FROM dbo.Diem
                      WHERE MaSV = @MaSV AND MaMH = @MaMH", conn))
                {
                    cmd.Parameters.Add("@MaSV", SqlDbType.VarChar, 20).Value =
                        txtMaSV.Text.Trim();

                    cmd.Parameters.Add("@MaMH", SqlDbType.VarChar, 20).Value =
                        txtMaMH.Text.Trim();

                    conn.Open();
                    int soDong = cmd.ExecuteNonQuery();

                    if (soDong > 0)
                    {
                        MessageBox.Show("Xoa diem thanh cong!");
                        XoaTrang();
                    }
                    else
                    {
                        MessageBox.Show("Khong tim thay diem cua sinh vien va mon hoc nay!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi: " + ex.Message);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            decimal diem;

            if (!KiemTraDuLieu(out diem))
                return;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(
                    @"UPDATE dbo.Diem
                      SET Diem = @Diem
                      WHERE MaSV = @MaSV AND MaMH = @MaMH", conn))
                {
                    cmd.Parameters.Add("@MaSV", SqlDbType.VarChar, 20).Value =
                        txtMaSV.Text.Trim();

                    cmd.Parameters.Add("@MaMH", SqlDbType.VarChar, 20).Value =
                        txtMaMH.Text.Trim();

                    cmd.Parameters.Add("@Diem", SqlDbType.Decimal).Value = diem;
                    cmd.Parameters["@Diem"].Precision = 4;
                    cmd.Parameters["@Diem"].Scale = 2;

                    conn.Open();
                    int soDong = cmd.ExecuteNonQuery();

                    if (soDong > 0)
                    {
                        MessageBox.Show("Sua diem thanh cong!");
                        XoaTrang();
                    }
                    else
                    {
                        MessageBox.Show("Khong tim thay diem can sua!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi: " + ex.Message);
            }
        }

        private void XoaTrang()
        {
            txtMaSV.Clear();
            txtMaMH.Clear();
            txtDiem.Clear();
            txtMaSV.Focus();
        }
    }
}

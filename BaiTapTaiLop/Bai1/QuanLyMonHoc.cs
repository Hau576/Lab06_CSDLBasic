using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BaiTapQuanLyMonHoc
{
    public partial class QuanLyMonHoc : Form
    {
        // Chuỗi kết nối tới Database QLSinhVien (Thay đổi Data Source nếu máy bạn đặt tên server khác)
        private string connectionString = @"Data Source=.;Initial Catalog=QLSinhVien_Chung;Integrated Security=True";

        public QuanLyMonHoc()
        {
            InitializeComponent();
        }

        // Chức năng Thêm dữ liệu
        private void btnThem_Click(object sender, EventArgs e)
        {
            string maMH = txtMaMH.Text.Trim();
            string tenMH = txtTenMH.Text.Trim();

            // Kiểm tra rỗng
            if (string.IsNullOrEmpty(maMH) || string.IsNullOrEmpty(tenMH))
            {
                MessageBox.Show("Mã môn học và Tên môn học không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "INSERT INTO MonHoc (MaMH, TenMH) VALUES (@MaMH, @TenMH)";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaMH", maMH);
                        cmd.Parameters.AddWithValue("@TenMH", tenMH);
                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Thêm môn học thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtMaMH.Clear();
                            txtTenMH.Clear();
                            txtMaMH.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm: " + ex.Message, "Lỗi Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Chức năng Xóa dữ liệu
        private void btnXoa_Click(object sender, EventArgs e)
        {
            string maMH = txtMaMH.Text.Trim();

            // Kiểm tra rỗng (Chỉ cần mã môn học)
            if (string.IsNullOrEmpty(maMH))
            {
                MessageBox.Show("Mã môn học không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult dr = MessageBox.Show($"Bạn có chắc chắn muốn xóa môn học có mã {maMH} không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No) return;

            string query = "DELETE FROM MonHoc WHERE MaMH = @MaMH";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaMH", maMH);
                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Xóa môn học thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtMaMH.Clear();
                            txtTenMH.Clear();
                            txtMaMH.Focus();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy mã môn học này để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Lỗi Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Chức năng Sửa dữ liệu
        private void btnSua_Click(object sender, EventArgs e)
        {
            string maMH = txtMaMH.Text.Trim();
            string tenMH = txtTenMH.Text.Trim();

            // Kiểm tra rỗng
            if (string.IsNullOrEmpty(maMH) || string.IsNullOrEmpty(tenMH))
            {
                MessageBox.Show("Mã môn học và Tên môn học không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "UPDATE MonHoc SET TenMH = @TenMH WHERE MaMH = @MaMH";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaMH", maMH);
                        cmd.Parameters.AddWithValue("@TenMH", tenMH);
                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Cập nhật tên môn học thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy mã môn học này để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi sửa: " + ex.Message, "Lỗi Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
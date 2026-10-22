using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BaiTapQuanLyLop
{
    public partial class ThongTinLop : Form
    {
        // Chuỗi kết nối dùng dấu chấm (.) để đi nộp bài cho chuẩn
        private string connectionString = @"Data Source=.;Initial Catalog=QLSinhVien_Chung;Integrated Security=True";

        public ThongTinLop()
        {
            InitializeComponent();
        }

        // ==========================================
        // 1. CHỨC NĂNG THÊM
        // ==========================================
        private void btnThem_Click(object sender, EventArgs e)
        {
            string maKhoa = txtMaKhoa.Text.Trim();
            string maLop = txtMaLop.Text.Trim();
            string tenLop = txtTenLop.Text.Trim();

            // Ràng buộc: Không được để trống bất kỳ trường nào
            if (string.IsNullOrEmpty(maKhoa) || string.IsNullOrEmpty(maLop) || string.IsNullOrEmpty(tenLop))
            {
                MessageBox.Show("Mã khoa, Mã lớp và Tên lớp không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "INSERT INTO Lop (MaLop, MaKhoa, TenLop) VALUES (@MaLop, @MaKhoa, @TenLop)";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaLop", maLop);
                        cmd.Parameters.AddWithValue("@MaKhoa", maKhoa);
                        cmd.Parameters.AddWithValue("@TenLop", tenLop);

                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Thêm lớp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearFields();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm (Có thể trùng Mã lớp): " + ex.Message, "Lỗi Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==========================================
        // 2. CHỨC NĂNG XÓA
        // ==========================================
        private void btnXoa_Click(object sender, EventArgs e)
        {
            string maLop = txtMaLop.Text.Trim();

            // Ràng buộc: Mã lớp không được để trống
            if (string.IsNullOrEmpty(maLop))
            {
                MessageBox.Show("Mã lớp không được để trống khi xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult dr = MessageBox.Show($"Bạn có chắc muốn xóa lớp có mã '{maLop}' không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No) return;

            string query = "DELETE FROM Lop WHERE MaLop = @MaLop";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaLop", maLop);

                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Xóa lớp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearFields();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy mã lớp này trong cơ sở dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Lỗi Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==========================================
        // 3. CHỨC NĂNG SỬA
        // ==========================================
        private void btnSua_Click(object sender, EventArgs e)
        {
            string maLop = txtMaLop.Text.Trim();
            string maKhoa = txtMaKhoa.Text.Trim();
            string tenLop = txtTenLop.Text.Trim();

            // Ràng buộc 1: Mã lớp không được để trống (làm gốc để tìm)
            if (string.IsNullOrEmpty(maLop))
            {
                MessageBox.Show("Mã lớp không được để trống khi sửa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Ràng buộc 2: Phải có ít nhất 1 trường (Mã khoa hoặc Tên lớp) có dữ liệu để update
            if (string.IsNullOrEmpty(maKhoa) && string.IsNullOrEmpty(tenLop))
            {
                MessageBox.Show("Vui lòng nhập Mã khoa hoặc Tên lớp để cập nhật!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Dùng Dynamic SQL để chỉ cập nhật những ô có nhập liệu
            List<string> updateFields = new List<string>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;

                        if (!string.IsNullOrEmpty(maKhoa))
                        {
                            updateFields.Add("MaKhoa = @MaKhoa");
                            cmd.Parameters.AddWithValue("@MaKhoa", maKhoa);
                        }

                        if (!string.IsNullOrEmpty(tenLop))
                        {
                            updateFields.Add("TenLop = @TenLop");
                            cmd.Parameters.AddWithValue("@TenLop", tenLop);
                        }

                        string query = "UPDATE Lop SET " + string.Join(", ", updateFields) + " WHERE MaLop = @MaLop";
                        cmd.CommandText = query;
                        cmd.Parameters.AddWithValue("@MaLop", maLop);

                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Sửa thông tin lớp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearFields();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy lớp có mã này để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi sửa: " + ex.Message, "Lỗi Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Dọn dẹp form sau khi xong việc
        private void ClearFields()
        {
            txtMaKhoa.Clear();
            txtMaLop.Clear();
            txtTenLop.Clear();
            txtMaKhoa.Focus();
        }
    }
}
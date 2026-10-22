using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BaiTapQuanLySinhVien
{
    public partial class QuanLySinhVien : Form
    {
        // Chuỗi kết nối chuẩn (dùng "." đại diện cho máy hiện tại)
        private string connectionString = @"Data Source=.;Initial Catalog=QLSinhVien_Chung;Integrated Security=True";

        public QuanLySinhVien()
        {
            InitializeComponent();
        }

        // ==========================================
        // 1. CHỨC NĂNG THÊM DỮ LIỆU
        // ==========================================
        private void btnThem_Click(object sender, EventArgs e)
        {
            string maLop = txtMaLop.Text.Trim();
            string maSV = txtMaSV.Text.Trim();
            string hoTen = txtHoTen.Text.Trim();
            string ngaySinh = mtbNgaySinh.Text;

            // Kiểm tra rỗng (mtbNgaySinh.MaskCompleted để check xem đã nhập đủ dd/MM/yyyy chưa)
            if (string.IsNullOrEmpty(maLop) || string.IsNullOrEmpty(maSV) ||
                string.IsNullOrEmpty(hoTen) || !mtbNgaySinh.MaskCompleted)
            {
                MessageBox.Show("Mã lớp, Mã sinh viên, Họ tên và Ngày sinh không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "INSERT INTO SinhVien (MaLop, MaSV, HoTen, NgaySinh) VALUES (@MaLop, @MaSV, @HoTen, @NgaySinh)";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaLop", maLop);
                        cmd.Parameters.AddWithValue("@MaSV", maSV);
                        cmd.Parameters.AddWithValue("@HoTen", hoTen);
                        cmd.Parameters.AddWithValue("@NgaySinh", ngaySinh);

                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Thêm sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearFields();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm (Có thể trùng Mã SV): " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==========================================
        // 2. CHỨC NĂNG XÓA DỮ LIỆU
        // ==========================================
        private void btnXoa_Click(object sender, EventArgs e)
        {
            string maSV = txtMaSV.Text.Trim();
            string hoTen = txtHoTen.Text.Trim();

            // Yêu cầu: Mã sinh viên, Tên sinh viên không được để trống
            if (string.IsNullOrEmpty(maSV) || string.IsNullOrEmpty(hoTen))
            {
                MessageBox.Show("Vui lòng nhập chính xác Mã sinh viên và Tên sinh viên để xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult dr = MessageBox.Show($"Bạn có chắc chắn muốn xóa sinh viên {hoTen} (Mã: {maSV}) không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No) return;

            string query = "DELETE FROM SinhVien WHERE MaSV = @MaSV AND HoTen = @HoTen";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaSV", maSV);
                        cmd.Parameters.AddWithValue("@HoTen", hoTen);

                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Xóa sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearFields();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy sinh viên khớp với Mã SV và Tên SV đã nhập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==========================================
        // 3. CHỨC NĂNG SỬA DỮ LIỆU (LINH HOẠT)
        // ==========================================
        private void btnSua_Click(object sender, EventArgs e)
        {
            string maSV = txtMaSV.Text.Trim();
            string maLop = txtMaLop.Text.Trim();
            string hoTen = txtHoTen.Text.Trim();

            // Check xem ô Ngày Sinh có trống hoàn toàn không (xóa hết dấu /)
            string rawNgaySinh = mtbNgaySinh.Text.Replace("/", "").Trim();
            bool isNgaySinhBlank = rawNgaySinh.Length == 0;

            // Bắt buộc phải có Mã SV làm gốc để tìm
            if (string.IsNullOrEmpty(maSV))
            {
                MessageBox.Show("Mã sinh viên không được để trống khi sửa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Phải có ít nhất 1 trường dữ liệu để sửa
            if (string.IsNullOrEmpty(maLop) && string.IsNullOrEmpty(hoTen) && isNgaySinhBlank)
            {
                MessageBox.Show("Vui lòng nhập dữ liệu vào Mã lớp, Họ tên, hoặc Ngày sinh để cập nhật!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Tạo câu lệnh SQL linh hoạt: Chỉ Update những trường nào được người dùng nhập vào
            List<string> updateFields = new List<string>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;

                        if (!string.IsNullOrEmpty(maLop))
                        {
                            updateFields.Add("MaLop = @MaLop");
                            cmd.Parameters.AddWithValue("@MaLop", maLop);
                        }

                        if (!string.IsNullOrEmpty(hoTen))
                        {
                            updateFields.Add("HoTen = @HoTen");
                            cmd.Parameters.AddWithValue("@HoTen", hoTen);
                        }

                        if (!isNgaySinhBlank)
                        {
                            // Nếu có nhập ngày sinh thì phải nhập cho đủ định dạng
                            if (!mtbNgaySinh.MaskCompleted)
                            {
                                MessageBox.Show("Vui lòng nhập đầy đủ định dạng Ngày sinh (dd/MM/yyyy)!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            updateFields.Add("NgaySinh = @NgaySinh");
                            cmd.Parameters.AddWithValue("@NgaySinh", mtbNgaySinh.Text);
                        }

                        // Lắp ráp câu lệnh SQL cuối cùng
                        string query = "UPDATE SinhVien SET " + string.Join(", ", updateFields) + " WHERE MaSV = @MaSV";
                        cmd.CommandText = query;
                        cmd.Parameters.AddWithValue("@MaSV", maSV);

                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Cập nhật thông tin sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearFields();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy sinh viên có mã này để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi sửa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Hàm hỗ trợ dọn dẹp form sau khi thao tác xong
        private void ClearFields()
        {
            txtMaLop.Clear();
            txtMaSV.Clear();
            txtHoTen.Clear();
            mtbNgaySinh.Clear();
            txtMaLop.Focus();
        }
    }
}
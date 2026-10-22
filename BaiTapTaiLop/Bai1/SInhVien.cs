using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BaiTapSinhVienCombo
{
    public partial class SinhVien : Form
    {
        private string connectionString = @"Data Source=.;Initial Catalog=QLSinhVien_Chung;Integrated Security=True";

        public SinhVien()
        {
            InitializeComponent();
        }

        // ==========================================
        // SỰ KIỆN FORM LOAD - ĐỔ DỮ LIỆU VÀO COMBOBOX
        // ==========================================
        private void SinhVien_Load(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT MaLop FROM Lop";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cboLop.DataSource = dt;
                    cboLop.DisplayMember = "MaLop";
                    cboLop.ValueMember = "MaLop";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách lớp: " + ex.Message, "Lỗi Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==========================================
        // 1. CHỨC NĂNG THÊM
        // ==========================================
        private void btnThem_Click(object sender, EventArgs e)
        {
            string maLop = cboLop.SelectedValue?.ToString();
            string maSV = txtMaSV.Text.Trim();
            string hoTen = txtHoTen.Text.Trim();
            string ngaySinh = mtbNgaySinh.Text;

            // Yêu cầu 1: Tất cả thông tin phải được nhập dữ liệu
            if (string.IsNullOrEmpty(maLop) || string.IsNullOrEmpty(maSV) || string.IsNullOrEmpty(hoTen) || !mtbNgaySinh.MaskCompleted)
            {
                MessageBox.Show("Tất cả các trường thông tin (Lớp, Mã SV, Họ tên, Ngày sinh) đều phải được nhập đầy đủ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Yêu cầu 2: Sinh viên phải có tuổi từ 17 trở lên (chỉ tính theo năm sinh)
            string[] parts = ngaySinh.Split('/');
            if (parts.Length == 3)
            {
                int namSinh = int.Parse(parts[2]);
                int namHienTai = DateTime.Now.Year;
                if ((namHienTai - namSinh) < 17)
                {
                    MessageBox.Show("Sinh viên phải có tuổi từ 17 trở lên!", "Cảnh báo độ tuổi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Yêu cầu 3: Kiểm tra trùng mã sinh viên
                    string checkQuery = "SELECT COUNT(*) FROM SinhVien WHERE MaSV = @MaSV";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@MaSV", maSV);
                        int count = (int)checkCmd.ExecuteScalar();
                        if (count > 0)
                        {
                            MessageBox.Show("Mã sinh viên này đã tồn tại trong hệ thống (Trùng mã)!", "Lỗi trùng lặp", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // Yêu cầu 4: Thêm vào bảng nếu thỏa mọi điều kiện
                    string insertQuery = "INSERT INTO SinhVien (MaSV, MaLop, HoTen, NgaySinh) VALUES (@MaSV, @MaLop, @HoTen, @NgaySinh)";
                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaSV", maSV);
                        cmd.Parameters.AddWithValue("@MaLop", maLop);
                        cmd.Parameters.AddWithValue("@HoTen", hoTen);
                        cmd.Parameters.AddWithValue("@NgaySinh", ngaySinh);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Thêm sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearFields();
                    }
                }
            }
            catch (SqlException ex)
            {
                // Kiểm tra ràng buộc khóa ngoại (Foreign Key) - Mã lỗi 547
                if (ex.Number == 547)
                {
                    MessageBox.Show("Mã lớp không tồn tại trong hệ thống (Lỗi Khóa Ngoại)!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Lỗi Database: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ==========================================
        // 2. CHỨC NĂNG XÓA
        // ==========================================
        private void btnXoa_Click(object sender, EventArgs e)
        {
            string maSV = txtMaSV.Text.Trim();
            if (string.IsNullOrEmpty(maSV))
            {
                MessageBox.Show("Vui lòng nhập Mã SV cần xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult dr = MessageBox.Show($"Bạn có chắc muốn xóa sinh viên có mã '{maSV}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No) return;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM SinhVien WHERE MaSV = @MaSV";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaSV", maSV);
                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Xóa sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearFields();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy mã sinh viên này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                // Kiểm tra nếu sinh viên này đang có điểm (Lỗi ràng buộc khóa ngoại với bảng Điểm nếu có)
                if (ex.Number == 547)
                    MessageBox.Show("Không thể xóa sinh viên này vì đang tồn tại dữ liệu liên quan (Ví dụ: Điểm) - Lỗi Khóa Ngoại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show("Lỗi Database: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==========================================
        // 3. CHỨC NĂNG SỬA
        // ==========================================
        private void btnSua_Click(object sender, EventArgs e)
        {
            string maLop = cboLop.SelectedValue?.ToString();
            string maSV = txtMaSV.Text.Trim();
            string hoTen = txtHoTen.Text.Trim();
            string ngaySinh = mtbNgaySinh.Text;

            if (string.IsNullOrEmpty(maSV))
            {
                MessageBox.Show("Mã sinh viên không được để trống khi sửa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // Sửa toàn bộ thông tin (Lớp, Họ tên, Ngày sinh) dựa theo Mã SV
                    string query = "UPDATE SinhVien SET MaLop = @MaLop, HoTen = @HoTen, NgaySinh = @NgaySinh WHERE MaSV = @MaSV";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaSV", maSV);
                        cmd.Parameters.AddWithValue("@MaLop", maLop);
                        cmd.Parameters.AddWithValue("@HoTen", hoTen);
                        cmd.Parameters.AddWithValue("@NgaySinh", ngaySinh);

                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Sửa thông tin sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearFields();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy mã sinh viên này để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                // Kiểm tra nếu sửa Mã Lớp thành 1 lớp không tồn tại (Lỗi khóa ngoại)
                if (ex.Number == 547)
                    MessageBox.Show("Lớp được chọn không tồn tại trong hệ thống (Lỗi Khóa Ngoại)!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show("Lỗi Database: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearFields()
        {
            txtMaSV.Clear();
            txtHoTen.Clear();
            mtbNgaySinh.Clear();
            txtMaSV.Focus();
        }
    }
}
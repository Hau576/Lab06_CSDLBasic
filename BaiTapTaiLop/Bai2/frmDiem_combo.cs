using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BaiTapQuanLyDiemCombo
{
    public partial class frmDiem_combo : Form
    {
        private string connectionString = @"Data Source=.;Initial Catalog=QLSinhVien_Chung;Integrated Security=True";

        public frmDiem_combo()
        {
            InitializeComponent();
        }

        // ==============================================================
        // SỰ KIỆN FORM LOAD - ĐỔ DỮ LIỆU VÀO COMBOBOX SINH VIÊN VÀ MÔN HỌC
        // ==============================================================
        private void frmDiem_combo_Load(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // 1. Tải danh sách Sinh viên
                    SqlDataAdapter daSV = new SqlDataAdapter("SELECT MaSV FROM SinhVien", conn);
                    DataTable dtSV = new DataTable();
                    daSV.Fill(dtSV);
                    cboSinhVien.DataSource = dtSV;
                    cboSinhVien.DisplayMember = "MaSV";
                    cboSinhVien.ValueMember = "MaSV";

                    // 2. Tải danh sách Môn học
                    SqlDataAdapter daMH = new SqlDataAdapter("SELECT MaMH FROM MonHoc", conn);
                    DataTable dtMH = new DataTable();
                    daMH.Fill(dtMH);
                    cboMonHoc.DataSource = dtMH;
                    cboMonHoc.DisplayMember = "MaMH";
                    cboMonHoc.ValueMember = "MaMH";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu Combobox: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==========================================
        // HÀM KIỂM TRA ĐIỀU KIỆN TRƯỚC KHI THAO TÁC
        // ==========================================
        private bool KiemTraDuLieu(out double diemHopLe)
        {
            diemHopLe = 0;

            if (cboSinhVien.SelectedValue == null || cboMonHoc.SelectedValue == null || string.IsNullOrWhiteSpace(txtDiem.Text))
            {
                MessageBox.Show("Vui lòng chọn Sinh viên, Môn học và nhập Điểm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Chuyển dấu phẩy thành dấu chấm để hệ thống dễ đọc số thập phân
            string strDiem = txtDiem.Text.Replace(',', '.');
            if (!double.TryParse(strDiem, out diemHopLe))
            {
                MessageBox.Show("Điểm phải là một số!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Yêu cầu: Điểm phải nằm trong khoảng (0-10)
            if (diemHopLe < 0 || diemHopLe > 10)
            {
                MessageBox.Show("Điểm phải nằm trong khoảng từ 0 đến 10!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        // ==========================================
        // 1. CHỨC NĂNG THÊM
        // ==========================================
        private void btnThem_Click(object sender, EventArgs e)
        {
            double diem;
            if (!KiemTraDuLieu(out diem)) return;

            string maSV = cboSinhVien.SelectedValue.ToString();
            string maMH = cboMonHoc.SelectedValue.ToString();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Kiểm tra trùng khóa chính (MaSV + MaMH)
                    string checkQuery = "SELECT COUNT(*) FROM Diem WHERE MaSV = @MaSV AND MaMH = @MaMH";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@MaSV", maSV);
                        checkCmd.Parameters.AddWithValue("@MaMH", maMH);
                        int count = (int)checkCmd.ExecuteScalar();
                        if (count > 0)
                        {
                            MessageBox.Show("Sinh viên này đã có điểm môn học này (Trùng dữ liệu)!", "Lỗi trùng lặp", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // Nếu không trùng, tiến hành thêm điểm
                    string insertQuery = "INSERT INTO Diem (MaSV, MaMH, Diem) VALUES (@MaSV, @MaMH, @Diem)";
                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaSV", maSV);
                        cmd.Parameters.AddWithValue("@MaMH", maMH);
                        cmd.Parameters.AddWithValue("@Diem", diem);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Thêm điểm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtDiem.Clear();
                    }
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                    MessageBox.Show("Mã Sinh viên hoặc Mã Môn học không tồn tại trong hệ thống (Lỗi Khóa Ngoại)!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show("Lỗi Database: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==========================================
        // 2. CHỨC NĂNG XÓA
        // ==========================================
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (cboSinhVien.SelectedValue == null || cboMonHoc.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn Sinh viên và Môn học để xóa điểm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maSV = cboSinhVien.SelectedValue.ToString();
            string maMH = cboMonHoc.SelectedValue.ToString();

            DialogResult dr = MessageBox.Show($"Bạn có chắc muốn xóa điểm môn {maMH} của sinh viên {maSV} không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No) return;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM Diem WHERE MaSV = @MaSV AND MaMH = @MaMH";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaSV", maSV);
                        cmd.Parameters.AddWithValue("@MaMH", maMH);
                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Xóa điểm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtDiem.Clear();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy điểm của sinh viên này để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        // 3. CHỨC NĂNG SỬA
        // ==========================================
        private void btnSua_Click(object sender, EventArgs e)
        {
            double diem;
            if (!KiemTraDuLieu(out diem)) return;

            string maSV = cboSinhVien.SelectedValue.ToString();
            string maMH = cboMonHoc.SelectedValue.ToString();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE Diem SET Diem = @Diem WHERE MaSV = @MaSV AND MaMH = @MaMH";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaSV", maSV);
                        cmd.Parameters.AddWithValue("@MaMH", maMH);
                        cmd.Parameters.AddWithValue("@Diem", diem);

                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Cập nhật điểm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtDiem.Clear();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy dữ liệu điểm này để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                    MessageBox.Show("Sinh viên hoặc Môn học không tồn tại (Lỗi Khóa Ngoại)!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show("Lỗi Database: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
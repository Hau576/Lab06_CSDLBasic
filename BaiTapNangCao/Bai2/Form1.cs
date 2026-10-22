using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DanhSachDiemApp
{
    public partial class Form2 : Form
    {
        // Chuỗi kết nối đến database QLSinhVien_Chung của ông
        SqlConnection connsql = new SqlConnection(@"Data Source=.;Initial Catalog=QLSinhVien_Chung;Integrated Security=True");

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // Cấu hình giao diện ListView dạng bảng
            listViewDiem.View = View.Details;
            listViewDiem.FullRowSelect = true;
            listViewDiem.GridLines = true;

            // Thêm các cột theo đúng yêu cầu đề bài (STT, Tên SV, Ngày sinh, Tên môn học, Điểm)
            listViewDiem.Columns.Add("STT", 60, HorizontalAlignment.Center);
            listViewDiem.Columns.Add("Tên SV", 180, HorizontalAlignment.Left);
            listViewDiem.Columns.Add("Ngày sinh", 140, HorizontalAlignment.Left);
            listViewDiem.Columns.Add("Tên môn học", 240, HorizontalAlignment.Left);
            listViewDiem.Columns.Add("Điểm", 120, HorizontalAlignment.Left);

            LoadComboBoxMonHoc();
            HienThiDuLieu(""); // Load tất cả dữ liệu lúc ban đầu
        }

        // 1. Load danh sách Môn học lên ComboBox, kèm thêm 1 item khoảng trắng (tất cả môn học)
        private void LoadComboBoxMonHoc()
        {
            try
            {
                connsql.Open();
                string query = "SELECT MaMH FROM MonHoc";
                SqlCommand cmd = new SqlCommand(query, connsql);
                SqlDataReader rd = cmd.ExecuteReader();

                cboMonHoc.Items.Clear();
                cboMonHoc.Items.Add(""); // Item khoảng trắng biểu thị tất cả môn học[cite: 11]

                while (rd.Read())
                {
                    cboMonHoc.Items.Add(rd["MaMH"].ToString());
                }

                rd.Close();
                connsql.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải ComboBox môn học: " + ex.Message);
                if (connsql.State == System.Data.ConnectionState.Open) connsql.Close();
            }
        }

        // 2. Hàm hiển thị dữ liệu lên ListView và tính tổng điểm
        private void HienThiDuLieu(string maMH)
        {
            try
            {
                listViewDiem.Items.Clear();
                connsql.Open();

                // Câu truy vấn kết 4 bảng: Diem, SinhVien, MonHoc
                string query = "SELECT SinhVien.HoTen, SinhVien.NgaySinh, MonHoc.TenMH, Diem.Diem " +
                               "FROM Diem " +
                               "INNER JOIN SinhVien ON Diem.MaSV = SinhVien.MaSV " +
                               "INNER JOIN MonHoc ON Diem.MaMH = MonHoc.MaMH";

                // Nếu có chọn mã môn học cụ thể (khác rỗng) thì thêm điều kiện WHERE lọc theo môn
                if (!string.IsNullOrEmpty(maMH))
                {
                    query += " WHERE Diem.MaMH = @MaMH";
                }

                SqlCommand cmd = new SqlCommand(query, connsql);
                if (!string.IsNullOrEmpty(maMH))
                {
                    cmd.Parameters.AddWithValue("@MaMH", maMH);
                }

                SqlDataReader rd = cmd.ExecuteReader();

                int stt = 1;
                double tongDiem = 0;

                while (rd.Read())
                {
                    ListViewItem item = new ListViewItem(stt.ToString());
                    item.SubItems.Add(rd["HoTen"].ToString());
                    item.SubItems.Add(rd["NgaySinh"].ToString());
                    item.SubItems.Add(rd["TenMH"].ToString());

                    double diem = Convert.ToDouble(rd["Diem"]);
                    item.SubItems.Add(diem.ToString());

                    listViewDiem.Items.Add(item);

                    tongDiem += diem; // Cộng dồn tổng điểm
                    stt++;
                }

                rd.Close();
                connsql.Close();

                // Gán tổng điểm vào TextBox Tổng điểm
                txtTongDiem.Text = tongDiem.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lọc dữ liệu: " + ex.Message);
                if (connsql.State == System.Data.ConnectionState.Open) connsql.Close();
            }
        }

        // 3. Sự kiện khi nhấn nút "Lọc dữ liệu"
        private void btnLoc_Click(object sender, EventArgs e)
        {
            string selectedMonHoc = cboMonHoc.SelectedItem != null ? cboMonHoc.SelectedItem.ToString() : "";
            HienThiDuLieu(selectedMonHoc);
        }
    }
}
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DanhSachLopApp
{
    public partial class Form1 : Form
    {
        // Chuỗi kết nối khớp chuẩn với database QLSinhVien_Chung của ông
        SqlConnection connsql = new SqlConnection(@"Data Source=.;Initial Catalog=QLSinhVien_Chung;Integrated Security=True");

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Cấu hình giao diện ListView hiển thị dạng bảng (Details)
            listViewLop.View = View.Details;
            listViewLop.FullRowSelect = true;
            listViewLop.GridLines = true;

            // Thiết lập các cột trên ListView (Thêm cột Tên khoa nếu ông muốn hiển thị rõ tên)
            listViewLop.Columns.Add("STT", 60, HorizontalAlignment.Center);
            listViewLop.Columns.Add("Mã lớp", 120, HorizontalAlignment.Left);
            listViewLop.Columns.Add("Tên lớp", 300, HorizontalAlignment.Left);
            listViewLop.Columns.Add("Mã khoa", 100, HorizontalAlignment.Left);
            listViewLop.Columns.Add("Tên khoa", 180, HorizontalAlignment.Left); // Cột kết nối bảng Khoa
        }

        // Sự kiện khi bấm nút "Hiển thị dữ liệu"
        private void btnHienThi_Click(object sender, EventArgs e)
        {
            try
            {
                listViewLop.Items.Clear(); // Xóa dữ liệu cũ
                connsql.Open();

                // Dùng JOIN giữa bảng Lop và bảng Khoa để lấy chính xác TenKhoa dựa vào MaKhoa
                string query = "SELECT Lop.MaLop, Lop.TenLop, Lop.MaKhoa, Khoa.TenKhoa " +
                               "FROM Lop INNER JOIN Khoa ON Lop.MaKhoa = Khoa.MaKhoa";

                SqlCommand cmd = new SqlCommand(query, connsql);
                SqlDataReader rd = cmd.ExecuteReader();

                int stt = 1;
                while (rd.Read())
                {
                    // Cột 1: Số thứ tự (STT)
                    ListViewItem item = new ListViewItem(stt.ToString());

                    // Các cột tiếp theo lấy từ bảng Lop và Khoa
                    item.SubItems.Add(rd["MaLop"].ToString());
                    item.SubItems.Add(rd["TenLop"].ToString());
                    item.SubItems.Add(rd["MaKhoa"].ToString());
                    item.SubItems.Add(rd["TenKhoa"].ToString()); // Hiển thị TenKhoa

                    // Đưa dòng vào ListView
                    listViewLop.Items.Add(item);
                    stt++;
                }

                rd.Close();
                connsql.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hiển thị dữ liệu: " + ex.Message);
                if (connsql.State == System.Data.ConnectionState.Open)
                    connsql.Close();
            }
        }

        // Sự kiện khi bấm nút "Thoát"
        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
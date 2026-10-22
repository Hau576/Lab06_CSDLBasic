
namespace BaiTapXuLyChuoi
{
    partial class Diem
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblMaSV;
        private System.Windows.Forms.Label lblMaMH;
        private System.Windows.Forms.Label lblDiem;
        private System.Windows.Forms.TextBox txtMaSV;
        private System.Windows.Forms.TextBox txtMaMH;
        private System.Windows.Forms.TextBox txtDiem;
        private System.Windows.Forms.Button btnThem;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.Button btnSua;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblMaSV = new System.Windows.Forms.Label();
            this.lblMaMH = new System.Windows.Forms.Label();
            this.lblDiem = new System.Windows.Forms.Label();
            this.txtMaSV = new System.Windows.Forms.TextBox();
            this.txtMaMH = new System.Windows.Forms.TextBox();
            this.txtDiem = new System.Windows.Forms.TextBox();
            this.btnThem = new System.Windows.Forms.Button();
            this.btnXoa = new System.Windows.Forms.Button();
            this.btnSua = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // Form
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(350, 175);
            this.FormBorderStyle =
                System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition =
                System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Name = "Diem";
            this.Text = "Quản lý Điểm";

            // Label MaSV
            this.lblMaSV.AutoSize = true;
            this.lblMaSV.Location = new System.Drawing.Point(12, 20);
            this.lblMaSV.Name = "lblMaSV";
            this.lblMaSV.Size = new System.Drawing.Size(40, 15);
            this.lblMaSV.Text = "Mã sv";

            // TextBox MaSV
            this.txtMaSV.Location = new System.Drawing.Point(85, 17);
            this.txtMaSV.Name = "txtMaSV";
            this.txtMaSV.Size = new System.Drawing.Size(145, 23);
            this.txtMaSV.TabIndex = 0;
            this.txtMaSV.MaxLength = 20;

            // Label MaMH
            this.lblMaMH.AutoSize = true;
            this.lblMaMH.Location = new System.Drawing.Point(12, 45);
            this.lblMaMH.Name = "lblMaMH";
            this.lblMaMH.Size = new System.Drawing.Size(44, 15);
            this.lblMaMH.Text = "Mã mh";

            // TextBox MaMH
            this.txtMaMH.Location = new System.Drawing.Point(85, 42);
            this.txtMaMH.Name = "txtMaMH";
            this.txtMaMH.Size = new System.Drawing.Size(145, 23);
            this.txtMaMH.TabIndex = 1;
            this.txtMaMH.MaxLength = 20;

            // Label Diem
            this.lblDiem.AutoSize = true;
            this.lblDiem.Location = new System.Drawing.Point(12, 70);
            this.lblDiem.Name = "lblDiem";
            this.lblDiem.Size = new System.Drawing.Size(32, 15);
            this.lblDiem.Text = "Điểm";

            // TextBox Diem
            this.txtDiem.Location = new System.Drawing.Point(85, 67);
            this.txtDiem.Name = "txtDiem";
            this.txtDiem.Size = new System.Drawing.Size(145, 23);
            this.txtDiem.TabIndex = 2;

            // Button Them
            this.btnThem.Location = new System.Drawing.Point(15, 110);
            this.btnThem.Name = "btnThem";
            this.btnThem.Size = new System.Drawing.Size(70, 30);
            this.btnThem.TabIndex = 3;
            this.btnThem.Text = "Thêm";
            this.btnThem.UseVisualStyleBackColor = true;
            this.btnThem.Click +=
                new System.EventHandler(this.btnThem_Click);

            // Button Xoa
            this.btnXoa.Location = new System.Drawing.Point(105, 110);
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.Size = new System.Drawing.Size(70, 30);
            this.btnXoa.TabIndex = 4;
            this.btnXoa.Text = "Xóa";
            this.btnXoa.UseVisualStyleBackColor = true;
            this.btnXoa.Click +=
                new System.EventHandler(this.btnXoa_Click);

            // Button Sua
            this.btnSua.Location = new System.Drawing.Point(195, 110);
            this.btnSua.Name = "btnSua";
            this.btnSua.Size = new System.Drawing.Size(70, 30);
            this.btnSua.TabIndex = 5;
            this.btnSua.Text = "Sửa";
            this.btnSua.UseVisualStyleBackColor = true;
            this.btnSua.Click +=
                new System.EventHandler(this.btnSua_Click);

            // Add controls
            this.Controls.Add(this.lblMaSV);
            this.Controls.Add(this.lblMaMH);
            this.Controls.Add(this.lblDiem);
            this.Controls.Add(this.txtMaSV);
            this.Controls.Add(this.txtMaMH);
            this.Controls.Add(this.txtDiem);
            this.Controls.Add(this.btnThem);
            this.Controls.Add(this.btnXoa);
            this.Controls.Add(this.btnSua);

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}

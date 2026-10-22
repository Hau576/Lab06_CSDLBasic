namespace DanhSachLopApp
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.btnHienThi = new System.Windows.Forms.Button();
            this.listViewLop = new System.Windows.Forms.ListView();
            this.btnThoat = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnHienThi
            // 
            this.btnHienThi.Location = new System.Drawing.Point(30, 25);
            this.btnHienThi.Name = "btnHienThi";
            this.btnHienThi.Size = new System.Drawing.Size(130, 35);
            this.btnHienThi.TabIndex = 0;
            this.btnHienThi.Text = "Hiển thị dữ liệu";
            this.btnHienThi.UseVisualStyleBackColor = true;
            this.btnHienThi.Click += new System.EventHandler(this.btnHienThi_Click);
            // 
            // listViewLop
            // 
            this.listViewLop.HideSelection = false;
            this.listViewLop.Location = new System.Drawing.Point(30, 75);
            this.listViewLop.Name = "listViewLop";
            this.listViewLop.Size = new System.Drawing.Size(740, 320);
            this.listViewLop.TabIndex = 1;
            this.listViewLop.UseCompatibleStateImageBehavior = false;
            // 
            // btnThoat
            // 
            this.btnThoat.Location = new System.Drawing.Point(670, 415);
            this.btnThoat.Name = "btnThoat";
            this.btnThoat.Size = new System.Drawing.Size(100, 35);
            this.btnThoat.TabIndex = 2;
            this.btnThoat.Text = "Thoát";
            this.btnThoat.UseVisualStyleBackColor = true;
            this.btnThoat.Click += new System.EventHandler(this.btnThoat_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 475);
            this.Controls.Add(this.btnThoat);
            this.Controls.Add(this.listViewLop);
            this.Controls.Add(this.btnHienThi);
            this.Name = "Form1";
            this.Text = "Danh sách Lớp";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Button btnHienThi;
        private System.Windows.Forms.ListView listViewLop;
        private System.Windows.Forms.Button btnThoat;
    }
}
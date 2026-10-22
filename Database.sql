USE QLSinhVien_Chung;
GO

-- ==========================================
-- 1. DỌN DẸP BẢNG CŨ BỊ LỖI (Xóa từ Con lên Cha)
-- ==========================================
DROP TABLE IF EXISTS Diem;
DROP TABLE IF EXISTS SinhVien;
DROP TABLE IF EXISTS Lop;
DROP TABLE IF EXISTS Khoa;
DROP TABLE IF EXISTS MonHoc;
GO

-- ==========================================
-- 2. TẠO LẠI CẤU TRÚC BẢNG CHUẨN
-- ==========================================
CREATE TABLE Khoa (
    MaKhoa NVARCHAR(50) PRIMARY KEY,
    TenKhoa NVARCHAR(200) NOT NULL
);

CREATE TABLE Lop (
    MaLop NVARCHAR(50) PRIMARY KEY,
    MaKhoa NVARCHAR(50) NOT NULL,
    TenLop NVARCHAR(200) NOT NULL,
    CONSTRAINT FK_Lop_Khoa FOREIGN KEY (MaKhoa) REFERENCES Khoa(MaKhoa)
);

CREATE TABLE SinhVien (
    MaSV NVARCHAR(50) PRIMARY KEY,
    HoTen NVARCHAR(200) NOT NULL,
    NgaySinh NVARCHAR(20) NOT NULL,
    MaLop NVARCHAR(50) NOT NULL,
    CONSTRAINT FK_SinhVien_Lop FOREIGN KEY (MaLop) REFERENCES Lop(MaLop)
);

CREATE TABLE MonHoc (
    MaMH NVARCHAR(50) PRIMARY KEY,
    TenMH NVARCHAR(200) NOT NULL
);

CREATE TABLE Diem (
    MaSV NVARCHAR(50) NOT NULL,
    MaMH NVARCHAR(50) NOT NULL,
    Diem FLOAT NOT NULL,
    PRIMARY KEY (MaSV, MaMH),
    CONSTRAINT FK_Diem_SinhVien FOREIGN KEY (MaSV) REFERENCES SinhVien(MaSV),
    CONSTRAINT FK_Diem_MonHoc FOREIGN KEY (MaMH) REFERENCES MonHoc(MaMH)
);
GO

-- ==========================================
-- 3. BƠM TOÀN BỘ DỮ LIỆU CÙNG 1 LÚC
-- ==========================================
INSERT INTO Khoa (MaKhoa, TenKhoa) VALUES 
('CNTT', N'Công nghệ thông tin'), 
('KT', N'Kế toán'),
('NN', N'Ngoại Ngữ'), 
('QT', N'Quản Trị Kinh Doanh'),
('DL', N'Du Lịch'),
('CK', N'Cơ Khí');
GO

INSERT INTO Lop (MaLop, MaKhoa, TenLop) VALUES 
('DCT124C1', 'CNTT', N'Cao đẳng tin học'),
('DCT124C2', 'KT', N'Kế toán khóa 05'),
('DCT124C3', 'NN', N'Cử nhân Tiếng Anh K06'),
('DCT124C4', 'QT', N'Quản trị kinh doanh K05'),
('DCT124C5', 'DL', N'Cao đẳng Du lịch K04'),
('DCT124C6', 'CK', N'Kỹ thuật Cơ khí K06');
GO

INSERT INTO SinhVien (MaSV, HoTen, NgaySinh, MaLop) VALUES 
('SV01', N'Trần Thanh Tùng', '12/10/1993', 'DCT124C1'),
('SV02', N'Nguyễn Thanh Bình', '02/04/1993', 'DCT124C2'),
('SV03', N'Lê Thị Thu', '20/05/1994', 'DCT124C3'),
('SV04', N'Phạm Hữu Phúc', '15/08/1993', 'DCT124C4'),
('SV05', N'Đặng Ngọc Hân', '02/02/1995', 'DCT124C5'),
('SV06', N'Hoàng Văn Nam', '25/11/1994', 'DCT124C6');
GO
INSERT INTO MonHoc (MaMH, TenMH) VALUES 
('MH01', N'Toán Cao Cấp'), 
('MH02', N'Lập trình Winform'),
('MH03', N'Tiếng Anh Giao Tiếp'), 
('MH04', N'Marketing Căn Bản'),
('MH05', N'Tổng Quan Du Lịch'),
('MH06', N'Cơ Học Đại Cương');
GO
INSERT INTO Diem (MaSV, MaMH, Diem) VALUES 
('SV01', 'MH01', 7),
('SV02', 'MH02', 8.5),
('SV03', 'MH03', 9.0),
('SV04', 'MH04', 7.5),
('SV05', 'MH05', 8.0),
('SV06', 'MH06', 6.5);
GO


-- ==========================================
-- 4. XUẤT KẾT QUẢ
-- ==========================================
PRINT N'--- BẢNG KHOA ---'; SELECT * FROM Khoa;
PRINT N'--- BẢNG LỚP ---'; SELECT * FROM Lop;
PRINT N'--- BẢNG SINH VIÊN ---'; SELECT * FROM SinhVien;
PRINT N'--- BẢNG MÔN HỌC ---'; SELECT * FROM MonHoc;
PRINT N'--- BẢNG ĐIỂM ---'; SELECT * FROM Diem;
GO
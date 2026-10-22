
IF DB_ID(N'QLSinhVien') IS NULL
    CREATE DATABASE QLSinhVien;
GO

USE QLSinhVien;
GO

IF OBJECT_ID(N'dbo.Diem', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Diem
    (
        MaSV VARCHAR(20) NOT NULL,
        MaMH VARCHAR(20) NOT NULL,
        Diem DECIMAL(4,2) NOT NULL,

        CONSTRAINT PK_Diem PRIMARY KEY (MaSV, MaMH),
        CONSTRAINT CK_Diem_Range CHECK (Diem >= 0 AND Diem <= 10)
    );
END;
GO

SELECT * FROM dbo.Diem;
GO

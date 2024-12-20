USE [2023_My_Pham]
GO
/****** Object:  User [CAPTAINM10]    Script Date: 24/11/2023 9:03:29 pm ******/
CREATE USER [CAPTAINM10] FOR LOGIN [CAPTAINM10] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [DESKTOP-TA9T2DE]    Script Date: 24/11/2023 9:03:29 pm ******/
CREATE USER [DESKTOP-TA9T2DE] FOR LOGIN [DESKTOP-TA9T2DE] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [trunghieu]    Script Date: 24/11/2023 9:03:29 pm ******/
CREATE USER [trunghieu] FOR LOGIN [trunghieu] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 24/11/2023 9:03:29 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Account]    Script Date: 24/11/2023 9:03:29 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[AccountId] [int] IDENTITY(1,1) NOT NULL,
	[Phone] [varchar](12) NULL,
	[AccountEmail] [nvarchar](50) NULL,
	[AccountPassword] [nvarchar](50) NULL,
	[Sail] [nchar](6) NULL,
	[Active] [bit] NOT NULL,
	[FullName] [nvarchar](50) NULL,
	[RoleId] [int] NULL,
	[LastLogin] [datetime] NULL,
	[CreateDate] [datetime] NULL,
	[MaKhachHang] [int] NULL,
 CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED 
(
	[AccountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChiTietDonHang]    Script Date: 24/11/2023 9:03:29 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChiTietDonHang](
	[MaDonHang] [varchar](50) NOT NULL,
	[MaSanPham] [varchar](50) NOT NULL,
	[SoLuong] [int] NULL,
	[GiaBan] [float] NULL,
 CONSTRAINT [PK_ChiTietDonHang] PRIMARY KEY CLUSTERED 
(
	[MaDonHang] ASC,
	[MaSanPham] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChiTietLoHang]    Script Date: 24/11/2023 9:03:29 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChiTietLoHang](
	[MaLoHang] [varchar](50) NOT NULL,
	[MaSanPham] [varchar](50) NOT NULL,
	[SoLuong] [int] NULL,
	[GiaNhap] [float] NULL,
 CONSTRAINT [PK_ChiTietLoHang] PRIMARY KEY CLUSTERED 
(
	[MaLoHang] ASC,
	[MaSanPham] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CTLoai]    Script Date: 24/11/2023 9:03:29 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CTLoai](
	[MaCTLoai] [varchar](50) NOT NULL,
	[TenCTLoai] [nvarchar](250) NULL,
	[MaLoai] [varchar](50) NULL,
 CONSTRAINT [PK_CTLoai] PRIMARY KEY CLUSTERED 
(
	[MaCTLoai] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DanhMucSanPham]    Script Date: 24/11/2023 9:03:29 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DanhMucSanPham](
	[MaDanhMuc] [varchar](50) NOT NULL,
	[TenDanhMuc] [nvarchar](250) NULL,
	[MoTa] [nvarchar](max) NULL,
	[MaCTLoai] [varchar](50) NULL,
	[MaHang] [varchar](50) NULL,
	[HinhAnh] [nvarchar](250) NULL,
	[DanhGia] [float] NULL,
	[Gia] [float] NULL,
	[CachDung] [nvarchar](max) NULL,
	[ChiTiet] [nvarchar](max) NULL,
 CONSTRAINT [PK_DanhMucSanPham] PRIMARY KEY CLUSTERED 
(
	[MaDanhMuc] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DiaChi]    Script Date: 24/11/2023 9:03:29 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DiaChi](
	[LocationID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[Type] [nvarchar](20) NULL,
	[Slug] [nvarchar](100) NULL,
	[NameWithType] [nvarchar](255) NULL,
	[PathWithType] [nvarchar](255) NULL,
	[ParentCode] [int] NULL,
	[Levels] [int] NULL,
 CONSTRAINT [PK_DiaChi] PRIMARY KEY CLUSTERED 
(
	[LocationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DonHang]    Script Date: 24/11/2023 9:03:29 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DonHang](
	[MaDonHang] [varchar](50) NOT NULL,
	[TongTien] [float] NULL,
	[TrangThaiDonHang] [int] NULL,
	[MaKhachHang] [int] NULL,
	[ThanhToan] [bit] NULL,
	[VanChuyen] [int] NULL,
	[PhiVanChuyen] [float] NULL,
	[NgayDatHang] [datetime] NULL,
 CONSTRAINT [PK_DonHang] PRIMARY KEY CLUSTERED 
(
	[MaDonHang] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Hang]    Script Date: 24/11/2023 9:03:29 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hang](
	[MaHang] [varchar](50) NOT NULL,
	[TenHang] [nvarchar](250) NULL,
	[XuatXu] [nvarchar](50) NULL,
 CONSTRAINT [PK_Hang] PRIMARY KEY CLUSTERED 
(
	[MaHang] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HoaDon]    Script Date: 24/11/2023 9:03:29 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HoaDon](
	[MaHoaDon] [varchar](50) NOT NULL,
	[NgayLap] [date] NULL,
	[TongTien] [float] NULL,
	[MaDonHang] [varchar](50) NULL,
 CONSTRAINT [PK_HoaDon] PRIMARY KEY CLUSTERED 
(
	[MaHoaDon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KhachHang]    Script Date: 24/11/2023 9:03:29 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KhachHang](
	[MaKhachHang] [int] IDENTITY(1,1) NOT NULL,
	[TenKhachHang] [nvarchar](250) NULL,
	[DiaChi] [nvarchar](max) NULL,
	[SoDienThoai] [nvarchar](50) NULL,
	[NgaySinh] [datetime] NULL,
	[Avatar] [nvarchar](250) NULL,
	[Address] [nvarchar](250) NULL,
	[Email] [nchar](150) NULL,
	[DisTrict] [int] NULL,
	[Ward] [int] NULL,
	[LocationID] [int] NULL,
	[CreateDate] [datetime] NULL,
	[Password] [nvarchar](50) NULL,
	[Salt] [nchar](8) NULL,
	[LastLogin] [datetime] NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_KhachHang] PRIMARY KEY CLUSTERED 
(
	[MaKhachHang] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Loai]    Script Date: 24/11/2023 9:03:29 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Loai](
	[MaLoai] [varchar](50) NOT NULL,
	[TenLoai] [nvarchar](250) NULL,
 CONSTRAINT [PK_Loai] PRIMARY KEY CLUSTERED 
(
	[MaLoai] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LoHang]    Script Date: 24/11/2023 9:03:29 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LoHang](
	[MaLoHang] [varchar](50) NOT NULL,
	[NgayNhan] [date] NULL,
	[MaNhaPP] [varchar](50) NULL,
	[GiaLo] [float] NULL,
 CONSTRAINT [PK_LoHang] PRIMARY KEY CLUSTERED 
(
	[MaLoHang] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NhaPhanPhoi]    Script Date: 24/11/2023 9:03:29 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NhaPhanPhoi](
	[MaNhaPP] [varchar](50) NOT NULL,
	[TenNhaPP] [nvarchar](250) NULL,
	[DiaChi] [nvarchar](max) NULL,
	[SDT] [varchar](50) NULL,
	[Email] [nvarchar](150) NULL,
 CONSTRAINT [PK_NhaPhanPhoi] PRIMARY KEY CLUSTERED 
(
	[MaNhaPP] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 24/11/2023 9:03:29 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](50) NULL,
	[Description] [nvarchar](50) NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SanPham]    Script Date: 24/11/2023 9:03:29 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SanPham](
	[MaSanPham] [varchar](50) NOT NULL,
	[TenSanPham] [nvarchar](250) NULL,
	[Mau] [nvarchar](50) NULL,
	[Anh] [nvarchar](max) NULL,
	[Gia] [float] NULL,
	[KhuyenMai] [float] NULL,
	[SLKho] [int] NULL,
	[NgaySX] [datetime] NULL,
	[LuotMua] [int] NULL,
	[MaDanhMuc] [varchar](50) NULL,
	[HomeFlag] [bit] NOT NULL,
	[BestSellers] [bit] NOT NULL,
	[MoTa] [nvarchar](max) NULL,
	[Active] [bit] NOT NULL,
	[NgaySua] [datetime] NULL,
 CONSTRAINT [PK_SanPham] PRIMARY KEY CLUSTERED 
(
	[MaSanPham] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[SanPham] ADD  CONSTRAINT [DF__SanPham__Active__3C34F16F]  DEFAULT ((0)) FOR [Active]
GO
ALTER TABLE [dbo].[Account]  WITH CHECK ADD  CONSTRAINT [FK_Account_KhachHang] FOREIGN KEY([MaKhachHang])
REFERENCES [dbo].[KhachHang] ([MaKhachHang])
GO
ALTER TABLE [dbo].[Account] CHECK CONSTRAINT [FK_Account_KhachHang]
GO
ALTER TABLE [dbo].[Account]  WITH CHECK ADD  CONSTRAINT [FK_Account_Role] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([RoleId])
GO
ALTER TABLE [dbo].[Account] CHECK CONSTRAINT [FK_Account_Role]
GO
ALTER TABLE [dbo].[ChiTietDonHang]  WITH CHECK ADD  CONSTRAINT [FK_ChiTietDonHang_DonHang] FOREIGN KEY([MaDonHang])
REFERENCES [dbo].[DonHang] ([MaDonHang])
GO
ALTER TABLE [dbo].[ChiTietDonHang] CHECK CONSTRAINT [FK_ChiTietDonHang_DonHang]
GO
ALTER TABLE [dbo].[ChiTietDonHang]  WITH CHECK ADD  CONSTRAINT [FK_ChiTietDonHang_SanPham] FOREIGN KEY([MaSanPham])
REFERENCES [dbo].[SanPham] ([MaSanPham])
GO
ALTER TABLE [dbo].[ChiTietDonHang] CHECK CONSTRAINT [FK_ChiTietDonHang_SanPham]
GO
ALTER TABLE [dbo].[ChiTietLoHang]  WITH CHECK ADD  CONSTRAINT [FK_ChiTietLoHang_LoHang] FOREIGN KEY([MaLoHang])
REFERENCES [dbo].[LoHang] ([MaLoHang])
GO
ALTER TABLE [dbo].[ChiTietLoHang] CHECK CONSTRAINT [FK_ChiTietLoHang_LoHang]
GO
ALTER TABLE [dbo].[ChiTietLoHang]  WITH CHECK ADD  CONSTRAINT [FK_ChiTietLoHang_SanPham] FOREIGN KEY([MaSanPham])
REFERENCES [dbo].[SanPham] ([MaSanPham])
GO
ALTER TABLE [dbo].[ChiTietLoHang] CHECK CONSTRAINT [FK_ChiTietLoHang_SanPham]
GO
ALTER TABLE [dbo].[CTLoai]  WITH CHECK ADD  CONSTRAINT [FK_CTLoai_Loai] FOREIGN KEY([MaLoai])
REFERENCES [dbo].[Loai] ([MaLoai])
GO
ALTER TABLE [dbo].[CTLoai] CHECK CONSTRAINT [FK_CTLoai_Loai]
GO
ALTER TABLE [dbo].[DanhMucSanPham]  WITH CHECK ADD  CONSTRAINT [FK_DanhMucSanPham_CTLoai] FOREIGN KEY([MaCTLoai])
REFERENCES [dbo].[CTLoai] ([MaCTLoai])
GO
ALTER TABLE [dbo].[DanhMucSanPham] CHECK CONSTRAINT [FK_DanhMucSanPham_CTLoai]
GO
ALTER TABLE [dbo].[DanhMucSanPham]  WITH CHECK ADD  CONSTRAINT [FK_DanhMucSanPham_Hang] FOREIGN KEY([MaHang])
REFERENCES [dbo].[Hang] ([MaHang])
GO
ALTER TABLE [dbo].[DanhMucSanPham] CHECK CONSTRAINT [FK_DanhMucSanPham_Hang]
GO
ALTER TABLE [dbo].[DonHang]  WITH CHECK ADD  CONSTRAINT [FK_DonHang_KhachHang] FOREIGN KEY([MaKhachHang])
REFERENCES [dbo].[KhachHang] ([MaKhachHang])
GO
ALTER TABLE [dbo].[DonHang] CHECK CONSTRAINT [FK_DonHang_KhachHang]
GO
ALTER TABLE [dbo].[HoaDon]  WITH CHECK ADD  CONSTRAINT [FK_HoaDon_DonHang] FOREIGN KEY([MaDonHang])
REFERENCES [dbo].[DonHang] ([MaDonHang])
GO
ALTER TABLE [dbo].[HoaDon] CHECK CONSTRAINT [FK_HoaDon_DonHang]
GO
ALTER TABLE [dbo].[LoHang]  WITH CHECK ADD  CONSTRAINT [FK_LoHang_NhaPhanPhoi] FOREIGN KEY([MaNhaPP])
REFERENCES [dbo].[NhaPhanPhoi] ([MaNhaPP])
GO
ALTER TABLE [dbo].[LoHang] CHECK CONSTRAINT [FK_LoHang_NhaPhanPhoi]
GO
ALTER TABLE [dbo].[SanPham]  WITH CHECK ADD  CONSTRAINT [FK_SanPham_DanhMucSanPham] FOREIGN KEY([MaDanhMuc])
REFERENCES [dbo].[DanhMucSanPham] ([MaDanhMuc])
GO
ALTER TABLE [dbo].[SanPham] CHECK CONSTRAINT [FK_SanPham_DanhMucSanPham]
GO

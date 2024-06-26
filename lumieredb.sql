USE [Lumiereshop]
GO
/****** Object:  Table [dbo].[ChiTietDonHang]    Script Date: 29/05/2024 7:49:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChiTietDonHang](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SoLuong] [int] NULL,
	[Gia] [int] NULL,
	[IDSanPham] [int] NULL,
	[IDDonHang] [int] NULL,
 CONSTRAINT [PK__ChiTietD__3214EC270DD3DDFD] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DonHang]    Script Date: 29/05/2024 7:49:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DonHang](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Ten] [nvarchar](255) NULL,
	[NgayDat] [date] NULL,
	[DiaChi] [nvarchar](255) NULL,
	[SoDienThoai] [nvarchar](255) NULL,
	[TrangThai] [int] NULL,
	[IDNguoiDung] [int] NULL,
 CONSTRAINT [PK__DonHang__3214EC27FED225C0] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LienHe]    Script Date: 29/05/2024 7:49:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LienHe](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Ten] [nvarchar](max) NULL,
	[NoiDung] [nvarchar](max) NULL,
 CONSTRAINT [PK__LienHe__3214EC27609A40BE] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Loai]    Script Date: 29/05/2024 7:49:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Loai](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Ten] [nvarchar](255) NULL,
 CONSTRAINT [PK__Loai__3214EC275175D369] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NguoiDung]    Script Date: 29/05/2024 7:49:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NguoiDung](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TaiKhoan] [nvarchar](255) NULL,
	[MatKhau] [nvarchar](255) NULL,
	[Ten] [nvarchar](255) NULL,
	[DiaChi] [nvarchar](255) NULL,
	[Quyen] [int] NULL,
 CONSTRAINT [PK__NguoiDun__3214EC275CA5BA30] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PhanHoi]    Script Date: 29/05/2024 7:49:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PhanHoi](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Ten] [nvarchar](255) NULL,
	[ChucDanh] [nvarchar](255) NULL,
	[Anh] [nvarchar](255) NULL,
	[NoiDung] [nvarchar](255) NULL,
 CONSTRAINT [PK__PhanHoi__3214EC27A91A45D1] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SanPham]    Script Date: 29/05/2024 7:49:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SanPham](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Ten] [nvarchar](255) NULL,
	[Anh] [nvarchar](255) NULL,
	[Gia] [int] NULL,
	[SoLuong] [int] NULL,
	[Size] [nvarchar](255) NULL,
	[LuuY] [nvarchar](max) NULL,
	[IDLoai] [int] NULL,
 CONSTRAINT [PK__SanPham__3214EC27CFB23E95] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Slide]    Script Date: 29/05/2024 7:49:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Slide](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Anh] [nvarchar](255) NULL,
 CONSTRAINT [PK__Slide__3214EC275F09B457] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ThamSo]    Script Date: 29/05/2024 7:49:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ThamSo](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Ma] [nvarchar](255) NULL,
	[Ten] [nvarchar](255) NULL,
	[NoiDung] [nvarchar](max) NULL,
	[Anh] [nvarchar](255) NULL,
 CONSTRAINT [PK__ThamSo__3214EC271A411C8C] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TinTuc]    Script Date: 29/05/2024 7:49:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TinTuc](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Ten] [nvarchar](255) NULL,
	[Anh] [nvarchar](255) NULL,
	[NoiDung] [nvarchar](max) NULL,
	[NgayDang] [date] NULL,
 CONSTRAINT [PK__TinTuc__3214EC279A14AB68] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[ChiTietDonHang] ON 

INSERT [dbo].[ChiTietDonHang] ([ID], [SoLuong], [Gia], [IDSanPham], [IDDonHang]) VALUES (13, 1, 100000, 11, 12)
INSERT [dbo].[ChiTietDonHang] ([ID], [SoLuong], [Gia], [IDSanPham], [IDDonHang]) VALUES (14, 1, 50000, 15, 12)
SET IDENTITY_INSERT [dbo].[ChiTietDonHang] OFF
GO
SET IDENTITY_INSERT [dbo].[DonHang] ON 

INSERT [dbo].[DonHang] ([ID], [Ten], [NgayDat], [DiaChi], [SoDienThoai], [TrangThai], [IDNguoiDung]) VALUES (12, N'admin', CAST(N'2024-05-26' AS Date), N'92 ngo quyen', N'0326203327', 0, 1)
SET IDENTITY_INSERT [dbo].[DonHang] OFF
GO
SET IDENTITY_INSERT [dbo].[LienHe] ON 

INSERT [dbo].[LienHe] ([ID], [Ten], [NoiDung]) VALUES (2, N'Làm thế nào để đặt hàng?', N'Để đặt hàng, bạn chỉ cần truy cập trang web của chúng tôi và duyệt qua các sản phẩm có sẵn. Khi bạn đã chọn được sản phẩm ưa thích, chỉ cần điền vào các thông tin cần thiết như địa chỉ giao hàng và thông tin thanh toán. Chúng tôi cam kết sẽ giao hàng đến địa chỉ một cách nhanh chóng và an toàn nhất.')
INSERT [dbo].[LienHe] ([ID], [Ten], [NoiDung]) VALUES (3, N'Bạn có chấp nhận đơn hàng quốc tế không?', N'Chúng tôi cũng nhận đơn hàng quốc tế, tuy nhiên, việc giao hàng quốc tế có thể mất thêm thời gian và cước phí phát sinh. Đội ngũ chăm sóc khách hàng của chúng tôi sẽ luôn sẵn sàng hỗ trợ bạn trong quá trình đặt hàng và trả lời mọi thắc mắc của bạn.')
INSERT [dbo].[LienHe] ([ID], [Ten], [NoiDung]) VALUES (4, N'Phương thức thanh toán nào bạn chấp nhận?', N'Chúng tôi chấp nhận thanh toán thông qua thẻ tín dụng/debit, chuyển khoản ngân hàng và thanh toán khi nhận hàng (COD).')
INSERT [dbo].[LienHe] ([ID], [Ten], [NoiDung]) VALUES (5, N'Bạn giao hàng đến đâu?', N'Chúng tôi giao hàng đến địa chỉ trong phạm vi quốc gia nội địa. Xin lưu ý rằng có thể có một số hạn chế về vùng giao hàng.')
INSERT [dbo].[LienHe] ([ID], [Ten], [NoiDung]) VALUES (6, N'Làm thế nào để liên hệ với dịch vụ khách hàng của bạn?', N'Bạn có thể liên hệ với chúng tôi qua số điện thoại hoặc email được cung cấp trên trang web của chúng tôi. Đội ngũ chăm sóc khách hàng của chúng tôi sẽ sẵn lòng hỗ trợ bạn.')
SET IDENTITY_INSERT [dbo].[LienHe] OFF
GO
SET IDENTITY_INSERT [dbo].[Loai] ON 

INSERT [dbo].[Loai] ([ID], [Ten]) VALUES (1, N'Áo ')
INSERT [dbo].[Loai] ([ID], [Ten]) VALUES (2, N'Váy')
INSERT [dbo].[Loai] ([ID], [Ten]) VALUES (3, N'Giày cao gót')
INSERT [dbo].[Loai] ([ID], [Ten]) VALUES (4, N'Giày bata')
INSERT [dbo].[Loai] ([ID], [Ten]) VALUES (10, N'Nón')
SET IDENTITY_INSERT [dbo].[Loai] OFF
GO
SET IDENTITY_INSERT [dbo].[NguoiDung] ON 

INSERT [dbo].[NguoiDung] ([ID], [TaiKhoan], [MatKhau], [Ten], [DiaChi], [Quyen]) VALUES (1, N'admin', N'96cae35ce8a9b0244178bf28e4966c2ce1b8385723a96a6b838858cdd6ca0a1e', N'admin', N'Việt Nam', 1)
INSERT [dbo].[NguoiDung] ([ID], [TaiKhoan], [MatKhau], [Ten], [DiaChi], [Quyen]) VALUES (10, N'user', N'e606e38b0d8c19b24cf0ee3808183162ea7cd63ff7912dbb22b5e803286b4446', N'Long', N'92 Ngô Quyền', 2)
SET IDENTITY_INSERT [dbo].[NguoiDung] OFF
GO
SET IDENTITY_INSERT [dbo].[PhanHoi] ON 

INSERT [dbo].[PhanHoi] ([ID], [Ten], [ChucDanh], [Anh], [NoiDung]) VALUES (1, N'Khải', N'Quản lý', N'member3_00d872d0.png', N'Bán hàng bằng cái tâm')
INSERT [dbo].[PhanHoi] ([ID], [Ten], [ChucDanh], [Anh], [NoiDung]) VALUES (2, N'Long', N'Nhân viên', N'member4_fa963133.png', N'Bán hàng bằng cái tâm')
INSERT [dbo].[PhanHoi] ([ID], [Ten], [ChucDanh], [Anh], [NoiDung]) VALUES (7, N'Tiến', N'Nhân viên', N'user-8_7616e970.png', N'Bán hàng bằng cái tâm')
SET IDENTITY_INSERT [dbo].[PhanHoi] OFF
GO
SET IDENTITY_INSERT [dbo].[SanPham] ON 

INSERT [dbo].[SanPham] ([ID], [Ten], [Anh], [Gia], [SoLuong], [Size], [LuuY], [IDLoai]) VALUES (11, N'Áo 1', N'p15_hover_a0cae002.jpg', 100000, 20, N'L', N'Áo thun nữ', 1)
INSERT [dbo].[SanPham] ([ID], [Ten], [Anh], [Gia], [SoLuong], [Size], [LuuY], [IDLoai]) VALUES (12, N'Váy 1', N'p13_hover_74e23acd.jpg', 150000, 10, N'L', N'Váy thời trang', 2)
INSERT [dbo].[SanPham] ([ID], [Ten], [Anh], [Gia], [SoLuong], [Size], [LuuY], [IDLoai]) VALUES (13, N'Giày cao gót 1', N'p3_1feccdfc.jpg', 200000, 50, N'L', N'Giày cao gót ', 3)
INSERT [dbo].[SanPham] ([ID], [Ten], [Anh], [Gia], [SoLuong], [Size], [LuuY], [IDLoai]) VALUES (14, N'Giày bata 1', N'p8_hover_0722e84e.jpg', 280000, 15, N'L', N'Giày bata trắng', 4)
INSERT [dbo].[SanPham] ([ID], [Ten], [Anh], [Gia], [SoLuong], [Size], [LuuY], [IDLoai]) VALUES (15, N'Nón 1', N'p10_hover_a9148611.jpg', 50000, 100, N'M', N'Nón thời trang', 1)
INSERT [dbo].[SanPham] ([ID], [Ten], [Anh], [Gia], [SoLuong], [Size], [LuuY], [IDLoai]) VALUES (16, N'Áo thun 2', N'p2_bb94fdb0.jpg', 110000, 60, N'L', N'Áo thun nữ 2', 1)
INSERT [dbo].[SanPham] ([ID], [Ten], [Anh], [Gia], [SoLuong], [Size], [LuuY], [IDLoai]) VALUES (17, N'Giày cao gót 2', N'p12_de8518f7.jpg', 200000, 10, N'L', N'Giày cao gót 2', 3)
SET IDENTITY_INSERT [dbo].[SanPham] OFF
GO
SET IDENTITY_INSERT [dbo].[Slide] ON 

INSERT [dbo].[Slide] ([ID], [Anh]) VALUES (3, N'top-menu-banner_faaaa016.jpg')
INSERT [dbo].[Slide] ([ID], [Anh]) VALUES (4, N'home-banner3_7457ed3c.jpg')
SET IDENTITY_INSERT [dbo].[Slide] OFF
GO
SET IDENTITY_INSERT [dbo].[ThamSo] ON 

INSERT [dbo].[ThamSo] ([ID], [Ma], [Ten], [NoiDung], [Anh]) VALUES (1, N'LOGO', N'Logo website', NULL, N'logo_cbb7f117.png')
INSERT [dbo].[ThamSo] ([ID], [Ma], [Ten], [NoiDung], [Anh]) VALUES (2, N'NUMBER', N'Số điện thoại liên hệ website', N'0987654321', NULL)
INSERT [dbo].[ThamSo] ([ID], [Ma], [Ten], [NoiDung], [Anh]) VALUES (3, N'EMAIL', N'Email liên hệ website', N'lumiereshop@gmail.com', NULL)
INSERT [dbo].[ThamSo] ([ID], [Ma], [Ten], [NoiDung], [Anh]) VALUES (4, N'ADDRESS', N'Địa chỉ của cửa hàng', N'Việt Nam', NULL)
INSERT [dbo].[ThamSo] ([ID], [Ma], [Ten], [NoiDung], [Anh]) VALUES (5, N'NAME', N'Tên của website', N'Lumière Shop', NULL)
SET IDENTITY_INSERT [dbo].[ThamSo] OFF
GO
ALTER TABLE [dbo].[ChiTietDonHang]  WITH CHECK ADD  CONSTRAINT [IDDonHang_DonHang] FOREIGN KEY([IDDonHang])
REFERENCES [dbo].[DonHang] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ChiTietDonHang] CHECK CONSTRAINT [IDDonHang_DonHang]
GO
ALTER TABLE [dbo].[ChiTietDonHang]  WITH CHECK ADD  CONSTRAINT [IDSanPham_SanPham] FOREIGN KEY([IDSanPham])
REFERENCES [dbo].[SanPham] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ChiTietDonHang] CHECK CONSTRAINT [IDSanPham_SanPham]
GO
ALTER TABLE [dbo].[DonHang]  WITH CHECK ADD  CONSTRAINT [IDNguoiDung_NguoiDung] FOREIGN KEY([IDNguoiDung])
REFERENCES [dbo].[NguoiDung] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DonHang] CHECK CONSTRAINT [IDNguoiDung_NguoiDung]
GO
ALTER TABLE [dbo].[SanPham]  WITH CHECK ADD  CONSTRAINT [IDLoai_Loai] FOREIGN KEY([IDLoai])
REFERENCES [dbo].[Loai] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SanPham] CHECK CONSTRAINT [IDLoai_Loai]
GO

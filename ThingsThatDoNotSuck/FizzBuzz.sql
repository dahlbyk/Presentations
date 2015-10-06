CREATE DATABASE [FizzBuzz]

CREATE TABLE [dbo].[Palabras](
	[Numero] [tinyint] NOT NULL,
	[Texto] [varchar](50) NULL
) ON [PRIMARY]
GO

INSERT [dbo].[Palabras] ([Numero], [Texto]) VALUES (3, N'Fizz')
INSERT [dbo].[Palabras] ([Numero], [Texto]) VALUES (5, N'Buzz')

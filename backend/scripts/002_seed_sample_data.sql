USE [VINAY];
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[Leads] WHERE [Phone] = N'9999999999')
BEGIN
    INSERT INTO [dbo].[Leads] ([Name], [Email], [Phone], [EmploymentType], [MonthlyIncome], [LoanAmount], [City], [Source], [CreatedDate], [Status])
    VALUES (N'John Doe', N'john@example.com', N'9999999999', N'Salaried', N'75000', 1500000.00, N'Mumbai', N'Website', GETUTCDATE(), N'Pending');
END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[Contacts] WHERE [Email] = N'contact@example.com')
BEGIN
    INSERT INTO [dbo].[Contacts] ([Name], [Email], [Phone], [Topic], [Message], [CreatedDate])
    VALUES (N'Jane Smith', N'contact@example.com', N'8888888888', N'General Inquiry', N'Hello from sample data.', GETUTCDATE());
END
GO


CREATE TABLE [L_PATIENT_INFO_DELETED] (
[Patient_ID] numeric(18, 0)NOT NULL,
[PID] nvarchar(50) NOT NULL,
[Patient_Name] nvarchar(100) NULL,
[Address] nvarchar(200) NULL,
[Age] smallint NULL,
[YEAR_BIRTH] int NULL,
[Sex] tinyint NULL,
[Insurance_Num] varchar(20) NULL,
[DATEUPDATE] datetime NULL,
[Diagnostic] nvarchar(100) NULL,
[IdentifyNum] nvarchar(20) NULL,
[DepartmentID] smallint NULL,
[Room] nvarchar(50) NULL,
[Bed] nvarchar(50) NULL,
[ObjectType] smallint NULL)
ON [PRIMARY];
GO



CREATE TABLE [dbo].[T_TEST_INFO_DELETED] (
[Test_ID] numeric(18, 0) NOT NULL,
[TestType_ID] numeric(18, 0) NULL,
[Barcode] nvarchar(50) NULL,
[Test_Seq] nchar(10) NULL,
[Patient_ID] numeric(18, 0) NULL,
[Test_Date] datetime NULL,
[Require_Date] datetime NULL,
[Assign_ID] numeric(18, 0) NULL,
[Diagnostician_ID] numeric(18, 0) NULL,
[Receiver_ID] numeric(18, 0) NULL,
[Test_Status] smallint NULL,
[Diag_Result] nvarchar(200) NULL,
[Update_User] numeric(18, 0) NULL,
[Update_Date] datetime NULL,
[His_Assign_id] int NULL,
[Device_ID] numeric(18, 0) NULL,
[Para_ID] int NULL,
[Printstatus] bit NULL)
GO

CREATE TABLE [T_RESULT_DETAIL_DELETED] (
[TestDetail_ID] numeric(18, 0) NOT NULL,
[Test_ID] numeric(18, 0) NULL,
[Patient_ID] numeric(18, 0) NULL,
[TestType_ID] int NULL,
[Test_Date] datetime NULL,
[Test_Sequence] nvarchar(50) NULL,
[Data_Sequence] int NULL,
[Test_Result] nvarchar(100) NULL,
[Normal_levelW] nvarchar(100) NULL,
[Normal_Level] nvarchar(100) NULL,
[Measure_Unit] nvarchar(50) NULL,
[Para_Name] nvarchar(100) NULL,
[Para_Status] smallint NOT NULL,
[Note] nvarchar(100) NULL,
[PrintData] bit NULL,
[Barcode] nvarchar(50) NULL,
[UpdateNum] int NULL)
ON [PRIMARY];
GO
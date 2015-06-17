SELECT * FROM T_RESULT_DETAIL trd

exec sp_DailyTestReportParamDetail_KQ_VD @pTestDateFrom=N'30/12/2011',@pTestDateTo=N'30/12/2011',@pTestType_ID=1,@ObjectType=-1,@DepartMent_ID=-1,@PID=N'NOTHING'

exec sp_Department_Doctor_TestReport @pTestDateFrom='2011-12-30 00:00:00:000',@pTestDateTo='2011-12-30 00:00:00:000',@pTestType_ID=-1,@DepartmentID=-1,@DoctorID=-1


exec [dbo].[spGetTestInfoGroupBySID] 
@pTestDateFrom=N'30/12/2011',@pTestDateTo=N'30/12/2011',@strTestType_ID=N'1,3,2,4,5',@Barcode=N'NOTHING',@PID=N'NOTHING',@Patient_Name=N'NOTHING',@Age=0,@Sex=-1,@strTestStatus=N'0,20,30,40,50,55,80,90'

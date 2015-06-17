
/*
* Lay data tu D_DATA_CONTROL theo Device
* */





CREATE PROCEDURE [dbo].[sp_Get_DataControl_Follow_Device]
	@Device_ID NUMERIC
AS
	IF @Device_ID IS NULL
	   OR @Device_ID = -1
	    SELECT D.Device_ID,
	           (
	               SELECT TOP 1 Device_Name
	               FROM   D_DEVICE_LIST DL
	               WHERE  DL.Device_ID = D.Device_ID
	           ) AS Device_Name,
	           d.DataType_ID,
	           D.Data_Sequence,
	           D.Control_Type,
	           D.Data_Name,
	           D.Alias_Name,
	           D.Measure_Unit,
	           D.Data_Point,
	           D.Normal_Level,
	           D.Normal_LevelW,
	           D.Data_View,
	           D.Data_Print,
	           D.[Description],
	           1 AS CHON
	    FROM   D_DATA_CONTROL D
	ELSE
	    SELECT D.Device_ID,
	           (
	               SELECT TOP 1 Device_Name
	               FROM   D_DEVICE_LIST DL
	               WHERE  DL.Device_ID = D.Device_ID
	           ) AS Device_Name,
	           D.Data_Sequence,
	           D.Control_Type,
	           D.Data_Name,
	           d.DataType_ID,
	           D.Alias_Name,
	           D.Measure_Unit,
	           D.Data_Point,
	           D.Normal_Level,
	           D.Normal_LevelW,
	           D.Data_View,
	           D.Data_Print,
	           D.[Description],
	           1 AS CHON
	    FROM   D_DATA_CONTROL D
	    WHERE  D.Device_ID = @Device_ID
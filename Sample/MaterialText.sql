Drop table #SalesText
SELECT distinct
       a.[MATNR] as  MaterialNumber
      ,[TEXTTYPE] as TextType
      ,[SPRAS_DETAIL] as Locale
      ,[DISPLAYTEXT] as DisplayText
      ,[DETAILTEXT] as DetailText
      ,[DESCRIPTION] as Description
  into #SalesText  
  FROM [dbo].[ZPD_EC_MAT_TEXT] as a 
  inner join dbo.ZPD_V_TO_SEACOL  as b 
		on a.MATNR = b.MATNR 
		and b.MANDT = N'100'
		and b.SEASON = N'S19'
  inner join dbo.MARA  as c
		on a.MATNR = b.MATNR 
		and b.MANDT = N'100'
		and c.MTART = 'Z600'
  where DISPLAYTEXT <> ''

  update #SalesText set Locale = 'en-GB' where Locale = 'en-UK'

  select Convert(int,MaterialNumber) as MaterialNumber
      ,Locale
      ,DisplayText
      ,DetailText
      ,Description 
    from #SalesText

  Drop table #SalesText
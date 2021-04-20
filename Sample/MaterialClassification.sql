USE [afsreplic]
GO

SELECT 
        convert(int, [MaterialNumber]) as MaterialNumber
      ,[Language] as Locale
      ,[AttributeId]
      ,[Label]
      ,[Value]
      ,[ValueText]
  FROM [product].[Classifications] as c
    inner join ZPD_V_TO_SEACOL as seasons 
  on seasons.MATNR = c.MaterialNumber
      inner join MARA as mara 
  on mara.MATNR = c.MaterialNumber
  and seasons.MANDT = N'100'
  where SEASON = 'S19'
  and MARA.MTART = 'Z600'
GO

USE [afsreplic]
GO

SELECT 
  convert(int,[MaterialNumber]) as MaterialNumber
     
      ,[Language] as Locale
      ,[MaterialComposition] as MaterialCompositionID
      ,[DeclarationGroup] as Component
      ,[DeclarationGroupText] as ComponentText
      ,[Proportion]
      ,[Text]
      ,[Abbreviation]
  FROM [product].[MaterialCompositions] as mc 
  inner join ZPD_V_TO_SEACOL as seasons 
  on seasons.MATNR = mc.MaterialNumber
  and seasons.MANDT = N'100'
   inner join MARA as mara 
  on mara.MATNR = mc.MaterialNumber
  and seasons.MANDT = N'100'
  where SEASON = 'S19'
  and mara.MTART = 'Z600'
GO


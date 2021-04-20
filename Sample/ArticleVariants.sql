

declare @SEASON nvarchar(3) ='S19'

select DISTINCT
	'0000000000' + EAN.MATNR COLLATE SQL_Latin1_General_CP1_CS_AS as MaterialNumber ,
	EAN.J_3AKORDX COLLATE SQL_Latin1_General_CP1_CS_AS  as ColorSize,
	EAN.EAN11
into #EAN

from  [PLM_COLLAB].[SAP].[MEAN] as EAN
inner join MARA as MARA
	on MARA.MATNR COLLATE SQL_Latin1_General_CP1_CS_AS = '0000000000' + EAN.MATNR COLLATE SQL_Latin1_General_CP1_CS_AS
		and MARA.MTART = 'Z600'
inner join [dbo].[ZPD_V_TO_SEACOL] as SC 
	on SC.MATNR COLLATE DATABASE_DEFAULT = MARA.MATNR
where SC.SEASON = @SEASON
CREATE NONCLUSTERED INDEX [IDX] ON #EAN
(
	MaterialNumber ASC,
	ColorSize ASC
)

select DISTINCT
	M.ZZMPGROUP as MainProductGroup,
	M.ZZRPGROUP as RetailProductGroup,
	M.ZZBRAND as Brand,
	M.ZZGENDER as Gender,
	M.ZZLABEL as Line,
	Convert(int,M.ZZFORM) as FormMaterialNumber,
	Convert(int,M.ZZQUALI) as QualityMaterialNumber,
    MAKTG as MaterialText,
	Convert(int,SC.MATNR) as MaterialNumber,
	SCC.COLOR as Color,
	SIZES.J_3AKORD2 as Size1,
	SIZES.J_3AKORD3 as Size2,
	SIZES.J_3AKORDX as ColorSize
into #MATERIALS
from [dbo].[ZPD_V_TO_SEACOL] as SC
    inner join MARA as M
    on SC.MATNR = M.MATNR
        and M.MANDT = N'100'
    inner join [dbo].[ZPD_SEACOL_COLOR] as SCC
    on SCC.MATNR = SC.MATNR
        and SCC.SEASON = SC.SEASON
        and SCC.COLLECT = SC.COLLECT
        and SCC.MANDT = N'100'
    inner join dbo.J_3APGEN as SIZES
    on SIZES.J_3APGNR = M.J_3APGNR
        and SIZES.J_3AKORD1 = SCC.COLOR
        and SIZES.MANDT = N'100'
    inner join dbo.MAKT as MAKT 
    on MAKT.MATNR = SC.MATNR
       and MAKT.SPRAS = 'E'
       and MAKT.MANDT = N'100' 

where SC.SEASON =@SEASON
    and M.MTART = 'Z600'
    and SIZES.J_3AKORDX not like '%$%'

CREATE NONCLUSTERED INDEX [IDX] ON #MATERIALS
(
	MaterialNumber ASC,
	ColorSize ASC
)

select distinct
  
 	Convert(int,a.MaterialNumber) as MaterialNumber, 
    a.ColorSize, 
    Convert(bigint,EAN11) as EAN,

    MainProductGroup, 
    RetailProductGroup,
    Brand,
    Gender, 
    Line, 
    FormMaterialNumber, 
    QualityMaterialNumber, 
    MaterialText, 
  
    Color, 
    Size1, 
    Size2
    
from #EAN as a 
inner join #MATERIALS as b 
	on  a.MaterialNumber = b.MaterialNumber
	and a.ColorSize		 = b.ColorSize


DROP TABLE #EAN
DROP TABLE #MATERIALS
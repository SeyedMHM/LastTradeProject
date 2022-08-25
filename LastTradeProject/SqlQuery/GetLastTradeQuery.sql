IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME ='LastTrade' AND XTYPE='U')
	IF (SELECT Count(*) FROM LastTrade) > 0
		TRUNCATE TABLE LastTrade

GO

IF NOT EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME ='LastTrade' AND XTYPE='U')
    CREATE TABLE LastTrade (
		Id [int] NOT NULL,
		InstrumentId [int] NOT NULL,
		ShortName nvarchar(20) NOT NULL,
		DateTimeEn [datetime] NOT NULL,
		[Open] [decimal](18, 4) NOT NULL,
		[High] [decimal](18, 4) NOT NULL,
		[Low] [decimal](18, 4) NOT NULL,
		[Close] [decimal](18, 4) NOT NULL,
		CONSTRAINT [PK_LastTrade] PRIMARY KEY CLUSTERED ([Id] ASC)
    )

GO

INSERT INTO LastTrade (Id, InstrumentId, ShortName, DateTimeEn, [Open], [High], [Low], [Close])
SELECT T.Id, T.InstrumentId, I.ShortName, T.DateTimeEn, T.[Open], T.[High], T.[Low], T.[Close]
FROM 
(
	SELECT *, ROW_NUMBER() OVER (PARTITION BY Trade.InstrumentId ORDER BY DateTimeEn DESC) AS RowNumber
	FROM Trade
) AS T
INNER JOIN Instrument AS I ON I.Id = T.InstrumentId
WHERE RowNumber = 1

GO

SELECT  * FROM LastTrade

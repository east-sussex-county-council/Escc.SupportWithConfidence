CREATE PROCEDURE [dbo].[usp_PostcodeUsage_Monthly_Breakdown]
	AS
	SELECT     
TOP 100 PERCENT DATENAME(month, i.DateUsed) AS Month, YEAR(i.DateUsed) AS Year, COUNT(i.Count) AS Count
FROM         dbo.PostcodeUsage AS i 
GROUP BY YEAR(i.DateUsed), MONTH(i.DateUsed), DATENAME(month, i.DateUsed)
ORDER BY YEAR(i.DateUsed) desc, MONTH(i.DateUsed)desc
	RETURN


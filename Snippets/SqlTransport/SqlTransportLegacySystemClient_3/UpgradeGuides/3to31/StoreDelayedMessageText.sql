startcode 3to31-StoreDelayedMessageTextSql

DECLARE @NOCOUNT VARCHAR(3) = 'OFF';
IF ( (512 & @@OPTIONS) = 512 ) SET @NOCOUNT = 'ON'
SET NOCOUNT ON;

INSERT INTO {0} (
    Headers,
    Body,
    Due)
VALUES (
    @Headers,
    @Body,
    @Due);

IF(@NOCOUNT = 'ON') SET NOCOUNT ON;
IF(@NOCOUNT = 'OFF') SET NOCOUNT OFF;
endcode

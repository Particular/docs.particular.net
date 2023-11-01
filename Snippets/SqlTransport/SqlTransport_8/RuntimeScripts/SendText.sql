startcode SendTextSql

DECLARE @NOCOUNT VARCHAR(3) = 'OFF';
IF ( (512 & @@OPTIONS) = 512 ) SET @NOCOUNT = 'ON'
SET NOCOUNT ON;

INSERT INTO {0} (
    Id,
    Expires,
    Headers,
    Body)
VALUES (
    @Id,
    CASE WHEN @TimeToBeReceivedMs IS NOT NULL
        THEN DATEADD(ms, @TimeToBeReceivedMs, GETUTCDATE()) END,
    @Headers,
    @Body);

IF (@NOCOUNT = 'ON') SET NOCOUNT ON;
IF (@NOCOUNT = 'OFF') SET NOCOUNT OFF;
endcode

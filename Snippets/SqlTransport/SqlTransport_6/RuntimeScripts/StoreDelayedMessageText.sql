startcode StoreDelayedMessageTextSql

DECLARE @NOCOUNT VARCHAR(3) = 'OFF';
IF ( (512 & @@OPTIONS) = 512 ) SET @NOCOUNT = 'ON'
SET NOCOUNT ON;

DECLARE @DueAfter DATETIME = GETUTCDATE();
SET @DueAfter = DATEADD(ms, @DueAfterMilliseconds, @DueAfter);
SET @DueAfter = DATEADD(s, @DueAfterSeconds, @DueAfter);
SET @DueAfter = DATEADD(n, @DueAfterMinutes, @DueAfter);
SET @DueAfter = DATEADD(hh, @DueAfterHours, @DueAfter);
SET @DueAfter = DATEADD(d, @DueAfterDays, @DueAfter);

INSERT INTO {0} (
    Headers,
    Body,
    Due)
VALUES (
    @Headers,
    @Body,
    @DueAfter);

IF(@NOCOUNT = 'ON') SET NOCOUNT ON;
IF(@NOCOUNT = 'OFF') SET NOCOUNT OFF;
endcode

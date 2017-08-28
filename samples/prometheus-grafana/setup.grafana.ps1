$user = Read-Host -Prompt 'Input username for grafana'
$pass = Read-Host -Prompt 'Input password for grafana'
$pair = "${user}:${pass}"
$bytes = [System.Text.Encoding]::ASCII.GetBytes($pair)
$base64 = [System.Convert]::ToBase64String($bytes)
$basicAuthValue = "Basic $base64"
$headers = @{ Authorization = $basicAuthValue }

$body = @"
{
  "name":"PrometheusNServiceBusDemo",
  "type":"prometheus",
  "url":"http://localhost:9090",
  "access":"proxy",
  "basicAuth":false,
  "isDefault":false
}
"@

Invoke-WebRequest http://localhost:3000/api/datasources -UseBasicParsing -ContentType "application/json" -Headers $headers -Method POST -Body $body

$dashboardbody = Get-Content -Raw -Path .\grafana-endpoints-dashboard.json

$body = @"
{
  "dashboard":$dashboardbody,
  "overwrite":true
}
"@

Invoke-WebRequest http://localhost:3000/api/dashboards/db -UseBasicParsing -ContentType "application/json" -Headers $headers -Method POST -Body $body
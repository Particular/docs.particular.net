#!/bin/bash
set -e

echo "Starting CosmosDB Transactions Sample Test..."

# Check for CosmosDB connection string
if [ -z "$COSMOS_CONNECTION_STRING" ]; then
    echo "WARNING: COSMOS_CONNECTION_STRING environment variable not set. Using emulator connection string."
fi

# Start server in background
echo "Starting Server..."
dotnet Server/Server.dll > server.log 2>&1 &
SERVER_PID=$!

# Wait for server to initialize
echo "Waiting for server to initialize..."
sleep 5

# Check if server is still running
if ! kill -0 $SERVER_PID 2>/dev/null; then
    echo "ERROR: Server failed to start"
    cat server.log
    exit 1
fi

echo "Server started successfully (PID: $SERVER_PID)"

# Start client and send messages using expect
echo "Starting Client and sending StartOrder messages..."

# Create expect script for client interaction
cat > client_script.exp << 'EOF'
#!/usr/bin/expect -f
set timeout 30

spawn dotnet Client/Client.dll
expect "Press 'S' to send a StartOrder message to the server endpoint"
expect "Press any other key to exit"

# Send first StartOrder message
send "S"
expect "StartOrder Message sent to Server with OrderId"
puts "First StartOrder message sent successfully"

# Wait a moment
sleep 2

# Send second StartOrder message
send "S"
expect "StartOrder Message sent to Server with OrderId"
puts "Second StartOrder message sent successfully"

# Wait for message processing
sleep 3

# Send third StartOrder message
send "S"
expect "StartOrder Message sent to Server with OrderId"
puts "Third StartOrder message sent successfully"

# Wait for processing then exit
sleep 5
send "q"
expect eof
EOF

chmod +x client_script.exp

# Run the client expect script
./client_script.exp > client.log 2>&1 &
CLIENT_PID=$!

# Wait for client to complete
wait $CLIENT_PID
CLIENT_EXIT_CODE=$?

# Allow additional time for saga processing and message completion
echo "Waiting for saga processing to complete..."
sleep 10

# Stop the server
echo "Stopping Server..."
kill $SERVER_PID 2>/dev/null || true
wait $SERVER_PID 2>/dev/null || true

# Analyze results
echo "=== ANALYSIS ==="

echo "=== Server Log ==="
cat server.log

echo "=== Client Log ==="
cat client.log

# Check for expected patterns in logs
SUCCESS=true

echo "=== Validation ==="

# Check that StartOrder messages were received
if grep -q "StartOrder Message sent to Server" client.log; then
    echo "✓ StartOrder messages sent successfully"
else
    echo "✗ No StartOrder messages found in client log"
    SUCCESS=false
fi

# Check that OrderSaga was created and processed
if grep -q "OrderSaga" server.log; then
    echo "✓ OrderSaga processing detected"
else
    echo "✗ No OrderSaga processing found in server log"
    SUCCESS=false
fi

# Check for CosmosDB persistence activity
if grep -q "partition key" server.log; then
    echo "✓ CosmosDB partition key extraction working"
else
    echo "✗ No CosmosDB partition key activity found"
    SUCCESS=false
fi

# Check for timeout/completion events
if grep -q "Saga.*completed\|OrderCompleted" server.log || grep -q "OrderCompleted" client.log; then
    echo "✓ Order completion events detected"
else
    echo "✗ No order completion events found"
    SUCCESS=false
fi

# Final result
if [ "$SUCCESS" = true ]; then
    echo "=== SUCCESS: CosmosDB Transactions sample validation passed ==="
    exit 0
else
    echo "=== FAILURE: CosmosDB Transactions sample validation failed ==="
    exit 1
fi

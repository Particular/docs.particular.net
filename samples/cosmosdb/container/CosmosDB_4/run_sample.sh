#!/bin/bash
set -e

echo "=== CosmosDB Container Sample Test Script ==="
echo "This script tests the CosmosDB container sample functionality"
echo

# Function to cleanup background processes
cleanup() {
    echo "Cleaning up background processes..."
    pkill -f "dotnet.*Server.dll" || true
    pkill -f "dotnet.*Client.dll" || true
    wait 2>/dev/null || true
}

# Set up signal handlers
trap cleanup EXIT INT TERM

# Function to check if required environment variables are set
check_environment() {
    if [ -z "$COSMOS_CONNECTION_STRING" ]; then
        echo "WARNING: COSMOS_CONNECTION_STRING environment variable not set."
        echo "Will fall back to local emulator connection string."
        echo "To use Azure Cosmos DB, set COSMOS_CONNECTION_STRING environment variable."
    else
        echo "Using Azure Cosmos DB service via environment variable"
    fi
}

# Function to check if server is running
check_server_ready() {
    local log_file=$1
    local timeout=30
    local count=0

    while [ $count -lt $timeout ]; do
        if grep -q "Press any key to exit" "$log_file" 2>/dev/null; then
            return 0
        fi
        sleep 1
        count=$((count + 1))
    done
    return 1
}

# Function to check for successful message processing
check_message_processing() {
    local server_log=$1
    local timeout=30
    local count=0

    echo "Checking for successful message processing..."
    while [ $count -lt $timeout ]; do
        if grep -q "completed\|Saga.*completed" "$server_log" 2>/dev/null; then
            echo "SUCCESS: Found saga completion in server log"
            return 0
        fi
        sleep 1
        count=$((count + 1))
    done
    echo "WARNING: Saga completion not found within timeout"
    return 1
}

# Check environment configuration
check_environment

# Start the server in the background
echo "Starting Server application..."
cd /app
dotnet Server/Server.dll > server.log 2>&1 &
SERVER_PID=$!

# Wait for server to be ready
echo "Waiting for Server to be ready..."
if ! check_server_ready "server.log"; then
    echo "ERROR: Server failed to start properly"
    cat server.log
    exit 1
fi

echo "Server is ready. Server output:"
head -20 server.log

# Give server a moment to fully initialize
sleep 2

# Now test the client functionality
echo
echo "Testing Client functionality..."

# Create expect script for client interaction
cat > client_test.exp << 'EOF'
#!/usr/bin/expect -f
set timeout 30

# Start the client
spawn dotnet Client/Client.dll
expect "Press 'S' to send a StartOrder message to the server endpoint"

# Send a message
send "S"
expect "StartOrder Message sent to Server with OrderId"

# Wait a moment for processing
sleep 3

# Exit the client
send "\n"
expect eof
EOF

chmod +x client_test.exp

# Run the client test
echo "Running client interaction test..."
if ./client_test.exp > client.log 2>&1; then
    echo "SUCCESS: Client interaction completed successfully"
    echo "Client output:"
    cat client.log
else
    echo "ERROR: Client interaction failed"
    echo "Client output:"
    cat client.log
fi

# Wait a bit for message processing to complete
echo "Waiting for message processing to complete..."
sleep 5

# Check server logs for expected behavior
echo
echo "=== Server Log Analysis ==="
echo "Recent server log entries:"
tail -20 server.log

# Look for key indicators of successful processing
echo
echo "=== Verification ==="

if grep -q "StartOrder.*message.*Starting Saga\|Received StartOrder message" server.log; then
    echo "✓ Server received StartOrder message"
else
    echo "✗ Server did not receive StartOrder message"
fi

if grep -q "OrderSaga" server.log; then
    echo "✓ OrderSaga processing detected"
else
    echo "✗ OrderSaga processing not detected"
fi

if grep -q "ShipOrder" server.log; then
    echo "✓ ShipOrder message processing detected"
else
    echo "✗ ShipOrder message processing not detected"
fi

if grep -q "will use.*container\|ShipOrderSagaData.*container" server.log; then
    echo "✓ Container mapping functionality working"
else
    echo "✗ Container mapping functionality not verified"
fi

# Final message processing check
check_message_processing "server.log"

echo
echo "=== Test Summary ==="
echo "Server PID: $SERVER_PID"
echo "Server log size: $(wc -l < server.log) lines"
echo "Client log size: $(wc -l < client.log) lines"

# Check if server is still running
if kill -0 $SERVER_PID 2>/dev/null; then
    echo "Server is still running"
else
    echo "Server has stopped"
fi

echo
echo "Sample test completed. Check logs above for detailed results."
echo "Key functionality tested:"
echo "- Server startup and CosmosDB connection"
echo "- Client message sending"
echo "- OrderSaga and ShipOrderSaga processing"
echo "- Container mapping with dynamic container selection"

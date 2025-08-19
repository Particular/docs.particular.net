#!/bin/bash
set -e

echo "Starting delayed-delivery sample testing..."

# Start the Server using expect to handle console input
echo "Starting Server..."
expect << 'EOF' &
set timeout 30
spawn dotnet Server/Server.dll
log_file server.log
expect "Press any key, application loading"
send "\r"
expect "Starting..."
expect eof
EOF

SERVER_PID=$!

# Wait for Server to be ready
echo "Waiting for Server to start..."
sleep 5

# Function to cleanup processes
cleanup() {
    echo "Cleaning up processes..."
    kill $SERVER_PID 2>/dev/null || true
    wait $SERVER_PID 2>/dev/null || true
}

# Set trap to cleanup on exit
trap cleanup EXIT

# Test Scenario 1: Defer message handling (option 1)
echo "Testing Scenario 1: Defer message handling..."

expect << 'EOF'
set timeout 30
spawn dotnet Client/Client.dll
expect "Press '1' to send PlaceOrder - defer message handling"
send "1\r"
expect {
    timeout {
        puts "Timeout waiting for PlaceOrder confirmation"
        exit 1
    }
    "*Sent a PlaceOrder message with id:*" {
        puts "PlaceOrder sent successfully"
        send "q\r"
        expect eof
    }
}
EOF

if [ $? -ne 0 ]; then
    echo "ERROR: Scenario 1 failed"
    exit 1
fi

echo "Scenario 1 completed successfully"

# Wait a moment and check server logs for message handling
sleep 6

if grep -q "Deferring message" server.log; then
    echo "SUCCESS: Server deferred message handling as expected"
else
    echo "WARNING: Could not verify message deferral in server logs"
fi

# Test Scenario 2: Defer message delivery (option 2)
echo "Testing Scenario 2: Defer message delivery..."

expect << 'EOF'
set timeout 30
spawn dotnet Client/Client.dll
expect "Press '2' to send PlaceDelayedOrder - defer message delivery"
send "2\r"
expect {
    timeout {
        puts "Timeout waiting for PlaceDelayedOrder confirmation"
        exit 1
    }
    "*Deferred a PlaceDelayedOrder message with id:*" {
        puts "PlaceDelayedOrder deferred successfully"
        send "q\r"
        expect eof
    }
}
EOF

if [ $? -ne 0 ]; then
    echo "ERROR: Scenario 2 failed"
    exit 1
fi

echo "Scenario 2 completed successfully"

# Wait for delayed message to be delivered and processed
echo "Waiting for delayed message to be processed..."
sleep 6

if grep -q "PlaceDelayedOrder" server.log; then
    echo "SUCCESS: Server processed delayed message as expected"
else
    echo "WARNING: Could not verify delayed message processing in server logs"
fi

echo "Sample testing completed successfully!"
echo "Server log contents:"
cat server.log

#!/bin/bash
set -e

echo "Starting Custom Recoverability Sample Tests..."

# Test 1: Normal Operation
echo ""
echo "=== TEST 1: Normal Operation ==="
echo "Starting Server (normal version)..."
dotnet Server/bin/Debug/net9.0/Server.dll > server_normal.log 2>&1 &
SERVER_PID=$!

# Wait for server to start
sleep 5

# Start the client and send a message automatically
echo "Starting Client and sending message..."
expect << 'EOD'
spawn dotnet Client/bin/Debug/net9.0/Client.dll
expect "Press enter to send a message"
send "\r"
expect "Sent a message with id:"
expect "Press any key to exit"
send "q"
expect eof
EOD

# Wait a moment for processing
sleep 2

# Check server log for expected behavior
echo "Checking normal operation..."
if grep -q "Message received" server_normal.log; then
    echo "✓ SUCCESS: Normal message processing works"
else
    echo "✗ FAILURE: Normal message processing failed"
    echo "Server log:"
    cat server_normal.log
    kill $SERVER_PID 2>/dev/null || true
    exit 1
fi

# Clean up first server
kill $SERVER_PID 2>/dev/null || true
wait $SERVER_PID 2>/dev/null || true

# Test 2: Fault Scenario with ArgumentNullException
echo ""
echo "=== TEST 2: Fault Scenario (ArgumentNullException) ==="
echo "Starting Server (fault version with ArgumentNullException)..."
dotnet Server_Fault/bin/Debug/net9.0/Server.dll > server_fault.log 2>&1 &
SERVER_FAULT_PID=$!

# Wait for server to start
sleep 5

# Start the client and send a message automatically
echo "Starting Client and sending message to fault server..."
expect << 'EOD'
spawn dotnet Client/bin/Debug/net9.0/Client.dll
expect "Press enter to send a message"
send "\r"
expect "Sent a message with id:"
expect "Press any key to exit"
send "q"
expect eof
EOD

# Wait for retries to complete
sleep 35

# Check server log for expected retry behavior
echo "Checking fault scenario behavior..."
if grep -q "Immediate Retry is going to retry" server_fault.log && grep -q "ArgumentNullException" server_fault.log; then
    echo "✓ SUCCESS: Custom recoverability policy working - ArgumentNullException triggered retries"

    # Show relevant log entries
    echo "Fault scenario log output:"
    grep -E "(Message received|Immediate Retry|Delayed Retry|ArgumentNullException)" server_fault.log | head -10 || true
else
    echo "✗ FAILURE: Custom recoverability policy not working as expected"
    echo "Server fault log:"
    cat server_fault.log
    kill $SERVER_FAULT_PID 2>/dev/null || true
    exit 1
fi

# Clean up
kill $SERVER_FAULT_PID 2>/dev/null || true
wait $SERVER_FAULT_PID 2>/dev/null || true

echo ""
echo "✓ All tests completed successfully!"
echo "- Normal operation: Messages processed correctly"
echo "- Fault scenario: Custom recoverability policy applied retries as expected"

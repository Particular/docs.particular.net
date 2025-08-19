#!/bin/bash

# Script to test the pubsub/native sample behavior
# Validates all scenarios described in the sample.md file

set -e

echo "Starting pubsub/native sample test..."
echo "======================================"

# Create a shared directory for LearningTransport
mkdir -p /tmp/learningtransport

# Start the subscriber in the background
echo "Starting Subscriber..."
dotnet /app/subscriber/Subscriber.dll > subscriber.log 2>&1 &
SUBSCRIBER_PID=$!

# Wait for subscriber to start
echo "Waiting for subscriber to start..."
sleep 5

# Function to cleanup processes
cleanup() {
    echo "Cleaning up processes..."
    kill $SUBSCRIBER_PID 2>/dev/null || true
    wait $SUBSCRIBER_PID 2>/dev/null || true
}

# Set trap to cleanup on exit
trap cleanup EXIT

# Test 1: Normal operation - publish messages using expect
echo "Test 1: Publishing messages with subscriber running..."

# Use expect to interact with the publisher - first message
expect << 'EOF'
set timeout 30
spawn dotnet /app/publisher/Publisher.dll
expect {
    "Press '1' to publish the OrderReceived event" {
        send "1\r"
        expect "Published OrderReceived Event with Id"
        send "x\r"
        expect eof
    }
    timeout {
        puts "Timeout waiting for publisher"
        exit 1
    }
}
EOF

if [ $? -ne 0 ]; then
    echo "✗ Test 1 FAILED: First publisher interaction failed"
    exit 1
fi

# Second message
expect << 'EOF'
set timeout 30
spawn dotnet /app/publisher/Publisher.dll
expect {
    "Press '1' to publish the OrderReceived event" {
        send "1\r"
        expect "Published OrderReceived Event with Id"
        send "x\r"
        expect eof
    }
    timeout {
        puts "Timeout waiting for publisher"
        exit 1
    }
}
EOF

if [ $? -ne 0 ]; then
    echo "✗ Test 1 FAILED: Publisher interaction failed"
    exit 1
fi

# Wait for message processing
echo "Waiting for message processing..."
sleep 3

# Check if subscriber received the messages
MESSAGE_COUNT=$(grep -c "Subscriber has received OrderReceived event" subscriber.log || echo "0")
echo "Subscriber received $MESSAGE_COUNT messages"

if [ "$MESSAGE_COUNT" -ge 2 ]; then
    echo "✓ Test 1 PASSED: Subscriber received published messages"
else
    echo "✗ Test 1 FAILED: Expected at least 2 messages, got $MESSAGE_COUNT"
    echo "Subscriber log:"
    cat subscriber.log
    exit 1
fi

# Test 2: Fault tolerance - Stop subscriber, publish messages, restart subscriber
echo ""
echo "Test 2: Testing fault tolerance..."

# Stop the subscriber
echo "Stopping subscriber to test fault tolerance..."
kill $SUBSCRIBER_PID 2>/dev/null || true
wait $SUBSCRIBER_PID 2>/dev/null || true

# Clear previous logs
> subscriber.log

# Publish messages while subscriber is down
echo "Publishing messages while subscriber is down..."

# First message while subscriber is down
expect << 'EOF'
set timeout 30
spawn dotnet /app/publisher/Publisher.dll
expect {
    "Press '1' to publish the OrderReceived event" {
        send "1\r"
        expect "Published OrderReceived Event with Id"
        send "x\r"
        expect eof
    }
    timeout {
        puts "Timeout during fault tolerance test"
        exit 1
    }
}
EOF

# Second message while subscriber is down
expect << 'EOF'
set timeout 30
spawn dotnet /app/publisher/Publisher.dll
expect {
    "Press '1' to publish the OrderReceived event" {
        send "1\r"
        expect "Published OrderReceived Event with Id"
        send "x\r"
        expect eof
    }
    timeout {
        puts "Timeout during fault tolerance test"
        exit 1
    }
}
EOF

# Third message while subscriber is down
expect << 'EOF'
set timeout 30
spawn dotnet /app/publisher/Publisher.dll
expect {
    "Press '1' to publish the OrderReceived event" {
        send "1\r"
        expect "Published OrderReceived Event with Id"
        send "x\r"
        expect eof
    }
    timeout {
        puts "Timeout during fault tolerance test"
        exit 1
    }
}
EOF

if [ $? -ne 0 ]; then
    echo "✗ Test 2 FAILED: Publisher interaction during fault tolerance test failed"
    exit 1
fi

echo "Waiting before restarting subscriber..."
sleep 2

# Restart the subscriber
echo "Restarting subscriber..."
dotnet /app/subscriber/Subscriber.dll > subscriber.log 2>&1 &
SUBSCRIBER_PID=$!

# Wait for subscriber to process queued messages
echo "Waiting for subscriber to process queued messages..."
sleep 10

# Check if subscriber received the queued messages
MESSAGE_COUNT=$(grep -c "Subscriber has received OrderReceived event" subscriber.log || echo "0")
echo "Subscriber received $MESSAGE_COUNT messages from queue"

if [ "$MESSAGE_COUNT" -ge 3 ]; then
    echo "✓ Test 2 PASSED: Fault tolerance working - subscriber received queued messages"
else
    echo "✗ Test 2 FAILED: Expected at least 3 messages, got $MESSAGE_COUNT"
    echo "Subscriber log:"
    cat subscriber.log
    exit 1
fi

echo ""
echo "========================================="
echo "All tests completed successfully!"
echo "Sample behavior matches expected behavior from sample.md"
echo "========================================="
echo ""
echo "Final subscriber log:"
echo "===================="
cat subscriber.log

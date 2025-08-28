#!/bin/bash

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

echo -e "${YELLOW}Starting Consumer-Driven Contracts Sample${NC}"

# Create logs directory
mkdir -p /app/logs

# Start Consumer1 in background
echo -e "${YELLOW}Starting Consumer1...${NC}"
dotnet run --project Consumer1 --framework net10.0 > /app/logs/consumer1.log 2>&1 &
CONSUMER1_PID=$!

# Start Consumer2 in background
echo -e "${YELLOW}Starting Consumer2...${NC}"
dotnet run --project Consumer2 --framework net10.0 > /app/logs/consumer2.log 2>&1 &
CONSUMER2_PID=$!# Wait for consumers to initialize by checking log files
echo -e "${YELLOW}Waiting for consumers to initialize...${NC}"
timeout=30
counter=0

while [ $counter -lt $timeout ]; do
    if grep -q "Press Ctrl+C to shut down" /app/logs/consumer1.log 2>/dev/null && \
       grep -q "Press Ctrl+C to shut down" /app/logs/consumer2.log 2>/dev/null; then
        break
    fi
    sleep 1
    counter=$((counter + 1))
done

if [ $counter -eq $timeout ]; then
    echo -e "${RED}Timeout: Consumers failed to initialize within ${timeout} seconds${NC}"
    echo -e "${YELLOW}Consumer1 log:${NC}"
    cat /app/logs/consumer1.log 2>/dev/null || echo "No log found"
    echo -e "${YELLOW}Consumer2 log:${NC}"
    cat /app/logs/consumer2.log 2>/dev/null || echo "No log found"
    exit 1
fi

echo -e "${GREEN}Consumers started successfully${NC}"

# Create expect script for automated Producer interaction
cat > /app/producer_automation.exp << 'EXPECT_EOF'
#!/usr/bin/expect

set timeout 30
spawn dotnet run --project Producer --framework net10.0

# Wait for the prompt
expect "Press 'p' to publish event"

# Send 'p' to publish
send "p\r"

# Wait a moment for the message to be published
sleep 2

# Send any other key to exit
send "q\r"

expect eof
EXPECT_EOF

chmod +x /app/producer_automation.exp

# Run the Producer with automated input
echo -e "${YELLOW}Starting Producer and publishing event...${NC}"
/app/producer_automation.exp > /app/logs/producer.log 2>&1

# Wait for message processing
echo -e "${YELLOW}Waiting for message processing...${NC}"
sleep 5

# Check logs for expected behavior
echo -e "${YELLOW}Verifying results...${NC}"

CONSUMER1_SUCCESS=false
CONSUMER2_SUCCESS=false

# Check Consumer1 log for Consumer1Info
if grep -q "Consumer1Info" /app/logs/consumer1.log; then
    echo -e "${GREEN}✓ Consumer1 successfully received and processed Consumer1Contract${NC}"
    CONSUMER1_SUCCESS=true
else
    echo -e "${RED}✗ Consumer1 did not process Consumer1Contract${NC}"
fi

# Check Consumer2 log for Consumer2Info
if grep -q "Consumer2Info" /app/logs/consumer2.log; then
    echo -e "${GREEN}✓ Consumer2 successfully received and processed Consumer2Contract${NC}"
    CONSUMER2_SUCCESS=true
else
    echo -e "${RED}✗ Consumer2 did not process Consumer2Contract${NC}"
fi

# Show log outputs
echo -e "\n${YELLOW}=== Consumer1 Log ===${NC}"
cat /app/logs/consumer1.log

echo -e "\n${YELLOW}=== Consumer2 Log ===${NC}"
cat /app/logs/consumer2.log

echo -e "\n${YELLOW}=== Producer Log ===${NC}"
cat /app/logs/producer.log

# Cleanup processes
kill $CONSUMER1_PID $CONSUMER2_PID 2>/dev/null || true

# Final result
if $CONSUMER1_SUCCESS && $CONSUMER2_SUCCESS; then
    echo -e "\n${GREEN}🎉 Sample completed successfully! Both consumers received their respective contracts.${NC}"
    exit 0
else
    echo -e "\n${RED}❌ Sample failed - not all consumers received their expected messages.${NC}"
    exit 1
fi

tmux \
  new-session   'dotnet run --project Billing --connectionstring "host=." --rabbitmq' \; \
  split-window  'dotnet run --project Sales --connectionstring "host=." --rabbitmq' \; \
  split-window  'dotnet run --project ClientUI --connectionstring "host=." --rabbitmq' \; \
  split-window  'dotnet run --project Shipping --connectionstring "host=." --rabbitmq' \; \
  select-layout even-vertical

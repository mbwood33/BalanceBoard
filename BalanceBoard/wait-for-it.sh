#!/bin/bash

# Set default timeout and quiet mode
TIMEOUT=15
QUIET=""

# Function to check if a service is available
check_service() {
  host="$1"
  port="$2"
  
  # Attempt to connect to the service
  nc -z "$host" "$port" > /dev/null 2>&1
  return $?
}

# Parse command line arguments
while [[ $# -gt 0 ]]; do
  case "$1" in
    -t|--timeout)
      TIMEOUT="$2"
      shift 2
      ;;
    -q|--quiet)
      QUIET="true"
      shift
      ;;
    -h|--help)
      echo "Usage: wait-for-it [-t|--timeout seconds] [-q|--quiet] host:port [command]"
      exit 0
      ;;
    *)
      SERVICES+=("$1")
      shift
      ;;
  esac
done

# Check if services are provided
if [[ -z "${SERVICES[@]}" ]]; then
  echo "Error: No services specified."
  exit 1
fi

# Wait for services to become available
start_time=$(date +%s)
while true; do
  all_ok=true
  for service in "${SERVICES[@]}"; do
    host=$(echo "$service" | cut -d':' -f1)
    port=$(echo "$service" | cut -d':' -f2)
    if ! check_service "$host" "$port"; then
      all_ok=false
      [[ -z "$QUIET" ]] && echo "Waiting for $service..."
      break
    fi
  done
  if $all_ok; then
      [[ -z "$QUIET" ]] && echo "All services are available."
      break
  fi
  current_time=$(date +%s)
  if ((current_time - start_time > TIMEOUT)); then
      echo "Timeout reached, services did not become available."
      exit 1
  fi
  sleep 1
done

# Execute the command if provided
if [[ $# -gt 0 ]]; then
  "$@"
fi
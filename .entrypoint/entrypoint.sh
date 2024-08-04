#!/bin/bash
set -e

# Wait for SQL Server to be available
./.entrypoint/wait-for-it.sh sqlserver:1433 -- echo "SQL Server is up"

# Apply all pending database migrations (no harm if already applied)
echo "Applying all pending migrations..."
dotnet ef database update

tail -f /dev/null

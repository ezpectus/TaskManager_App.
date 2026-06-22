#!/bin/bash

echo "============================================"
echo "  Task Manager App - Starting..."
echo "============================================"
echo ""

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PROJECT_DIR="$SCRIPT_DIR/TaskManagerProject"

echo "[1/4] Restoring NuGet packages..."
cd "$PROJECT_DIR" || exit 1
dotnet restore
if [ $? -ne 0 ]; then
    echo "ERROR: NuGet restore failed."
    exit 1
fi
echo ""

echo "[2/4] Building solution..."
dotnet build --no-restore
if [ $? -ne 0 ]; then
    echo "ERROR: Build failed."
    exit 1
fi
echo ""

echo "[3/4] Starting backend API (http://localhost:5000)..."
cd "$PROJECT_DIR/TaskManager.API" || exit 1
dotnet run --no-build &
BACKEND_PID=$!
echo "Backend PID: $BACKEND_PID"

echo "[4/4] Starting frontend (http://localhost:3000)..."
cd "$PROJECT_DIR/frontend" || exit 1
if [ ! -d "node_modules" ]; then
    echo "Installing frontend dependencies..."
    npm install
fi
npm run dev &
FRONTEND_PID=$!
echo "Frontend PID: $FRONTEND_PID"

echo ""
echo "============================================"
echo "  Both servers are starting!"
echo "  Backend:  http://localhost:5000"
echo "  Frontend: http://localhost:3000"
echo "  Swagger:  http://localhost:5000/swagger"
echo "============================================"
echo ""
echo "Press Ctrl+C to stop both servers."

cleanup() {
    echo ""
    echo "Shutting down servers..."
    kill $BACKEND_PID 2>/dev/null
    kill $FRONTEND_PID 2>/dev/null
    echo "Done."
    exit 0
}

trap cleanup SIGINT SIGTERM
wait

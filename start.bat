@echo off
echo ============================================
echo   Task Manager App - Starting...
echo ============================================
echo.

echo [1/4] Restoring NuGet packages...
cd /d "%~dp0TaskManagerProject"
dotnet restore
if %errorlevel% neq 0 (
    echo ERROR: NuGet restore failed.
    pause
    exit /b 1
)
echo.

echo [2/4] Building solution...
dotnet build --no-restore
if %errorlevel% neq 0 (
    echo ERROR: Build failed.
    pause
    exit /b 1
)
echo.

echo [3/4] Starting backend API (http://localhost:5000)...
start "TaskManager API" cmd /k "cd /d %~dp0TaskManagerProject\TaskManager.API && dotnet run --no-build"

echo [4/4] Starting frontend (http://localhost:3000)...
cd /d "%~dp0TaskManagerProject\frontend"
if not exist "node_modules" (
    echo Installing frontend dependencies...
    call npm install
)
start "TaskManager Frontend" cmd /k "cd /d %~dp0TaskManagerProject\frontend && npm run dev"

echo.
echo ============================================
echo   Both servers are starting!
echo   Backend:  http://localhost:5000
echo   Frontend: http://localhost:3000
echo   Swagger:  http://localhost:5000/swagger
echo ============================================
echo.
echo Test commands:
echo   Backend tests:  dotnet test TaskManagerProject\TaskManagerSolution.sln
echo   Frontend tests: cd TaskManagerProject\frontend ^&^& npx vitest run
echo   Type check:     cd TaskManagerProject\frontend ^&^& npx tsc --noEmit
echo.
echo Close both command windows to stop the servers.
pause

@echo off
echo Testing Nuget packing
powershell -ExecutionPolicy ByPass -NoProfile -command "& """%~dp0..\eng\build.ps1""" -pack -c Release -officialSourceBranchName releases/V1.0.x -officialBuildId 1234.5 /p:OfficialBuild=true"
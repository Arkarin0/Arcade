@echo off
echo Testing: releases
powershell -ExecutionPolicy ByPass -NoProfile -command "& """%~dp0..\eng\publish-assets.ps1""" -configuration Release -releaseName V1.0.X -test"


echo Testing: main
powershell -ExecutionPolicy ByPass -NoProfile -command "& """%~dp0..\eng\publish-assets.ps1""" -configuration Release -branchName main -test"
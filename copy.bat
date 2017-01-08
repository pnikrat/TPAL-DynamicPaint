@ECHO OFF
SET OriginPath="%~dp0\PluginFill\bin\Debug\PluginFill.dll"
SET DestinPath="%~dp0\Dynamic Paint\bin\Debug\plugins\PluginFill.dll"

COPY %OriginPath% %DestinPath% /Y
pause
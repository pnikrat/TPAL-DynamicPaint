@ECHO OFF
SET OriginPath="%~dp0\PluginResize\bin\Debug\PluginResize.dll"
SET DestinPath="%~dp0\Dynamic Paint\bin\Debug\plugins\PluginResize.dll"

COPY %OriginPath% %DestinPath% /Y
pause
@ECHO OFF
SET OriginPath="%~dp0\PluginResize\bin\Debug\PluginResize.dll"
SET DestinPath="%~dp0\Dynamic Paint\bin\Debug\plugins\PluginResize.dll"

SET OriginPathSecond="%~dp0\PluginImage\bin\Debug\PluginImage.dll"
SET DestinPathSecond="%~dp0\Dynamic Paint\bin\Debug\plugins\PluginImage.dll"
COPY %OriginPath% %DestinPath% /Y
COPY %OriginPathSecond% %DestinPathSecond% /Y
pause
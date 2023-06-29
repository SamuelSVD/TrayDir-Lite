"C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" ..\TrayDirLite\TrayDirLite.csproj /property:Configuration=Release
"C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" ..\TrayDirLite\TrayDirLite.csproj /property:Configuration=Release(x64)

"C:\Program Files (x86)\Inno Setup 6\ISCC.exe" installer.iss

mkdir _Temp_
mkdir _Temp_\TrayDirLite_32bit
mkdir _Temp_\TrayDirLite_64bit

xcopy ..\TrayDirLite\bin\Release _Temp_\TrayDirLite_32bit /E
xcopy ..\TrayDirLite\bin\Release(x64) _Temp_\TrayDirLite_64bit /E

del _Temp_\TrayDirLite_32bit\config.xml
del _Temp_\TrayDirLite_32bit\TrayDirLite.application
del _Temp_\TrayDirLite_32bit\TrayDirLite.exe.config
del _Temp_\TrayDirLite_32bit\TrayDirLite.exe.manifest
del _Temp_\TrayDirLite_32bit\TrayDirLite.pdb
del _Temp_\TrayDirLite_32bit\TrayDirLite.chm

rmdir _Temp_\TrayDirLite_32bit\app.publish /S /Q

del _Temp_\TrayDirLite_64bit\config.xml
del _Temp_\TrayDirLite_64bit\TrayDirLite.application
del _Temp_\TrayDirLite_64bit\TrayDirLite.exe.config
del _Temp_\TrayDirLite_64bit\TrayDirLite.exe.manifest
del _Temp_\TrayDirLite_64bit\TrayDirLite.pdb
del _Temp_\TrayDirLite_64bit\TrayDirLite.chm

rmdir _Temp_\TrayDirLite_64bit\app.publish /S /Q

cd _Temp_\TrayDirLite_32bit\
"C:\Program Files\7-Zip\7z.exe" a ..\..\bin\TrayDirLite_32bit.zip "*"
cd ..\..

cd _Temp_\TrayDirLite_64bit\
"C:\Program Files\7-Zip\7z.exe" a ..\..\bin\TrayDirLite_64bit.zip "*"
cd ..\..

rmdir _Temp_ /S /Q
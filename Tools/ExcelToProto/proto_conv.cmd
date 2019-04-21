@IF NOT EXIST m:\ (
@ECHO 未设置m盘
@PAUSE
@EXIT
)

@SET CSC20=c:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe
@echo 目前语言参数 -ch 简体中文, -en 英文, -ct繁体中文, -ko 韩文
@echo 系统准备导出配置信息

set /p language=请输入语言参数

DEL code\*.* /Q
DEL protodata\*.* /Q

proto_conv\bin\Release\proto_conv.exe %language% 关卡表.xls



@pause
DEL m:\Assets\Lib\ClientProto\*.* /Q
DEL m:\Assets\Resources\Config\ClientProto\*.* /Q
@FOR %%P IN (code\*) DO xcopy %%P m:\Assets\Lib\ClientProto\ /Y /Q
@FOR %%P IN (protodata\*) DO xcopy %%P m:\Assets\Resources\Config\ClientProto\ /Y /Q
	
@pause

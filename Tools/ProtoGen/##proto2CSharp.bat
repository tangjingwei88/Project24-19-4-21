echo on

set Path=protogen.exe

%Path% -i:protoFile\test.proto -o:CsharpFile\test.cs
%Path% -i:protoFile\test2.proto -o:CsharpFile\test2.cs

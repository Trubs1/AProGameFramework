echo off

rmdir /s /q ..\Assets\LuaFramework\ToLua\Source\Generate

mkdir ..\Assets\LuaFramework\ToLua\Source\Generate

copy DelegateFactory.cs ..\Assets\LuaFramework\ToLua\Source\Generate\DelegateFactory.cs
copy LuaBinder.cs ..\Assets\LuaFramework\ToLua\Source\Generate\LuaBinder.cs

echo ������ɣ����Unity����ִ��lua/clear wrap files���ʣ��Ĳ���
pause
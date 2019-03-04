echo off

rmdir /s /q ..\Assets\LuaFramework\ToLua\Source\Generate

mkdir ..\Assets\LuaFramework\ToLua\Source\Generate

copy DelegateFactory.cs ..\Assets\LuaFramework\ToLua\Source\Generate\DelegateFactory.cs
copy LuaBinder.cs ..\Assets\LuaFramework\ToLua\Source\Generate\LuaBinder.cs

echo 处理完成，请打开Unity工程执行lua/clear wrap files完成剩余的步骤
pause
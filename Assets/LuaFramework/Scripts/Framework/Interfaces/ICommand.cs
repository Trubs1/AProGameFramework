/* 
 LuaFramework Code By Jarjin lee
*/
using System;
namespace LuaFramework
{
    public interface ICommand
    {
        void Execute(IMessage message);
    }

}

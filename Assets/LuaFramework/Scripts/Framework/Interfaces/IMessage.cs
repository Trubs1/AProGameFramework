/* 
 LuaFramework Code By Jarjin lee 
*/
using System;
namespace LuaFramework
{
    public interface IMessage
    {
        string Name { get; }

        object Body { get; set; }

        string Type { get; set; }

        string ToString();
    }
}

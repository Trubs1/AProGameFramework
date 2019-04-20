using System;
namespace LuaFramework
{
    public interface IView
    {
        void OnMessage(IMessage message);
    }
}
// Description:引擎的扩展
// Author:WangQiang
// Date:2019/03/25

using UnityEngine;

public static class UnityExtension
{
    public static Transform FindTransInChildren(this Transform trans, string childName)
    {
        Transform childTrans = trans.Find(childName);
        if (childTrans != null) return childTrans;

        for (int i = 0; i < trans.childCount; i++)
        {
            childTrans = FindTransInChildren(trans.GetChild(i), childName);
            if (childTrans != null) return childTrans;
        }
        return null;
    }
    public static T GetOrAddComponet<T>(this Transform trans) where T : Component
    {
        T compnt = trans.GetComponent<T>();
        if (null == compnt)
            compnt = trans.gameObject.AddComponent<T>();
        return compnt;
    }

    /// <summary>
    /// 首字母小写
    /// </summary>
    public static string InitialToLower(this string str)
    {
        if (str.Length < 0) return string.Empty;
        var firstC = str[0];
        if ('Z' >= firstC && 'A' <= firstC )
            str = (char)(firstC + 32) + str.Substring(1);
        return str;
    }

    /// <summary>
    /// 首字母大写
    /// </summary>
    public static string InitialToUpper(this string str)
    {
        if (str.Length < 0) return string.Empty;
        //System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"^[A-Za-z0-9]+$");
        var firstC = str[0];
        if ('a' <= firstC && 'z' <= firstC)
            str = (char)(firstC - 32) + str.Substring(1);
        return str;
    }
}

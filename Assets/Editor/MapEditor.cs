/*--------------------------------------------------------
Name:       地图编辑器
Function:   Unity里地图打点,导出各种对象的Json格式的坐标数据
Author:     WangQiang
Date:       2018/06/11
*///------------------------------------------------------
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using MiniJSON;

/// <summary>  
/// 地图编辑器
/// </summary>  
public class MapEditor : EditorWindow
{
    private Dictionary<string, Dictionary<string, object>> mapData = new Dictionary<string, Dictionary<string, object>>();
    private const string EDITOR_VERSION = "v0.04";  // 这个编辑器的版本号
    private Vector2 m_scrollPos;                    // 记录 gui 界面的滚动
    public Transform mapRoot;


    Dictionary<string, object> oldMapData;

    /// <summary>  
    /// 数据目录  
    /// </summary>  
    static string AppDataPath
    {
        get { return Application.dataPath.ToLower(); }
    }

    [MenuItem("Tools/MapEditor")]
    static void Init()
    {
        Debug.Log("初始化转化MapEditor");
        // Get existing open window or if none, make a new one:  
        MapEditor window = (MapEditor)GetWindow(typeof(MapEditor));
    }

    // UI 按钮显示 //  
    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        m_scrollPos = GUILayout.BeginScrollView(m_scrollPos, GUILayout.Width(Screen.width), GUILayout.Height(Screen.height));
        GUILayout.BeginVertical(); // begin  

        GUILayout.Space(20);
        mapRoot = (Transform)EditorGUILayout.ObjectField("mapRoot", mapRoot, typeof(Transform), true);
        GUILayout.Space(100);

        if (GUILayout.Button("\n 导出所有点的坐标 \n"))
        {
            //oldMapData = GetOldMapData();
            this.Export();
        }
        GUILayout.Space(20);
        GUILayout.Label("工具版本号: " + EDITOR_VERSION);

        GUILayout.EndVertical(); // end  
        GUILayout.EndScrollView();
        GUILayout.EndHorizontal();
    }

    private Dictionary<string, object> GetOldMapData()
    {
        string fullPath = string.Format("{0}/Resources/Battle/Data/Invariant/{1}.txt", AppDataPath, mapRoot.name);
        StreamReader sr = File.OpenText(fullPath);
        string str = sr.ReadToEnd();
        sr.Close();
        Debug.Log("str:::" + str);

        return Json.Deserialize(str) as Dictionary<string, object>;
    }

    void FillMapData(Transform trans)
    {
        object obj;
        if (oldMapData.TryGetValue(trans.gameObject.name, out obj))
        {
            var temp = obj as Dictionary<string, object>;
            Debug.Log("eulerAngles:  " + temp["eulerAngles"]);
            temp["position"] = trans.position;
            temp["eulerAngles"] = trans.eulerAngles;
            temp["functionType"] = trans.parent.gameObject.name;

        }
    }

private void DoWritePosData(Transform child, Dictionary<string, object> dic)
    {
        for (int j = 0; j <= child.childCount - 1; j++)
        {
            Transform trans = child.GetChild(j);
            if (0 == trans.childCount)
            {
                //string[] strArr = pos.name.Split('_');//若没有关键字符,[0]为本身 (为了能递归创建，所以分割字符串放到运行逻辑里）
                dic[trans.position.ToString()+"_"+trans.eulerAngles.ToString()] = trans.name;//因为策划确定不会出现重复坐标点,所以可以用坐标点当key;否则可以用name+instanceId;
                //FillMapData(trans);
                //dic[pos.name] = pos.position;
            }
            else
            {
                dic[trans.name] = new Dictionary<string, object>();
                DoWritePosData(trans, (Dictionary<string, object>)dic[trans.name]);
             }
        }
    }

    private void WritePosData(Transform trans)
    {
        for (int i = 0; i <= trans.childCount - 1; i++)
        {
            Transform child = trans.GetChild(i);
            if (0 < child.childCount)
            {
                mapData[child.name] = new Dictionary<string, object>();
                DoWritePosData(child, mapData[child.name]);
            }
            else
            {
                Debug.Log("无效目标:" + child.name + child.position);
            }
        }
    }

    // 开始转化  
    private bool Export()
    {
        if (mapRoot == null || 0 == mapRoot.childCount)
        {
            Debug.LogError("导出失败 请检查mapRoot ");
            return false;
        }
        WritePosData(mapRoot);
        string str = Json.Serialize(mapData);
        Debug.Log("mapData:" + str);
        string filePath = string.Format("{0}/Resources/Battle/Data/{1}.txt", AppDataPath, mapRoot.name);
        FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate,FileAccess.ReadWrite);
        byte[] bts = Encoding.UTF8.GetBytes(str);
        fs.Write(bts,0, bts.Length);
        fs.Close();
        AssetDatabase.Refresh();
        Debug.Log(string.Format("<color=#36ADB8FF>############## 导出完成 ################\n存储路径:{0}</color>", filePath));
        return true;
    }
}


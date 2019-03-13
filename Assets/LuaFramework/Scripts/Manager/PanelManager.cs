///Copyright (c) 2019 WangQiang(279980661@qq.com)
///description: ��������
///author:Trubs (WQ)
///Date:2019/03/05

#define DirectLoadResourceMode //ֱ�Ӽ�����Դģʽ,uiԤ�ƵȽ�ֱ�Ӽ���,���㼴�ļ���


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using LuaInterface;


namespace LuaFramework
{
    public class PanelManager : Manager
    {

        public GameObject CreatePanelBySync(string panelName, LuaTable table)
        {
            GameObject prefab;

#if !DirectLoadResourceMode
                prefab = ResManager.LoadAssetSync<GameObject>("prefabs_ui_panels", panelName);
#else
            //Resources.LoadAssetAtPath��������,�������UnityEditor�µ�,�����ʱ��ᱨ��,���ƺ���û��Ӱ�������,��ʱ��������
            prefab = (GameObject)UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Prefabs/UI/Panels/" + panelName + ".prefab", typeof(GameObject));
#endif

            if (prefab == null)
            {
                Debug.LogError("~~~~CreatePanelSync faile::>> " + panelName + " " + prefab);
                return null;
            }
            GameObject go = Instantiate(prefab) as GameObject;
            go.name = panelName;
            go.layer = LayerMask.NameToLayer("Default");
            go.transform.SetParent(Parent);
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = Vector3.zero;
            var behaviour = go.GetComponent<LuaBehaviour>();
            behaviour.Initiate(table);
            return go;
        }


        private Transform parent;

        Transform Parent
        {
            get
            {
                if (parent == null)
                {
                    GameObject go = GameObject.Find("Canvas");
                    if (go != null) parent = go.transform;
                }
                return parent;
            }
        }

        public void ClosePanel(string name)
        {
            var panelName = name + "Panel";
            var panelObj = Parent.Find(panelName);
            if (panelObj == null) return;
            Destroy(panelObj.gameObject);
        }
    }
}
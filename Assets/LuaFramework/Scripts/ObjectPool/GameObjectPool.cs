using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace LuaFramework
{

    [Serializable]
    public class PoolInfo
    {
        public string poolName;
        public GameObject prefab;
        public int poolSize;
        public bool fixedSize;
    }

    public class GameObjectPool
    {
        private int maxSize;
        private int curPoolSize;
        private string poolName;
        private Transform poolRoot;
        private GameObject poolObjectPrefab;
        private Stack<GameObject> availableObjStack = new Stack<GameObject>();

        public GameObjectPool(string poolName, GameObject poolObjectPrefab, int initCount, int maxSize, Transform pool)
        {
            this.poolName = poolName;
            this.curPoolSize = initCount;
            this.maxSize = maxSize;
            this.poolRoot = pool;
            this.poolObjectPrefab = poolObjectPrefab;

            //populate the pool
            for (int index = 0; index < initCount; index++)
            {
                AddObjectToPool(NewObjectInstance());
            }
        }

        private void AddObjectToPool(GameObject go)
        {
            //不可扩展池 在此处删除多余的对象,若要改成可扩展池,注释本语句块
            if (availableObjStack.Count > maxSize)
            {
                GameObject.Destroy(go);
                //Debug.Log(string.Format("<color=yellow>AddObjectToPool 超出池计划的最大数量:{0}</color>", go));
                return;
            }
            go.SetActive(false);
            availableObjStack.Push(go);
            go.transform.SetParent(poolRoot, false);
        }

        private GameObject NewObjectInstance()
        {
            if (poolObjectPrefab != null)
            {
                return GameObject.Instantiate(poolObjectPrefab) as GameObject;
            }
            return null;
        }

        public GameObject NextAvailableObject()
        {
            GameObject go = null;
            if (availableObjStack.Count > 0)
            {
                go = availableObjStack.Pop();
            }
            else
            {
                go = NewObjectInstance();
                curPoolSize++;
                //if (curPoolSize > maxSize) maxSize = curPoolSize;//可扩展池
            }
            //if (go != null)
            //{
            //    go.SetActive(true);
            //}
            return go;
        }

        public void ReturnObjectToPool(string pool, GameObject po)
        {
            if (poolName.Equals(pool))
            {
                AddObjectToPool(po);
            }
            else
            {
                Debug.LogError(string.Format("Trying to add object to incorrect pool {0} ", poolName));
            }
        }

        public void DestoryPool()
        {
            while (availableObjStack.Count > 0)
            {
                GameObject.Destroy(availableObjStack.Pop());
            }
            availableObjStack.Clear();

            this.poolObjectPrefab = null;
        }
    }
}

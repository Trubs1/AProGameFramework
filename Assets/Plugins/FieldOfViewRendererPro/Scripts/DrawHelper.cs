// Description:图形绘制器 请将此脚本挂在MainCamera上
// Copyright (c) 2018 WangQiang(279980661@qq.com)
// Author:Trubs (WQ)
// Date:2018/08/08

using UnityEngine;
using System.Collections.Generic;
namespace TrubsDrawHelper
{
    /// <summary>
    /// 图形绘制器 请将此脚本挂在MainCamera上
    /// </summary>
    public class DrawHelper : MonoBehaviour
    {
        /// <summary>
        /// 显示的材质
        /// </summary>
        public Material mat;
        [HideInInspector]
        public static DrawHelper It;

        public float disMin = 0.4f;//视野显示的最新距离,即扇形的小r
        private List<Sector> allSectors = new List<Sector>();
        private Vector3 vertice;

        [Tooltip("框选的颜色")]
        public Color rectColor = new Color(1, 1, 0, 0.06f);
        private Vector2 startPos;
        private Vector2 endPos;
        private bool isBoxSelect = false;

        void Awake()
        {
            It = this;
            CheckMat();
            Debug.Log("DrawHelper Awake !");
        }

        //绘制入口 此函数由相机调用
        void OnPostRender()
        {
            if (isBoxSelect)
                BoxSelectRender();//若不需要绘制框选框 请注释此语句
            if (allSectors.Count > 0)
                DrawTriangle(allSectors);


            //绘制其他图形
        }
        void Update()
        {
            DealBoxSelect();//若不需要框选,屏蔽此行
        }
        void BoxSelectRender()
        {
            GL.PushMatrix();

            mat.SetInt("_ZWrite", 0);

            mat.SetPass(0);
            //GL.LoadOrtho();
            GL.LoadPixelMatrix();
            GL.Begin(GL.QUADS);
            GL.Color(rectColor);
            GL.Vertex3(startPos.x, startPos.y, 0);
            GL.Vertex3(endPos.x, startPos.y, 0);
            GL.Vertex3(endPos.x, endPos.y, 0);
            GL.Vertex3(startPos.x, endPos.y, 0);
            GL.End();

            GL.Begin(GL.LINES);
            GL.Color(Color.yellow);
            GL.Vertex3(startPos.x, startPos.y, 0);
            GL.Vertex3(endPos.x, startPos.y, 0);
            GL.Vertex3(endPos.x, startPos.y, 0);
            GL.Vertex3(endPos.x, endPos.y, 0);
            GL.Vertex3(endPos.x, endPos.y, 0);
            GL.Vertex3(startPos.x, endPos.y, 0);
            GL.Vertex3(startPos.x, endPos.y, 0);
            GL.Vertex3(startPos.x, startPos.y, 0);
            GL.End();
            GL.PopMatrix();
        }

        void DealBoxSelect()
        {
            if (Input.GetMouseButtonDown(0))
            {
                isBoxSelect = true;
                startPos = Input.mousePosition;
            }
            if (isBoxSelect)
            {
                endPos = Input.mousePosition;
                if (Input.GetMouseButtonUp(0))
                {
                    isBoxSelect = false;
                    ExchangeTwoPoint();
                    //SelectGameObject();
                }
            }
        }

        void ExchangeTwoPoint()
        {
            if (startPos.x > endPos.x)
            {
                float position1 = startPos.x;
                startPos.x = endPos.x;
                endPos.x = position1;
            }


            if (startPos.y > endPos.y)
            {
                float position2 = startPos.y;
                startPos.y = endPos.y;
                endPos.y = position2;
            }
        }


        //private static List<HeroView> unSeletedheroViews = new List<HeroView>();
        //void SelectGameObject()
        //{
        //    bool selectedListHadCleared = false;
        //    unSeletedheroViews.Clear();
        //    for (int i = 0; i < HeroView.heroViews.Count; i++)
        //    {
        //        var heroView = HeroView.heroViews[i];
        //        Vector3 position = Camera.main.WorldToScreenPoint(heroView.transform.position);
        //        if (position.x >= startPos.x & position.x <= endPos.x & position.y >= startPos.y & position.y <= endPos.y)
        //        {
        //            if (!selectedListHadCleared)
        //            {
        //                HeroView.seletedheroViews.Clear();
        //                selectedListHadCleared = true;
        //                Debug.Log(string.Format("<color=yellow>框选到人啦:{0}</color>", heroView.Unit.Id));
        //            }
        //            HeroView.seletedheroViews.Add(heroView);
        //        }
        //        else
        //        {
        //            unSeletedheroViews.Add(heroView);
        //        }
        //    }
        //    if (!selectedListHadCleared) return;
        //    for (int i = 0; i < HeroView.seletedheroViews.Count; i++)
        //    {
        //        HeroView.seletedheroViews[i].ShowSelectedEft();
        //    }
        //    for (int i = 0; i < unSeletedheroViews.Count; i++)
        //    {
        //        unSeletedheroViews[i].HideSelectedEft();
        //    }
        //}

        /// <summary>
        /// 添加需要绘制扇形(视野)
        /// </summary>
        /// <param name="sector"></param>
        public void AddSector(Sector sector)
        {
            if (allSectors.Contains(sector)) return;
            allSectors.Add(sector);
        }

        //正式项目中需要用到移除视野(如死亡的时候)
        public void RemoveSector(Sector sector)
        {
            if (allSectors.Contains(sector))
                allSectors.Remove(sector);
        }

        private void DrawTriangle(List<Sector> sectors)
        {
            if (sectors.Count <= 0) return;
            GL.PushMatrix();
            mat.SetPass(0);
            //GL.LoadOrtho();
            GL.Begin(GL.TRIANGLES);
            foreach (var sector in sectors)
            {
                sector.PushGLVertex();
            }

            GL.End();
            GL.PopMatrix();
        }

        ///**************以下是其他绘制,可以不care************************
        private void DrawTriangle(Vector3[] vertices, Transform eyeTrans)
        {
            GL.PushMatrix();
            mat.SetPass(0);
            //GL.LoadOrtho();
            GL.Begin(GL.TRIANGLES);
            //GL.Color(Color.green);

            int verCount = 0;
            for (int i = 0; i < vertices.Length; i++)
            {
                verCount++;
                vertice = vertices[i];
                GL.Vertex3(vertice.x, vertice.y, vertice.z);
                if (0 != disMin)
                {
                    if (verCount >= 3)
                    {
                        i = i - 2;
                        verCount = 0;
                    }
                }
                else
                {
                    if (verCount >= 2)
                    {
                        i--;
                        verCount = 0;
                        GL.Vertex3(eyeTrans.position.x, eyeTrans.position.y, eyeTrans.position.z);
                    }
                }
            }

            GL.End();
            GL.PopMatrix();
        }

        private void DrawLine(Vector3[] vertices, Material tempMat = null)
        {
            GL.PushMatrix();
            if (null == tempMat) tempMat = mat;
            tempMat.SetPass(0);
            //绘制2D线段，注释掉GL.LoadOrtho();则绘制3D图形
            //GL.LoadOrtho();
            GL.Begin(GL.LINES);
            for (int i = 0; i < vertices.Length; i++)
            {
                vertice = vertices[i];
                GL.Vertex3(vertice.x, vertice.y, vertice.z);
            }
            GL.End();
            GL.PopMatrix();
        }

        void DrawRect(Vector3[] vertices, Transform eyeTrans)
        {
            if (0 == disMin)
            {
                DrawTriangle(vertices, eyeTrans);
                return;
            }
            GL.PushMatrix();
            mat.SetPass(0);
            //GL.LoadOrtho();
            GL.Begin(GL.QUADS);

            int verCount = 0;
            for (int i = 0; i < vertices.Length; i++)
            {
                verCount++;
                if (verCount % 4 > 0 && verCount % 4 < 3)
                    vertice = vertices[i];
                else if (3 == verCount % 4)
                {
                    vertice = vertices[i + 1];
                }
                else if (0 == verCount % 4 && 0 != verCount)
                {
                    vertice = vertices[i - 1];
                    i = i - 2;
                    verCount = 0;
                }
                GL.Vertex3(vertice.x, vertice.y, vertice.z);
            }

            GL.End();
            GL.PopMatrix();
        }

        private void CheckMat()
        {
            if (null != mat) return;
            Debug.LogWarning("There is no material for render.Please Drag FieldOfViewRenderer to the DrawHelper.");
            Shader fieldOfViewRenderer = Shader.Find("Unlit/FieldOfViewRenderer");
            mat = new Material(fieldOfViewRenderer);
            mat.name = "TempMat";

            mat.hideFlags = HideFlags.HideAndDontSave;
            mat.shader.hideFlags = HideFlags.HideAndDontSave;
        }

    }
}
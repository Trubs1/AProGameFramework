// Copyright (c) 2018 WangQiang(279980661@qq.com)
// author:Trubs (WQ)
// Date:2018/08/08

using UnityEngine;

namespace TrubsDrawHelper
{
    /// <summary>
    /// 扇形绘制对象
    /// </summary>
    public class Sector
    {
        public Transform origin;
        public float angleFov = 90;                 //视野角度
        public float R = 2f;                        //视野最远距离
        public float r = 0.3f;
        public int quality = 72;                    //越大质量越高(12的倍数)
        public float startAngle = 0;
        public Color color = Color.green;           //视野颜色(可以通过改变颜色显示不同的状态,如看到敌人变红色)
        public Vector3[] vertices;
        /// <summary>
        /// 视野显示的高度
        /// </summary>
        public float offsetY = 0.2f;

        float angleDelta;
        Vector3 offsetVec3;
        public Sector(Transform origin, float angleFov, float R)
        {
            this.origin = origin;
            this.angleFov = angleFov;
            this.R = R;
            Init();
        }

        public Sector(Transform origin, float angleFov, float R, float r)
        {
            this.origin = origin;
            this.angleFov = angleFov;
            this.R = R;
            this.r = r;
            Init();
        }

        public Sector(Transform origin)
        {
            this.origin = origin;
            Init();
        }

        public Sector(Transform origin, Vector3 fromVector, Vector3 toVector, float R, float r)
        {
            float angle = Vector3.Angle(fromVector, toVector); //求出两向量之间的夹角
            Vector3 normal = Vector3.Cross(fromVector, toVector);//叉乘求出法线向量
            angle *= Mathf.Sign(Vector3.Dot(normal, Vector3.up));  //求法线向量与物体上方向向量点乘，结果为1或-1，修正旋转方向

            this.origin = origin;
            this.angleFov = angle;
            this.R = R;
            this.r = r;
            Init();
        }

        private void Init()
        {
            if (0 != r)
                vertices = new Vector3[quality * 2 + 2];
            else
                vertices = new Vector3[quality + 1];
            angleDelta = angleFov / quality;
        }

        /// <summary>
        /// 更新扇形顶点数据
        /// </summary>
        /// <returns></returns>
        public Vector3[] RefreshVertices()
        {
            startAngle = origin.eulerAngles.y + angleFov / 2;
            float angleCurr = startAngle;
            offsetVec3 = new Vector3(origin.position.x, offsetY, origin.position.z);
            for (int i = 0; i <= quality; i++)
            {
                Vector3 curVec = new Vector3();
                curVec.z = Mathf.Cos(Mathf.Deg2Rad * angleCurr);
                curVec.x = Mathf.Sin(Mathf.Deg2Rad * angleCurr);

                Vector3 posCurrMax;
                RaycastHit hit;
                //Debug.DrawRay(origin.position, curVec * R, Color.green,0.1f);//可以取消此行注释显示扫描射线
                //if (Physics.Raycast(origin.position, curVec * R, out hit, R, ~(int)Layers.Corpse))//可以过滤掉某些层
                if (Physics.Raycast(origin.position, curVec * R, out hit, R))
                {
                    posCurrMax = curVec * Vector3.Distance(hit.point, origin.position) + offsetVec3;
                    //Debug.DrawLine(origin.position,hit.point,Color.red,0.1f);
                }
                else
                {
                    posCurrMax = curVec * R + offsetVec3;
                }
                Vector3 posCurrMin = curVec * r + offsetVec3;

                if (0 != r)
                {
                    vertices[2 * i] = posCurrMin;
                    vertices[2 * i + 1] = posCurrMax;
                }
                else
                {
                    vertices[i] = posCurrMax;
                }
                angleCurr -= angleDelta;
            }
            return vertices;
        }

        private Vector3 vertice;
        /// <summary>
        /// 将顶点等数据传送给GL
        /// </summary>
        public void PushGLVertex()
        {
            GL.Color(color);
            int verCount = 0;
            var vertices = this.RefreshVertices();
            if (null == vertices) return;
            int wholeCount = vertices.Length - vertices.Length % 3;
            if (0 == this.r)
                wholeCount = vertices.Length;

            //以下为舍进算法,可能会导致渲染出的FOV和配档差一点点
            for (int i = 0; i < wholeCount; i++)
            {
                verCount++;
                vertice = vertices[i];
                GL.TexCoord2(GetUVx(vertice), 0.5f);
                GL.Vertex3(vertice.x, vertice.y, vertice.z);

                if (0 != this.r)
                {
                    if (verCount >= 3 && vertices.Length - i > 3)
                    {
                        i = i - 2;
                        verCount = 0;
                    }
                }
                else
                {
                    if (verCount >= 2)
                    {
                        if (vertices.Length - i > 1)
                        {
                            i--;
                        }
                        verCount = 0;
                        GL.TexCoord2(GetUVx(this.origin.position), 0.5f);
                        GL.Vertex3(this.origin.position.x, this.origin.position.y, this.origin.position.z);
                    }
                }
            }
        }

        //获取UV
        private float GetUVx(Vector3 vertex)
        {
            return Mathf.Clamp01(Vector3.SqrMagnitude(vertex - origin.position) / (R * R));
        }
    }
}
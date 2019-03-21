// Copyright (c) 2018 WangQiang(279980661@qq.com)
// author:Trubs (WQ)
// Date:2018/08/08

using UnityEngine;
using UnityEngine.Rendering;

namespace TrubsDrawHelper
{
    /// <summary>
    /// 通过Mesh绘制(其他方案) 
    /// 此方案每个单位多了个mesh,如果数量大,范围广,会分增加渲染批次
    /// </summary>
    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
    public class MeshSectorDrawer : MonoBehaviour
    {
        int[] triangles;
        Vector3[] vertices;
        Vector2[] uvs;
        Vector3[] normals;
        Vector3 posCurrMin;
        Vector3 posCurrMax;

        public float angleFov = 90;
        public int quality = 64;
        public float rMin = 0.4f;
        public float LookRange = 4f;
        public MeshFilter meshFilter;

        void Awake()
        {
            triangles = new int[quality * 2 * 3];
            vertices = new Vector3[quality * 2 + 2];
            uvs = new Vector2[vertices.Length];
            normals = new Vector3[vertices.Length];

            var render = GetComponent<MeshRenderer>();
            render.lightProbeUsage = LightProbeUsage.Off;
            render.reflectionProbeUsage = ReflectionProbeUsage.Off;
            render.shadowCastingMode = ShadowCastingMode.Off;
            render.receiveShadows = false;

            meshFilter = GetComponent<MeshFilter>();
            meshFilter.mesh = new Mesh();
            DrawTriangleOnce();
        }

        //绘制一次,需要更新的时候才检测绘制(如果旋转)
        void DrawTriangleOnce()
        {
            RefreshMeshData();
            RenderMesh();
        }

        #region///每帧检测碰撞绘制(可以注释节约开销)
        Vector3 lastPos = Vector3.zero;
        Vector3 lastAngles = Vector3.zero;
        void Update()
        {
            if (lastPos != transform.position || lastAngles != transform.eulerAngles)
            {
                lastPos = transform.position;
                lastAngles = transform.eulerAngles;
                if (RefreshMeshData())
                    RenderMesh();
            }
        }
        #endregion

        bool RefreshMeshData()
        {
            //meshFilter.mesh.Clear();
            float startAngle = transform.localEulerAngles.y + angleFov / 2;
            float angleDelta = angleFov / quality;

            float angleCurr = startAngle;
            bool isHit = false;
            for (int i = 0; i <= quality; i++)
            {
                Vector3 sphereCurr = new Vector3();
                sphereCurr.z = Mathf.Cos(Mathf.Deg2Rad * angleCurr);
                sphereCurr.x = Mathf.Sin(Mathf.Deg2Rad * angleCurr);

                Vector3 curVec = Quaternion.Euler(0, angleCurr, 0) * transform.forward;
                Debug.DrawRay(transform.position, curVec * LookRange, Color.green);
                RaycastHit hit;
                if (Physics.Raycast(transform.position, curVec, out hit, LookRange))
                {
                    isHit = true;
                    Debug.DrawLine(transform.position, hit.point, Color.red);
                    posCurrMax = sphereCurr * Vector3.Distance(hit.point, transform.position);
                }
                else
                {
                    posCurrMax = sphereCurr * LookRange;
                }
                posCurrMin = sphereCurr * rMin;

                vertices[2 * i + 0] = posCurrMin;
                vertices[2 * i + 1] = posCurrMax;

                uvs[2 * i + 0] = new Vector2((float)(quality - i) / quality, 0);
                uvs[2 * i + 1] = new Vector2((float)(quality - i) / quality, 1);

                normals[2 * i + 0] = Vector3.up;
                normals[2 * i + 1] = Vector3.up;

                angleCurr -= angleDelta;
            }

            for (int i = 0; i < quality; i++)
            {
                //if (i >= 4 && i <= 6) continue;
                //  5---3---1
                //  |  /|  /|
                //  | / | / |
                //  |/  |/  |
                //  4---2---0

                int indexMinCur = i * 2 + 0;
                int indexMaxCur = i * 2 + 1;
                int indexMinNext = i * 2 + 2;
                int indexMaxNext = i * 2 + 3;

                triangles[6 * i + 0] = indexMinCur;
                triangles[6 * i + 1] = indexMinNext;
                triangles[6 * i + 2] = indexMaxCur;
                triangles[6 * i + 3] = indexMinNext;
                triangles[6 * i + 4] = indexMaxNext;
                triangles[6 * i + 5] = indexMaxCur;

            }
            return isHit;
        }

        void RenderMesh()
        {
            //meshFilter.mesh.Clear();
            meshFilter.sharedMesh.vertices = vertices;
            meshFilter.sharedMesh.triangles = triangles;
            meshFilter.sharedMesh.uv = uvs;
        }
    }
}
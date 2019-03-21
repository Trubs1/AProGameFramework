// Copyright (c) 2018 WangQiang(279980661@qq.com)
// author:Trubs (WQ)
// Date:2018/08/08

using UnityEngine;

namespace TrubsDrawHelper
{
    /// <summary>
    /// 可视单位的眼睛
    /// </summary>
    public class Eye : MonoBehaviour
    {
        //配档数值
        public float fov = 120; //视野的角度
        public float lookR = 4; //视野的长度

        private Sector sector;
        private void Start()
        {
            sector = new Sector(transform, fov, lookR);
            DrawHelper.It.AddSector(sector);
        }
        private void OnDestroy()
        {
            DrawHelper.It.RemoveSector(sector);
        }
    }
}
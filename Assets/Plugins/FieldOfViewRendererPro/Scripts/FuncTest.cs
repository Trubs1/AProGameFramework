// Copyright (c) 2018 WangQiang(279980661@qq.com)
// author:Trubs (WQ)
// Date:2018/08/08

using UnityEngine;
using UnityEngine.UI;

namespace TrubsDrawHelper
{
    /// <summary>
    /// 测试辅助(与逻辑无关,可忽略)
    /// </summary>
    public class FuncTest : MonoBehaviour
    {
        private float timePassed;
        private int frameCount = 0;
        private float fps = 0.0f;

        public GameObject[] objs;
        public Text codeTxt;
        public Text fpsTxt;
        public Button runCodeBtn;
        public InputField inputField;
        public float fpsRefreshTime = 1.0f;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            Debug.Log("FuncTest_Start");
            //var m_DepthRenderCamera = gameObject.AddComponent<Camera>();
            //m_DepthRenderCamera.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
            //runCodeBtn.onClick.AddListener(RunCode);
        }

        private void RunCode()
        {
            //try
            //{
            //    int codeNumber = int.Parse(inputField.text);
            //    GameObject obj = objs[codeNumber];
            //    obj.SetActive(!obj.activeSelf);
            //    Debug.Log("obj" + obj);
            //}
            //catch
            //{
            //    Debug.Log("error info:" + inputField.text);
            //}
        }

        private void Update()
        {
            frameCount++;
            timePassed += Time.deltaTime;

            if (timePassed > fpsRefreshTime)
            {
                fps = frameCount / timePassed;
                //fpsTxt.text = "fps:" + m_FPS;
                timePassed = 0.0f;
                frameCount = 0;
            }
        }

        private void OnGUI()//
        {
            GUIStyle bb = new GUIStyle();
            bb.normal.background = null;    //这是设置背景填充的
            bb.normal.textColor = Color.yellow;
            bb.fontSize = 40;
            GUI.Label(new Rect((Screen.width / 2) - 40, 0, 200, 200), "FPS: " + fps, bb);
        }
    }
}
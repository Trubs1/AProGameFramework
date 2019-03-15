using UnityEngine;
using System.Collections;
using System.IO;
using LuaInterface;

namespace LuaFramework {
    /// <summary>
    /// 集成自LuaFileUtils，重写里面的ReadFile，
    /// </summary>
    public class LuaLoader : LuaFileUtils {
        private ResourceManager m_resMgr;

        ResourceManager resMgr {
            get { 
                if (m_resMgr == null)
                    m_resMgr = AppFacade.Instance.GetManager<ResourceManager>(ManagerName.Resource);
                return m_resMgr;
            }
        }

        // Use this for initialization
        public LuaLoader() {
            instance = this;
            beZip = AppConst.LuaBundleMode;
        }

        /// <summary>
        /// 添加打入Lua代码的AssetBundle //以前是枚举路径,然后这里拼接(舍弃)
        /// </summary>
        /// <param name="bundle"></param>
        public void AddBundle(string bundle_path)
        {
            AssetBundle bundle = AssetBundle.LoadFromFile(bundle_path);
            if (bundle != null)
            {
                bundle_path = bundle_path.Replace(".unity3d", "");
                int pos = bundle_path.LastIndexOf('/');
                if (pos < 0)
                {
                    pos = bundle_path.LastIndexOf('\\');
                }

                if (pos > 0)
                {
                    bundle_path = bundle_path.Substring(pos + 1);
                }
                base.AddSearchBundle(bundle_path.ToLower(), bundle);
            }
        }

        /// <summary>
        /// 当LuaVM加载Lua文件的时候，这里就会被调用，
        /// 用户可以自定义加载行为，只要返回byte[]即可。
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public override byte[] ReadFile(string fileName) {
            return base.ReadFile(fileName);     
        }
    }
}
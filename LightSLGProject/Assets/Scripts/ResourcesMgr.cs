using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using System.Linq;

#endif


//物体分类，一个物体可以是多种类型
public enum ResourceType
{
    None = 0,
    Audio, 
    Model,
    Texture,
    Scene,
    TextAssets,
    AssetBudnle,
    EveryThing,
}
[Serializable]
public class ResourceItem
{
    public string name;//资源名字
    public string path;//资源resources路径
    public ResourceType type;//资源类型
    [Range(0,100)]
    public int PreloadingCount;
    public bool IsPreLoading;

    public ResourceItem(string name, string path, ResourceType type,int count,bool isPreloading)
    {
        this.name = name;
        this.path = path;
        this.type = type;
        this.PreloadingCount = count;
        this.IsPreLoading = isPreloading;
    }
}
[Serializable]
public class ResourceRes<T> where T : UnityEngine.Object
{
    public string name;//资源名字
    public T res;//资源

    public ResourceRes(string name, T res)
    {
        this.name = name;
        this.res = res;
    }
}
[Serializable]
public class ResourceList
{
    public List<ResourceItem> value = new List<ResourceItem>();
}
public class ResourcesMgr
{
    public static readonly ResourcesMgr Instance = new ResourcesMgr();
    private ResourcesMgr() { }

    private string m_configPath = "configs/resourcesmgr";

    public ResourceList m_ResLis;

    private Dictionary<string, ResourceItem> m_ResDic;

    /// <summary>
    /// 加载配置文件
    /// </summary>
    public void LoadConfigs()
    {
        m_ResDic = new Dictionary<string, ResourceItem>();
        TextAsset asset = Resources.Load<TextAsset>(m_configPath);
        if (asset == null)
        {
            Debug.Log("未加载到数据表");
            return;
        }
        if (string.IsNullOrEmpty(asset.text))
        {
            Debug.Log("资源配置表为空");
            return;
        }
        m_ResLis = JsonUtility.FromJson<ResourceList>(asset.text);
        Debug.Assert(null != m_ResLis, "LoadConfig _audioResLst is null.");
        MaskAssetsIndex();
    }

    /// <summary>
    /// 设置物体索引
    /// </summary>
    private void MaskAssetsIndex()
    {
        foreach (var item in m_ResLis.value)
        {
            if (!m_ResDic.ContainsKey(item.name))
            {
                m_ResDic.Add(item.name, item);
            }
           
        }
    }
    /// <summary>
    /// 获取资源
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private ResourceItem GetItemFromName(string name)
    {
        if (m_ResDic.ContainsKey(name))
        {
            return m_ResDic[name];
        }
        else
        {
            Debug.LogError(name + "——————资源未加载成功");
            return null;
        }
       
    }
    /// <summary>
    /// 加载资源
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    public ResourceRes<T> Load<T>(string name) where T : UnityEngine.Object
    {
        ResourceItem item = null;

        item = GetItemFromName(name);
        if (item == null)
        {
            return null;
        }
        T obj = Resources.Load<T>(item.path);
        if (obj == null)
        {
            return null;
        }
        else
        {
            return new ResourceRes<T>(name, obj);
        }

    }
    /// <summary>
    /// 异步加载资源
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name">资源名</param>
    /// <param name="onComplete">回调</param>
    /// <param name="type">物体类型</param>
    public void LoadAsync<T>(string name, Action<ResourceRes<T>> onComplete, ResourceType type = 0) where T : UnityEngine.Object
    {
        ResourceItem item = null;

        item = GetItemFromName(name);

        T obj = null;
        Debug.Assert(null != item, "item is Null");
        if (!string.IsNullOrEmpty(item.path))
        {
            ResourceRequest req = Resources.LoadAsync<T>(item.path);
            Debug.Assert(req != null, "srcpath:" + item.path);

            req.completed += p =>
            {
                if (req != null)
                {
                    obj = req.asset as T;
                    if (null != onComplete)
                    {
                        onComplete(new ResourceRes<T>(item.name, obj));
                    }
                }

            };
        }
    }

#if UNITY_EDITOR
//编辑器扩展

[CustomEditor(typeof(ResourcesMgr))]
[CanEditMultipleObjects()]
public class ResourceManagerEdit : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Edit Config"))
        {
            ResourceManagerWindow.ShowWindow();
        }
    }
}
    public class ResourceManagerWindow : EditorWindow
    {
        [MenuItem("编辑器拓展/资源加载管理器")]
        public static void ShowWindow()
        {
            ResourceManagerWindow window = GetWindow<ResourceManagerWindow>("Resource Mgr");
            window.Show();
        }

        [Serializable]
        public class ResourceManagerConfig
        {
            public string configPath = "resources/configs/resourcesmgr.json";
        }

        string editConfigPath = "";
        ResourceManagerConfig editConfig = null;
        ResourceList _lst = null;

        public void OnEnable()
        {
            editConfigPath = Path.Combine(Application.dataPath, "../ProjectSettings/ResourceManagerConfig.json");
            if (File.Exists(editConfigPath))
            {
                string editConfigMsg = File.ReadAllText(editConfigPath);
                editConfig = JsonUtility.FromJson<ResourceManagerConfig>(editConfigMsg);
            }
            if (null == editConfig)
            {
                editConfig = new ResourceManagerConfig();
            }
            if (_lst == null)
            {
                Load();
            }

        }

        public void OnDisable()
        {
            Save();

            string editConfigMsg = JsonUtility.ToJson(editConfig);
            File.WriteAllText(editConfigPath, editConfigMsg);
        }

        void Load()
        {
            string path = Path.Combine(Application.dataPath, editConfig.configPath);
            if (File.Exists(path))
            {
                string configMsg = File.ReadAllText(path);
                _lst = JsonUtility.FromJson<ResourceList>(configMsg);
                SortResources();
            }
            if (null == _lst)
            {
                _lst = new ResourceList();
            }
        }
        private void SortResources()
        {
            if (_lst != null)
            {
                // _lst.value.Sort((x, y) => string.Compare(x.name, y.name));
                _lst.value = _lst.value.OrderBy(restype => restype.type.ToString()).ThenBy(resname => resname.name).ToList();
            }

        }


        void Save()
        {
            if (!string.IsNullOrEmpty(editConfig.configPath))
            {
                string path = Path.Combine(Application.dataPath, editConfig.configPath);
                if (!Directory.Exists(Path.GetDirectoryName(path)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                }

                string configMsg = JsonUtility.ToJson(_lst);
                File.WriteAllText(path, configMsg);

                if (File.Exists(path))
                {
                    AssetDatabase.ImportAsset("Assets/" + editConfig.configPath, ImportAssetOptions.ForceSynchronousImport);
                }
            }
        }

        Vector2 _scrollPos;
        bool isTipShow = false;
        private string SearchInputValue;
        private bool IsShowPreLoadingView;
        private ResourceType SearchResouceType = ResourceType.EveryThing;
        private bool IsSelectAll;
        private bool IsUnSelectAll;
        public void OnGUI()
        {

            GUILayout.Label("Root is Assets folder path.");
            GUILayout.BeginHorizontal();
            GUILayout.Label("Path:", GUILayout.MaxWidth(50));
            editConfig.configPath = GUILayout.TextField(editConfig.configPath);
            if (GUILayout.Button("Load", GUILayout.Width(40), GUILayout.Height(14)))
            {
                Load();
            }
            GUILayout.EndHorizontal();
            GUILayout.Label("     ");
            GUILayout.BeginHorizontal();

            GUILayout.Label("按名称搜索", GUILayout.Width(70));
            SearchInputValue = SearchField(SearchInputValue, GUILayout.Width(300));
            GUILayout.Label("     ", GUILayout.Width(50));
            GUILayout.Label("按类型搜索", GUILayout.Width(70));
            SearchResouceType = (ResourceType)EditorGUILayout.EnumPopup(SearchResouceType, GUILayout.Width(100));
            GUILayout.Label("     ", GUILayout.Width(50));
            IsShowPreLoadingView = GUILayout.Toggle(IsShowPreLoadingView, "是否显示预加载资源配置", GUILayout.Width(150));
            if (IsShowPreLoadingView)
            {
                if (GUILayout.Button("当前所有物体全部预加载", GUILayout.Width(200)))
                {
                    IsSelectAll = true;
                }
                if (GUILayout.Button("取消当前所有物体全部预加载", GUILayout.Width(200)))
                {
                    IsUnSelectAll = true;
                }
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginVertical();
            GUILayout.Space(20);
            if (GUILayout.Button("清理", GUILayout.Width(70)))
            {
                ClearNullRes();
                Debug.Log("清理完毕");
            }
            GUILayout.Label("     ");
            GUILayout.BeginHorizontal();
            GUILayout.Label("Name:", GUILayout.Width(250));
            GUILayout.Label("Path:", GUILayout.Width(500));
            GUILayout.Label("Type:", GUILayout.Width(100));
            if (IsShowPreLoadingView)
            {
                GUILayout.Label("是否预加载:", GUILayout.Width(100));
                GUILayout.Label("预加载数量:", GUILayout.Width(100));
            }

            GUILayout.EndHorizontal();

            _scrollPos = GUILayout.BeginScrollView(_scrollPos);
            for (int index = 0, cnt = _lst.value.Count; index < cnt; index++)
            {
                GUILayout.BeginHorizontal();
                if (string.IsNullOrEmpty(SearchInputValue) == false)
                {

                    ResourceItem item = _lst.value[index];
                    if (item.name.Contains(SearchInputValue) && SearchResouceType == item.type || item.name.Contains(SearchInputValue) && SearchResouceType == ResourceType.EveryThing)
                    {
                        item.name = GUILayout.TextField(item.name, GUILayout.Width(250));
                        item.path = GUILayout.TextField(item.path, GUILayout.Width(500));
                        // item.type = (ResourceType)EditorGUILayout.EnumMaskField(item.type, GUILayout.Width(100));
                        item.type = (ResourceType)EditorGUILayout.EnumFlagsField(item.type, GUILayout.Width(100));

                        if (IsShowPreLoadingView)
                        {
                            if (IsSelectAll)
                            {
                                item.IsPreLoading = true;
                            }
                            if (IsUnSelectAll)
                            {
                                item.IsPreLoading = false;
                            }
                            item.IsPreLoading = GUILayout.Toggle(item.IsPreLoading, "是否预加载", GUILayout.Width(100));
                            if (item.IsPreLoading == false)
                            {
                                item.PreloadingCount = 0;
                            }
                            if (item.IsPreLoading == true && item.PreloadingCount == 0)
                            {
                                item.PreloadingCount = 5;
                            }

                            item.PreloadingCount = (int)GUILayout.HorizontalSlider(item.PreloadingCount, 0, 100, GUILayout.Width(100));
                            item.PreloadingCount = int.Parse(GUILayout.TextField(item.PreloadingCount.ToString(), GUILayout.Width(50)));
                            if (item.PreloadingCount > 0)
                            {
                                item.IsPreLoading = true;
                            }
                            else
                            {
                                item.IsPreLoading = false;
                            }
                        }
                        if (GUILayout.Button("-", GUILayout.Width(20), GUILayout.Height(14)))
                        {
                            _lst.value.RemoveAt(index);
                            --index;
                            --cnt;

                            continue;
                        }

                    }


                }
                else
                {

                    ResourceItem item = _lst.value[index];
                    // Debug.Log(SearchResouceType+"..."+item.type);
                    if (SearchResouceType == item.type || SearchResouceType == ResourceType.EveryThing)
                    {
                        item.name = GUILayout.TextField(item.name, GUILayout.Width(250));
                        item.path = GUILayout.TextField(item.path, GUILayout.Width(500));
                        // item.type = (ResourceType)EditorGUILayout.EnumMaskField(item.type, GUILayout.Width(100));
                        item.type = (ResourceType)EditorGUILayout.EnumFlagsField(item.type, GUILayout.Width(100));

                        if (IsShowPreLoadingView)
                        {
                            if (IsSelectAll)
                            {
                                item.IsPreLoading = true;
                            }
                            if (IsUnSelectAll)
                            {
                                item.IsPreLoading = false;
                            }
                            item.IsPreLoading = GUILayout.Toggle(item.IsPreLoading, "是否预加载", GUILayout.Width(100));
                            if (item.IsPreLoading == false)
                            {
                                item.PreloadingCount = 0;
                            }
                            if (item.IsPreLoading == true && item.PreloadingCount == 0)
                            {
                                item.PreloadingCount = 5;
                            }
                            item.PreloadingCount = (int)GUILayout.HorizontalSlider(item.PreloadingCount, 0, 100, GUILayout.Width(100));
                            item.PreloadingCount = int.Parse(GUILayout.TextField(item.PreloadingCount.ToString(), GUILayout.Width(50)));
                            if (item.PreloadingCount > 0)
                            {
                                item.IsPreLoading = true;
                            }
                            else
                            {
                                item.IsPreLoading = false;
                            }
                        }
                        if (GUILayout.Button("-", GUILayout.Width(20), GUILayout.Height(14)))
                        {
                            _lst.value.RemoveAt(index);
                            --index;
                            --cnt;

                            continue;
                        }
                    }

                }
                GUILayout.EndHorizontal();
            }
            IsSelectAll = false;
            IsUnSelectAll = false;

            GUILayout.EndScrollView();




            if (GUILayout.Button("添加"))
            {
                _lst.value.Add(new ResourceItem("", "", 0, 0, true));
            }
            isTipShow = GUILayout.Toggle(isTipShow, new GUIContent("是否提示重复文件"));

            if (GUILayout.Button("读取"))
            {

                if (_lst == null)
                {
                    Load();
                }

                UnityEngine.Object[] objs = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);

                if (objs.Length != 0)
                {
                    string path = "";
                    string name = "";
                    string objsPath = "";
                    int precount = 0;
                    for (int i = 0; i < objs.Length; i++)
                    {

                        if (objs[i].GetType() != typeof(DefaultAsset))
                        {
                            if (!AssetDatabase.GetAssetPath(objs[i]).ToString().Contains("Resources"))
                            {
                                this.ShowNotification(new GUIContent("目录选择错误,请选择Resources下的目录"));
                                Debug.LogError(AssetDatabase.GetAssetPath(objs[i]).ToString() + "---->当前文件不在Resource目录");
                                continue;
                            }
                            if (objs[i].GetType() != typeof(TextAsset))
                            {
                                objsPath = objs[i].ToString();
                                name = objsPath.Remove(objsPath.IndexOf("("));
                                path = AssetDatabase.GetAssetPath(objs[i]).ToString();
                                path = path.Replace("Assets/Resources/", "");
                                path = path.Split('.')[0];
                            }
                            else
                            {
                                path = AssetDatabase.GetAssetPath(objs[i]).ToString();
                                path = path.Replace("Assets/Resources/", "");
                                path = path.Split('.')[0];
                                name = path.Substring(path.LastIndexOf('/'));
                                name = name.Remove(name.IndexOf('/'), 1);
                            }
                            name = name.Trim();
                            path = path.Trim();
                            precount = 0;
                            if (name.Contains("EF") || name.Contains("Effect") || path.Contains("EffectPrefab") || path.Contains("BulletPrefab") || path.Contains("NumberPrefab"))
                            {
                                precount = 5;
                            }
                            if (path.Contains("NumberPrefab") || path.Contains("FoodPrefab"))
                            {
                                precount = 5;
                            }
                            if (path.Contains("MonsterPrefab"))
                            {
                                precount = 5;
                            }
                            bool isPreloading = precount > 0 ? true : false;
                            ResourceItem item = new ResourceItem(name, path, GetObjType(objs[i].GetType()), precount, isPreloading);
                            if (_lst.value.Count(a => a.name == item.name && a.type == item.type && a.path == item.path) == 0)
                            {
                                _lst.value.Add(item);
                            }
                            else
                            {
                                this.ShowNotification(new GUIContent("文件已包含"));
                                if (isTipShow == true)
                                {
                                    Debug.LogError("当前目录已包含名字为:" + item.name + "路径为:" + item.path + "类型为:" + item.type + " 的文件");
                                }

                                continue;
                            }
                        }

                    }
                    SortResources();

                }
                else
                {
                    this.ShowNotification(new GUIContent("当前文件夹没有读取到资源或没有选择文件夹"));
                }
            }
            if (GUILayout.Button("更新预加载信息"))
            {
                for (int i = 0; i < _lst.value.Count; i++)
                {
                    if (_lst.value[i].name.Contains("EF") || _lst.value[i].name.Contains("Effect") ||
                        _lst.value[i].path.Contains("EffectPrefab")
                        || _lst.value[i].path.Contains("BulletPrefab") || _lst.value[i].path.Contains("NumberPrefab"))
                    {
                        _lst.value[i].PreloadingCount = 5;
                    }
                    if (_lst.value[i].path.Contains("NumberPrefab") || _lst.value[i].path.Contains("FoodPrefab"))
                    {
                        _lst.value[i].PreloadingCount = 5;
                    }
                    if (_lst.value[i].path.Contains("MonsterPrefab"))
                    {
                        _lst.value[i].PreloadingCount = 5;
                    }
                    _lst.value[i].IsPreLoading = _lst.value[i].PreloadingCount > 0 ? true : false;
                }
            }
            GUILayout.EndVertical();
        }

        public static string SearchField(string value, params GUILayoutOption[] options)
        {
            MethodInfo info = typeof(EditorGUILayout).GetMethod("ToolbarSearchField", BindingFlags.NonPublic | BindingFlags.Static, null, new System.Type[] { typeof(string), typeof(GUILayoutOption[]) }, null);
            if (info != null)
            {
                value = (string)info.Invoke(null, new object[] { value, options });
            }
            return value;
        }
        private void ClearNullRes()
        {
            for (int i = _lst.value.Count - 1; i > 0; i--)
            {
                if (Resources.Load(_lst.value[i].path) == null)
                {
                    _lst.value.Remove(_lst.value[i]);
                    Debug.Log("名称：" + _lst.value[i].name + "不存在，已删除!");
                }
            }
        }
        private ResourceType GetObjType(Type mType)
        {
            ResourceType ResType = 0;
            if (mType == typeof(GameObject))
            {
                ResType = ResourceType.Model;
            }
            if (mType == typeof(Texture2D) || mType == typeof(UnityEngine.Texture))
            {
                ResType = ResourceType.Texture;
            }
            if (mType == typeof(TextAsset))
            {
                ResType = ResourceType.TextAssets;
            }
            if (mType == typeof(AudioClip))
            {
                ResType = ResourceType.Audio;
            }
            if (mType == typeof(SceneAsset))
            {
                ResType = ResourceType.Scene;
            }
            if (mType == typeof(AssetBundle))
            {
                ResType = ResourceType.AssetBudnle;
            }
            return ResType;
        }
        private Type ConversionTypeByResourceTYpe(ResourceType resType)
        {
            switch (resType)
            {
                case ResourceType.Audio:
                    return typeof(AudioClip);
                case ResourceType.Model:
                    return typeof(GameObject);
                case ResourceType.Texture:
                    return typeof(Texture2D);
                case ResourceType.Scene:
                    return typeof(SceneAsset);
                case ResourceType.TextAssets:
                    return typeof(TextAsset);
                case ResourceType.AssetBudnle:
                    return typeof(AssetBundle);
                case ResourceType.EveryThing:
                    return null;
                default:
                    return null;
                    break;
            }
        }
    }
}
#endif
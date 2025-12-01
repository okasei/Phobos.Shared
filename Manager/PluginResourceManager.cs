using System;
using System.Windows;

namespace Phobos.Shared.Manager
{
    /// <summary>
    /// 插件资源管理器
    /// 用于在插件中正确管理和应用主题资源
    /// </summary>
    public class PluginResourceManager
    {
        private ResourceDictionary? _hostTheme;
        private ResourceDictionary? _pluginStyles;
        private ResourceDictionary? _combinedResources;

        /// <summary>
        /// 获取合并后的资源字典（单例）
        /// </summary>
        public ResourceDictionary CombinedResources
        {
            get
            {
                if (_combinedResources == null)
                {
                    _combinedResources = new ResourceDictionary();
                }
                return _combinedResources;
            }
        }

        /// <summary>
        /// 设置主程序主题资源
        /// </summary>
        public void SetHostTheme(ResourceDictionary? theme)
        {
            if (theme == null) return;

            // 如果已有旧主题，先移除
            if (_hostTheme != null && CombinedResources.MergedDictionaries.Contains(_hostTheme))
            {
                CombinedResources.MergedDictionaries.Remove(_hostTheme);
            }

            _hostTheme = theme;

            // 主题始终放在最前面
            CombinedResources.MergedDictionaries.Insert(0, _hostTheme);

            System.Diagnostics.Debug.WriteLine($"[PluginResources] Host theme set, keys: {_hostTheme.Keys.Count}");
        }

        /// <summary>
        /// 加载插件本地样式
        /// </summary>
        /// <param name="styleUri">样式文件的 Pack URI</param>
        public void LoadPluginStyles(string styleUri)
        {
            if (_pluginStyles != null)
            {
                // 已加载过，不重复加载
                return;
            }

            try
            {
                _pluginStyles = new ResourceDictionary
                {
                    Source = new Uri(styleUri)
                };

                // 样式放在主题之后
                CombinedResources.MergedDictionaries.Add(_pluginStyles);

                System.Diagnostics.Debug.WriteLine($"[PluginResources] Plugin styles loaded from {styleUri}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[PluginResources] Failed to load styles: {ex.Message}");
            }
        }

        /// <summary>
        /// 将资源应用到窗口
        /// 必须在 InitializeComponent() 之前调用！
        /// </summary>
        public void ApplyToWindow(Window window)
        {
            // 直接使用同一个 CombinedResources 实例
            // 这样所有窗口共享同一份资源，主题更新时自动生效
            if (!window.Resources.MergedDictionaries.Contains(CombinedResources))
            {
                window.Resources.MergedDictionaries.Insert(0, CombinedResources);
            }

            System.Diagnostics.Debug.WriteLine($"[PluginResources] Applied to window: {window.GetType().Name}");
        }

        /// <summary>
        /// 更新主题（主程序主题切换时调用）
        /// </summary>
        public void UpdateTheme(ResourceDictionary newTheme)
        {
            SetHostTheme(newTheme);

            // 由于所有窗口共享同一个 CombinedResources 实例
            // 这里更新后，所有使用 DynamicResource 的绑定都会自动更新
        }

        /// <summary>
        /// 调试：打印当前资源状态
        /// </summary>
        public void DebugPrint()
        {
            System.Diagnostics.Debug.WriteLine("=== Plugin Resource Manager ===");
            System.Diagnostics.Debug.WriteLine($"Combined MergedDictionaries: {CombinedResources.MergedDictionaries.Count}");

            for (int i = 0; i < CombinedResources.MergedDictionaries.Count; i++)
            {
                var dict = CombinedResources.MergedDictionaries[i];
                System.Diagnostics.Debug.WriteLine($"  [{i}] Keys: {dict.Keys.Count}, Source: {dict.Source}");

                // 打印几个关键资源
                if (dict.Contains("PrimaryBrush"))
                {
                    var brush = dict["PrimaryBrush"] as System.Windows.Media.SolidColorBrush;
                    if (brush != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"       PrimaryBrush = #{brush.Color.R:X2}{brush.Color.G:X2}{brush.Color.B:X2}");
                    }
                }
            }
        }
    }
}
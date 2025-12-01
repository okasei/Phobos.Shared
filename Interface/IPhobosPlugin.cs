using System.Windows;

namespace Phobos.Shared.Interface
{
    /// <summary>
    /// 插件依赖项
    /// </summary>
    public class PluginDependency
    {
        public string PackageName { get; set; } = string.Empty;
        public string MinVersion { get; set; } = "1.0.0";
        public bool IsOptional { get; set; } = false;
    }

    /// <summary>
    /// 插件文件信息
    /// </summary>
    public class PluginFileInfo
    {
        /// <summary>
        /// 相对路径（相对于插件目录）
        /// </summary>
        public string RelativePath { get; set; } = string.Empty;

        /// <summary>
        /// 文件类型
        /// </summary>
        public PluginFileType FileType { get; set; } = PluginFileType.Library;

        /// <summary>
        /// 是否为主程序集
        /// </summary>
        public bool IsMainAssembly { get; set; } = false;

        /// <summary>
        /// 是否必需
        /// </summary>
        public bool IsRequired { get; set; } = true;
    }

    /// <summary>
    /// 插件文件类型
    /// </summary>
    public enum PluginFileType
    {
        /// <summary>
        /// 主程序集
        /// </summary>
        MainAssembly,

        /// <summary>
        /// 依赖库
        /// </summary>
        Library,

        /// <summary>
        /// 资源文件
        /// </summary>
        Resource,

        /// <summary>
        /// 配置文件
        /// </summary>
        Config,

        /// <summary>
        /// 本地化文件
        /// </summary>
        Localization,

        /// <summary>
        /// 其他文件
        /// </summary>
        Other
    }

    /// <summary>
    /// 插件元数据
    /// </summary>
    public class PluginMetadata
    {
        public string Name { get; set; } = string.Empty;
        public string PackageName { get; set; } = string.Empty;
        public string Manufacturer { get; set; } = string.Empty;
        public string Version { get; set; } = "1.0.0";
        public string Secret { get; set; } = string.Empty;
        public string DatabaseKey { get; set; } = string.Empty;
        public List<PluginDependency> Dependencies { get; set; } = new();
        public Dictionary<string, string> LocalizedNames { get; set; } = new();
        public Dictionary<string, string> LocalizedDescriptions { get; set; } = new();

        /// <summary>
        /// 插件文件列表（用于安装/卸载时复制/删除文件）
        /// </summary>
        public List<PluginFileInfo> FileList { get; set; } = new();

        /// <summary>
        /// 插件图标路径（相对于插件目录的路径）
        /// 例如: "Assets/icon.png" 或 "icon.ico"
        /// </summary>
        public string? Icon { get; set; }

        /// <summary>
        /// 是否为系统插件
        /// 系统插件不可被用户卸载，只能由系统管理
        /// </summary>
        public bool IsSystemPlugin { get; set; } = false;

        /// <summary>
        /// 设置页面的命令 URI
        /// 例如: "log://setting" 或 "calculator://preferences"
        /// 为空表示插件没有设置页面
        /// </summary>
        public string? SettingUri { get; set; }

        /// <summary>
        /// 特殊卸载提示信息
        /// 用于系统插件或需要特殊说明的插件
        /// 卸载时会显示此提示，一般插件不需要设置
        /// </summary>
        public PluginUninstallInfo? UninstallInfo { get; set; }

        /// <summary>
        /// 插件描述
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 插件主页 URL
        /// </summary>
        public string? HomepageUrl { get; set; }

        /// <summary>
        /// 最小 Phobos 版本要求
        /// </summary>
        public string? MinPhobosVersion { get; set; }

        /// <summary>
        /// [已弃用] 请使用 Icon 属性
        /// </summary>
        [Obsolete("Use Icon property instead")]
        public string? IconPath
        {
            get => Icon;
            set => Icon = value;
        }

        /// <summary>
        /// 获取本地化名称
        /// </summary>
        public string GetLocalizedName(string languageCode)
        {
            if (LocalizedNames.TryGetValue(languageCode, out var name))
                return name;
            if (LocalizedNames.TryGetValue("en-US", out var defaultName))
                return defaultName;
            return Name;
        }

        /// <summary>
        /// 获取本地化描述
        /// </summary>
        public string GetLocalizedDescription(string languageCode)
        {
            if (LocalizedDescriptions.TryGetValue(languageCode, out var desc))
                return desc;
            if (LocalizedDescriptions.TryGetValue("en-US", out var defaultDesc))
                return defaultDesc;
            return Description;
        }

        /// <summary>
        /// 获取主程序集文件名
        /// </summary>
        public string? GetMainAssemblyFileName()
        {
            return FileList.Find(f => f.IsMainAssembly || f.FileType == PluginFileType.MainAssembly)?.RelativePath;
        }

        /// <summary>
        /// 获取图标完整路径
        /// </summary>
        /// <param name="pluginDirectory">插件目录</param>
        /// <returns>图标完整路径，如果没有设置则返回 null</returns>
        public string? GetIconFullPath(string pluginDirectory)
        {
            if (string.IsNullOrEmpty(Icon))
                return null;
            return System.IO.Path.Combine(pluginDirectory, Icon);
        }
    }

    /// <summary>
    /// 插件卸载信息
    /// </summary>
    public class PluginUninstallInfo
    {
        /// <summary>
        /// 是否允许卸载
        /// </summary>
        public bool AllowUninstall { get; set; } = true;

        /// <summary>
        /// 卸载提示标题
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 卸载提示消息
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 本地化的卸载提示标题
        /// </summary>
        public Dictionary<string, string> LocalizedTitles { get; set; } = new();

        /// <summary>
        /// 本地化的卸载提示消息
        /// </summary>
        public Dictionary<string, string> LocalizedMessages { get; set; } = new();

        /// <summary>
        /// 卸载前需要执行的命令（可选）
        /// </summary>
        public string? PreUninstallCommand { get; set; }

        /// <summary>
        /// 卸载后需要执行的命令（可选）
        /// </summary>
        public string? PostUninstallCommand { get; set; }

        /// <summary>
        /// 获取本地化标题
        /// </summary>
        public string GetLocalizedTitle(string languageCode)
        {
            if (LocalizedTitles.TryGetValue(languageCode, out var title))
                return title;
            if (LocalizedTitles.TryGetValue("en-US", out var defaultTitle))
                return defaultTitle;
            return Title;
        }

        /// <summary>
        /// 获取本地化消息
        /// </summary>
        public string GetLocalizedMessage(string languageCode)
        {
            if (LocalizedMessages.TryGetValue(languageCode, out var message))
                return message;
            if (LocalizedMessages.TryGetValue("en-US", out var defaultMessage))
                return defaultMessage;
            return Message;
        }
    }

    /// <summary>
    /// 插件调用者上下文 - 用于标识调用来源
    /// </summary>
    public class PluginCallerContext
    {
        /// <summary>
        /// 调用者名称
        /// </summary>
        public Dictionary<string, string> Name { get; set; } = new();
        /// <summary>
        /// 调用者包名
        /// </summary>
        public string PackageName { get; set; } = string.Empty;

        /// <summary>
        /// 调用者数据库键前缀
        /// </summary>
        public string DatabaseKey { get; set; } = string.Empty;

        /// <summary>
        /// 是否为受信任的插件
        /// </summary>
        public bool IsTrusted { get; set; } = false;

        /// <summary>
        /// 调用时间
        /// </summary>
        public DateTime CallTime { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// 请求结果
    /// </summary>
    public class RequestResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<object> Data { get; set; } = new();
        public Exception? Error { get; set; }
    }

    /// <summary>
    /// 配置读写结果
    /// </summary>
    public class ConfigResult
    {
        public bool Success { get; set; }
        public string Key { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }

    /// <summary>
    /// 启动项结果
    /// </summary>
    public class BootResult
    {
        public bool Success { get; set; }
        public string UUID { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }

    /// <summary>
    /// 链接关联配置
    /// </summary>
    public class LinkAssociation
    {
        public string Protocol { get; set; } = string.Empty;
        //只是用来获取可用列表, 调用的时候会通过 callerContext 传进去
        public string PackageName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Command { get; set; } = string.Empty;
        public Dictionary<string, string> LocalizedDescriptions { get; set; } = new();

        public string GetLocalizedDescription(string languageCode)
        {
            if (LocalizedDescriptions.TryGetValue(languageCode, out var desc))
                return desc;
            if (LocalizedDescriptions.TryGetValue("en-US", out var defaultDesc))
                return defaultDesc;
            return Description;
        }
    }

    /// <summary>
    /// 协议打开方式选项
    /// </summary>
    public class ProtocolHandlerOption
    {
        public string UUID { get; set; } = string.Empty;
        public string Protocol { get; set; } = string.Empty;
        public string AssociatedItem { get; set; } = string.Empty;
        public string PackageName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Command { get; set; } = string.Empty;
        public DateTime UpdateTime { get; set; }
        public bool IsUpdated { get; set; } = false; // 标记是否为新更新的选项
        public bool IsDefault { get; set; } = false; // 标记是否为当前默认
    }

    /// <summary>
    /// 插件接口
    /// </summary>
    public interface IPhobosPlugin
    {
        /// <summary>
        /// 插件元数据
        /// </summary>
        PluginMetadata Metadata { get; }

        /// <summary>
        /// 插件内容区域
        /// </summary>
        FrameworkElement? ContentArea { get; }

        /// <summary>
        /// 安装时调用
        /// </summary>
        Task<RequestResult> OnInstall(params object[] args);

        /// <summary>
        /// 启动时调用
        /// </summary>
        Task<RequestResult> OnLaunch(params object[] args);

        /// <summary>
        /// 关闭时调用
        /// </summary>
        Task<RequestResult> OnClosing(params object[] args);

        /// <summary>
        /// 卸载时调用
        /// </summary>
        Task<RequestResult> OnUninstall(params object[] args);

        /// <summary>
        /// 更新时调用
        /// </summary>
        Task<RequestResult> OnUpdate(string oldVersion, string newVersion, params object[] args);

        /// <summary>
        /// 从主程序请求数据
        /// </summary>
        Task<List<object>> RequestPhobos(params object[] args);

        /// <summary>
        /// 主程序/其他插件调用插件方法
        /// </summary>
        Task<RequestResult> Run(params object[] args);

        /// <summary>
        /// 向主程序发送链接关联请求
        /// </summary>
        Task<RequestResult> Link(LinkAssociation association);

        /// <summary>
        /// 向主程序发送命令请求
        /// </summary>
        Task<RequestResult> Request(string command, Action<RequestResult>? callback = null, params object[] args);

        /// <summary>
        /// 请求设置为默认打开方式
        /// </summary>
        Task<RequestResult> LinkDefault(string protocol);

        /// <summary>
        /// 读取插件配置
        /// </summary>
        Task<ConfigResult> ReadConfig(string key, string? targetPackageName = null);

        /// <summary>
        /// 写入插件配置
        /// </summary>
        Task<ConfigResult> WriteConfig(string key, string value, string? targetPackageName = null);

        /// <summary>
        /// 读取系统配置
        /// </summary>
        Task<ConfigResult> ReadSysConfig(string key);

        /// <summary>
        /// 写入系统配置
        /// </summary>
        Task<ConfigResult> WriteSysConfig(string key, string value);

        /// <summary>
        /// 注册随 Phobos 启动的命令
        /// </summary>
        Task<BootResult> BootWithPhobos(string command, int priority = 100, params object[] args);

        /// <summary>
        /// 取消随 Phobos 启动
        /// </summary>
        Task<BootResult> RemoveBootWithPhobos(string? uuid = null);

        /// <summary>
        /// 获取本插件的所有启动项
        /// </summary>
        Task<List<object>> GetBootItems();

        /// <summary>
        /// 订阅主程序事件
        /// </summary>
        /// <param name="eventId">事件大类 ID</param>
        /// <param name="eventName">事件小类名称</param>
        /// <param name="args">预留参数</param>
        Task<RequestResult> Subscribe(string eventId, string eventName, params object[] args);

        /// <summary>
        /// 取消订阅主程序事件
        /// </summary>
        /// <param name="eventId">事件大类 ID</param>
        /// <param name="eventName">事件小类名称</param>
        /// <param name="args">预留参数</param>
        Task<RequestResult> Unsubscribe(string eventId, string eventName, params object[] args);

        /// <summary>
        /// 获取主程序的主题资源字典，用于插件窗口保持主题一致
        /// </summary>
        ResourceDictionary? GetMergedDictionaries();

        /// <summary>
        /// 主程序通知插件事件触发（由主程序调用）
        /// </summary>
        /// <param name="eventId">事件大类 ID</param>
        /// <param name="eventName">事件小类名称</param>
        /// <param name="args">事件参数</param>
        Task OnEventReceived(string eventId, string eventName, params object[] args);

        #region Logger 方法

        /// <summary>
        /// 记录调试日志
        /// </summary>
        void LogDebug(string message, params object[] args);

        /// <summary>
        /// 记录信息日志
        /// </summary>
        void LogInfo(string message, params object[] args);

        /// <summary>
        /// 记录警告日志
        /// </summary>
        void LogWarning(string message, params object[] args);

        /// <summary>
        /// 记录错误日志
        /// </summary>
        void LogError(string message, Exception? exception = null, params object[] args);

        /// <summary>
        /// 记录严重错误日志
        /// </summary>
        void LogCritical(string message, Exception? exception = null, params object[] args);

        #endregion
    }

    /// <summary>
    /// 订阅结果
    /// </summary>
    public class SubscribeResult
    {
        public bool Success { get; set; }
        public string EventId { get; set; } = string.Empty;
        public string EventName { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }

    /// <summary>
    /// 预定义的事件 ID
    /// </summary>
    public static class PhobosEventIds
    {
        /// <summary>
        /// 主题相关事件
        /// </summary>
        public const string Theme = "Theme";

        /// <summary>
        /// 语言相关事件
        /// </summary>
        public const string Language = "Language";

        /// <summary>
        /// 插件相关事件
        /// </summary>
        public const string Plugin = "Plugin";

        /// <summary>
        /// 系统相关事件
        /// </summary>
        public const string System = "System";

        /// <summary>
        /// 窗口相关事件
        /// </summary>
        public const string Window = "Window";
    }

    /// <summary>
    /// 预定义的事件名称
    /// </summary>
    public static class PhobosEventNames
    {
        // Theme 事件
        public const string ThemeChanged = "Changed";
        public const string ThemeLoaded = "Loaded";

        // Language 事件
        public const string LanguageChanged = "Changed";

        // Plugin 事件
        public const string PluginInstalled = "Installed";
        public const string PluginUninstalled = "Uninstalled";
        public const string PluginEnabled = "Enabled";
        public const string PluginDisabled = "Disabled";

        // System 事件
        public const string SystemShutdown = "Shutdown";
        public const string SystemSuspend = "Suspend";
        public const string SystemResume = "Resume";

        // Window 事件
        public const string WindowActivated = "Activated";
        public const string WindowDeactivated = "Deactivated";
        public const string WindowMinimized = "Minimized";
        public const string WindowRestored = "Restored";
    }

    /// <summary>
    /// 日志级别
    /// </summary>
    public enum LogLevel
    {
        Debug,
        Info,
        Warning,
        Error,
        Critical
    }

    /// <summary>
    /// 日志条目
    /// </summary>
    public class LogEntry
    {
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public LogLevel Level { get; set; } = LogLevel.Info;
        public string Source { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public Exception? Exception { get; set; }
        public object[]? Args { get; set; }

        public override string ToString()
        {
            var msg = $"[{Timestamp:yyyy-MM-dd HH:mm:ss}] [{Level}] [{Source}] {Message}";
            if (Exception != null)
            {
                msg += $"{Environment.NewLine}Exception: {Exception.GetType().Name}: {Exception.Message}";
            }
            return msg;
        }
    }
}
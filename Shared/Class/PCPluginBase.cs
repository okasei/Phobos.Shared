using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using Phobos.Shared.Interface;

namespace Phobos.Shared.Class
{
    /// <summary>
    /// 插件处理器委托定义 - 带调用者上下文
    /// </summary>
    public class PluginHandlers
    {
        public Func<PluginCallerContext, object[], Task<List<object>>>? RequestPhobos { get; set; }
        public Func<PluginCallerContext, LinkAssociation, Task<RequestResult>>? Link { get; set; }
        public Func<PluginCallerContext, string, Action<RequestResult>?, object[], Task<RequestResult>>? Request { get; set; }
        public Func<PluginCallerContext, string, Task<RequestResult>>? LinkDefault { get; set; }
        public Func<PluginCallerContext, string, string?, Task<ConfigResult>>? ReadConfig { get; set; }
        public Func<PluginCallerContext, string, string, string?, Task<ConfigResult>>? WriteConfig { get; set; }
        public Func<PluginCallerContext, string, Task<ConfigResult>>? ReadSysConfig { get; set; }
        public Func<PluginCallerContext, string, string, Task<ConfigResult>>? WriteSysConfig { get; set; }
        public Func<PluginCallerContext, string, int, object[], Task<BootResult>>? BootWithPhobos { get; set; }
        public Func<PluginCallerContext, string?, Task<BootResult>>? RemoveBootWithPhobos { get; set; }
        public Func<PluginCallerContext, Task<List<object>>>? GetBootItems { get; set; }
        public Func<PluginCallerContext, string, string, object[], Task<RequestResult>>? Subscribe { get; set; }
        public Func<PluginCallerContext, string, string, object[], Task<RequestResult>>? Unsubscribe { get; set; }
        public Func<PluginCallerContext, ResourceDictionary?>? GetMergedDictionaries { get; set; }

        // Logger 处理器
        public Func<PluginCallerContext, LogLevel, string, Exception?, object[]?, Task<RequestResult>>? Log { get; set; }
    }

    /// <summary>
    /// 插件基类，提供默认实现
    /// </summary>
    public abstract class PCPluginBase : IPhobosPlugin
    {
        private PluginHandlers? _handlers;

        /// <summary>
        /// 当前订阅的事件列表（用于关闭时自动取消订阅）
        /// </summary>
        private readonly List<(string EventId, string EventName)> _subscriptions = new();

        /// <summary>
        /// 插件元数据
        /// </summary>
        public abstract PluginMetadata Metadata { get; }

        /// <summary>
        /// 插件内容区域
        /// </summary>
        public virtual FrameworkElement? ContentArea { get; protected set; }

        /// <summary>
        /// 获取当前插件的调用者上下文
        /// </summary>
        protected PluginCallerContext GetCallerContext()
        {
            return new PluginCallerContext
            {
                Name = Metadata.LocalizedNames,
                PackageName = Metadata.PackageName,
                DatabaseKey = Metadata.DatabaseKey,
                IsTrusted = false, // 默认不信任，由主程序判断
                CallTime = DateTime.Now
            };
        }

        /// <summary>
        /// 设置主程序处理器
        /// </summary>
        public void SetPhobosHandlers(PluginHandlers handlers)
        {
            _handlers = handlers;
        }

        public virtual async Task<RequestResult> OnInstall(params object[] args)
        {
            return await Task.FromResult(new RequestResult { Success = true, Message = "Installed successfully" });
        }

        public virtual async Task<RequestResult> OnLaunch(params object[] args)
        {
            return await Task.FromResult(new RequestResult { Success = true, Message = "Launched successfully" });
        }

        public virtual async Task<RequestResult> OnClosing(params object[] args)
        {
            // 自动取消所有订阅
            await UnsubscribeAll();
            return new RequestResult { Success = true, Message = "Closing" };
        }

        public virtual async Task<RequestResult> OnUninstall(params object[] args)
        {
            // 确保取消所有订阅
            await UnsubscribeAll();
            return new RequestResult { Success = true, Message = "Uninstalled successfully" };
        }

        public virtual async Task<RequestResult> OnUpdate(string oldVersion, string newVersion, params object[] args)
        {
            return await Task.FromResult(new RequestResult { Success = true, Message = $"Updated from {oldVersion} to {newVersion}" });
        }

        public virtual async Task<List<object>> RequestPhobos(params object[] args)
        {
            if (_handlers?.RequestPhobos != null)
                return await _handlers.RequestPhobos(GetCallerContext(), args);
            return new List<object>();
        }

        public virtual async Task<RequestResult> Run(params object[] args)
        {
            return await Task.FromResult(new RequestResult { Success = true, Message = "Run completed" });
        }

        public virtual async Task<RequestResult> Link(LinkAssociation association)
        {
            if (_handlers?.Link != null)
                return await _handlers.Link(GetCallerContext(), association);
            return new RequestResult { Success = false, Message = "Link handler not set" };
        }

        public virtual async Task<RequestResult> Request(string command, Action<RequestResult>? callback = null, params object[] args)
        {
            if (_handlers?.Request != null)
                return await _handlers.Request(GetCallerContext(), command, callback, args);
            return new RequestResult { Success = false, Message = "Request handler not set" };
        }

        public virtual async Task<RequestResult> LinkDefault(string protocol)
        {
            if (_handlers?.LinkDefault != null)
                return await _handlers.LinkDefault(GetCallerContext(), protocol);
            return new RequestResult { Success = false, Message = "LinkDefault handler not set" };
        }

        public virtual async Task<ConfigResult> ReadConfig(string key, string? targetPackageName = null)
        {
            if (_handlers?.ReadConfig != null)
                return await _handlers.ReadConfig(GetCallerContext(), key, targetPackageName ?? Metadata.PackageName);
            return new ConfigResult { Success = false, Key = key, Message = "ReadConfig handler not set" };
        }

        public virtual async Task<ConfigResult> WriteConfig(string key, string value, string? targetPackageName = null)
        {
            if (_handlers?.WriteConfig != null)
                return await _handlers.WriteConfig(GetCallerContext(), key, value, targetPackageName ?? Metadata.PackageName);
            return new ConfigResult { Success = false, Key = key, Message = "WriteConfig handler not set" };
        }

        public virtual async Task<ConfigResult> ReadSysConfig(string key)
        {
            if (_handlers?.ReadSysConfig != null)
                return await _handlers.ReadSysConfig(GetCallerContext(), key);
            return new ConfigResult { Success = false, Key = key, Message = "ReadSysConfig handler not set" };
        }

        public virtual async Task<ConfigResult> WriteSysConfig(string key, string value)
        {
            if (_handlers?.WriteSysConfig != null)
                return await _handlers.WriteSysConfig(GetCallerContext(), key, value);
            return new ConfigResult { Success = false, Key = key, Message = "WriteSysConfig handler not set" };
        }

        public virtual async Task<BootResult> BootWithPhobos(string command, int priority = 100, params object[] args)
        {
            if (_handlers?.BootWithPhobos != null)
                return await _handlers.BootWithPhobos(GetCallerContext(), command, priority, args);
            return new BootResult { Success = false, Message = "BootWithPhobos handler not set" };
        }

        public virtual async Task<BootResult> RemoveBootWithPhobos(string? uuid = null)
        {
            if (_handlers?.RemoveBootWithPhobos != null)
                return await _handlers.RemoveBootWithPhobos(GetCallerContext(), uuid);
            return new BootResult { Success = false, Message = "RemoveBootWithPhobos handler not set" };
        }

        public virtual async Task<List<object>> GetBootItems()
        {
            if (_handlers?.GetBootItems != null)
                return await _handlers.GetBootItems(GetCallerContext());
            return new List<object>();
        }

        #region 订阅管理

        public virtual async Task<RequestResult> Subscribe(string eventId, string eventName, params object[] args)
        {
            if (_handlers?.Subscribe != null)
            {
                var result = await _handlers.Subscribe(GetCallerContext(), eventId, eventName, args);
                if (result.Success)
                {
                    // 记录订阅，以便关闭时自动取消
                    if (!_subscriptions.Contains((eventId, eventName)))
                    {
                        _subscriptions.Add((eventId, eventName));
                    }
                }
                return result;
            }
            return new RequestResult { Success = false, Message = "Subscribe handler not set" };
        }

        public virtual async Task<RequestResult> Unsubscribe(string eventId, string eventName, params object[] args)
        {
            if (_handlers?.Unsubscribe != null)
            {
                var result = await _handlers.Unsubscribe(GetCallerContext(), eventId, eventName, args);
                if (result.Success)
                {
                    _subscriptions.Remove((eventId, eventName));
                }
                return result;
            }
            return new RequestResult { Success = false, Message = "Unsubscribe handler not set" };
        }

        /// <summary>
        /// 取消所有订阅
        /// </summary>
        protected async Task UnsubscribeAll()
        {
            if (_handlers?.Unsubscribe == null)
                return;

            var subscriptionsCopy = new List<(string, string)>(_subscriptions);
            foreach (var (eventId, eventName) in subscriptionsCopy)
            {
                try
                {
                    await _handlers.Unsubscribe(GetCallerContext(), eventId, eventName, Array.Empty<object>());
                }
                catch
                {
                    // 忽略取消订阅时的错误
                }
            }
            _subscriptions.Clear();
        }

        /// <summary>
        /// 主程序调用此方法通知事件
        /// </summary>
        public virtual async Task OnEventReceived(string eventId, string eventName, params object[] args)
        {
            // 默认实现：子类可以重写此方法处理事件
            await Task.CompletedTask;

            // 也可以触发 C# 事件
            EventReceived?.Invoke(this, new PluginEventArgs
            {
                EventId = eventId,
                EventName = eventName,
                Args = args
            });
        }

        /// <summary>
        /// 事件接收事件（可选：供子类使用事件模式）
        /// </summary>
        public event EventHandler<PluginEventArgs>? EventReceived;

        #endregion

        #region 主题获取

        public virtual ResourceDictionary? GetMergedDictionaries()
        {
            if (_handlers?.GetMergedDictionaries != null)
                return _handlers.GetMergedDictionaries(GetCallerContext());
            return null;
        }

        /// <summary>
        /// 将主程序主题应用到窗口
        /// </summary>
        protected void ApplyThemeToWindow(Window window)
        {
            var themeResources = GetMergedDictionaries();
            if (themeResources != null && window != null)
            {
                // 确保不重复添加
                if (!window.Resources.MergedDictionaries.Contains(themeResources))
                {
                    window.Resources.MergedDictionaries.Add(themeResources);
                }
            }
        }

        #endregion

        #region Logger

        /// <summary>
        /// 记录日志的核心方法
        /// </summary>
        private void Log(LogLevel level, string message, Exception? exception = null, params object[]? args)
        {
            if (_handlers?.Log != null)
            {
                _handlers.Log(GetCallerContext(), level, message, exception, args);
            }
            else
            {
                // 回退到 Debug 输出
                var entry = new LogEntry
                {
                    Timestamp = DateTime.Now,
                    Level = level,
                    Source = Metadata.PackageName,
                    Message = message,
                    Exception = exception,
                    Args = args
                };
                Debug.WriteLine(entry.ToString());
            }
        }

        public void LogDebug(string message, params object[] args)
        {
            Log(LogLevel.Debug, message, null, args);
        }

        public void LogInfo(string message, params object[] args)
        {
            Log(LogLevel.Info, message, null, args);
        }

        public void LogWarning(string message, params object[] args)
        {
            Log(LogLevel.Warning, message, null, args);
        }

        public void LogError(string message, Exception? exception = null, params object[] args)
        {
            Log(LogLevel.Error, message, exception, args);
        }

        public void LogCritical(string message, Exception? exception = null, params object[] args)
        {
            Log(LogLevel.Critical, message, exception, args);
        }

        #endregion
    }

    /// <summary>
    /// 插件事件参数
    /// </summary>
    public class PluginEventArgs : EventArgs
    {
        public string EventId { get; set; } = string.Empty;
        public string EventName { get; set; } = string.Empty;
        public object[] Args { get; set; } = Array.Empty<object>();
    }
}
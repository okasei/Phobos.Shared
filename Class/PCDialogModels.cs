using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Phobos.Shared.Class
{
    /// <summary>
    /// 对话框内容模式
    /// </summary>
    public enum DialogContentMode
    {
        /// <summary>
        /// 模式1: 居中图片，下方文字
        /// </summary>
        ImageWithText = 1,

        /// <summary>
        /// 模式2: 居中文字
        /// </summary>
        CenteredText = 2,

        /// <summary>
        /// 模式3: 左对齐文字
        /// </summary>
        LeftAlignedText = 3,

        /// <summary>
        /// 自定义内容
        /// </summary>
        Custom = 0
    }

    /// <summary>
    /// 对话框按钮类型
    /// </summary>
    public enum DialogButtonType
    {
        /// <summary>
        /// 主要按钮（最右侧，高亮样式）
        /// </summary>
        Primary,

        /// <summary>
        /// 次要按钮
        /// </summary>
        Secondary,

        /// <summary>
        /// 取消按钮（最左侧）
        /// </summary>
        Cancel
    }

    /// <summary>
    /// 对话框结果
    /// </summary>
    public enum DialogResult
    {
        None = 0,
        Cancel = 1,
        Button1 = 2,  // 最右侧 Primary
        Button2 = 3,
        Button3 = 4,
        Button4 = 5
    }

    /// <summary>
    /// 对话框定位模式
    /// </summary>
    public enum DialogPositionMode
    {
        /// <summary>
        /// 居中于屏幕
        /// </summary>
        CenterScreen,

        /// <summary>
        /// 居中于父窗口
        /// </summary>
        CenterOwner,

        /// <summary>
        /// 自定义位置
        /// </summary>
        Custom
    }

    /// <summary>
    /// 对话框按钮配置
    /// </summary>
    public class DialogButton
    {
        /// <summary>
        /// 按钮文本
        /// </summary>
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// 本地化文本
        /// </summary>
        public Dictionary<string, string>? LocalizedTexts { get; set; }

        /// <summary>
        /// 按钮类型
        /// </summary>
        public DialogButtonType ButtonType { get; set; } = DialogButtonType.Secondary;

        /// <summary>
        /// 按钮标识（用于结果识别）
        /// </summary>
        public string Tag { get; set; } = string.Empty;

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// 点击时是否关闭对话框
        /// </summary>
        public bool CloseOnClick { get; set; } = true;

        /// <summary>
        /// 按下回调
        /// </summary>
        public Action<DialogButton>? OnClick { get; set; }

        /// <summary>
        /// 获取本地化文本
        /// </summary>
        public string GetLocalizedText(string languageCode)
        {
            if (LocalizedTexts != null && LocalizedTexts.TryGetValue(languageCode, out var text))
                return text;
            if (LocalizedTexts != null && LocalizedTexts.TryGetValue("en-US", out var fallback))
                return fallback;
            return Text;
        }
    }

    /// <summary>
    /// 对话框配置
    /// </summary>
    public class DialogConfig
    {
        /// <summary>
        /// 对话框标题
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// 本地化标题
        /// </summary>
        public Dictionary<string, string>? LocalizedTitles { get; set; }

        /// <summary>
        /// 调用者图标路径
        /// </summary>
        public string? CallerIconPath { get; set; }

        /// <summary>
        /// 调用者图标（ImageSource）
        /// </summary>
        public ImageSource? CallerIcon { get; set; }

        /// <summary>
        /// 内容模式
        /// </summary>
        public DialogContentMode ContentMode { get; set; } = DialogContentMode.CenteredText;

        /// <summary>
        /// 内容图片路径（模式1）
        /// </summary>
        public string? ContentImagePath { get; set; }

        /// <summary>
        /// 内容图片（ImageSource）
        /// </summary>
        public ImageSource? ContentImage { get; set; }

        /// <summary>
        /// 内容图片最大宽度
        /// </summary>
        public double ContentImageMaxWidth { get; set; } = 200;

        /// <summary>
        /// 内容图片最大高度
        /// </summary>
        public double ContentImageMaxHeight { get; set; } = 150;

        /// <summary>
        /// 内容文本
        /// </summary>
        public string? ContentText { get; set; }

        /// <summary>
        /// 本地化内容文本
        /// </summary>
        public Dictionary<string, string>? LocalizedContentTexts { get; set; }

        /// <summary>
        /// 自定义内容区域
        /// </summary>
        public FrameworkElement? CustomContent { get; set; }

        /// <summary>
        /// 自定义按钮区域
        /// </summary>
        public FrameworkElement? CustomButtonArea { get; set; }

        /// <summary>
        /// 按钮配置（从右到左排列，最右侧为 Primary）
        /// </summary>
        public List<DialogButton> Buttons { get; set; } = new();

        /// <summary>
        /// 显示按钮数量（2-4）
        /// </summary>
        public int VisibleButtonCount { get; set; } = 2;

        /// <summary>
        /// 显示取消按钮
        /// </summary>
        public bool ShowCancelButton { get; set; } = true;

        /// <summary>
        /// 取消按钮文本
        /// </summary>
        public string CancelButtonText { get; set; } = "Cancel";

        /// <summary>
        /// 本地化取消按钮文本
        /// </summary>
        public Dictionary<string, string>? LocalizedCancelButtonTexts { get; set; }

        /// <summary>
        /// 对话框宽度
        /// </summary>
        public double Width { get; set; } = 420;

        /// <summary>
        /// 对话框最小高度
        /// </summary>
        public double MinHeight { get; set; } = 200;

        /// <summary>
        /// 对话框最大高度
        /// </summary>
        public double MaxHeight { get; set; } = 600;

        /// <summary>
        /// 定位模式
        /// </summary>
        public DialogPositionMode PositionMode { get; set; } = DialogPositionMode.CenterScreen;

        /// <summary>
        /// 父窗口（用于 CenterOwner 模式）
        /// </summary>
        public Window? OwnerWindow { get; set; }

        /// <summary>
        /// 自定义位置偏移量
        /// </summary>
        public Point CustomOffset { get; set; } = new Point(0, 0);

        /// <summary>
        /// 自定义绝对位置
        /// </summary>
        public Point? CustomPosition { get; set; }

        /// <summary>
        /// 是否可拖动
        /// </summary>
        public bool IsDraggable { get; set; } = true;

        /// <summary>
        /// 是否显示关闭按钮
        /// </summary>
        public bool ShowCloseButton { get; set; } = false;

        /// <summary>
        /// 按 ESC 关闭
        /// </summary>
        public bool CloseOnEscape { get; set; } = true;

        /// <summary>
        /// 点击背景关闭（仅模态对话框）
        /// </summary>
        public bool CloseOnBackgroundClick { get; set; } = false;

        /// <summary>
        /// 是否为模态对话框
        /// </summary>
        public bool IsModal { get; set; } = true;

        /// <summary>
        /// 获取本地化标题
        /// </summary>
        public string? GetLocalizedTitle(string languageCode)
        {
            if (LocalizedTitles != null && LocalizedTitles.TryGetValue(languageCode, out var text))
                return text;
            if (LocalizedTitles != null && LocalizedTitles.TryGetValue("en-US", out var fallback))
                return fallback;
            return Title;
        }

        /// <summary>
        /// 获取本地化内容文本
        /// </summary>
        public string? GetLocalizedContentText(string languageCode)
        {
            if (LocalizedContentTexts != null && LocalizedContentTexts.TryGetValue(languageCode, out var text))
                return text;
            if (LocalizedContentTexts != null && LocalizedContentTexts.TryGetValue("en-US", out var fallback))
                return fallback;
            return ContentText;
        }

        /// <summary>
        /// 获取本地化取消按钮文本
        /// </summary>
        public string GetLocalizedCancelButtonText(string languageCode)
        {
            if (LocalizedCancelButtonTexts != null && LocalizedCancelButtonTexts.TryGetValue(languageCode, out var text))
                return text;
            if (LocalizedCancelButtonTexts != null && LocalizedCancelButtonTexts.TryGetValue("en-US", out var fallback))
                return fallback;
            return CancelButtonText;
        }
    }

    /// <summary>
    /// 对话框回调结果
    /// </summary>
    public class DialogCallbackResult
    {
        /// <summary>
        /// 对话框结果
        /// </summary>
        public DialogResult Result { get; set; } = DialogResult.None;

        /// <summary>
        /// 点击的按钮
        /// </summary>
        public DialogButton? ClickedButton { get; set; }

        /// <summary>
        /// 按钮标识
        /// </summary>
        public string? ButtonTag { get; set; }

        /// <summary>
        /// 是否被取消
        /// </summary>
        public bool IsCancelled => Result == DialogResult.Cancel;

        /// <summary>
        /// 附加数据
        /// </summary>
        public object? Data { get; set; }
    }

    /// <summary>
    /// 预设对话框类型
    /// </summary>
    public static class DialogPresets
    {
        /// <summary>
        /// 确认对话框（确定/取消）
        /// </summary>
        public static DialogConfig Confirm(string message, string? title = null)
        {
            return new DialogConfig
            {
                Title = title ?? "Confirm",
                LocalizedTitles = new Dictionary<string, string>
                {
                    { "en-US", title ?? "Confirm" },
                    { "zh-CN", title ?? "确认" },
                    { "zh-TW", title ?? "確認" },
                    { "ja-JP", title ?? "確認" }
                },
                ContentMode = DialogContentMode.CenteredText,
                ContentText = message,
                ShowCancelButton = true,
                CancelButtonText = "Cancel",
                LocalizedCancelButtonTexts = new Dictionary<string, string>
                {
                    { "en-US", "Cancel" },
                    { "zh-CN", "取消" },
                    { "zh-TW", "取消" },
                    { "ja-JP", "キャンセル" }
                },
                VisibleButtonCount = 2,
                Buttons = new List<DialogButton>
                {
                    new DialogButton
                    {
                        Text = "OK",
                        LocalizedTexts = new Dictionary<string, string>
                        {
                            { "en-US", "OK" },
                            { "zh-CN", "确定" },
                            { "zh-TW", "確定" },
                            { "ja-JP", "OK" }
                        },
                        ButtonType = DialogButtonType.Primary,
                        Tag = "ok"
                    }
                }
            };
        }

        /// <summary>
        /// 信息对话框
        /// </summary>
        public static DialogConfig Info(string message, string? title = null)
        {
            return new DialogConfig
            {
                Title = title ?? "Information",
                LocalizedTitles = new Dictionary<string, string>
                {
                    { "en-US", title ?? "Information" },
                    { "zh-CN", title ?? "信息" },
                    { "zh-TW", title ?? "資訊" },
                    { "ja-JP", title ?? "情報" }
                },
                ContentMode = DialogContentMode.CenteredText,
                ContentText = message,
                ShowCancelButton = false,
                VisibleButtonCount = 2,
                Buttons = new List<DialogButton>
                {
                    new DialogButton
                    {
                        Text = "OK",
                        LocalizedTexts = new Dictionary<string, string>
                        {
                            { "en-US", "OK" },
                            { "zh-CN", "确定" },
                            { "zh-TW", "確定" },
                            { "ja-JP", "OK" }
                        },
                        ButtonType = DialogButtonType.Primary,
                        Tag = "ok"
                    }
                }
            };
        }

        /// <summary>
        /// 警告对话框
        /// </summary>
        public static DialogConfig Warning(string message, string? title = null)
        {
            var config = Info(message, title ?? "Warning");
            config.LocalizedTitles = new Dictionary<string, string>
            {
                { "en-US", title ?? "Warning" },
                { "zh-CN", title ?? "警告" },
                { "zh-TW", title ?? "警告" },
                { "ja-JP", title ?? "警告" }
            };
            return config;
        }

        /// <summary>
        /// 错误对话框
        /// </summary>
        public static DialogConfig Error(string message, string? title = null)
        {
            var config = Info(message, title ?? "Error");
            config.LocalizedTitles = new Dictionary<string, string>
            {
                { "en-US", title ?? "Error" },
                { "zh-CN", title ?? "错误" },
                { "zh-TW", title ?? "錯誤" },
                { "ja-JP", title ?? "エラー" }
            };
            return config;
        }

        /// <summary>
        /// 是/否对话框
        /// </summary>
        public static DialogConfig YesNo(string message, string? title = null)
        {
            return new DialogConfig
            {
                Title = title ?? "Question",
                LocalizedTitles = new Dictionary<string, string>
                {
                    { "en-US", title ?? "Question" },
                    { "zh-CN", title ?? "询问" },
                    { "zh-TW", title ?? "詢問" },
                    { "ja-JP", title ?? "質問" }
                },
                ContentMode = DialogContentMode.CenteredText,
                ContentText = message,
                ShowCancelButton = false,
                VisibleButtonCount = 2,
                Buttons = new List<DialogButton>
                {
                    new DialogButton
                    {
                        Text = "Yes",
                        LocalizedTexts = new Dictionary<string, string>
                        {
                            { "en-US", "Yes" },
                            { "zh-CN", "是" },
                            { "zh-TW", "是" },
                            { "ja-JP", "はい" }
                        },
                        ButtonType = DialogButtonType.Primary,
                        Tag = "yes"
                    },
                    new DialogButton
                    {
                        Text = "No",
                        LocalizedTexts = new Dictionary<string, string>
                        {
                            { "en-US", "No" },
                            { "zh-CN", "否" },
                            { "zh-TW", "否" },
                            { "ja-JP", "いいえ" }
                        },
                        ButtonType = DialogButtonType.Secondary,
                        Tag = "no"
                    }
                }
            };
        }

        /// <summary>
        /// 是/否/取消对话框
        /// </summary>
        public static DialogConfig YesNoCancel(string message, string? title = null)
        {
            var config = YesNo(message, title);
            config.ShowCancelButton = true;
            config.CancelButtonText = "Cancel";
            config.LocalizedCancelButtonTexts = new Dictionary<string, string>
            {
                { "en-US", "Cancel" },
                { "zh-CN", "取消" },
                { "zh-TW", "取消" },
                { "ja-JP", "キャンセル" }
            };
            return config;
        }
    }
}

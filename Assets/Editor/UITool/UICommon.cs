public interface UICommon
{
    //用于匹配符合规则的组件元素，例如 [Button]Close
    //字符串前 @ 表示取消转义
    public static string COMPONENT_PATTERN = @"^\[[A-Z][A-z]+\]";
}
/// <summary>
/// 组件数据
/// </summary>
public class EditorObjectData
{
    public int instId;
    public string fieldName;
    public string fieldType;

    public EditorObjectData() {}

    public EditorObjectData(int instId, string fieldName, string fieldType)
    {
        this.instId = instId;
        this.fieldName = fieldName;
        this.fieldType = fieldType;
    }
}
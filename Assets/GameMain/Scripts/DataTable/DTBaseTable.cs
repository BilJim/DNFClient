//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2024-04-30 14:03:28.262
//------------------------------------------------------------

using GameFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 数据表。
/// </summary>
public class DTBaseTable : DataRowBase
{
    private int m_Id = 0;

    /// <summary>
    /// 获取编号。
    /// </summary>
    public override int Id
    {
        get
        {
            return m_Id;
        }
    }

	/// <summary>
	/// 获取要加载的数据表。
	/// </summary>
	public List<string> TableNames
	{
		get;
		private set;
	}

	public override bool ParseDataRow(string dataRowString, object userData)
	{
		string[] columnStrings = dataRowString.Split(DataTableExtension.DataSplitSeparators);
		for (int i = 0; i < columnStrings.Length; i++)
		{
			columnStrings[i] = columnStrings[i].Trim(DataTableExtension.DataTrimSeparators);
		}

		int index = 0;
		index++;
		m_Id = int.Parse(columnStrings[index++]);
		index++;
		TableNames = DataTableExtension.ParseStringList(columnStrings[index++]);

		GeneratePropertyArray();
		return true;
	}

	public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
	{
		using (MemoryStream memoryStream = new MemoryStream(dataRowBytes, startIndex, length, false))
		{
			using (BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))
			{
				m_Id = binaryReader.Read7BitEncodedInt32();
				TableNames = binaryReader.ReadStringList();
			}
		}

		GeneratePropertyArray();
		return true;
	}


	private void GeneratePropertyArray()
	{

	}
}
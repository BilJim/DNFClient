﻿//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2024-05-01 10:05:53.498
//------------------------------------------------------------

using GameFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 场景配置表。
/// </summary>
public class DTScene : DataRowBase
{
    private int m_Id = 0;

    /// <summary>
    /// 获取场景编号。
    /// </summary>
    public override int Id
    {
        get
        {
            return m_Id;
        }
    }

	/// <summary>
	/// 获取资源名称。
	/// </summary>
	public string AssetName
	{
		get;
		private set;
	}

	/// <summary>
	/// 获取资源地址。
	/// </summary>
	public string AssetPath
	{
		get;
		private set;
	}

	/// <summary>
	/// 获取背景音乐编号。
	/// </summary>
	public int BackgroundMusicId
	{
		get;
		private set;
	}

	/// <summary>
	/// 获取默认要打开的UI表单，0 表示无。
	/// </summary>
	public int DefaultFormId
	{
		get;
		private set;
	}

	/// <summary>
	/// 获取所属流程类。通过反射切换不同场景流程。
	/// </summary>
	public string TypeStr
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
		AssetName = columnStrings[index++];
		AssetPath = columnStrings[index++];
		BackgroundMusicId = int.Parse(columnStrings[index++]);
		DefaultFormId = int.Parse(columnStrings[index++]);
		TypeStr = columnStrings[index++];

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
				AssetName = binaryReader.ReadString();
				AssetPath = binaryReader.ReadString();
				BackgroundMusicId = binaryReader.Read7BitEncodedInt32();
				DefaultFormId = binaryReader.Read7BitEncodedInt32();
				TypeStr = binaryReader.ReadString();
			}
		}

		GeneratePropertyArray();
		return true;
	}


	private void GeneratePropertyArray()
	{

	}
}
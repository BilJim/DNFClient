﻿//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2024-05-20 10:29:36.053
//------------------------------------------------------------

using GameFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 实体表。
/// </summary>
public class DTMonster : DataRowBase
{
    private int m_Id = 0;

    /// <summary>
    /// 获取实体编号。
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
	/// 获取资源地址(相对路径)。
	/// </summary>
	public string AssetPath
	{
		get;
		private set;
	}

	/// <summary>
	/// 获取出生点。
	/// </summary>
	public Vector3 BornPosition
	{
		get;
		private set;
	}

	/// <summary>
	/// 获取碰撞体中心点。
	/// </summary>
	public Vector3 ColliderCenter
	{
		get;
		private set;
	}

	/// <summary>
	/// 获取碰撞体Size。
	/// </summary>
	public Vector3 ColliderSize
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
		BornPosition = DataTableExtension.ParseVector3(columnStrings[index++]);
		ColliderCenter = DataTableExtension.ParseVector3(columnStrings[index++]);
		ColliderSize = DataTableExtension.ParseVector3(columnStrings[index++]);

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
				BornPosition = binaryReader.ReadVector3();
				ColliderCenter = binaryReader.ReadVector3();
				ColliderSize = binaryReader.ReadVector3();
			}
		}

		GeneratePropertyArray();
		return true;
	}


	private void GeneratePropertyArray()
	{

	}
}
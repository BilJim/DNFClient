﻿using System;
using GameFramework.DataTable;
using UnityGameFramework.Runtime;

public static class EntityExtension
{
    // 关于 EntityId 的约定：
    // 0 为无效
    // 正值用于和服务器通信的实体（如玩家角色、NPC、怪等，服务器只产生正值）
    // 负值用于本地生成的临时实体（如特效、FakeObject等）
    private static int s_SerialId = 0;

    public static void HideEntity(this EntityComponent entityComponent, Entity entity)
    {
        entityComponent.HideEntity(entity.Entity);
    }

    public static void AttachEntity(this EntityComponent entityComponent, Entity entity, int ownerId,
        string parentTransformPath = null, object userData = null)
    {
        entityComponent.AttachEntity(entity.Entity, ownerId, parentTransformPath, userData);
    }

    public static void ShowHeroEntity(this EntityComponent entityComponent, Type logicType, string entityGroup,
        int priority, EntityData data)
    {
        if (data == null)
        {
            Log.Warning("Data is invalid.");
            return;
        }

        IDataTable<DTHero> dtHero = GameEntry.DataTable.GetDataTable<DTHero>();
        DTHero heroData = dtHero.GetDataRow(data.TypeId);
        if (heroData == null)
        {
            Log.Warning("Can not load entity id '{0}' from data table.", data.TypeId.ToString());
            return;
        }
        
        entityComponent.ShowEntity(data.Id, logicType, heroData.AssetPath, entityGroup,
            priority, data);
    }

    public static int GenerateSerialId(this EntityComponent entityComponent)
    {
        return --s_SerialId;
    }
}
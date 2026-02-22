using System.Collections.Generic;
using EventCenter;
using UnityEngine;

public class TUIDataServer
{
	private static TUIDataServer instance;

	public static TUIDataServer Instance()
	{
		if (instance == null)
		{
			instance = new TUIDataServer();
		}
		return instance;
	}

	public void Initialize()
	{
		global::EventCenter.EventCenter.Instance.Register<TUIEvent.SendEvent_SceneMainMenu>(TUIEvent_BackInfo_SceneMainMenu);
		global::EventCenter.EventCenter.Instance.Register<TUIEvent.SendEvent_SceneEquip>(TUIEvent_BackInfo_SceneEquip);
		global::EventCenter.EventCenter.Instance.Register<TUIEvent.SendEvent_SceneStash>(TUIEvent_BackInfo_SceneStash);
		global::EventCenter.EventCenter.Instance.Register<TUIEvent.SendEvent_SceneSkill>(TUIEvent_BackInfo_SceneSkill);
		global::EventCenter.EventCenter.Instance.Register<TUIEvent.SendEvent_SceneForge>(TUIEvent_BackInfo_SceneForge);
		global::EventCenter.EventCenter.Instance.Register<TUIEvent.SendEvent_SceneTavern>(TUIEvent_BackInfo_SceneTavern);
		global::EventCenter.EventCenter.Instance.Register<TUIEvent.SendEvent_SceneMap>(TUIEvent_BackInfo_SceneMap);
		global::EventCenter.EventCenter.Instance.Register<TUIEvent.SendEvent_SceneIAP>(TUIEvent_BackInfo_SceneIAP);
	}

	private void TUIEvent_BackInfo_SceneMainMenu(object sender, TUIEvent.SendEvent_SceneMainMenu m_event)
	{
		if (m_event.GetEventName() == "TUIEvent_TopBar")
		{
			iGameData gameData = iGameApp.GetInstance().m_GameData;
			if (gameData == null)
			{
				return;
			}
			iDataCenter dataCenter = gameData.GetDataCenter();
			if (dataCenter == null)
			{
				return;
			}
			CCharSaveInfo character = dataCenter.GetCharacter(dataCenter.CurCharID);
			if (character != null)
			{
				CCharacterInfoLevel characterInfo = gameData.GetCharacterInfo(character.nID, character.nLevel);
				if (characterInfo != null)
				{
					TUIGameInfo tUIGameInfo = new TUIGameInfo();
					tUIGameInfo.player_info = new TUIPlayerInfo();
					tUIGameInfo.player_info.avatar_id = character.nID;
					tUIGameInfo.player_info.level = character.nLevel;
					tUIGameInfo.player_info.level_exp = characterInfo.nExp;
					tUIGameInfo.player_info.exp = character.nExp;
					tUIGameInfo.player_info.gold = dataCenter.Gold;
					tUIGameInfo.player_info.crystal = dataCenter.Crystal;
					global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.BackEvent_SceneMainMenu(m_event.GetEventName(), tUIGameInfo));
				}
			}
		}
		else if (m_event.GetEventName() == "TUIEvent_OptionInfo")
		{
			TUIGameInfo tUIGameInfo2 = new TUIGameInfo();
			tUIGameInfo2.option_info = new TUIOptionInfo();
			tUIGameInfo2.option_info.music_open = true;
			tUIGameInfo2.option_info.sfx_open = false;
			global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.BackEvent_SceneMainMenu(m_event.GetEventName(), tUIGameInfo2));
		}
		else if (m_event.GetEventName() == "TUIEvent_ChangeOption")
		{
			bool flag = m_event.GetWParam() == 1;
			bool flag2 = m_event.GetLparam() == 1;
			global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.BackEvent_SceneMainMenu(m_event.GetEventName(), true));
		}
		else if (!(m_event.GetEventName() == "TUIEvent_SetAcheviement"))
		{
		}
	}

	private void TUIEvent_BackInfo_SceneEquip(object sender, TUIEvent.SendEvent_SceneEquip m_event)
	{
		if (m_event.GetEventName() == "TUIEvent_TopBar")
		{
			iGameData gameData = iGameApp.GetInstance().m_GameData;
			if (gameData == null)
			{
				return;
			}
			iDataCenter dataCenter = gameData.GetDataCenter();
			if (dataCenter == null)
			{
				return;
			}
			CCharSaveInfo character = dataCenter.GetCharacter(dataCenter.CurCharID);
			if (character != null)
			{
				CCharacterInfoLevel characterInfo = gameData.GetCharacterInfo(character.nID, character.nLevel);
				if (characterInfo != null)
				{
					TUIGameInfo tUIGameInfo = new TUIGameInfo();
					tUIGameInfo.player_info = new TUIPlayerInfo();
					tUIGameInfo.player_info.avatar_id = character.nID;
					tUIGameInfo.player_info.level = character.nLevel;
					tUIGameInfo.player_info.level_exp = characterInfo.nExp;
					tUIGameInfo.player_info.exp = character.nExp;
					tUIGameInfo.player_info.gold = dataCenter.Gold;
					tUIGameInfo.player_info.crystal = dataCenter.Crystal;
					global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.BackEvent_SceneEquip(m_event.GetEventName(), tUIGameInfo));
				}
			}
		}
		else if (m_event.GetEventName() == "TUIEvent_RoleSign")
		{
			iGameData gameData2 = iGameApp.GetInstance().m_GameData;
			if (gameData2 == null)
			{
				return;
			}
			iDataCenter dataCenter2 = gameData2.GetDataCenter();
			if (dataCenter2 == null)
			{
				return;
			}
			CCharSaveInfo character2 = dataCenter2.GetCharacter(dataCenter2.CurCharID);
			if (character2 == null)
			{
				return;
			}
			TUIGameInfo tUIGameInfo2 = new TUIGameInfo();
			tUIGameInfo2.equip_info = new TUIEquipInfo();
			string name = "No Name";
			string introduce = "No Desc";
			CCharacterInfoLevel cCharacterInfoLevel = null;
			cCharacterInfoLevel = gameData2.GetCharacterInfo(dataCenter2.CurCharID, 1);
			if (cCharacterInfoLevel != null)
			{
				name = cCharacterInfoLevel.sName;
				introduce = cCharacterInfoLevel.sDesc;
			}
			tUIGameInfo2.equip_info.role = new TUIPopupInfo(dataCenter2.CurCharID, name, introduce);
			int[] array = new int[5] { 1, 2, 3, 4, 5 };
			tUIGameInfo2.equip_info.roles_list = new List<TUIPopupInfo>();
			for (int i = 0; i < array.Length; i++)
			{
				if (dataCenter2.GetCharacter(array[i]) != null)
				{
					cCharacterInfoLevel = gameData2.GetCharacterInfo(array[i], 1);
					if (cCharacterInfoLevel != null)
					{
						name = cCharacterInfoLevel.sName;
						introduce = cCharacterInfoLevel.sDesc;
					}
					tUIGameInfo2.equip_info.roles_list.Add(new TUIPopupInfo(array[i], name, introduce));
				}
			}
			global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.BackEvent_SceneEquip(m_event.GetEventName(), tUIGameInfo2));
		}
		else if (m_event.GetEventName() == "TUIEvent_SkillSign")
		{
			iGameData gameData3 = iGameApp.GetInstance().m_GameData;
			if (gameData3 == null)
			{
				return;
			}
			iDataCenter dataCenter3 = gameData3.GetDataCenter();
			if (dataCenter3 == null)
			{
				return;
			}
			iSkillCenter skillCenter = gameData3.GetSkillCenter();
			if (skillCenter == null)
			{
				return;
			}
			CCharSaveInfo character3 = dataCenter3.GetCharacter(dataCenter3.CurCharID);
			if (character3 == null)
			{
				return;
			}
			CCharacterInfoLevel characterInfo2 = gameData3.GetCharacterInfo(character3.nID, character3.nLevel);
			if (characterInfo2 == null)
			{
				return;
			}
			TUIGameInfo tUIGameInfo3 = new TUIGameInfo();
			tUIGameInfo3.equip_info = new TUIEquipInfo();
			int num = -1;
			int nSkillLevel = 0;
			CSkillInfoLevel cSkillInfoLevel = null;
			num = characterInfo2.nSkill;
			cSkillInfoLevel = gameData3.GetSkillInfo(num, 1);
			if (cSkillInfoLevel != null)
			{
				tUIGameInfo3.equip_info.skill01 = new TUIPopupInfo(num, cSkillInfoLevel.sName, cSkillInfoLevel.sDesc);
			}
			num = dataCenter3.GetSelectPassiveSkill(0);
			if (dataCenter3.GetPassiveSkill(num, ref nSkillLevel))
			{
				cSkillInfoLevel = gameData3.GetSkillInfo(num, nSkillLevel);
				if (cSkillInfoLevel != null)
				{
					tUIGameInfo3.equip_info.skill02 = new TUIPopupInfo(num, cSkillInfoLevel.sName, cSkillInfoLevel.sDesc);
				}
			}
			num = dataCenter3.GetSelectPassiveSkill(1);
			if (dataCenter3.GetPassiveSkill(num, ref nSkillLevel))
			{
				cSkillInfoLevel = gameData3.GetSkillInfo(num, nSkillLevel);
				if (cSkillInfoLevel != null)
				{
					tUIGameInfo3.equip_info.skill03 = new TUIPopupInfo(num, cSkillInfoLevel.sName, cSkillInfoLevel.sDesc);
				}
			}
			num = dataCenter3.GetSelectPassiveSkill(2);
			if (dataCenter3.GetPassiveSkill(num, ref nSkillLevel))
			{
				cSkillInfoLevel = gameData3.GetSkillInfo(num, nSkillLevel);
				if (cSkillInfoLevel != null)
				{
					tUIGameInfo3.equip_info.skill04 = new TUIPopupInfo(num, cSkillInfoLevel.sName, cSkillInfoLevel.sDesc);
				}
			}
			tUIGameInfo3.equip_info.skill_list = new List<TUIPopupInfo>();
			Dictionary<int, CSkillInfo> dataSkillInfo = skillCenter.GetDataSkillInfo();
			if (dataSkillInfo != null)
			{
				foreach (CSkillInfo value in dataSkillInfo.Values)
				{
					if (value.nID >= 10000)
					{
						continue;
					}
					int nSkillLevel2 = 0;
					if (dataCenter3.GetPassiveSkill(value.nID, ref nSkillLevel2))
					{
						cSkillInfoLevel = value.Get(nSkillLevel2);
						if (cSkillInfoLevel != null && cSkillInfoLevel.nType == 1)
						{
							tUIGameInfo3.equip_info.skill_list.Add(new TUIPopupInfo(value.nID, cSkillInfoLevel.sName, cSkillInfoLevel.sDesc));
						}
					}
				}
			}
			global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.BackEvent_SceneEquip(m_event.GetEventName(), tUIGameInfo3));
		}
		else if (m_event.GetEventName() == "TUIEvent_PropSign")
		{
			TUIGameInfo tUIGameInfo4 = new TUIGameInfo();
			tUIGameInfo4.equip_info = new TUIEquipInfo();
			tUIGameInfo4.equip_info.prop01 = new TUIPopupInfo(1, "Abundance", "This is Abundance", 11);
			tUIGameInfo4.equip_info.prop02 = new TUIPopupInfo(2, "Fury", "This is Fury", 44);
			global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.BackEvent_SceneEquip(m_event.GetEventName(), tUIGameInfo4));
		}
		else if (m_event.GetEventName() == "TUIEvent_WeaponSign")
		{
			iGameData gameData4 = iGameApp.GetInstance().m_GameData;
			if (gameData4 == null)
			{
				return;
			}
			iDataCenter dataCenter4 = gameData4.GetDataCenter();
			if (dataCenter4 == null)
			{
				return;
			}
			iWeaponCenter weaponCenter = gameData4.GetWeaponCenter();
			if (weaponCenter == null)
			{
				return;
			}
			iItemCenter itemCenter = gameData4.GetItemCenter();
			if (itemCenter == null)
			{
				return;
			}
			TUIGameInfo tUIGameInfo5 = new TUIGameInfo();
			tUIGameInfo5.equip_info = new TUIEquipInfo();
			int num2 = -1;
			int num3 = -1;
			CWeaponInfoLevel cWeaponInfoLevel = null;
			num2 = dataCenter4.GetSelectWeapon(0);
			num3 = dataCenter4.GetWeaponLevel(num2);
			cWeaponInfoLevel = gameData4.GetWeaponInfo(num2, num3);
			if (cWeaponInfoLevel != null)
			{
				TUIWeaponAttribute tUIWeaponAttribute = new TUIWeaponAttribute(0, 0, 0, 0, 0);
				tUIWeaponAttribute.ammo = cWeaponInfoLevel.nCapacity;
				tUIWeaponAttribute.blast_radius = 20;
				tUIWeaponAttribute.damage = (int)cWeaponInfoLevel.fDamage;
				tUIWeaponAttribute.fire_rate = (int)cWeaponInfoLevel.fShootSpeed;
				for (int j = 0; j < 3; j++)
				{
					if (cWeaponInfoLevel.arrFunc[j] == 4)
					{
						tUIWeaponAttribute.knockback = cWeaponInfoLevel.arrValueY[j];
						break;
					}
				}
				tUIGameInfo5.equip_info.weapon01 = new TUIPopupInfo(num2, cWeaponInfoLevel.sName, string.Empty, tUIWeaponAttribute);
			}
			num2 = dataCenter4.GetSelectWeapon(1);
			num3 = dataCenter4.GetWeaponLevel(num2);
			cWeaponInfoLevel = gameData4.GetWeaponInfo(num2, num3);
			if (cWeaponInfoLevel != null)
			{
				TUIWeaponAttribute tUIWeaponAttribute2 = new TUIWeaponAttribute(0, 0, 0, 0, 0);
				tUIWeaponAttribute2.ammo = cWeaponInfoLevel.nCapacity;
				tUIWeaponAttribute2.blast_radius = 20;
				tUIWeaponAttribute2.damage = (int)cWeaponInfoLevel.fDamage;
				tUIWeaponAttribute2.fire_rate = (int)cWeaponInfoLevel.fShootSpeed;
				for (int k = 0; k < 3; k++)
				{
					if (cWeaponInfoLevel.arrFunc[k] == 4)
					{
						tUIWeaponAttribute2.knockback = cWeaponInfoLevel.arrValueY[k];
						break;
					}
				}
				tUIGameInfo5.equip_info.weapon02 = new TUIPopupInfo(num2, cWeaponInfoLevel.sName, string.Empty, tUIWeaponAttribute2);
			}
			num2 = dataCenter4.GetSelectWeapon(2);
			num3 = dataCenter4.GetWeaponLevel(num2);
			cWeaponInfoLevel = gameData4.GetWeaponInfo(num2, num3);
			if (cWeaponInfoLevel != null)
			{
				TUIWeaponAttribute tUIWeaponAttribute3 = new TUIWeaponAttribute(0, 0, 0, 0, 0);
				tUIWeaponAttribute3.ammo = cWeaponInfoLevel.nCapacity;
				tUIWeaponAttribute3.blast_radius = 20;
				tUIWeaponAttribute3.damage = (int)cWeaponInfoLevel.fDamage;
				tUIWeaponAttribute3.fire_rate = (int)cWeaponInfoLevel.fShootSpeed;
				for (int l = 0; l < 3; l++)
				{
					if (cWeaponInfoLevel.arrFunc[l] == 4)
					{
						tUIWeaponAttribute3.knockback = cWeaponInfoLevel.arrValueY[l];
						break;
					}
				}
				tUIGameInfo5.equip_info.weapon03 = new TUIPopupInfo(num2, cWeaponInfoLevel.sName, string.Empty, tUIWeaponAttribute3);
			}
			int nItemLevel = 0;
			if (dataCenter4.GetEquipStone(dataCenter4.CurEquipStone, ref nItemLevel))
			{
				CItemInfoLevel itemInfo = gameData4.GetItemInfo(dataCenter4.CurEquipStone, nItemLevel);
				if (itemInfo != null && itemInfo.nType == 1)
				{
					tUIGameInfo5.equip_info.weapon04 = new TUIPopupInfo(itemInfo.nID, itemInfo.sName, itemInfo.sDesc);
				}
			}
			tUIGameInfo5.equip_info.weapon_list01 = new List<TUIPopupInfo>();
			tUIGameInfo5.equip_info.weapon_list02 = new List<TUIPopupInfo>();
			Dictionary<int, CWeaponInfo> data = weaponCenter.GetData();
			if (data != null)
			{
				foreach (CWeaponInfo value2 in data.Values)
				{
					int weaponLevel = dataCenter4.GetWeaponLevel(value2.nID);
					if (weaponLevel == -1)
					{
						continue;
					}
					cWeaponInfoLevel = value2.Get(weaponLevel);
					if (cWeaponInfoLevel == null)
					{
						continue;
					}
					TUIWeaponAttribute tUIWeaponAttribute4 = new TUIWeaponAttribute(0, 0, 0, 0, 0);
					tUIWeaponAttribute4.ammo = cWeaponInfoLevel.nCapacity;
					tUIWeaponAttribute4.blast_radius = 20;
					tUIWeaponAttribute4.damage = (int)cWeaponInfoLevel.fDamage;
					tUIWeaponAttribute4.fire_rate = (int)cWeaponInfoLevel.fShootSpeed;
					for (int m = 0; m < 3; m++)
					{
						if (cWeaponInfoLevel.arrFunc[m] == 4)
						{
							tUIWeaponAttribute4.knockback = cWeaponInfoLevel.arrValueY[m];
							break;
						}
					}
					if (cWeaponInfoLevel.nType == 1)
					{
						tUIGameInfo5.equip_info.weapon_list01.Add(new TUIPopupInfo(value2.nID, cWeaponInfoLevel.sName, string.Empty, tUIWeaponAttribute4));
					}
					else
					{
						tUIGameInfo5.equip_info.weapon_list02.Add(new TUIPopupInfo(value2.nID, cWeaponInfoLevel.sName, string.Empty, tUIWeaponAttribute4));
					}
				}
			}
			tUIGameInfo5.equip_info.weapon_list03 = new List<TUIPopupInfo>();
			Dictionary<int, CItemInfo> data2 = itemCenter.GetData();
			if (data2 != null)
			{
				foreach (CItemInfo value3 in data2.Values)
				{
					int nItemLevel2 = 0;
					if (dataCenter4.GetEquipStone(value3.nID, ref nItemLevel2))
					{
						CItemInfoLevel cItemInfoLevel = value3.Get(nItemLevel2);
						if (cItemInfoLevel != null && cItemInfoLevel.nType == 1)
						{
							tUIGameInfo5.equip_info.weapon_list03.Add(new TUIPopupInfo(cItemInfoLevel.nID, cItemInfoLevel.sName, cItemInfoLevel.sDesc));
						}
					}
				}
			}
			global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.BackEvent_SceneEquip(m_event.GetEventName(), tUIGameInfo5));
		}
		else if (m_event.GetEventName() == "TUIEvent_RoleEquip")
		{
			iGameData gameData5 = iGameApp.GetInstance().m_GameData;
			if (gameData5 == null)
			{
				return;
			}
			iDataCenter dataCenter5 = gameData5.GetDataCenter();
			if (dataCenter5 == null)
			{
				return;
			}
			int wParam = m_event.GetWParam();
			if (dataCenter5.GetCharacter(wParam) == null)
			{
				return;
			}
			dataCenter5.CurCharID = wParam;
			TUIGameInfo tUIGameInfo6 = new TUIGameInfo();
			tUIGameInfo6.player_info = new TUIPlayerInfo();
			tUIGameInfo6.equip_info = new TUIEquipInfo();
			CCharSaveInfo character4 = dataCenter5.GetCharacter(dataCenter5.CurCharID);
			CCharacterInfoLevel characterInfo3 = gameData5.GetCharacterInfo(character4.nID, character4.nLevel);
			if (character4 != null && characterInfo3 != null)
			{
				tUIGameInfo6.player_info.avatar_id = character4.nID;
				tUIGameInfo6.player_info.level = character4.nLevel;
				tUIGameInfo6.player_info.level_exp = characterInfo3.nExp;
				tUIGameInfo6.player_info.exp = character4.nExp;
				tUIGameInfo6.player_info.gold = dataCenter5.Gold;
				tUIGameInfo6.player_info.crystal = dataCenter5.Crystal;
				int nSkill = characterInfo3.nSkill;
				CSkillInfoLevel skillInfo = gameData5.GetSkillInfo(nSkill, 1);
				if (skillInfo != null)
				{
					tUIGameInfo6.equip_info.skill01 = new TUIPopupInfo(nSkill, skillInfo.sName, skillInfo.sDesc);
				}
			}
			global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.BackEvent_SceneEquip(m_event.GetEventName(), tUIGameInfo6, true));
		}
		else if (m_event.GetEventName() == "TUIEvent_SkillEquip")
		{
			iGameData gameData6 = iGameApp.GetInstance().m_GameData;
			if (gameData6 != null)
			{
				iDataCenter dataCenter6 = gameData6.GetDataCenter();
				if (dataCenter6 != null)
				{
					int wParam2 = m_event.GetWParam();
					int lparam = m_event.GetLparam();
					dataCenter6.SetSelectPassiveSkill(wParam2 - 2, lparam);
					global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.BackEvent_SceneEquip(m_event.GetEventName(), true));
				}
			}
		}
		else if (m_event.GetEventName() == "TUIEvent_SkillUnEquip")
		{
			iGameData gameData7 = iGameApp.GetInstance().m_GameData;
			if (gameData7 != null)
			{
				iDataCenter dataCenter7 = gameData7.GetDataCenter();
				if (dataCenter7 != null)
				{
					int wParam3 = m_event.GetWParam();
					int lparam2 = m_event.GetLparam();
					dataCenter7.SetSelectPassiveSkill(wParam3 - 1, -1);
					global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.BackEvent_SceneEquip(m_event.GetEventName(), true));
				}
			}
		}
		else if (m_event.GetEventName() == "TUIEvent_SkillExchange")
		{
			iGameData gameData8 = iGameApp.GetInstance().m_GameData;
			if (gameData8 != null)
			{
				iDataCenter dataCenter8 = gameData8.GetDataCenter();
				if (dataCenter8 != null)
				{
					int wParam4 = m_event.GetWParam();
					int lparam3 = m_event.GetLparam();
					int selectPassiveSkill = dataCenter8.GetSelectPassiveSkill(wParam4 - 1);
					int selectPassiveSkill2 = dataCenter8.GetSelectPassiveSkill(lparam3 - 1);
					dataCenter8.SetSelectPassiveSkill(wParam4 - 1, selectPassiveSkill2);
					dataCenter8.SetSelectPassiveSkill(lparam3 - 1, selectPassiveSkill);
					global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.BackEvent_SceneEquip(m_event.GetEventName(), true));
				}
			}
		}
		else if (m_event.GetEventName() == "TUIEvent_WeaponEquip")
		{
			iGameData gameData9 = iGameApp.GetInstance().m_GameData;
			if (gameData9 == null)
			{
				return;
			}
			iDataCenter dataCenter9 = gameData9.GetDataCenter();
			if (dataCenter9 != null)
			{
				int wParam5 = m_event.GetWParam();
				int lparam4 = m_event.GetLparam();
				if (dataCenter9.GetWeaponLevel(lparam4) != -1)
				{
					dataCenter9.SetSelectWeapon(wParam5 - 1, lparam4);
				}
				int nItemLevel3 = 0;
				if (dataCenter9.GetEquipStone(lparam4, ref nItemLevel3))
				{
					dataCenter9.CurEquipStone = lparam4;
				}
				global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.BackEvent_SceneEquip(m_event.GetEventName(), true));
			}
		}
		else if (m_event.GetEventName() == "TUIEvent_WeaponExchange")
		{
			iGameData gameData10 = iGameApp.GetInstance().m_GameData;
			if (gameData10 != null)
			{
				iDataCenter dataCenter10 = gameData10.GetDataCenter();
				if (dataCenter10 != null)
				{
					int wParam6 = m_event.GetWParam();
					int lparam5 = m_event.GetLparam();
					int selectWeapon = dataCenter10.GetSelectWeapon(wParam6 - 1);
					int selectWeapon2 = dataCenter10.GetSelectWeapon(lparam5 - 1);
					dataCenter10.SetSelectWeapon(wParam6 - 1, selectWeapon2);
					dataCenter10.SetSelectWeapon(lparam5 - 1, selectWeapon);
					global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.BackEvent_SceneEquip(m_event.GetEventName(), true));
				}
			}
		}
		else
		{
			if (!(m_event.GetEventName() == "TUIEvent_Back"))
			{
				return;
			}
			iGameData gameData11 = iGameApp.GetInstance().m_GameData;
			if (gameData11 != null)
			{
				iDataCenter dataCenter11 = gameData11.GetDataCenter();
				if (dataCenter11 != null)
				{
					dataCenter11.Save();
					global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.BackEvent_SceneEquip(m_event.GetEventName()));
				}
			}
		}
	}

	private void TUIEvent_BackInfo_SceneStash(object sender, TUIEvent.SendEvent_SceneStash m_event)
	{
		if (m_event.GetEventName() == "TUIEvent_TopBar")
		{
			iGameData gameData = iGameApp.GetInstance().m_GameData;
			if (gameData == null)
			{
				return;
			}
			iDataCenter dataCenter = gameData.GetDataCenter();
			if (dataCenter == null)
			{
				return;
			}
			CCharSaveInfo character = dataCenter.GetCharacter(dataCenter.CurCharID);
			if (character != null)
			{
				CCharacterInfoLevel characterInfo = gameData.GetCharacterInfo(character.nID, character.nLevel);
				if (characterInfo != null)
				{
					TUIGameInfo tUIGameInfo = new TUIGameInfo();
					tUIGameInfo.player_info = new TUIPlayerInfo();
					tUIGameInfo.player_info.avatar_id = character.nID;
					tUIGameInfo.player_info.level = character.nLevel;
					tUIGameInfo.player_info.level_exp = characterInfo.nExp;
					tUIGameInfo.player_info.exp = character.nExp;
					tUIGameInfo.player_info.gold = dataCenter.Gold;
					tUIGameInfo.player_info.crystal = dataCenter.Crystal;
					global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.BackEvent_SceneStash(m_event.GetEventName(), tUIGameInfo));
				}
			}
		}
		else if (m_event.GetEventName() == "TUIEvent_StashInfo")
		{
			iGameData gameData2 = iGameApp.GetInstance().m_GameData;
			if (gameData2 == null)
			{
				return;
			}
			iDataCenter dataCenter2 = gameData2.GetDataCenter();
			if (dataCenter2 == null)
			{
				return;
			}
			iItemCenter itemCenter = gameData2.GetItemCenter();
			if (itemCenter == null)
			{
				return;
			}
			iStashCapacityCenter stashCapacityCenter = gameData2.GetStashCapacityCenter();
			if (stashCapacityCenter == null)
			{
				return;
			}
			TUIGameInfo tUIGameInfo2 = new TUIGameInfo();
			tUIGameInfo2.stash_info = new TUIStashInfo();
			List<TUIStashUpdateInfo> list = new List<TUIStashUpdateInfo>();
			Dictionary<int, CStashCapacity> data = stashCapacityCenter.GetData();
			if (data != null)
			{
				foreach (CStashCapacity value in data.Values)
				{
					list.Add(new TUIStashUpdateInfo(value.nLevel, new TUIPriceInfo(value.nPrice, value.isCrystalPurchase ? UnitType.Crystal : UnitType.Gold), value.nCapacity, value.sLevelUpDesc));
				}
			}
			tUIGameInfo2.stash_info.goods_info_list = new List<TUIGoodsInfo>();
			Dictionary<int, CItemInfo> data2 = itemCenter.GetData();
			if (data2 != null)
			{
				CItemInfoLevel cItemInfoLevel = null;
				int num = 0;
				foreach (CItemInfo value2 in data2.Values)
				{
					cItemInfoLevel = value2.Get(1);
					if (cItemInfoLevel != null && cItemInfoLevel.nType == 3)
					{
						num = dataCenter2.GetMaterialNum(value2.nID);
						if (num == -1)
						{
							num = 0;
						}
						tUIGameInfo2.stash_info.goods_info_list.Add(new TUIGoodsInfo(value2.nID, GoodsQualityType.Quality01, cItemInfoLevel.sName, num, new TUIPriceInfo(cItemInfoLevel.nSellPrice, cItemInfoLevel.isCrystalSell ? UnitType.Crystal : UnitType.Gold)));
					}
				}
			}
			tUIGameInfo2.stash_info = new TUIStashInfo(dataCenter2.StashLevel, list.ToArray(), tUIGameInfo2.stash_info.goods_info_list);
			global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.BackEvent_SceneStash(m_event.GetEventName(), tUIGameInfo2));
		}
		else if (m_event.GetEventName() == "TUIEvent_AddCapacity")
		{
			bool success = false;
			iGameData gameData3 = iGameApp.GetInstance().m_GameData;
			if (gameData3 != null)
			{
				iDataCenter dataCenter3 = gameData3.GetDataCenter();
				if (dataCenter3 != null)
				{
					CStashCapacity stashCapacity = gameData3.GetStashCapacity(dataCenter3.StashLevel);
					CStashCapacity stashCapacity2 = gameData3.GetStashCapacity(dataCenter3.StashLevel + 1);
					if (stashCapacity != null && stashCapacity2 != null)
					{
						if (stashCapacity.isCrystalPurchase)
						{
							if (dataCenter3.Crystal >= stashCapacity.nPrice)
							{
								dataCenter3.AddCrystal(-stashCapacity.nPrice);
								dataCenter3.StashLevel++;
								dataCenter3.Save();
								success = true;
							}
						}
						else if (dataCenter3.Gold >= stashCapacity.nPrice)
						{
							dataCenter3.AddGold(-stashCapacity.nPrice);
							dataCenter3.StashLevel++;
							dataCenter3.Save();
							success = true;
						}
					}
				}
			}
			global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.BackEvent_SceneStash(m_event.GetEventName(), success));
		}
		else if (m_event.GetEventName() == "TUIEvent_SellGoods")
		{
			iGameData gameData4 = iGameApp.GetInstance().m_GameData;
			if (gameData4 == null)
			{
				return;
			}
			iDataCenter dataCenter4 = gameData4.GetDataCenter();
			if (dataCenter4 == null)
			{
				return;
			}
			bool flag = false;
			int wParam = m_event.GetWParam();
			int lparam = m_event.GetLparam();
			int rparam = m_event.GetRparam();
			CItemInfoLevel itemInfo = gameData4.GetItemInfo(wParam, 1);
			if (itemInfo == null || itemInfo.nType != 3)
			{
				return;
			}
			int materialNum = dataCenter4.GetMaterialNum(wParam);
			if (materialNum != -1)
			{
				materialNum = ((rparam <= materialNum) ? (materialNum - rparam) : 0);
				dataCenter4.SetMaterialNum(wParam, materialNum);
				if (itemInfo.isCrystalSell)
				{
					dataCenter4.AddCrystal(itemInfo.nSellPrice * rparam);
				}
				else
				{
					dataCenter4.AddGold(itemInfo.nSellPrice * rparam);
				}
				flag = true;
				dataCenter4.Save();
				global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.BackEvent_SceneStash(m_event.GetEventName(), flag));
			}
		}
		else if (m_event.GetEventName() == "TUIEvent_Back")
		{
			global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.BackEvent_SceneStash(m_event.GetEventName()));
		}
		else if (m_event.GetEventName() == "TUIEvent_SearchGoodsDrop")
		{
			int wParam2 = m_event.GetWParam();
			iGameState gameState = iGameApp.GetInstance().m_GameState;
			if (gameState != null)
			{
				gameState.m_nMaterialIDFromEquip = wParam2;
				global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.BackEvent_SceneStash(m_event.GetEventName()));
			}
		}
	}

	private void TUIEvent_BackInfo_SceneSkill(object sender, TUIEvent.SendEvent_SceneSkill m_event)
	{
		if (m_event.GetEventName() == "TUIEvent_TopBar")
		{
			iGameData gameData = iGameApp.GetInstance().m_GameData;
			if (gameData == null)
			{
				return;
			}
			iDataCenter dataCenter = gameData.GetDataCenter();
			if (dataCenter == null)
			{
				return;
			}
			CCharSaveInfo character = dataCenter.GetCharacter(dataCenter.CurCharID);
			if (character != null)
			{
				CCharacterInfoLevel characterInfo = gameData.GetCharacterInfo(character.nID, character.nLevel);
				if (characterInfo != null)
				{
					TUIGameInfo tUIGameInfo = new TUIGameInfo();
					tUIGameInfo.player_info = new TUIPlayerInfo();
					tUIGameInfo.player_info.avatar_id = character.nID;
					tUIGameInfo.player_info.level = character.nLevel;
					tUIGameInfo.player_info.level_exp = characterInfo.nExp;
					tUIGameInfo.player_info.exp = character.nExp;
					tUIGameInfo.player_info.gold = dataCenter.Gold;
					tUIGameInfo.player_info.crystal = dataCenter.Crystal;
					global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.BackEvent_SceneSkill(m_event.GetEventName(), tUIGameInfo));
				}
			}
		}
		else if (m_event.GetEventName() == "TUIEvent_SkillInfo")
		{
			iGameData gameData2 = iGameApp.GetInstance().m_GameData;
			if (gameData2 == null)
			{
				return;
			}
			iDataCenter dataCenter2 = gameData2.GetDataCenter();
			if (dataCenter2 == null)
			{
				return;
			}
			CCharacterInfo cCharacterInfo = null;
			CSkillInfo cSkillInfo = null;
			CSkillInfoLevel cSkillInfoLevel = null;
			int[] array = new int[5] { 1, 2, 3, 4, 5 };
			TUISkillListInfo[] array2 = new TUISkillListInfo[array.Length];
			for (int i = 0; i < array2.Length; i++)
			{
				List<TUISkillInfo> list = new List<TUISkillInfo>();
				cCharacterInfo = gameData2.GetCharacterInfo(array[i]);
				if (cCharacterInfo != null)
				{
					for (int j = 0; j < cCharacterInfo.ltCharacterPassiveSkill.Count; j++)
					{
						cSkillInfo = gameData2.GetSkillInfo(cCharacterInfo.ltCharacterPassiveSkill[j]);
						if (cSkillInfo == null)
						{
							continue;
						}
						Dictionary<int, TUIPriceInfo> dictionary = new Dictionary<int, TUIPriceInfo>();
						for (int k = 1; k <= 5; k++)
						{
							cSkillInfoLevel = cSkillInfo.Get(k);
							if (cSkillInfoLevel != null)
							{
								dictionary.Add(k, new TUIPriceInfo(cSkillInfoLevel.nPurchasePrice, cSkillInfoLevel.isCrystalPurchase ? UnitType.Crystal : UnitType.Gold));
							}
							else
							{
								dictionary.Add(k, new TUIPriceInfo(999999, UnitType.Crystal));
							}
							Debug.Log("level:" + k + ":" + dictionary[k].price);
						}
						Dictionary<int, string> dictionary2 = new Dictionary<int, string>();
						Dictionary<int, string> dictionary3 = new Dictionary<int, string>();
						for (int l = 1; l <= 5; l++)
						{
							cSkillInfoLevel = cSkillInfo.Get(l);
							if (cSkillInfoLevel != null)
							{
								dictionary2.Add(l, cSkillInfoLevel.sLevelUpDesc);
								dictionary3.Add(l, cSkillInfoLevel.sDesc);
							}
						}
						cSkillInfoLevel = cSkillInfo.Get(1);
						if (cSkillInfoLevel != null)
						{
							int nSkillLevel = 0;
							dataCenter2.GetPassiveSkill(cSkillInfo.nID, ref nSkillLevel);
							list.Add(new TUISkillInfo(cSkillInfo.nID, cSkillInfoLevel.sName, (nSkillLevel != -1) ? nSkillLevel : 0, nSkillLevel != 0, new TUIPriceInfo(cSkillInfo.nUnlockPrice, cSkillInfo.isCrystalUnlock ? UnitType.Crystal : UnitType.Gold), dictionary, dictionary2, dictionary3));
						}
					}
				}
				array2[i] = new TUISkillListInfo(array[i], list.ToArray());
			}
			TUIGameInfo tUIGameInfo2 = new TUIGameInfo();
			tUIGameInfo2.all_skill_info = new TUIAllSkillInfo(array2);
			global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.BackEvent_SceneSkill(m_event.GetEventName(), tUIGameInfo2));
		}
		else if (m_event.GetEventName() == "TUIEvent_SkillUnlcok")
		{
			int lparam = m_event.GetLparam();
			bool success = false;
			iGameData gameData3 = iGameApp.GetInstance().m_GameData;
			if (gameData3 != null)
			{
				CSkillInfo skillInfo = gameData3.GetSkillInfo(lparam);
				iDataCenter dataCenter3 = gameData3.GetDataCenter();
				if (skillInfo != null && dataCenter3 != null)
				{
					int nSkillLevel2 = 0;
					if (!dataCenter3.GetPassiveSkill(skillInfo.nID, ref nSkillLevel2))
					{
						if (skillInfo.isCrystalUnlock)
						{
							if (dataCenter3.Crystal >= skillInfo.nUnlockPrice)
							{
								dataCenter3.AddCrystal(-skillInfo.nUnlockPrice);
								dataCenter3.UnlockPassiveSkill(skillInfo.nID);
								dataCenter3.Save();
								success = true;
							}
						}
						else if (dataCenter3.Gold >= skillInfo.nUnlockPrice)
						{
							dataCenter3.AddGold(-skillInfo.nUnlockPrice);
							dataCenter3.UnlockPassiveSkill(skillInfo.nID);
							dataCenter3.Save();
							success = true;
						}
					}
				}
			}
			global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.BackEvent_SceneSkill(m_event.GetEventName(), success));
		}
		else if (m_event.GetEventName() == "TUIEvent_SkillBuy")
		{
			int lparam2 = m_event.GetLparam();
			bool success2 = false;
			iGameData gameData4 = iGameApp.GetInstance().m_GameData;
			if (gameData4 != null)
			{
				CSkillInfo skillInfo2 = gameData4.GetSkillInfo(lparam2);
				iDataCenter dataCenter4 = gameData4.GetDataCenter();
				if (skillInfo2 != null && dataCenter4 != null)
				{
					int nSkillLevel3 = 0;
					if (dataCenter4.GetPassiveSkill(skillInfo2.nID, ref nSkillLevel3) && nSkillLevel3 == -1)
					{
						CSkillInfoLevel cSkillInfoLevel2 = skillInfo2.Get(1);
						if (cSkillInfoLevel2 != null)
						{
							if (cSkillInfoLevel2.isCrystalPurchase)
							{
								if (dataCenter4.Crystal >= cSkillInfoLevel2.nPurchasePrice)
								{
									dataCenter4.AddCrystal(-cSkillInfoLevel2.nPurchasePrice);
									dataCenter4.SetPassiveSkill(skillInfo2.nID, 1);
									dataCenter4.Save();
									success2 = true;
								}
							}
							else if (dataCenter4.Gold >= cSkillInfoLevel2.nPurchasePrice)
							{
								dataCenter4.AddGold(-cSkillInfoLevel2.nPurchasePrice);
								dataCenter4.SetPassiveSkill(skillInfo2.nID, 1);
								dataCenter4.Save();
								success2 = true;
							}
						}
					}
				}
			}
			global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.BackEvent_SceneSkill(m_event.GetEventName(), success2));
		}
		else if (m_event.GetEventName() == "TUIEvent_SkillUpdate")
		{
			int lparam3 = m_event.GetLparam();
			bool success3 = false;
			iGameData gameData5 = iGameApp.GetInstance().m_GameData;
			if (gameData5 != null)
			{
				CSkillInfo skillInfo3 = gameData5.GetSkillInfo(lparam3);
				iDataCenter dataCenter5 = gameData5.GetDataCenter();
				if (skillInfo3 != null && dataCenter5 != null)
				{
					int nSkillLevel4 = 0;
					if (dataCenter5.GetPassiveSkill(skillInfo3.nID, ref nSkillLevel4) && nSkillLevel4 > 0)
					{
						CSkillInfoLevel skillInfo4 = gameData5.GetSkillInfo(skillInfo3.nID, nSkillLevel4);
						CSkillInfoLevel skillInfo5 = gameData5.GetSkillInfo(skillInfo3.nID, nSkillLevel4 + 1);
						if (skillInfo4 != null && skillInfo5 != null)
						{
							if (skillInfo4.isCrystalPurchase)
							{
								if (dataCenter5.Crystal >= skillInfo4.nPurchasePrice)
								{
									dataCenter5.AddCrystal(-skillInfo4.nPurchasePrice);
									dataCenter5.SetPassiveSkill(skillInfo3.nID, skillInfo5.nLevel);
									dataCenter5.Save();
									success3 = true;
								}
							}
							else if (dataCenter5.Gold >= skillInfo4.nPurchasePrice)
							{
								dataCenter5.AddGold(-skillInfo4.nPurchasePrice);
								dataCenter5.SetPassiveSkill(skillInfo3.nID, skillInfo5.nLevel);
								dataCenter5.Save();
								success3 = true;
							}
						}
					}
				}
			}
			global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.BackEvent_SceneSkill(m_event.GetEventName(), success3));
		}
		else if (m_event.GetEventName() == "TUIEvent_Back")
		{
			global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.BackEvent_SceneSkill(m_event.GetEventName()));
		}
	}

	private void TUIEvent_BackInfo_SceneForge(object sender, TUIEvent.SendEvent_SceneForge m_event)
	{
		if (m_event.GetEventName() == "TUIEvent_TopBar")
		{
			iGameData gameData = iGameApp.GetInstance().m_GameData;
			if (gameData == null)
			{
				return;
			}
			iDataCenter dataCenter = gameData.GetDataCenter();
			if (dataCenter == null)
			{
				return;
			}
			CCharSaveInfo character = dataCenter.GetCharacter(dataCenter.CurCharID);
			if (character != null)
			{
				CCharacterInfoLevel characterInfo = gameData.GetCharacterInfo(character.nID, character.nLevel);
				if (characterInfo != null)
				{
					TUIGameInfo tUIGameInfo = new TUIGameInfo();
					tUIGameInfo.player_info = new TUIPlayerInfo();
					tUIGameInfo.player_info.avatar_id = character.nID;
					tUIGameInfo.player_info.level = character.nLevel;
					tUIGameInfo.player_info.level_exp = characterInfo.nExp;
					tUIGameInfo.player_info.exp = character.nExp;
					tUIGameInfo.player_info.gold = dataCenter.Gold;
					tUIGameInfo.player_info.crystal = dataCenter.Crystal;
					global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.BackEvent_SceneForge(m_event.GetEventName(), tUIGameInfo));
				}
			}
		}
		else if (m_event.GetEventName() == "TUIEvent_WeaponInfo")
		{
			iGameData gameData2 = iGameApp.GetInstance().m_GameData;
			if (gameData2 == null)
			{
				return;
			}
			iDataCenter dataCenter2 = gameData2.GetDataCenter();
			if (dataCenter2 == null)
			{
				return;
			}
			iItemCenter itemCenter = gameData2.GetItemCenter();
			if (itemCenter == null)
			{
				return;
			}
			Dictionary<int, CItemInfo> data = itemCenter.GetData();
			if (data == null)
			{
				return;
			}
			iWeaponCenter weaponCenter = gameData2.GetWeaponCenter();
			if (weaponCenter == null)
			{
				return;
			}
			Dictionary<int, CWeaponInfo> data2 = weaponCenter.GetData();
			if (data2 == null)
			{
				return;
			}
			Dictionary<int, TUIGoodsInfo> dictionary = new Dictionary<int, TUIGoodsInfo>();
			if (data != null)
			{
				CItemInfoLevel cItemInfoLevel = null;
				foreach (CItemInfo value in data.Values)
				{
					cItemInfoLevel = gameData2.GetItemInfo(value.nID, 1);
					if (cItemInfoLevel != null)
					{
						int num = dataCenter2.GetMaterialNum(value.nID);
						if (num == -1)
						{
							num = 0;
						}
						dictionary.Add(value.nID, new TUIGoodsInfo(value.nID, GoodsQualityType.Quality01, cItemInfoLevel.sName, num, new TUIPriceInfo(cItemInfoLevel.nPurchasePrice, cItemInfoLevel.isCrystalPurchase ? UnitType.Crystal : UnitType.Gold)));
					}
				}
			}
			TUIGameInfo tUIGameInfo2 = new TUIGameInfo();
			tUIGameInfo2.weapon_info = new TUIWeaponInfo();
			CWeaponInfoLevel[] array = new CWeaponInfoLevel[5];
			foreach (CWeaponInfo value2 in data2.Values)
			{
				bool flag = false;
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = value2.Get(i + 1);
					if (array[i] == null)
					{
						Debug.LogWarning("id " + value2.nID + " lvl " + (i + 1) + " does not exist!");
						flag = true;
					}
				}
				if (flag)
				{
					continue;
				}
				Dictionary<int, TUIPriceInfo> dictionary2 = new Dictionary<int, TUIPriceInfo>();
				for (int j = 0; j < array.Length; j++)
				{
					dictionary2.Add(j + 1, new TUIPriceInfo(array[j].nPurchasePrice, array[j].isCrystalPurchase ? UnitType.Crystal : UnitType.Gold));
				}
				Dictionary<int, int> dictionary3 = new Dictionary<int, int>();
				for (int k = 0; k < array.Length; k++)
				{
					dictionary3.Add(k + 1, (int)array[k].fDamage);
				}
				Dictionary<int, float> dictionary4 = new Dictionary<int, float>();
				for (int l = 0; l < array.Length; l++)
				{
					dictionary4.Add(l + 1, array[l].fShootSpeed);
				}
				Dictionary<int, int> dictionary5 = new Dictionary<int, int>();
				for (int m = 0; m < array.Length; m++)
				{
					dictionary5.Add(m + 1, 50);
				}
				Dictionary<int, int> dictionary6 = new Dictionary<int, int>();
				for (int n = 0; n < array.Length; n++)
				{
					for (int num2 = 0; num2 < 3; num2++)
					{
						if (array[n].arrFunc[num2] == 4)
						{
							dictionary6.Add(n + 1, array[n].arrValueY[num2]);
							break;
						}
					}
				}
				Dictionary<int, int> dictionary7 = new Dictionary<int, int>();
				for (int num3 = 0; num3 < array.Length; num3++)
				{
					dictionary7.Add(num3 + 1, array[num3].nCapacity);
				}
				Dictionary<int, string> dictionary8 = new Dictionary<int, string>();
				for (int num4 = 0; num4 < array.Length; num4++)
				{
					dictionary8.Add(num4 + 1, array[num4].sLevelUpDesc);
				}
				TUIWeaponUpdateInfo weapon_update_info = new TUIWeaponUpdateInfo(dictionary2, dictionary3, dictionary4, dictionary5, dictionary6, dictionary7, dictionary8);
				List<TUIGoodsNeedInfo>[] array2 = new List<TUIGoodsNeedInfo>[5];
				for (int num5 = 0; num5 < array2.Length; num5++)
				{
					array2[num5] = new List<TUIGoodsNeedInfo>();
					for (int num6 = 0; num6 < array[num5].ltMaterials.Count && num6 < array[num5].ltMaterialsCount.Count; num6++)
					{
						array2[num5].Add(new TUIGoodsNeedInfo(array[num5].ltMaterials[num6], GoodsQualityType.Quality01, array[num5].ltMaterialsCount[num6]));
					}
				}
				TUILevelGoodsNeedInfo level_goods_need_info = new TUILevelGoodsNeedInfo(array2[0], array2[1], array2[2], array2[3], array2[4]);
				WeaponType type = WeaponType.CloseWeapon;
				switch (array[0].nType)
				{
				case 1:
					type = WeaponType.CloseWeapon;
					break;
				case 2:
					type = WeaponType.MachineGun;
					break;
				case 0:
					type = WeaponType.Crossbow;
					break;
				case 4:
					type = WeaponType.LiquidFireGun;
					break;
				case 5:
					type = WeaponType.RPG;
					break;
				case 3:
					type = WeaponType.ViolenceGun;
					break;
				}
				int num7 = dataCenter2.GetWeaponLevel(value2.nID);
				if (num7 == -1)
				{
					num7 = 0;
				}
				tUIGameInfo2.weapon_info.AddItem(new TUIWeaponAttributeInfo(type, value2.nID, array[0].sName, num7, weapon_update_info, level_goods_need_info, dictionary));
			}
			if (data != null)
			{
				CItemInfoLevel[] array3 = new CItemInfoLevel[5];
				foreach (CItemInfo value3 in data.Values)
				{
					bool flag2 = false;
					for (int num8 = 0; num8 < array3.Length; num8++)
					{
						array3[num8] = value3.Get(num8 + 1);
						if (array3[num8] == null)
						{
							flag2 = true;
						}
						else if (array3[num8].nType != 1)
						{
							flag2 = true;
						}
					}
					if (flag2)
					{
						continue;
					}
					Dictionary<int, int> dictionary9 = new Dictionary<int, int>();
					for (int num9 = 0; num9 < array3.Length; num9++)
					{
						for (int num10 = 0; num10 < 3; num10++)
						{
							if (array3[num9].arrFunc[num10] == 1)
							{
								dictionary9.Add(num9 + 1, MyUtils.Low32(array3[num9].arrValueY[num10]));
							}
						}
					}
					Dictionary<int, TUIPriceInfo> dictionary10 = new Dictionary<int, TUIPriceInfo>();
					for (int num11 = 0; num11 < array3.Length; num11++)
					{
						dictionary10.Add(num11 + 1, new TUIPriceInfo(array3[num11].nPurchasePrice, array3[num11].isCrystalPurchase ? UnitType.Crystal : UnitType.Gold));
					}
					Dictionary<int, string> dictionary11 = new Dictionary<int, string>();
					Dictionary<int, string> dictionary12 = new Dictionary<int, string>();
					for (int num12 = 0; num12 < array3.Length; num12++)
					{
						dictionary11.Add(num12 + 1, array3[num12].sLevelUpDesc);
						dictionary12.Add(num12 + 1, array3[num12].sDesc);
					}
					List<TUIGoodsNeedInfo>[] array4 = new List<TUIGoodsNeedInfo>[5];
					for (int num13 = 0; num13 < array4.Length; num13++)
					{
						array4[num13] = new List<TUIGoodsNeedInfo>();
						for (int num14 = 0; num14 < array3[num13].ltMaterials.Count && num14 < array3[num13].ltMaterialsCount.Count; num14++)
						{
							array4[num13].Add(new TUIGoodsNeedInfo(array3[num13].ltMaterials[num14], GoodsQualityType.Quality01, array3[num13].ltMaterialsCount[num14]));
						}
					}
					TUILevelGoodsNeedInfo level_goods_need_info2 = new TUILevelGoodsNeedInfo(array4[0], array4[1], array4[2], array4[3], array4[4]);
					TUIWeaponUpdateInfo weapon_update_info2 = new TUIWeaponUpdateInfo(dictionary10, dictionary11, dictionary9, dictionary12);
					int nItemLevel = 0;
					dataCenter2.GetEquipStone(value3.nID, ref nItemLevel);
					tUIGameInfo2.weapon_info.AddItem(new TUIWeaponAttributeInfo(WeaponType.Stoneskin, value3.nID, array3[0].sName, nItemLevel, weapon_update_info2, level_goods_need_info2, dictionary));
				}
			}
			global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.BackEvent_SceneForge(m_event.GetEventName(), tUIGameInfo2));
		}
		else if (m_event.GetEventName() == "TUIEvent_WeaponGoodsBuy")
		{
			bool success = false;
			iGameData gameData3 = iGameApp.GetInstance().m_GameData;
			if (gameData3 != null)
			{
				iDataCenter dataCenter3 = gameData3.GetDataCenter();
				if (dataCenter3 != null)
				{
					int wParam = m_event.GetWParam();
					int rparam = m_event.GetRparam();
					int lparam = m_event.GetLparam();
					Debug.Log(rparam);
					CItemInfoLevel itemInfo = gameData3.GetItemInfo(wParam, 1);
					if (itemInfo != null && itemInfo.nType == 3)
					{
						if (itemInfo.isCrystalPurchase)
						{
							if (dataCenter3.Crystal >= itemInfo.nPurchasePrice)
							{
								dataCenter3.AddCrystal(-itemInfo.nPurchasePrice);
								dataCenter3.AddMaterialNum(wParam, rparam);
								dataCenter3.Save();
								success = true;
							}
						}
						else if (dataCenter3.Gold >= itemInfo.nPurchasePrice)
						{
							dataCenter3.AddGold(-itemInfo.nPurchasePrice);
							dataCenter3.AddMaterialNum(wParam, rparam);
							dataCenter3.Save();
							success = true;
						}
					}
				}
			}
			global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.BackEvent_SceneForge(m_event.GetEventName(), success));
		}
		else if (m_event.GetEventName() == "TUIEvent_WeaponUpdate")
		{
			bool success2 = false;
			int wparam = 0;
			iGameData gameData4 = iGameApp.GetInstance().m_GameData;
			if (gameData4 != null)
			{
				iDataCenter dataCenter4 = gameData4.GetDataCenter();
				if (dataCenter4 != null)
				{
					int wParam2 = m_event.GetWParam();
					CLevelUpWeapon cLevelUpWeapon = new CLevelUpWeapon();
					if (cLevelUpWeapon != null)
					{
						cLevelUpWeapon.Initialize(wParam2);
						if (cLevelUpWeapon.LevelUp())
						{
							wparam = ((!cLevelUpWeapon.isCrystalTrade) ? 1 : 2);
							success2 = true;
						}
					}
					CLevelUpEquip cLevelUpEquip = new CLevelUpEquip();
					if (cLevelUpEquip != null)
					{
						cLevelUpEquip.Initialize(wParam2);
						if (cLevelUpEquip.LevelUp())
						{
							wparam = ((!cLevelUpEquip.isCrystalTrade) ? 1 : 2);
							success2 = true;
						}
					}
				}
			}
			global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.BackEvent_SceneForge(m_event.GetEventName(), success2, wparam));
		}
		else if (m_event.GetEventName() == "TUIEvent_Back")
		{
			global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.BackEvent_SceneForge(m_event.GetEventName()));
		}
		else if (m_event.GetEventName() == "TUIEvent_SearchGoodsDrop")
		{
			int wParam3 = m_event.GetWParam();
			int lparam2 = m_event.GetLparam();
			iGameState gameState = iGameApp.GetInstance().m_GameState;
			if (gameState != null)
			{
				gameState.m_nMaterialIDFromEquip = wParam3;
				global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.BackEvent_SceneForge(m_event.GetEventName()));
			}
		}
	}

	private void TUIEvent_BackInfo_SceneTavern(object sender, TUIEvent.SendEvent_SceneTavern m_event)
	{
		if (m_event.GetEventName() == "TUIEvent_TopBar")
		{
			iGameData gameData = iGameApp.GetInstance().m_GameData;
			if (gameData == null)
			{
				return;
			}
			iDataCenter dataCenter = gameData.GetDataCenter();
			if (dataCenter == null)
			{
				return;
			}
			CCharSaveInfo character = dataCenter.GetCharacter(dataCenter.CurCharID);
			if (character != null)
			{
				CCharacterInfoLevel characterInfo = gameData.GetCharacterInfo(character.nID, character.nLevel);
				if (characterInfo != null)
				{
					TUIGameInfo tUIGameInfo = new TUIGameInfo();
					tUIGameInfo.player_info = new TUIPlayerInfo();
					tUIGameInfo.player_info.avatar_id = character.nID;
					tUIGameInfo.player_info.level = character.nLevel;
					tUIGameInfo.player_info.level_exp = characterInfo.nExp;
					tUIGameInfo.player_info.exp = character.nExp;
					tUIGameInfo.player_info.gold = dataCenter.Gold;
					tUIGameInfo.player_info.crystal = dataCenter.Crystal;
					global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.BackEvent_SceneTavern(m_event.GetEventName(), tUIGameInfo));
				}
			}
		}
		else if (m_event.GetEventName() == "TUIEvent_AllRoleInfo")
		{
			iGameData gameData2 = iGameApp.GetInstance().m_GameData;
			if (gameData2 == null)
			{
				return;
			}
			iCharacterCenter characterCenter = gameData2.GetCharacterCenter();
			if (characterCenter == null)
			{
				return;
			}
			iDataCenter dataCenter2 = gameData2.GetDataCenter();
			if (dataCenter2 == null)
			{
				return;
			}
			TUIGameInfo tUIGameInfo2 = new TUIGameInfo();
			tUIGameInfo2.all_role_info = new TUIAllRoleInfo();
			tUIGameInfo2.all_role_info.role_list = new TUIRoleInfo[5];
			Dictionary<int, CCharacterInfo> data = characterCenter.GetData();
			if (data != null)
			{
				int num = 0;
				foreach (CCharacterInfo value in data.Values)
				{
					if (num >= tUIGameInfo2.all_role_info.role_list.Length)
					{
						break;
					}
					CCharacterInfoLevel cCharacterInfoLevel = characterCenter.Get(value.nID, 1);
					if (cCharacterInfoLevel != null)
					{
						CCharSaveInfo character2 = dataCenter2.GetCharacter(value.nID);
						tUIGameInfo2.all_role_info.role_list[num++] = new TUIRoleInfo(value.nID, cCharacterInfoLevel.sName, cCharacterInfoLevel.sDesc, character2 != null, new TUIPriceInfo(value.nUnLockPrice, value.isCrystalUnLock ? UnitType.Crystal : UnitType.Gold), character2 != null && character2.nLevel >= 1, new TUIPriceInfo(value.nPurchasePrice, value.isCrystalPurchase ? UnitType.Crystal : UnitType.Gold));
					}
				}
			}
			global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.BackEvent_SceneTavern(m_event.GetEventName(), tUIGameInfo2));
		}
		else if (m_event.GetEventName() == "TUIEvent_RoleUnlock")
		{
			bool success = false;
			iGameData gameData3 = iGameApp.GetInstance().m_GameData;
			if (gameData3 != null)
			{
				iDataCenter dataCenter3 = gameData3.GetDataCenter();
				if (dataCenter3 != null)
				{
					int wParam = m_event.GetWParam();
					CCharSaveInfo character3 = dataCenter3.GetCharacter(wParam);
					if (character3 == null)
					{
						CCharacterInfo characterInfo2 = gameData3.GetCharacterInfo(wParam);
						if (characterInfo2 != null)
						{
							if (characterInfo2.isCrystalUnLock)
							{
								if (dataCenter3.Crystal > characterInfo2.nUnLockPrice)
								{
									dataCenter3.AddCrystal(-characterInfo2.nUnLockPrice);
									dataCenter3.UnlockCharacter(wParam);
									dataCenter3.Save();
									success = true;
								}
							}
							else if (dataCenter3.Gold > characterInfo2.nUnLockPrice)
							{
								dataCenter3.AddGold(-characterInfo2.nUnLockPrice);
								dataCenter3.UnlockCharacter(wParam);
								dataCenter3.Save();
								success = true;
							}
						}
					}
				}
			}
			global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.BackEvent_SceneTavern(m_event.GetEventName(), success));
		}
		else if (m_event.GetEventName() == "TUIEvent_RoleBuy")
		{
			bool success2 = false;
			iGameData gameData4 = iGameApp.GetInstance().m_GameData;
			if (gameData4 != null)
			{
				iDataCenter dataCenter4 = gameData4.GetDataCenter();
				if (dataCenter4 != null)
				{
					int wParam2 = m_event.GetWParam();
					CCharSaveInfo character4 = dataCenter4.GetCharacter(wParam2);
					if (character4 != null && character4.nLevel < 0)
					{
						CCharacterInfo characterInfo3 = gameData4.GetCharacterInfo(wParam2);
						if (characterInfo3 != null)
						{
							if (characterInfo3.isCrystalPurchase)
							{
								if (dataCenter4.Crystal > characterInfo3.nPurchasePrice)
								{
									dataCenter4.AddCrystal(-characterInfo3.nPurchasePrice);
									dataCenter4.SetCharacter(wParam2, 1, 0);
									dataCenter4.Save();
									success2 = true;
								}
							}
							else if (dataCenter4.Gold > characterInfo3.nPurchasePrice)
							{
								dataCenter4.AddGold(-characterInfo3.nPurchasePrice);
								dataCenter4.SetCharacter(wParam2, 1, 0);
								dataCenter4.Save();
								success2 = true;
							}
						}
					}
				}
			}
			global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.BackEvent_SceneTavern(m_event.GetEventName(), success2));
		}
		else if (m_event.GetEventName() == "TUIEvent_RoleChange")
		{
			bool success3 = false;
			TUIGameInfo tUIGameInfo3 = new TUIGameInfo();
			tUIGameInfo3.player_info = new TUIPlayerInfo();
			iGameData gameData5 = iGameApp.GetInstance().m_GameData;
			if (gameData5 != null)
			{
				iDataCenter dataCenter5 = gameData5.GetDataCenter();
				if (dataCenter5 != null)
				{
					int wParam3 = m_event.GetWParam();
					CCharSaveInfo character5 = dataCenter5.GetCharacter(wParam3);
					if (character5 != null && character5.nLevel >= 1)
					{
						CCharacterInfoLevel characterInfo4 = gameData5.GetCharacterInfo(character5.nID, character5.nLevel);
						if (characterInfo4 != null)
						{
							dataCenter5.CurCharID = wParam3;
							dataCenter5.Save();
							success3 = true;
							tUIGameInfo3.player_info.avatar_id = character5.nID;
							tUIGameInfo3.player_info.level = character5.nLevel;
							tUIGameInfo3.player_info.level_exp = characterInfo4.nExp;
							tUIGameInfo3.player_info.exp = character5.nExp;
							tUIGameInfo3.player_info.gold = dataCenter5.Gold;
							tUIGameInfo3.player_info.crystal = dataCenter5.Crystal;
						}
					}
				}
			}
			global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.BackEvent_SceneTavern(m_event.GetEventName(), tUIGameInfo3, success3));
		}
		else if (m_event.GetEventName() == "TUIEvent_Back")
		{
			global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.BackEvent_SceneTavern(m_event.GetEventName()));
		}
	}

	private void TUIEvent_BackInfo_SceneMap(object sender, TUIEvent.SendEvent_SceneMap m_event)
	{
		if (m_event.GetEventName() == "TUIEvent_TopBar")
		{
			iGameData gameData = iGameApp.GetInstance().m_GameData;
			if (gameData == null)
			{
				return;
			}
			iDataCenter dataCenter = gameData.GetDataCenter();
			if (dataCenter == null)
			{
				return;
			}
			CCharSaveInfo character = dataCenter.GetCharacter(dataCenter.CurCharID);
			if (character != null)
			{
				CCharacterInfoLevel characterInfo = gameData.GetCharacterInfo(character.nID, character.nLevel);
				if (characterInfo != null)
				{
					TUIGameInfo tUIGameInfo = new TUIGameInfo();
					tUIGameInfo.player_info = new TUIPlayerInfo();
					tUIGameInfo.player_info.avatar_id = character.nID;
					tUIGameInfo.player_info.level = character.nLevel;
					tUIGameInfo.player_info.level_exp = characterInfo.nExp;
					tUIGameInfo.player_info.exp = character.nExp;
					tUIGameInfo.player_info.gold = dataCenter.Gold;
					tUIGameInfo.player_info.crystal = dataCenter.Crystal;
					global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.BackEvent_SceneMap(m_event.GetEventName(), tUIGameInfo));
				}
			}
		}
		else if (m_event.GetEventName() == "TUIEvent_MapEnterInfo")
		{
			iGameState gameState = iGameApp.GetInstance().m_GameState;
			if (gameState == null)
			{
				return;
			}
			iGameData gameData2 = iGameApp.GetInstance().m_GameData;
			if (gameData2 == null)
			{
				return;
			}
			iDataCenter dataCenter2 = gameData2.GetDataCenter();
			if (dataCenter2 == null)
			{
				return;
			}
			iGameLevelCenter gameLevelCenter = gameData2.GetGameLevelCenter();
			if (gameLevelCenter == null)
			{
				return;
			}
			List<int> list = new List<int>();
			List<int> list2 = new List<int>();
			List<int> levelList = dataCenter2.GetLevelList();
			bool flag = false;
			for (int i = 0; i < levelList.Count; i++)
			{
				if (flag)
				{
					list2.Add(levelList[i]);
				}
				else
				{
					list.Add(levelList[i]);
				}
				if (dataCenter2.LatestLevel == levelList[i])
				{
					flag = true;
				}
			}
			Dictionary<int, GameLevelInfo> data = gameLevelCenter.GetData();
			TUIGameInfo tUIGameInfo2 = new TUIGameInfo();
			if (gameState.m_nMaterialIDFromEquip == -1)
			{
				int nNewLevel = -1;
				if (dataCenter2.GetNewLevel(ref nNewLevel))
				{
					tUIGameInfo2.map_info = new TUIMapInfo(MapEnterType.OpenNewLevel, dataCenter2.LatestLevel, nNewLevel, list.ToArray(), list2.ToArray());
					dataCenter2.UnlockNewLevelConfirm(nNewLevel);
					dataCenter2.Save();
				}
				else
				{
					tUIGameInfo2.map_info = new TUIMapInfo(MapEnterType.Normal, list[list.Count - 1], list.ToArray(), list2.ToArray());
				}
			}
			else
			{
				List<int> list3 = new List<int>();
				int nMaterialIDFromEquip = gameState.m_nMaterialIDFromEquip;
				gameState.m_nMaterialIDFromEquip = -1;
				if (data != null)
				{
					foreach (GameLevelInfo value in data.Values)
					{
						if (value.ltRewardMaterial == null)
						{
							continue;
						}
						foreach (CRewardMaterial item in value.ltRewardMaterial)
						{
							if (nMaterialIDFromEquip == item.nID)
							{
								list3.Add(value.nID);
								break;
							}
						}
					}
				}
				tUIGameInfo2.map_info = new TUIMapInfo(MapEnterType.SearchGoods, list[list.Count - 1], list.ToArray(), list2.ToArray(), list3.ToArray());
			}
			global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.BackEvent_SceneMap(m_event.GetEventName(), tUIGameInfo2));
		}
		else if (m_event.GetEventName() == "TUIEvent_LevelInfo")
		{
			iGameData gameData3 = iGameApp.GetInstance().m_GameData;
			if (gameData3 == null)
			{
				return;
			}
			iDataCenter dataCenter3 = gameData3.GetDataCenter();
			if (dataCenter3 == null)
			{
				return;
			}
			int wParam = m_event.GetWParam();
			GameLevelInfo gameLevelInfo = gameData3.GetGameLevelInfo(wParam);
			if (gameLevelInfo == null)
			{
				return;
			}
			TUIRecommendRoleInfo recommend_role_info = new TUIRecommendRoleInfo(1, true, true, false);
			TUIRecommendWeaponInfo tUIRecommendWeaponInfo = new TUIRecommendWeaponInfo(1, 3, 5, true, false);
			string sLevelDesc = gameLevelInfo.sLevelDesc;
			string introduce = "Exp: " + gameLevelInfo.nRewardExp + "\nGold: " + gameLevelInfo.nRewardGold;
			List<TUIGoodsInfo> list4 = new List<TUIGoodsInfo>();
			if (gameLevelInfo.ltRewardMaterial != null)
			{
				foreach (CRewardMaterial item2 in gameLevelInfo.ltRewardMaterial)
				{
					if (item2.nID != 0)
					{
						list4.Add(new TUIGoodsInfo(item2.nID));
					}
				}
			}
			TUILevelInfo level_info = new TUILevelInfo(wParam, sLevelDesc, introduce, list4, recommend_role_info, null, gameLevelInfo.sLevelName);
			TUIGameInfo tUIGameInfo3 = new TUIGameInfo();
			tUIGameInfo3.map_info = new TUIMapInfo(level_info);
			global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.BackEvent_SceneMap(m_event.GetEventName(), tUIGameInfo3));
		}
		else if (m_event.GetEventName() == "TUIEvent_EnterLevel")
		{
			iGameState gameState2 = iGameApp.GetInstance().m_GameState;
			if (gameState2 != null)
			{
				int wParam2 = m_event.GetWParam();
				Debug.Log(wParam2);
				string text = "Scene_Main";
				gameState2.GameLevel = wParam2;
				iGameApp.GetInstance().EnterScene(kGameSceneEnum.Game);
			}
		}
		else if (m_event.GetEventName() == "TUIEvent_Back")
		{
			global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.BackEvent_SceneMap(m_event.GetEventName()));
		}
	}

	private void TUIEvent_BackInfo_SceneIAP(object sender, TUIEvent.SendEvent_SceneIAP m_event)
	{
		if (m_event.GetEventName() == "TUIEvent_TopBar")
		{
			iGameData gameData = iGameApp.GetInstance().m_GameData;
			if (gameData == null)
			{
				return;
			}
			iDataCenter dataCenter = gameData.GetDataCenter();
			if (dataCenter == null)
			{
				return;
			}
			CCharSaveInfo character = dataCenter.GetCharacter(dataCenter.CurCharID);
			if (character != null)
			{
				CCharacterInfoLevel characterInfo = gameData.GetCharacterInfo(character.nID, character.nLevel);
				if (characterInfo != null)
				{
					TUIGameInfo tUIGameInfo = new TUIGameInfo();
					tUIGameInfo.player_info = new TUIPlayerInfo();
					tUIGameInfo.player_info.avatar_id = character.nID;
					tUIGameInfo.player_info.level = character.nLevel;
					tUIGameInfo.player_info.level_exp = characterInfo.nExp;
					tUIGameInfo.player_info.exp = character.nExp;
					tUIGameInfo.player_info.gold = dataCenter.Gold;
					tUIGameInfo.player_info.crystal = dataCenter.Crystal;
					global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.BackEvent_SceneIAP(m_event.GetEventName(), tUIGameInfo));
				}
			}
		}
		else if (m_event.GetEventName() == "TUIEvent_IAPBuy")
		{
			int wParam = m_event.GetWParam();
			if (iGameApp.GetInstance().m_IAPManager != null)
			{
				iGameApp.GetInstance().m_IAPManager.Purchase(wParam);
			}
			global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.BackEvent_SceneIAP(m_event.GetEventName()));
		}
		else if (m_event.GetEventName() == "TUIEvent_Back")
		{
			global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.BackEvent_SceneIAP(m_event.GetEventName()));
		}
	}
}

using Dissonance.Integrations.MirrorIgnorance;
using Harmony;
using Mirror;
using RemoteAdmin;
using System.Reflection;
using UnityEngine;

namespace BetterSCP939.Patches
{
	[HarmonyPatch(typeof(PlayerStats), nameof(PlayerStats.HurtPlayer))]
	public class PlayerHurtOverride
	{
		public static bool Prefix(PlayerStats __instance, PlayerStats.HitInfo info, GameObject go, ref bool __result, bool ____allowSPDmg, Scp079Interactable.InteractableType[] ____filters, ref int ___killstreak, ref float ___killstreak_time, bool ____pocketCleanup)
		{
			bool result = false;
			bool flag = go == null;
			if (info.Amount < 0f)
			{
				if (flag)
				{
					info.Amount = Mathf.Abs(999999f);
				}
				else
				{
					PlayerStats component = go.GetComponent<PlayerStats>();
					info.Amount = ((component != null) ? Mathf.Abs(component.health + component.syncArtificialHealth + 10f) : Mathf.Abs(999999f));
				}
			}
			if (info.Amount > 2.14748365E+09f)
			{
				info.Amount = 2.14748365E+09f;
			}
			if (flag)
			{
				__result = result;
				return false;
			}
			PlayerStats component2 = go.GetComponent<PlayerStats>();
			CharacterClassManager component3 = go.GetComponent<CharacterClassManager>();
			if (component2 == null || component3 == null)
			{
				__result = false;
				return false;
			}
			if (component3.GodMode)
			{
				__result = false;
				return false;
			}
			if (__instance.ccm.Classes.SafeGet(__instance.ccm.CurClass).team == Team.SCP && __instance.ccm.Classes.SafeGet(component3.CurClass).team == Team.SCP && __instance.ccm != component3)
			{
				__result = false;
				return false;
			}
			if (component3.SpawnProtected && !____allowSPDmg)
			{
				__result = false;
				return false;
			}
			if (__instance.isLocalPlayer && info.PlyId != go.GetComponent<QueryProcessor>().PlayerId)
			{
				RoundSummary.Damages += ((component2.health < info.Amount) ? component2.health : info.Amount);
			}
			if (__instance.lastHitInfo.Attacker == "ARTIFICIALDEGEN")
			{
				component2.unsyncedArtificialHealth -= info.Amount;
				if (component2.unsyncedArtificialHealth < 0f)
				{
					component2.unsyncedArtificialHealth = 0f;
				}
			}
			else
			{
				if (component2.unsyncedArtificialHealth > 0f && component3.CurClass != RoleType.Scp93953 && component3.CurClass != RoleType.Scp93989)
				{
					float num = info.Amount * __instance.artificialNormalRatio;
					float num2 = info.Amount - num;
					component2.unsyncedArtificialHealth -= num;
					if (component2.unsyncedArtificialHealth < 0f)
					{
						num2 += Mathf.Abs(component2.unsyncedArtificialHealth);
						component2.unsyncedArtificialHealth = 0f;
					}
					component2.health -= num2;
					if (component2.health > 0f && component2.health - num <= 0f)
					{
						__instance.TargetAchieve(__instance.connectionToClient, "didntevenfeelthat");
					}
				}
				else
				{
					component2.health -= info.Amount;
				}
				if (component2.health < 0f)
				{
					component2.health = 0f;
				}
				component2.lastHitInfo = info;
			}
			if (component2.health < 1f && component3.CurClass != RoleType.Spectator)
			{
				foreach (Scp079PlayerScript scp079PlayerScript in Scp079PlayerScript.instances)
				{
					Scp079Interactable.ZoneAndRoom otherRoom = go.GetComponent<Scp079PlayerScript>().GetOtherRoom();
					bool flag2 = false;
					foreach (Scp079Interaction scp079Interaction in scp079PlayerScript.ReturnRecentHistory(12f, ____filters))
					{
						foreach (Scp079Interactable.ZoneAndRoom zoneAndRoom in scp079Interaction.interactable.currentZonesAndRooms)
						{
							if (zoneAndRoom.currentZone == otherRoom.currentZone && zoneAndRoom.currentRoom == otherRoom.currentRoom)
							{
								flag2 = true;
							}
						}
					}
					if (flag2)
					{
						scp079PlayerScript.RpcGainExp(ExpGainType.KillAssist, component3.CurClass);
					}
				}
				if (RoundSummary.RoundInProgress() && RoundSummary.roundTime < 60)
				{
					__instance.TargetAchieve(component3.connectionToClient, "wowreally");
				}
				if (__instance.isLocalPlayer && info.PlyId != go.GetComponent<QueryProcessor>().PlayerId)
				{
					RoundSummary.Kills++;
				}
				result = true;
				Scp049PlayerScript[] array = UnityEngine.Object.FindObjectsOfType<Scp049PlayerScript>();
				for (int i = 0; i < array.Length; i++)
				{
					array[i].RpcSetDeathTime(go);
				}
				if (component3.CurClass == RoleType.Scp096 && go.GetComponent<Scp096PlayerScript>().enraged == Scp096PlayerScript.RageState.Panic)
				{
					__instance.TargetAchieve(component3.connectionToClient, "unvoluntaryragequit");
				}
				else if (info.GetDamageType() == DamageTypes.Pocket)
				{
					__instance.TargetAchieve(component3.connectionToClient, "newb");
				}
				else if (info.GetDamageType() == DamageTypes.Scp173)
				{
					__instance.TargetAchieve(component3.connectionToClient, "firsttime");
				}
				else if (info.GetDamageType() == DamageTypes.Grenade && info.PlyId == go.GetComponent<QueryProcessor>().PlayerId)
				{
					__instance.TargetAchieve(component3.connectionToClient, "iwanttobearocket");
				}
				else if (info.GetDamageType().isWeapon)
				{
					Inventory component4 = component3.GetComponent<Inventory>();
					if (component3.CurClass == RoleType.Scientist)
					{
						Item itemByID = component4.GetItemByID(component4.curItem);
						if (itemByID != null && itemByID.itemCategory == ItemCategory.Keycard && __instance.GetComponent<CharacterClassManager>().CurClass == RoleType.ClassD)
						{
							__instance.TargetAchieve(__instance.connectionToClient, "betrayal");
						}
					}
					if (Time.realtimeSinceStartup - ___killstreak_time > 30f || ___killstreak == 0)
					{
						___killstreak = 0;
						___killstreak_time = Time.realtimeSinceStartup;
					}
					if (__instance.GetComponent<WeaponManager>().GetShootPermission(component3, true))
					{
						___killstreak++;
					}
					if (___killstreak > 5)
					{
						__instance.TargetAchieve(__instance.connectionToClient, "pewpew");
					}
					if ((__instance.ccm.Classes.SafeGet(__instance.ccm.CurClass).team == Team.MTF || __instance.ccm.Classes.SafeGet(__instance.ccm.CurClass).team == Team.RSC) && component3.CurClass == RoleType.ClassD)
					{
						__instance.TargetStats(__instance.connectionToClient, "dboys_killed", "justresources", 50);
					}
					if (__instance.ccm.Classes.SafeGet(__instance.ccm.CurClass).team == Team.RSC && __instance.ccm.Classes.SafeGet(component3.CurClass).team == Team.SCP)
					{
						__instance.TargetAchieve(__instance.connectionToClient, "timetodoitmyself");
					}
				}
				else if (__instance.ccm.Classes.SafeGet(__instance.ccm.CurClass).team == Team.SCP && go.GetComponent<MicroHID>().CurrentHidState != MicroHID.MicroHidState.Idle)
				{
					__instance.TargetAchieve(__instance.connectionToClient, "illpassthanks");
				}
				ServerLogs.AddLog(ServerLogs.Modules.ClassChange, string.Concat(new string[]
				{
				go.GetComponent<NicknameSync>().MyNick,
				" (",
				go.GetComponent<CharacterClassManager>().UserId,
				") killed by ",
				info.Attacker,
				" using ",
				info.GetDamageName(),
				"."
				}), ServerLogs.ServerLogType.KillLog);
				if (!____pocketCleanup || info.GetDamageType() != DamageTypes.Pocket)
				{
					go.GetComponent<Inventory>().ServerDropAll();
					if (component3.Classes.CheckBounds(component3.CurClass) && info.GetDamageType() != DamageTypes.RagdollLess)
					{
						__instance.GetComponent<RagdollManager>().SpawnRagdoll(go.transform.position, go.transform.rotation, (int)component3.CurClass, info, component3.Classes.SafeGet(component3.CurClass).team > Team.SCP, go.GetComponent<MirrorIgnorancePlayer>().PlayerId, go.GetComponent<NicknameSync>().MyNick, go.GetComponent<QueryProcessor>().PlayerId);
					}
				}
				else
				{
					go.GetComponent<Inventory>().Clear();
				}
				component3.NetworkDeathPosition = go.transform.position;
				if (component3.Classes.SafeGet(component3.CurClass).team == Team.SCP)
				{
					if (component3.CurClass == RoleType.Scp0492)
					{
						NineTailedFoxAnnouncer.CheckForZombies(go);
					}
					else
					{
						GameObject x = null;
						foreach (GameObject gameObject in PlayerManager.players)
						{
							if (gameObject.GetComponent<QueryProcessor>().PlayerId == info.PlyId)
							{
								x = gameObject;
							}
						}
						if (x != null)
						{
							NineTailedFoxAnnouncer.AnnounceScpTermination(component3.Classes.SafeGet(component3.CurClass), info, "");
						}
						else
						{
							DamageTypes.DamageType damageType = info.GetDamageType();
							if (damageType == DamageTypes.Tesla)
							{
								NineTailedFoxAnnouncer.AnnounceScpTermination(component3.Classes.SafeGet(component3.CurClass), info, "TESLA");
							}
							else if (damageType == DamageTypes.Nuke)
							{
								NineTailedFoxAnnouncer.AnnounceScpTermination(component3.Classes.SafeGet(component3.CurClass), info, "WARHEAD");
							}
							else if (damageType == DamageTypes.Decont)
							{
								NineTailedFoxAnnouncer.AnnounceScpTermination(component3.Classes.SafeGet(component3.CurClass), info, "DECONTAMINATION");
							}
							else if (component3.CurClass != RoleType.Scp079)
							{
								NineTailedFoxAnnouncer.AnnounceScpTermination(component3.Classes.SafeGet(component3.CurClass), info, "UNKNOWN");
							}
						}
					}
				}
				component2.SetHPAmount(100);
				component3.SetClassID(RoleType.Spectator);
			}
			else
			{
				Vector3 pos = Vector3.zero;
				float num3 = 40f;
				if (info.GetDamageType().isWeapon)
				{
					var GetPlayerOfID = (typeof(PlayerStats)).GetMethod("GetPlayerOfID", BindingFlags.Instance | BindingFlags.NonPublic);
					GameObject playerOfID = (GameObject)GetPlayerOfID.Invoke(__instance, new object[] { info.PlyId });
					if (playerOfID != null)
					{
						pos = go.transform.InverseTransformPoint(playerOfID.transform.position).normalized;
						num3 = 100f;
					}
				}
				else if (info.GetDamageType() == DamageTypes.Pocket)
				{
					PlyMovementSync component5 = __instance.ccm.GetComponent<PlyMovementSync>();
					if (component5.RealModelPosition.y > -1900f)
					{
						component5.OverridePosition(Vector3.down * 1998.5f, 0f, true);
					}
				}
				var TargetOofEffect = (typeof(PlayerStats)).GetMethod("TargetOofEffect", BindingFlags.Instance | BindingFlags.NonPublic);
				TargetOofEffect.Invoke(__instance, new object[] { go.GetComponent<NetworkIdentity>().connectionToClient, pos, Mathf.Clamp01(info.Amount / num3) });
			}
			__result = result;
			return false;
		}
	}
}

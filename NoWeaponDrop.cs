﻿// Reference: 0Harmony

using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using Oxide.Plugins;
using Harmony;
using UnityEngine;

namespace Oxide.Plugins
{
	[Info("No Weapon Drop", "Strobez", "1.0.0")]
	internal class NoWeaponDrop : RustPlugin
	{
		private static HarmonyInstance? _harmonyInstance;

		private void OnServerInitialized()
		{
			try
			{
				_harmonyInstance = HarmonyInstance.Create(Name + "Patches");
				_harmonyInstance.PatchAll();
			}
			catch (Exception ex)
			{
				Debug.LogError($"Error creating Harmony instance: {ex}");
			}
		}

		private void Unload()
		{
			try
			{
				_harmonyInstance?.UnpatchAll(_harmonyInstance.Id);
			}
			catch (Exception ex)
			{
				Debug.LogError($"Error unpatching Harmony instance: {ex}");
			}
		}

		[HarmonyPatch(typeof(BasePlayer), nameof(BasePlayer.ShouldDropActiveItem))]
		internal class BasePlayer_ShouldDropActiveItem_Patch
		{
			[HarmonyTranspiler]
			internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> originalInstructions)
			{
				List<CodeInstruction> list = new(originalInstructions)
				{
					new CodeInstruction(OpCodes.Ldc_I4_0),
					new CodeInstruction(OpCodes.Ret)
				};

				return list;
			}
		}
	}
}

using HarmonyLib;

namespace VAC.AntiParams
{
    [HarmonyPatch(typeof (Character), "RPC_Damage")]
    public class DamageCharacter_Patch
    {
        private static bool Prefix(ref Character __instance, ref long sender, ref HitData hit)
        {
            if (VACPlugin.AntiParams_IsEnabled.Value)
            {
                if(VACPlugin.debugmode.Value)
                    ZLog.LogWarning("Damage to Character");
                return Damage_Rule.Execute(hit);
            }
            else
            {
                return true;
            }
        }
    }
    
    [HarmonyPatch(typeof (WearNTear), "RPC_Damage")]
    public class DamageWearNTear_Patch
    {
        private static bool Prefix(ref WearNTear __instance,ref long sender, ref HitData hit)
        {
            if (VACPlugin.AntiParams_IsEnabled.Value)
            {
                if(VACPlugin.debugmode.Value)
                    ZLog.LogWarning("Damage to WearNTear");
                return Damage_Rule.Execute(hit);
            }
            else
            {
                return true;
            }
        }
    }
    
    [HarmonyPatch(typeof (TreeBase), "RPC_Damage")]
    public class DamageTreeBase_Patch
    {
        private static bool Prefix(ref TreeBase __instance,ref long sender, ref HitData hit)
        {
            if (VACPlugin.AntiParams_IsEnabled.Value)
            {
                if(VACPlugin.debugmode.Value)
                    ZLog.LogWarning("Damage to TreeBase");
                return Damage_Rule.Execute(hit);
            }
            else
            {
                return true;
            }
        }
    }
    
    [HarmonyPatch(typeof (TreeLog), "RPC_Damage")]
    public class DamageTreeLog_Patch
    {
        private static bool Prefix(ref TreeLog __instance,ref long sender, ref HitData hit)
        {
            if (VACPlugin.AntiParams_IsEnabled.Value)
            {
                if(VACPlugin.debugmode.Value)
                    ZLog.LogWarning("Damage to TreeBase");
                return Damage_Rule.Execute(hit);
            }
            else
            {
                return true;
            }
        }
    }
        
    [HarmonyPatch(typeof (MineRock5), "RPC_Damage")]
    public class DamageMineRock5_Patch
    {
        private static bool Prefix(ref MineRock5 __instance,ref long sender, ref HitData hit)
        {
            if (VACPlugin.AntiParams_IsEnabled.Value)
            {
                if(VACPlugin.debugmode.Value)
                    ZLog.LogWarning("Damage to MineRock5");
                return Damage_Rule.Execute(hit);
            }
            else
            {
                return true;
            }
        }
    }
    
    [HarmonyPatch(typeof (Destructible), "RPC_Damage")]
    public class DamageDestructible_Patch
    {
        private static bool Prefix(ref Destructible __instance,ref long sender, ref HitData hit)
        {
            if (VACPlugin.AntiParams_IsEnabled.Value)
            {
                if(VACPlugin.debugmode.Value)
                    ZLog.LogWarning("Damage to MineRock5");
                return Damage_Rule.Execute(hit);
            }
            else
            {
                return true;
            }
        }
    }
}
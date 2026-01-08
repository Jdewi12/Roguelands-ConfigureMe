using UnityEngine;
using GadgetCore.API;

namespace ConfigureMe
{
    [Gadget(nameof(ConfigureMe), RequiredOnClients: false)]
    public class ConfigureMe : Gadget 
    {

        // todo: Make sure we're applying the enemy hp/exp/credit multipliers in as many ways as possible
        public const string MOD_VERSION = "1.2";
        public const string CONFIG_VERSION = "1.1"; // Increment whenever config format changes.

        public static GadgetCore.GadgetLogger logger;

        public static float TimeScale;
        public static float DamageTakenMultiplier;
        public static int DamageTakenCap;
        public static float BurnReceivedMultiplier;
        public static int BurnReceivedLimit;
        public static float PoisonReceivedMultiplier;
        public static int PoisonReceivedLimit;
        public static float FrostReceivedMultiplier;
        public static int FrostReceivedLimit;
        public static int[] BonusStats = new int[6];
        public static float EnemyHpMultiplier;
        public static float EnemyHpPlayerMultiplier;
        public static float EnemyExpMultiplier;
        public static float EnemyCreditsMultiplier;
        public static float GearExpMultiplier;
        public static float GunCannonDamageMultiplier;
        public static float StaffDamageMultiplier;
        public static float GauntletDamageMultiplier;
        public static float MeleeDamageMultiplier;

        public static void Log(string text)
        {
            logger.Log(text);
        }

        protected override void LoadConfig()
        {
            logger = base.Logger;
            Config.Load();
            string fileVersion = Config.ReadString("ConfigVersion", CONFIG_VERSION, comments: "The Config Version (not to be confused with mod version)");
            if(fileVersion != CONFIG_VERSION)
            {
                Config.Reset();
                Config.WriteString("ConfigVersion", CONFIG_VERSION, comments: "The Config Version (not to be confused with mod version)");
            }
            TimeScale = Config.ReadFloat(nameof(TimeScale), 1f, vanillaValue: 1, comments: "Multiplier for the speed at which the game runs. Examples values: 0.5 is half, 1 is normal, 2 is double.");
            DamageTakenMultiplier = Config.ReadFloat(nameof(DamageTakenMultiplier), 1f, vanillaValue: 1, comments: "Multiplier for damage taken by the player. Applied before reductions from uniforms/augment. Example values: 0.5 is half, 1 is normal, 2 is double. Damage is rounded after multiplying.");
            DamageTakenCap = Config.ReadInt(nameof(DamageTakenCap), int.MaxValue, vanillaValue: int.MaxValue, comments: $"Cap for the total damage an enemy can deal in one hit. This is separate from the effect that lets you survive on 1hp. Applied before reductions from uniforms/augments and before nuldmg, but after { nameof(DamageTakenMultiplier) }.");
            BurnReceivedMultiplier = Config.ReadFloat(nameof(BurnReceivedMultiplier), 1f, vanillaValue: 1f, comments: "Multiplier for any burn effect received (the percentage that stacks up). Examples values: 0.5 is half, 1 is normal, 2 is double.");
            BurnReceivedLimit = Config.ReadInt(nameof(BurnReceivedLimit), 100, vanillaValue: int.MaxValue, comments: "Cap for the total burn percentage the player can have. Example values: 50 is 50%, 100 is 100%. In vanilla there is no cap.");
            PoisonReceivedMultiplier = Config.ReadFloat(nameof(PoisonReceivedMultiplier), 1f, vanillaValue: 1f, comments: "Multiplier for any poison effect received.");
            PoisonReceivedLimit = Config.ReadInt(nameof(PoisonReceivedLimit), 100, vanillaValue: int.MaxValue, comments: "Cap for the total poison percentage the player can have.");
            FrostReceivedMultiplier = Config.ReadFloat(nameof(FrostReceivedMultiplier), 1f, vanillaValue: 1f, comments: "Multiplier for any frost effect received.");
            FrostReceivedLimit = Config.ReadInt(nameof(FrostReceivedLimit), 100, vanillaValue: int.MaxValue, comments: "Cap for the total frost percentage the player can have.");
            BonusStats[0] = Config.ReadInt(nameof(BonusStats) + "VIT", 0, vanillaValue: 0, comments: "Bonus stats, applied to all new and existing characters.");
            BonusStats[1] = Config.ReadInt(nameof(BonusStats) + "STR", 0, vanillaValue: 0, comments: "");
            BonusStats[2] = Config.ReadInt(nameof(BonusStats) + "DEX", 0, vanillaValue: 0, comments: "");
            BonusStats[3] = Config.ReadInt(nameof(BonusStats) + "TEC", 0, vanillaValue: 0, comments: "");
            BonusStats[4] = Config.ReadInt(nameof(BonusStats) + "MAG", 0, vanillaValue: 0, comments: "");
            BonusStats[5] = Config.ReadInt(nameof(BonusStats) + "FTH", 0, vanillaValue: 0, comments: "");

            EnemyHpMultiplier = Config.ReadFloat(nameof(EnemyHpMultiplier), 1f, vanillaValue: 1f, comments: "Multiplier for enemies' HP.");
            EnemyHpPlayerMultiplier = Config.ReadFloat(nameof(EnemyHpPlayerMultiplier), 0f, vanillaValue: 0f, comments: "Adds this amount to the above EnemyHpMultiplier for each player after the first in multiplayer.");
            EnemyExpMultiplier = Config.ReadFloat(nameof(EnemyExpMultiplier), 1f, vanillaValue: 1f, comments: "Multiplier for the amount of exp dropped by enemies. Does not apply to some enemies that have hard-coded drops.");
            EnemyCreditsMultiplier = Config.ReadFloat(nameof(EnemyCreditsMultiplier), 1f, vanillaValue: 1f, comments: "Multiplier for the number of credits dropped by enemies. Does not apply to some enemies that have hard-coded drops.");
            GearExpMultiplier = Config.ReadFloat(nameof(GearExpMultiplier), 1f, vanillaValue: 1f, comments: "Multiplier for the amount of gear exp rewarded at the end of each planet.");
            MeleeDamageMultiplier = Config.ReadFloat(nameof(MeleeDamageMultiplier), 1f, vanillaValue: 1f, comments: "Multiplier for the amount of damage dealy by the player with sword and lance melee attacks");
            GunCannonDamageMultiplier = Config.ReadFloat(nameof(GunCannonDamageMultiplier), 1f, vanillaValue: 1f, comments: "Multiplier for the amount of damage dealy by the player with gun and cannon projectiles.");
            GauntletDamageMultiplier = Config.ReadFloat(nameof(GauntletDamageMultiplier), 1f, vanillaValue: 1f, comments: "Multiplier for the amount of damage dealy by the player with gauntlet projectiles.");
            StaffDamageMultiplier = Config.ReadFloat(nameof(StaffDamageMultiplier), 1f, vanillaValue: 1f, comments: "Multiplier for the amount of damage dealy by the player with staff projectiles.");

            Config.Save();
        }

        protected override void Initialize()
        {
            Logger.Log(nameof(ConfigureMe) + " v" + Info.Mod.Version);
            
            if(Gadgets.GetGadget("BigNumberCore") != null)
            {
                logger.LogWarning("BigNumberCore present. Some things may not work.");
            }
            
        }

        public static int ScaleEnemyHP(int oldHP, int numAdditionalPlayers)
        {
            return Mathf.RoundToInt(oldHP * (EnemyHpMultiplier + EnemyHpPlayerMultiplier * numAdditionalPlayers));
        }

        public static int ScaleEnemyExp(int oldExp, int numAdditionalPlayers)
        {
            return Mathf.RoundToInt(oldExp * EnemyExpMultiplier);
        }

        public static int ScaleEnemyCredits(int oldCredits, int numAdditionalPlayers)
        {
            return Mathf.RoundToInt(oldCredits * EnemyCreditsMultiplier);
        }
    }
}
using Kriptok.Extensions;
using Kriptok.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.JAEditor.Data
{
    public enum ItemsEnum
    {
        Nothing = 0,

        W_Gun_38 = 1 | HasBullets | MaxBullets6,
        W_Colt_45 = 2 | HasBullets | MaxBullets6 | AllowSilencer | AllowSniperScope,
        W_Pistol_9mm = 3 | HasBullets | MaxBullets15 | AllowSilencer | AllowSniperScope,
        W_Magnum_357 = 4 | HasBullets | MaxBullets10 | AllowSilencer | AllowSniperScope,
        W_Colt_45_Modified = 5 | HasBullets | MaxBullets6 | AllowSilencer | AllowSniperScope,
        W_Pistol_9mm_Modified = 6 | HasBullets | MaxBullets15 | AllowSilencer | AllowSniperScope,
        W_Magnum_357_Modified = 7 | HasBullets | MaxBullets10 | AllowSilencer | AllowSniperScope,
        W_Shotgun_12g = 8 | HasBullets | MaxBullets6,
        W_Rifle_12g = 9 | HasBullets | MaxBullets6 | AllowSniperScope,
        W_Rifle_M14 = 10 | HasBullets | MaxBullets20 | AllowSniperScope,
        W_Rifle_M16 = 11 | HasBullets | MaxBullets20 | AllowSniperScope,
        W_Rifle_12g_Modified = 12 | HasBullets | MaxBullets6 | AllowSniperScope,
        W_Rifle_M14_Modified = 13 | HasBullets | MaxBullets20 | AllowSniperScope,
        W_Rifle_M16_Modified = 14 | HasBullets | MaxBullets20 | AllowSniperScope,

        Knife = 15,
        CombatKnife = 16,
        Mine = 17,

        // EnemyMine = 18, // BUG!!!
        // FlaggedMine = 19, // BUG!!! 
        // SmokeBomb = 20,
        // EagleSmokeBall = 21,

        TearGas = 22 | Max5,
        EagleScreamer = 23,
        StunGrenade = 24 | Max5,
        EagleSilencer = 25,
        MustardGas = 26 | Max5,
        EagleDog = 27,

        Grenade = 28 | Max5,
        MolotovCoctail = 29,

        EagleFearball = 30,
        LiveExplosives = 31,
        LivePlasticExplosives = 32,
        PlasticExplosives = 33,
        Explosives = 34,

        // --------------------------------------------------------------
        Helmet = 35 | IsHelmet,
        TreatedHelmet = 36 | IsHelmet,
        KevlarHelmet = 37 | IsHelmet,
        TreatedKevlarHelmet = 38 | IsHelmet,
        // --------------------------------------------------------------
        SunGoggles = 39 | IsFaceItem | Max3,
        GasMask = 40 | IsFaceItem,
        // UnusedFace = 41,
        // --------------------------------------------------------------
        Radio = 42 | IsEarsItem,
        ExtendedEar = 43 | IsEarsItem,
        // UnusedRadio = 44,
        // --------------------------------------------------------------
        KevlarVest = 45 | IsBodyItem,
        TreatedKevlarVest = 46 | IsBodyItem,
        SpectraShield = 47 | IsBodyItem,
        TreatedSpectraShield = 48 | IsBodyItem,
        UltraVest = 49 | IsVest,
        // --------------------------------------------------------------
        Vest2Pockets = 50 | IsVest,
        Vest3Pockets = 51 | IsVest,
        Vest4Pockets = 52 | IsVest,
        Vest5Pockets = 53 | IsVest,
        // --------------------------------------------------------------
        Ammo_38 = 54 | IsAmmo,
        Ammo_45 = 55 | IsAmmo,
        Ammo_9mm = 56 | IsAmmo,
        Ammo_357 = 57 | IsAmmo,
        // UnusedBullets_58 = 58 | IsAmmo,
        Ammo_12g = 59 | IsAmmo,
        // --------------------------------------------------------------
        Ammo_14m = 60 | IsAmmo,
        Ammo_16m = 61 | IsAmmo,
        // UnusedBullets_62 = 62,
        // UnusedBullets_63 = 63,
        Detonator = 64 | Max2,
        Canteen = 65 | Max5,
        SniperScope = 66 | Max2,
        Steel = 67 | Max2,
        LocatorStrobe = 68,
        WallProbe = 69,
        // --------------------------------------------------------------
        Camera = 70,
        CamouflageKit = 71 | Max2,
        Compound17 = 72 | Max2,
        FirstAidKit = 73 | Max3,
        MedicalKit = 74 | Max2,
        // --------------------------------------------------------------
        Toolbox = 75,
        Locksmith = 76,
        MetalDetector = 77,
        // GasDetector = 78,
        CaseBeer = 79,
        // --------------------------------------------------------------        
        // MineMarker = 80,
        PomillaFlower = 81,
        // PlasticBag = 82,
        GasCan = 83,
        GlassJar = 84 | Max3,
        Rag = 85 | Max3,
        OilCan = 86,
        // ---------------------------------------------------------------
        Key = 87 | Max5,
        PadlockKey = 88 | Max5,
        Note01 = 89,
        Note02 = 90,
        // --------------------------------------------------------------
        Silencer = 91 | Max3,
        // --------------------------------------------------------------
        BrendasBra = 92,
        // --------------------------------------------------------------
        Rock = 93 | Max5,
        // --------------------------------------------------------------
        Antidote = 94,
        Sapling = 95,
        // --------------------------------------------------------------
        Note03 = 96,
        Note04 = 97,
        Note05 = 98,
        Note06 = 99,
        Note07 = 100,
        Note08 = 101,
        Note09 = 102,
        Note10 = 103,
        Note11 = 104,
        Note12_Brenda = 105,
        Note13 = 106,
        Note14 = 107,
        Note15 = 108,
        // Note_Empty = 109,
        BrendasJournal = 110,
        // Unused = 111,
        // Unused = 112,
        // Unused = 113,
        // Unused = 114,
        // ---------------------------------------------------------------
        PieceOfPaper1 = 115 | Max3,
        PieceOfPaper2 = 116,
        // ---------------------------------------------------------------
        FishRemains = 117,
        Crowbar = 118,
        TumbStone = 119,
        // ---------------------------------------------------------------
        ClockBugDetector = 120,
        // ---------------------------------------------------------------        
        Micropurifier = 121,
        BrendasBlouse = 122,
        DestroyedSapling = 123,
        DestroyedJournal = 124,
        // ---------------------------------------------------------------        
        GlassJarWithOil = 125,
        GlassJarWithGas = 126,
        GlassJarWithRag = 127,
        GlassJarWithGasAndOil = 128,
        GlassJarWithGasAndRag = 129,
        GlassJarWithOilAndRag = 130,
        // ---------------------------------------------------------------
        GasCanWithOil = 131,
        GasCanWithRag = 132,
        GasCanWithOilAndRag = 133,      
        BrendasLabCoat = 134,
        // ---------------------------------------------------------------
        PieceOfPaper3 = 135,
        // ---------------------------------------------------------------
        Max2             = 0b100000000,
        Max3             = 0b1000000000,
        Max5             = 0b10000000000,
        IsHelmet         = 0b100000000000,
        IsFaceItem       = 0b1000000000000,
        IsEarsItem       = 0b10000000000000,
        IsBodyItem       = 0b100000000000000,
        IsVest           = 0b1000000000000000,
        HasBullets       = 0b10000000000000000,
        MaxBullets6      = 0b100000000000000000,
        MaxBullets10     = 0b1000000000000000000,
        MaxBullets15     = 0b10000000000000000000,
        MaxBullets20     = 0b100000000000000000000,
        AllowSilencer    = 0b1000000000000000000000,
        AllowSniperScope = 0b10000000000000000000000,
        AllowAttachment  = AllowSilencer | AllowSniperScope,
        IsAmmo = Max5,

        MaxUnknown = Max2
    }

    public static class ItemsEnumExtensions
    {
        public static int GetMin(this ItemsEnum ie)
        {
            if ((ie & ItemsEnum.HasBullets) == ItemsEnum.HasBullets)
            {
                // Si tiene balas, puede estar en 0 balas.
                return 0;
            }
            else
            {
                return 1;
            }
        }

        public static int GetMax(this ItemsEnum ie)
        {
            if ((ie & ItemsEnum.MaxBullets6) == ItemsEnum.MaxBullets6)
            {
                return 6;
            }
            else if ((ie & ItemsEnum.MaxBullets10) == ItemsEnum.MaxBullets10)
            {
                return 10;
            }
            else if ((ie & ItemsEnum.MaxBullets15) == ItemsEnum.MaxBullets15)
            {
                return 15;
            }
            else if ((ie & ItemsEnum.MaxBullets20) == ItemsEnum.MaxBullets20)
            {
                return 20;
            }
            else if ((ie & ItemsEnum.Max2) == ItemsEnum.Max2)
            {
                return 2;
            }
            else if ((ie & ItemsEnum.Max3) == ItemsEnum.Max3)
            {
                return 3;
            }
            else if ((ie & ItemsEnum.Max5) == ItemsEnum.Max5)
            {
                return 5;
            }
            return 1;
        }
    }

    public static class ItemsEnumHelper
    {
        public static ItemsEnum Find(byte val)
        {
            if (val != 0)
            {
                var items = EnumTypeHelper.GetValues<ItemsEnum>();
                if (items.Any(p => ((byte)p & 0xFF) == val))
                {
                    return items.FirstOrDefault(p => ((byte)p & 0xFF) == val);
                }
                else
                {
#if DEBUG
                    Trace.WriteLine($"-- ITEM NOT FOUND: {val} --");
                    Debugger.Break();
#endif
                    return (ItemsEnum)val;
                }                
            }
            return ItemsEnum.Nothing;
        }

        public static ItemsEnum[] GetAttachments() => new ItemsEnum[]
        {
            ItemsEnum.SniperScope,
            ItemsEnum.Silencer
        };

        public static ItemsEnum[] WithoutFlags()
        {
            return EnumTypeHelper.GetValues<ItemsEnum>().Where(p => !p.In(new ItemsEnum[]
            {
                ItemsEnum.Nothing,
                ItemsEnum.IsAmmo,
                ItemsEnum.Max2,
                ItemsEnum.Max3,
                ItemsEnum.Max5,
                ItemsEnum.IsHelmet,
                ItemsEnum.IsFaceItem,
                ItemsEnum.IsEarsItem,
                ItemsEnum.IsBodyItem,
                ItemsEnum.IsVest,
                ItemsEnum.HasBullets,
                ItemsEnum.MaxBullets6,
                ItemsEnum.MaxBullets10,
                ItemsEnum.MaxBullets15,
                ItemsEnum.MaxBullets20,
                ItemsEnum.AllowSilencer,
                ItemsEnum.AllowSniperScope,
                ItemsEnum.AllowAttachment,
                ItemsEnum.MaxUnknown
            })).ToArray();
        }
    }
}

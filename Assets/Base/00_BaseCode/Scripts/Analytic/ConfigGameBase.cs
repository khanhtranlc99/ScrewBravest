using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigGameBase
{
    public static string settingProductName = "Nuts And Bolts: Screw Master";

    public const string settingKeyStore = "screwmaster";
    public static string keyaliasPass = "12345678two";
    public static string keystorePass = "12345678two";
    public static string settingAliasName = "screwmaster";

    public const string settingLogo = "GAME_ICON";

    public static int versionCode = 2023022501;//sua
    public static string versionName = "1.0.9";//sua
    public static int settingVersionCode = 2023022501;//sua
    public static string settingVersionName = "1.0.9";//sua

    public static string productNameBuild = "Nuts And Bolts: Screw Master";

    public static int VersionCodeAll
    {
        get
        {
            return versionCode / 100;
        }
    }

    public static int VersionFirstInstall
    {
        get
        {
            int data = PlayerPrefs.GetInt(StringHelper.VERSION_FIRST_INSTALL, 0);
            if (data == 0)
            {
                PlayerPrefs.SetInt(StringHelper.VERSION_FIRST_INSTALL, versionCode);
                data = versionCode;
            }

            return data;
        }
    }

    public static string inappAndroidKeyHash
        = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAxH15vU9+fsUnRq1d9d/ETWrY+DXxhWX3hE310JTGVRo7CLrY8auzrzvummp2MB7ryC5MbYdWrOcKgRPYvpy0qRSOB0HxEDLgHkH2rpia7n5yLoX6gSQ0KwlgC0pLDuK+2XsuayAjTg7xnbQfspzPbY+bP6vkqZAADLAue7Iq94g9QXd7ISOFjN/v7L3dppr5TtwHfZAbD+yZyreFbowGhxdMWKkMKBuskBJYD3KpO3KxSSyo/MUz0zpw2hCizOHqUIh08FG3TA7qd0y0gsWFqcPgSKkgV0JZ/WvvCd5vO4BAFuE8bpdQYKCspdUFeJ9iLqiu2agGz//Q498Iu++ogwIDAQAB";
#if UNITY_ANDROID
    public const string package_name = "com.zgames.screw.puzzle.nuts.bolts";
#else
    public const string package_name = "com.zgames.screw.puzzle.nuts.bolts";
#endif



#if UNITY_ANDROID
    public static string OPEN_LINK_RATE = "market://details?id=" + package_name;
#else
    public static string OPEN_LINK_RATE = "itms-apps://itunes.apple.com/app/id6443535076";
#endif

    public static string FanpageLinkWeb = "https://www.facebook.com/groups/402513540729752/";
    public static string FanpageLinkApp = "https://www.facebook.com/groups/402513540729752/";

    public static string LinkFeedback = "https://www.facebook.com/groups/402513540729752/";
    public static string LinkPolicy = "https://sites.google.com/view/onesoft/privacy-policy";
    public static string LinkTerm = "https://sites.google.com/view/mini-game-puzzle-fun-policy/";

#if UNITY_ANDROID
    public const string IRONSOURCE_DEV_KEY = "1583b0af5";
#else
 public const string IRONSOURCE_DEV_KEY = "16a9dd175";
#endif


#if UNITY_ANDROID
    public const string ADJUST_APP_TOKEN = "fpe2gt5c3f9c";
#else
    public const string ADJUST_APP_TOKEN = "fpe2gt5c3f9c";
#endif
}

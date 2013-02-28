using UnityEngine;
using System.Collections;

public class Main
{

    public const float GAMEWIDTH = 1024;
    public const float GAMEHEIGHT = 768;
    public const float fixedratio = 4f / 3f;

    public const float FixedGameWidth = 1024f;
    public const float FixedGameHeight = 768f;
    public const float HD_HEIGHT = 720f;


    public class Mz_AppVersion
    {

        public enum AppVersion { Free = 0, Pro = 1 };
        public AppVersion appVersion;
        public static AppVersion getAppVersion;
    }

    public class Mz_AppLanguage
    {
        public enum SupportLanguage
        {
            EN = 0,
            TH = 1,
        };
        public static SupportLanguage appLanguage;
    }
}
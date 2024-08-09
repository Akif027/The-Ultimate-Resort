
public static class Settings
{
    public static readonly string GAME_DATA = "GAME_DATA";

    public static readonly string CURRENT_LEVEL = "CURRENT_LEVEL";
    public static readonly string LEVEL_MAX = "LEVEL_MAX";
    public static readonly string MONEY_KEY = "MONEY";

    public static SecuredDouble Money
    {
        get => SecuredDouble.Parse(IPlayerPrefs.GetString(MONEY_KEY));
        set => IPlayerPrefs.SetString(MONEY_KEY, value.ToString());
    }

    public static int LevelMax
    {
        get
        {
            return IPlayerPrefs.GetInt(LEVEL_MAX);
        }

        set
        {
            IPlayerPrefs.SetInt(LEVEL_MAX, value);
        }
    }



    public static int CurrentLevel
    {
        get
        {
            return IPlayerPrefs.GetInt(CURRENT_LEVEL);
        }

        set
        {
            IPlayerPrefs.SetInt(CURRENT_LEVEL, value);
            if (LevelMax < CurrentLevel)
            {
                LevelMax = CurrentLevel;
            }
        }
    }

    public static GameData GameData
    {
        get
        {
            return IPlayerPrefs.Get<GameData>(GAME_DATA);
        }

        set
        {
            IPlayerPrefs.Set(GAME_DATA, value);
        }
    }
}


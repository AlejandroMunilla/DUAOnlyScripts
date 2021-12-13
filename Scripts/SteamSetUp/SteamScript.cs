
using UnityEngine;
using Steamworks;
using System.Collections;
using System;



public class SteamScript : MonoBehaviour
{


    protected Callback<GameOverlayActivated_t> m_GameOverlayActivated;
    protected CallResult<NumberOfCurrentPlayers_t> m_NumberOfCurrentPlayers;
    private SteamAPIWarningMessageHook_t m_SteamAPIWarningMessageHook;
    private string masterIDSteam;
    public bool Check_It;
    private bool bRunning = true;
    private AppId_t myID;
    private bool m_bInitialized;
    public static bool Initialized
    {
        get
        {
            return Instance.m_bInitialized;
        }
    }


    private static SteamScript s_instance;
  
    private static SteamScript Instance
    {
        get
        {
            return s_instance ?? new GameObject("SteamManager").AddComponent<SteamScript>();
        }
    }

    private void Awake()
    {

        /*
         try
         {
             if (SteamAPI.RestartAppIfNecessary(())
             {
                 Application.Quit();
                 return;
             }
         }
         catch (System.DllNotFoundException e)
         {
             Debug.LogError("[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n" + e, this);

             Application.Quit();
             return;
         }*/

        m_bInitialized = SteamAPI.Init();
        if (!m_bInitialized)
        {
            Debug.LogError("[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.", this);

            return;
        }
    }



    private void OnEnable()
    {
        if (SteamManager.Initialized)
        {
            m_GameOverlayActivated = Callback<GameOverlayActivated_t>.Create(OnGameOverlayActivated);
            m_NumberOfCurrentPlayers = CallResult<NumberOfCurrentPlayers_t>.Create(OnNumeOnNumberOfCurrentPlayers);
        }

        //By calling SteamClient.SetWarningMessageHook() with a function delegate we can intercept warning messages from Steam under certain situations.
        // Note that we ensure that the Steam API is initialized before calling any Steamworks functions.
        //We call this in OnEnable so that it gets recreated after Unity does an Assembly Reload such as when recompiling scripts.
        if (!m_bInitialized)
        {
            return;
        }

        if (m_SteamAPIWarningMessageHook == null)
        {
            m_SteamAPIWarningMessageHook = new SteamAPIWarningMessageHook_t(SteamAPIDebugTextHook);
            SteamClient.SetWarningMessageHook(m_SteamAPIWarningMessageHook);
        }
    }
    void Start()
    {
        if (SteamManager.Initialized)
        {
            masterIDSteam = SteamFriends.GetPersonaName();
            Debug.Log(masterIDSteam);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SteamAPICall_t handle = SteamUserStats.GetNumberOfCurrentPlayers();
            m_NumberOfCurrentPlayers.Set(handle);
            Debug.Log("Called GetNumberOfCurrentPlayers()");

            GetListOfFriends();

            float Stats;
            Stats = GetStats("Skeleton") + 1;
            Debug.Log(Stats);
      //      SetStats("Skeleton", Stats, false);

            bool tutorialCompleted;
            SteamUserStats.GetAchievement("ACH_25_Skeleton", out tutorialCompleted);
            tutorialCompleted = SteamUserStats.GetAchievement("ACH_25_Skeleton", out tutorialCompleted);

            Debug.Log(tutorialCompleted);
            Debug.Log("Achievement: " + SteamUserStats.GetAchievement("ACH_25_Skeleton", out Check_It));
            
            if (SteamManager.Initialized)
            {
                SteamUserStats.SetAchievement("ACH_100_Skeleton");
                SteamUserStats.StoreStats();
            }


        }

        if (!m_bInitialized)
        {
            return;
        }

        // Run Steam client callbacks
        SteamAPI.RunCallbacks();
    }

    private void OnGameOverlayActivated(GameOverlayActivated_t pCallback)
    {
        if (pCallback.m_bActive != 0)
        {
            Debug.Log("Steam Overlay has been activated");
        }
        else
        {
            Debug.Log("Steam Overlay has been closed");
        }
    }

    private void OnNumeOnNumberOfCurrentPlayers (NumberOfCurrentPlayers_t pCallback, bool bIOFailure)
    {
        if (pCallback.m_bSuccess != 1 || bIOFailure)
        {
            Debug.Log("There was an error retrieving the NumberOfCurrentPlayers.");
        }
        else
        {
            Debug.Log("The number of players playing your game: " + pCallback.m_cPlayers);
        }
    }

    private static void SteamAPIDebugTextHook(int nSeverity, System.Text.StringBuilder pchDebugText)
    {
        Debug.LogWarning(pchDebugText);
    }


    private void GetListOfFriends ()
    {
        int friendCount = SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagImmediate);
        Debug.Log("[STEAM-FRIENDS] Listing " + friendCount + " Friends.");
        for (int i = 0; i < friendCount; ++i)
        {
            CSteamID friendSteamId = SteamFriends.GetFriendByIndex(i, EFriendFlags.k_EFriendFlagImmediate);
            string friendName = SteamFriends.GetFriendPersonaName(friendSteamId);
            EPersonaState friendState = SteamFriends.GetFriendPersonaState(friendSteamId);

            Debug.Log(friendName + " is " + friendState + "/" + friendSteamId);
     


        }
    }

    private void OnDestroy()
    {
        if (!m_bInitialized)
        {
            return;
        }

        SteamAPI.Shutdown();
    }

    private void CheckStatus ()
    {
        bool idontknow = true;
        bool skeletonBool = SteamUserStats.GetAchievement("ACH_KILL_25_SKELETONS", out idontknow);
        Debug.Log(skeletonBool);


        int skeleton = 0;
        string skeletonString = "Skeleton";
        Debug.Log(Steamworks.SteamUserStats.GetStat(skeletonString, out skeleton));
        
    }


    static public float GetStats(string StatusName)
    {
        float Stats;
        SteamUserStats.GetStat(StatusName, out Stats);
        Debug.Log(Stats);
        return Stats;
    }

    static public void SetStats(string StatusName, float Value, bool toINT)
    {
        if (SteamManager.Initialized)
        {
            if (toINT)
            {
                //int intData = (int)Value;
                //SteamUserStats.SetStat (StatusName, intData);
            }
            Debug.Log("Setting stats of: " + StatusName + ", To: " + Value);
            SteamUserStats.SetStat(StatusName, Value);
            SteamUserStats.StoreStats();
        }
    }

}
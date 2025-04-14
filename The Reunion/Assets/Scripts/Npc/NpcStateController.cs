using UnityEngine;


// In any script, you can access and modify the flags like this:
// NPCStateManager.Instance.act1 = true;  // Door 1 is now accessible
// NPCStateManager.Instance.act2 = false; // Door 2 is no longer accessible


public class NPCStateManager : MonoBehaviour
{
    // Singleton instance for easy access
    public static NPCStateManager Instance;

    // Room access flags with property observers
    private bool _act1 = true;
    private bool _act2 = false;
    private bool _act3 = false;
    private bool _maxSuspicion = false;

    public bool act1
    {
        get => _act1;
        set
        {
            _act1 = value;
            UpdateNPCNavigation();
        }
    }

    public bool act2
    {
        get => _act2;
        set
        {
            _act2 = value;
            UpdateNPCNavigation();
        }
    }

    public bool act3
    {
        get => _act3;
        set
        {
            _act3 = value;
            UpdateNPCNavigation();
        }
    }

    public bool maxSuspicion
    {
        get => _maxSuspicion;
        private set
        {
            _maxSuspicion = value;
            OnSuspicionChanged?.Invoke(value);
        }
    }

    public event System.Action<bool> OnSuspicionChanged;
    private Vector3 lastPlayerPosition;


    void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
            InitializeFromGameState();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetMaxSuspicion(bool suspicious)
    {
        maxSuspicion = suspicious;
        if (!suspicious)
        {
            lastPlayerPosition = Vector3.zero;
        }
    }

    public void AlertAllNPCs(Vector3 playerPosition)
    {
        lastPlayerPosition = playerPosition;
        UpdateNPCNavigation();
    }

    private void InitializeFromGameState()
    {
        if (SuspicionManager.Instance != null)
        {
            int act = SuspicionManager.Instance.currentAct;
            act1 = act >= 1;
            act2 = act >= 2;
            act3 = act >= 3;
        }
    }

    private void UpdateNPCNavigation()
    {
        var npcs = FindObjectsByType<DELETE>(FindObjectsSortMode.None);
        foreach (var npc in npcs)
        {
            if (npc.isActiveAndEnabled)
            {
                npc.HandleStateUpdate(maxSuspicion, lastPlayerPosition);
            }
        }
    }

    private Transform _playerTransform;
    public Transform PlayerTransform
    {
        get
        {
            if (_playerTransform == null)
            {
                FindPlayer();
            }
            return _playerTransform;
        }
    }

    void FindPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            _playerTransform = playerObj.transform;
            Debug.Log("Found player in scene");
        }
        else
        {
            Debug.LogWarning("Player not found in scene!");
        }
    }

    public void AlertAllNPCs()
    {
        if (PlayerTransform != null)
        {
            lastPlayerPosition = PlayerTransform.position;
            UpdateNPCNavigation();
        }
    }
}
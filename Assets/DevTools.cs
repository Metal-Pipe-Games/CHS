using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DevTools : MonoBehaviour
{
    public ChallangeModeController controller;
    public PlayerHealthController healthController;
    public ToolHandler handler;

    public InputActionAsset actions;
    private InputActionMap actionMap;

    public bool active = false;

    CanvasGroup group;
    public CursorLocker locker;

    void OnEnable()
    {
        actions.FindActionMap("Player").Enable();
    }
    void OnDisable()
    {
        actions.FindActionMap("Player").Disable();
    }

    private void Awake()
    {
        actionMap = actions.FindActionMap("Player");

        actionMap.FindAction("ToggleDevTools").performed += Toggle;
    }
    // Start is called before the first frame update
    void Start()
    {
        group = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Toggle(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (active)
            {
                group.alpha = 0;
                group.interactable =
                    group.blocksRaycasts =
                    active = false;
                locker.CursorLocked = true;
            }
            else
            {
                group.alpha = 1;
                group.interactable =
                    group.blocksRaycasts =
                    active = true;
                locker.CursorLocked = false;
            }
        }
    }

    public string Health
    {
        set {
            try
            {
                healthController.health = float.Parse(value);
            }
            catch { };
        }
    }
    public string Ammo
    {
        set
        {
            try
            {
                var g = handler.tools[Tools.Gun] as Gun;
                g.TotalAmmo = int.Parse(value);
            }
            catch { }
        }
    }
    public string EnemyMult
    {
        set
        {
            try
            {
                controller.strongerEnemyMultiplier = float.Parse(value);
            }
            catch { }
        }
    }
    public string Delay
    {
        set
        {
            try
            {
                controller.spawnDelay = float.Parse(value);
            }
            catch { }
        }
    }
}
public class DevToolsData
{
    public float spawnDelay;
    public float strongerEnemyMult;
}
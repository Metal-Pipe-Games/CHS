using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ToolHandler : MonoBehaviour
{
    private Animator animator;
    public Dictionary<Tools, ITool> tools = new();
    public Tools activeTool = Tools.Gun;
    public int activeAttack = 0;
    public int finishedAttack = 0;
    public CanvasGroup AmmoHudGroup;
    public TMP_Text TotalAmmoText;
    public TMP_Text AmmoText;
    public bool usingTool = false;

    public InputActionAsset actions;
    private InputActionMap actionMap;

    public bool active = true;

    private void Awake()
    {
        tools[Tools.Gun] = new Gun();
        tools[Tools.Knife] = new Knife();

        actionMap = actions.FindActionMap("Player");

        actionMap.FindAction("Use").performed += OnUse;
        actionMap.FindAction("Reload").performed += OnReload;
    }
    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        ITool.animator = animator;
        ITool.handler = this;
    }
    private void Update()
    {
        if (active)
        {
            for(int i = 0; i < tools.Count; i++)
            {
                if (actionMap.FindAction("Tool" + (i+1)).WasPerformedThisFrame())
                {
                    animator.SetInteger("Tool", i);
                    activeTool = (Tools)i;
                    AmmoHudGroup.alpha = tools[activeTool].AmmoHud ? 1 : 0;
                    break;
                }
            }
            if (tools[activeTool].canAim)
            {
                var aim = actionMap.FindAction("Aim");
                if (aim.WasPressedThisFrame() && aim.IsPressed())
                {
                    tools[activeTool].Aim();
                }
                else if(aim.WasReleasedThisFrame())
                {
                    tools[activeTool].EndAim();
                }
            }
        }
    }

    private void OnUse(InputAction.CallbackContext context)
    {
        if (active && context.performed)
        {
            tools[activeTool].Attack();
            usingTool = true;
        }
    }

    public void OnUseEnd()
    {
        usingTool = false;
    }

    private void OnReload(InputAction.CallbackContext context)
    {
        if (active && context.performed) tools[activeTool].Reload();
    }

    public void Timeout(int v)
    {
        animator.SetBool("Timeout", v != 0);
    }

    public void FinishedAttack(int v)
    {
        tools[activeTool].finishedAttack = v;
    }

    public void AddAmmo(int v)
    {
        int h = Mathf.FloorToInt(v * 0.5f);
        tools[activeTool].AddAmmo(Random.Range(v-h,v+h));
    }

    public void EndReload()
    {
        tools[activeTool].EndReload();
    }

    void OnEnable()
    {
        actions.FindActionMap("Player").Enable();
    }
    void OnDisable()
    {
        actions.FindActionMap("Player").Disable();
    }
}
public enum Tools
{
    Gun,
    Knife
}
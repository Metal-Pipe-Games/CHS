using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public MenuTypes active = MenuTypes.Main;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Return()
    {

    }
}
public enum MenuTypes
{
    Main, // Always fallback
    Settings,
    Extras
}
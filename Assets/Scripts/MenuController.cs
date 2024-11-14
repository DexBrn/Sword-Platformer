using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Transform MenuPanel;
    Image MenuPanelMain;
    public Transform AbilityPanel;
    string AbilityToSwitch; 

    // Start is called before the first frame update
    void Start()
    {
        MenuPanelMain = MenuPanel.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape) && MenuPanelMain.enabled == false)
        {
            MenuPanel.gameObject.SetActive(true);
            MenuPanelMain.enabled = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && MenuPanelMain.enabled == true)
        {
            MenuPanel.gameObject.SetActive(false);
            MenuPanelMain.enabled = false;
            Cursor.lockState = CursorLockMode.Locked;
        }


        



    }

    public void AbilityPanelState()
    {
        if (AbilityPanel.gameObject.active == false) 
        { 
            AbilityPanel.gameObject.SetActive(true);

        
        }
        else
        {
            AbilityPanel.gameObject.SetActive(false);
            AbilityToSwitch = AbilityPanel.GetChild(0).GetComponent<TextMeshPro>().text;
            MenuPanel.GetChild(2).GetComponent<TextMeshPro>().text = AbilityToSwitch;
        }
    }









}

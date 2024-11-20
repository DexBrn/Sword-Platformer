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
    Transform MenuAbilityNum;

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
            if (AbilityPanel.gameObject.active == true)
            {
                AbilityPanel.gameObject.SetActive(false);
                

            }
        }


        



    }

    public void AbilityPanelState(Transform ThisButton)
    {
        if (AbilityPanel.gameObject.active == false) 
        { 
            AbilityPanel.gameObject.SetActive(true);
            MenuAbilityNum = ThisButton;
        
        }
        else
        {
            AbilityPanel.gameObject.SetActive(false);
            AbilityToSwitch = ThisButton.GetChild(0).GetComponent<TMP_Text>().text;
            MenuAbilityNum.GetChild(0).GetComponent<TMP_Text>().text = AbilityToSwitch;
            
            
        }
    }



    public void CloseGame()
    {
        Application.Quit();
    }





}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Transform MenuPanel;
    Image MenuPanelMain;
    public Transform AbilityPanel;
    string AbilityToSwitch;
    Transform MenuAbilityNum;
    Sprite IconToSwitch;




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
            AbilityToSwitch = ThisButton.GetComponentInChildren<TMP_Text>().text;
            IconToSwitch = ThisButton.GetComponent<Image>().sprite;
            MenuAbilityNum.GetComponentInChildren<TMP_Text>().text = AbilityToSwitch;
            MenuAbilityNum.GetComponent<Image>().sprite = IconToSwitch;

        }
    }



    public void CloseGame()
    {
        Application.Quit();
    }


    public void TutorialTeleport()
    {
        transform.position = new Vector3 (-7031.598f, 32.203f, -86.91359f);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene("Main Scene");
    }




}

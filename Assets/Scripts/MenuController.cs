using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Transform MenuPanel;
    Image MenuPanelMain;

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
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && MenuPanelMain.enabled == true)
        {
            MenuPanel.gameObject.SetActive(false);
            MenuPanelMain.enabled = false;
        }


    }
}

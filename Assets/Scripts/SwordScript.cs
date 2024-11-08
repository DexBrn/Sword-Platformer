using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SwordScript : MonoBehaviour
{
    Rigidbody rb;

    [Header("Sword Dash")]
    public float DashFowardPower;
    public float DashUpPower;
    bool CanDash = true;
    public float DashCoolDown;
    private float MaxDashCoolDown;
    public TMP_Text DashCoolDownText;
    public float DashCharge = 1;
    public Outline BladeOutline;
    public Outline HandleOutline;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        MaxDashCoolDown = DashCoolDown;
    }

    // Update is called once per frame
    void Update()
    {

        if (CanDash == false)
        {
            DashCoolDown -= Time.deltaTime;
            DashCoolDownText.text = ("Sword Dash: " + Mathf.Round(DashCoolDown * 1000) / 1000);
            if (DashCoolDown < 0)
            {
                CanDash = true;
                DashCoolDown = 0;
                DashCoolDownText.text = ("Sword Dash: Ready");
            }
        }

        if (Input.GetKey(KeyCode.Q))
        {
            Color ChargeOneColour;
            Color ChargeTwoColour;
            Color MaxChargeColour;
            ColorUtility.TryParseHtmlString("#FFFA00", out ChargeOneColour);
            ColorUtility.TryParseHtmlString("#FFD500", out ChargeTwoColour);
            ColorUtility.TryParseHtmlString("#FF9600", out MaxChargeColour);
            DashCharge += Time.deltaTime;
            
            
            BladeOutline.OutlineWidth += DashCharge / 100;
            HandleOutline.OutlineWidth += DashCharge / 100;

            if (DashCharge > 1)
            {
                BladeOutline.OutlineColor = ChargeOneColour;
                HandleOutline.OutlineColor = ChargeOneColour;
                //BladeOutline.OutlineColor = new Color(DashCharge * 50, 05, 0);
                //HandleOutline.OutlineColor = new Color(DashCharge * 50, 05, 0);
            }

            if (DashCharge > 2)
            {
                BladeOutline.OutlineColor = ChargeTwoColour;
                HandleOutline.OutlineColor = ChargeTwoColour;
                
            }

            if (DashCharge > 3)
            {
                
                BladeOutline.OutlineColor = MaxChargeColour;
                HandleOutline.OutlineColor = MaxChargeColour;
                DashCharge = 3;
                BladeOutline.OutlineWidth = 12;
                HandleOutline.OutlineWidth = 12;
            }
        }

        if (Input.GetKeyUp(KeyCode.Q) && CanDash)
        {
            rb.AddForce(((transform.forward * DashFowardPower) + (transform.up * DashUpPower)) * (DashCharge / 1.5f), ForceMode.Impulse);
            CanDash = false;
            DashCoolDown = MaxDashCoolDown;
            DashCharge = 1;
            BladeOutline.OutlineColor = new Color(0, 255, 250);
            HandleOutline.OutlineColor = new Color(0, 255, 250);
            BladeOutline.OutlineWidth = 4;
            HandleOutline.OutlineWidth = 4;
        }

        

    }
}

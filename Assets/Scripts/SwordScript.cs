using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//using Unity.VisualScripting;

public class SwordScript : MonoBehaviour
{
    Rigidbody rb;

    [Header("Basic Attack")]
    public Transform BasicHitbox;
    public float AttackDuration;
    float MaxAttackDuration;
    public float AttackCooldown;
    float MaxAttackCooldown;
    bool CanAttack = true;
    bool IsSwinging = false;


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
        MaxAttackCooldown = AttackCooldown;
        MaxAttackDuration = AttackDuration;
    }

    // Update is called once per frame
    void Update()
    {

        /////// Basic Attack ///////
        ///

        if (!CanAttack)
        {
            AttackCooldown -= Time.deltaTime;
            if (AttackCooldown < 0)
            {
                CanAttack = true;
                AttackCooldown = MaxAttackCooldown;
            }


        }

        if (IsSwinging)
        {
            AttackDuration -= Time.deltaTime;
            if (AttackDuration < 0)
            {
                IsSwinging = false;
                BasicHitbox.GetComponent<Collider>().enabled = false;
                AttackDuration = MaxAttackDuration;
            }
        }


        if (Input.GetMouseButtonDown(0) && CanAttack && !IsSwinging)
        {
            BasicHitbox.GetComponent<Collider>().enabled = true;
            IsSwinging = true;
            CanAttack = false;
        }

        



        /////// Dash Logic ///////

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
            
            
            BladeOutline.OutlineWidth += Time.deltaTime * 3;
            HandleOutline.OutlineWidth += Time.deltaTime * 3;

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
            BasicHitbox.GetComponent<Collider>().enabled = true;
            IsSwinging = true;
            DashCoolDown = MaxDashCoolDown;
            DashCharge = 1;
            BladeOutline.OutlineColor = new Color(0, 255, 250);
            HandleOutline.OutlineColor = new Color(0, 255, 250);
            BladeOutline.OutlineWidth = 4;
            HandleOutline.OutlineWidth = 4;
        }


        

    }

    


}

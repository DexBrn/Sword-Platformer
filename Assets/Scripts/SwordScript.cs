using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//using Unity.VisualScripting;

public class SwordScript : MonoBehaviour
{
    Rigidbody rb;
    public Transform Camera;

    public string[] Abilities;
    public Transform SelectedAbility1;
    public Transform SelectedAbility2;
    KeyCode SwordDashBind;
    KeyCode GroundSlamBind;
    KeyCode OmniDashBind;

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

    [Header("Omni Dash")]
    public float OmniDashPower;
    bool CanOmniDash = true;
    public float OmniDashCoolDown;
    private float MaxOmniDashCoolDown;
    public TMP_Text OmniDashCoolDownText;
    public float OmniDashCharge = 1;
    //public Outline BladeOutline;
    //public Outline HandleOutline;

    [Header("Ground Slam")]
    public float GroundSlamForce;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        MaxDashCoolDown = DashCoolDown;
        MaxAttackCooldown = AttackCooldown;
        MaxAttackDuration = AttackDuration;
        MaxOmniDashCoolDown = OmniDashCoolDown;
    }

    // Update is called once per frame
    void Update()
    {

        ////////////////////////////// Basic Attack //////////////////////////////
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





        ////////////////////////////////// Dash Logic ////////////////////////////////

        if (SelectedAbility1.GetChild(0).GetComponent<TMP_Text>().text == "Sword Dash")
        {
            SwordDashBind = KeyCode.Q;
        }
        else if (SelectedAbility2.GetChild(0).GetComponent<TMP_Text>().text == "Sword Dash")
        {
            SwordDashBind = KeyCode.E;
        }
        else
        {
            SwordDashBind = KeyCode.None;
        }

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

        if (Input.GetKey(SwordDashBind))
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

        if (Input.GetKeyUp(SwordDashBind) && CanDash)
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





        /////////////////////////////////////// Omni Dash Logic //////////////////////////

        if (SelectedAbility1.GetChild(0).GetComponent<TMP_Text>().text == "Omni Dash")
        {
            OmniDashBind = KeyCode.Q;
        }
        else if (SelectedAbility2.GetChild(0).GetComponent<TMP_Text>().text == "Omni Dash")
        {
            OmniDashBind = KeyCode.E;
        }
        else
        {
            OmniDashBind = KeyCode.None;
        }

        if (CanOmniDash == false)
        {
            OmniDashCoolDown -= Time.deltaTime;
            OmniDashCoolDownText.text = ("Omni Dash: " + Mathf.Round(OmniDashCoolDown * 1000) / 1000);
            if (OmniDashCoolDown < 0)
            {
                CanOmniDash = true;
                OmniDashCoolDown = 0;
                OmniDashCoolDownText.text = ("Omni Dash: Ready");
            }
        }

        if (Input.GetKey(OmniDashBind))
        {
            Color ChargeOneColour;
            Color ChargeTwoColour;
            Color MaxChargeColour;
            ColorUtility.TryParseHtmlString("#FFFA00", out ChargeOneColour);
            ColorUtility.TryParseHtmlString("#FFD500", out ChargeTwoColour);
            ColorUtility.TryParseHtmlString("#FF9600", out MaxChargeColour);
            OmniDashCharge += Time.deltaTime;


            BladeOutline.OutlineWidth += Time.deltaTime * 3;
            HandleOutline.OutlineWidth += Time.deltaTime * 3;

            if (OmniDashCharge > 1)
            {
                BladeOutline.OutlineColor = ChargeOneColour;
                HandleOutline.OutlineColor = ChargeOneColour;
                //BladeOutline.OutlineColor = new Color(DashCharge * 50, 05, 0);
                //HandleOutline.OutlineColor = new Color(DashCharge * 50, 05, 0);
            }

            if (OmniDashCharge > 2)
            {
                BladeOutline.OutlineColor = ChargeTwoColour;
                HandleOutline.OutlineColor = ChargeTwoColour;

            }

            if (OmniDashCharge > 3)
            {

                BladeOutline.OutlineColor = MaxChargeColour;
                HandleOutline.OutlineColor = MaxChargeColour;
                OmniDashCharge = 3;
                BladeOutline.OutlineWidth = 12;
                HandleOutline.OutlineWidth = 12;
            }
        }

        if (Input.GetKeyUp(OmniDashBind) && CanOmniDash)
        {
            rb.AddForce(((Camera.forward * OmniDashPower) + (transform.up * 0)) * (DashCharge / 1.5f), ForceMode.Impulse);
            CanOmniDash = false;
            BasicHitbox.GetComponent<Collider>().enabled = true;
            IsSwinging = true;
            OmniDashCoolDown = MaxOmniDashCoolDown;
            OmniDashCharge = 1;
            BladeOutline.OutlineColor = new Color(0, 255, 250);
            HandleOutline.OutlineColor = new Color(0, 255, 250);
            BladeOutline.OutlineWidth = 4;
            HandleOutline.OutlineWidth = 4;
        }





        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /*
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartAbility1();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartAbility2();
        }

        */


    }

    private void FixedUpdate()
    {

        ///////// Ground Slam //////////////

        if (SelectedAbility1.GetChild(0).GetComponent<TMP_Text>().text == "Ground Slam")
        {
            GroundSlamBind = KeyCode.Q;
        }
        else if (SelectedAbility2.GetChild(0).GetComponent<TMP_Text>().text == "Ground Slam")
        {
            GroundSlamBind = KeyCode.E;
        }
        else
        {
            GroundSlamBind = KeyCode.None;
        }


        if (Input.GetKey(GroundSlamBind))
        {
            rb.AddForce(-transform.up * GroundSlamForce, ForceMode.Impulse);
        }
    }


    /*
    void StartAbility1()
    {
       
        for (int i = 0; i < 8; i++) 
        {
            if (SelectedAbility1.GetChild(0).GetComponent<TMP_Text>().text == Abilities[i])
            {
                print(Abilities[i]);

            }
        }
    }
    void StartAbility2() 
    {
        for (int i = 0; i < 8; i++)
        {
            if (SelectedAbility2.GetChild(0).GetComponent<TMP_Text>().text == Abilities[i])
            {
                print(Abilities[i]);
            }
        }
    }


    */

}

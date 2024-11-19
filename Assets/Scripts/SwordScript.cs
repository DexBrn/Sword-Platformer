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
    public TMP_Text Ability1Text;
    public float DashCharge = 1;
    public Outline BladeOutline;
    public Outline HandleOutline;
    KeyCode SwordDashBind;

    [Header("Omni Dash")]
    public float OmniDashPower;
    bool CanOmniDash = true;
    public float OmniDashCoolDown;
    private float MaxOmniDashCoolDown;
    public TMP_Text Ability2Text;
    public float OmniDashCharge = 1;
    KeyCode OmniDashBind;

    [Header("Multi Dash")]
    public float MultiDashPower;
    bool CanMultiDash = true;
    public float MultiDashCoolDown;
    private float MaxMultiDashCoolDown;
    public float MultiDashCharge = 1;
    public float MultiAirTime;
    float MaxMultiAirTime;
    bool MultiFloat = false;
    bool ExtraMultiFloat = false;
    float MiniMultiDashCoolDown = 1;
    KeyCode MultiDashBind;

    [Header("Ground Slam")]
    public float GroundSlamForce;
    KeyCode GroundSlamBind;

    [Header("Double Jump")]
    KeyCode DoubleJumpBind;
    KeyCode AltDoubleJumpBind;
    public bool CanDoubleJump = false;
    PlayerMovement PlayerMovement;

    [Header("Enemy Leap")]
    KeyCode EnemyLeapBind;
    public float InitialDashTime;
    public float LeapForce;
    Vector3 LeapStartPos;
    public float LeapRange;
    public LayerMask EnemyLayer;
    float ElapsedTime;
    bool IsLeaping;
    Vector3 EnemyPos;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        MaxDashCoolDown = DashCoolDown;
        MaxAttackCooldown = AttackCooldown;
        MaxAttackDuration = AttackDuration;
        MaxOmniDashCoolDown = OmniDashCoolDown;
        MaxMultiDashCoolDown = MultiDashCoolDown;
        MaxMultiAirTime = MultiAirTime;
        PlayerMovement = transform.GetComponent<PlayerMovement>();
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
            Ability1Text.text = ("Sword Dash: " + Mathf.Round(DashCoolDown * 1000) / 1000);
            if (DashCoolDown < 0)
            {
                CanDash = true;
                DashCoolDown = 0;
                Ability1Text.text = ("Sword Dash: Ready");
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
            Ability2Text.text = ("Omni Dash: " + Mathf.Round(OmniDashCoolDown * 1000) / 1000);
            if (OmniDashCoolDown < 0)
            {
                CanOmniDash = true;
                OmniDashCoolDown = 0;
                Ability2Text.text = ("Omni Dash: Ready");
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




        /////////////////////////////////////// Multi Dash Logic //////////////////////////

        if (SelectedAbility1.GetChild(0).GetComponent<TMP_Text>().text == "Multi Dash")
        {
            MultiDashBind = KeyCode.Q;
        }
        else if (SelectedAbility2.GetChild(0).GetComponent<TMP_Text>().text == "Multi Dash")
        {
            MultiDashBind = KeyCode.E;
        }
        else
        {
            MultiDashBind = KeyCode.None;
        }

        

        if (MultiFloat == true)
        {
            
            rb.useGravity = false;
            //rb.velocity = Vector3.zero;
            MiniMultiDashCoolDown -= Time.deltaTime;
            if (MiniMultiDashCoolDown > 0)
                ExtraMultiFloat = true;
            MultiAirTime -= Time.deltaTime;
            
            if (MultiAirTime < 0)
            {
                MultiFloat = false;
                ExtraMultiFloat = false;
                rb.useGravity = true;
                CanMultiDash = false;
            }
        }

        if (CanMultiDash == false)
        {
            MultiDashCoolDown -= Time.deltaTime;
            Ability2Text.text = ("Multi Dash: " + Mathf.Round(MultiDashCoolDown * 1000) / 1000);
            if (MultiDashCoolDown < 0)
            {
                MultiFloat = false;
                CanMultiDash = true;
                MultiDashCoolDown = 0;
                Ability2Text.text = ("Multi Dash: Ready");
            }
        }

        if (Input.GetKey(MultiDashBind))
        {
            Color ChargeOneColour;
            Color ChargeTwoColour;
            Color MaxChargeColour;
            ColorUtility.TryParseHtmlString("#FFFA00", out ChargeOneColour);
            ColorUtility.TryParseHtmlString("#FFD500", out ChargeTwoColour);
            ColorUtility.TryParseHtmlString("#FF9600", out MaxChargeColour);
            MultiDashCharge += Time.deltaTime;


            BladeOutline.OutlineWidth += Time.deltaTime * 3;
            HandleOutline.OutlineWidth += Time.deltaTime * 3;

            if (MultiDashCharge > 1)
            {
                BladeOutline.OutlineColor = ChargeOneColour;
                HandleOutline.OutlineColor = ChargeOneColour;

            }

            if (MultiDashCharge > 2)
            {
                BladeOutline.OutlineColor = ChargeTwoColour;
                HandleOutline.OutlineColor = ChargeTwoColour;

            }

            if (MultiDashCharge > 3)
            {

                BladeOutline.OutlineColor = MaxChargeColour;
                HandleOutline.OutlineColor = MaxChargeColour;
                MultiDashCharge = 3;
                BladeOutline.OutlineWidth = 12;
                HandleOutline.OutlineWidth = 12;
            }
        }

        if (Input.GetKeyUp(MultiDashBind) && CanMultiDash && !MultiFloat)
        {
            rb.AddForce(((Camera.forward * MultiDashPower) + (transform.up * 0)) * (DashCharge / 1.5f), ForceMode.Impulse);
            
            
            BasicHitbox.GetComponent<Collider>().enabled = true;
            IsSwinging = true;
            MultiAirTime = MaxMultiAirTime;
            MultiDashCharge = 1;
            BladeOutline.OutlineColor = new Color(0, 255, 250);
            HandleOutline.OutlineColor = new Color(0, 255, 250);
            BladeOutline.OutlineWidth = 4;
            HandleOutline.OutlineWidth = 4;
            MultiFloat = true;
            MultiAirTime = MaxMultiAirTime;

        }

        if (Input.GetKeyDown(MultiDashBind) && CanMultiDash && MultiFloat)
        {

            
            rb.AddForce(((Camera.forward * MultiDashPower) + (transform.up * 0)) * (DashCharge / 1.5f), ForceMode.Impulse);
            CanMultiDash = false;
            BasicHitbox.GetComponent<Collider>().enabled = true;
            IsSwinging = true;
            MultiDashCoolDown = MaxMultiDashCoolDown;
            MultiDashCharge = 1;
            BladeOutline.OutlineColor = new Color(0, 255, 250);
            HandleOutline.OutlineColor = new Color(0, 255, 250);
            BladeOutline.OutlineWidth = 4;
            HandleOutline.OutlineWidth = 4;
            rb.useGravity = true;
            MultiFloat = false;

        }



        /////////////////////////////////////// Double Jump Logic //////////////////////////

        if (SelectedAbility1.GetChild(0).GetComponent<TMP_Text>().text == "Double Jump")
        {
            DoubleJumpBind = KeyCode.Q;
            AltDoubleJumpBind = KeyCode.Space;
        }
        else if (SelectedAbility2.GetChild(0).GetComponent<TMP_Text>().text == "Double Jump")
        {
            DoubleJumpBind = KeyCode.E;
            AltDoubleJumpBind = KeyCode.Space;
           
        }
        else
        {
            DoubleJumpBind = KeyCode.None;
            AltDoubleJumpBind = KeyCode.None;
        }


        if (!PlayerMovement.IsGrounded() && CanDoubleJump && (Input.GetKeyDown(AltDoubleJumpBind)))
        {
            float JumpPower = PlayerMovement.JumpPower;
            rb.AddForce(transform.up * JumpPower, ForceMode.Impulse);
            CanDoubleJump = false;
        }

        if (!PlayerMovement.IsGrounded() && CanDoubleJump && Input.GetKeyDown(DoubleJumpBind))
        {
            float JumpPower = PlayerMovement.JumpPower;
            rb.AddForce(transform.up * JumpPower, ForceMode.Impulse);
            CanDoubleJump = false;
        }

        /////////////////////////////////////// Enemy Leap Logic //////////////////////////

        if (SelectedAbility1.GetChild(0).GetComponent<TMP_Text>().text == "Enemy Leap")
        {
            EnemyLeapBind = KeyCode.Q;
        }
        else if (SelectedAbility2.GetChild(0).GetComponent<TMP_Text>().text == "Enemy Leap")
        {
            EnemyLeapBind = KeyCode.E;
        }
        else
        {
            EnemyLeapBind = KeyCode.None;
        }

        


        if (Input.GetKeyDown(EnemyLeapBind) && EnemyInRange())
        {
            GetCurrentPosition();
            IsLeaping = true;
            


        }

        Collider[] LeapCollider = Physics.OverlapSphere(transform.position, LeapRange);
        for (int i = 0, n = LeapCollider.Length; i < n; i++)
        {

            if (LeapCollider[i].gameObject.layer == EnemyLayer)
                print(LeapCollider[i].transform.position);
            EnemyPos = LeapCollider[i].transform.position;
            print(EnemyPos);

        }

        if (IsLeaping)
        {
            ElapsedTime += Time.deltaTime;
            float PercentComplete = ElapsedTime / InitialDashTime;
            
            
            //print(EnemyPos);
            //print(LeapStartPos);
            transform.position = Vector3.Lerp(LeapStartPos, EnemyPos, PercentComplete);
            
        }
        if (ElapsedTime > InitialDashTime)
        {
            IsLeaping = false;
            ElapsedTime = 0;
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


    private void GetCurrentPosition()
    {
        LeapStartPos = transform.position; 
    }

    public bool EnemyInRange()
    {
        return Physics.CheckSphere(transform.position, LeapRange, EnemyLayer);
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

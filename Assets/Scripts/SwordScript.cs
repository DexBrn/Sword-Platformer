using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Audio;


public class SwordScript : MonoBehaviour
{
    Rigidbody rb;
    public Transform Camera;

    public string[] Abilities;
    public Transform SelectedAbility1;
    public Transform SelectedAbility2;
    public TMP_Text Ability1Text;
    public TMP_Text Ability2Text;


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
    public float DashCharge = 1;
    public Outline BladeOutline;
    public Outline HandleOutline;
    KeyCode SwordDashBind;
    TMP_Text SwordDashText;


    [Header("Omni Dash")]
    public float OmniDashPower;
    bool CanOmniDash = true;
    public float OmniDashCoolDown;
    private float MaxOmniDashCoolDown;
    public float OmniDashCharge = 1;
    KeyCode OmniDashBind;
    TMP_Text OmniDashText;

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
    TMP_Text MultiDashText;

    [Header("Ground Slam")]
    public float GroundSlamForce;
    KeyCode GroundSlamBind;

    [Header("Double Jump")]
    KeyCode DoubleJumpBind;
    KeyCode AltDoubleJumpBind;
    public bool CanDoubleJump = false;
    PlayerMovement PlayerMovement;


    [Header("Enemy Leap")]
    public float InitialDashTime;
    KeyCode EnemyLeapBind;
    public float LeapForce;
    public float LeapUpForce;
    Vector3 LeapStartPos;
    public float LeapRange;
    float ElapsedTime;
    bool IsLeaping;
    Vector3 EnemyPos;
    GameObject[] EnemyList;
    public LayerMask EnemyLayer;


    [Header("Grapple")]
    public float MaxGrappleDistance;
    private Vector3 GrapplePoint;
    LineRenderer LineRenderer;
    KeyCode GrappleBind;
    public Transform SwordTip;
    public Transform KatanaHolder;

    [Header("Summon Swords")]
    public Transform SummonSwordPrefab;
    KeyCode SummonSwordBind;
    bool SwordFiring = false;
    public float InitialSwordFireTime;
    float SummonSwordElapsedTime;
    GameObject[] SwordList;
    Vector3 EnemyPosS;
    float PercentCompleteSword;

    [Header("Audio")]
    public AudioClip SwordSwing;
    public AudioClip SwordHit;
    public AudioClip SwordDash;
    public AudioClip GroundSlam;
    public AudioClip SwordCharge;
    public AudioClip DoubleJump;
    public AudioClip SummonSword;
    public AudioClip EnemyLeap;
    AudioSource audioSource;


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
        EnemyList = GameObject.FindGameObjectsWithTag("Enemy");
        SwordList = GameObject.FindGameObjectsWithTag("Enemy");
        LineRenderer = GetComponent<LineRenderer>();
        audioSource = GetComponent<AudioSource>();
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
            if (ShouldHit())
                audioSource.PlayOneShot(SwordHit, 0.5f);
            else
                audioSource.PlayOneShot(SwordSwing);
            BasicHitbox.GetComponent<Collider>().enabled = true;
            IsSwinging = true;
            CanAttack = false;
        }





        ////////////////////////////////// Dash Logic ////////////////////////////////

        if (SelectedAbility1.GetChild(0).GetComponent<TMP_Text>().text == "Sword Dash")
        {
            SwordDashBind = KeyCode.Q;
            SwordDashText = Ability1Text;
        }
        else if (SelectedAbility2.GetChild(0).GetComponent<TMP_Text>().text == "Sword Dash")
        {
            SwordDashBind = KeyCode.E;
            SwordDashText = Ability2Text;
        }
        else
        {
            SwordDashBind = KeyCode.None;
            SwordDashText = null;
        }

        if (SwordDashText != null && DashCoolDown <= 0)
            SwordDashText.text = ("Sword Dash: Ready");

        if (CanDash == false)
        {
            DashCoolDown -= Time.deltaTime;
            if (SwordDashText != null)
                SwordDashText.text = ("Sword Dash: " + Mathf.Round(DashCoolDown * 1000) / 1000);
            if (DashCoolDown < 0)
            {
                CanDash = true;
                DashCoolDown = 0;
                SwordDashText.text = ("Sword Dash: Ready");
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
            audioSource.PlayOneShot(SwordCharge, 0.05f);


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
            audioSource.Stop();
            audioSource.PlayOneShot(SwordDash, 3.5f * (DashCharge / 3.5f));
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
            OmniDashText = Ability1Text;
        }
        else if (SelectedAbility2.GetChild(0).GetComponent<TMP_Text>().text == "Omni Dash")
        {
            OmniDashBind = KeyCode.E;
            OmniDashText = Ability2Text;
        }
        else
        {
            OmniDashBind = KeyCode.None;
            OmniDashText = null;
        }

        if (OmniDashText != null && OmniDashCoolDown <= 0)
            OmniDashText.text = ("Omni Dash: Ready");

        if (CanOmniDash == false)
        {
            OmniDashCoolDown -= Time.deltaTime;
            if (OmniDashText != null)
                OmniDashText.text = ("Omni Dash: " + Mathf.Round(OmniDashCoolDown * 1000) / 1000);
            if (OmniDashCoolDown < 0)
            {
                CanOmniDash = true;
                OmniDashCoolDown = 0;
                OmniDashText.text = ("Omni Dash: Ready");
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
            audioSource.PlayOneShot(SwordCharge, 0.05f);

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
            audioSource.Stop();
            audioSource.PlayOneShot(SwordDash, 3.5f * (OmniDashCharge / 3.5f));
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
            MultiDashText = Ability1Text;
        }
        else if (SelectedAbility2.GetChild(0).GetComponent<TMP_Text>().text == "Multi Dash")
        {
            MultiDashBind = KeyCode.E;
            MultiDashText = Ability2Text;
        }
        else
        {
            MultiDashBind = KeyCode.None;
            MultiDashText = null;
        }

        if (MultiDashText != null && MultiDashCoolDown <= 0)
            MultiDashText.text = ("Multi Dash: Ready");

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
            if (MultiDashText != null)
                MultiDashText.text = ("Multi Dash: " + Mathf.Round(MultiDashCoolDown * 1000) / 1000);
            if (MultiDashCoolDown < 0)
            {
                MultiFloat = false;
                CanMultiDash = true;
                MultiDashCoolDown = 0;
                MultiDashText.text = ("Multi Dash: Ready");
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
            if (MultiDashCharge > 1.5f)
                audioSource.PlayOneShot(SwordCharge, 0.05f);
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
            audioSource.Stop(); 
            audioSource.PlayOneShot(SwordDash, 3.5f * (MultiDashCharge / 3.5f));

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


            rb.AddForce(((Camera.forward * MultiDashPower) + (transform.up * 0)) * (DashCharge / 1.5f) * 2, ForceMode.Impulse);
            audioSource.Stop();
            audioSource.Stop();
            audioSource.PlayOneShot(SwordDash, 3.5f * (MultiDashCharge / 3.5f));
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
            audioSource.PlayOneShot(DoubleJump, 1f);
        }

        if (!PlayerMovement.IsGrounded() && CanDoubleJump && Input.GetKeyDown(DoubleJumpBind))
        {
            float JumpPower = PlayerMovement.JumpPower;
            rb.AddForce(transform.up * JumpPower, ForceMode.Impulse);
            CanDoubleJump = false;
            audioSource.PlayOneShot(DoubleJump, 1f);
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
            audioSource.PlayOneShot(EnemyLeap, 0.15f);
            //audioSource.PlayOneShot(DoubleJump, 0.7f);

        }

        GameObject nearestEnemy = EnemyList[0];
        float DistanceToNearest = Vector3.Distance(transform.position, nearestEnemy.transform.position);

        for (int i = 1; i < EnemyList.Length; i++)
        {
            float DistanceToCurrent = Vector3.Distance(transform.position, EnemyList[i].transform.position);

            if (DistanceToCurrent < DistanceToNearest)
            {
                nearestEnemy = EnemyList[i];
                DistanceToNearest = DistanceToCurrent;
            }


        }

        EnemyPos = nearestEnemy.transform.position;

        if (IsLeaping)
        {
            ElapsedTime += Time.deltaTime;
            float PercentComplete = ElapsedTime / InitialDashTime;
            transform.position = Vector3.Lerp(LeapStartPos, EnemyPos, PercentComplete);
            BasicHitbox.GetComponent<Collider>().enabled = true;
            IsSwinging = true;

        }
        if (ElapsedTime > InitialDashTime)
        {
            IsLeaping = false;
            ElapsedTime = 0;
            rb.AddForce(((transform.forward * LeapForce) + (transform.up * LeapUpForce)), ForceMode.Impulse);

        }



        /////////////////////////////////////// Grapple Logic //////////////////////////

        if (SelectedAbility1.GetChild(0).GetComponent<TMP_Text>().text == "Grapple")
        {
            GrappleBind = KeyCode.Q;
        }
        else if (SelectedAbility2.GetChild(0).GetComponent<TMP_Text>().text == "Grapple")
        {
            GrappleBind = KeyCode.E;
        }
        else
        {
            GrappleBind = KeyCode.None;
        }



        if (Input.GetKeyDown(GrappleBind))
        {
            StartGrapple();
            audioSource.PlayOneShot(DoubleJump, 1f);
        }
        if (Input.GetKeyUp(GrappleBind))
            StopGrapple();



        /////////////////////////////////////// Summon Swords Logic //////////////////////////

        if (SelectedAbility1.GetChild(0).GetComponent<TMP_Text>().text == "Summon Swords")
        {
            SummonSwordBind = KeyCode.Q;
        }
        else if (SelectedAbility2.GetChild(0).GetComponent<TMP_Text>().text == "Summon Swords")
        {
            SummonSwordBind = KeyCode.E;
        }
        else
        {
            SummonSwordBind = KeyCode.None;
        }

        if (Input.GetKeyDown(SummonSwordBind) && EnemyInRange())
        {
            SpawnSwords();
            audioSource.PlayOneShot(SummonSword, 1f);
        }



        EnemyPos = nearestEnemy.transform.position;

        if (SwordFiring)
        {
            SummonSwordElapsedTime += Time.deltaTime;
            PercentCompleteSword = SummonSwordElapsedTime / InitialSwordFireTime;
            //print(SummonSwordElapsedTime);
            if (PercentCompleteSword >= 1) 
            {
                SwordFiring = false;
                SummonSwordElapsedTime = 0f;
                PercentCompleteSword = 0;
            }
        }

        if (SwordFiring)
        {
            

            for (int i = 1; i < 7; i++)
            {

                if (SwordFiring)
                {
                    
                    LeapStartPos = SwordList[i].transform.position;
                    SwordList[i].transform.LookAt(EnemyPosS);
                    SwordList[i].transform.position = Vector3.Lerp(LeapStartPos, EnemyPosS, PercentCompleteSword);
                    SwordList[i].GetComponentInChildren<Collider>().enabled = true;
                    
                }
                if (SummonSwordElapsedTime > InitialDashTime)
                {

                    ElapsedTime = 0;
                    Destroy(SwordList[i]);

                }


            }

            //SwordFiring = false;

        }







        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



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
            audioSource.PlayOneShot(GroundSlam, 0.4f);
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

    private void StartGrapple()
    {

        RaycastHit hit;
        if (Physics.Raycast(Camera.position, Camera.forward, out hit, MaxGrappleDistance))
        {
            GrapplePoint = hit.point;
            SpringJoint joint = transform.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = GrapplePoint;

            float DistanceFromPoint = Vector3.Distance(transform.position, GrapplePoint);

            joint.maxDistance = DistanceFromPoint * 0.8f;
            joint.minDistance = DistanceFromPoint * 0.25f;

            joint.spring = 60.5f;
            joint.damper = 3;
            joint.massScale = 10.5f;

            LineRenderer.positionCount = 2;

        }
    }

    private void LateUpdate()
    {

        DrawGrapple();

    }

    void DrawGrapple()
    {
        if (!transform.gameObject.GetComponent<SpringJoint>())
        {
            //KatanaHolder.localRotation =  Quaternion.Euler(0, -90, 0);
            KatanaHolder.localRotation = Quaternion.Lerp(KatanaHolder.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * 5);
            return;

        }


        LineRenderer.SetPosition(0, SwordTip.position);
        LineRenderer.SetPosition(1, GrapplePoint);
        KatanaHolder.LookAt(GrapplePoint);

    }

    private void StopGrapple()
    {
        LineRenderer.positionCount = 0;
        Destroy(transform.gameObject.GetComponent<SpringJoint>());
    }


    private void SpawnSwords()
    {
        //return;
        Vector3 SpawnSpot = new Vector3(1.4f, 1.1f, 0);
        for (int i = 1; i < 7; i++)
        {

            if (i < 4)
            {

                SpawnSpot.y = SpawnSpot.y - 0.4f;
                ///print(SpawnSpot.y);

            }
            if (i == 4)
                SpawnSpot.x = -SpawnSpot.x;
            //SpawnSpot.y = 0.7f;
            if (i >= 4)
            {
                if (SpawnSpot.y <= -0.09f)
                {
                    SpawnSpot.y = 1.1f;
                }

                SpawnSpot.y = SpawnSpot.y - 0.4f;
            }

            Transform SpawnedSword = Instantiate(SummonSwordPrefab, transform);
            SpawnedSword.localPosition = SpawnSpot;
            SpawnedSword.localRotation = Quaternion.Euler(0, -90, 0);


        }
        Array.Clear(SwordList, 0, SwordList.Length);
        SwordList = GameObject.FindGameObjectsWithTag("SummonSword");


        SwordFiring = true;
        SaveEnemyPos();


    }

    private void SaveEnemyPos()
    {
        EnemyPosS = EnemyPos;
    }

    bool ShouldHit()
    {
        return Physics.CheckSphere(BasicHitbox.position, 3f, EnemyLayer);
    }



}

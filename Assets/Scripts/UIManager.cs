using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update

    public TMP_Text magText;
    public Image reloadSlider;
    public Image vortexSlider;
    public GameObject rgun;
    public GameObject lgun;
    public Toggle isAutoToggle; // later change the codes to manage multiple weapons.

    private AttackManager rgunAtkScript, lgunAtkScript;
    private int maxMag;
    private int bulletIndex;

    void Start()
    {
        magText.text = "YEE";
        rgunAtkScript = rgun.GetComponent<AttackManager>();
        lgunAtkScript = lgun.GetComponent<AttackManager>();
        maxMag = bulletIndex = rgunAtkScript.maxMag;
    }

    // Update is called once per frame
    void Update()
    {
        rgunAtkScript.isAuto = lgunAtkScript.isAuto = isAutoToggle.isOn;
        maxMag = rgunAtkScript.maxMag;
        bulletIndex = rgunAtkScript.bulletIndex;
        // reloadSlider.fillAmount = 
        slideControl();
        magTextControl();
    }
    void slideControl()
    {
        switch (rgunAtkScript.gunstate)
        {
            case gunState.READY:
            case gunState.IS_VORTEXING:
                vortexSlider.fillAmount = rgunAtkScript.gunstate == gunState.READY || rgunAtkScript.vortexInterval == 0f ? 1 : rgunAtkScript.vortexTimer/rgunAtkScript.vortexInterval;
                reloadSlider.fillAmount = (float)(maxMag - bulletIndex) / maxMag;
                break;
            case gunState.IS_RELOADING:
                reloadSlider.fillAmount = rgunAtkScript.reloadInterval == 0f ? 1 : rgunAtkScript.reloadTimer / rgunAtkScript.reloadInterval;
                break;
        }
    }

    void magTextControl()
    {
        magText.text = (maxMag - bulletIndex).ToString() + "/" + (maxMag).ToString();
    }
}

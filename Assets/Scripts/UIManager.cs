using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update

    public TMP_Text magText;
    public Image reloadSlider;
    public Image vortexSlider;
    public GameObject rgun;
    public GameObject lgun;

    private AttackManager rgunAtkScript;
    private int maxMag;
    private int bulletIndex;

    void Start()
    {
        magText.text = "YEE";
        rgunAtkScript = rgun.GetComponent<AttackManager>();
        maxMag = bulletIndex = rgunAtkScript.maxMag;
    }

    // Update is called once per frame
    void Update()
    {
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
                vortexSlider.fillAmount = rgunAtkScript.gunstate == gunState.READY ? 1 : rgunAtkScript.vortexTimer/rgunAtkScript.vortexInterval;
                reloadSlider.fillAmount = (float)(maxMag - bulletIndex) / maxMag;
                break;
            case gunState.IS_RELOADING:
                reloadSlider.fillAmount = rgunAtkScript.reloadTimer / rgunAtkScript.reloadInterval;
                break;
        }
    }

    void magTextControl()
    {
        magText.text = (maxMag - bulletIndex).ToString() + "/" + (maxMag).ToString();
    }
}

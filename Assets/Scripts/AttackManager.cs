using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable] // ?
public enum gunState
{
    IDLE,   // just in case
    READY,  // firable
    IS_VORTEXING, // vortexing after fire
    IS_RELOADING, // reloading
};

public class AttackManager : MonoBehaviour
{
    [Header("Ingame")] // 'Header'. visualize nyum. // in-play editing
    public float fps;

    [Header("Outgame")]

    public gunState gunstate;
    public GameObject bullet, bulletPool, player; // bulletPool을 구별!
    public AudioClip fireSound, dryFireSound, reloadSound;
    public AudioClip[] shellHitSounds;
    public float bulletSpeed = 40;
    public float reloadInterval = 1f;

    // bulletCount is the maximum value of the maxMag.
    // maxMag is the maximum number of the firable bullets in this reloaded phase.
    public int bulletCount = 24;
    public int maxMag = 6;

    public bool isAuto = false;
    public float vortexInterval = 0.125f;


    public int bulletIndex;
    public List<GameObject> bullets;
    public AudioSource audio;
    public float reloadTimer, vortexTimer;

    // Start is called before the first frame update
    void Start()
    {
        gunstate = gunState.READY;
        bulletIndex = 0;
        bullets = new List<GameObject>();
        audio = GetComponent<AudioSource>();

        for (int i = 0; i < bulletCount; ++i)
        {
            bullets.Add(Instantiate(bullet, bulletPool.transform.position, Quaternion.identity, bulletPool.transform));
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (gunstate)
        {
            case gunState.IS_VORTEXING:
                vortexTimer += Time.deltaTime;
                if (vortexTimer >= vortexInterval)
                {
                    gunstate = gunState.READY;
                }
                break;
            case gunState.IS_RELOADING:
                reloadTimer += Time.deltaTime;
                if (reloadTimer >= reloadInterval)
                {
                    reloadComplete();
                    reloadTimer = 0f;
                    gunstate = gunState.READY;
                }
                break;
        }
        // object pulling: creation and destruction 횟수를 최소로 할 수 있다.
        if (isAuto && Input.GetKey(KeyCode.X) || !isAuto && Input.GetKeyDown(KeyCode.X))
        {
            switch (gunstate)
            {
                case gunState.READY:
                    normalAttack();
                    vortexTimer = 0f;
                    gunstate = gunState.IS_VORTEXING;
                    if (bulletIndex >= maxMag)
                    { // bullets.Count List.Count; List의 길이
                        reload();
                        gunstate = gunState.IS_RELOADING;
                    }
                    break;
                // case gunState.IS_RELOADING:
                //     noBulletEffect();
                //     break;
            }
        }
        if (Input.GetKeyDown(KeyCode.X) && gunstate == gunState.IS_RELOADING) noBulletEffect();
    }

    void normalAttack()
    {
        // audio.clip=fireSound;
        fire(bullets[bulletIndex]);
        ++bulletIndex;
    }

    void fire(GameObject bullet)
    {
        bullet.transform.position = this.transform.position;
        bullet.GetComponent<Rigidbody>().velocity = Vector3.right * bulletSpeed * (player.GetComponent<PlayerManager>().isfront ? 1 : -1);
        audio.Stop();
        audio.PlayOneShot(fireSound);
        bullet.SetActive(true);
        if (shellHitSounds.Length > 0) audio.PlayOneShot(shellHitSounds[Random.Range(0, shellHitSounds.Length)]);
        //transform.parent.transform.localScale.x;
    }

    void noBulletEffect()
    { // https://pixabay.com/sound-effects/search/revolver/?pagi=2&duration=0-30
        audio.PlayOneShot(dryFireSound);
    }

    void reload()
    {
        audio.clip = reloadSound;
        // audio.loop=true;
        audio.Play();
    }

    void reloadComplete()
    {
        // audio.loop=false;
        // audio.Stop();
        bulletIndex = 0;
    }
}

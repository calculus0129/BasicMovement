using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public GameObject bullet, bulletPool, player; // bulletPool을 구별!
    public AudioClip fireSound, dryFireSound, reloadSound;
    public AudioClip[] shellHitSounds;
    public float bulletSpeed = 40;
    public float reloadTime = 1;
    public int bulletCount = 6;


    public int bulletIndex;
    public List<GameObject> bullets;
    public AudioSource audio;

    bool hasAmmo=true, isReloading=false;
    float reloadTimer;

    // Start is called before the first frame update
    void Start()
    {
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
        if(isReloading) reloadTimer += Time.deltaTime;
        // object pulling: creation and destruction 횟수를 최소로 할 수 있다.
        if (Input.GetKeyDown(KeyCode.X))
        {
            if(hasAmmo) normalAttack();
            else {
                noBulletEffect();
                if(!isReloading) reload();
                if(reloadTimer>=reloadTime) {
                    reloadComplete();
                }
            }
            // Instantiate(bullet, this.transform.position, Quaternion.identity);
        }
    }

    void normalAttack()
    {
        fire(bullets[bulletIndex]);
        ++bulletIndex;
        hasAmmo = bulletIndex < bullets.Count; // List.Count; List의 길이
    }

    void fire(GameObject bullet)
    {
        bullet.transform.position = this.transform.position;
        bullet.GetComponent<Rigidbody>().velocity = Vector3.right * bulletSpeed * (player.GetComponent<PlayerManager>().isfront ? 1 : -1);
        audio.PlayOneShot(fireSound);
        bullet.SetActive(true);
        if(shellHitSounds.Length>0) audio.PlayOneShot(shellHitSounds[Random.Range(0, shellHitSounds.Length)]);
        //transform.parent.transform.localScale.x;
    }

    void noBulletEffect()
    { // https://pixabay.com/sound-effects/search/revolver/?pagi=2&duration=0-30
        audio.PlayOneShot(dryFireSound);
    }

    void reload() {
        isReloading=true;
        audio.clip = reloadSound;
        // audio.loop=true;
        audio.Play();
    }

    void reloadComplete() {
        // audio.loop=false;
        reloadTimer=0;
        bulletIndex=0;
        hasAmmo=true;
        isReloading=false;
    }
}

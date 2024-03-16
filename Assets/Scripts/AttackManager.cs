using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public GameObject bullet, bulletPool, player; // bulletPool을 구별!
    public AudioClip fireSound, dryFireSound;
    public float bulletSpeed = 40;
    public int bulletCount = 6;


    public int bulletIndex = 0;
    public List<GameObject> bullets;
    public AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
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
        // object pulling: creation and destruction 횟수를 최소로 할 수 있다.
        if (Input.GetKeyDown(KeyCode.X))
        {
            normalAttack();
            // Instantiate(bullet, this.transform.position, Quaternion.identity);
        }
    }

    void normalAttack()
    {
        if (bulletIndex < bullets.Count) // List.Count; List의 길이 
        {
            fire(bullets[bulletIndex]);
            ++bulletIndex;
        }
        else
        {
            noBulletEffect();
        }
        audio.Play();
    }

    void fire(GameObject bullet)
    {
        audio.clip = fireSound;
        bullet.transform.position = this.transform.position;
        bullet.GetComponent<Rigidbody>().velocity = Vector3.right * bulletSpeed * (player.GetComponent<PlayerManager>().isfront ? 1 : -1);
        bullet.SetActive(true);
        //transform.parent.transform.localScale.x;
    }

    void noBulletEffect()
    { // https://pixabay.com/sound-effects/search/revolver/?pagi=2&duration=0-30
        audio.clip = dryFireSound;

    }
}

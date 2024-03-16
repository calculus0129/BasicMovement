using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public GameObject bullet, bulletPool, player; // bulletPool을 구별!
    public float bulletSpeed = 40;
    public int bulletCount = 6;


    public int bulletIndex = 0;
    public List<GameObject> bullets;

    // Start is called before the first frame update
    void Start()
    {
        bullets = new List<GameObject>();

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
            bullets[bulletIndex].transform.position = this.transform.position;
            bullets[bulletIndex].SetActive(true);
            bullets[bulletIndex].GetComponent<Rigidbody>().velocity = Vector3.right * bulletSpeed * (player.GetComponent<PlayerManager>().isfront ? 1 : -1);//transform.parent.transform.localScale.x;
        }
        else {
            noBulletEffect();
        }
    }

    void noBulletEffect() { // https://pixabay.com/sound-effects/search/revolver/?pagi=2&duration=0-30

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public GameObject bullet, bulletPool, player; // bulletPool을 구별!
    public float bulletSpeed = 20;
    // Start is called before the first frame update

    public int bulletIndex = 0;

    public List<GameObject> bullets;
    void Start()
    {
        bullets = new List<GameObject>();
        bulletIndex = 0;

        for(int i=0;i<10;++i) {
            bullets.Add(Instantiate(bullet, this.transform.position, Quaternion.identity, bulletPool.transform));
        }
    }

    // Update is called once per frame
    void Update()
    {
        // object pulling: creation and destruction을 작게 할 수 있다.
        if(Input.GetKeyDown(KeyCode.X)) {
            bullets[bulletIndex].transform.position = this.transform.position;
            bullets[bulletIndex].SetActive(true);
            bullets[bulletIndex].GetComponent<Rigidbody>().velocity = Vector3.right * bulletSpeed * (player.GetComponent<PlayerManager>().isfront?1:-1);//transform.parent.transform.localScale.x;
            bulletIndex++;
            if(bulletIndex >= bullets.Count) { // List.Count; Array의 길이 
                bulletIndex = 0;
            }
            // Instantiate(bullet, this.transform.position, Quaternion.identity);
        }
    }
}

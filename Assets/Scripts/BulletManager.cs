using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    public float duration=1.0f;
    float timer; // 0 으로 초기화
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime; // 활성화된지 지난 시간 in seconds
        if(timer > duration) {
            this.gameObject.SetActive(false);
            timer = 0f;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(!other.gameObject.CompareTag("player")) {
            gameObject.SetActive(false);
        }
    }
}

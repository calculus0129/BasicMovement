using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // public => Editor에서 drag and Drop 가능
    public GameObject player; // gunpivot => 그걸 돌려줄 수도 있다! 아니면 scale을 (-1,1,1)로!
    public float jumpMulti = 10;
    public float moveMulti = 2;
    public bool isfront = true;
    public int maxjumpcnt = 2;
    public int curJumpableCnt = 0;
    public Vector3 nowVel;

    // public bool grounded = false; // 봐야겠다 싶어서 public으로 함

    // Start is called before the first frame update
    // By Drag and Drop
    void Start()
    {
        curJumpableCnt = 1;
    }

    // Update is called once per frame // => 성능에 따라 다르게 간격으로 불러짐.
    void Update()
    {
        // KeyDown: 키를 누른 이벤트에 작동
        // Key: 키를 누르는 도중에 작동
        // KeyUp: 키를 뗄 때 한 번만
        nowVel = player.GetComponent<Rigidbody>().velocity;
        Jump();
        Move();
        player.GetComponent<Rigidbody>().velocity = nowVel;

    }

    void Jump()
    {
        if(Input.GetKeyDown(KeyCode.C) && curJumpableCnt>0) {
            --curJumpableCnt;
            nowVel.y = jumpMulti;
            // Vector3.up: (0, 1, 0)
        }
    }

    void Move()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            nowVel.z = moveMulti;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            nowVel.z = -moveMulti;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) && isfront && !Input.GetKeyDown(KeyCode.RightArrow))
        {
            isfront = false;
            player.transform.Rotate(new Vector3(0, 180, 0));
            
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && !isfront && !Input.GetKeyDown(KeyCode.LeftArrow))
        {
            isfront = true;
            player.transform.Rotate(new Vector3(0, 180, 0));
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            nowVel.x = -moveMulti;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            nowVel.x = moveMulti;
        }
    }

    // 딱 collide할때 스 순간만
    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("ground")) {
            curJumpableCnt = maxjumpcnt;
        }
        // curJumpableCnt = maxjumpcnt;
    }

    // collide되고있는 순간
    public void OnCollisionStay(Collision collision)
    {
    }

    // collide 딱 떨어졌을 때
    public void OnCollisionExit(Collision collision)
    {
        // grounded
    }

    // FixedUpdate: 고정된 시간 간격마다 프레임을 불러올 때. UnityEditor에서 결정. e.g. 리듬게임 등
    // 그게 따로 로드가 들어간다고 한다.

}

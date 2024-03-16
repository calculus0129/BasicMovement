using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // public => Editor에서 drag and Drop 가능
    public GameObject player; // gunpivot => 그걸 돌려줄 수도 있다! 아니면 scale을 (-1,1,1)로!
    public float jumpMulti = 10;
    public float moveMulti = 2;
    public bool isfront;
    public int maxjumpcnt = 2;
    public int curJumpableCnt = 0; // 봐야겠다 싶어서 public으로 함
    public Vector3 nowVel;

    enum KeyTypes
    {
        DOWN, KEY, UP
    }

    // public bool grounded = false;

    readonly bool[][] keys = new bool[4][];
    void getKeys()
    {
        keys[0][(int)KeyTypes.DOWN] = Input.GetKeyDown(KeyCode.RightArrow);
        keys[0][(int)KeyTypes.KEY] = Input.GetKey(KeyCode.RightArrow);
        keys[0][(int)KeyTypes.UP] = Input.GetKeyUp(KeyCode.RightArrow);
        keys[1][(int)KeyTypes.DOWN] = Input.GetKeyDown(KeyCode.LeftArrow);
        keys[1][(int)KeyTypes.KEY] = Input.GetKey(KeyCode.LeftArrow);
        keys[1][(int)KeyTypes.UP] = Input.GetKeyUp(KeyCode.LeftArrow);
        keys[2][(int)KeyTypes.DOWN] = Input.GetKeyDown(KeyCode.UpArrow);
        keys[2][(int)KeyTypes.KEY] = Input.GetKey(KeyCode.UpArrow);
        keys[2][(int)KeyTypes.UP] = Input.GetKeyUp(KeyCode.UpArrow);
        keys[3][(int)KeyTypes.DOWN] = Input.GetKeyDown(KeyCode.DownArrow);
        keys[3][(int)KeyTypes.KEY] = Input.GetKey(KeyCode.DownArrow);
        keys[3][(int)KeyTypes.UP] = Input.GetKeyUp(KeyCode.DownArrow);
    }

    // Start is called before the first frame update
    void Start()
    {
        isfront = true;
        for(int i=0,e=keys.Count();i<e;++i) keys[i] = new bool[3];
    }

    // Update is called once per frame // => 성능에 따라 다르게 간격으로 불러짐.
    void Update()
    {
        // KeyDown: 키를 누른 이벤트에 작동
        // Key: 키를 누르는 도중에 작동
        // KeyUp: 키를 뗄 때 한 번만
        nowVel = player.GetComponent<Rigidbody>().velocity;
        getKeys();
        Jump();
        Move();
        // https://en.wikipedia.org/wiki/Quaternions_and_spatial_rotation
        // isfront ? new Quaternion(1, 0, 0, 0) : new Quaternion(0, 0, 1, 0);
        
        // https://docs.unity3d.com/kr/2022.3/Manual/class-Quaternion.html
        player.transform.rotation = isfront ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0);
        player.GetComponent<Rigidbody>().velocity = nowVel;
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.C) && curJumpableCnt > 0)
        {
            --curJumpableCnt;
            nowVel.y = jumpMulti;
            // Vector3.up: (0, 1, 0)
        }
    }

    void Move()
    {
        if (keys[0][(int)KeyTypes.DOWN] || (keys[1][(int)KeyTypes.UP] && keys[0][(int)KeyTypes.KEY]))
        {
            isfront = true;
            // nowVel.x = moveMulti;
        }
        if (keys[1][(int)KeyTypes.DOWN] || (keys[0][(int)KeyTypes.UP] && keys[1][(int)KeyTypes.KEY]))
        {
            isfront = false;
            // nowVel.x = -moveMulti;
        }
        // if (isfront && keys[0][(int)KeyTypes.KEY]) nowVel.x = moveMulti;
        // if (!isfront && keys[1][(int)KeyTypes.KEY]) nowVel.x = -moveMulti;

        setVel(keys[0], keys[1], moveMulti, 0);
        setVel(keys[2], keys[3], moveMulti, 2);
    }

    void setVel(bool[] plus, bool[] minus, float multi, int axis) {
        int status=0;
        if(plus[(int)KeyTypes.DOWN] || (!minus[(int)KeyTypes.KEY] && plus[(int)KeyTypes.KEY])) {
            status = 1;
        }
        if(minus[(int)KeyTypes.DOWN] || (!plus[(int)KeyTypes.KEY] && minus[(int)KeyTypes.KEY])) {
            status = -1;
        }
        if(status!=0) switch(axis) {
            case 0:
                nowVel.x = status*multi;
                break;
            case 1:
                nowVel.y = status*multi;
                break;
            case 2:
                nowVel.z = status*multi;
                break;
        }
    }

    // 딱 collide할때 그 순간만
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            curJumpableCnt = maxjumpcnt;
        }
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

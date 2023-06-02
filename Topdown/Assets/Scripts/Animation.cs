using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb;
    private GameObject player;
    private int currentIndex;

    private string[] triggers = new string[] { "Forward", "RightForward", "Right", "RightBackward", "Backward", "LeftBackward", "Left", "LeftForward" };

    // Start is called before the first frame update
    void Start()
    {
        animator = GameObject.FindGameObjectWithTag("AvatarMesh").GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        currentIndex = 100;
    }

    // Update is called once per frame
    void Update()
    {
        // 마우스 커서 위치 , 플레이어 위치로 애니메이션 결정

        Vector3 mp = Input.mousePosition;
        int index = -9;

        if (Input.GetKeyDown(KeyCode.R))
        {
            animator.SetTrigger("Reloading");
        }

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetBool("Aiming",true);
        }

        if (Input.GetMouseButtonUp(0))
        {
            animator.SetBool("Aiming", false);
        }

        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
        {
            animator.SetBool("IsWalking", false);
        }
        else
        {
            animator.SetBool("IsWalking", true);

            if ((mp.x >= Screen.width * 2 / 5 && mp.x <= Screen.width * 3 / 5) || (mp.y >= Screen.height * 2 / 5 && mp.y <= Screen.height * 3 / 5)) // 위 아래 좌 우
            {
                if (mp.x >= Screen.width * 2 / 5 && mp.x <= Screen.width * 3 / 5)
                {
                    if (mp.y > Screen.height / 2)
                    {
                        index = SetAnimation()-1;
                    }
                    else
                    {
                        index = SetAnimation()-5;
                    }
                }
                else if((mp.y >= Screen.height * 2 / 5 && mp.y <= Screen.height * 3 / 5))
                {
                    if (mp.x > Screen.width / 2)
                    {
                        index = SetAnimation()-3;
                    }
                    else
                    {
                        index = SetAnimation()-7;
                    }
                }
            }
            else // 대각선
            {
                if (mp.x > Screen.width / 2)
                {
                    if (mp.y > Screen.height / 2)
                    {
                        index = SetAnimation()-2;
                    }
                    else
                    {
                        index = SetAnimation()-4;
                    }
                }
                else
                {
                    if (mp.y > Screen.height / 2)
                    {
                        index = SetAnimation();
                    }
                    else
                    {
                        index = SetAnimation()-6;
                    }
                }
            }
        }

        if (index < 1)
        {
            index += 8;
        }

        if (index != currentIndex)
        {
            if (index != -1)
            {
                animator.SetTrigger(triggers[index - 1]);
            }
            
            currentIndex = index;
        }
        
    }

    int SetAnimation()
    {
        if (Input.GetAxisRaw("Vertical")==0 || Input.GetAxisRaw("Horizontal") == 0)
        {
            if (Input.GetAxisRaw("Vertical") == 0)
            {
                if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    return 4;
                }
                else
                {
                    return 8;
                }
            }

            if (Input.GetAxisRaw("Horizontal") == 0)
            {
                if (Input.GetAxisRaw("Vertical") > 0)
                {
                    return 2;
                }
                else
                {
                    return 6;
                }
            }
        }
        else
        {
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                if(Input.GetAxisRaw("Vertical") > 0)
                {
                    return 2;
                }
                else
                {
                    return 6;
                }
            }
            else
            {
                if (Input.GetAxisRaw("Vertical") > 0)
                {
                    return 2;
                }
                else
                {
                    return 6;
                }
            }
        }
        return -9;
    }
}

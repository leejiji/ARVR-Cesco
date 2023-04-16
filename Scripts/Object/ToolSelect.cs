using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolSelect : MonoBehaviour
{
    [SerializeField] private GameObject weaponContents;
    [SerializeField] private GameObject scrollBar;

    private float scroll_pos;
    private float distance;
    private float[] scroll_size;

    private void Start()
    {
        scroll_pos = scrollBar.GetComponent<Scrollbar>().value;
        scroll_size = new float[weaponContents.transform.childCount];
        distance = 1f / (scroll_size.Length - 1f);

        for (int i = 0; i < scroll_size.Length; i++)
        {
            scroll_size[i] = distance * i;
        }
    }

    void Update()
    {       
        if(!GameManager.instance.isToolChange)
        {
            return;
        }

        if (Input.GetMouseButton(0))
        {
            scroll_pos = scrollBar.GetComponent<Scrollbar>().value;
        }
        else
        {
            for (int i = 0; i < scroll_size.Length; i++)
            {
                if (scroll_pos < scroll_size[i] + (distance / 2) && scroll_pos > scroll_size[i] - (distance / 2))
                {
                    scrollBar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollBar.GetComponent<Scrollbar>().value, scroll_size[i], 0.1f); //이동

                    weaponContents.transform.GetChild(i).localScale = Vector2.Lerp(weaponContents.transform.GetChild(i).localScale, new Vector2(1f, 1f), 0.1f); //크기
                    weaponContents.transform.GetChild(i).GetComponent<Button>().enabled = true;

                    for (int j = 0; j < scroll_size.Length; j++)
                    {
                        if (j != i)
                        {
                            weaponContents.transform.GetChild(j).localScale = Vector2.Lerp(weaponContents.transform.GetChild(j).localScale, new Vector2(0.8f, 0.8f), 0.1f);
                            weaponContents.transform.GetChild(j).GetComponent<Button>().enabled = false;
                        }
                    }
                }
            }
        }

        
    }
}

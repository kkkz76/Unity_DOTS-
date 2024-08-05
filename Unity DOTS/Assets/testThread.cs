using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testThread : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(delete());

        Testing();
    }

    // Update is called once per frame
    void Update()
    {



    }
    IEnumerator delete()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }

    private void Testing()
    {
        float power = 1f;
        for (int i = 0; i < 100000; i++)
        {
            power *= 2f;
            power /= 2f;
        }
    }
}

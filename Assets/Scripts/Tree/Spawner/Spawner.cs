using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    GameObject treeTemplate;


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 10000))
            {
                Debug.Log("hit");
                Debug.Log(hit.transform.name);
                Debug.Log(hit.point);

                GameObject tree = Instantiate(treeTemplate, hit.point, Quaternion.identity);
                tree.GetComponent<Tree>().Initialize();
                tree.GetComponent<Tree>().Spawn();
            }
        }
    }

}

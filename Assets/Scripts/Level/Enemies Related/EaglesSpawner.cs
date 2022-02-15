using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EaglesSpawner : MonoBehaviour
{
    public List<Transform> eaglesList;
    Transform target;
    float detectDistance = 19;

    // Start is called before the first frame update
    void Start()
    {
        target = Player.Instance.transform;
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Transform eagle in eaglesList)
        {
            if(eagle != null)
            {
                float dist = Vector2.Distance(eagle.position, target.position);
                if (dist < detectDistance)
                    eagle.gameObject.SetActive(true);
            }
            
        }
    }
}

using Framework.SO;
using Game.Tech;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var tree = SingletonSOManager.Instance.GetSOFile<TechTreeSO>("TechTree").GetTree();
        Debug.Log(tree);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

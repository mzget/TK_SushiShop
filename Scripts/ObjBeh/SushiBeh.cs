using UnityEngine;
using System;
using System.Collections;

public class SushiBeh : GoodsBeh {

	// Use this for initialization
    protected override void Start()
    {
        base.Start();

        _canDragaable = true;
//        offsetPos = Vector3.down * 7f;
    }
	
	// Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}

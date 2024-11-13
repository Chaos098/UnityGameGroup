using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class DeathBringerBoss : Boss
{
    // Start is called before the first frame update
    #region State
    public DeathBringerIdleState IdleState { get; set; }
    public BossState attackState { get; internal set; }
    #endregion
    protected override void Awake()
    {
        base.Awake();
        IdleState = new DeathBringerIdleState(stateMachine,this,"Idle",this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(IdleState);
    }

    protected override void Update()
    {
        base.Update();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerIdleState : BossState
{
    private DeathBringerBoss enemy;
    public DeathBringerIdleState(BossStateMachine stateMachine, Boss bossBase, string animBoolName,DeathBringerBoss DeathBringerBoss) : base(stateMachine, bossBase, animBoolName)
    {
        this.enemy = DeathBringerBoss;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.idleTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
        if(Input.GetKeyDown(KeyCode.V))
        {
            stateMachine.ChangeState(enemy.TeleportState);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerTeleportState : BossState
{
    private DeathBringerBoss enemy;
    public DeathBringerTeleportState(BossStateMachine stateMachine, Boss bossBase, string animBoolName,DeathBringerBoss _enemy) : base(stateMachine, bossBase, animBoolName)
    {
        this.enemy = _enemy;
    }
    public override void Enter()
    {
        base.Enter();
        enemy.FindPosition();
        stateTimer = 1;
    }
    public override void Update()
    {
        base.Update();
        if(stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.IdleState);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerAttackState : BossState
{
    private DeathBringerBoss enemy;
    public DeathBringerAttackState(BossStateMachine stateMachine, Boss bossBase, string animBoolName, DeathBringerBoss enemy) : base(stateMachine, bossBase, animBoolName)
    {
        this.enemy = enemy;
    }
}

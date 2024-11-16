using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerSpellCastState : BossState
{
    private DeathBringerBoss enemy;
    public DeathBringerSpellCastState(BossStateMachine stateMachine, Boss bossBase, string animBoolName, DeathBringerBoss enemy) : base(stateMachine, bossBase, animBoolName)
    {
        this.enemy = enemy;
    }
}

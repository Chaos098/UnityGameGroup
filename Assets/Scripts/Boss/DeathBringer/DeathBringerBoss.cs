using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class DeathBringerBoss : Boss
{
    // Start is called before the first frame update
    #region State
    public DeathBringerIdleState IdleState { get; set; }
    public DeathBringerAttackState AttackState { get; internal set; }
    public DeathBringerBattleState BattleState { get; internal set; }
    public DeathBringerDeadState DeadState { get; internal set; }
    public DeathBringerSpellCastState SpellCastState { get; internal set; }
    public DeathBringerTeleportState TeleportState { get; internal set; }
    #endregion
    [Header("teleport detail")]
    [SerializeField] private BoxCollider2D arena;
    [SerializeField] private Vector2 surroundingCheckSize;
    protected override void Awake()
    {
        base.Awake();
        SetupDefailtFacingDir(-1);
        IdleState = new DeathBringerIdleState(stateMachine,this,"Idle",this);
        AttackState = new DeathBringerAttackState(stateMachine, this, "Attack", this);
        DeadState = new DeathBringerDeadState(stateMachine, this, "Idle", this);
        BattleState = new DeathBringerBattleState(stateMachine, this, "Move", this);
        SpellCastState = new DeathBringerSpellCastState(stateMachine, this, "SpellCast", this);
        TeleportState = new DeathBringerTeleportState(stateMachine, this, "Teleport", this);
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
    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(DeadState);
    }

    private RaycastHit2D GroundBelow() => Physics2D.Raycast(transform.position, Vector2.down, 100, whatIsGround);
    private bool SomethingIsAround() => Physics2D.BoxCast(transform.position, surroundingCheckSize, 0, Vector2.zero, 0, whatIsGround);

    public void FindPosition()
    {
        float x = Random.Range(arena.bounds.min.x + 3, arena.bounds.max.x - 3);
        float y = Random.Range(arena.bounds.min.y + 3, arena.bounds.max.y - 3);

        transform.position = new Vector3(x, y);
        transform.position = new Vector3(transform.position.x, transform.position.y - GroundBelow().distance + (cd.size.y / 2));

        if (!GroundBelow() || SomethingIsAround())
        {
            //Debug.Log("Looking for new position");
            FindPosition();
        }
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - GroundBelow().distance));
        Gizmos.DrawWireCube(transform.position, surroundingCheckSize);
    }

}

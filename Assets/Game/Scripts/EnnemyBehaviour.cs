using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyBehaviour : CharacterBehaviour
{
    public CharacterBehaviour player;
    public CharacterBehaviour targetEnnemy;

    void Start()
    {
        health.onDieCallback.AddListener(AddXp);
        player.onMoveCompleteCallback.AddListener(FindATarget);
        Initialize();
    }

    public void AddXp()
    {
        //Debug.Log("Add XP ! ");
        if (player != null)
            player.character.GainXp(character.level.Value);
        //health.onDieCallback.RemoveListener(AddXp);
    }


    #region FSM
    public enum STATE
    {
        IDLE,
        MOVE,
        ATTACK,
        DEAD
    }

    public STATE currentState;

    public bool findNewEnnemy;

    public void Initialize()
    {
        currentState = STATE.IDLE;
    }

    float _timer;

    public void ChangeState()
    {
        //currentState = newState;
        if (currentState == STATE.MOVE)
        {
            if (targetEnnemy != null)
            {
                float distance = (targetEnnemy.transform.position - this.transform.position).sqrMagnitude;
                //Debug.Log(distance + " / " + character.weapon.attackRange);
                if (distance <= (character.weapon.attackRange * character.weapon.attackRange) + 0.3f)
                {
                    currentState = STATE.ATTACK;
                    //StartCoroutine(Attack());
                    _timer = 0f;

                    if (targetEnnemy != null)
                    {
                        Vector3 targetposition = targetEnnemy.transform.position;
                        targetposition.y = 0f;
                        Vector3 myposition = transform.position;
                        myposition.y = 0f;

                        _animator.gameObject.transform.LookAt(transform.position - (targetposition - myposition));
                    }

                }
            }
            else
            {
                if (null == path || path.Count == 0)
                {
                    //path.Clear();
                    currentState = STATE.IDLE;
                }
                else
                {
                    if (FindNewTarget())
                    {
                        GetPathToTarget();
                    }
                }
            }
        }
        else if (currentState == STATE.ATTACK)
        {
            if (targetEnnemy == null)
            {
                //StopCoroutine(Attack());
                //Debug.Log("### path count " + path.Count);
                if (hostileCharacters.Count > 0 && path.Count == 0)
                {
                    if (FindNewTarget())
                    {
                        GetPathToTarget();
                    }
                }
                else
                {
                    // No more ennemy
                    currentState = STATE.MOVE;
                }
            }
            else
            {
                float distance = (targetEnnemy.transform.position - this.transform.position).sqrMagnitude;
                if (distance > (character.weapon.attackRange * character.weapon.attackRange) + 0.3f)
                {
                    currentState = STATE.IDLE;
                    //StopCoroutine(Attack());
                }
                else
                {
                    _timer += Time.fixedDeltaTime;
                    if (_timer >= character.AttackSpeed())
                    {
                        targetEnnemy.health.Damage(character.Damage());
                        _timer = 0f;
                    }
                }
            }
        }
        else if (currentState == STATE.IDLE)
        {
            if (path != null)
            {
                if (hostileCharacters.Count > 0)
                {
                    if (FindNewTarget())
                    {
                        //pfController.DikjtraAlgo();
                        GetPathToTarget();
                        if (path.Count > 0)
                        {
                            currentState = STATE.MOVE;
                        }
                        else
                        {
                            currentState = STATE.ATTACK;
                        }
                    }
                }
            }
        }
        //Debug.Log("New State : " + currentState);
    }

    public void GetPathToTarget()
    {
        if (targetEnnemy != null)
        {
            Vector2Int[] newpath = pfController.PathToPlayer(transform.position);
            
            path.Clear();

            foreach (var p in newpath)
            {
                path.Add(p);
            }
            /*path.Clear();
            if(newpath.Length > 0)
                path.Add(newpath[0]);*/
            //InversePathForPlayer(newpath);


            startPosition = pfController.ConvertWorldPositionToArray(transform.position);
        }

    }

    public void FindATarget()
    {
        //if (FindNewTarget())
        //path.Clear();
        targetEnnemy = player;
        GetPathToTarget();
    }

    public void UpdateState()
    {
        switch (currentState)
        {
            case STATE.IDLE:

                break;
            case STATE.MOVE:

                this.Movement();

                break;
            case STATE.ATTACK:

                //nothing

                break;
            case STATE.DEAD:
                break;
        }
        ChangeState();
    }

    public IEnumerator Attack()
    {
        while (targetEnnemy != null && health.currentHp > 0)
        {
            yield return new WaitForSeconds(character.AttackSpeed());
            targetEnnemy.health.Damage(character.Damage());
        }
    }
    #endregion



    public bool FindNewTarget()
    {
        targetEnnemy = hostileCharacters.GetNearest(transform.position);
        if (targetEnnemy != null)
        {
            // trouvé
            return true;
        }
        else
        {
            return false;
        }
    }


    public override void FixedUpdate()
    {
        if (pause)
            return;
        UpdateState();
    }



    public override void OnDestroy()
    {
        base.OnDestroy();
        /*if(player != null)
        {
            player.GetANewPath();
        }*/
    }




}

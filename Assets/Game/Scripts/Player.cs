using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharacterBehaviour
{
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
        if(currentState == STATE.MOVE)
        {
            if (targetEnnemy != null)
            {
                float distance = (targetEnnemy.transform.position - this.transform.position).sqrMagnitude;
                //Debug.Log(distance + " / " + (character.weapon.attackRange * character.weapon.attackRange) + 0.3f);
                if (distance <= (character.weapon.attackRange * character.weapon.attackRange) + 0.3f)
                {
                    currentState = STATE.ATTACK;
                    //StartCoroutine(Attack());
                    _timer = 0f;
                }
                else
                {

                    if (FindNewTarget())
                    {
                        GetPathToTarget();
                    }
                }

            }
            else
            {
                

                if (null == path || path.Count == 0)
                {
                    currentState = STATE.IDLE;
                }
                else
                {
                    
                    if (FindNewTarget())
                    {
                        //GetPathToTarget();
                    }
                }
            }
        }
        else if(currentState == STATE.ATTACK)
        {
            if (targetEnnemy == null)
            {
                //StopCoroutine(Attack());
                //Debug.Log("### path count " + path.Count);
                if (hostileCharacters.Count > 0 && path.Count == 0)
                {
                    if (FindNewTarget())
                    {
                        //GetPathToTarget();
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
                if (distance > ( character.weapon.attackRange * character.weapon.attackRange) +0.3f)
                {
                    currentState = STATE.IDLE;
                    //StopCoroutine(Attack());
                }
                else
                {
                    _timer += Time.fixedDeltaTime;
                    if(_timer >= character.AttackSpeed())
                    {
                        targetEnnemy.health.Damage(character.Damage());
                        _timer = 0f;
                    }
                }
            }
        }
        else if(currentState == STATE.IDLE)
        {
            if (path != null)
            {
                if (hostileCharacters.Count > 0)
                {
                    //pfController.DikjtraAlgo();
                    if (FindNewTarget())
                    {
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

    


    public void UpdateState()
    {
        switch(currentState)
        {
            case STATE.IDLE:
                if(_animator != null)
                { 
                    _animator.SetFloat("speed", 0f);
                    _animator.SetBool("attackNormal", false);
                }

                break;
            case STATE.MOVE:
                if (_animator != null)
                {
                    _animator.SetFloat("speed", 1f);
                    _animator.SetBool("attackNormal", false);
                }

                this.Movement();

                break;
            case STATE.ATTACK:
                if (_animator != null)
                {
                    _animator.SetFloat("speed", 0f);
                    _animator.SetBool("attackNormal", true);
                    if(targetEnnemy != null)
                    _animator.gameObject.transform.LookAt(transform.position + (transform.position -targetEnnemy.transform.position).normalized);
                }
                //if (_animator != null)

                //nothing

                break;
            case STATE.DEAD:
                if (_animator != null)
                {
                    _animator.SetFloat("speed", 0f);
                    _animator.SetBool("attackNormal", false);
                }

                break;
        }
        ChangeState();
    }

    public IEnumerator Attack()
    {
        while(targetEnnemy != null && health.currentHp > 0)
        {
            


            yield return new WaitForSeconds(character.AttackSpeed());
            targetEnnemy.health.Damage(character.Damage());
        }
    }
    #endregion


    public void UpdateTargetPath()
    {
        pfController.DikjtraAlgo();
        GetPathToTarget();
    }

    // Start is called before the first frame update
    public void Start()
    {
        Initialize();

        onMoveCompleteCallback.AddListener(UpdateTargetPath);

        //pfController.onCarteUpdatedCallback.AddListener(GetANewPath);
    }


    public override void FixedUpdate()
    {
        UpdateState();
    }



    

    public CharacterBehaviour targetEnnemy;
    public void GetANewPath()
    {
        targetEnnemy = hostileCharacters.GetNearest(transform.position);
        if(targetEnnemy != null)
        {
            //Debug.Log("Player go to => " + targetEnnemy.name);
            Vector2Int[] newpath = pfController.PathToPlayer(targetEnnemy.transform.position);
            InversePathForPlayer(newpath);

            startPosition = pfController.ConvertWorldPositionToArray(transform.position);
            idle = false;
        }
    }

    

    public void GetPathToTarget()
    {
        //Debug.Log("Player go to => " + targetEnnemy.name);
        if (targetEnnemy != null)
        {
            Vector2Int[] newpath = pfController.PathToPlayer(targetEnnemy.transform.position);
            InversePathForPlayer(newpath);

            startPosition = pfController.ConvertWorldPositionToArray(transform.position);
        }
    }

    public void InversePathForPlayer(Vector2Int[] path)
    {
        Vector2Int[] newpath = new Vector2Int[path.Length];
        for (int i = 0; i < path.Length; i++)
        {
            //path[i] = path[i] * -1;
            newpath[path.Length - i - 1] = path[i] * -1;
        }
        AddNewPath(newpath);
    }

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

    /*public void FixedUpdate()
    {
        //if(targetEnnemy == null)
        //{
        //    GetANewPath();
        //}
        UpdateState();
    }*/

    /*private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ennemy"))
        {
            Destroy(collision.gameObject);
            pfController.DikjtraAlgo();
            //index = 0;
        }
    }*/
}

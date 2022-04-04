using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerNewTest : CharacterBehaviour
{
    public IntReference     mana;
    public IntReference     circularAttackCost;
    public IntReference     circularDamage;
    public FloatReference   circularRange;
    public LayerMask        ennemyMask;

    public AudioSource audioSimpleAttack;
    public AudioSource audioCircularAttack;

    public enum STATE
    {
        IDLE,
        MOVE,
        ATTACK,
        CIRCULARATTACK,
        DEAD
    }

    public STATE currentState;


    public CharacterBehaviour target;


    // Start is called before the first frame update
    void Start()
    {
        currentState = STATE.IDLE;
        mana.Variable.ChangeValue( character.stats.mana.Value );
        StartCoroutine(ManaRegenarationUpdate());
    }

    float _timer = 0f;
    //float _timerMove = 0f;
    // Update is called once per frame
    public override void FixedUpdate()
    {
        if (pause)
            return;

        if(currentState == STATE.IDLE)
        {

            if (_animator != null)
            {
                _animator.SetFloat("speed", 0f);
                _animator.SetBool("attackNormal", false);
            }

            // Check
            if(target == null)
            {
                FindNewTarget();
                GetPathToTarget();
                if (ppath != null && ppath.Count > 0)
                {
                    currentState = STATE.MOVE;
                }
            }
            else
            {
                if (ppath != null && ppath.Count > 0)
                {
                    currentState = STATE.MOVE;
                }
            }
        }
        else if(currentState == STATE.MOVE)
        {

            if (_animator != null)
            {
                _animator.SetFloat("speed", 1f);
                _animator.SetBool("attackNormal", false);
            }
            
            // Move To target
            //Vector3 position = 
            if (ppath != null && ppath.Count > 0)
            {
                Vector3 direction = ppath[0] - new Vector3(transform.position.x, 0f, transform.position.z);
                direction.y = 0f;

                _rigidbody.velocity = direction.normalized * speed + new Vector3(0f, _rigidbody.velocity.y, 0f);

                if (_animator != null)
                    _animator.gameObject.transform.LookAt(transform.position - direction);

                /*Debug.Log((ppath[0] - new Vector3(transform.position.x, 0f, transform.position.z)).sqrMagnitude);
                Debug.Log(transform.position);
                Debug.Log(ppath[0]);*/
                //Debug.Log((ppath[0] - transform.position).sqrMagnitude);
                

                if ((ppath[0] - transform.position).sqrMagnitude < 0.1f)
                {
                    ppath.RemoveAt(0);
                    onMoveCompleteCallback?.Invoke();
                }
            }

            if (target != null)
            {
                Vector3 targetposition = target.transform.position;
                targetposition.y = 0f;
                Vector3 myposition = transform.position;
                myposition.y = 0f;

                float distance = (targetposition - myposition).sqrMagnitude;
                //Debug.Log(distance + " / " + (character.weapon.attackRange * character.weapon.attackRange) + 0.3f);
                //Debug.Log(distance + " / " + (character.weapon.attackRange * character.weapon.attackRange) + 0.3f);
                if (distance <= (character.weapon.attackRange * character.weapon.attackRange) + 0.3f)
                {
                    currentState = STATE.ATTACK;
                    //StartCoroutine(Attack());
                    _timer = 0f;

                }
                else
                {
                    if (ppath.Count == 0)
                    {
                        ppath.Add(targetposition);
                    }
                }

            }
            else
            {
                currentState = STATE.IDLE;
            }
        }
        else if(currentState == STATE.ATTACK)
        {
            if (_animator != null)
            {
                _animator.SetFloat("speed", 0f);
                _animator.SetBool("attackNormal", true);

                /*if (target != null)
                {
                    Vector3 targetposition = target.transform.position;
                    targetposition.y = 0f;
                    Vector3 myposition = transform.position;
                    myposition.y = 0f;

                    _animator.gameObject.transform.LookAt(targetposition - myposition);
                }*/

            }

            if (target != null)
            {
                Vector3 targetposition = target.transform.position;
                targetposition.y = 0f;
                Vector3 myposition = transform.position;
                myposition.y = 0f;

                _animator.gameObject.transform.LookAt(transform.position - (targetposition - myposition));
            }


            _timer += Time.fixedDeltaTime;
            if (_timer >= character.AttackSpeed())
            {
                //DamageDeal?.Invoke(character.Damage());
                target.health.Damage(character.Damage());
                _timer = 0f;
                if (audioSimpleAttack != null)
                {
                    if (audioSimpleAttack.isPlaying)
                        audioSimpleAttack.Stop();
                    audioSimpleAttack.Play();
                }
            }

            

            if (target == null)
            {
                FindNewTarget();
                GetPathToTarget();
                if (ppath != null && ppath.Count > 0)
                {
                    currentState = STATE.MOVE;
                }
                else
                {
                    currentState = STATE.IDLE;
                }
            }
            else
            {
                Vector3 targetposition = target.transform.position;
                targetposition.y = 0f;
                Vector3 myposition = transform.position;
                myposition.y = 0f;

                float distance = (targetposition - myposition).sqrMagnitude;
                if (distance > (character.weapon.attackRange * character.weapon.attackRange) + 0.3f)
                {
                    currentState = STATE.IDLE;
                }
            }
        }
        /*else if(currentState == STATE.CIRCULARATTACK)
        {

        }*/
    }



    public void TourbiLol()
    {
        if (mana.Value >= circularAttackCost)
        {
            if (_animator != null)
            {
                _animator.SetTrigger("attackCircular");
            }

            //mana.Value -= circularAttackCost;
            mana.Variable.ApplyChange(-circularAttackCost);
            //Debug.Log(mana.Value);

            RaycastHit[] objects = Physics.SphereCastAll(transform.position, circularRange, Vector3.down, ennemyMask);
            foreach (var o in objects)
            {
                Health h = o.collider.GetComponent<Health>();
                if(h != null)
                {
                    h.Damage(circularDamage.Value);
                }
            }
            if (audioCircularAttack != null)
            {
                if (audioCircularAttack.isPlaying)
                    audioCircularAttack.Stop();
                audioCircularAttack.Play();
            }
        }
    }

    public IEnumerator ManaRegenarationUpdate()
    {
        while(true)
        {
            yield return new WaitForSeconds(5f);
            if (!pause)
            {
                //Debug.Log(Mathf.Clamp(mana.Value + character.stats.manaRegeneration.Value, 0, character.stats.mana.Value));
                mana.Variable.ChangeValue( Mathf.Clamp(mana.Value + character.stats.manaRegeneration.Value, 0, character.stats.mana.Value));
            }
        }
    }


    #region PathFinding Function
    public void SetNewTarget(CharacterBehaviour target)
    {
        this.target = target;

    }

    public bool FindNewTarget()
    {
        SetNewTarget(hostileCharacters.GetNearest(transform.position));
        if (target != null)
        {
            // trouvé
            return true;
        }
        else
        {
            return false;
        }
    }

    public void GetPathToTarget()
    {
        //Debug.Log("Player go to => " + target.name);
        if (target != null)
        {
            Vector3[] newpath = pfController.PathToPlayerWithPosition(target.transform.position);
            InversePathForPlayer(newpath);

            //startPosition = pfController.ConvertWorldPositionToArray(transform.position);
        }
    }

    public void InversePathForPlayer(Vector3[] path)
    {
        Vector3[] newpath = new Vector3[path.Length];
        for (int i = 0; i < path.Length; i++)
        {
            //path[i] = path[i] * -1;
            newpath[path.Length - i - 1] = path[i];// * -1;
        }
        AddNewPath(newpath);
    }

    public List<Vector3> ppath;

    public void AddNewPath(Vector3[] newpath)
    {
        ppath.Clear();
        foreach (var p in newpath)
        {
            ppath.Add(p);
        }
    }

    #endregion
}

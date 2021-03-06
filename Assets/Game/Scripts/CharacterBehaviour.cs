using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterBehaviour : MonoBehaviour
{
    public GameController gameManager;

    public CharacterSO character;
    public PathFinding pfController;
    
    public Characters factionCharacters;
    public Characters hostileCharacters;

    public float speed = 5f;

    public List<Vector2Int> path;
    public Vector2Int startPosition;
    public int index;

    public bool idle;
    public Health health;

    public UnityEvent onMoveCompleteCallback;

    protected Rigidbody _rigidbody;
    protected Animator _animator;

    public bool pause = false;


    public virtual void Awake()
    {
        index = 0;
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        _animator = gameObject.GetComponentInChildren<Animator>();
        health = gameObject.GetComponent<Health>();
        factionCharacters.Add(this);
        gameManager.onPauseEvent.AddListener(Pause);
        gameManager.onResumeEvent.AddListener(Resume);
    }

    public void Pause()
    {
        pause = true;
    }

    public void Resume()
    {
        pause = false;
    }



    public virtual void Movement()
    {
        if (path == null || path.Count <= 0 )
            return;

        Vector3 direction = new Vector3(path[0].x, 0f, path[0].y);
        //direction = direction.normalized;

        _rigidbody.velocity = direction.normalized * speed + new Vector3(0f, _rigidbody.velocity.y, 0f);

        if(_animator != null)
            _animator.gameObject.transform.LookAt(transform.position - direction);

        Vector3 nextPosition = new Vector3(startPosition.x, 0f, startPosition.y) * 3f + direction * 3f;
        nextPosition.y = 1f;

        
        //Debug.Log("distance " + Vector3.Distance(nextPosition , _rigidbody.position));
        //Debug.Log("prochaine position : " + nextPosition);
        //Debug.Log("prochaine position : " + _rigidbody.position);
        /*if(gameObject.name == "Player")
        Debug.Log((nextPosition - _rigidbody.position).sqrMagnitude);*/

        if ((nextPosition - _rigidbody.position).sqrMagnitude < 1.1f)
        {
            nextPosition.y = 1f;
            //_rigidbody.position = nextPosition;
            startPosition += path[0];
            path.RemoveAt(0);
            onMoveCompleteCallback?.Invoke();
            /*index++;
            if (index == path.Count)
            {
                idle = true;
                index = 0;
                path = null;
                _rigidbody.velocity = Vector3.zero + new Vector3(0f, _rigidbody.velocity.y, 0f);
            }*/
        }
    }



    public void AddNewPath(Vector2Int[] newpath)
    {
        path.Clear();
        foreach (var p in newpath)
        {
            path.Add(p);
        }
    }

    public virtual void OnDestroy()
    {
        factionCharacters.Remove(this);
        gameManager.onPauseEvent.RemoveListener(Pause);
        gameManager.onResumeEvent.RemoveListener(Resume);
    }


    // Update is called once per frame
    public virtual void FixedUpdate()
    {
        Movement();
    }
}

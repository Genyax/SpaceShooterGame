using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 2.5f;
    private Player _player;
    private Animator _anim;
    [SerializeField]
    private GameObject _laserPrefab;
    private AudioSource _audioSource;
    private float _fireRate = 3.0f;
    private float _canFire = -1f;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        if (_player == null)
        {
            Debug.Log("The Player is NULL");
        }

        _anim = GetComponent<Animator>();
        

        if (_anim == null)
        {
            Debug.Log("The animator is NULL");

        }

        if (_audioSource == null)
        {
            Debug.Log("The AudioSource on the enemy is NULL.");
        }

    }

    // Update is called once per frame
    void Update()
    {

        CalculateMovement();

        if(Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser  =Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
            
        }


    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -5f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7, 0);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D otherObject)
    {
        if(otherObject.tag == "Player")
        {
            Player player = otherObject.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            Destroy(this.gameObject, 2.8f);
            

        }

        if (otherObject.tag == "Laser")
        {
            Destroy(otherObject.gameObject);
            if (_player != null)
            {
                _player.AddScore(10);
            }

            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();

            Destroy(GetComponent <Collider2D>());
            Destroy(this.gameObject, 2.8f);
        }

    }
}

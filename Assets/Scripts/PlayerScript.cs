using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerScript : MonoBehaviour
{

    public Camera cam;
    public NavMeshAgent agent;
    public Animator anim;
    public GameObject bullet;
    public GameObject bulletPlace;
    public Terrain terrain;
    public GameObject enemyPrefab;

    public Light terrainLight;
    public Light treesLight;


    private void Awake()
    {
       GameManager.init();
       GameManager.manager.terrain = terrain;
       GameManager.manager.player = gameObject;
        GameManager.manager.enemyPrefab = enemyPrefab;
       GameManager.manager.processTerrain();
       GameManager.manager.initializeNewEnemy(10);
       agent.updateRotation = false;
       anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // moveWithMouse();       
        moveWithKeyboard();
        updateAgentAnimation();
        processShooting();
        updateLightRadius();
    }

    void updateLightRadius()
    {
        this.terrainLight.range = GameManager.manager.lightRadius;
        this.treesLight.range = GameManager.manager.lightRadius;
    }

    float getAgentSpeed()
    {
        return agent.velocity.magnitude / agent.speed;
    }

    void updateAgentAnimation()
    {
        anim.SetBool("isWalking", getAgentSpeed() > 0);
    }

    void processShooting()
    {
        if (!Input.GetMouseButtonDown(0))
        {
            anim.SetBool("isShooting", false);
            agent.isStopped = false;
            return;
        }

        if (!GameManager.manager.canShoot())
        {
            return;
        }

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameManager.manager.didShoot();
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            anim.SetBool("isShooting", true);
            FaceTarget(hit.point);
            shootBullet();
        }

    }

    void shootBullet()
    {
        GameObject tempBullet = Instantiate(bullet, bulletPlace.transform.position, bulletPlace.transform.rotation) as GameObject;
        tempBullet.GetComponent<Rigidbody>().AddForce(transform.forward * 100);
        Destroy(tempBullet, 10.0f);
    }

    void stopMoving()
    {
        agent.SetDestination(this.transform.position);
        agent.velocity = Vector3.zero;
    }

    void moveWithMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }

        }
    }

    private void FaceTarget(Vector3 destination)
    {
        Vector3 lookPos = destination - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 2);
    }

    void moveWithKeyboard()
    {
        float horInput = Input.GetAxis("Horizontal");
        float verInput = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horInput, 0f, verInput);
        Vector3 moveDestination = transform.position + movement;

        if (agent.velocity.sqrMagnitude > Mathf.Epsilon)
        {
            transform.rotation = Quaternion.LookRotation(agent.velocity.normalized);
        }

        agent.destination = moveDestination; 
    }
}

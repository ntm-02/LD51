using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerCombat : MonoBehaviour, IKillable
{
    BoxCollider2D boxCollider;
    GameObject attackCollider;
    DamageableComponent damageableComponent;
    GameObject[] neighborTiles;
    [SerializeField] int damagePerHit = 10;
    [SerializeField] private Light2D damageLight;

    private float playerToMouseAngle = 0f;
    Quaternion angle = new();

    private bool firstTime = true;  // this wil stop us from trying to access the grid before it exists

    void Start()
    {
        damageableComponent = this.gameObject.AddComponent<DamageableComponent>();
        //boxCollider = this.gameObject.GetComponent<BoxCollider2D>();
        attackCollider = this.gameObject.transform.Find("AttackCollider").gameObject;
        //damageLight.enabled = false;
    }


    public void UpdateTiles()
    {
        if (!GameManager.IsPlayerMoving)
        {
            //print(GameManager.PlayerGridPos);
            //neighborTiles = TilePathFinding.adjacentToPoint(FindObjectOfType<TilePathFinding>().getGrid(), GameManager.PlayerGridPos);
            //foreach (GameObject tile in neighborTiles)
            //{
            //    Debug.Log(tile.GetComponent<Tile>().GetCollisionObject().name);
            //}
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            damageableComponent.TakeDamage(damagePerHit);
        }
    }

    public void OrientAttackCollider()
    {

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - transform.position;
        float mouseAngle = Vector2.SignedAngle(Vector2.right, direction);
        Debug.Log("angle : " + playerToMouseAngle);
        if ((Mathf.Abs(mouseAngle) > 150))
        {
            //Debug.Log("left");
            angle.eulerAngles = new Vector3(0, 0, 0);
        } else if (Mathf.Abs(mouseAngle) < 40) {
            //Debug.Log("right");
            angle.eulerAngles = new Vector3(0, 0, 180);
        } else if ((mouseAngle <= 150) && (mouseAngle >= 40))                   // facing north
        {
            //Debug.Log("North");
            angle.eulerAngles = new Vector3(0, 0, 270);
        }
        else if ((mouseAngle >= -150) && (mouseAngle <= -40))
        {
            //Debug.Log("South");
            angle.eulerAngles = new Vector3(0, 0, 90);
        }
        

        /*playerToMouseAngle = Mathf.Abs(Vector3.Angle(transform.position, Input.mousePosition));
        Debug.Log("angle : " + playerToMouseAngle);
        if (playerToMouseAngle >= 0f && playerToMouseAngle < 90f)
        {
            angle.eulerAngles = new Vector3(0, 0, 0);
            Debug.Log("Left");
        }
        else if (playerToMouseAngle >= 90f && playerToMouseAngle < 180f)
        {
            angle.eulerAngles = new Vector3(0, 0, 90);
            Debug.Log("Down");
        }
        else if (playerToMouseAngle >= 180f && playerToMouseAngle < 270f)
        {
            angle.eulerAngles = new Vector3(0, 0, 180);
            Debug.Log("Right");
        }
        else
        {
            angle.eulerAngles = new Vector3(0, 0, 270);
            Debug.Log("Up");
        }*/

        attackCollider.transform.eulerAngles = new Vector3 (0, 0, angle.eulerAngles.z);
    }

    public void EndPlayerTurn()
    {
        PlayerTime.currPlayerTime = 0f;
        GameManager.IsPlayerTurn = false;
    }

    public void Die()
    {
        GameManager.IsPlayerDead = true;
        Destroy(gameObject);
    }

    public void NotifyDamage()
    {
        StartCoroutine(DamageLightToggle());
    }

    IEnumerator DamageLightToggle()
    {
        damageLight.enabled = true;
        yield return new WaitForSeconds(.5f);
        damageLight.enabled = false;
    }

    IEnumerator UpdateTileWait()
    {
        yield return new WaitForSeconds(0.1f);
        firstTime = false;
        if (firstTime)
        {
            UpdateTiles();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (firstTime)
        {
           // StartCoroutine(UpdateTileWait());
        }
        else
        {
          //  UpdateTiles();
        }

        //OrientAttackCollider();

        if (Input.GetButtonDown("Fire1"))
        {
            OrientAttackCollider();

        }
    }
}

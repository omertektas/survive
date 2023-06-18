using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class characterController : MonoBehaviour
{
    
    public static characterController Instance;

    [Header("TOKYO")]
    [SerializeField] private float characterSpeed;   
    [SerializeField] private GameObject car;
    [SerializeField] private GameObject carCamera; 
    [SerializeField] private GameObject fText;
    [SerializeField] private MonoBehaviour carScript;
    Animator anim;
    [SerializeField] private GameObject[] TokyoMesh;
    [SerializeField] private TextMeshProUGUI health;
    [SerializeField] GameObject damagePanel;
    private AudioSource tokyoSoundSource;

    public int hth = 100;
    

    void Start()
    {
        
        Instance = this;
        anim = GetComponent<Animator>();
        anim.SetBool("dead", false);
        tokyoSoundSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        
        deadFunc();
        move();
        getInThecar();
       
        
    }

    void move()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");
        anim.SetFloat("Horizontal",hor);
        anim.SetFloat("Vertical",ver);
        this.gameObject.transform.Translate(hor * characterSpeed*Time.deltaTime, 0, ver * characterSpeed*Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "damage")
        {
            if (hth>0) hth -= 10;
            health.text = hth.ToString();
            StartCoroutine(damageAnimation());
            tokyoSoundSource.Play();
            //sound effect
        }
    }

    private void deadFunc()
    {
        if (hth<=0)
        {         
            anim.SetBool("dead",true);
            Invoke("gameOver",4);
        }
    }
    public void gameOver()
    {
        
        SceneManager.LoadScene(0);
        //next
    }



    private void getInThecar()
    {      
        if (Vector3.Distance(this.transform.position, car.transform.position)<=4)
        {
            fText.SetActive(true);
            if (Input.GetKeyDown(KeyCode.F))
            {
                carScript.enabled = true;
                carCamera.SetActive(true);               
                
                

                 this.gameObject.SetActive(false);
            }
            
        }
        else
        {
            carScript.enabled = false;

            fText.SetActive(false);
           
        }
    }

    private IEnumerator damageAnimation()
    {
        damagePanel.SetActive(true);
        yield return new WaitForSeconds(1);
        damagePanel.SetActive(false);

    }
}

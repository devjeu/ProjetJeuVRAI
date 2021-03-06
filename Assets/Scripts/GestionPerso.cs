﻿//////////////////////////////////////////
////Philippe Thibeault////////////////////
//////////////////////////////////////////
////Dernière modification : 2018-12-20////
//////////////////////////////////////////
/*Script qui gère toutes les interactions du personnage, soit:
    -La vie
    -Le tir
    -La collision avec les ennemis
    */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GestionPerso : MonoBehaviour
{

    //Variables pour la vie totale et vie actuelle du personnage
    public float vieTotale;
    public float vieActuelle;
    public float degats;

    //Vitesse de tir du personnage
    public float vitesseTir;

    //Image pour afficher la vie restante du personnage
    public Image viePerso;

    //Particule illustrant une balle de fusil
    public GameObject particuleTir;
	public GameObject FusilSon;
    //Game Object du fusil du personnage
    public GameObject fusil;

    //Booléennes pour déterminer si le personnage peut tirer et si l'ennemi est proche
    public bool peutTirer = false;
    private bool estDansZone;

    //Animator du personnage
    private Animator anim;

    void Start ()
    {
        DeplacementPerso.estMort = false;

        vieActuelle = vieTotale;
        anim = GetComponent<Animator>();

        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.name == "SceneDystopia")
        {
            peutTirer = true;
        }
    }
	
	void Update ()
    {
        //Si le personnage n'est pas mort
        if (!DeplacementPerso.estMort)
        {
            //Afficher sa vie actuelle selon sa vie totale
            viePerso.fillAmount = vieActuelle / vieTotale;

            //Si le joueur clique et le personnage peut tirer
            if (Input.GetMouseButtonDown(0) && peutTirer)
            {
                FusilSon.SetActive(true);
                //Instantier une particule positionnée au fusil et tournée au même angle que le personnage
                var clone = Instantiate(particuleTir);
                clone.transform.position = fusil.transform.position;
                clone.transform.localEulerAngles = transform.localEulerAngles;
                //Démarrer l'animation de tir
                anim.SetBool("Tir", true);
                //Activer la particule
                clone.SetActive(true);
                //Empêcher de tirer à nouveau pour un certain délai
                peutTirer = false;
                StartCoroutine("AttenteTir");
                //Détruire la particule après un certain temps
                StartCoroutine(DetruireParticule(clone));
            }
        }
    }

    //Sur collision avec le personnage
    private void OnTriggerEnter(Collider other)
    {
        //Si c'est un ennemi qui n'est pas déjà dans la zone
        if (other.gameObject.tag == "ennemi" && estDansZone == false)
        {
            //Indiquer qu'il est dans la zone et qu'il touche le personnage
            estDansZone = true;
            Ennemis.touchePerso = true;
            //Le faire attaquer à une intervalle fixe
            InvokeRepeating("AttaquePerso", 0, Ennemis.vitesseAttaque);
        }
        // Si le joueur touche l'arme, il peut tirer
        if (other.gameObject.tag == "Arme")
        {
            peutTirer = true;
        }
    }

    //Si l'ennemi s'éloigne du personnage
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "ennemi")
        {
            //Indiquer qu'il a quitté la zone et qu'il ne touche pas le personnage
            estDansZone = false;
            Ennemis.touchePerso = false;
            //Arrêter d'attaquer
            CancelInvoke("AttaquePerso");
        }
    }

    //Attaquer le personnage
    void AttaquePerso()
    {
        //Diminuer la vie du personnage selon les dégâts de l'ennemi
        vieActuelle -= Ennemis.degatEnnemi;
        //S'il n'y a plus de vie
        if (vieActuelle <= 0)
        {
            //Personnage est mort
            DeplacementPerso.estMort = true;
            vieActuelle = 1;
        }
        //Si l'ennemi quitte la zone du personnage, on arrête d'attaquer
        if (!Ennemis.touchePerso)
        {
            CancelInvoke("AttaquePerso");
        }
    }

    //Attendre un certain temps avant de pouvoir tirer encore
    IEnumerator AttenteTir()
    {
        yield return new WaitForSeconds(vitesseTir);
		FusilSon.SetActive (false);
        peutTirer = true;
    }

    //Détruire la particule de tir
    IEnumerator DetruireParticule(GameObject particule)
    {
        yield return new WaitForSeconds(2);
        Destroy(particule);
    }

    //Arrêter l'attaque de l'ennemi
    public void StopInvoke()
    {
        CancelInvoke("AttaquePerso");
    }
    
}

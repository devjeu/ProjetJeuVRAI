﻿//////////////////////////////////////////
////Philippe Thibeault////////////////////
//////////////////////////////////////////
////Dernière modification : 2018-12-20////
//////////////////////////////////////////
/*Script qui gère la collision la collision
entre les particules des ennemis à distance
et le perso. Ce script détecte le lieu de
collision et y génère une nouvelle particule.*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionParticuleEnnemi : MonoBehaviour {

    //Particule de touche d'une balle
    public GameObject particuleHit;

    //Evénements de collision d'une particule
    public List<ParticleCollisionEvent> collisionEvents;

    //Stocker une position
    private Vector3 pos;

    void Start () {
        //Créer une nouvelle liste de collisions
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    //S'il y a une collision d'une particule
    private void OnParticleCollision(GameObject other)
    {
        //Stocker la collision
        int numCollisionEvents = gameObject.GetComponent<ParticleSystem>().GetCollisionEvents(other, collisionEvents);

        int i = 0;

        while (i < numCollisionEvents)
        {
            //Stocker le lieu de la collision
            pos = collisionEvents[i].intersection;
            i++;
        }

        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<GestionPerso>().vieActuelle -= 0.1f;
        }

        var clone = Instantiate(particuleHit);
        clone.transform.position = pos;
        clone.SetActive(true);

        //Détruire la particule de la balle
        Destroy(gameObject);
    }
}

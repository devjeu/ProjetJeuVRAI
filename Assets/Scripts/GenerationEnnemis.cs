﻿//////////////////////////////////////////
////Philippe Thibeault////////////////////
//////////////////////////////////////////
////Dernière modification : 2018-12-20////
//////////////////////////////////////////
/*Script qui gère les vagues et la création
des ennemis ainsi que des failles*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GenerationEnnemis : MonoBehaviour
{
    //Game objects pour stocker les ennemis
    public GameObject EnnemiBleu;
    public GameObject EnnemiJaune;
    public GameObject EnnemiMauve;
    public GameObject EnnemiOrange;
    public GameObject EnnemiRouge;

    //Game object pour stocker le boss
    public GameObject Boss;

    //Game objects pour stocker les ennemis à distance
    public GameObject ChampiBleu;
    public GameObject ChampiRouge;
    public GameObject ChampiVert;

    //Game object pour stocker la faille à instancier
    public GameObject faille;

    //Game objects pour stocker les points d'apparition des ennemis
    public GameObject spawn1;
    public GameObject spawn2;
    public GameObject spawn3;
    public GameObject spawn4;
    public GameObject spawn5;
    public GameObject spawn6;
    public GameObject spawnBoss;

    //Nombres pour déterminer le nombre d'ennemis total ainsi que la vitesse à laquelle ils sont créés
    public int nbEnnemisMax;
    public int spawnRate = 1;

    //Nombre d'ennemis créé
    private int nbEnnemisRange = 0;
    private int nbEnnemisProche = 0;
    private int nbEnnemisRangeTotal = 0;
    private int nbEnnemisProcheTotal = 0;
    public int nbEnnemisTotal;

    private int randEnnemi;

    public static int iNoVague = 8;
    public int iNbEnnemisMorts;
    public bool bossMort = false;

    void Start()
    {
        iNoVague++;
        SetVieEnnemiProche(2);
        SetVieEnnemiRange(1);
        //Instantier des ennemis
        GenererFailles();
        InvokeRepeating("CreationEnnemiSpawn", 1, spawnRate);
    }

    private void Update()
    {
        //Si le boss et tous les ennemis sont morts 
        if (bossMort && iNbEnnemisMorts >= nbEnnemisMax)
        {
            SceneManager.LoadScene(8);
        }
    }

    //Instancier une faille à chaque endroit où il y a des ennemis
    void GenererFailles()
    {
        for (int i = 1; i <= 6; i++)
        {
            var cloneFaille = Instantiate(faille);
            switch (i)
            {
                case 1:
                    cloneFaille.transform.position = spawn1.transform.position;
                    break;
                case 2:
                    cloneFaille.transform.position = spawn2.transform.position;
                    break;
                case 3:
                    cloneFaille.transform.position = spawn3.transform.position;
                    break;
                case 4:
                    cloneFaille.transform.position = spawn4.transform.position;
                    break;
                case 5:
                    cloneFaille.transform.position = spawn5.transform.position;
                    break;
                case 6:
                    cloneFaille.transform.position = spawn6.transform.position;
                    break;
            }
            cloneFaille.SetActive(true);
        }
    }

    //Créer des ennemis
    void CreationEnnemiSpawn()
    {
        var ennemi = "";
        nbEnnemisRangeTotal = nbEnnemisMax * 2 / 5;
        nbEnnemisProcheTotal = nbEnnemisMax - nbEnnemisRangeTotal;

        if (nbEnnemisRange < nbEnnemisRangeTotal && nbEnnemisProche < nbEnnemisProcheTotal)
        {
            randEnnemi = Random.Range(1, 3);

            switch (randEnnemi)
            {
                case 1:
                default:
                    ennemi = "ennemiRange";
                    break;
                case 2:
                    ennemi = "ennemiProche";
                    break;
            }
        }
        else if (nbEnnemisRange >= nbEnnemisRangeTotal && nbEnnemisProche < nbEnnemisProcheTotal)
        {
            ennemi = "ennemiProche";
        }
        else if (nbEnnemisRange < nbEnnemisRangeTotal && nbEnnemisProche >= nbEnnemisProcheTotal)
        {
            ennemi = "ennemiRange";
        }

        //Si le nombre maximal d'ennemis n'est pas atteint
        if (nbEnnemisTotal < nbEnnemisMax)
        {
            var ennemiCree = EnnemiOrange;

            int i = Random.Range(1, 6);
            int j = Random.Range(1, 7);
            int k = Random.Range(1, 4);

            if (ennemi == "ennemiProche")
            {
                //Selon le premier chiffre aléatoire, on créé un ennemi d'une certaine couleur
                switch (i)
                {
                    case 1:
                    default:
                        ennemiCree = EnnemiBleu;
                        break;
                    case 2:
                        ennemiCree = EnnemiJaune;
                        break;
                    case 3:
                        ennemiCree = EnnemiMauve;
                        break;
                    case 4:
                        ennemiCree = EnnemiOrange;
                        break;
                    case 5:
                        ennemiCree = EnnemiRouge;
                        break;
                }
                nbEnnemisProche++;
            }
            else if (ennemi == "ennemiRange")
            {

                //Selon le premier chiffre aléatoire, on créé un ennemi d'une certaine couleur
                switch (k)
                {
                    case 1:
                    default:
                        ennemiCree = ChampiBleu;
                        break;
                    case 2:
                        ennemiCree = ChampiRouge;
                        break;
                    case 3:
                        ennemiCree = ChampiVert;
                        break;
                }
                nbEnnemisRange++;
            }
            
            var clone = Instantiate(ennemiCree);

            //Selon le deuxième chiffre aléatoire, on créé l'ennemi à un certain endroit
            switch (j)
            {
                case 1:
                default:
                    clone.transform.position = spawn1.transform.position;
                    break;
                case 2:
                    clone.transform.position = spawn2.transform.position;
                    break;
                case 3:
                    clone.transform.position = spawn3.transform.position;
                    break;
                case 4:
                    clone.transform.position = spawn4.transform.position;
                    break;
                case 5:
                    clone.transform.position = spawn5.transform.position;
                    break;
                case 6:
                    clone.transform.position = spawn6.transform.position;
                    break;
            }
            //Selon le troisième chiffre aléatoire, on définit une certaine taille et on active l'ennemi
            clone.SetActive(true);

            //Augmenter le nombre d'ennemis créé
            nbEnnemisTotal++;
        }
        //Sinon on en créé plus
        else
        {
            CancelInvoke("CreationEnnemiSpawn");
        }
    }

    //Fonction qui génère la prochaine vague d'ennemis selon les paramètres donnés
    public void ProchaineVague(int nbEnnemisACreer, int vieEnnemisProche, int vieEnnemisRange, float degatEnnemis, float degatEnnemisRange)
    {
        nbEnnemisTotal = 0;
        iNbEnnemisMorts = 0;
        nbEnnemisMax = nbEnnemisACreer;
        SetVieEnnemiProche(vieEnnemisProche);
        SetDegatEnnemiProche(degatEnnemis);
        SetVieEnnemiRange(vieEnnemisRange);
        SetDegatEnnemiRange(degatEnnemisRange);
        GenererFailles();
        InvokeRepeating("CreationEnnemiSpawn", 1, spawnRate);
    }

    public static void SetVieEnnemiProche(int vie)
    {
        Ennemis.vieEnnemi = vie;
    }

    public static void SetDegatEnnemiProche(float degats)
    {
        Ennemis.degatEnnemi = degats;
    }

    public static void SetVieEnnemiRange(int vie)
    {
        EnnemiRange.vieEnnemi = vie;
    }

    public static void SetDegatEnnemiRange(float degats)
    {
        EnnemiRange.degatEnnemi = degats;
    }

    public void GererBoss()
    {
        Boss.transform.position = spawnBoss.transform.position;
        Boss.SetActive(true);
        ProchaineVague(35, 3, 2, 0.2f, 0.3f);
    }
}
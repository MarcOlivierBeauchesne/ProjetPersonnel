using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TacheDestruction : MonoBehaviour
{
    [SerializeField] GameObject[] _tJoint; // tableau des joints a detruire
    [SerializeField] GameObject[] _tMorceaux; // tableau des morceau qui se detache de la machine
    [SerializeField] Sprite[] _tImgMontant; // tableau des image des joints pour la progression
    [SerializeField] int _taskValue; // valeur de la tache
    [Header("Sons")] // identification de la section Sons
    [SerializeField] AudioClip _sonUserJoint; // son quand le joueur use un joint
    [SerializeField] AudioClip _sonBriserJoint; // son quand le joueur brise un joint

    int _clicAmount = 0; // nombre de clic effectue
    int _joint = 0; // indicatif de quel joint le joueur est rendu

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        DesactiverJoint(); // on appel DesactiverJoint
    }

    /// <summary>
    /// fonction qui desactive les joints
    /// </summary>
    private void DesactiverJoint(){
        for (int i = 0; i < _tJoint.Length; i++) // boucle selon la longueur du tableau _tJoint
        {
            _tJoint[i].GetComponent<JointTache>().timer = GetComponentInParent<Tache>().perso.timer; // le timer du JointTache prend la valeur du timer du perso
            if(i == 0){ // si i est egal a 0
                GameObject jointText = _tJoint[i].transform.GetChild(0).gameObject; // jointText prend la valeur du 1er enfant du joint i
                _tJoint[i].SetActive(true); // on active le joint _tJoint[i]
                jointText.GetComponent<TextMeshPro>().text = _tJoint[i].GetComponent<JointTache>().actualClic.ToString(); // on affiche le nombre de clic a faire sur le joint
            }
            else{ // si i est different de 0
                _tJoint[i].SetActive(false); // on desactive _tJoint[i]
            }
        }
    }

    /// <summary>
    /// fonction publique qui use un joint et active d'autre joint
    /// </summary>
    /// <param name="actualClic">nombre de clic restant</param>
    /// <param name="goalClic">objectif de clic</param>
    public void UserJoint(int actualClic, int goalClic){
        GameAudio.instance.JouerSon(_sonUserJoint); // on joue un son quand le joueur use un joint
        _clicAmount++; // on augmente le nombre de clic de 1
        SpriteRenderer spriteJoint = _tJoint[_joint].GetComponent<SpriteRenderer>(); // spriteJoint prend la valeur du SpriteRenderer du joint _tJoint[_joint]
        int tier = goalClic - (goalClic/3); // tier prend la valeur du total moins 1/3
        int tier2 = goalClic - ((goalClic/3) * 2); // tier2 prend la valeur du total moint 2/3
        if(actualClic > tier){ // si actualClic est plus grand que tier
            spriteJoint.sprite = _tImgMontant[0]; // le joint affiche la 1ere image de _tImgMontant
        }
        else if(actualClic > tier2){ // si actualClic est plus grand que tier2
            spriteJoint.sprite = _tImgMontant[1];// le joint affiche la 2em image de _tImgMontant
        }
        else if(actualClic < tier2){ // si actualClic est plus petit que tier2
            spriteJoint.sprite = _tImgMontant[2];// le joint affiche la 3em image de _tImgMontant
            if(actualClic <= 0){ // si actualClic est plus petit ou egal a 0
                _tMorceaux[_joint].GetComponent<Rigidbody2D>().gravityScale = 1; // on ajoute la gravite au morceau _tMorceaux[_joint]
                Destroy(_tJoint[_joint]); // on detruit le morceau _tMorceaux[_joint]
                _joint++; // _joint augmente de 1
                if(_joint<_tJoint.Length){ // si _joint est plus petit que la longueur du tableau _tJoint
                    _tJoint[_joint].SetActive(true); // on active le joint _tJoint[_joint]
                    GameAudio.instance.JouerSon(_sonBriserJoint); // on joue un son quand le joueur brise un joint
                }
                if(_joint >= _tJoint.Length){ // si _joint est plus grand ou egal a la longueur du tableau _tJoint
                    float npGain = GetComponentInParent<Tache>().perso.basicStats.npGain; // npGain prend la valeur de npGain du BasicStats
                    GameAudio.instance.JouerSon(_sonBriserJoint); // on joue un son quand le joueur brise un joint
                    GetComponentInParent<Tache>().FinirTache(_taskValue + (_clicAmount * (int)npGain)); // on demande a Tache dans le parent de terminer la tache et on avoie les points
                }
            }
        }
    }
}

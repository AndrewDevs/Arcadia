using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class seasonsScript : MonoBehaviour
{
    public Material forestGrassMaterial;
    public Material plainsGrassMaterial;
    public Material regularTreeMaterial;
    public Material smallTreeMaterial;
    public Material bigTreeMaterial;
    public Material grassObjectMaterial;


    public float objectiveTime = 0;

    public bool playing = false;

    Color summerForestGrass = new Color32(0, 212, 59, 255);
    Color summerPlainsGrass = new Color32(31, 226, 49, 255);
    Color summerRegularTree = new Color32(26, 128, 37, 255);
    Color summerSmallTree = new Color32(18, 89, 20, 255);
    Color summerBigTree = new Color32(47, 173, 30, 255);
    Color summerGrassObject = new Color32(3, 190, 26, 255);


    Color fallForestGrass = new Color32(106, 159, 52, 255);
    Color fallPlainsGrass = new Color32(140, 188, 79, 255);
    Color fallRegularTree = new Color32(134, 174, 47, 255);
    Color fallSmallTree = new Color32(152, 52, 13, 255);
    Color fallBigTree = new Color32(173, 129, 32, 255);
    Color fallGrassObject = new Color32(118, 190, 3, 255);


    Color winterForestGrass = new Color32(243, 243, 243, 255);
    Color winterPlainsGrass = new Color32(231, 239, 229, 255);
    Color winterRegularTree = new Color32(224, 224, 224, 255);
    Color winterSmallTree = new Color32(201, 210, 201, 255);
    Color winterBigTree = new Color32(219, 219, 219, 255);
    Color winterGrassObject = new Color32(198, 219, 198, 255);

    Color springForestGrass = new Color32(0, 212, 27, 255);
    Color springPlainsGrass = new Color32(10, 210, 21, 255);
    Color springRegularTree = new Color32(36, 174, 51, 255);
    Color springSmallTree = new Color32(12, 166, 16, 255);
    Color springBigTree = new Color32(32, 215, 8, 255);
    Color springGrassObject = new Color32(19, 228, 45, 255);





    public int summerTime;
    public int fallTime;


    // Start is called before the first frame update
    void Start()
    {
        playing = true;
    //    StartCoroutine(objectiveTimeFunction());

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator objectiveTimeFunction()
    {

        while (playing == true)
        {
            yield return new WaitForSeconds(1);
            objectiveTime++;
            if (objectiveTime < 10)
            {
                forestGrassMaterial.color = summerForestGrass;
                plainsGrassMaterial.color = summerPlainsGrass;
                regularTreeMaterial.color = summerRegularTree;
                smallTreeMaterial.color = summerSmallTree;
                bigTreeMaterial.color = summerBigTree;
                grassObjectMaterial.color = summerGrassObject;

            } else if( objectiveTime >= 10 && objectiveTime < 20)
            {
                forestGrassMaterial.color = fallForestGrass;
                plainsGrassMaterial.color = fallPlainsGrass;
                regularTreeMaterial.color = fallRegularTree;
                smallTreeMaterial.color = fallSmallTree;
                bigTreeMaterial.color = fallBigTree;
                grassObjectMaterial.color = fallGrassObject;



            }
            else if (objectiveTime >= 20 && objectiveTime < 30)
            {
                forestGrassMaterial.color = winterForestGrass;
                plainsGrassMaterial.color = winterPlainsGrass;
                regularTreeMaterial.color = winterRegularTree;
                smallTreeMaterial.color = winterSmallTree;
                bigTreeMaterial.color = winterBigTree;
                grassObjectMaterial.color = winterGrassObject;




            }
            else if(objectiveTime >=30 && objectiveTime < 40)
            {
                forestGrassMaterial.color = springForestGrass;
                plainsGrassMaterial.color = springPlainsGrass;
                regularTreeMaterial.color = springRegularTree;
                smallTreeMaterial.color = springSmallTree;
                bigTreeMaterial.color = springBigTree;
                grassObjectMaterial.color = springGrassObject;
            }
            else
            {
                objectiveTime = 0;

            }
        }
    }
}

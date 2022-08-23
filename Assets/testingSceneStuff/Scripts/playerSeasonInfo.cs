using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerSeasonInfo : MonoBehaviour
{
    public Material forestGrassMaterial;
    public Material plainsGrassMaterial;
    public Material regularTreeMaterial;
    public Material smallTreeMaterial;
    public Material bigTreeMaterial;
    public Material grassObjectMaterial;
    public Material redWoodTreeMaterial;
    public Material pineTreeMaterial;
    public Material redWoodChunkGrassMaterial;
    public Material sunFlowerStem;
    public Material sunFlowerLeaf;
    public Material roseStem;
    public Material orangeFlowerStem;


    Color summerForestGrass = new Color32(0, 212, 59, 255);
    Color summerPlainsGrass = new Color32(31, 226, 49, 255);
    Color summerRegularTree = new Color32(26, 128, 37, 255);
    Color summerSmallTree = new Color32(18, 89, 20, 255);
    Color summerBigTree = new Color32(47, 173, 30, 255);
    Color summerGrassObject = new Color32(3, 190, 26, 255);
    Color summerRedWood = new Color32(11, 94, 5, 255);
    Color summerPineTree = new Color32(16, 137, 12, 255);
    Color summerRedWoodGrass = new Color32(17, 185, 24, 255);
    Color summerSunFlowerStem = new Color32(27, 150, 27, 255);
    Color summerSunFlowerLeaf = new Color32(228, 255, 0, 255);
    Color summerRoseStem = new Color32(29, 248, 9, 255);

    Color fallForestGrass = new Color32(106, 159, 52, 255);
    Color fallPlainsGrass = new Color32(140, 188, 79, 255);
    Color fallRegularTree = new Color32(134, 174, 47, 255);
    Color fallSmallTree = new Color32(152, 52, 13, 255);
    Color fallBigTree = new Color32(173, 129, 32, 255);
    Color fallGrassObject = new Color32(118, 190, 3, 255);
    Color fallRedWood = new Color32(208, 112, 40, 255);
    Color fallPineTree = new Color32(231, 197, 14, 255);
    Color fallRedWoodGrass = new Color32(96, 176, 48, 255);
    Color fallSunFlowerStem = new Color32(40, 166, 39, 255);
    Color fallRoseStem = new Color32(16, 142, 12, 255);

    Color winterForestGrass = new Color32(243, 243, 243, 255);
    Color winterPlainsGrass = new Color32(231, 239, 229, 255);
    Color winterRegularTree = new Color32(224, 224, 224, 255);
    Color winterSmallTree = new Color32(201, 210, 201, 255);
    Color winterBigTree = new Color32(219, 219, 219, 255);
    Color winterGrassObject = new Color32(198, 219, 198, 255);
    Color winterRedWood = new Color32(207, 207, 207, 255);
    Color winterPineTree = new Color32(212, 212, 212, 255);
    Color winterRedWoodGrass = new Color32(229, 229, 229, 255);
    Color winterSunFlowerStem = new Color32(226, 231, 225, 255);
    Color winterSunFlowerLeaf = new Color32(251, 255, 215, 255);
    Color winterRoseStem = new Color32(251, 255, 215, 255);

    Color springForestGrass = new Color32(0, 212, 27, 255);
    Color springPlainsGrass = new Color32(10, 210, 21, 255);
    Color springRegularTree = new Color32(36, 174, 51, 255);
    Color springSmallTree = new Color32(12, 166, 16, 255);
    Color springBigTree = new Color32(32, 215, 8, 255);
    Color springGrassObject = new Color32(19, 228, 45, 255);
    Color springRedWood = new Color32(24, 171, 11, 255);
    Color springPineTree = new Color32(47, 193, 10, 255);
    Color springRedWoodGrass = new Color32(44, 203, 3, 255);
    Color springSunFlowerStem = new Color32(27, 150, 27, 255);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeSeason(int objectiveTime)
    {
        if (objectiveTime < 60)
        {
            forestGrassMaterial.color = summerForestGrass;
            plainsGrassMaterial.color = summerPlainsGrass;
            regularTreeMaterial.color = summerRegularTree;
            smallTreeMaterial.color = summerSmallTree;
            bigTreeMaterial.color = summerBigTree;
            grassObjectMaterial.color = summerGrassObject;
            redWoodTreeMaterial.color = summerRedWood;
            pineTreeMaterial.color = summerPineTree;
            redWoodChunkGrassMaterial.color = summerRedWoodGrass;
            sunFlowerStem.color = summerSunFlowerStem;
            sunFlowerLeaf.color = summerSunFlowerLeaf;
            roseStem.color = summerRoseStem;
            orangeFlowerStem.color = summerSunFlowerStem;

        }
        else if (objectiveTime >= 60 && objectiveTime < 120)
        {
            forestGrassMaterial.color = fallForestGrass;
            plainsGrassMaterial.color = fallPlainsGrass;
            regularTreeMaterial.color = fallRegularTree;
            smallTreeMaterial.color = fallSmallTree;
            bigTreeMaterial.color = fallBigTree;
            grassObjectMaterial.color = fallGrassObject;
            redWoodTreeMaterial.color = fallRedWood;
            pineTreeMaterial.color = fallPineTree;
            redWoodChunkGrassMaterial.color = fallRedWoodGrass;
            sunFlowerStem.color = fallSunFlowerStem;
            sunFlowerLeaf.color = summerSunFlowerLeaf;
            roseStem.color = fallRoseStem;
            orangeFlowerStem.color = summerSunFlowerStem;


        }
        else if (objectiveTime >= 120 && objectiveTime < 180)
        {
            forestGrassMaterial.color = winterForestGrass;
            plainsGrassMaterial.color = winterPlainsGrass;
            regularTreeMaterial.color = winterRegularTree;
            smallTreeMaterial.color = winterSmallTree;
            bigTreeMaterial.color = winterBigTree;
            grassObjectMaterial.color = winterGrassObject;
            redWoodTreeMaterial.color = winterRedWood;
            pineTreeMaterial.color = winterPineTree;
            redWoodChunkGrassMaterial.color = winterRedWoodGrass;
            sunFlowerStem.color = winterSunFlowerStem;
            sunFlowerLeaf.color = winterSunFlowerLeaf;
            roseStem.color = winterRoseStem;
            orangeFlowerStem.color = winterSunFlowerStem;


        }
        else if (objectiveTime >= 180 && objectiveTime < 240)
        {
            forestGrassMaterial.color = springForestGrass;
            plainsGrassMaterial.color = springPlainsGrass;
            regularTreeMaterial.color = springRegularTree;
            smallTreeMaterial.color = springSmallTree;
            bigTreeMaterial.color = springBigTree;
            grassObjectMaterial.color = springGrassObject;
            redWoodTreeMaterial.color = springRedWood;
            pineTreeMaterial.color = springPineTree;
            redWoodChunkGrassMaterial.color = springRedWoodGrass;
            sunFlowerStem.color = springSunFlowerStem;
            sunFlowerLeaf.color = summerSunFlowerLeaf;
            roseStem.color = summerRoseStem;
            orangeFlowerStem.color = summerSunFlowerStem;
        }
        else
        {
            objectiveTime = 0;

        }



    }


}

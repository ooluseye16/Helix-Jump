using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelixController : MonoBehaviour
{
    private Vector2 lastTapPos;
    private bool isDragging = false;
    private Vector3 startRotation;

    public Transform topTransform;
    public Transform goalTransform;
    public GameObject helixLevelPrefab;

    public List<Stage> allStages = new List<Stage>();
    private float helixDistance;
    private List<GameObject> spawnLevels = new List<GameObject>();

    // Start is called before the first frame update
    private void Awake()
    {
        startRotation = transform.localEulerAngles;
        helixDistance = topTransform.localPosition.y - (goalTransform.localPosition.y + 0.1f);
        LoadStage(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastTapPos = Input.mousePosition; // Capture initial tap position
            isDragging = true; // Start dragging
        }

        // Step 2: Handle dragging (mouse button is held down)
        if (Input.GetMouseButton(0) && isDragging)
        {
            Vector2 curTapPos = Input.mousePosition;

            if (lastTapPos != Vector2.zero)
            {
                float delta = lastTapPos.x - curTapPos.x; // Calculate movement difference
                transform.Rotate(Vector3.up * delta); // Rotate object based on horizontal movement
            }

            lastTapPos = curTapPos; // Update last tap position
        }

        // Step 3: Handle when the mouse button is released
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false; // Stop dragging
            lastTapPos = Vector2.zero; // Reset last tap position
        }
    }

    public void LoadStage(int stageNumber)
    {
        Stage stage = allStages[Mathf.Clamp(stageNumber, 0, allStages.Count - 1)];

        if (stage == null)
        {
            Debug.LogError("No stage " + stageNumber + " found in all stages List. Are all stages assigned in th list?");
            return;
        }

        // Change color of stage background
        Camera.main.backgroundColor = allStages[stageNumber].stageBackgroundColor;
        //Change color of the ball in the stage
        FindObjectOfType<BallController>().GetComponent<Renderer>().material.color = allStages[stageNumber].stageBallColor;

        //Change color of top transform
        foreach (Transform t in topTransform)
        {
            t.GetComponent<Renderer>().material.color = allStages[stageNumber].stageLevelPartColor;
        }
        //Reset Helix rotation
        transform.localEulerAngles = startRotation;

        //Destroy existing stages
        foreach (GameObject go in spawnLevels)
        {
            Destroy(go);
        }

        //create new level / platforms

        float levelDistance = helixDistance / stage.levels.Count;
        float spawnPosY = topTransform.localPosition.y;

        for (int i = 0; i < stage.levels.Count; i++)
        {
            spawnPosY -= levelDistance;

            //Create levl within scene
            GameObject level = Instantiate(helixLevelPrefab, transform);
            level.transform.localPosition = new Vector3(0, spawnPosY, 0);
            spawnLevels.Add(level);

            //creating the gaps
            int partToDisable = 12 - stage.levels[i].partCount;
            List<GameObject> disabledParts = new();

            while (disabledParts.Count < partToDisable)
            {
                GameObject randomPart = level.transform.GetChild(Random.Range(0, level.transform
                .childCount)).gameObject;
                if (!disabledParts.Contains(randomPart))
                {
                    randomPart.SetActive(false);
                    disabledParts.Add(randomPart);
                }
            }

            List<GameObject> leftParts = new List<GameObject>();

            foreach (Transform t in level.transform)
            {
                t.GetComponent<Renderer>().material.color = allStages[stageNumber].stageLevelPartColor;
                if (t.gameObject.activeInHierarchy)
                {
                    leftParts.Add(t.gameObject);
                }
            }

            List<GameObject> deathParts = new List<GameObject>();
            HashSet<GameObject> usedParts = new HashSet<GameObject>();

            while (deathParts.Count < stage.levels[i].deathPartCount)
            {
                GameObject randomPart = leftParts[Random.Range(0, leftParts.Count)];

                if (usedParts.Add(randomPart)) // Add returns false if the part is already in the HashSet
                {
                    randomPart.AddComponent<DeathPart>();
                    randomPart.GetComponent<Renderer>().material.color = allStages[stageNumber].stageDeathPartColor;
                    deathParts.Add(randomPart);
                }
            }
        }
    }
}

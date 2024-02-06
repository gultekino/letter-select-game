using UnityEngine;

public class SmallGoalGridManager : MonoBehaviour
{
    public GameObject smallGoalGrid;
    public GameObject smallGoalPrefab;
    public int smallGoalGridSize;
    public float smallGoalGridSpacing;
    public float smallGoalGridOffset;
    public float smallGoalGridYOffset;
    [SerializeField] Vector3 smallGoalGridLocation;
    //Return location of the goal grid small size
    
    public void InitializeSmallGoalGrid(string[] goalData)
    {
        //Calculate the size of screen and how much can be fit in the screen
        for (int i = 0; i < smallGoalGridSize; i++)
        {
            for (int j = 0; j < smallGoalGridSize; j++)
            {
                Vector3 smallGoalPosition = new Vector3(i * smallGoalGridSpacing + smallGoalGridOffset, smallGoalGridYOffset, j * smallGoalGridSpacing + smallGoalGridOffset);
                GameObject smallGoal = Instantiate(smallGoalPrefab, smallGoalPosition, Quaternion.identity, smallGoalGrid.transform);
                smallGoal.name = "SmallGoal_" + i + "_" + j;
            }
        }
    }
    
    public Vector3 GetSmallGoalGridLocation(int index)
    {
        return smallGoalGridLocation;
    }
}
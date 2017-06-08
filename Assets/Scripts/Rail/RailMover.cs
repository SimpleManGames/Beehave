using UnityEngine;

public class RailMover : MonoBehaviour
{
    /// <summary>
    /// The reference to a rail object in the scene that we will move along
    /// </summary>
    [SerializeField]
    [Tooltip("The rail that holds the position we will move along")]
    private Rail rail;

    /// <summary>
    /// The style in which we will move along the rail
    /// </summary>
    [SerializeField]
    [Tooltip("How this object moves along the rail")]
    private PlayBack play;

    /// <summary>
    /// The time it takes to move between the nodes using the magnitude from current to next node
    /// </summary>
    [SerializeField]
    [Tooltip("The time it takes to reach the next node")]
    private float speed = 3f;

    /// <summary>
    /// Toggles which way this object moves along the rails
    /// </summary>
    [SerializeField]
    [Tooltip("Should we play in the other direction\nIf Ping Pong is on, this value will toggle back and forth")]
    private bool isReversed;

    /// <summary>
    /// Toggles if we should restart from the beginning for just end at the last node
    /// </summary>
    [SerializeField]
    [Tooltip("Do we go back to the start\nIf not, then we will stop at the last node")]
    private bool isLooping;

    /// <summary>
    /// Toggles if we should reserve the direction when we reach the end
    /// </summary>
    [SerializeField]
    [Tooltip("If this is on then we will reverse the order when we hit the end")]
    private bool pingPong;

    /// <summary>
    /// The current segment we are on
    /// Segment here is defined by which two nodes we are between
    /// </summary>
    private int currentSeg;
    /// <summary>
    /// The position between the two current and next node on a scale on 0-1
    /// </summary>
    private float transition;
    /// <summary>
    /// Indicates if we have finished going to end defined by isReversed
    /// </summary>
    private bool isCompleted;

    public void Update()
    {
        // If we don't have anything to move along then term
        if (!rail)
            return;

        // If we haven't completed
        if (!isCompleted)
            // Play in the direction specified
            Play(!isReversed);
    }

    /// <summary>
    /// Main function for moving this object
    /// </summary>
    private void Play(bool forward = true)
    {
        // Calculate the true speed using the magnitude
        float mag = (rail.nodes[currentSeg + 1].position - rail.nodes[currentSeg].position).magnitude;
        float s = (Time.deltaTime * 1 / mag) * speed;
        transition += (forward) ? s : -s;

        // If at the end
        if (transition > 1)
            PlayForward();
        // Else if at the start
        else if (transition < 0)
            PlayBackwards();

        transform.position = rail.Position(currentSeg, transition, play);
        transform.rotation = rail.Rotation(currentSeg, transition);
    }

    /// <summary>
    /// Function for handling the conditions when moving forwards
    /// </summary>
    private void PlayForward()
    {
        // Reset to the beginning of the segment
        transition = 0;
        // Tick up so we will move to the next node
        currentSeg++;
        // If we arent at the end
        if (currentSeg == rail.nodes.Length - 1)
        {
            if (isLooping)
            {
                if (pingPong)
                {
                    // Set us at the end of the last node and flip our direction
                    transition = 1;
                    currentSeg = rail.nodes.Length - 2;
                    isReversed = !isReversed;
                }
                else
                    // Else, start us at the beginning
                    currentSeg = 0;

                isCompleted = false;
            }
            else
                isCompleted = true;
        }
    }
    /// <summary>
    /// Function for handling the conditions when moving backwards
    /// </summary>
    private void PlayBackwards()
    {
        // Reset to the end of the segment
        transition = 1;
        // Tick down so we will start to move to the previous node
        currentSeg--;
        if (currentSeg == -1)
        {
            if (isLooping)
            {
                if (pingPong)
                {
                    // Set us at the beginning of the first node and flip our direction
                    transition = 0;
                    currentSeg = 0;
                    isReversed = !isReversed;
                }
                else
                    // Else, set up to the end node
                    currentSeg = rail.nodes.Length - 2;

                isCompleted = false;
            }
            else
                isCompleted = true;
        }
    }
}

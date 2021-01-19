using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TextureVisuals : MonoBehaviour
{
    static int WIDTH = 100;
    public Text speedBufferTextbox;

    Texture2D texture;
    RenderTexture renderTexture;
    RenderTextureDescriptor descriptor;

    int[] numset = new int[WIDTH];
    bool runningCoroutine = false;

    int waitIndex = 0;
    static float[] waitTimes = {0.001f, 0.01f, 0.1f, 0.25f, 0.5f, 1.0f};
    WaitForSeconds wait = new WaitForSeconds(waitTimes[0]);

    #region UnityFunctions

    // Awake is called when the engine loads the script instance
    void Awake()
    {
        //assign instance reliant variables
        texture = new Texture2D(WIDTH, WIDTH);
        descriptor = new RenderTextureDescriptor(WIDTH, WIDTH, RenderTextureFormat.ARGB32);
        renderTexture = new RenderTexture(descriptor);
        texture.filterMode = FilterMode.Point;
        GetComponent<Renderer>().material.mainTexture = texture;
        GetComponent<Renderer>().material.mainTextureScale = new Vector2(1.0f, 1.0f);


        //initialize the array to be sorted, with consecutive numbers from 1 to WIDTH
        for (int i = 0; i < numset.Length; i++)
        {
            numset[i] = i + 1;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Print initial text box value and draw the first frame
        speedBufferTextbox.text = "Speed Buffer: " + waitTimes[waitIndex] + "sec";
        DrawSet(texture, numset);
    }

    // Update is called every frame update
    void Update()
    {
        //input controls to Change the speed buffer  
        if (Input.GetKeyDown(KeyCode.UpArrow)) //UP ARROW
        {
            if (waitIndex < waitTimes.Length - 1)
            {
                waitIndex += 1;
                speedBufferTextbox.text = "Speed Buffer: " + waitTimes[waitIndex] +"sec";
            }
            wait = new WaitForSeconds(waitTimes[waitIndex]);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow)) //DOWN ARROW
        {
            if (waitIndex > 0)
            {
                waitIndex -= 1;
                speedBufferTextbox.text = "Speed Buffer: " + waitTimes[waitIndex] + "sec";
            }
            wait = new WaitForSeconds(waitTimes[waitIndex]);
        }
    }

    #endregion

    //Functions to assign to UI buttons in engine
    #region ButtonFunctions 

    public void ShuffleButton()
    {
        if (runningCoroutine == false)
        {
            runningCoroutine = true;
            StartCoroutine( ShuffleSet() );
        }
    }

    public void BubbleButton()
    {
        if (runningCoroutine == false)
        {
            runningCoroutine = true;
            StartCoroutine( BubbleSort() );
        }
    }

    public void ShellButton()
    {
        if (runningCoroutine == false)
        {
            runningCoroutine = true;
            StartCoroutine(ShellSort());
        }
    }

    public void SelectionButton()
    {
        if (runningCoroutine == false)
        {
            runningCoroutine = true;
            StartCoroutine(SelectionSort());
        }
    }

    public void InsertionButton()
    {
        if (runningCoroutine == false)
        {
            runningCoroutine = true;
            StartCoroutine(InsertionSort());
        }
    }

    public void QuicksortButton()
    {
        if (runningCoroutine == false)
        {
            runningCoroutine = true;
            StartCoroutine( QuickSort(0, numset.Length - 1) );
        }
    }

    public void MergeButton()
    {
        if (runningCoroutine == false)
        {
            runningCoroutine = true;
            StartCoroutine( MergeSort() );
        }
    }

    #endregion

    //Algorithms and Algortihm Helper functions
    #region AlgorithmFunctions

    IEnumerator ShuffleSet()
    {
        int[] temp = numset;
        numset = new int[temp.Length];

        for (int i = 0; i < numset.Length; i++)
        {
            int num = Random.Range(0, temp.Length);
            numset[i] = temp[num];
            temp = temp.Except(new int[] { temp[num] }).ToArray();
            DrawSet(texture, numset);
            yield return wait;
        }

        runningCoroutine = false;
    }

    IEnumerator BubbleSort()
    {
        int n = numset.Length;

        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                if (numset[j] > numset[j + 1])
                {
                    int temp = numset[j];
                    numset[j] = numset[j + 1];
                    numset[j + 1] = temp;
                    DrawSet(texture, numset);
                    yield return wait;
                }
            }
        }

        runningCoroutine = false;
    }

    IEnumerator ShellSort()
    {
        for (int gap = numset.Length / 2; gap > 0; gap /= 2)
        {
            for (int i = gap; i < numset.Length; i += 1)
            {
                int temp = numset[i];
                int j;

                for (j = i; j >= gap && numset[j - gap] > temp; j -= gap)
                {
                    numset[j] = numset[j - gap];
                    DrawSet(texture, numset);
                    yield return wait;
                }

                numset[j] = temp;
                DrawSet(texture, numset);
                yield return wait;
            }
        }

        runningCoroutine = false;
    }

    IEnumerator InsertionSort()
    {
        for (int i = 1; i < numset.Length; i++)
        {
            int key = numset[i];
            int j = i - 1;

            while (j >= 0 && numset[j] > key)
            {
                numset[j + 1] = numset[j];
                DrawSet(texture, numset);
                yield return wait;
                j--;
            }
            numset[j + 1] = key;
            DrawSet(texture, numset);
            yield return wait;
        }

        runningCoroutine = false;
    }

    IEnumerator SelectionSort()
    {
        for (int i = 0; i < numset.Length - 1; i++)
        {
            int minimumIndex = i;
            for (int j = i + 1; j < numset.Length; j++)
            {
                if (numset[j] < numset[minimumIndex])
                {
                    minimumIndex = j;
                }
            }
            Swap(ref numset[minimumIndex], ref numset[i]);
            DrawSet(texture, numset);
            yield return wait;
        }
        runningCoroutine = false;
    }

    IEnumerator QuickSort(int low, int high)
    {
        int[] stack = new int[high - low + 1];
        int top = -1;

        stack[++top] = low;
        stack[++top] = high;

        while (top >= 0)
        {
            high = stack[top--];
            low = stack[top--];
            int pivot = numset[high];
            int i = low;

            for (int j = low; j < high; j++)
            {
                if (numset[j] < pivot)
                {
                    Swap(ref numset[i], ref numset[j]);
                    i++;
                    DrawSet(texture, numset);
                    yield return wait;
                }
            }

            Swap(ref numset[i], ref numset[high]);
            pivot = i;
            DrawSet(texture, numset);
            yield return wait;

            if (pivot - 1 > low)
            {
                stack[++top] = low;
                stack[++top] = pivot - 1;
            }

            if (pivot + 1 < high)
            {
                stack[++top] = pivot + 1;
                stack[++top] = high;
            }
        }

        runningCoroutine = false;
    }

    IEnumerator MergeSort()
    {
        int length = 1;

        while (length < numset.Length)
        {
            int i = 0;

            while (i < numset.Length)
            {
                int leftLo = i;
                int leftHi = i + length;
                int rightLo = i + length - 1;
                int rightHi = i + 2 * length - 1;

                if (leftHi >= numset.Length)
                {
                    break;
                }

                if (rightHi >= numset.Length)
                {
                    rightHi = numset.Length - 1;
                }

                int[] temp = Merge(ref numset, leftLo, rightLo, leftHi, rightHi);

                for (int j = 0; j < rightHi - leftLo + 1; j++)
                {
                    numset[i + j] = temp[j];
                    DrawSet(texture, numset);
                    yield return wait;
                }

                i = i + 2 * length;
            }

            length = 2 * length;
        }

        runningCoroutine = false;
    }

    int[] Merge(ref int[] arr, int leftLo, int rightLo, int leftHi, int rightHi)
    {
        int[] temp = new int[arr.Length];
        int i = 0;

        while (leftLo <= rightLo && leftHi <= rightHi)
        {
            if (arr[leftLo] <= arr[leftHi])
            {
                temp[i] = arr[leftLo];
                i++;
                leftLo++;
            }
            else
            {
                temp[i] = arr[leftHi];
                i++;
                leftHi++;
            }
        }

        while (leftLo <= rightLo)
        {
            temp[i] = arr[leftLo];
            i++;
            leftLo++;
        }

        while (leftHi <= rightHi)
        {
            temp[i] = arr[leftHi];
            i++;
            leftHi++;
        }

        return temp;
    }

    #endregion

    //Utility Functions
    #region HelperFunctions

    //Set All pixels of the texture to black
    void DrawClearScreen(Texture2D texture)
    {
        for (int y = 0; y < WIDTH; y++)
        {
            for (int x = 0; x < WIDTH; x++)
            {
                texture.SetPixel(x, y, Color.black);
            }
        }
    }

    //Draw the set of numbers as vertical white bars
    void DrawSet(Texture2D texture, int[] numset)
    {
        RenderTexture.active = renderTexture;

        DrawClearScreen(texture);
        for (int y = 0; y < numset.Length; y++)
        {
            for (int x = 0; x < numset[y]; x++)
            {
                texture.SetPixel(WIDTH - x - 1, y, Color.white);
            }
        }
        texture.Apply();
        RenderTexture.active = null;
    }

    //Swap 2 values directly
    void Swap(ref int x, ref int y)
    {
        int temp = x;
        x = y;
        y = temp;
    }

    #endregion
}

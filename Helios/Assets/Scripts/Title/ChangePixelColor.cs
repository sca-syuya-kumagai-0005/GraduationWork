using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChangePixelColor : MonoBehaviour
{
    [SerializeField] RawImage targetRawImage;
    Texture2D texture;
    //List<PixelVector> values = new List<PixelVector>();
    public struct PixelVector
    {
        public int x, y;
        public PixelVector(int _x, int _y)
        {
            x = _x;
            y = _y;
        }

        public static readonly PixelVector zeroVector = new PixelVector(0, 0);
        public static readonly PixelVector upperVector = new PixelVector(0, 1);
        public static readonly PixelVector rightVector = new PixelVector(1, 0);
        public static readonly PixelVector downVector = new PixelVector(0, -1);
        public static readonly PixelVector leftVector = new PixelVector(-1, 0);

        public static PixelVector zero
        {
            get
            {
                return zeroVector;
            }
        }

        public static PixelVector upper
        {
            get
            {
                return upperVector;
            }
        }

        public static PixelVector right
        {
            get
            {
                return rightVector;
            }
        }

        public static PixelVector down
        {
            get
            {
                return downVector;
            }
        }

        public static PixelVector left
        {
            get
            {
                return leftVector;
            }
        }

        public static PixelVector operator +(PixelVector a, PixelVector b)
        {
            return new PixelVector(a.x + b.x, a.y + b.y);
        }

        public static PixelVector operator -(PixelVector a, PixelVector b)
        {
            return new PixelVector(a.x - b.x, a.y - b.y);
        }

        public static PixelVector operator *(PixelVector a, PixelVector b)
        {
            return new PixelVector(a.x * b.x, a.y * b.y);
        }

        public static PixelVector operator /(PixelVector a, PixelVector b)
        {
            return new PixelVector(a.x / b.x, a.y / b.y);
        }
        public static PixelVector operator -(PixelVector a)
        {
            return new PixelVector(0 - a.x, 0 - a.y);
        }

        public static PixelVector operator *(PixelVector a, int d)
        {
            return new PixelVector(a.x * d, a.y * d);
        }

        public static PixelVector operator *(int d, PixelVector a)
        {
            return new PixelVector(a.x * d, a.y * d);
        }
        public static PixelVector operator /(PixelVector a, int d)
        {
            return new PixelVector(a.x / d, a.y / d);
        }
    }

    enum Direction
    {
        UPPER,
        RIGHT,
        DOWN,
        LEFT,
        MAX,
    }

    //PixelVector[] pixelDirection;

    /// <summary>
    /// ‰º‚©‚ç‘±‚«
    /// </summary>

    List<int> nums = new List<int>();
    int[] pixelDirection;
    int size; //ŽG

    private void Start()
    {
        if (targetRawImage.texture != null)
        {
            Texture2D mainTex = targetRawImage.texture as Texture2D;
            texture = new Texture2D(mainTex.width, mainTex.height);
            texture.SetPixels(mainTex.GetPixels());
        }
        else
        {
            RectTransform myRect = GetComponent<RectTransform>();
            texture = new Texture2D((int)myRect.sizeDelta.x, (int)myRect.sizeDelta.y);
            var data = texture.GetPixelData<Color32>(0);
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = Color.white;
            }
        }
            
        texture.Apply();
        targetRawImage.texture = texture;
        nums.Add(0);
        //values.Add(PixelVector.zero);
        //pixelDirection = new PixelVector[(int)Direction.MAX];
        //pixelDirection[(int)Direction.UPPER] = PixelVector.upper;
        //pixelDirection[(int)Direction.RIGHT] = PixelVector.right;
        //pixelDirection[(int)Direction.DOWN] = PixelVector.down;
        //pixelDirection[(int)Direction.LEFT] = PixelVector.left;
        size = texture.width * texture.height;
        pixelDirection = new int[(int)Direction.MAX];
        pixelDirection[(int)Direction.UPPER] = texture.width;
        pixelDirection[(int)Direction.RIGHT] = 1;
        pixelDirection[(int)Direction.DOWN] = -texture.width;
        pixelDirection[(int)Direction.LEFT] = -1;
    }

    [Button]
    public void OnAnim()
    {
        Debug.Log("clear");
        int process = texture.width + texture.height;
        multipleNum = 15;
        float diray = time / process * multipleNum;
        StartCoroutine(Amim(diray));
    }

    [SerializeField] float time;
    int multipleNum;
    IEnumerator Amim(float diray)
    {
        var pixelData = texture.GetPixelData<Color32>(0);
        for (int i = 0; i < multipleNum; i++)
        {
            List<int> nextNums = new List<int>();
            foreach (var num in nums)
            {
                SearchNextPixel(num, ref nextNums, pixelData);
                pixelData[num] = Color.clear;
            }
            nums.Clear();
            nums = nextNums;
        }
        
        texture.Apply();
        if (nums.Count != 0)
        {
            yield return new WaitForSeconds(diray);
            StartCoroutine(Amim(diray));
        }
    }

    void SearchNextPixel(int pos, ref List<int> list, NativeArray<Color32> pixelData)
    {
        for (int i = 0; i < (int)Direction.MAX; i++)
        {
            int p = pos + pixelDirection[i];
            if (p < 0 || p > size - 1) continue;
            if (((Direction)i == Direction.RIGHT && p % texture.width == 0) || ((Direction)i == Direction.LEFT && p % texture.width == texture.width - 1)) continue;
            if (pixelData[p] == Color.white && !list.Contains(p))
            {
                list.Add(p);
            }
        }
    }

    //IEnumerator Amim()
    //{
    //    for (int i = 0; i < 10; i++)
    //    {
    //        List<PixelVector> nextValues = new List<PixelVector>();
    //        foreach (var value in values)
    //        {
    //            SearchNextPixel(value, ref nextValues);
    //            texture.SetPixel(value.x, value.y, Color.clear);
    //        }
    //        values.Clear();
    //        values = nextValues;
    //    }

    //    texture.Apply();
    //    targetRawImage.texture = texture;
    //    if (values.Count != 0)
    //    {
    //        yield return null;
    //        count++;
    //        StartCoroutine(Amim());
    //    }
    //    else
    //    {
    //        Debug.Log(count);
    //    }
    //}

    //void SearchNextPixel(PixelVector pos, ref List<PixelVector> list)
    //{
    //    for(int i = 0;i < (int)Direction.MAX; i++)
    //    {
    //        PixelVector p = pos + pixelDirection[i];
    //        if (p.x < 0 || p.x > texture.width || p.y < 0 || p.y > texture.height) continue;
    //        if (texture.GetPixel(p.x,p.y) == Color.white && !list.Contains(p))
    //        {
    //            list.Add(p);
    //        }
    //    }
    //}
}

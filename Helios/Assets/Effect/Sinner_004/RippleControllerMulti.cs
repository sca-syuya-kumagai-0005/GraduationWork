using UnityEngine;
using System.Collections;

public class RippleControllerMulti : MonoBehaviour
{
    public Material rippleMaterial;
    public int maxWaves = 10;

    private Vector4[] waveCount;
    [Header("�g��̒��S�_�ݒ�")]
    [SerializeField]private Vector2[] wavePositon;
    private float[] waveStartTimes;
    private int activeCount = 0;

    void Start()
    {
        waveCount = new Vector4[maxWaves];
        waveStartTimes = new float[maxWaves];
        StartCoroutine("Ripple");
    }

    void Update()
    {
        // �}�E�X�N���b�N�Ŕg�ǉ�
        if (Input.GetMouseButtonDown(0))
        {
            AddRipple(Input.mousePosition);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddRipple(new Vector2(Screen.width / 2f, Screen.height / 2f));
        }

        // ���t���[���V�F�[�_�[�ɔ��f
        rippleMaterial.SetInt("_ActiveWaveCount", activeCount);
        rippleMaterial.SetVectorArray("_WaveStartPos", waveCount);
        rippleMaterial.SetFloatArray("_WaveStartTime", waveStartTimes);
    }
    
    void AddRipple(Vector2 screenPos)
    {
        if (activeCount >= maxWaves) activeCount = 0; // �z���p

        Vector2 uv = new Vector2(screenPos.x / Screen.width, screenPos.y / Screen.height);

        uv = new Vector2(1f - uv.x, 1f - uv.y);
        waveCount[activeCount] = new Vector4(uv.x, uv.y, 0, 0);
        waveStartTimes[activeCount] = Time.time;

        activeCount++;
    }

    IEnumerator Ripple()
    {

        while (true)
        {
            // �����_�����W�𐶐�
            Vector2 randomPos = new Vector2(
                Random.Range(0f, Screen.width),
                Random.Range(0f, Screen.height)
            );

            AddRipple(randomPos);

            yield return new WaitForSeconds(0.5f);
        }

    }
}

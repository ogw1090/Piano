using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piano : MonoBehaviour
{
    const int SampleRate = 44100;
    const float BaseFrequency = 440f;
    const int Semitone = 12;
    readonly Queue<AudioSource> m_AudioSources = new Queue<AudioSource>();
    [SerializeField]
    bool m_IsCord;
    [SerializeField]
    bool m_IsCustomCord;
    [SerializeField]
    bool m_IsMajor = true;
    [SerializeField]
    bool m_ManualCord;
    int m_Count = 0;

    [SerializeField]
    Keyboard m_MajorChordKey;
    
    [SerializeField]
    Keyboard m_MinorChordKey;

    void Start()
    {
        var sources = GetComponents<AudioSource>();
        foreach (var source in sources)
        {
            m_AudioSources.Enqueue(source);
        }
    }

    public void OnPressed(int i)
    {
        float[] frequencies;

        m_Count++;

        if (m_ManualCord)
        {
            if(m_MajorChordKey.IsPressed)
            {
                frequencies = new float[] { CalculateFrequency(i), CalculateFrequency(i + 4), CalculateFrequency(i + 7) };
            }
            else if(m_MinorChordKey.IsPressed)
            {
                frequencies = new float[] { CalculateFrequency(i), CalculateFrequency(i + 3), CalculateFrequency(i + 7) };
            }
            else
            {
                frequencies = new float[] { CalculateFrequency(i) };
            }
        }
        else
        {
            if (m_IsCord && !(m_IsCustomCord && m_Count % 3 != 1))
            {
                frequencies = m_IsMajor
                    ? new float[] { CalculateFrequency(i), CalculateFrequency(i + 4), CalculateFrequency(i + 7) }
                    : new float[] { CalculateFrequency(i), CalculateFrequency(i + 3), CalculateFrequency(i + 7) };
            }
            else
            {
                frequencies = new float[] { CalculateFrequency(i) };
            }
        }

        GenerateTone(frequencies);
    }

    void GenerateTone(params float[] frequencies)
    {
        var position = 0;
        AudioClip clip = AudioClip.Create("Sound", SampleRate * 2, 1, SampleRate, true,
            data =>
            {
                for (int count = 0; count < data.Length; count++, position++)
                {
                    float t = (float)position / SampleRate;
                    data[count] = GenerateSample(frequencies, t) * 0.1f * Mathf.Exp(-3 * t);
                }
            },
            newPosition => position = newPosition
        );

        PlayClip(clip);
    }

    float CalculateFrequency(int note)
    {
        return BaseFrequency * Mathf.Pow(2, (float)note / Semitone);
    }

    float GenerateSample(float[] frequencies, float t)
    {
        float value = 0f;

        for (int i = 0; i < frequencies.Length; i++)
        {
            value += Mathf.Sin(2 * Mathf.PI * frequencies[i] * t);
        }

        return value;
    }

    void PlayClip(AudioClip clip)
    {
        var source = m_AudioSources.Dequeue();
        source.clip = clip;
        source.Play();
        m_AudioSources.Enqueue(source);
    }
}

// Description:音频管理器  主要是提供外部调用,所以方法按调用权重排序
// Author:WangQiang
// Date:2019/03/25

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LuaFramework
{
    public class SoundManager : MonoBehaviour
    {
        private Hashtable sounds = new Hashtable();
        private static new AudioSource audio;
        private List<AudioSource> extraAudios = null;

        private void Awake()
        {
            audio = GetComponent<AudioSource>();
        }

        public float Play2Dsound(string path, bool isLoop = false)
        {
            if (audio.isPlaying)
            {
                return (Play2DsoundByExtraAudio(path, isLoop));
            }

            if (null != path && "" != path)
            {
                audio.clip = LoadAudioClip(path);
            }

            audio.loop = isLoop;
            audio.Play();
            return audio.clip.length;
        }

        public float Play2DsoundByExtraAudio(string path, bool isLoop = false)
        {
            AudioSource extraAudio = null;
            if (null == extraAudios)
            {
                extraAudio = AddAudio();
            }
            else
            {
                foreach (var tempAudio in extraAudios)
                {
                    if (!tempAudio.isPlaying)
                        extraAudio = tempAudio;
                }
            }
            if (null == extraAudio)
                extraAudio = AddAudio();
            if (null != path && "" != path)
                extraAudio.clip = LoadAudioClip(path);

            extraAudio.loop = isLoop;
            extraAudio.Play();
            return extraAudio.clip.length;
        }

        public void Stop2Dsound()
        {
            audio.Stop();
        }

        public void Pause2Dsound()
        {
            audio.Pause();
        }

        private void Add(string key, AudioClip value)
        {
            if (null != sounds[key] || null == value) return;
            sounds.Add(key, value);
        }

        private AudioClip Get(string key)
        {
            if (null == sounds[key]) return null;
            return sounds[key] as AudioClip;
        }

        private AudioClip LoadAudioClip(string path)
        {
            AudioClip ac = Get(path);
            if (null == ac)
            {
                ac = (AudioClip)Resources.Load(path, typeof(AudioClip));
                Add(path, ac);
            }
            return ac;
        }

        private AudioSource AddAudio()
        {
            AudioSource addAudio = gameObject.AddComponent<AudioSource>();
            if (null == extraAudios)
                extraAudios = new List<AudioSource>();
            extraAudios.Add(addAudio);
            return addAudio;
        }
    }
}
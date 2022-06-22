using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

#region PlayableDirector Summary
/*
 * 1. PlayableDirector
 * Timeline �ν��Ͻ��� TimelineAsset ������ ��ũ�� �����ϴ� Ŭ����
 * ex. "GameObject" ��� ���ӿ�����Ʈ�� PlayableDirector�� �߰��Ͽ� "GameObjectTimeline" Ÿ�Ӷ��� ������ PlayableDirector.Playable�� ����
 * 
 * 2. Bindings
 * Binding ������ ����Ͽ� ���� ���� ������Ʈ�� Ÿ�Ӷ��� ���°� ������ �� �ִ�.
 * Binding ������ Timeline â���� Ʈ���� �ǹ���.
 * Binding ������ �� ���� ���� ���еǴµ�
 * ù��°(�պκ�)���� Ÿ�Ӷ����� ���� Ʈ��. �����ܰ� Ʈ��Ÿ���� ��Ÿ���� �κ�
 * �ι�°(�޺κ�)���� Ʈ���� ����� ���ӿ�����Ʈ
 * 
 * 3. struct PlayableBinding
 * PlayableAsset�� ��¿� ���� ������ �����ϴ� ����ü
 * PlayableAsset�� PlayableBinding���� ����Ͽ� ��� ������ �����Ѵ�.
 * 
 */
#endregion

namespace Study.Cutscene
{
    public class RobotKyleDirector : MonoBehaviour
    {
        public PlayableDirector director;
        public GameObject robotKyle;
        bool _fire1;

        private void Awake()
        {
            director = GetComponent<PlayableDirector>();
        }

        private void Update()
        {
            if (Input.GetButton("Fire1"))
            {
                if (!_fire1)
                {
                    var timelineAsset = director.playableAsset;
                    foreach (var output in timelineAsset.outputs)
                    {
                        // PlayableBinding.sourceObject: PlayableBinding�� Key��
                        // ���� Ʈ���� ����� ������Ʈ�� robotKyle�� ���Ѵ�.
                        director.SetGenericBinding(output.sourceObject, robotKyle);
                        if (director.GetGenericBinding(output.sourceObject))
                            Debug.Log($"{output.streamName}�� {robotKyle}�� ����Ǿ����ϴ�.");
                    }
                    director.Play();
                    _fire1 = true;
                }
            }
        }
    }
}


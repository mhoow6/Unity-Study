using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;

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
        PlayableDirector director;
        public GameObject robotKyle;
        public CinemachineBrain brain;

        public List<CinemachineVirtualCamera> cameras = new List<CinemachineVirtualCamera>();

        bool _fire1;

        private void Awake()
        {
            director = GetComponent<PlayableDirector>();
        }

        private void Start()
        {
            // Binding Clear
            var timelineAsset = director.playableAsset;
            foreach (var output in timelineAsset.outputs)
                director.ClearGenericBinding(output.sourceObject);
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
                        string bindingObjectName = string.Empty;
                        // PlayableBinding.sourceObject: PlayableBinding�� Key��
                        // ���� Ʈ���� ����� ������Ʈ�� robotKyle�� ���Ѵ�.
                        switch (output.streamName)
                        {
                            case "Animation Track":
                                bindingObjectName = robotKyle.name;
                                director.SetGenericBinding(output.sourceObject, robotKyle);
                                break;
                            case "Transform Track":
                                bindingObjectName = robotKyle.name;
                                director.SetGenericBinding(output.sourceObject, robotKyle);
                                break;
                            case "Cinemachine Track":
                                bindingObjectName = brain.name;
                                director.SetGenericBinding(output.sourceObject, brain);
                                break;
                            default:
                                break;
                        }

                        if (director.GetGenericBinding(output.sourceObject))
                            Debug.Log($"{output.streamName}�� {bindingObjectName}�� ����Ǿ����ϴ�.");
                    }
                    director.Play();
                    _fire1 = true;
                }
            }
        }

        public void Signal_0296()
        {
            Debug.Log("02:96�ʿ��� ȣ��");

            cameras[1].gameObject.SetActive(true);
            cameras[1].LookAt = robotKyle.transform;
        }
    }
}


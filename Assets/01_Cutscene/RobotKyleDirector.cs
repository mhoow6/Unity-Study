using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;
using UnityEngine.Timeline;

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
        public RobotKyle robotKyle;
        public CinemachineBrain brain;
        public List<CinemachineVirtualCamera> cameras = new List<CinemachineVirtualCamera>();
        public CinemachineDollyCart VCAM02cart;

        bool _fire1;
        PlayableDirector _director;
        bool[] _signalUsed = new bool[2];

        # region �ƽſ� ������ ���� Transform �ʱ�ȭ�뵵
        AnimationPlayableAsset recorded_0000_0900;
        #endregion


        private void Awake()
        {
            _director = GetComponent<PlayableDirector>();
        }

        private void Start()
        {
            // ���ε� ���� �ʱ�ȭ
            var timelineAsset = _director.playableAsset;
            foreach (var output in timelineAsset.outputs)
                _director.ClearGenericBinding(output.sourceObject);

            // �ƽ� �������� �̺�Ʈ
            _director.stopped += (director) =>
            {
                recorded_0000_0900.position = Vector3.zero;
            };

        }

        private void Update()
        {
            if (Input.GetButton("Fire1"))
            {
                if (!_fire1)
                {
                    var timelineAsset = _director.playableAsset;
                    foreach (var output in timelineAsset.outputs)
                    {
                        string bindingObjectName = string.Empty;

                        // PlayableBinding.sourceObject: PlayableBinding�� Key��
                        // ���� Ʈ���� ����� ������Ʈ�� robotKyle�� ���Ѵ�.
                        switch (output.streamName)
                        {
                            case "Animation Track":
                                bindingObjectName = robotKyle.name;
                                _director.SetGenericBinding(output.sourceObject, robotKyle.gameObject);
                                break;
                            case "Transform Track":
                                bindingObjectName = robotKyle.name;
                                _director.SetGenericBinding(output.sourceObject, robotKyle.gameObject);

                                // ���� �ִ� ���ӿ�����Ʈ�� ���� ��ġ���� �����Ѵ�.
                                var secondTrack = output.sourceObject as AnimationTrack;
                                var secondTrackClips = secondTrack.GetClips();
                                foreach (var clip in secondTrackClips)
                                {
                                    switch (clip.displayName)
                                    {
                                        case "Recorded_0000-0900":
                                            var animationPlayableAsset = clip.asset as AnimationPlayableAsset;

                                            recorded_0000_0900 = animationPlayableAsset;

                                            animationPlayableAsset.position = robotKyle.transform.position;
                                            break;
                                        default:
                                            break;
                                    }
                                }

                                break;
                            case "Cinemachine Track":
                                bindingObjectName = brain.name;
                                _director.SetGenericBinding(output.sourceObject, brain);

                                break;
                            default:
                                break;
                        }

                        if (_director.GetGenericBinding(output.sourceObject))
                            Debug.Log($"{output.streamName}�� {bindingObjectName}�� ����Ǿ����ϴ�.");
                    }
                    _director.Play();
                    _fire1 = true;
                }
            }
        }

        public void Signal_0296()
        {
            if (_signalUsed[0])
                Debug.LogError($"���� �ñ׳��� �̹� ���Ǿ����ϴ�.");

            Debug.Log("02:96�ʿ��� ȣ��");

            cameras[0].gameObject.SetActive(false);
            cameras[1].gameObject.SetActive(true);
            cameras[1].LookAt = robotKyle.head;

            StartCoroutine(VCAM02FollowRobotKyle());

            _signalUsed[0] = true;
        }

        IEnumerator VCAM02FollowRobotKyle()
        {
            Vector3 bias = robotKyle.transform.position;
            float deltaDistance = 0f;
            while (true)
            {
                deltaDistance = Vector3.Distance(bias, robotKyle.transform.position);
                VCAM02cart.m_Position = deltaDistance;

                yield return null;
            }
        }

        public void Signal_0900()
        {
            if (_signalUsed[1])
                Debug.LogError($"���� �ñ׳��� �̹� ���Ǿ����ϴ�.");

            Debug.Log("09:00�ʿ��� ȣ��");

            StopAllCoroutines();

            _signalUsed[1] = true;
        }
    }
}


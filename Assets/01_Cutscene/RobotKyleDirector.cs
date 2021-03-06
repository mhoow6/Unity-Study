using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;
using UnityEngine.Timeline;

#region PlayableDirector Summary
/*
 * 1. PlayableDirector
 * Timeline 인스턴스와 TimelineAsset 사이의 링크를 저장하는 클래스
 * ex. "GameObject" 라는 게임오브젝트에 PlayableDirector를 추가하여 "GameObjectTimeline" 타임라인 에셋을 PlayableDirector.Playable에 연결
 * 
 * 2. Bindings
 * Binding 영역을 사용하여 씬의 게임 오브젝트를 타임라인 에셋과 연결할 수 있다.
 * Binding 영역은 Timeline 창에서 트랙을 의미함.
 * Binding 영역은 두 가지 열로 구분되는데
 * 첫번째(앞부분)열은 타임라인의 에셋 트랙. 아이콘과 트랙타입이 나타나는 부분
 * 두번째(뒷부분)열은 트랙에 연결된 게임오브젝트
 * 
 * 3. struct PlayableBinding
 * PlayableAsset의 출력에 대한 정보를 저장하는 구조체
 * PlayableAsset은 PlayableBinding들을 사용하여 출력 유형을 지정한다.
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

        # region 컷신에 쓰였던 에셋 Transform 초기화용도
        AnimationPlayableAsset recorded_0000_0900;
        #endregion


        private void Awake()
        {
            _director = GetComponent<PlayableDirector>();
        }

        private void Start()
        {
            // 바인딩 정보 초기화
            var timelineAsset = _director.playableAsset;
            foreach (var output in timelineAsset.outputs)
                _director.ClearGenericBinding(output.sourceObject);

            // 컷신 마무리시 이벤트
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

                        // PlayableBinding.sourceObject: PlayableBinding의 Key값
                        // 현재 트랙의 연결된 오브젝트를 robotKyle로 정한다.
                        switch (output.streamName)
                        {
                            case "Animation Track":
                                bindingObjectName = robotKyle.name;
                                _director.SetGenericBinding(output.sourceObject, robotKyle.gameObject);
                                break;
                            case "Transform Track":
                                bindingObjectName = robotKyle.name;
                                _director.SetGenericBinding(output.sourceObject, robotKyle.gameObject);

                                // 씬에 있는 게임오브젝트에 맞춰 위치값을 조정한다.
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
                            Debug.Log($"{output.streamName}에 {bindingObjectName}이 연결되었습니다.");
                    }
                    _director.Play();
                    _fire1 = true;
                }
            }
        }

        public void Signal_0296()
        {
            if (_signalUsed[0])
                Debug.LogError($"현재 시그널은 이미 사용되었습니다.");

            Debug.Log("02:96초에서 호출");

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
                Debug.LogError($"현재 시그널은 이미 사용되었습니다.");

            Debug.Log("09:00초에서 호출");

            StopAllCoroutines();

            _signalUsed[1] = true;
        }
    }
}


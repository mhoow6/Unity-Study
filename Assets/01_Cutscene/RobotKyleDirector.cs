using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;

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
                        // PlayableBinding.sourceObject: PlayableBinding의 Key값
                        // 현재 트랙의 연결된 오브젝트를 robotKyle로 정한다.
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
                            Debug.Log($"{output.streamName}에 {bindingObjectName}이 연결되었습니다.");
                    }
                    director.Play();
                    _fire1 = true;
                }
            }
        }

        public void Signal_0296()
        {
            Debug.Log("02:96초에서 호출");

            cameras[1].gameObject.SetActive(true);
            cameras[1].LookAt = robotKyle.transform;
        }
    }
}


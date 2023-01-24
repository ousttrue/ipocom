using System.Collections.Generic;

namespace Ipocom.SonyMotionFormat
{
    public static class Definition
    {
        public const int BONE_COUNT = 27;

        public static IReadOnlyDictionary<int, UnityEngine.HumanBodyBones> HumanBoneMap = new Dictionary<int, UnityEngine.HumanBodyBones>
        {
            { 0 , UnityEngine.HumanBodyBones.Hips           },
            { 3 , UnityEngine.HumanBodyBones.Spine          },
            { 5 , UnityEngine.HumanBodyBones.Chest          },
            { 6 , UnityEngine.HumanBodyBones.UpperChest     },
            { 8 , UnityEngine.HumanBodyBones.Neck           },
            { 10, UnityEngine.HumanBodyBones.Head            },
            { 11, UnityEngine.HumanBodyBones.LeftShoulder    },
            { 12, UnityEngine.HumanBodyBones.LeftUpperArm    },
            { 13, UnityEngine.HumanBodyBones.LeftLowerArm    },
            { 14, UnityEngine.HumanBodyBones.LeftHand        },
            { 15, UnityEngine.HumanBodyBones.RightShoulder   },
            { 16, UnityEngine.HumanBodyBones.RightUpperArm   },
            { 17, UnityEngine.HumanBodyBones.RightLowerArm   },
            { 18, UnityEngine.HumanBodyBones.RightHand       },
            { 19, UnityEngine.HumanBodyBones.LeftUpperLeg    },
            { 20, UnityEngine.HumanBodyBones.LeftLowerLeg    },
            { 21, UnityEngine.HumanBodyBones.LeftFoot        },
            { 22, UnityEngine.HumanBodyBones.LeftToes     },
            { 23, UnityEngine.HumanBodyBones.RightUpperLeg   },
            { 24, UnityEngine.HumanBodyBones.RightLowerLeg   },
            { 25, UnityEngine.HumanBodyBones.RightFoot       },
            { 26, UnityEngine.HumanBodyBones.RightToes    },
        };
    }
}

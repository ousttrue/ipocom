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

        public static (Bones, Bones)[] HeadTailPairs = new (Bones, Bones)[]{
            (Bones.root, Bones.torso_1),
            (Bones.torso_1, Bones.torso_2),
            (Bones.torso_2, Bones.torso_3),
            (Bones.torso_3, Bones.torso_4),
            (Bones.torso_4, Bones.torso_5),
            (Bones.torso_5, Bones.torso_6),
            (Bones.torso_6, Bones.torso_7),
            (Bones.torso_7, Bones.neck_1),
            (Bones.neck_1, Bones.neck_2),
            (Bones.neck_2, Bones.head),
            // (Bones.head),
            (Bones.l_shoulder, Bones.l_up_arm),
            (Bones.l_up_arm, Bones.l_low_arm),
            (Bones.l_low_arm, Bones.l_hand),
            // (Bones.l_hand),
            (Bones.r_shoulder, Bones.r_up_arm),
            (Bones.r_up_arm, Bones.r_low_arm),
            (Bones.r_low_arm, Bones.r_hand),
            // (Bones.r_hand),
            (Bones.l_up_leg, Bones.l_low_leg),
            (Bones.l_low_leg, Bones.l_foot),
            (Bones.l_foot, Bones.l_toes),
            // (Bones.l_toes),
            (Bones.r_up_leg, Bones.r_low_leg),
            (Bones.r_low_leg, Bones.r_foot),
            (Bones.r_foot, Bones.r_toes),
            // (Bones.r_toes),
        };
    }
}

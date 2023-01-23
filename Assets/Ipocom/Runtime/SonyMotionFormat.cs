using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Ipocom
{
    public static class SonyMotionFormat
    {
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

        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct Transform
        {
            public float f0;
            public float f1;
            public float f2;
            public float f3;
            public float f4;
            public float f5;
            public float f6;

            public Quaternion Rotation()
            {
                return new Quaternion(
                    f0,
                    f1,
                    f2,
                    f3
                );
            }

            public Vector3 Translation()
            {
                return new Vector3(f4, f5, f6);
            }
        }

        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BoneId
        {
            public UInt32 bnid0;
            public UInt16 bnid1;
        }

        // 80 byte
        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct MocopiHeader
        {
            public UInt32 magic;
            public UInt32 head_key;
            public UInt32 head;
            public UInt32 ftyp;
            // 22byte
            public UInt64 ftyp0;
            public UInt64 ftyp1;
            public UInt32 ftyp2;
            public UInt16 ftyp3;
            // 
            public UInt32 vrsn_key;
            public UInt32 vrsn0;
            public byte vrsn1;
            public UInt32 sndf_key;
            public UInt32 sndf;
            public UInt32 ipad_key; // 12
            public UInt64 ipad0;
            public UInt32 ipad1;
            public UInt32 rcvp_key; // 6
            public UInt32 rcvp0;
            public UInt16 rcvp1;
        }

        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct SkeletonHeader
        {
            public MocopiHeader header;
            public UInt32 skdf_key;
            public UInt32 skdf;
            public UInt32 bons;
        }

        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct SkeletonBone
        {
            public UInt32 magic; // 0x38 00 00 00
            public UInt32 bndt_key; // "bndt" => 4byte
            public UInt32 bndt;
            public UInt32 bnid_key; // "bndt" => 6byte
            public BoneId bnid; // BoNe ID ?
            public UInt32 pbid_key; // "pbid" => 6byte
            public BoneId pbid; // Parent Bone ID ?
            public UInt32 tran_key;
            public Transform tran;
        }

        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct FrameHeader
        {
            public MocopiHeader header;
            public UInt32 fram_key;
            public UInt32 fram;
            public UInt32 fnum_key;
            public UInt64 fnum;
            public UInt32 time_key;
            public UInt64 time;
            public UInt32 btrs_key;
        }

        // 54
        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct FrameBone
        {
            public UInt32 magic; // 0x2E 00 00 00
            public UInt32 btdt_key;
            public UInt32 btdt;
            public UInt32 bnid_key;
            public BoneId bnid;
            public UInt32 tran_key;
            public Transform tran;
        }

        const int BONE_COUNT = 27;
        static readonly int SKELETON_HEADER_SIZE = Marshal.SizeOf<SkeletonHeader>();
        static readonly int SKELETON_BONE_SIZE = Marshal.SizeOf<SkeletonBone>();
        static readonly int SKELETON_BYTES_SIZE = SKELETON_HEADER_SIZE + BONE_COUNT * SKELETON_BONE_SIZE;
        static readonly int FRAME_HEADER_SIZE = Marshal.SizeOf<FrameHeader>();
        static readonly int FRAME_BONE_SIZE = Marshal.SizeOf<FrameBone>();
        static readonly int FRAME_BYTES_SIZE = FRAME_HEADER_SIZE + BONE_COUNT * FRAME_BONE_SIZE;
        public static object Parse(byte[] bytes)
        {
            if (bytes.Length == SKELETON_BYTES_SIZE)
            {
                var header = new SkeletonHeader[1];
                using (var pin = new ArrayPin(header))
                {
                    Marshal.Copy(bytes, 0, pin.Ptr, SKELETON_HEADER_SIZE);
                }
                var bones = new SkeletonBone[BONE_COUNT];
                using (var pin = new ArrayPin(bones))
                {
                    Marshal.Copy(bytes, SKELETON_HEADER_SIZE, pin.Ptr, BONE_COUNT * SKELETON_BONE_SIZE);
                }
                return new SkeletonMessage
                {
                    header = header[0],
                    bones = bones,
                };
            }
            else if (bytes.Length == FRAME_BYTES_SIZE)
            {
                var header = new FrameHeader[1];
                using (var pin = new ArrayPin(header))
                {
                    Marshal.Copy(bytes, 0, pin.Ptr, FRAME_HEADER_SIZE);
                }
                var bones = new FrameBone[BONE_COUNT];
                using (var pin = new ArrayPin(bones))
                {
                    Marshal.Copy(bytes, FRAME_HEADER_SIZE, pin.Ptr, BONE_COUNT * FRAME_BONE_SIZE);
                }
                return new FrameMessage
                {
                    header = header[0],
                    bones = bones,
                };
            }
            else
            {
                throw new ArgumentException($"{bytes.Length}");
            }
        }
    }
}
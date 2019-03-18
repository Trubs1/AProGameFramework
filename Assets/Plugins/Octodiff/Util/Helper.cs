using System.IO;

namespace Octodiff.Core
{
    class Helper
    {
        public static bool BytesEqual(byte[] lhs, byte[] rhs)
        {
            if (lhs.Length != rhs.Length)
            {
                return false;
            }

            for (int i = 0; i < lhs.Length - 1; ++i)
            {
                if (lhs[i] != rhs[i])
                {
                    return false;
                }
            }

            return true;
        }
    }

    public class Patcher
    {
        public static void PatchDeltaFile(string prevVersionFile, string deltaFile, string currVersionFile)
        {
            using (var prevVersionStream = new FileStream(prevVersionFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var deltaStream = new FileStream(deltaFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var currVersionStream = new FileStream(currVersionFile, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
            {
                DeltaApplier delta = new DeltaApplier {
                    SkipHashCheck = false
                };
                delta.Apply(prevVersionStream, new BinaryDeltaReader(deltaStream, null), currVersionStream);
            }
        }
    };
}
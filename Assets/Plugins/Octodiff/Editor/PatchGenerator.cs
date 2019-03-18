using System.IO;
using System.Text;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Octodiff.Core;
using LuaInterface;

namespace Octodiff.Editor
{
    public class PatchGenerator
    {
        public static string applicationName = "gouzi";
        public static string versionRoot = @"F:\Patch\version_forutil\";
        public static string patchRoot = @"F:\Patch\patch_forserver\";
        public static string prevVersion = null;
        public static string currVersion = null;
        public static string versionManifest = patchRoot + "version.manifest";

        public static string Md5File(string filename)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] digest = md5.ComputeHash(fs);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < digest.Length; i++)
                {
                    sb.Append(digest[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        public static void CopyTree(string srcRoot, string dstRoot)
        {
            string[] files = Directory.GetFiles(srcRoot, "*", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                if (file.EndsWith(".meta") || file.Contains(".DS_Store"))
                {
                    continue;
                }

                string relpath = file.Replace(srcRoot, "");
                string dstfile = dstRoot + "/" + relpath;

                DirectoryInfo dir = new DirectoryInfo(Path.GetDirectoryName(dstfile));
                if (!dir.Exists)
                {
                    dir.Create();
                }

                File.Copy(file, dstfile);
            }
        }

        [MenuItem("Octodiff/GeneratePatch", false, 100)]
        public static void GeneratePatch()
        {
            string assetsPath = Application.streamingAssetsPath;
            currVersion = File.ReadAllText(assetsPath + "/PatchIndex.txt");
            if (string.IsNullOrEmpty(currVersion))
            {
                Debugger.LogError("please set current version:" + assetsPath + "/PatchIndex.txt");
                return;
            }

            List<string> versionList = null;
            if (File.Exists(versionManifest))
            {
                versionList = new List<string>(File.ReadAllLines(versionManifest));
                if (versionList.Contains(currVersion))
                {
                    Debugger.LogError(string.Format("{0} is already a history version number", currVersion));
                    return;
                }
            }

            string versionPath = versionRoot + currVersion;
            if (Directory.Exists(versionPath))
            {
                Directory.Delete(versionPath, true);
            }

            Directory.CreateDirectory(versionPath);
            CopyTree(assetsPath, versionPath);

            if (versionList == null)
            {
                using (StreamWriter manifestWriter = new StreamWriter(versionManifest))
                {
                    manifestWriter.Write(currVersion);
                }
                Debug.Log(string.Format("<color=yellow>the first version generaed success !</color>"));
                //Debugger.LogError("this is the first version, not patch file generaed!");
                return;
            }

            for (int i = versionList.Count - 1; i >= 0; --i)
            {
                string version = versionList[i];
                if (!string.IsNullOrEmpty(version))
                {
                    prevVersion = version;
                    break;
                }
            }

            if (string.IsNullOrEmpty(prevVersion))
            {
                Debugger.LogError("failed to find prev version!");
                return;
            }

            string prevVersionPath = versionRoot + prevVersion + "/";
            string currVersionPath = versionRoot + currVersion + "/";

            Debugger.Log(string.Format("start generate patch {0} -> {1} ",
                prevVersionPath, currVersionPath));

            string patchPath = string.Format("{0}/{1}/{2}-{3}/", Path.GetTempPath(),
                applicationName, prevVersion, currVersion);
            if (Directory.Exists(patchPath))
            {
                Directory.Delete(patchPath, true);
            }
            Directory.CreateDirectory(patchPath);

            List<string> delFileList = new List<string>();
            Dictionary<string, string> newFileMd5 = new Dictionary<string, string>();
            Dictionary<string, string> modFileMd5 = new Dictionary<string, string>();

            List<string> prevVersionFileList = new List<string>(
                Directory.GetFiles(prevVersionPath, "*", SearchOption.AllDirectories));
            List<string> currVersionFileList = new List<string>(
                Directory.GetFiles(currVersionPath, "*", SearchOption.AllDirectories));

            foreach (string prevVersionFile in prevVersionFileList)
            {
                string relativePath = prevVersionFile.Replace(prevVersionPath, "");
                if (!currVersionFileList.Contains(currVersionPath + relativePath))
                {
                    delFileList.Add(relativePath);
                    Debugger.Log(string.Format("[remove] {0}", relativePath));
                }
            }

            foreach (string currVersionFile in currVersionFileList)
            {
                string relativePath = currVersionFile.Replace(currVersionPath, "");
                string prevVersionFile = prevVersionPath + relativePath;

                if (prevVersionFileList.Contains(prevVersionFile))
                {

                    string prevFileMd5 = Md5File(prevVersionFile);
                    string currFileMd5 = Md5File(currVersionFile);
                    if (currFileMd5 != prevFileMd5)
                    {
                        string deltaFile = patchPath + relativePath + ".delta";
                        DirectoryInfo parentDir = new FileInfo(deltaFile).Directory;
                        if (!parentDir.Exists)
                        {
                            parentDir.Create();
                        }

                        Debugger.Log(string.Format("[delta] {0}", relativePath));
                        CreateDeltaFile(prevVersionFile, currVersionFile, deltaFile);
                        modFileMd5[relativePath] = currFileMd5;
                    }
                }
                else
                {
                    newFileMd5[relativePath] = Md5File(currVersionFile);
                    string destPath = patchPath + relativePath;
                    DirectoryInfo parentDir = new FileInfo(destPath).Directory;
                    if (!parentDir.Exists)
                    {
                        parentDir.Create();
                    }
                    Debugger.Log(string.Format("[copy] {0}", relativePath));
                    File.Copy(currVersionFile, destPath);
                }
            }

            Debugger.Log(string.Format("write to update.manifest"));
            string manifest = patchPath + "update.manifest";
            using (FileStream of = new FileStream(manifest, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                Encoding encoder = Encoding.UTF8;

                foreach (string filename in delFileList)
                {
                    byte[] bytes = encoder.GetBytes("del | " + filename + "\n");
                    of.Write(bytes, 0, bytes.Length);
                }

                foreach (var node in newFileMd5)
                {
                    byte[] bytes = encoder.GetBytes("new | " + node.Key + " | " + node.Value + "\n");
                    of.Write(bytes, 0, bytes.Length);
                }

                bool isUpdateVersion = true;
                string nodeValue = "";
                foreach (var node in modFileMd5)
                {
                    if (!string.Equals(node.Key, "PatchIndex.txt"))
                    {
                        byte[] bytes = encoder.GetBytes("mod | " + node.Key + " | " + node.Value + "\n");
                        of.Write(bytes, 0, bytes.Length);
                    }
                    else
                    {
                        nodeValue = node.Value;
                        isUpdateVersion = true;
                    }
                }
                if (isUpdateVersion)
                {
                    UnityEngine.Debug.Log("node.Value=" + nodeValue);
                    byte[] bytes = encoder.GetBytes("mod | " + "PatchIndex.txt" + " | " + nodeValue + "\n");
                    of.Write(bytes, 0, bytes.Length);
                }
            }

            string zipFile = patchRoot + "patch" + prevVersion + "-" + currVersion + ".zip";
            Debugger.Log(string.Format("zip patch file {0}", zipFile));

            using (var patchZip = new Ionic.Zip.ZipFile(Encoding.UTF8))
            {
                List<string> fileList = new List<string>(Directory.GetFiles(patchPath, "*", SearchOption.AllDirectories));

                foreach (string fileName in fileList)
                {
                    string relativePath = Path.GetDirectoryName(fileName.Replace(patchPath, ""));
                    patchZip.AddFile(fileName, relativePath);
                }

                patchZip.Save(zipFile);
            }

            using (StreamWriter writer = File.AppendText(versionManifest))
            {
                writer.Write("\n" + currVersion);
            }

            Directory.Delete(patchPath, true);
        }

        public static void CreateDeltaFile(string prevVersionFile, string currVersionFile, string deltaFile)
        {
            string signatureFilePath = Path.GetTempFileName();
            SignatureBuilder signatureBuilder = new SignatureBuilder();
            DeltaBuilder deltaBuilder = new DeltaBuilder();

            using (FileStream prevVersionStream = new FileStream(prevVersionFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (FileStream signatureStream = new FileStream(signatureFilePath, FileMode.Create, FileAccess.Read | FileAccess.Write, FileShare.Read))
            using (FileStream currVersionStream = new FileStream(currVersionFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (FileStream deltaStream = new FileStream(deltaFile, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                signatureBuilder.Build(prevVersionStream, new SignatureWriter(signatureStream));
                signatureStream.Seek(0, SeekOrigin.Begin);
                deltaBuilder.BuildDelta(currVersionStream,
                    new SignatureReader(signatureStream, deltaBuilder.ProgressReporter),
                    new AggregateCopyOperationsDecorator(new BinaryDeltaWriter(deltaStream)));
            }
        }

        public static void PatchDeltaFile(string prevVersionFile, string deltaFile, string currVersionFile)
        {
            using (var prevVersionStream = new FileStream(prevVersionFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var deltaStream = new FileStream(deltaFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var currVersionStream = new FileStream(currVersionFile, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
            {
                DeltaApplier delta = new DeltaApplier
                {
                    SkipHashCheck = false
                };
                delta.Apply(prevVersionStream, new BinaryDeltaReader(deltaStream, null), currVersionStream);
            }
        }
    };
}
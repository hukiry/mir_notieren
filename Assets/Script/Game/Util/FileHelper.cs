using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

/**
 * 文件帮助类
 * @author Hukiry
 */
public class FileHelper
{

    /// <summary>
    /// 删除目录
    /// </summary>
    /// <param name="path"></param>
    public static void DeleteDirectory(string path, bool recursive)
    {
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }
    }

	/// <summary>
	/// 删除指定文件
	/// </summary>
	/// <param name="path"></param>
	public static void DeleteFile(string path) {
		if (File.Exists(path)) {
			File.Delete(path);
		}
	}

	/// <summary>
	/// 删除所有子目录
	/// </summary>
	/// <param name="path"></param>
	public static void DeleteChildrenDirectory(string path, bool recursive)
    {
        string[] dirs = Directory.GetDirectories(path);
        for (int i = 0; i < dirs.Length; i++)
        {
            DeleteDirectory(dirs[i], recursive);
        }
    }

    /// <summary>
    /// 移动文件
    /// </summary>
    /// <param name="sourcePath"></param>
    /// <param name="newPath"></param>
    public static void MoveFile(string sourcePath, string newPath, bool overwrite)
    {
        if (overwrite)
        {
            if (File.Exists(newPath))
            {
                File.Delete(newPath);
            }
        }
        if (File.Exists(sourcePath))
        {
            if (!Directory.Exists(newPath))
            {
                CreateDirectory(newPath);
            }
            File.Move(sourcePath, newPath);
        }
    }

    /// <summary>
    /// 拷贝文件夹
    /// </summary>
    /// <param name="srcPath"></param>
    /// <param name="dstPath"></param>
    /// <param name="filers"></param>
    /// <param name="extskips"></param>
    public static void CopyDirectory(
        string srcPath, string dstPath, string[] skipsDir = null, string[] skipsExt = null)
    {
        DirectoryInfo source = new DirectoryInfo(srcPath);
        DirectoryInfo target = new DirectoryInfo(dstPath);

        if (target.FullName.StartsWith(source.FullName))
        {
            throw new System.Exception("父目录不能拷贝到子目录！");
        }

        if (!source.Exists) return;

        //跳过不做复制的文件夹
        int skipDirLength = skipsDir != null ? skipsDir.Length : 0;
        for (int j = 0; j < skipDirLength; ++j)
        {
            if (srcPath.Contains(skipsDir[j]))
            {
                Debug.LogFormat("skip dir {0}", srcPath); return;
            }
        }

        if (!target.Exists) target.Create();

        int skipExtLength = skipsExt != null ? skipsExt.Length : 0;

        //拷贝当前目录文件
        FileInfo[] files = source.GetFiles();
        for (int i = 0; i < files.Length; ++i)
        {
            bool isSkiped = false;

            //文件后缀过滤
            for (int j = 0; j < skipExtLength; ++j)
            {
                if (files[i].Name.EndsWith(skipsExt[j]))
                {
                    isSkiped = true; break;
                }
            }

            if (isSkiped)
            {
                Debug.LogFormat("skip file {0}", files[i].FullName); continue;
            }

            string path = target.FullName + "/" + files[i].Name;
            path = path.Replace("\\", "/");
            File.Copy(files[i].FullName, path, true);
        }

        //拷贝当前目录下的文件夹
        DirectoryInfo[] dirs = source.GetDirectories();
        for (int j = 0; j < dirs.Length; ++j)
        {
            CopyDirectory(dirs[j].FullName,
                target.FullName + "/" + dirs[j].Name, skipsDir, skipsExt);
        }
    }

    /// <summary>
    /// 拷贝文件夹
    /// </summary>
    /// <param name="srcPath"></param>
    /// <param name="dstPath"></param>
    /// <param name="filers"></param>
    /// <param name="extskips"></param>
    public static void CopyDirectoryAndRename(
        string srcPath, string dstPath, string[] skipsDir = null, string[] skipsExt = null, string suffix = "")
    {
        DirectoryInfo source = new DirectoryInfo(srcPath);
        DirectoryInfo target = new DirectoryInfo(dstPath);

        if (target.FullName.StartsWith(source.FullName))
        {
            throw new System.Exception("父目录不能拷贝到子目录！");
        }

        if (!source.Exists) return;

        //跳过不做复制的文件夹
        int skipDirLength = skipsDir != null ? skipsDir.Length : 0;
        for (int j = 0; j < skipDirLength; ++j)
        {
            if (srcPath.Contains(skipsDir[j]))
            {
                Debug.LogFormat("skip dir {0}", srcPath); return;
            }
        }

        if (!target.Exists) target.Create();

        int skipExtLength = skipsExt != null ? skipsExt.Length : 0;

        //拷贝当前目录文件
        FileInfo[] files = source.GetFiles();
        for (int i = 0; i < files.Length; ++i)
        {
            bool isSkiped = false;

            //文件后缀过滤
            for (int j = 0; j < skipExtLength; ++j)
            {
                if (files[i].Name.EndsWith(skipsExt[j]))
                {
                    isSkiped = true; break;
                }
            }

            if (isSkiped)
            {
                Debug.LogFormat("skip file {0}", files[i].FullName); continue;
            }
            string path = target.FullName + "/" + files[i].Name + suffix;
            path = path.Replace("\\", "/");
            File.Copy(files[i].FullName, path, true);
        }

        //拷贝当前目录下的文件夹
        DirectoryInfo[] dirs = source.GetDirectories();
        for (int j = 0; j < dirs.Length; ++j)
        {
            CopyDirectoryAndRename(dirs[j].FullName,
                target.FullName + "/" + dirs[j].Name, skipsDir, skipsExt, suffix);
        }
    }


    /// <summary>
    /// 拷贝文件夹
    /// </summary>
    /// <param name="srcPath">原始文件夹</param>
    /// <param name="dstPath">目标文件夹</param>
    /// <param name="ext">拷贝指定后缀的文件</param>
    /// <param name="skipsDir">跳过目录</param>
    /// <param name="suffix">替换的后缀名</param>
    public static void CopyDirectoryAndRenameLua(
        string srcPath, string dstPath, string ext, string[] skipsDir = null, string suffix = "")
    {
        DirectoryInfo source = new DirectoryInfo(srcPath);
        DirectoryInfo target = new DirectoryInfo(dstPath);

        if (target.FullName.StartsWith(source.FullName))
        {
            throw new System.Exception("父目录不能拷贝到子目录！");
        }

        if (!source.Exists) return;

        //跳过不做复制的文件夹
        int skipDirLength = skipsDir != null ? skipsDir.Length : 0;
        for (int j = 0; j < skipDirLength; ++j)
        {
            if (srcPath.Contains(skipsDir[j]))
            {
                return;
            }
        }

        FileHelper.CreateDirectory(dstPath);

        //拷贝当前目录文件
        FileInfo[] files = source.GetFiles();
        for (int i = 0; i < files.Length; ++i)
        {
            if (files[i].Name.EndsWith(ext))
            {
                string path = target.FullName + "/" + files[i].Name.Replace(ext, suffix);
                path = path.Replace("\\", "/");
                File.Copy(files[i].FullName, path, true);
            }
        }

        //拷贝当前目录下的文件夹
        DirectoryInfo[] dirs = source.GetDirectories();
        for (int j = 0; j < dirs.Length; ++j)
        {
            CopyDirectoryAndRenameLua(dirs[j].FullName,
                target.FullName + "/" + dirs[j].Name, ext, skipsDir, suffix);
        }
    }

    //复制文件
    public static void CopyFile(string sourceFileName, string destFileName, bool overwrite)
    {
        CreateDirectory(destFileName);
        File.Copy(sourceFileName, destFileName, overwrite);
    }

    /// <summary>
    /// 获取某个目录下所有符合扩展名的文件
    /// </summary>
    /// <param name="path">要查找的目录</param>
    /// <param name="extend">要查找的文件扩展名</param>
    /// <returns></returns>
    public static FileInfo[] FindFiles(string path, string extend)
    {
        DirectoryInfo dic = new DirectoryInfo(path);
        return dic.GetFiles(extend, SearchOption.AllDirectories);
    }

    /// <summary>
    /// 获取某个目录下所有符合扩展名的文件
    /// </summary>
    /// <param name="path">要查找的目录</param>
    /// <param name="extend">要查找的文件扩展名</param>
    /// <returns></returns>
    public static List<FileInfo> FindFiles(string path, string[] extends)
    {
        List<FileInfo> result = new List<FileInfo>();
        DirectoryInfo dic = new DirectoryInfo(path);
        for (int i = 0; i < extends.Length; i++)
        {
            FileInfo[] files = dic.GetFiles(extends[i], SearchOption.AllDirectories);
            result.AddRange(files.ToList());
        }
        return result;
    }

    /// <summary>
	/// 获取某个目录下除指定扩展名外的所有文件
	/// </summary>
	/// <param name="path">要查找的目录</param>
    /// <param name="skipsDir">跳过文件夹</param>
	/// <param name="extend">要剔除的文件扩展名</param>
	/// <returns></returns>
	public static List<FileInfo> FindFiles2(string path, string[] skipsDir = null, string[] extends = null)
    {
        List<FileInfo> files = new List<FileInfo>();
        List<FileInfo> result = new List<FileInfo>();
        GetFile(path, files, skipsDir);

        for (int i = 0; i < files.Count; i++)
        {
            bool has = false;
            for (int j = 0; j < extends.Length; j++)
            {
                has = Path.GetExtension(files[i].FullName) == extends[j];
                if (has)
                {
                    break;
                }
            }
            if (!has)
            {
                result.Add(files[i]);
            }
        }
        return result;
    }


	/// <summary>
	/// 获取某个目录下指定扩展名的所有文件
	/// </summary>
	/// <param name="path">要查找的目录</param>
	/// <param name="skipsDir">跳过文件夹</param>
	/// <param name="extend">要获取的文件扩展名</param>
	/// <returns></returns>
	public static List<FileInfo> FindFiles3(string path, string[] skipsDir = null, string[] extends = null)
	{
		List<FileInfo> files = new List<FileInfo>();
		List<FileInfo> result = new List<FileInfo>();
		GetFile(path, files, skipsDir);

		for (int i = 0; i < files.Count; i++)
		{
			for (int j = 0; j < extends.Length; j++)
			{
				if (files[i].FullName.EndsWith(extends[j]))
				{
					result.Add(files[i]);
				}
			}
		}
		return result;
	}

	/// <summary>
	/// 获取路径下所有文件以及子文件夹中文件
	/// </summary>
	/// <param name="path">全路径根目录</param>
	/// <param name="FileList">存放所有文件的全路径</param>
	/// <returns></returns>
	public static List<FileInfo> GetFile(string path, List<FileInfo> fileList, string[] skipsDir = null)
    {
        //跳过不做处理的文件夹
        int skipDirLength = skipsDir != null ? skipsDir.Length : 0;
        for (int j = 0; j < skipDirLength; j++)
        {
            if (path.Contains(skipsDir[j]))
            {
                return fileList;
            }
        }

        DirectoryInfo dir = new DirectoryInfo(path);
        FileInfo[] fil = dir.GetFiles();
        DirectoryInfo[] dii = dir.GetDirectories();
        foreach (FileInfo f in fil)
        {
            fileList.Add(f);//添加文件路径到列表中
        }
        //获取子文件夹内的文件列表，递归遍历
        foreach (DirectoryInfo d in dii)
        {
            GetFile(d.FullName, fileList, skipsDir);
        }
        return fileList;
    }

    /// <summary>
    /// 删除某个目录下所有符合扩展名的文件
    /// </summary>
    /// <param name="path">要查找的目录</param>
    /// <param name="extend">要查找的文件扩展名</param>
    /// <returns></returns>
    public static void DeleteDirectoryInExtends(string path, string[] extends)
    {
        DirectoryInfo dic = new DirectoryInfo(path);
        foreach(var extend in extends)
        {
            FileInfo[] files = dic.GetFiles(extend, SearchOption.AllDirectories);
            files.ToList().ForEach(fileInfo => File.Delete(fileInfo.FullName));
        }
    }

	/// <summary>
	/// 获取文件名
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	public static string GetFileName(string path)
    {
        string file = path;
        if (file.LastIndexOf('.') >= 0)
            file = file.Substring(0, file.LastIndexOf('.'));
        if (file.LastIndexOf('\\') >= 0)
            file = file.Substring(file.LastIndexOf('\\') + 1);
        if (file.LastIndexOf('/') >= 0)
            file = file.Substring(file.LastIndexOf('/') + 1);
        return file;
    }

    /// <summary>
    /// 获取文件夹名
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string GetFolderName(string path)
    {
        string file = path;
        if (file.LastIndexOf('/') >= 0)//去掉文件名
            file = file.Substring(0, file.LastIndexOf('/'));
        if (file.LastIndexOf('/') >= 0)//截取文件夹名
            file = file.Substring(file.LastIndexOf('/') + 1);
        return file;
    }

    /// <summary>
    /// 获取扩展名   .ab
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static string GetExtend(string path)
    {
        int index = path.LastIndexOf(".");
        if (index != -1)
        {
            return path.Substring(index);
        }
        return path;
    }

    /// <summary>
    /// 去除扩展名
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static string DeleteExtend(string fileName)
    {
        int index = fileName.LastIndexOf(".");
        if (index != -1)
        {
            return fileName.Substring(0, fileName.LastIndexOf("."));
        }
        return fileName;
    }

    /// <summary>
    /// 替换扩展名
    /// </summary>
    /// <returns></returns>
    public static string ReplaceExtend(string fileName, string reExtend)
    {
        int index = fileName.LastIndexOf(".");
        if (index == -1)
        {
            return fileName + reExtend;
        }
        else
        {
            return fileName.Substring(0, index) + reExtend;
        }
    }

    public static string GetAssetPath(string str)
    {
        return str.Substring(str.IndexOf("Assets"), str.Length - str.IndexOf("Assets"));
    }

    /// <summary>
    /// 创建一个文件夹
    /// </summary>
    /// <param name="folder"></param>
    public static void CreateDirectory(string folder)
    {
        if (folder.LastIndexOf('.') != -1)
        {
            folder = folder.Substring(0, folder.LastIndexOf('/'));
        }
        folder = folder.Replace("\\", "/");
        if (folder[folder.Length - 1] != Path.DirectorySeparatorChar)
            folder += Path.DirectorySeparatorChar;
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);
    }

    /// <summary>
    /// 获取文件的创建时间
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    public static DateTime GetFileCreateTime(string filename)
    {
        if (!File.Exists(filename))
            return new DateTime();

        FileInfo fileInfo = new FileInfo(filename);
        return fileInfo.CreationTime;
    }

    /// <summary>
    /// 获取文件的大小
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    public static long GetFileSize(string filename)
    {
        if (!File.Exists(filename))
            return 0;
        FileInfo fileInfo = new FileInfo(filename);
        return fileInfo.Length;
    }

    /// <summary>
    /// 写入文件
    /// </summary>
    /// <param name="path"></param>
    /// <param name="content"></param>
    public static void WriteFile(string path, string content)
    {
        CreateDirectory(path);
        FileInfo file1 = new FileInfo(path);
        StreamWriter sw1 = file1.CreateText();
        sw1.WriteLine(content);
        sw1.Close();
        sw1.Dispose();
    }


    /// <summary>
    /// 拷贝目录+
    /// </summary>
    /// <param name="sourcePath">原始目录</param>
    /// <param name="savePath">目标目录</param>
    /// <param name="skipsExt">过滤掉的后缀文件</param>
    public static void CopyFolder(string sourcePath, string savePath, params string[] skipsExt)
    {
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

        string[] labDirs = Directory.GetDirectories(sourcePath);//目录
        string[] labFiles = Directory.GetFiles(sourcePath);//文件
        if (labFiles.Length > 0)
        {
            int skipExtLength = skipsExt == null ? 0 : skipsExt.Length;

            for (int i = 0; i < labFiles.Length; i++)
            {
                bool isSkiped = false;
                for (int j = 0; j < skipExtLength; ++j)
                {
                    if (labFiles[i].EndsWith(skipsExt[j]))
                    {
                        isSkiped = true; break;
                    }
                }
                if (isSkiped) continue;
                File.Copy(sourcePath + "/" + Path.GetFileName(labFiles[i]), savePath + "/" + Path.GetFileName(labFiles[i]), true);
            }
        }

        if (labDirs.Length > 0)
        {
            for (int j = 0; j < labDirs.Length; j++)
            {
                Directory.GetDirectories(sourcePath + "/" + Path.GetFileName(labDirs[j]));
                //递归调用
                CopyFolder(sourcePath + "/" + Path.GetFileName(labDirs[j]), savePath + "/" + Path.GetFileName(labDirs[j]), skipsExt);
            }
        }
    }


    public static void DeleteDirectory(string path)
    {
        try
        {
            if (!Directory.Exists(path))
            {
                return;
            }
            string[] fileList = Directory.GetFiles(path);
            string[] directoryList = Directory.GetDirectories(path);

            foreach (string dic in directoryList)
            {
                DeleteDirectory(dic);
            }

            foreach (string file in fileList)
            {
                if (Directory.Exists(file))
                {
                    DeleteDirectory(file);
                }
                else
                {
                    File.Delete(file);
                }
            }
            Directory.Delete(path);
        }
        catch (Exception ex)
        {
            Debug.Log("删除文件夹出现异常:" + ex.Message);
        }
    }
}

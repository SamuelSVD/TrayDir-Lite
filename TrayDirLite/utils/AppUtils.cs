using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace TrayDir {
	internal class AppUtils {
		internal static string RUNAS = "runas";
		internal static string CMD = "cmd";
		internal static string EXPLORER = "explorer.exe";
		internal static bool PathIsDirectory(string path) {
			if (path != string.Empty && path != null) {
				try {
					FileAttributes attr = File.GetAttributes(path);
					//detect whether its a directory or file
					return ((attr & FileAttributes.Directory) == FileAttributes.Directory);
				}
				catch {
					return false;
				}
			}
			return false;
		}
		internal static bool PathIsFile(string path) {
			if (path != string.Empty && path != null) {
				try {
					FileAttributes attr = File.GetAttributes(path);
					//detect whether its a directory or file
					return !((attr & FileAttributes.Directory) == FileAttributes.Directory);
				}
				catch {
					return false;
				}
			}
			return false;
		}
		internal static string SplitCamelCase(string input) {
			return Regex.Replace(input, "([A-Z])", " $1", RegexOptions.Compiled).Trim();
		}
		internal static void ProcessStart(string fileName) {
			ProcessStart(fileName, string.Empty);
		}
		internal static void ProcessStart(string fileName, string parameters) {
			ProcessStart(string.Empty, fileName, parameters);
		}
		internal static void ProcessStart(string startingPath, string fileName, string parameters) {
			Process proc = new Process();
			if ((startingPath == null || startingPath == string.Empty)) {
				if (PathIsFile(fileName)) {
					proc.StartInfo.WorkingDirectory = new FileInfo(fileName).Directory.FullName;
				}
			} else {
				proc.StartInfo.WorkingDirectory = startingPath;
			}
			if (PathIsDirectory(fileName)) {
				proc.StartInfo.FileName = EXPLORER;
				proc.StartInfo.Arguments = fileName;
			} else {
				proc.StartInfo.FileName = fileName;
				proc.StartInfo.Arguments = parameters != null ? parameters.Trim() : string.Empty;
			}
			try {
				proc.Start();
			}
			catch (Exception e) {
				MessageBox.Show(String.Format(Properties.Strings.ErrorStartingProcess, fileName, e.Message), Properties.Strings.Form_Error);
			}
		}
		internal static void OpenPath(string path) {
			ProcessStart(path);
		}
		internal static void OpenCmdPath(string path) {
			if (PathIsFile(path)) {
				ProcessStart(new FileInfo(path).Directory.FullName, CMD, string.Empty);
			} else {
				ProcessStart(path, CMD, string.Empty);
			}
		}
		internal static void ExplorePath(string path) {
			if (PathIsFile(path)) {
				ProcessStart(EXPLORER, new FileInfo(path).Directory.FullName);
			} else {
				ProcessStart(EXPLORER, path);
			}
		}
		internal static bool StrToBool(string value) {
			return value == "1" ? true : false;
		}
		internal static void ExportInstance(TrayInstance instance) {
			SaveFileDialog sfd = new SaveFileDialog();
			TrayInstance copy_instance = instance.Copy();
			sfd.Filter = "Tray Instance Export | *.tde";
			sfd.FileName = Regex.Replace(copy_instance.instanceName, @"[^0-9a-zA-Z()_ ]+", "_");
			if (sfd.ShowDialog() == DialogResult.OK) {
				XMLUtils.SaveToFile(copy_instance, sfd.FileName);
				MessageBox.Show(string.Format(Properties.Strings.ExportedTo, sfd.FileName), Properties.Strings.Form_ExportDone);
			}
		}
		internal static void Run(IMenuItem menuItem) {
			if (menuItem.GetType() == typeof(IPathMenuItem)) {
				if (((IPathMenuItem)menuItem).isDir) {
					OpenPath(new DirectoryInfo(((TrayInstancePath)menuItem.Item.TrayInstanceItem).path).FullName);
				} else if (((IPathMenuItem)menuItem).isFile) {
					OpenPath(Path.GetFullPath(((TrayInstancePath)menuItem.Item.TrayInstanceItem).path));
				}
			} else if (menuItem.GetType() == typeof(IWebLinkMenuItem)) {
				if (((IWebLinkMenuItem)menuItem).isWebLink) {
					Run((TrayInstanceWebLink)menuItem.Item.TrayInstanceItem);
				}
			} else if (menuItem.GetType() == typeof(IVirtualFolderMenuItem)) {
				foreach (IMenuItem imi in menuItem.nodeChildren) {
					Run(imi);
				}
			}
		}
		internal static void OpenCmd(IMenuItem menuItem) {
			if (menuItem.GetType() == typeof(IPathMenuItem)) {
				if (((IPathMenuItem)menuItem).isDir) {
					OpenCmdPath(new DirectoryInfo(((TrayInstancePath)menuItem.Item.TrayInstanceItem).path).FullName);
				} else if (((IPathMenuItem)menuItem).isFile) {
					OpenCmdPath(Path.GetFullPath(((TrayInstancePath)menuItem.Item.TrayInstanceItem).path));
				}
			}
		}
		internal static void Run(TrayInstanceNode node) {
			switch (node.type) {
				case TrayInstanceNode.NodeType.Path:
					Run(node.GetPath());
					break;
				case TrayInstanceNode.NodeType.VirtualFolder:
					foreach(TrayInstanceNode childNode in node.children) {
						Run(childNode);
					}
					break;
				case TrayInstanceNode.NodeType.WebLink:
					Run(node.GetWebLink());
					break;
			}
		}
		internal static void Explore(IMenuItem menuItem) {
			if (menuItem.GetType() == typeof(IPathMenuItem)) {
				if (((IPathMenuItem)menuItem).isDir) {
					ExplorePath(new DirectoryInfo(((TrayInstancePath)menuItem.Item.TrayInstanceItem).path).FullName);
				} else if (((IPathMenuItem)menuItem).isFile) {
					ExplorePath(Path.GetFullPath(((TrayInstancePath)menuItem.Item.TrayInstanceItem).path));
				}
			}
		}

		internal static TrayInstance ImportInstance(string path) {
			TrayInstance i = null;
			i = XMLUtils.LoadFromFile<TrayInstance>(path);
			i.FixPaths();
			i.FixNodes();
			i.nodes.SetInstance(i);
			i.nodes.FixChildren();
			return i;
		}
		internal static void Open(IMenuItem menuItem) {
			if (menuItem.GetType() == typeof(IPathMenuItem)) {
				if (((IPathMenuItem)menuItem).isDir && (menuItem.instance.settings.ExploreFoldersInTrayMenu || ((TrayInstancePath)menuItem.Item.TrayInstanceItem).shortcut)) {
					OpenPath(new DirectoryInfo(((TrayInstancePath)menuItem.Item.TrayInstanceItem).path).FullName);
				} else if (((IPathMenuItem)menuItem).isFile) {
					OpenPath(Path.GetFullPath(((TrayInstancePath)menuItem.Item.TrayInstanceItem).path));
				}
			} else if (menuItem.GetType() == typeof(IWebLinkMenuItem)) {
				if (((IWebLinkMenuItem)menuItem).isWebLink) {
					Run((TrayInstanceWebLink)((IWebLinkMenuItem)menuItem).Item.TrayInstanceItem);
				}
			}
		}
		internal static void Run(TrayInstancePath tip) {
			if (tip.isDir) {
				OpenPath(new DirectoryInfo(tip.path).FullName);
			} else if (tip.isFile) {
				OpenPath(Path.GetFullPath(tip.path));
			}
		}
		internal static void Run(TrayInstanceWebLink tiwl) {
			Uri uriResult;
			if (Uri.TryCreate(tiwl.URL, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps)) {
				OpenPath(tiwl.URL);
			}
		}
	}
}

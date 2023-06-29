using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;


namespace TrayDir {
	[XmlRoot(ElementName = "Instance")]
	public class TrayInstance {
		[XmlElement(ElementName = "Settings")]
		public TrayInstanceSettings settings;
		[XmlAttribute]
		public string iconPath;
		[XmlAttribute]
		public string instanceName;
		[XmlAttribute]
		public string ignoreRegex = string.Empty;
		[XmlAttribute]
		public string Version = Assembly.GetEntryAssembly().GetName().Version.ToString();
		[XmlIgnore]
		internal IView view;
		public List<TrayInstancePath> paths = new List<TrayInstancePath>();
		public List<TrayInstanceVirtualFolder> vfolders = new List<TrayInstanceVirtualFolder>();
		public List<TrayInstanceWebLink> weblinks = new List<TrayInstanceWebLink>();
		public TrayInstanceNode nodes = new TrayInstanceNode();

		public byte[] iconData;
		public static string defaultPath = string.Empty;
		public int PathCount { get { return paths.Count; } }
		public int NodeCount { get { int i = 0; i += nodes.NodeCount - 1; return i; } }
		public string[] regexList { get { return ignoreRegex.Split('\r', '\n'); } }
		public TrayInstance() : this("New Instance") { }
		public TrayInstance(string instanceName) : this(instanceName, new TrayInstanceSettings()) {
		}
		public TrayInstance(String instanceName, TrayInstanceSettings settings) {
			this.settings = settings;
			this.instanceName = instanceName;
		}
		public void Repair() {
			// Separate nodes into their types
			List<TrayInstanceNode> pathNodes = new List<TrayInstanceNode>();
			List<TrayInstanceNode> vfolderNodes = new List<TrayInstanceNode>();
			List<TrayInstanceNode> weblinkNodes = new List<TrayInstanceNode>();
			foreach (TrayInstanceNode node in nodes.GetAllChildNodes()) {
				switch (node.type) {
					case TrayInstanceNode.NodeType.Path:
						pathNodes.Add(node);
						break;
					case TrayInstanceNode.NodeType.VirtualFolder:
						vfolderNodes.Add(node);
						break;
					case TrayInstanceNode.NodeType.WebLink:
						weblinkNodes.Add(node);
						break;
				}
			}
			// Link nodes to their corresponding data
			foreach (TrayInstanceNode node in pathNodes) {
				node.__item = paths[node.id];
			}
			foreach (TrayInstanceNode node in vfolderNodes) {
				node.__item = vfolders[node.id];
			}
			foreach (TrayInstanceNode node in weblinkNodes) {
				node.__item = weblinks[node.id];
			}

			// Detect and remove paths that are not used
			List<TrayInstancePath> deletablePaths = paths.FindAll(path => !pathNodes.Exists(pathNode => pathNode.__item == path));
			paths.RemoveAll(path => deletablePaths.Contains(path));

			// Detect and remove paths that are not used
			List<TrayInstanceVirtualFolder> deletableVFolders = vfolders.FindAll(vfolder => !vfolderNodes.Exists(vfolderNode => vfolderNode.__item == vfolder));
			vfolders.RemoveAll(vfolder => deletableVFolders.Contains(vfolder));

			// Detect and remove paths that are not used
			List<TrayInstanceWebLink> deletableWebLinks = weblinks.FindAll(weblink => !weblinkNodes.Exists(weblinkNode => weblinkNode.__item == weblink));
			weblinks.RemoveAll(weblink => deletableWebLinks.Contains(weblink));

			// Fix all IDs
			foreach (TrayInstanceNode node in pathNodes) {
				node.id = paths.IndexOf((TrayInstancePath)node.__item);
			}
			foreach (TrayInstanceNode node in vfolderNodes) {
				node.id = vfolders.IndexOf((TrayInstanceVirtualFolder)node.__item);
			}
			foreach (TrayInstanceNode node in weblinkNodes) {
				node.id = weblinks.IndexOf((TrayInstanceWebLink)node.__item);
			}
		}
		public void FixPaths() {
			if (settings.paths != null) {
				if (settings.paths.Count > 0) {
					foreach (string path in settings.paths) {
						paths.Add(new TrayInstancePath(path));
					}
				}
			}
			settings.paths = null;
		}
		public void FixNodes() {
			if (PathCount != NodeCount) {
				nodes.children.Clear();
				foreach (TrayInstancePath tip in paths) {
					TrayInstanceNode tin = new TrayInstanceNode();
					tin.id = paths.IndexOf(tip);
					tin.type = TrayInstanceNode.NodeType.Path;
					nodes.children.Add(tin);
					tin.parent = nodes;
				}
			}
			nodes.SetInstance(this);
			nodes.FixChildren();
		}
		public TrayInstance Copy() {
			TrayInstance ti = new TrayInstance();
			ti.settings = settings.Copy();
			ti.iconPath = iconPath;
			ti.instanceName = instanceName;
			ti.ignoreRegex = ignoreRegex;
			ti.iconData = iconData;
			foreach (TrayInstancePath tip in paths) {
				ti.paths.Add((TrayInstancePath)tip.Copy());
			}
			foreach (TrayInstanceVirtualFolder tivf in vfolders) {
				ti.vfolders.Add((TrayInstanceVirtualFolder)tivf.Copy());
			}
			ti.nodes = nodes.Copy();
			return ti;
		}
	}
}

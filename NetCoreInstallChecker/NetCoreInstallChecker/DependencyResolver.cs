using System.Collections.Generic;
using System.Linq;
using NetCoreInstallChecker.Interfaces;
using NetCoreInstallChecker.Structs;
using NetCoreInstallChecker.Structs.Config;
using NuGet.Versioning;

namespace NetCoreInstallChecker
{
    /// <summary>
    /// Resolves dependencies for user given frameworks.
    /// </summary>
    public class DependencyResolver
    {
        /// <summary>
        /// The found frameworks.
        /// </summary>
        public FrameworkFinder Finder { get; private set; }

        public DependencyResolver(FrameworkFinder finder)
        {
            Finder = finder;
        }

        /// <summary>
        /// Resolves the dependencies for a given framework.
        /// </summary>
        /// <param name="options">The config options to resolve the dependencies for.</param>
        /// <returns>The results of this dependency search.</returns>
        public DependencySearchResult<FrameworkOptionsTuple, Framework> Resolve(RuntimeOptions options)
        {
            var allFrameworks   = Finder.GetFrameworks();
            var result          = new DependencySearchResult<FrameworkOptionsTuple, Framework>();

            // Create a mapping dictionary of framework to options.
            var nodesDict = new Dictionary<string, List<Node<FrameworkOptionsTuple>>>();
            foreach (var framework in allFrameworks)
            {
                var list     = new List<Node<FrameworkOptionsTuple>>(32);
                var configs  = Finder.GetConfigs(framework);
                
                foreach (var config in configs)
                    list.Add(config);

                nodesDict[framework] = list;
            }

            // Perform graph traversal to find out if all dependencies have been found.
            var tuple = new FrameworkOptionsTuple(null, options, null);
            ResolveRecursive(new List<Node<FrameworkOptionsTuple>>() { tuple }, nodesDict, result);

            return result;
        }

        /// <summary>
        /// Resolves the dependencies for a given framework.
        /// </summary>
        /// <param name="nodes">The nodes to resolve the dependencies for.</param>
        /// <param name="allNodesDict">List of all nodes.</param>
        /// <param name="result">The result of this dependency search.</param>
        private void ResolveRecursive(List<Node<FrameworkOptionsTuple>> nodes,
            Dictionary<string, List<Node<FrameworkOptionsTuple>>> allNodesDict,
            DependencySearchResult<FrameworkOptionsTuple, Framework> result)
        {
            foreach (var node in nodes)
            {
                // Already fully visited, already in list.
                if (node.Visited == Mark.Visited)
                    return;

                // Disallow looping on itself.
                // Not a directed acyclic graph.
                if (node.Visited == Mark.Visiting)
                    return;

                node.Visited = Mark.Visiting;
                ResolveDependency(node, allNodesDict, result);

                if (node.Edges.Count > 0)
                    ResolveRecursive(node.Edges, allNodesDict, result);

                // Set visited and return to next in stack.
                node.Visited = Mark.Visited;
            }
        }

        /// <summary>
        /// Resolves an individual dependency for a given nodes and sets the nodes's edge to that dependency.
        /// </summary>
        /// <param name="node">The nodes to resolve the dependency for.</param>
        /// <param name="allNodesDict">List of all nodes.</param>
        /// <param name="result">The result of this dependency search.</param>
        private void ResolveDependency(Node<FrameworkOptionsTuple> node,
            Dictionary<string, List<Node<FrameworkOptionsTuple>>> allNodesDict,
            DependencySearchResult<FrameworkOptionsTuple, Framework> result)
        {
            // Framework has no config file, i.e. Microsoft.NETCore.App
            if (node.Element.Options == null)
                return;

            var rollForwardPolicy = node.Element.Options.RollForward;
            var frameworks = node.Element.Options.GetAllFrameworks();
            foreach (var dependency in frameworks)
            {
                var policy = RollForwardPolicy.GetPolicy(rollForwardPolicy);

                // Get all candidate dependencies.
                var candidates = new List<Node<FrameworkOptionsTuple>>(allNodesDict.Count);
                var versions = new List<NuGetVersion>(allNodesDict.Count);

                if (allNodesDict.TryGetValue(dependency.Name, out var dependencies))
                {
                    candidates.AddRange(dependencies);
                    versions.AddRange(dependencies.Select(x => x.Element.Framework.NuGetVersion));
                }

                if (RollForwardPolicy.TryGetSupportedVersion(policy, dependency.NuGetVersion, versions,
                    out var supportedVersion))
                {
                    var edge = candidates.First(x => x.Element.Framework.Version == supportedVersion.ToString());
                    node.Edges.Add(edge);
                    result.Dependencies.Add(edge.Element);
                }
                else
                {
                    result.MissingDependencies.Add(dependency);
                }
            }
        }
    }
}

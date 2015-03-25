using System;
using System.Collections.Generic;
using System.Linq;

namespace Liquid.NET.Utils
{

    public class TreeNode<T>
    {

        public TreeNode(T data)
        {
            Data = data;
            Children = new List<TreeNode<T>>();
        }

        public TreeNode<T> Parent { get; private set; }

        public List<TreeNode<T>> Children { get; private set; }

        public T Data { get; private set; }

        public void AddChild(TreeNode<T> childNode)
        {
            childNode.Parent = this;
            Children.Add(childNode);
        }

        public void AddChildren(IEnumerable<TreeNode<T>> children)
        {
            foreach (var child in children)
            {
                AddChild(child);
            }
        }

        public static IEnumerable<TreeNode<T>> FindWhere(
         IEnumerable<TreeNode<T>> nodes,
         Func<T, bool> predicate)
        {
            if (nodes == null)
            {
                return new List<TreeNode<T>>();
            }

            return nodes.SelectMany(node => FindWhere((TreeNode<T>) node, predicate));
        }

        public static IEnumerable<TreeNode<T>> FindWhere(
            TreeNode<T> node,
            Func<T, bool> predicate)
        {
            if (node == null) return new List<TreeNode<T>>();
            IList<TreeNode<T>> matches = new List<TreeNode<T>>();
            if (node.Data != null && predicate(node.Data))
            {
                matches.Add(node);
            }
            return matches;
        }

        public TreeNode<T> this[int i]
        {
            get { return Children[i]; }
        }
    }


}

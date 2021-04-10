using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using JetBrains.Annotations;
using SimpleJSON;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

[ExecuteInEditMode]
public class SMITH_PS_W9 : MonoBehaviour
{

    public class Node
    {
        public List<Node> children;
        public int value;

        public Node(int value, params Node[] children)
        {
            this.value = value;
            this.children = children.ToList();
        }
    }
    
    // write a function that returns true if a tree contains a number, false if it doesn't.
    public bool ContainsNumber(Node root, int number)
    {
        return ContainsNumberRecursive(root, number);
    }

    private bool ContainsNumberRecursive(Node node, int number)
    {
        if (node.value == number)
            return true;

        foreach (Node child in node.children)
        {
            if (ContainsNumberRecursive(child, number))
                return true;
        }
        return false;
    }

    // write a function that returns true if the tree contains duplicates, false if not.
    public bool ContainsDuplicates(Node root) => ContainsDuplicatesRecursive(root, new List<int>());

    private bool CheckContainsNumber(List<int> seenNumbers, int number)
    {
        foreach (int seenInt in seenNumbers)
        {
            if (seenInt == number)
                return true;
        }
        return false;
    }

    private bool ContainsDuplicatesRecursive(Node node, List<int> seenNumbers)
    {
        if (CheckContainsNumber(seenNumbers, node.value))
            return true;

        seenNumbers.Add(node.value);

        foreach (Node child in node.children)
        {
            if (ContainsDuplicatesRecursive(child, seenNumbers))
                return true;
        }
        return false;
    }

    // write a function to add a new node to the tree as a child of a node w/ value 'toAddTo'
    // return false if you can't find the node to add to, true if you successfully add it.
    public bool AddAsChild(Node root, int toAddTo, int numberToAdd)
    {
        return AddAsChildRecursive(root, toAddTo, numberToAdd);
    }

    private bool AddAsChildRecursive(Node node, int toAddTo, int numberToAdd)
    {
        if (node.value == toAddTo)
        {
            node.children.Add(new Node(numberToAdd));
            return true;
        }

        foreach (Node child in node.children)
        {
            if (AddAsChildRecursive(child, toAddTo, numberToAdd))
                return true;
        }
        return false;
    }
    
    // write a function that returns the depth of a number in the tree.  The root should be 0, it's children should
    // be 1, and so on.  Return -1 if it can't find the number in the tree.
    public int DepthOfNumber(Node root, int number)
    {
        return DepthOfNumberRecursive(root, number);
    }

    private int DepthOfNumberRecursive(Node node, int number, int depth = 0)
    {
        if (node.value == number)
            return depth;

        foreach (Node child in node.children)
        {
            if (DepthOfNumberRecursive(child, number, depth + 1) != -1)
                return depth + 1;
        }
        return -1;
    }

    // =========================== DON'T EDIT BELOW THIS LINE =========================== //

    public TextMeshProUGUI left, right;
    
    private void Update()
    {
        var simpleTree = new Node(1, new Node(2, new Node(3), new Node(4)), new Node(5, new Node(6), new Node(7)));
        var treeWithDuplicates = new Node(1, new Node(2, new Node(3), new Node(1)), new Node(5, new Node(6), new Node(7)));

        
        left.text =  "Contains Number\n<align=left>\n";
        left.text += Success(ContainsNumber(simpleTree, 6)) + " returns true correctly.\n";
        left.text += Success(!ContainsNumber(simpleTree, 8)) + " returns false correctly.\n";
        left.text +=  "\n</align>Contains Duplicates\n<align=left>\n";
        left.text += Success(!ContainsDuplicates(simpleTree)) + " correct for tree without duplicates.\n";
        left.text += Success(ContainsDuplicates(treeWithDuplicates)) + " correct for tree with duplicates.\n";

        var addAsChild1 = new Node(10);
        right.text = "Add as Child\n<align=left>\n";
        right.text += Success(!AddAsChild(addAsChild1, 9, 1)) + " add as child works correctly if can't find number.\n";

        var correct = AddAsChild(addAsChild1, 10, 1);
        right.text += Success(correct && addAsChild1.children[0].value == 1) + " adds child correctly.\n";
        
        
        var depthTree1 = new Node(0, new Node(1));
        right.text += "\n</align>Depth of Number\n<align=left>\n";
        right.text += Success(DepthOfNumber(depthTree1, 0) == 0) + " finds depth 0 correctly.\n";
        right.text += Success(DepthOfNumber(depthTree1, 1) == 1) + " finds depth 1 correctly.\n";
        right.text += Success(DepthOfNumber(depthTree1, 2) == -1) + " returns correctly when number not in tree.\n";
    }

    private string Success(bool test)
    {
        return test ? "<color=\"green\">PASS</color>" : "<color=\"red\">FAIL</color>";
    }
}
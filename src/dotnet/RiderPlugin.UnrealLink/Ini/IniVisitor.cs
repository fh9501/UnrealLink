﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;
using RiderPlugin.UnrealLink.Ini.IniLanguage;

namespace RiderPlugin.UnrealLink.Ini
{
    /// <summary>
    /// Class which goes through syntax tree
    /// </summary>
    public class IniVisitor
    {
        private List<IIniCacheBuilder> cacheBuilders = new List<IIniCacheBuilder>();
        private string curSection = "INVALID_SECTION";

        private IFile myFile;
        private FileSystemPath myFilepath;
        
        public IniVisitor()
        {
        }

        /// <summary>
        /// Adds custom cache builder
        /// </summary>
        public void AddCacher(IIniCacheBuilder cacheBuilder)
        {
            cacheBuilders.Add(cacheBuilder);
        }
        
        /// <summary>
        /// Processes file
        /// </summary>
        public void VisitFile(IFile file, FileSystemPath filepath)
        {
            curSection = "INVALID_SECTION";
            myFile = file;
            myFilepath = filepath;
            
            foreach (var node in myFile.Children())
            {
                if (node.NodeType == IniCompositeNodeType.SECTION)
                {
                    curSection = GetSectionHeader(node);
                    
                    foreach (var child in node.Children())
                    {
                        VisitSection(child);
                    }
                }
            }
        }

        private void VisitSection(ITreeNode node)
        {
            if (node.NodeType == IniCompositeNodeType.PROPERTY)
            {
                string curProperty = "INVALID_PROPERTY";
                var op = IniPropertyOperators.New;
                var val = new IniCachedItem();
                
                foreach (var child in node.Children())
                {
                    var curType = child.GetTokenType();
                    if (child.NodeType == IniCompositeNodeType.VAL_OBJECT)
                    {
                        VisitObject(child, val);
                    }
                    else if (curType == IniTokenType.PROP_KEY)
                    {
                        curProperty = child.GetText();
                    }
                    else if (curType == IniTokenType.ADD)
                    {
                        op = IniPropertyOperators.Add;
                    }
                    else if (curType == IniTokenType.ADD_WITH_CHECK)
                    {
                        op = IniPropertyOperators.AddWithCheck;
                    }
                    else if (curType == IniTokenType.RM_LN)
                    {
                        op = IniPropertyOperators.RemoveLn;
                    }
                    else if (curType == IniTokenType.RM_PROP)
                    {
                        op = IniPropertyOperators.RemoveProperty;
                    }
                    else if (curType == IniTokenType.PROP_VAL)
                    {
                        val.Value = child.GetText();
                    }
                    else if (curType == IniTokenType.STR_LITERAL)
                    {
                        val.Value = child.GetText();
                    }
                }

                val.File = myFilepath;

                foreach (var cacheBuilder in cacheBuilders)
                {
                    cacheBuilder.ProcessProperty(myFilepath, curSection,  curProperty, op, val);
                }
            }
        }

        private void VisitObject(ITreeNode node, IniCachedItem obj) 
        {
            foreach (var prop in node.Children())
            {
                if (prop.NodeType == IniCompositeNodeType.PROPERTY)
                {
                    string key = "INVALID_KEY";
                    foreach (var child in prop.Children())
                    {
                        var curType = child.GetTokenType();
                        if (child.NodeType == IniCompositeNodeType.VAL_OBJECT)
                        {
                            var val = new IniCachedItem();
                            VisitObject(child, val);
                            obj.AddValue(key, val);
                        }
                        else if (curType == IniTokenType.OBJECT_KEY)
                        {
                            key = child.GetText();
                        }
                        else if (curType == IniTokenType.OBJECT_VAL || curType == IniTokenType.STR_LITERAL)
                        {
                            var val = new IniCachedItem();
                            val.Value = child.GetText();
                            obj.AddValue(key, val);
                        }
                    }
                }
            }
        }
        
        private string GetSectionHeader(ITreeNode section) {
            if (section.NodeType != IniCompositeNodeType.SECTION)
            {
                throw new InvalidOperationException();
            }

            var sectionHeader = section.Children().First();
            if (sectionHeader.NodeType != IniCompositeNodeType.SECTION_HEADER)
            {
                throw new InvalidOperationException();
            }

            var headerBuilder = new StringBuilder();
            
            foreach (var item in sectionHeader.Children()) 
            {
                if (item.GetTokenType() == IniTokenType.SECTION_NAME || item.GetTokenType() == IniTokenType.WHITESPACE)
                {
                    headerBuilder.Append(item.GetText());
                }
                else if (item.GetTokenType() == IniTokenType.BAD_CHAR)
                {
                    return "INVALID_SECTION";
                }
            }

            return headerBuilder.ToString();
        }
    }
}
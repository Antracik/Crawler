using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MA_Crawler
{
    public partial class Form1 : Form
    {
        private readonly HashSet<string> _allowedTags;
        private readonly HashSet<string> _selfClosingTags;
        private readonly List<TagNode> _scannedNodes;
        private readonly Dictionary<string, WordOccurrence> _wordsCount;
        private readonly HashSet<string> _notAllowed;
        private readonly Dictionary<string, HtmlDocumentInfo> _docs;

        public Form1()
        {
            InitializeComponent();

            _allowedTags = new HashSet<string>();
            _selfClosingTags = new HashSet<string>();
            _scannedNodes = new List<TagNode>();
            _wordsCount = new Dictionary<string, WordOccurrence>();
            _notAllowed = new HashSet<string>();
            _docs = new Dictionary<string, HtmlDocumentInfo>();
            btn_search.Enabled = false;
            btn_clear.Enabled = false;

            foreach (var line in File.ReadAllLines("HtmlTags.txt"))
            {
                _allowedTags.Add(line.ToLower().Trim());
            }

            foreach (var line in File.ReadAllLines("selfClosingTags.txt"))
            {
                _selfClosingTags.Add(line.ToLower().Trim());
            }

            foreach (var line in File.ReadAllLines("notAllowedWords.txt"))
            {
                _notAllowed.Add(line.ToLower().Trim());
            }


        }

        #region TagClass
        public enum TagType
        {
            StartTag,
            EndTag,
            SelfClosing,
            Comment,
            Text
        }

        [DebuggerDisplay("{Name}, {NormalizedName}, {Depth} ,{NodeType}")]
        public class TagNode
        {
            public string Name { get; set; }
            public string NormalizedName
            {
                get
                {
                    string temp = "";
                    if (NodeType != TagType.Text)
                    {
                        foreach (char ch in Name)
                        {
                            if (char.IsLetterOrDigit(ch))
                            {
                                temp += ch;
                            }
                            else if (char.IsWhiteSpace(ch))
                            {
                                break;
                            }
                        }
                    }

                    return temp = string.IsNullOrWhiteSpace(temp) ? Name : temp;
                }
            }

            public string PureTag { get; set; }

            public int TagPosInDocument { get; set; }

            public TagType NodeType { get; set; }

            public int Depth { get; set; }
            public bool IsSelfClosing
            {
                get
                {
                    return (NodeType == TagType.SelfClosing);
                }
            }

            public bool HasStartEndTag { get; set; }

        }

        private string ExtractTag(string tag)
        {
            string result = "";

            if (tag.StartsWith("<") && tag.EndsWith(">"))
            {
                foreach (var temp in tag)
                {
                    if (char.IsWhiteSpace(temp))
                    {
                        result += ">";
                        break;
                    }
                    result += temp;
                }
            }

            return result;
        }

        private bool IsTagSelfClosing(string tag)
        {
            string temp = ExtractTag(tag);

            return _selfClosingTags.Contains(temp);
        }


        private bool VerifyTag(string tag, out string actualTag)
        {
            bool recognized = true;
            actualTag = "";

            string temp = ExtractTag(tag);

            if (string.IsNullOrWhiteSpace(temp))
            {
                recognized = false;

                return recognized;
            }

            if (!_allowedTags.TryGetValue(temp, out actualTag))
            {
                recognized = false;
            }

            //return recognized;
            return true;
        }

        #endregion

        #region Document
        public class HtmlDocumentInfo
        {
            public string Name { get; set; }
            public string Content { get; set; }
            public int WordCount { get; set; }

            public List<TagNode> ParsedContent { get; set; }

            public HtmlDocumentInfo()
            {
            }
        }

        public class WordOccurrence
        {
            public string Word { get; set; }
            public List<DocumentOccurrence> DocOcc { get; set; }

            public override string ToString()
            {
                StringBuilder ret = new StringBuilder();

                ret.Append($"{Word}: {{");

                foreach (var el in DocOcc)
                {
                    ret.Append($"{el.ToString()},");
                }
                ret.Remove(ret.Length - 1, 1);
                ret.Append("}");

                return ret.ToString();
            }
        }

        public class DocumentOccurrence
        {
            public string Document { get; set; }
            public int Count { get; set; }
            public int Pos { get; set; }

            public override string ToString()
            {
                return $" {Document}({Count})";
            }
        }

        #endregion

        private async void btn_start_Click(object sender, EventArgs e)
        {
            string link = textB_link.Text;
            _wordsCount.Clear();
            _docs.Clear();
            _scannedNodes.Clear();
            treeV_crawl.BeginUpdate();
            treeV_crawl.Nodes.Add(link);
            btn_start.Enabled = false;

            await GetDocs(link, treeV_crawl.Nodes[0]);

            treeV_crawl.ExpandAll();
            treeV_crawl.EndUpdate();

            ParseDoc();

            btn_search.Enabled = true;
            btn_clear.Enabled = true;
        }

        private async Task GetDocs(string link, TreeNode treeNode, int depth = 0)
        {
            if (depth == 4)
                return;

            HttpClient httpClient = new HttpClient();

            HttpRequestMessage request =
                   new HttpRequestMessage(HttpMethod.Get,
                      new Uri(link));

            try
            {
                HttpResponseMessage response =
                   await httpClient.SendAsync(request);

                string content = await response.Content.ReadAsStringAsync();

                if (!_docs.ContainsKey(link))
                {
                    _docs.Add(link, new HtmlDocumentInfo() { Name = link, Content = content });

                    _docs[link].ParsedContent = ParseHtml(content, out List<string> links);

                    foreach (string ahref in links)
                    {
                        await GetDocs(ahref, treeNode.Nodes.Add(ahref), depth + 1);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Please check your internet connection!", "Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private List<TagNode> ParseHtml(string htmlDoc, out List<string> links)
        {
            StringBuilder tagTemp = new StringBuilder();
            links = new List<string>();
            bool startTag = false;
            bool text = false;
            int tagPosInDoc = 0;
            bool closingSlash = false;

            List<TagNode> scannedNodes = new List<TagNode>();

            foreach (char ch in htmlDoc)
            {
                tagPosInDoc++;
                if (ch == '<')
                {
                    if (text)
                    {
                        if (!string.IsNullOrWhiteSpace(tagTemp.ToString()))
                        {
                            scannedNodes.Add(new TagNode { Name = tagTemp.ToString(), NodeType = TagType.Text, TagPosInDocument = tagPosInDoc - tagTemp.ToString().Length });
                        }
                        tagTemp.Clear();
                    }
                    if (!startTag)
                    {
                        startTag = true;
                    }
                    tagTemp.Append(ch);
                }
                else if (ch == '>')
                {
                    tagTemp.Append(ch);
                    if (!string.IsNullOrWhiteSpace(tagTemp.ToString()))
                    {
                        if (closingSlash)
                        {
                            string actualTag;
                            if (tagTemp.ToString().StartsWith("<!--") && tagTemp.ToString().EndsWith("-->"))
                            {
                                scannedNodes.Add(new TagNode { Name = tagTemp.ToString(), NodeType = TagType.Comment, TagPosInDocument = tagPosInDoc - tagTemp.ToString().Length });
                            }
                            else if (VerifyTag(tagTemp.ToString().ToLower(), out actualTag) && !IsTagSelfClosing(tagTemp.ToString().ToLower()))
                            {
                                scannedNodes.Add(new TagNode { Name = tagTemp.ToString(), NodeType = TagType.EndTag, PureTag = actualTag, TagPosInDocument = tagPosInDoc - tagTemp.ToString().Length });
                                closingSlash = false;
                            }
                        }
                        else
                        {
                            string actualTag;
                            if (tagTemp.ToString().ToString().StartsWith("<!--") && tagTemp.ToString().EndsWith("-->"))
                            {
                                scannedNodes.Add(new TagNode { Name = tagTemp.ToString(), NodeType = TagType.Comment, TagPosInDocument = tagPosInDoc - tagTemp.ToString().Length });
                            }
                            else if (VerifyTag(tagTemp.ToString().ToLower(), out actualTag))
                            {
                                var tagType = IsTagSelfClosing(tagTemp.ToString().ToLower()) ? TagType.SelfClosing : TagType.StartTag;
                                scannedNodes.Add(new TagNode { Name = tagTemp.ToString(), NodeType = tagType, PureTag = actualTag, TagPosInDocument = tagPosInDoc - tagTemp.ToString().Length });
                            }
                        }
                    }

                    tagTemp.Clear();
                }
                else if (ch == '/' && startTag)
                {
                    if (tagTemp.ToString().Length > 0)
                    {
                        if ('<' == tagTemp.ToString().ElementAt(tagTemp.ToString().Length - 1))
                        {
                            startTag = false;
                            closingSlash = true;
                        }
                        tagTemp.Append(ch);
                    }

                }
                else if (ch == '\n')
                {
                    if (text)
                    {
                        tagTemp.Append(ch);
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    text = true;
                    tagTemp.Append(ch);
                }
            }

            _scannedNodes.AddRange(scannedNodes);

            var linkParser = new Regex(@"<a\s+(?:[^>]*?\s+)?href=([""'])(.*?)\1", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            var nodes = scannedNodes.Where(x => x.NodeType == TagType.StartTag && x.NormalizedName == "a").ToList();

            foreach (var temp in nodes)
            {
                var match = linkParser.Match(temp.Name);
                if (match.Success && (match.Groups[2].Value.EndsWith("html") || match.Groups[2].Value.EndsWith("htm")))
                {
                    links.Add($"{textB_link.Text}/{match.Groups[2].Value}");
                }
            }

            return scannedNodes;
        }

        private void ParseDoc()
        {
            StringBuilder textBuffer = new StringBuilder();
            string temp;
            foreach (var pair in _docs)
            {
                int wordCount = 0;

                foreach (var node in pair.Value.ParsedContent.Where(x => x.NodeType == TagType.Text))
                {
                    int tagPosOffset = 0;
                    foreach (char ch in node.Name)
                    {
                        tagPosOffset++;
                        if (char.IsLetter(ch))
                        {
                            textBuffer.Append(ch);
                        }
                        else
                        {
                            temp = textBuffer.ToString().ToLower();
                            wordCount++;
                            if (!string.IsNullOrWhiteSpace(temp) && !_notAllowed.Contains(temp) && temp.Length > 2)
                            {
                                _wordsCount.TryGetValue(temp, out WordOccurrence word);
                                if (word == null)
                                {
                                    word = new WordOccurrence()
                                    {
                                        Word = temp,
                                        DocOcc = new List<DocumentOccurrence>()
                                    };

                                    word.DocOcc.Add(new DocumentOccurrence() { Count = 1, Document = pair.Key, Pos = (node.TagPosInDocument + tagPosOffset) - temp.Length });
                                }
                                else
                                {
                                    if (!word.DocOcc.Any(x => x.Document == pair.Key))
                                    {
                                        word.DocOcc.Add(new DocumentOccurrence() { Count = 1, Document = pair.Key, Pos = (node.TagPosInDocument + tagPosOffset) - temp.Length });
                                    }
                                    else
                                    {
                                        word.DocOcc.Find(x => x.Document == pair.Key).Count++;
                                    }

                                }
                                _wordsCount[temp] = word;
                            }
                            textBuffer.Clear();
                        }
                    }
                }
                _docs[pair.Key].WordCount = wordCount;
            }
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            string searchText = textB_search.Text.ToLower();
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                var searchWordsList = searchText.Split(' ')
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .ToList();

                int foundCount = 0;

                foreach (string searchWord in searchWordsList)
                {
                    if (_wordsCount.TryGetValue(searchWord, out var word))
                    {
                        foundCount++;
                        if (!dataGV_crawl.Columns.Contains(searchWord))
                        {
                            dataGV_crawl.Columns.Add(new DataGridViewColumn(dataGV_crawl.Columns["col_Document"].CellTemplate) { Name = searchWord, Resizable = DataGridViewTriState.False, HeaderText = searchWord, MinimumWidth = 20, Width = 30 });

                            foreach (var doc in word.DocOcc)
                            {
                                double freq = ((double)doc.Count / (double)_docs[doc.Document].WordCount) * Math.Log(((double)_docs.Count / (double)word.DocOcc.Count));

                                freq *= 100;
                                int temp = (int)freq;
                                freq = (double)temp / 100;

                                if (!(freq >= 0.01d))
                                {
                                    freq = 0.01d;
                                }

                                bool found = false;
                                int rowIndex = 0;

                                for (int i = 0; i < dataGV_crawl.Rows.Count; i++)
                                {
                                    if (doc.Document == dataGV_crawl.Rows[i].Cells[0].Value.ToString())
                                    {
                                        found = true;
                                        rowIndex = i;
                                        break;
                                    }
                                }

                                if (!found)
                                {
                                    rowIndex = dataGV_crawl.Rows.Add();
                                    dataGV_crawl.Rows[dataGV_crawl.Rows.Count - 1].Cells["col_Document"].Value = doc.Document;
                                }
                                dataGV_crawl.Rows[rowIndex].Cells[searchWord].Value = $"C({doc.Count})W({freq})P({doc.Pos})";
                            }
                        }

                        textB_search.Text = "";

                    }
                    else if (searchWordsList.Count == 1)
                    {
                        MessageBox.Show($"Word {textB_search.Text} not found in index!", "Not found in index", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                label_foundCnt.Text = $"Found {foundCount}/{searchWordsList.Count}";
            }
            else
            {
                MessageBox.Show("Plase enter a search word!", "No word entered", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            var tempCol = dataGV_crawl.Columns["col_Document"];
            tempCol.HeaderText = "Document";
            dataGV_crawl.Rows.Clear();
            label_foundCnt.Text = "Found 0/0";
            dataGV_crawl.Columns.Clear();
            dataGV_crawl.Columns.Add(tempCol);
        }

        private void textB_search_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsWhiteSpace(e.KeyChar))
            {
                if (char.IsWhiteSpace(textB_search.Text.ElementAtOrDefault(textB_search.Text.Length - 1)))
                {
                    e.Handled = true;
                }
                else
                {
                    e.Handled = false;
                }
            }
            else
            {
                e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back);
            }
        }
    }
}

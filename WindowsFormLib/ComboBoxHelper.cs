using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
namespace WinFormsLib
{
    public class ComboListBoxHelper
    {
        static public  int GetIndexOf( string item, ComboBox.ObjectCollection comboBoxitems )
        {
            
            int index = 0;
            int i = 0;
            item = item.ToUpper();
            foreach (object o in comboBoxitems)
            {
                string s = o.ToString();
                s = s.ToUpper();
                if (item == s.ToString())
                {
                    index = i;
                    break;
                }
                i++;
            }
            return index;
        }

        static public void FillComboBox(ComboBox control, string[] items)
        {            
            control.Items.Clear();
            control.Items.AddRange(items);            
            control.Refresh();
        }
        static public void FillComboBox(ComboBox control, string[] items,int selectedIndex)
        {
           
            control.Items.Clear();
            control.Items.AddRange(items);
            control.SelectedIndex = selectedIndex;
            control.Refresh();
        }
        static public void FillListBox(ListBox control, string[] items)
        {            
            control.Items.Clear();
            control.Items.AddRange(items);            
            control.Refresh();
        }
        static public void FillListBox(ListBox control, string[] items, int selectedIndex)
        {
            control.Items.Clear();
            control.Items.AddRange(items);
            control.SelectedIndex = selectedIndex;
            control.Refresh();
        }
        static public List<string> GetListFromXMLFile(string fileName, string selectionNode)
        {
            List<string> items = new List<string>();
            try
            { 
                XmlDocument doc = new XmlDocument();
                doc.Load(fileName);
                XmlNodeList itemsList = doc.SelectNodes(selectionNode);           

                foreach (XmlNode item in itemsList)
                {
                    items.Add(item.InnerXml);
                }
                return items;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
           
        }
        static public List<string> GetListFromXMLFile(string fileName, string selectionNode, string attributeName)
        {

            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);
            XmlNodeList itemsList = doc.SelectNodes(selectionNode);
            List<string> items = new List<string>();
            
            foreach (XmlNode item in itemsList)
            {
                
                string name = item.Attributes[attributeName].Value.ToString();
                items.Add(name);
            }
            return items;
        }
    }
}

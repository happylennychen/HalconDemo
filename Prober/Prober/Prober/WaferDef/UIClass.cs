using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Prober.WaferDef {
    public class UIClass
    {

        /// <summary>
        /// 把对象集合写到CSV文件。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="inList"></param>
        /// <returns></returns>
        public static bool WriteListToXml<T>(string path, List<T> inList) where T : class
        {
            try
            {
                Type t = typeof(T);
                var props = t.GetProperties();
                string[] nameList = props.Select(p => p.Name).ToArray();
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(string.Join(",", nameList));
                foreach (var item in inList)
                {
                    string content = string.Empty;
                    for (int i = 0; i < nameList.Length; i++)
                    {
                        content += t.GetProperty(nameList[i]).GetValue(item).ToString() + ",";
                    }
                    sb.AppendLine(content.TrimEnd(','));
                }
                File.WriteAllText(path, sb.ToString());
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        


        /// <summary>
        /// 将一个对象的属性的值复制给另一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t1">源对象</param>
        /// <param name="t2">被赋值对象</param>
        public static void CopyClass<T>(T t1, T t2) where T : class
        {
            Type ty = typeof(T);
            foreach (var item in ty.GetProperties())
            {
                item.SetValue(t2, item.GetValue(t1));
            }
        }

        private static List<Control> GetAllControls(Control ctl)
        {
            List<Control> controls = new List<Control>();
            foreach (Control item in ctl.Controls)
            {
                controls.Add(item);
                if (item.Controls.Count > 0)
                {
                    controls.AddRange(GetAllControls(item));
                }
            }
            return controls;
        }


        /// <summary>
        /// 将控件的信息复制到对象属性中
        /// </summary>
        /// <param name="ctl"></param>
        /// <param name="obj"></param>
        public static void ControlToObject(Control ctl, object obj)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            List<Control> ctlList = GetAllControls(ctl);
            foreach (Control item in ctlList)
            {
                var split = item.Name.Split('_');
                if (split.Length < 2)
                {
                    continue;
                }
                string key = item.Name.Substring(item.Name.IndexOf('_') + 1); //split[1];

                if (item is TextBox txt)
                {
                    dic[key] = txt.Text;
                }
                if (item is Label lbl)
                {
                    dic[key] = lbl.Text;
                }
                else if (item is NumericUpDown nup)
                {
                    dic[key] = nup.Value;
                }
                else if (item is ComboBox cbox)
                {
                    dic[key] = cbox.Text;
                }
                else if (item is CheckBox chbox)
                {
                    dic[key] = chbox.Checked;
                }
                else if (item is RadioButton rbtn)
                {
                    dic[key] = rbtn.Checked;
                }                
            }

            var t = obj.GetType();
            foreach (var item in t.GetProperties())
            {
                if (dic.ContainsKey(item.Name))
                {
                    if (item.PropertyType == typeof(double))
                    {
                        double v = 0;
                        double.TryParse(dic[item.Name].ToString(), out v);
                        item.SetValue(obj, v);
                    }
                    else if (item.PropertyType == typeof(string))
                    {
                        item.SetValue(obj, Convert.ToString(dic[item.Name]));
                    }
                    else if (item.PropertyType == typeof(int))
                    {
                        int v = 0;
                        int.TryParse(dic[item.Name].ToString(), out v);
                        item.SetValue(obj, v);
                    }
                    else if (item.PropertyType == typeof(bool))
                    {
                        bool b = false;
                        Boolean.TryParse(dic[item.Name].ToString(), out b);
                        item.SetValue(obj, b);
                    }
                    else if (item.PropertyType == typeof(decimal))
                    {
                        decimal d = 0;
                        decimal.TryParse(dic[item.Name].ToString(), out d);
                        item.SetValue(obj, d);
                    }
                    else if (item.PropertyType == typeof(DateTime))
                    {
                        item.SetValue(obj, Convert.ToDateTime(dic[item.Name]));
                    }
                }
            }
        }


        /// <summary>
        /// 将对象属性值赋值给UI控件内容
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="ctl"></param>
        public static void ObjectToControl(object obj, Control ctl)
        {
            var t = obj.GetType();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            foreach (var item in t.GetProperties())
            {
                dic.Add(item.Name, item.GetValue(obj));
            }
            List<Control> ctlList = GetAllControls(ctl);
            foreach (Control item in ctlList)
            {
                var split = item.Name.Split('_');
                if (split.Length < 2)
                {
                    continue;
                }
                string key = item.Name.Substring(item.Name.IndexOf('_') + 1);
                //string key = split[1];
                if (!dic.Keys.Contains(key))
                {
                    continue;
                }
                if (item is TextBox txt)
                {
                    txt.Text = dic[key].ToString();
                }
                else if (item is NumericUpDown nup)
                {
                    nup.Value = Convert.ToDecimal(dic[key]);
                }
                else if (item is ComboBox cbox)
                {
                    cbox.Text = dic[key].ToString();
                }
                else if (item is CheckBox chbox)
                {
                    chbox.Checked = Convert.ToBoolean(dic[key]);
                }
                else if (item is RadioButton rbtn)
                {
                    rbtn.Checked = Convert.ToBoolean(dic[key]);
                }
                else if (item is Label lbl)
                {
                    lbl.Text = dic[key].ToString();
                    if (double.TryParse(lbl.Text, out double d))
                    {
                        lbl.Text = d.ToString("f2");
                    }
                }
            }
        }
    }
}

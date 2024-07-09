using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace Prober.WaferDef {
    public class FileHelper
    {
        public static string ErrorMsg = string.Empty;
        /// <summary>
        /// 用二进制的方式写配置文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static bool WriteBinaryFile(string path, object obj)
        {
            FileStream stream = null;
            try
            {
                stream = new FileStream(path, FileMode.Create, FileAccess.Write);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                return true;
            }
            catch (Exception ex)
            {
                ErrorMsg = $"WriteBinaryFile({path})出错！" + ex.Message;
                return false;
            }
            finally
            {
                stream?.Close();
                stream?.Dispose();
            }
        }

        /// <summary>
        /// 用二进制的方式读取配置文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static object ReadBinaryFile(string path)
        {
            FileStream stream = null;
            try
            {
                stream = new FileStream(path, FileMode.Open, FileAccess.Read);
                BinaryFormatter formatter = new BinaryFormatter();
                stream.Position = 0;
                //stream.Seek(0, SeekOrigin.Begin);
                return formatter.Deserialize(stream);

            }
            catch (Exception ex)
            {
                ErrorMsg = $"ReadBinaryFile({path})出错！" + ex.Message;
                return null;
            }
            finally
            {
                stream?.Close();
                stream?.Dispose();
            }
        }

        /// <summary>
        /// 保存XML格式的配置文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="o">对象</param>
        /// <param name="in_type">对象类型</param>
        /// <returns></returns>
        public static bool SaveXml(string path, object o, Type in_type)
        {
            FileStream stream = null;
            try
            {
                XmlSerializer xml = new XmlSerializer(in_type);
                stream = new FileStream(path, FileMode.Create, FileAccess.Write);
                xml.Serialize(stream, o);
            }
            catch (Exception ex)
            {
                ErrorMsg = $"SaveXml({path})出错！" + ex.Message;
                return false;
            }
            finally
            {
                stream?.Close();
                stream?.Dispose();
            }
            return true;
        }

        /// <summary>
        /// 加载XML配置文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="in_type">加载的数据类型</param>
        /// <returns></returns>
        public static object LoadXml(string path, Type in_type)
        {
            FileStream stream = null;
            try
            {
                XmlSerializer xml = new XmlSerializer(in_type);
                stream = new FileStream(path, FileMode.OpenOrCreate);
                object o = xml.Deserialize(stream);
                return o;
            }
            catch (Exception ex)
            {
                ErrorMsg = $"LoadXml({path})出错！" + ex.Message;
                return null;
            }
            finally
            {
                stream?.Close();
                stream?.Dispose();
            }
        }

        /// <summary>
        /// 将一个新的对象放入XML文件中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool SaveNewToXml<T>(string path, T input) where T : XMLBase
        {
            List<T> list = LoadXml(path, typeof(List<T>)) as List<T>;
            if (list == null)
                list = new List<T>();
            T curT = list.FirstOrDefault(t => t.Key == input.Key);
            if (curT == null)
            {
                list.Add(input);
            }
            else
            {
                int index = list.IndexOf(curT);
                list.Remove(curT);
                list.Insert(index, input);
            }
            return SaveXml(path, list, list.GetType());
        }

        public static bool DeleteFrmoXml<T>(string path, string key) where T : XMLBase
        {
            List<T> list = LoadXml(path, typeof(List<T>)) as List<T>;
            if (list == null)
            {
                ErrorMsg = "加载文件失败！";
                return false;
            }
            T curT = list.FirstOrDefault(t => t.Key == key);
            if (curT == null)
            {
                ErrorMsg = "不存在" + key;
                return false;
            }
            else
            {
                list.Remove(curT);
            }
            return SaveXml(path, list, list.GetType());
        }

    }

    public class XMLBase
    {
        public virtual string Key { get; set; } = string.Empty;
    }
}

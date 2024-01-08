using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using QuickNote.Model;
using SQLite;
using static SQLite.SQLite3;

namespace QuickNote.ViewModel.Helper
{
    public class DatabaseHelper
    {
        private static string dbFile = Path.Combine(Environment.CurrentDirectory, "notesDb.db3");
        private static string dbPath = "https://quicknote-wpf-default-rtdb.firebaseio.com/";
        public static async Task<bool> Insert<T>(T item)
        {
            //bool result = false;
            //using(SQLiteConnection conn = new SQLiteConnection(dbFile))
            //{
            //    conn.CreateTable<T>();
            //    int row = conn.Insert(item);
            //    if(row > 0)
            //    {
            //        result = true;
            //    }
            //}
            //return result;
            var jsonBody = JsonConvert.SerializeObject(item);
            var content = new StringContent(jsonBody,Encoding.UTF8,"application/json");
            HttpResponseMessage result;
            using(var client = new HttpClient())
            { 
                result = await client.PostAsync($"{dbPath}{item.GetType().Name.ToLower()}.json",content);
                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

        public static async Task<bool> Update<T>(T item) where T : HasId
        {
            //bool result = false;
            //using (SQLiteConnection conn = new SQLiteConnection(dbFile))
            //{
            //    conn.CreateTable<T>();
            //    int row = conn.Update(item);
            //    if (row > 0)
            //    {
            //        result = true;
            //    }
            //}
            //return result;
            var jsonBody = JsonConvert.SerializeObject(item);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            HttpResponseMessage result;
            using (var client = new HttpClient())
            {
                result = await client.PatchAsync($"{dbPath}{item.GetType().Name.ToLower()}/{item.Id}.json", content);
                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

        public static async Task<bool> Delete<T>(T item) where T : HasId
        {
            //bool result = false;
            //using (SQLiteConnection conn = new SQLiteConnection(dbFile))
            //{
            //    conn.CreateTable<T>();
            //    int row = conn.Delete(item);
            //    if (row > 0)
            //    {
            //        result = true;
            //    }
            //}
            //return result;
            var jsonBody = JsonConvert.SerializeObject(item);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            HttpResponseMessage result;
            using (var client = new HttpClient())
            {
                result = await client.DeleteAsync($"{dbPath}{item.GetType().Name.ToLower()}/{item.Id}.json");
                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }
        public static async Task<List<T>> Read<T>() where T : HasId
        {
            //List<T> items;
            //using (SQLiteConnection conn = new SQLiteConnection(dbFile))
            //{
            //    conn.CreateTable<T>();
            //    items = conn.Table<T>().ToList();
            //}
            //return items;
            HttpResponseMessage result;
            List<T> resultList = new List<T>();
            using (var client = new HttpClient())
            {
                result = await client.GetAsync($"{dbPath}{typeof(T).Name.ToLower()}.json");
                var jsonObj = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                {
                    var objects = JsonConvert.DeserializeObject<Dictionary<string, T>>(jsonObj);
                    if(objects != null)
                    {
                        foreach (var o in objects)
                        {
                            o.Value.Id = o.Key;
                            resultList.Add(o.Value);
                        }
                    }
                    return resultList;
                }
                else
                {
                    return resultList;
                }
            }
            return resultList;
        }
    }
}

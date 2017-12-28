using NPoco;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Web;

namespace RTS
{
    public static class SqlExec
    {
        public static List<Model.Category> GetCategories()
        {
            using (var dbConnection = new SQLiteConnection(Properties.Settings.Default.DbConnectionString))
            {
                dbConnection.Open();
                using (var db = new NPoco.Database(dbConnection, DatabaseType.SQLite))
                {
                    var sql = new NPoco.Sql();
                    sql.Select("*");
                    sql.From("Category");
                    sql.OrderBy("SortId ASC, CategoryName ASC");
                    var results = db.Fetch<Model.Category>(sql);
                    return results;
                }
            }
        }

        public static Model.Category GetCategoryByCategoryCode(string categoryCode)
        {
            using (var dbConnection = new SQLiteConnection(Properties.Settings.Default.DbConnectionString))
            {
                dbConnection.Open();
                using (var db = new NPoco.Database(dbConnection, DatabaseType.SQLite))
                {
                    var sql = new NPoco.Sql();
                    sql.Select("*");
                    sql.From("Category");
                    sql.Where("CategoryCode = @0", categoryCode);
                    sql.OrderBy("SortId ASC, CategoryName ASC");
                    var results = db.Fetch<Model.Category>(sql);
                    if (results.Count > 0)
                        return results[0];
                    else
                        return null;
                }
            }
        }

        public static long GetToneCount()
        {
            using (var dbConnection = new SQLiteConnection(Properties.Settings.Default.DbConnectionString))
            {
                dbConnection.Open();
                using (var db = new NPoco.Database(dbConnection, DatabaseType.SQLite))
                {
                    var sql = new NPoco.Sql();
                    sql.Select("COUNT(*) AS ToneCount");
                    sql.From("Tone");
                    var results = db.Fetch<dynamic>(sql);
                    if (results.Count > 0)
                        return results[0].ToneCount;
                    else
                        return 0;
                }
            }
        }

        public static List<Model.Tone> GetTopTenTones()
        {
            using (var dbConnection = new SQLiteConnection(Properties.Settings.Default.DbConnectionString))
            {
                dbConnection.Open();
                using (var db = new NPoco.Database(dbConnection, DatabaseType.SQLite))
                {
                    var sql = new NPoco.Sql();
                    sql.Select("*");
                    sql.From("Tone");
                    sql.OrderBy("Counter DESC");
                    sql.Append("LIMIT 10");
                    var results = db.Fetch<Model.Tone>(sql);
                    return results;
                }
            }
        }

        public static List<Model.Tone> GetTonesByCategory(string category)
        {
            using (var dbConnection = new SQLiteConnection(Properties.Settings.Default.DbConnectionString))
            {
                dbConnection.Open();
                using (var db = new NPoco.Database(dbConnection, DatabaseType.SQLite))
                {
                    var sql = new NPoco.Sql();
                    sql.Select("*");
                    sql.From("Tone");
                    sql.Where("Category LIKE @0", "%-" + category + "-%");
                    sql.OrderBy("Artist ASC, Title ASC");
                    var results = db.Fetch<Model.Tone>(sql);
                    return results;
                }
            }
        }

        public static void IncrementDownloadCount(int toneId)
        {
            using (var dbConnection = new SQLiteConnection(Properties.Settings.Default.DbConnectionString))
            {
                dbConnection.Open();
                using (var db = new NPoco.Database(dbConnection, DatabaseType.SQLite))
                {
                    var sql = new NPoco.Sql();
                    sql.Select("*");
                    sql.From("Tone");
                    sql.Where("ToneId = @0", toneId);
                    var results = db.Fetch<Model.Tone>(sql);
                    if (results.Count > 0)
                    {
                        results[0].Counter = results[0].Counter + 1;
                        db.Update(results[0]);
                    }
                }
            }
        }
        public static List<Model.Tone> GetTonesBySearch(string search)
        {
            using (var dbConnection = new SQLiteConnection(Properties.Settings.Default.DbConnectionString))
            {
                dbConnection.Open();
                using (var db = new NPoco.Database(dbConnection, DatabaseType.SQLite))
                {
                    var sql = new NPoco.Sql();
                    sql.Select("*");
                    sql.From("Tone");
                    sql.Where("Artist LIKE @0 OR Title LIKE @0 OR ToneId = @1", "%" + search + "%", search);
                    sql.OrderBy("Artist ASC, Title ASC");
                    var results = db.Fetch<Model.Tone>(sql);
                    return results;
                }
            }
        }

        public static Model.Tone GetToneById(int id)
        {
            using (var dbConnection = new SQLiteConnection(Properties.Settings.Default.DbConnectionString))
            {
                dbConnection.Open();
                using (var db = new NPoco.Database(dbConnection, DatabaseType.SQLite))
                {
                    var sql = new NPoco.Sql();
                    sql.Select("*");
                    sql.From("Tone");
                    sql.Where("ToneId = @0", id);
                    var results = db.Fetch<Model.Tone>(sql);
                    if (results.Count > 0)
                    {
                        return results[0];
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public static Model.HitCount GetHitCounter()
        {
            using (var dbConnection = new SQLiteConnection(Properties.Settings.Default.DbConnectionString))
            {
                dbConnection.Open();
                using (var db = new NPoco.Database(dbConnection, DatabaseType.SQLite))
                {
                    var sql = new NPoco.Sql();
                    sql.Select("*");
                    sql.From("HitCount");
                    var results = db.Fetch<Model.HitCount>(sql);
                    if (results.Count > 0)
                    {
                        return results[0];
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public static bool UpdateHitCounter(string ipAddress)
        {
            CheckForNewDay();
            if (Properties.Settings.Default.ResetVisitorsDaily)
            {
                ResetVisitors();
            }

            using (var dbConnection = new SQLiteConnection(Properties.Settings.Default.DbConnectionString))
            {
                dbConnection.Open();
                using (var db = new NPoco.Database(dbConnection, DatabaseType.SQLite))
                {
                    var sql = new NPoco.Sql();
                    sql.Select("*");
                    sql.From("Visitor");
                    sql.Where("IPAddress = @0", ipAddress);
                    var results = db.Fetch<Model.Visitor>(sql);
                    if (results.Count > 0)
                    {
                        results[0].LastVisit = DateTime.Now;
                        db.Update(results[0]);
                        IncrementHitCounterTotal(false);

                    }
                    else
                    {
                        InsertIPAddress(ipAddress);
                        IncrementHitCounterTotal(true);
                    }
                }
            }

            return true;
        }

        private static void InsertIPAddress(string ipAddress)
        {
            using (var dbConnection = new SQLiteConnection(Properties.Settings.Default.DbConnectionString))
            {
                dbConnection.Open();
                using (var db = new NPoco.Database(dbConnection, DatabaseType.SQLite))
                {
                    var visitor = new Model.Visitor();
                    visitor.IPAddress = ipAddress;
                    var visitTime = DateTime.Now;
                    visitor.FirstVisit = visitTime;
                    visitor.LastVisit = visitTime;
                    db.Insert(visitor);
                }
            }
        }

        private static void IncrementHitCounterTotal(bool isUnique)
        {
            using (var dbConnection = new SQLiteConnection(Properties.Settings.Default.DbConnectionString))
            {
                dbConnection.Open();
                using (var db = new NPoco.Database(dbConnection, DatabaseType.SQLite))
                {
                    var sql = new NPoco.Sql();
                    sql.Append("UPDATE HitCount SET");
                    sql.Append("PageViewsTotal = PageViewsTotal + 1");
                    if (isUnique)
                    {
                        sql.Append(", UniqueHitsTotal = UniqueHitsTotal + 1");
                    }
                    var results = db.Execute(sql);
                }
            }
        }

        private static void CheckForNewDay()
        {
            using (var dbConnection = new SQLiteConnection(Properties.Settings.Default.DbConnectionString))
            {
                dbConnection.Open();
                using (var db = new NPoco.Database(dbConnection, DatabaseType.SQLite))
                {
                    var sql = new NPoco.Sql();
                    sql.Select("*");
                    sql.From("HitCount");
                    var results = db.Fetch<Model.HitCount>(sql);
                    if (results.Count > 0 && results[0].CurrentDate != DateTime.Parse(DateTime.Now.ToShortDateString()))
                    {
                        sql = new NPoco.Sql();
                        sql.Append("UPDATE HitCount SET CurrentDate = date('now')");
                        db.Execute(sql);
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }

        private static void ResetVisitors()
        {
            using (var dbConnection = new SQLiteConnection(Properties.Settings.Default.DbConnectionString))
            {
                dbConnection.Open();
                using (var db = new NPoco.Database(dbConnection, DatabaseType.SQLite))
                {
                    var sql = new NPoco.Sql();
                    sql.Append("DELETE FROM Visitor");
                    db.Execute(sql);

                }
            }
        }
    }
}
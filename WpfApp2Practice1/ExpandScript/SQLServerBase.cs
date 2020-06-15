using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2Practice1.ExpandScript
{
    class SQLServerBase
    {
        public int insertRowCount;
        public int updateRowCount;

        public SQLServerBase() { }
        /// <summary>
        /// 连接数据库重新创建一个库并创建表
        /// </summary>
        public SqlConnection Update_SqlServer_Initialize()
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "LAPTOP-BOS920GT";
                builder.UserID = "sa";
                builder.Password = "123456";
                builder.InitialCatalog = "master";

                SqlConnection connection = new SqlConnection(builder.ConnectionString);
                connection.Open();
                StringBuilder sb = new StringBuilder();
                sb.Append("IF NOT EXISTS (SELECT * FROM sys.databases WHERE [Name] = 'StudentCenterDB') ");
                sb.Append(" CREATE DATABASE StudentCenterDB ");
                String sql = sb.ToString();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }

                sb.Clear();
                sb.Append("USE StudentCenterDB; ");
                sb.Append("if object_id(N'Students',N'U') is NULL ");
                sb.Append("	CREATE TABLE Students(Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY, Name NVARCHAR(50), No NVARCHAR(20), Score_0 FLOAT, Score_1 FLOAT, Score_2 FLOAT, Score_3 FLOAT, Score_4 FLOAT, Score_5 FLOAT, Score_6 FLOAT) ");
                sql = sb.ToString();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }

                sb.Clear();
                sb.Append("if object_id(N'Temp',N'U') is NULL ");
                sb.Append("	CREATE TABLE Temp(Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY, Name NVARCHAR(50), No NVARCHAR(20), Score_0 FLOAT, Score_1 FLOAT, Score_2 FLOAT, Score_3 FLOAT, Score_4 FLOAT, Score_5 FLOAT, Score_6 FLOAT) ");
                sql = sb.ToString();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
                return connection;
            }
            catch (SqlException e)
            {
                System.Windows.MessageBox.Show("数据库打开失败，详细信息：" + e.ToString());
                return null;
            }
        }

        /// <summary>
        /// 插入数据到临时表中
        /// </summary>
        /// <param name="connection">连接</param>
        /// <param name="student">学生数据</param>
        /// <param name="idManager">ID管理码</param>
        public void Update_SqlServer_Insert(SqlConnection connection, MainWindow.Student student, string idManager)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                sb.Append("INSERT INTO Temp (Name, No, Score_0, Score_1, Score_2, Score_3, Score_4, Score_5, Score_6) ");
                sb.Append("VALUES (@Name, @No, @Score_0, @Score_1, @Score_2, @Score_3, @Score_4, @Score_5, @Score_6);");
                String sql = sb.ToString();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Name", student.Name);
                    command.Parameters.AddWithValue("@No", idManager + student.StuNo.ToString().PadLeft(3, '0'));
                    command.Parameters.AddWithValue("@Score_0", student.Score_0);
                    command.Parameters.AddWithValue("@Score_1", student.Score_1);
                    command.Parameters.AddWithValue("@Score_2", student.Score_2);
                    command.Parameters.AddWithValue("@Score_3", student.Score_3);
                    command.Parameters.AddWithValue("@Score_4", student.Score_4);
                    command.Parameters.AddWithValue("@Score_5", student.Score_5);
                    command.Parameters.AddWithValue("@Score_6", student.Score_6);
                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException e)
            {
                System.Windows.MessageBox.Show("插入数据失败，详细信息：" + e.ToString());
            }
        }

        /// <summary>
        /// 合并数据
        /// </summary>
        /// <param name="connection">连接</param>
        public void Update_SqlServer_Join(SqlConnection connection)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                sb.AppendLine("INSERT INTO Students(Name,No,Score_0,Score_1,Score_2,Score_3,Score_4,Score_5,Score_6)");
                sb.AppendLine("SELECT Name, No, Score_0, Score_1, Score_2, Score_3, Score_4, Score_5, Score_6");
                sb.AppendLine("FROM Temp");
                sb.AppendLine("WHERE Temp.No NOT IN (");
                sb.AppendLine("	SELECT No");
                sb.AppendLine("	FROM Students)");
                String sql = sb.ToString();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    insertRowCount = command.ExecuteNonQuery();
                }

                sb.Clear();
                sb.AppendLine("UPDATE Students");
                sb.AppendLine("SET Students.Name = Temp.Name, Students.Score_0 = Temp.Score_0, Students.Score_1 = Temp.Score_1, Students.Score_2 = Temp.Score_2, Students.Score_3 = Temp.Score_3, Students.Score_4 = Temp.Score_4, Students.Score_5 = Temp.Score_5, Students.Score_6 = Temp.Score_6");
                sb.AppendLine("FROM Temp");
                sb.AppendLine("WHERE Students.No = Temp.No AND ( Students.Name != Temp.Name OR Students.Score_0 != Temp.Score_0 OR Students.Score_1 != Temp.Score_1 OR Students.Score_2 != Temp.Score_2 OR Students.Score_3 != Temp.Score_3 OR Students.Score_4 != Temp.Score_4 OR Students.Score_5 != Temp.Score_5 OR Students.Score_6 != Temp.Score_6 )");
                sql = sb.ToString();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    updateRowCount = command.ExecuteNonQuery();
                }

                sb.Clear();
                sb.Append("TRUNCATE TABLE Temp;");
                sql = sb.ToString();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException e)
            {
                System.Windows.MessageBox.Show("合并数据失败，详细信息：" + e.ToString());
            }
        }

        #region 无用封装
        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="connection">连接</param>
        public void Update_SqlServer_Update(SqlConnection connection)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                String userToUpdate = "Nikita";
                sb.Append("UPDATE Employees SET Location = N'United States' WHERE Name = @name");
                String sql = sb.ToString();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@name", userToUpdate);
                    int rowAffected = command.ExecuteNonQuery();
                    Console.WriteLine(rowAffected + " row(s) updated");
                }
            }
            catch (SqlException e)
            {
                System.Windows.MessageBox.Show("修改数据失败，详细信息：" + e.ToString());
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="connection">连接</param>
        public void Update_SqlServer_Delete(SqlConnection connection)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                String userToDelete = "Jared";
                sb.Append("DELETE FROM Employees WHERE Name = @name;");
                String sql = sb.ToString();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@name", userToDelete);
                    int rowAffected = command.ExecuteNonQuery();
                    Console.WriteLine(rowAffected + " row(s) deleted");
                }
            }
            catch (SqlException e)
            {
                System.Windows.MessageBox.Show("删除数据失败，详细信息：" + e.ToString());
            }
        }

        /// <summary>
        /// 查找数据
        /// </summary>
        /// <param name="connection">连接</param>
        public void Update_SqlServer_Select(SqlConnection connection)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                String sql = "SELECT Id, Name, Location FROM Employees;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine("{0} {1} {2}", reader.GetInt32(0), reader.GetString(1), reader.GetString(2));
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                System.Windows.MessageBox.Show("查找数据失败，详细信息：" + e.ToString());
            }
        }
        #endregion
    }
}

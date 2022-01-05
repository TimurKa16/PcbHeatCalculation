using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Диплом
{
    public static class DBClass
    {
        static SqlConnection sqlConnection;
        static SqlDataReader sqlReader;
        static SqlCommand command;

        static List<int> elementIds = new List<int>();

        public static void LoadDB()
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\timur\Документы\ProjectsToUpload\PcbHeatCalculation\Database1.mdf;Integrated Security=True";
            sqlConnection = new SqlConnection(connectionString);

            try
            {
                sqlConnection.Open();
                sqlConnection.Close();
                sqlConnection.Open();
                sqlConnection.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Извините, база данных не загрузилась." + Environment.NewLine +
                    "Попробуйте повторить попытку!");
                Application.Exit();
            }
        }


        public static void ReadElements(ref List<string> lbList, ref List<string[]> elementList)
        {
            sqlConnection.Open();

            lbList = new List<string> { "ID элемента", "Название",
                "Длина", "Ширина", "Мощность тепловыделения", "Допустимая температура" };
            elementList = new List<string[]>();
            //string s = "";
            sqlReader = null;
            command = new SqlCommand("SELECT * FROM [Element]", sqlConnection);

            try
            {
                sqlReader = command.ExecuteReader();

                while (sqlReader.Read())
                {
                    elementList.Add(new string[6]);
                    elementList[elementList.Count - 1][0] = Convert.ToString(sqlReader["ElementId"]);
                    elementList[elementList.Count - 1][1] = Convert.ToString(sqlReader["ElementName"]);
                    elementList[elementList.Count - 1][2] = Convert.ToString(sqlReader["ElementLength"]);
                    elementList[elementList.Count - 1][3] = Convert.ToString(sqlReader["ElementWidth"]);
                    elementList[elementList.Count - 1][4] = Convert.ToString(sqlReader["ElementPower"]);
                    elementList[elementList.Count - 1][5] = Convert.ToString(sqlReader["ElementMaxTemperature"]);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка!");
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Close();
            }
            sqlConnection.Close();
        }

        public static void ReadProjectElements(ref Project project)
        {
            project.elementList = new List<Element>();
            sqlConnection.Open();

            try
            {
                sqlReader = null;
                command = new SqlCommand("SELECT * FROM [ProjectElement] WHERE [PEProjectId] = @projectId", sqlConnection);
                command.Parameters.AddWithValue("projectId", project.ProjectId);

                sqlReader = command.ExecuteReader();

                while (sqlReader.Read())
                {
                    project.elementList.Add(new Element());
                    project.elementList[project.elementList.Count - 1].Id = Convert.ToInt32(sqlReader["PEElementId"]);
                    project.elementList[project.elementList.Count - 1].XCoordinate = Convert.ToInt32(sqlReader["ElementXCoord"]);
                    project.elementList[project.elementList.Count - 1].YCoordinate = Convert.ToInt32(sqlReader["ElementYCoord"]);
                }
                for (int i = 0; i < project.elementList.Count; i++)
                {
                    if (sqlReader != null)
                        sqlReader.Close();
                    command = new SqlCommand("SELECT * FROM [Element] WHERE [ElementId] = @elId", sqlConnection);
                    command.Parameters.AddWithValue("elId", project.elementList[i].Id);

                    sqlReader = command.ExecuteReader();

                    while (sqlReader.Read())
                    {
                        project.elementList[i].Name = Convert.ToString(sqlReader["ElementName"]);
                        project.elementList[i].Length = Convert.ToInt32(sqlReader["ElementLength"]);
                        project.elementList[i].Width = Convert.ToInt32(sqlReader["ElementWidth"]);
                        project.elementList[i].Power = Convert.ToDouble(sqlReader["ElementPower"]);
                        project.elementList[i].MaxTemperature = Convert.ToDouble(sqlReader["ElementMaxTemperature"]);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка!");
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Close();
            }
            sqlConnection.Close();
        }

        public static void ReadElement(List<string> listtb, List<string> lbList)
        {
            sqlConnection.Open();
            lbList = new List<string> { "ID проекта", "Имя проекта", "Дата создания" };
            listtb = new List<string> { "ElementID", "ElementName", "ElementLength", "ElementWidth" };
            string s = "";
            sqlReader = null;
            command = new SqlCommand("SELECT * FROM [Element]", sqlConnection);

            try
            {
                sqlReader = command.ExecuteReader();

                while (sqlReader.Read())
                {
                    s += Convert.ToString(sqlReader["ElementId"]) + "\t";
                    s += Convert.ToString(sqlReader["ElementName"]) + "\t";
                    s += Convert.ToString(sqlReader["ElementLength"]) + "\t";
                    s += Convert.ToString(sqlReader["ElementWidth"]) + "\t";
                }
                MessageBox.Show(s);

            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка!");
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Close();
            }
            sqlConnection.Close();
        }
        
        public static void ReadProjects(ref List<string[]> projectList, ref List<string> lbList)
        {
            sqlConnection.Open();

            lbList = new List<string> { "ID проекта", "Имя проекта", "Дата создания" };
            projectList = new List<string[]>();
            sqlReader = null;
            command = new SqlCommand("SELECT * FROM [Project]", sqlConnection);

            try
            {
                sqlReader = command.ExecuteReader();

                while (sqlReader.Read())
                {
                    projectList.Add(new string[3]);
                    projectList[projectList.Count - 1][0] = Convert.ToString(sqlReader["ProjectId"]);
                    projectList[projectList.Count - 1][1] = Convert.ToString(sqlReader["ProjectName"]);
                    projectList[projectList.Count - 1][2] = Convert.ToString(sqlReader["ProjectDate"]);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка!");
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Close();
            }
            sqlConnection.Close();
        }

        public static void CreateProject(ref Project project)
        {            
            sqlConnection.Open();

            command = new SqlCommand("INSERT INTO [Project] (ProjectName, ProjectDate)VALUES(@ProjectName, @ProjectDate)", sqlConnection);

            command.Parameters.AddWithValue("ProjectName", project.Name.ToString());
            command.Parameters.AddWithValue("ProjectDate", project.Date.ToString());
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка!");
            }

            int id = 0;

            sqlReader = null;
            command = new SqlCommand("SELECT [ProjectId] FROM [Project] " +
                "WHERE ProjectName = @projectName AND ProjectDate = @projectDate", sqlConnection);
            command.Parameters.AddWithValue("projectName", project.Name);
            command.Parameters.AddWithValue("projectDate", project.Date);
            try
            {
                sqlReader = command.ExecuteReader();

                while (sqlReader.Read())
                {
                    id = Convert.ToInt32(sqlReader["ProjectId"]);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка!");
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Close();
            }
            
            sqlConnection.Close();
            project.ProjectId = id;
        }


        public static void ReadProject(ref Project project)
        {
            sqlConnection.Open();

            sqlReader = null;
            command = new SqlCommand("SELECT * FROM [Project] WHERE [ProjectId] = @projectId", sqlConnection);
            command.Parameters.AddWithValue("projectId", project.ProjectId);

            try
            {
                sqlReader = command.ExecuteReader();

                while (sqlReader.Read())
                {
                    project.Name = Convert.ToString(sqlReader["ProjectName"]);
                    project.Date = Convert.ToString(sqlReader["ProjectDate"]);
                    project.pcb.Id = Convert.ToInt32(sqlReader["ProjectPCBId"]);
                    project.firstOverheat = Convert.ToInt32(sqlReader["ProjectFirstOverHeat"]);
                    project.totalTime = Convert.ToInt32(sqlReader["ProjectTotalTime"]);
                    project.stepTime = Convert.ToInt32(sqlReader["ProjectStepTime"]);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка!");
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Close();
            }
            sqlConnection.Close();
        }

        public static void ReadProjectElement(ref Project project)
        {
            sqlConnection.Open();

            sqlReader = null;
            command = new SqlCommand("SELECT * FROM [Project], WHERE [ProjectId] = @projectId", sqlConnection);
            command.Parameters.AddWithValue("projectId", project.ProjectId);

            try
            {
                sqlReader = command.ExecuteReader();

                while (sqlReader.Read())
                {
                    project.Name = Convert.ToString(sqlReader["ProjectName"]);
                    project.Date = Convert.ToString(sqlReader["ProjectDate"]);
                    project.pcb.Id = Convert.ToInt32(sqlReader["ProjectPCBId"]);
                    project.firstOverheat = Convert.ToInt32(sqlReader["ProjectFirstOverHeat"]);
                    project.totalTime = Convert.ToInt32(sqlReader["ProjectTotalTime"]);
                    project.stepTime = Convert.ToInt32(sqlReader["ProjectStepTime"]);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка!");
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Close();
            }
            sqlConnection.Close();
        }

        public static void ReadPCBs(ref List<string> lbList, ref List<string[]> pcbList)
        {
            sqlConnection.Open();

            lbList = new List<string> { "ID ПП", "Название",
                "Длина", "Ширина", "Плотность", "Коэффициент теплоотдачи", "Теплопроводность", "Теплоемкость" };
            pcbList = new List<string[]>();
            sqlReader = null;
            command = new SqlCommand("SELECT * FROM [PCB]", sqlConnection);

            try
            {
                sqlReader = command.ExecuteReader();

                while (sqlReader.Read())
                {
                    pcbList.Add(new string[9]);
                    pcbList[pcbList.Count - 1][0] = Convert.ToString(sqlReader["PCBId"]);
                    pcbList[pcbList.Count - 1][1] = Convert.ToString(sqlReader["PCBName"]);
                    pcbList[pcbList.Count - 1][2] = Convert.ToString(sqlReader["PCBLength"]);
                    pcbList[pcbList.Count - 1][3] = Convert.ToString(sqlReader["PCBWidth"]);
                    pcbList[pcbList.Count - 1][4] = Convert.ToString(sqlReader["PCBHeight"]);
                    pcbList[pcbList.Count - 1][5] = Convert.ToString(sqlReader["PCBDencity"]);
                    pcbList[pcbList.Count - 1][6] = Convert.ToString(sqlReader["PCBHeatSurfaceFilmK"]);
                    pcbList[pcbList.Count - 1][7] = Convert.ToString(sqlReader["PCBHeatConduction"]);
                    pcbList[pcbList.Count - 1][8] = Convert.ToString(sqlReader["PCBHeatCapacity"]);
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка!");
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Close();
            }
            sqlConnection.Close();
        }



        public static void ReadPCB(ref Project project)
        {
            sqlConnection.Open();

            sqlReader = null;
            command = new SqlCommand("SELECT * FROM [PCB] WHERE [PCBId] = @pcbId", sqlConnection);
            command.Parameters.AddWithValue("pcbId", project.pcb.Id);
            try
            {
                sqlReader = command.ExecuteReader();

                while (sqlReader.Read())
                {
                    //pcbList[pcbList.Count - 1][0] = Convert.ToString(sqlReader["PCBId"]);
                    project.pcb.Name = Convert.ToString(sqlReader["PCBName"]);
                    project.pcb.Length = Convert.ToInt32(sqlReader["PCBLength"]);
                    project.pcb.Width = Convert.ToInt32(sqlReader["PCBWidth"]);
                    project.pcb.Height = Convert.ToDouble(sqlReader["PCBHeight"]);
                    project.pcb.Dencity = Convert.ToDouble(sqlReader["PCBDencity"]);
                    project.pcb.HeatSurfaceFilmK = Convert.ToDouble(sqlReader["PCBHeatSurfaceFilmK"]);
                    project.pcb.HeatConduction = Convert.ToDouble(sqlReader["PCBHeatConduction"]);
                    project.pcb.HeatCapacity = Convert.ToDouble(sqlReader["PCBHeatCapacity"]);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка!");
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Close();
            }
            sqlConnection.Close();
        }

        static void Load()
        {

        }

        #region Save

        public static void SaveAnalysisParameters(Project project)
        {
            sqlConnection.Open();

            command = new SqlCommand("UPDATE [Project] " +
                "SET ProjectName = @projectName, ProjectDate = @projectDate, ProjectPCBId = @projectPCBId," +
                "ProjectFirstOverheat = @projectFirstOverheat, ProjectTotalTime = @projectTotalTime, " +
                "ProjectStepTime = @projectStepTime" +
                " WHERE ProjectId = @projectId; ", sqlConnection);

            command.Parameters.AddWithValue("projectName", project.Name.ToString());
            command.Parameters.AddWithValue("projectDate", project.Date.ToString());
            command.Parameters.AddWithValue("projectPCBId", project.pcb.Id);
            command.Parameters.AddWithValue("projectFirstOverheat", project.firstOverheat.ToString());
            command.Parameters.AddWithValue("projectTotalTime", project.totalTime.ToString());
            command.Parameters.AddWithValue("projectStepTime", project.totalTime.ToString());
            command.Parameters.AddWithValue("projectId", Convert.ToInt32(project.ProjectId));

            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка");
            }
            sqlConnection.Close();
        }

        public static void SavePCB(Project project)
        {
            sqlConnection.Open();

            command = new SqlCommand("UPDATE [PCB] " +
                "SET PCBName = @pcbName, PCBLength = @pcbLength, PCBWidth = @pcbWidth, PCBHeight = @pcbHeight, " +
                "PCBDencity = @pcbDencity, PCBHeatSurfaceFilmK = @pcbHeatSurfaceFilmK, " +
                "PCBHeatConduction = @pcbHeatConduction, PCBHeatCapacity = @pcbHeatCapacity " +
                "WHERE PCBId = @pcbId; ", sqlConnection);

            command.Parameters.AddWithValue("pcbName", project.pcb.Name.ToString());
            command.Parameters.AddWithValue("pcbLength", project.pcb.Length.ToString());
            command.Parameters.AddWithValue("pcbWidth", project.pcb.Width.ToString());
            command.Parameters.AddWithValue("pcbHeight", "0,2");
            command.Parameters.AddWithValue("pcbDencity", project.pcb.Dencity.ToString());
            command.Parameters.AddWithValue("pcbHeatSurfaceFilmK", project.pcb.HeatSurfaceFilmK.ToString());
            command.Parameters.AddWithValue("pcbHeatConduction", project.pcb.HeatConduction.ToString());
            command.Parameters.AddWithValue("pcbHeatCapacity", project.pcb.HeatCapacity.ToString());
            command.Parameters.AddWithValue("pcbId", project.pcb.Id);

            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка");
            }
            sqlConnection.Close();
        }

        public static void SaveElementsWithIds(Project project)
        {
            sqlConnection.Open();
            try
            {
                for (int i = 0; i < project.elementList.Count; i++)
                {
                    if (project.elementList[i].Id != 0)
                    {
                        command = new SqlCommand("UPDATE [Element] " +
                            "SET ElementName = @elementName, ElementLength = @elementLength, " +
                            "ElementWidth = @elementWidth, ElementPower = @elementPower, ElementMaxTemperature = @elementMaxTemperature" +
                            " WHERE ElementId = @elementId ", sqlConnection);

                        command.Parameters.AddWithValue("elementName", project.elementList[i].Name.ToString());
                        command.Parameters.AddWithValue("elementLength", project.elementList[i].Length.ToString());
                        command.Parameters.AddWithValue("elementWidth", project.elementList[i].Width.ToString());
                        command.Parameters.AddWithValue("elementPower", project.elementList[i].Power.ToString());
                        command.Parameters.AddWithValue("elementMaxTemperature", project.elementList[i].MaxTemperature.ToString());
                        command.Parameters.AddWithValue("elementId", project.elementList[i].Id.ToString());
                        
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка");
            }
            sqlConnection.Close();
        }

        public static void SaveElementsWithoutIds(Project project)
        {
            int count = 0;
            sqlConnection.Open();
            try
            {
                for (int i = 0; i < project.elementList.Count; i++)
                {
                    if (project.elementList[i].Id == 0)
                    {
                        count++;

                        command = new SqlCommand("INSERT INTO [Element] " +
                            "(ElementName, ElementLength, ElementWidth, ElementPower, ElementMaxTemperature)" +
                            "VALUES(@elementName, @elementLength, @elementWidth, @elementPower, " +
                            "@elementMaxTemperature)", sqlConnection);

                        command.Parameters.AddWithValue("elementName", project.elementList[i].Name.ToString());
                        command.Parameters.AddWithValue("elementLength", project.elementList[i].Length.ToString());
                        command.Parameters.AddWithValue("elementWidth", project.elementList[i].Width.ToString());
                        command.Parameters.AddWithValue("elementPower", project.elementList[i].Power.ToString());
                        command.Parameters.AddWithValue("elementMaxTemperature", project.elementList[i].MaxTemperature.ToString());

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка");
            }
            sqlConnection.Close();
                                          
            sqlConnection.Open();

            try
            {
                
                    sqlReader = null;
                    command = new SqlCommand("SELECT ElementId FROM [Element]", sqlConnection);

                    sqlReader = command.ExecuteReader();

                    
                    while (sqlReader.Read())
                    {
                        elementIds.Add(new int());
                        elementIds[elementIds.Count - 1] = Convert.ToInt32(sqlReader["ElementId"]);
                    }
                
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка!");
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Close();
            }
            sqlConnection.Close();

            List<int> list = new List<int>();

            for (int i = elementIds.Count - count; i < elementIds.Count; i++)
                list.Add(elementIds[i]);
            elementIds = list;            
        }        

        public static void SaveOldProjectElement(Project project)
        {
            sqlConnection.Open();
            try
            {
                for (int i = 0; i < project.elementList.Count; i++)
                {
                    if (project.elementList[i].Id != 0)
                    {
                        command = new SqlCommand("UPDATE [ProjectElement] " +
                            "SET [ElementXCoord] = @elementXCoord, [ElementYCoord] = @elementYCoord " +
                            "WHERE [PEProjectId] = @projectId AND [PEElementId] = @elementId", sqlConnection);

                        command.Parameters.AddWithValue("elementXCoord", project.elementList[i].XCoordinate.ToString());
                        command.Parameters.AddWithValue("elementYCoord", project.elementList[i].YCoordinate.ToString());
                        command.Parameters.AddWithValue("projectId", Convert.ToInt32(project.ProjectId));
                        command.Parameters.AddWithValue("elementId", Convert.ToInt32(project.elementList[i].Id));

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка");
            }
            sqlConnection.Close();
        }

        public static void SaveNewProjectElement(Project project)
        {
            int k = 0;
            sqlConnection.Open();
            try
            {
                for (int i = 0; i < project.elementList.Count; i++)
                {
                    if (project.elementList[i].Id == 0)
                    {
                        command = new SqlCommand("INSERT INTO [ProjectElement] (PEProjectId, PEElementId, ElementXCoord, ElementYCoord)" +
                            "VALUES(@projectId, @elementId, @elementXCoord, @elementYCoord)", sqlConnection);

                        command.Parameters.AddWithValue("projectId", Convert.ToInt32(project.ProjectId));
                        command.Parameters.AddWithValue("elementId", Convert.ToInt32(elementIds[k]));
                        command.Parameters.AddWithValue("elementXCoord", project.elementList[i].XCoordinate.ToString());
                        command.Parameters.AddWithValue("elementYCoord", project.elementList[i].YCoordinate.ToString());

                        command.ExecuteNonQuery();
                        k++;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка");
            }
            sqlConnection.Close();
        }

        public static void SaveElements(Project project)
        {
            SaveElementsWithIds(project);
            SaveOldProjectElement(project);
            SaveElementsWithoutIds(project);
            SaveNewProjectElement(project);
        }
        #endregion
    }
}
